using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CVnHR.Business.Services
{
    public class SettingsService : ISettingsService
    {
        private const string CertificatesPath = "Certificates";
        private readonly IDataProtector _dataProtector;
        // TODO: DI/IoC for FileSystem to test this class?

        public SettingsService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("CVnHR.certificate.dataprotector");
        }

        public T GetSettings<T>()
        {
            var fileName = GetFileName<T>();
            if (!File.Exists(fileName))
            {
                var directoryName = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                UpdateSettings(Activator.CreateInstance<T>());
            }
            var item = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(item);
        }

        public void UpdateSettings<T>(T newSettings)
        {
            var json = JsonConvert.SerializeObject(newSettings, Formatting.Indented);
            File.WriteAllText(GetFileName<T>(), json);
        }

        public string GetCertificateName()
        {
            return Directory.GetFiles(CertificatesPath, "*.pfx")
                    .SingleOrDefault()?
                    .Replace(CertificatesPath + "\\", string.Empty).Replace(".pfx", string.Empty);
        }

        public void UploadCertificate(IFormFile certificate, string password)
        {
            if (!certificate.FileName.EndsWith(".pfx"))
            {
                throw new ArgumentException("Expecting a pfx file!");
            }

            var currentCertificate = GetCertificateName();
            File.Delete(Path.Combine(CertificatesPath, $"{currentCertificate}.pfx"));
            File.Delete(Path.Combine(CertificatesPath, $"{currentCertificate}.txt"));

            var newFilePath = Path.Combine(CertificatesPath, certificate.FileName);
            using (var ms = new MemoryStream())
            {
                certificate.CopyTo(ms);
                File.WriteAllBytes(newFilePath, ms.ToArray());
            }

            var protectedPassword = _dataProtector.Protect(password);
            var passwordPath = newFilePath.Replace(".pfx", ".txt");
            File.WriteAllText(passwordPath, protectedPassword);
        }

        public X509Certificate2 GetCertificate()
        {
            var encryptedPassword = File.ReadAllText($"Certificates/{GetCertificateName()}.txt");
            var password = _dataProtector.Unprotect(encryptedPassword);
            var certificate = new X509Certificate2($"Certificates/{GetCertificateName()}.pfx", password);

            return certificate;
        }

        private string GetFileName<T>()
        {
            var typeName = typeof(T).Name;
            typeName = $"{char.ToLowerInvariant(typeName[0])}{typeName.Substring(1)}";
            return $"Config/{typeName}.json";
        }
    }
}

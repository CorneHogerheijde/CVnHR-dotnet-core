using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CVnHR.Business.Services
{
    public interface ISettingsService
    {
        T GetSettings<T>();

        void UpdateSettings<T>(T newSettings);

        string GetCertificateName();

        void UploadCertificate(IFormFile certificate, string password);

        X509Certificate2 GetCertificate();
    }
}

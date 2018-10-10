using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CVnHR.manage.Models;
using Microsoft.Extensions.Configuration;

namespace CVnHR.manage.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private const string CertificatesPath = "Certificates";
        private readonly KvkSettings _kvkSettings;
        private readonly IConfiguration _configuration;

        public SettingsController(IOptions<KvkSettings> kvkSettings, IConfiguration configuration)
        {
            _kvkSettings = kvkSettings.Value;
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public CVnHRSettings GetSettings()
        {
            return new CVnHRSettings() {
                Certificates = Directory.GetFiles(CertificatesPath, "*.pfx")
                    .Select(p => p.Replace(CertificatesPath + "\\", string.Empty).Replace(".pfx", string.Empty)),
                ApiKey = _kvkSettings.ApiKey
            };
        }

        [HttpPut("[action]")]
        public string UpdateApiKey(string newApiKey)
        {
            // TODO
            //_configuration.


            return newApiKey;
        }
    }

    public class CVnHRSettings // TODO move
    {
        public IEnumerable<string> Certificates { get; set; }
        public string ApiKey { get; set; }
    }
}
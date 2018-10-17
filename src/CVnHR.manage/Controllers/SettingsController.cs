using CVnHR.Business.Services;
using CVnHR.manage.Models;
using CVnHR.manage.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace CVnHR.manage.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private const string CertificatesPath = "Certificates";
        private readonly KvkSettings _kvkSettings;
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _kvkSettings = _settingsService.GetSettings<KvkSettings>();
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
        public string UpdateApiKey([FromBody]string newApiKey)
        {
            _kvkSettings.ApiKey = newApiKey;
            _settingsService.UpdateSettings(_kvkSettings);

            return newApiKey;
        }
    }
}
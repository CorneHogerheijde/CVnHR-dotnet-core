using CVnHR.Business.Kvk.Api.Entities;
using CVnHR.Business.Services;
using CVnHR.manage.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace CVnHR.manage.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private const string CertificatesPath = "Certificates";
        private KvkApiSettings _kvkApiSettings;
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _kvkApiSettings = _settingsService.GetSettings<KvkApiSettings>();
        }

        [HttpGet("[action]")]
        public CVnHRSettings GetSettings()
        {
            return new CVnHRSettings() {
                Certificates = Directory.GetFiles(CertificatesPath, "*.pfx")
                    .Select(p => p.Replace(CertificatesPath + "\\", string.Empty).Replace(".pfx", string.Empty)),
                KvkApiSettings = _kvkApiSettings
            };
        }

        [HttpPut("[action]")]
        public KvkApiSettings UpdateKvkApiSettings([FromBody]KvkApiSettings kvkApiSettings)
        {
            _kvkApiSettings = kvkApiSettings;
            _settingsService.UpdateSettings(_kvkApiSettings);

            return _kvkApiSettings;
        }
    }
}
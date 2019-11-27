using CVnHR.Business.HrDataserviceHelpers;
using CVnHR.Business.Kvk;
using CVnHR.Business.Kvk.Api.Entities;
using CVnHR.Business.Services;
using CVnHR.manage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVnHR.manage.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private KvkApiSettings _kvkApiSettings;
        private readonly ISettingsService _settingsService;
        private HrDataserviceSettings _hrDataserviceSettings;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _kvkApiSettings = _settingsService.GetSettings<KvkApiSettings>();
            _hrDataserviceSettings = _settingsService.GetSettings<HrDataserviceSettings>();
        }

        [HttpGet("[action]")]
        public CVnHRSettings GetSettings()
        {
            return new CVnHRSettings() {
                Certificate = _settingsService.GetCertificateName(),
                KvkApiSettings = _kvkApiSettings,
                HRDataServiceSettings = _hrDataserviceSettings
            };
        }

        [HttpPut("[action]")]
        public KvkApiSettings UpdateKvkApiSettings([FromBody]KvkApiSettings kvkApiSettings)
        {
            _settingsService.UpdateSettings(kvkApiSettings);
            _kvkApiSettings = kvkApiSettings;

            return _kvkApiSettings;
        }

        [HttpPut("[action]")]
        public HrDataserviceSettings UpdateHRDataServiceSettings([FromBody]HrDataserviceSettings hrDataserviceSettings)
        {
            _settingsService.UpdateSettings(hrDataserviceSettings);
            _hrDataserviceSettings = hrDataserviceSettings;

            return _hrDataserviceSettings;
        }

        [HttpPost("[action]")]
        public CVnHRSettings UpdateCertificate([FromForm]IFormFile certificate, [FromForm]string password)
        {
            _settingsService.UploadCertificate(certificate, password);

            return GetSettings();
        }
    }
}

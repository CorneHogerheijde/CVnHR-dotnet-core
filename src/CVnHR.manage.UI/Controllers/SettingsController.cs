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

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _kvkApiSettings = _settingsService.GetSettings<KvkApiSettings>();
        }

        [HttpGet("[action]")]
        public CVnHRSettings GetSettings()
        {
            return new CVnHRSettings() {
                Certificate = _settingsService.GetCertificateName(),
                KvkApiSettings = _kvkApiSettings
            };
        }

        [HttpPut("[action]")]
        public KvkApiSettings UpdateKvkApiSettings([FromBody]KvkApiSettings kvkApiSettings)
        {
            _settingsService.UpdateSettings(_kvkApiSettings);
            _kvkApiSettings = kvkApiSettings;

            return _kvkApiSettings;
        }

        [HttpPost("[action]")]
        public CVnHRSettings UpdateCertificate([FromForm]IFormFile certificate, [FromForm]string password)
        {
            _settingsService.UploadCertificate(certificate, password);

            return new CVnHRSettings()
            {
                Certificate = _settingsService.GetCertificateName(),
                KvkApiSettings = _kvkApiSettings
            };
        }
    }
}

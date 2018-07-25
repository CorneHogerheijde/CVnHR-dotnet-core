using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CVnHR.manage.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private const string CertificatesPath = "Certificates";

        [HttpGet("[action]")]
        public IEnumerable<string> GetCertificates()
        {
            return Directory.GetFiles(CertificatesPath, "*.pfx")
                    .Select(p=> p.Replace(CertificatesPath+"\\", string.Empty).Replace(".pfx", string.Empty));
        }
    }
}
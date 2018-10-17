using System.Collections.Generic;

namespace CVnHR.manage.Models
{
    public class CVnHRSettings
    {
        public IEnumerable<string> Certificates { get; set; }
        public string ApiKey { get; set; }
    }
}

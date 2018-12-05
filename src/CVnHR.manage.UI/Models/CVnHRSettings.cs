using CVnHR.Business.Kvk.Api.Entities;
using System.Collections.Generic;

namespace CVnHR.manage.Models
{
    public class CVnHRSettings
    {
        public IEnumerable<string> Certificates { get; set; }
        public KvkApiSettings KvkApiSettings { get; set; }
    }
}

using CVnHR.Business.Kvk;
using CVnHR.Business.Kvk.Api.Entities;
using System.Collections.Generic;

namespace CVnHR.manage.Models
{
    public class CVnHRSettings
    {
        public string Certificate { get; set; }
        public KvkApiSettings KvkApiSettings { get; set; }
        public HrDataserviceSettings HRDataServiceSettings { get; set; }
    }
}

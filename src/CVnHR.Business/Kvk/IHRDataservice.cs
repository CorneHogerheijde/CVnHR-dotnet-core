using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVnHR.Business.Kvk
{
    public interface IHrDataservice
    {
        Task<object> GetInschrijvingFromKvK(string kvkNummer);
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CVnHR.Business.HrDataservice
{
    public interface IHrDataServiceHttpClient
    {
        HttpClientHandler GetHttpClientHandler();

        X509Certificate2 GetCertificate();

        void InstallCertificate(X509Certificate2 certificate);

        HttpClient GetHttpClient();
    }
}

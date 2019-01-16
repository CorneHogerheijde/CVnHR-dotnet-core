using CVnHR.Business.Logging;
using CVnHR.Business.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CVnHR.Business.HrDataservice
{
    public class HrDataServiceHttpClient : IHrDataServiceHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISettingsService _settingsService;

        public HrDataServiceHttpClient(IHttpClientFactory httpClientFactory,
            ISettingsService settingsService)
        {
            _httpClientFactory = httpClientFactory;
            _settingsService = settingsService;
        }

        public HttpClientHandler GetHttpClientHandler()
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (a, b, c, d) => { return true; }
            };

            return handler;
        }

        public X509Certificate2 GetCertificate()
        {
            return _settingsService.GetCertificate();
        }

        public void InstallCertificate(X509Certificate2 certificate)
        {
            // Ensure the certificate exists in the store
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadWrite);

                if (!store.Certificates.Contains(certificate))
                {
                    store.Add(certificate);
                }
            }
        }

        public HttpClient GetHttpClient()
        {
            var client = _httpClientFactory.CreateClient("hrDataservice");

            client.DefaultRequestHeaders.Connection.Add("Keep-Alive");
            client.DefaultRequestHeaders.ExpectContinue = true;
            client.DefaultRequestHeaders.Add("SOAPAction", "\"http://es.kvk.nl/ophalenInschrijving\"");

            return client;
        }
    }
}

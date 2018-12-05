using CVnHR.Business.Logging;
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

        public HrDataServiceHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
            // TODO
            var password = File.ReadAllText("Certificates/digilevering.drenthe.nl.txt");
            var certificate = new X509Certificate2("Certificates/digilevering.drenthe.nl.pfx", password);

            return certificate;
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

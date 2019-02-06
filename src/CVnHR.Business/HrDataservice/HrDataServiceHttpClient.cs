using CVnHR.Business.Services;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

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

using System.Net.Http;

namespace CVnHR.Business.HrDataserviceHelpers
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

        public HttpClient GetHttpClient(string action)
        {
            var client = _httpClientFactory.CreateClient("hrDataservice");

            client.DefaultRequestHeaders.Connection.Add("Keep-Alive");
            client.DefaultRequestHeaders.ExpectContinue = true;
            client.DefaultRequestHeaders.Add("SOAPAction", $"\"http://es.kvk.nl/{action}\"");

            return client;
        }
    }
}

using System.Net.Http;

namespace CVnHR.Business.HrDataserviceHelpers
{
    public interface IHrDataServiceHttpClient
    {
        HttpClientHandler GetHttpClientHandler();
        HttpClient GetHttpClient(string action);
    }
}

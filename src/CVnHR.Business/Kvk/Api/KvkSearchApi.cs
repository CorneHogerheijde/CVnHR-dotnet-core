using CVnHR.Business.Kvk.Api.Entities;
using CVnHR.Business.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CVnHR.Business.Kvk.Api
{
    public class KvkSearchApi : IKvkSearchApi
    {
        private readonly ISettingsService _settingsService;
        private readonly IHttpClientFactory _httpClientFactory;

        public KvkSearchApi(ISettingsService settingsService, IHttpClientFactory httpClientFactory)
        {
            _settingsService = settingsService;
            _httpClientFactory = httpClientFactory;
        }

        public string GetFormattedQueryString()
        {
            var apiSettings = _settingsService.GetSettings<KvkApiSettings>();
            var baseUrl = apiSettings.BaseUrl.TrimEnd('/');
            var searchUrl = apiSettings.SearchUrl.TrimStart('/');
            var apiKey = apiSettings.ApiKey;

            return $"{baseUrl}/{searchUrl}?apiKey={HttpUtility.UrlEncode(apiKey)}";
        }

        public async Task<KvkSearchApiResult> Search(KvkSearchApiParameters parameters)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var search = new List<string>
            {
                GetFormattedQueryString()
            };
            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                search.Add($"q={parameters.Q}");
            }
            if (parameters.StartPage > 0)
            {
                search.Add($"startpage={parameters.StartPage}");
            }
            // TODO (?): add other parameters

            var url = string.Join("&", search);
            var result = await httpClient.GetAsync(url);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();

            var apiResult = JsonConvert.DeserializeObject<KvkSearchApiResultWrapper>(content);
            return apiResult.Data;
        }
    }
}


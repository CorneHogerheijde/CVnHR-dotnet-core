namespace CVnHR.Business.Kvk.Api.Entities
{
    public class KvkApiSettings
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; } = "https://api.kvk.nl";
        public string SearchUrl { get; set; } = "/api/v2/search/companies";
        public string ProfileUrl { get; set; } = "/api/v2/profiles/companies";
    }
}

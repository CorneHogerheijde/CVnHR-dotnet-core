using CVnHR.Business.Kvk.Api;
using CVnHR.Business.Kvk.Api.Entities;
using CVnHR.Business.Services;
using MaxKagamine.Moq.HttpClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CVnHR.Business.Tests.Kvk.Api
{
    [TestClass]
    public class KvkSearchApiTests
    {
        private readonly Mock<ISettingsService> _settingsServiceMock;
        public KvkSearchApiTests()
        {
            var settings = new KvkApiSettings()
            {
                ApiKey = "unittest-apikey",
                BaseUrl = "http://unittest.ut",
                ProfileUrl = "/unittestsearch"
            };
            _settingsServiceMock = new Mock<ISettingsService>();
            _settingsServiceMock.Setup(s => s.GetSettings<KvkApiSettings>())
                .Returns(settings);
        }

        [TestMethod, Description("Should correctly format the url and querystring to do the search")]
        public void GetFormattedQueryStringTest()
        {
            // Arrange
            var searchApi = new KvkSearchApi(_settingsServiceMock.Object, null);

            // Act
            var result = searchApi.GetFormattedQueryString();

            // Assert
            Assert.AreEqual("http://unittest.ut/api/v2/search/companies?apiKey=unittest-apikey", result, 
                "Should correctly format the querystring");
        }

        [TestMethod, Description("Should correctly call search api")]
        public async Task SearchTest()
        {
            // Arrange
            var expectedData = new KvkSearchApiResult()
            {
                ItemsPerPage = 10,
                StartPage = 1,
                Items = new List<ApiItem>() { new ApiItem { KvkNumber = "123456" } },
                TotalItems = 1
            };
            var jsonResponse = JsonConvert.SerializeObject(new KvkSearchApiResultWrapper()
            {
                ApiVersion = "1.0",
                Data = expectedData
            });
            var handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest()
                .ReturnsResponse(jsonResponse, "application/json");

            var searchApi = new KvkSearchApi(_settingsServiceMock.Object, handler.CreateClientFactory());

            // Act
            var result = await searchApi.Search(new KvkSearchApiParameters() { Q = "test" });

            // Assert
            Assert.AreEqual(expectedData.ToString(), result.ToString(), "Expected data does not match returned data.");
            handler.VerifyRequest("http://unittest.ut/api/v2/search/companies?apiKey=unittest-apikey&q=test", Times.Once());
        }

        [TestMethod, Description("Should correctly call search api")]
        public async Task SearchTest_RequestNextPage()
        {
            // Arrange
            var expectedData = new KvkSearchApiResult();
            var jsonResponse = JsonConvert.SerializeObject(new KvkSearchApiResultWrapper()
            {
                ApiVersion = "1.0",
                Data = expectedData
            });
            var handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest()
                .ReturnsResponse(jsonResponse, "application/json");

            var searchApi = new KvkSearchApi(_settingsServiceMock.Object, handler.CreateClientFactory());

            // Act
            var result = await searchApi.Search(new KvkSearchApiParameters() { StartPage = 1 });

            // Assert
            handler.VerifyRequest("http://unittest.ut/api/v2/search/companies?apiKey=unittest-apikey&startpage=1", Times.Once());
        }

        [TestMethod, Description("Should correctly throw an HttpRequestException when result from kvk fails")]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task SearchTest_HttpRequestException()
        {
            // Arrange
            var expectedData = new KvkSearchApiResult()
            {
                ItemsPerPage = 10,
                StartPage = 1,
                Items = new List<ApiItem>() { new ApiItem { KvkNumber = "123456" } },
                TotalItems = 1
            };
            var jsonResponse = JsonConvert.SerializeObject(new KvkSearchApiResultWrapper()
            {
                ApiVersion = "1.0",
                Data = expectedData
            });
            var handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.InternalServerError);

            var searchApi = new KvkSearchApi(_settingsServiceMock.Object, handler.CreateClientFactory());

            // Act
            var result = await searchApi.Search(new KvkSearchApiParameters() { Q = "test" });
        }
    }
}

using CVnHR.Business.Kvk.Api;
using CVnHR.Business.Kvk.Api.Entities;
using CVnHR.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVnHR.Business.Tests.Kvk.Api
{
    [TestClass]
    public class KvkSearchApiTests
    {
        [TestMethod, Description("Should correctly format the url and querystring to do the search")]
        public void GetFormattedQueryStringTest()
        {
            // Arrange
            var settings = new KvkApiSettings()
            {
                ApiKey = "unittest-apikey",
                BaseUrl = "http://unittest.ut",
                ProfileUrl = "/unittestsearch"
            };
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.GetSettings<KvkApiSettings>())
                .Returns(settings);
            var searchApi = new KvkSearchApi(settingsService.Object);

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
            var settings = new KvkApiSettings()
            {
                ApiKey = "unittest-apikey",
                BaseUrl = "http://unittest.ut",
                ProfileUrl = "/unittestsearch"
            };
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(s => s.GetSettings<KvkApiSettings>())
                .Returns(settings);
            var searchApi = new KvkSearchApi(settingsService.Object);

            // Act
            var result = await searchApi.Search(new KvkSearchApiParameters() { Q = "test" });

            // TODO: fix this, make httpClient configurable etc.

            // Assert
            Assert.AreEqual(result.)
        }
    }
}

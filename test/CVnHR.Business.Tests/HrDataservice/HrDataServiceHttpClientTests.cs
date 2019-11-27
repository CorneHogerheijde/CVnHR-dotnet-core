using CVnHR.Business.HrDataserviceHelpers;
using CVnHR.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net.Http;

namespace CVnHR.Business.Tests
{
    [TestClass]
    public class HrDataServiceHttpClientTests
    {
        [TestMethod, Description("Should return a correct HttpClientHandler")]
        public void GetHttpClientHandlerTest()
        {
            // Arrange
            var client = new HrDataServiceHttpClient(null);

            // Act
            var handler = client.GetHttpClientHandler();

            // Assert
            Assert.IsNotNull(handler, "Expected a handler");
            Assert.AreEqual(ClientCertificateOption.Manual, handler.ClientCertificateOptions, "ClientCertificateOption should be set to Manual");
            Assert.IsNotNull(handler.ServerCertificateCustomValidationCallback, "ServerCertiificateCustomValidationCallback should be set");
        }

        [TestMethod, Description("Should create a httpClient for hrDataservice and set DefaultRequestHeaders")]
        public void GetHttpClientTest()
        {
            // Arrange
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());
            var client = new HrDataServiceHttpClient(httpClientFactory.Object);
            var action = "http://es.kvk.nl/ophalenInschrijving";

            // Act
            var httpClient = client.GetHttpClient(action);

            // Assert
            httpClientFactory.Verify(f => f.CreateClient("hrDataservice"), Times.Once, "Should create a hrDataservice client");

            Assert.AreEqual(1, httpClient.DefaultRequestHeaders.Connection.Count, 
                "Should have one connection request header added");
            Assert.AreEqual("Keep-Alive", httpClient.DefaultRequestHeaders.Connection.First(),
                "Should have a Keep-Alive connection request header added");
            Assert.IsTrue(httpClient.DefaultRequestHeaders.ExpectContinue == true, "Expect continue should be set to true");
            Assert.IsTrue(httpClient.DefaultRequestHeaders.Contains("SOAPAction"), "Should have a SOAPAction header");
            Assert.AreEqual($"\"{action}\"", httpClient.DefaultRequestHeaders.GetValues("SOAPAction").First(),
                "Should have correct SOAPAction header set (including surrounding quotes)");
        }
    }
}

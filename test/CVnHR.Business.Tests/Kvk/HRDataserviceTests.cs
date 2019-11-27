using MaxKagamine.Moq.HttpClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CVnHR.Business.Kvk;
using CVnHR.Business.HrDataserviceHelpers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CVnHR.Business.Services;

namespace CVnHR.Business.Tests.Kvk
{
    [TestClass]
    public class HRDataserviceTests
    {
        // TODO: more tests and refactors.

        [TestMethod, Description("Should correctly format the envelope to be send to the HR-Dataservice")]
        public async Task GetInschrijvingFromKvKStringArgument()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest()
                .ReturnsResponse("test", "application/xml");

            var hrDataServiceHttpClient = new Mock<IHrDataServiceHttpClient>();

            var req = new CertificateRequest("cn=unittest", RSA.Create(), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddYears(5));

            var settingsServiceMock = new Mock<ISettingsService>();
            settingsServiceMock.Setup(c => c.GetCertificate())
                .Returns(cert);
            settingsServiceMock.Setup(c => c.GetSettings<HrDataserviceSettings>())
                .Returns(new HrDataserviceSettings { KlantReferentie = "abcd" });

            hrDataServiceHttpClient.Setup(c => c.GetHttpClient(It.IsAny<string>()))
                .Returns(handler.CreateClient());

            var expectedMessage = "message";

            var hRDataserviceMessageParserMock = new Mock<IHRDataserviceMessageParser>();
            hRDataserviceMessageParserMock.Setup(h => h.SerializeOphalenInschrijvingRequest(It.IsAny<ophalenInschrijvingRequest>()))
                .Returns(expectedMessage);

            var hrDataservice = new HrDataservice(
                hrDataServiceHttpClient.Object, 
                hRDataserviceMessageParserMock.Object, 
                settingsServiceMock.Object
            );

            // Act
            var result = await hrDataservice.GetInschrijvingFromKvK("21345");

            // Assert
            Assert.AreEqual("test", result, "HrDataservice result should be unmodified!");
            handler.VerifyRequest("https://webservices.kvk.nl/postbus1", Times.Once());
            // TODO: verify call, url, message etc.
            handler.VerifyRequest(async (request) =>
            {
                var content = await request.Content.ReadAsStringAsync();
                StringAssert.Contains(content, expectedMessage, "Should contain correct message!");
                //Assert.IsTrue(content.Contains(expectedMessage));
                return true;
            });
        }
    }
}

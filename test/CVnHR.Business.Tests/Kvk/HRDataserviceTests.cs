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
using CVnHR.Business.HrDataservice;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CVnHR.Business.Tests.Kvk
{
    [TestClass]
    public class HRDataserviceTests
    {
        [TestMethod, Description("Should correctly format the envelope to be send to the HR-Dataservice")]
        public async Task GetInschrijvingFromKvK()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest()
                .ReturnsResponse("test", "application/xml");

            var hrDataServiceHttpClient = new Mock<IHrDataServiceHttpClient>();

            var req = new CertificateRequest("cn=unittest", RSA.Create(), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddYears(5));

            hrDataServiceHttpClient.Setup(c => c.GetCertificate())
                .Returns(cert);

            hrDataServiceHttpClient.Setup(c => c.GetHttpClient())
                .Returns(handler.CreateClient());

            var hrDataservice = new Business.Kvk.HrDataservice(It.IsAny<string>(), hrDataServiceHttpClient.Object);

            // Act
            var result = await hrDataservice.GetInschrijvingFromKvK("12345678");

            // Assert
            Assert.AreEqual("test", result, "HrDataservice result should be unmodified!");
            // TODO: verify call, url etc.
        }
    }
}

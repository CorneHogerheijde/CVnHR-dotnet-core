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
                .ReturnsResponse(HttpStatusCode.OK);

            var hrDataServiceHttpClient = new Mock<IHrDataServiceHttpClient>();

            var ecdsa = ECDsa.Create(); // generate asymmetric key pair
            var req = new CertificateRequest("cn=unittest", ecdsa, HashAlgorithmName.SHA256);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

            // TODO => fix certificate not working!

            hrDataServiceHttpClient.Setup(c => c.GetCertificate())
                .Returns(cert);

            var hrDataservice = new Business.Kvk.HrDataservice(It.IsAny<string>(), hrDataServiceHttpClient.Object);

            // Act
            var result = await hrDataservice.GetInschrijvingFromKvK("12345678");

            // Assert
            Assert.AreEqual("", result);
        }
    }
}

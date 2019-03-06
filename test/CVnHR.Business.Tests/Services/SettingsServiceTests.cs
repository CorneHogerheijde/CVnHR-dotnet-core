using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVnHR.Business.Tests.Services
{
    [TestClass]
    public class SettingsServiceTests
    {
        [TestMethod, Description("Should return the certificate obtained from the settings service")]
        public void GetCertificateTest()
        {
            // Arrange
            //var settingsService = new Mock<ISettingsService>();
            //settingsService.Setup(s => s.GetCertificate());
            //var client = new HrDataServiceHttpClient(null, settingsService.Object);

            //// Act
            //var certificate = client.GetCertificate();

            //// Assert
            //settingsService.Verify(s => s.GetCertificate(), Times.Once, "Expected to receive the certificate from the settingsService");
        }
    }
}

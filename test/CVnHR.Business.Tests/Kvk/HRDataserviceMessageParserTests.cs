using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVnHR.Business.Tests.Kvk
{
    [TestClass]
    public class HRDataserviceMessageParserTests
    {
        [TestMethod, Description("Should correctly parse a message to a MaatschappelijkeActiviteitResponseType")]
        public async Task Parse()
        {
            Assert.IsTrue(false, "TODO");
        }

        [TestMethod, Description("Should correctly serialize a ophalenInschrijvingRequest message")]
        public async Task SerializeOphalenInschrijvingRequest()
        {
            // Arrange
            var reference = "ACC_I_002";
            var kvkNumber = "8541214521";
            var expectedMessage = $@"<ophalenInschrijvingRequest xmlns=""http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02""><klantreferentie>{reference}</klantreferentie><kvkNummer>{kvkNumber}</kvkNummer></ophalenInschrijvingRequest>";

            // Act

            //Assert

            Assert.IsTrue(false, "TODO");

        }
    }
}

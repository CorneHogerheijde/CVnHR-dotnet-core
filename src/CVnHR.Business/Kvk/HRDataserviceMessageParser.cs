using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CVnHR.Business.Kvk
{
    public class HRDataserviceMessageParser : IHRDataserviceMessageParser
    {
        public MaatschappelijkeActiviteitResponseType Parse(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new SerializationException("Cannot parse empty message...");

            var ns = "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02";
            var type = typeof(MaatschappelijkeActiviteitResponseType);
            var serializer = new XmlSerializer(type, ns);
                
            var xDoc = XDocument.Parse(message.Replace(typeof(ophalenInschrijvingResponse).Name, type.Name));
            var nodeName = XName.Get(type.Name, ns);
            var ophalenInschrijvingResponseNode = xDoc.Descendants(nodeName);
            var ophalenInschrijvingResponseXml = ophalenInschrijvingResponseNode.FirstOrDefault();
            using (var reader = ophalenInschrijvingResponseXml.CreateReader())
            {
                var result = serializer.Deserialize(reader);
                return (MaatschappelijkeActiviteitResponseType)result;
            }
        }

        public string SerializeOphalenInschrijvingRequest(ophalenInschrijvingRequest request)
        {
            if (request == null)
                return string.Empty;

            // parse the request to this xml
            // Althoug it would be better to use datacontractserializer, this won't fly one way or another...
            var elementName = request.ophalenInschrijvingRequest1.ItemElementName;
            return $@"<ophalenInschrijvingRequest xmlns=""http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02"">
                <klantreferentie>{request.ophalenInschrijvingRequest1.klantreferentie}</klantreferentie>
                <{elementName}>{request.ophalenInschrijvingRequest1.Item}</{elementName}>
             </ophalenInschrijvingRequest>";

            //var ophalenInschrijvingRequest1 = request.ophalenInschrijvingRequest1;
            //var ophalenInschrijvingRequest1Type = ophalenInschrijvingRequest1.GetType();
            //var requestType = request.GetType().Name;
            //var ns = new XmlSerializerNamespaces();
            //ns.Add("", "");

            //var serializer = new XmlSerializer(ophalenInschrijvingRequest1Type);
            //var sb = new StringBuilder();
            //var settings = new XmlWriterSettings
            //{
            //    Indent = false,
            //    OmitXmlDeclaration = true,
            //    WriteEndDocumentOnClose = true,
            //    NamespaceHandling = NamespaceHandling.OmitDuplicates,

            //};
            //using (var w = XmlWriter.Create(sb, settings))
            //{
            //    serializer.Serialize(w, ophalenInschrijvingRequest1, ns);
            //}
            //var tempResult = sb.ToString()
            //    .Replace(@" xmlns=""http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02""", string.Empty)
            //    .Replace(ophalenInschrijvingRequest1Type.Name, requestType)
            //    .Replace($@"<{requestType}>", $@"<{requestType} xmlns=""http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02"">");


            //return tempResult;
        }
    }
}

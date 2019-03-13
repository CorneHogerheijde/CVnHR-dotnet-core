using System;
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

            //var ns = "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02";
            //var type = typeof(ophalenInschrijvingRequest);
            //var serializer = new XmlSerializer(type, ns);

            //var xDoc = XDocument.Parse(message.Replace(typeof(ophalenInschrijvingRequest).Name, type.Name));
            //var nodeName = XName.Get(type.Name, ns);
            //var ophalenInschrijvingResponseNode = xDoc.Descendants(nodeName);
            //var ophalenInschrijvingResponseXml = ophalenInschrijvingResponseNode.FirstOrDefault();
            //using (var reader = ophalenInschrijvingResponseXml.CreateReader())
            //{
            //    var result = serializer.Serialize(reader);
            //    return (MaatschappelijkeActiviteitResponseType)result;

            //}

            // TODO: parse the request to this xml
            return $@"<ophalenInschrijvingRequest xmlns=""http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02"">
                <klantreferentie>{request.ophalenInschrijvingRequest1.klantreferentie}</klantreferentie>
                <kvkNummer>{request.ophalenInschrijvingRequest1.Item}</kvkNummer>
             </ophalenInschrijvingRequest>"
             .Replace(Environment.NewLine, string.Empty)
             .Replace("  ", string.Empty)
             .Replace("  ", string.Empty)
             .Replace("  ", string.Empty)
             .Replace(" <", "<");


            var serializer = new XmlSerializer(request.GetType());
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = false,
                OmitXmlDeclaration = true,
                WriteEndDocumentOnClose = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
            };
            using (var w = XmlWriter.Create(sb, settings))
            {
                serializer.Serialize(w, request);
            }
            return sb.ToString();
        }
    }
}

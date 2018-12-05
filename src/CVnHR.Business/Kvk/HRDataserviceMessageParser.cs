using System;
using System.Linq;
using System.Runtime.Serialization;
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
    }
}

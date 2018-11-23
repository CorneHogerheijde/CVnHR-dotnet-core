using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
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
                
            var nodeNameM = "MaatschappelijkeActiviteitResponseType";
            var xDoc = XDocument.Parse(message.Replace(typeof(ophalenInschrijvingResponse).Name, nodeNameM));
            var nodeName = XName.Get(nodeNameM, "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02");
            var ophalenInschrijvingResponseNode = xDoc.Descendants(nodeName);
            var ophalenInschrijvingResponseXml = ophalenInschrijvingResponseNode.FirstOrDefault();
            using (var reader = ophalenInschrijvingResponseXml.CreateReader())
            {

                if (serializer.CanDeserialize(reader))
                {
                    var result = serializer.Deserialize(reader);
                    return (MaatschappelijkeActiviteitResponseType)result;
                }
                throw new Exception("Cannot deserialize the message.");
            }
        }
    }
}

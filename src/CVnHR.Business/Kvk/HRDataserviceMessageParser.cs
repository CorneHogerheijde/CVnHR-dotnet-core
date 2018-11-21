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
        public ophalenInschrijvingResponse Parse(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new SerializationException("Cannot parse empty message...");

            //            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
            //            {
            //                var msg = Message.CreateMessage(XmlReader.Create(stream), int.MaxValue, MessageVersion.Soap11);

            //                if (!msg.IsFault)
            //                {
            //                    var type = typeof(ophalenInschrijvingResponse);
            //                    var knownTypes = type.Assembly.GetTypes()
            //                        .Where(t => t.Namespace == type.Namespace)
            //                        .Except(new[] { type });
            //                    var serializer = new DataContractSerializer(type,
            //                        typeof(ophalenInschrijvingResponse).Name,
            //                        "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02",
            //                        knownTypes);



            //                    var result = msg.GetBody<ophalenInschrijvingResponse>(serializer);
            //                    return result;
            ////                    return new ophalenInschrijvingResponse { ophalenInschrijvingResponse1 = result };
            //                }

            //                throw new Exception("TODO: msg is fault!");
            //                //var importer = new SoapReflectionImporter();
            //                //var mapping = importer.ImportTypeMapping(typeof(InschrijvingResponseType));
            //                //var serializer = new XmlSerializer(mapping);
            //                //var response = serializer.Deserialize(msg.GetReaderAtBodyContents()) as InschrijvingResponseType;
            //                //return new ophalenInschrijvingResponse
            //                //{
            //                //    ophalenInschrijvingResponse1 = response
            //                //};
            //                //var serializer = new DataContractSerializer(typeof(InschrijvingResponseType),
            //                //    typeof(InschrijvingResponseType).Name,
            //                //    "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02");

            //                //using (var reader = msg.GetReaderAtBodyContents())
            //                //{
            //                //    var response = (InschrijvingResponseType)serializer.ReadObject(reader, false);
            //                //    return new ophalenInschrijvingResponse
            //                //    {
            //                //        ophalenInschrijvingResponse1 = response
            //                //    };
            //                //}
            //            }


            /*var serializer = new DataContractSerializer(typeof(ophalenInschrijvingResponse));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
            {
                return (ophalenInschrijvingResponse)serializer.ReadObject(stream);
            }*/
            //var type = typeof(InschrijvingResponseType);
            //var knownTypes = type.Assembly.GetTypes()
            //    .Where(t => t.IsClass && !t.Name.Contains("ResponseTypeMeldingen") && t.Namespace == type.Namespace)
            //    .Where(t=> new[] { "VestigingResponseTypeProduct", "ResponseTypeMeldingen", "MaatschappelijkeActiviteitResponseTypeProduct" }.All(n=> !n.Contains(t.Name)))
            //    .Except(new[] { type });
            //var xDoc = XDocument.Parse(message);
            //var nodeName = XName.Get(typeof(ophalenInschrijvingResponse).Name, "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02");
            //var ophalenInschrijvingResponseNode = xDoc.Descendants(nodeName);
            //var ophalenInschrijvingResponseXml = ophalenInschrijvingResponseNode.FirstOrDefault();
            //var serializer = new DataContractSerializer(typeof(InschrijvingResponseType),
            //    typeof(ophalenInschrijvingResponse).Name,
            //    "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02",
            //    knownTypes);
            //using (var reader = ophalenInschrijvingResponseXml.CreateReader())
            //{
            //    var result = (InschrijvingResponseType)serializer.ReadObject(reader);
            //    return new ophalenInschrijvingResponse { ophalenInschrijvingResponse1 = result };
            //}

            //var serializer = new XmlSerializer(typeof(ophalenInschrijvingResponse), 
            //    null,
            //    knownTypes.ToArray(),
            //    new XmlRootAttribute(typeof(ophalenInschrijvingResponse).Name),
            //    "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02");
            //using (var reader = ophalenInschrijvingResponseXml.CreateReader())
            //{
            //    var result = (ophalenInschrijvingResponse)serializer.Deserialize(reader);
            //    return result;
            //}

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
            {
                var msg = Message.CreateMessage(XmlReader.Create(stream), int.MaxValue, MessageVersion.Soap11);

                //var type = typeof(InschrijvingResponseType);
                //var knownTypes = type.Assembly.GetTypes()
                //    .Where(t => t.IsClass && !t.Name.Contains("ResponseTypeMeldingen") && t.Namespace == type.Namespace)
                //    .Where(t => new[] { "VestigingResponseTypeProduct", "ResponseTypeMeldingen", "MaatschappelijkeActiviteitResponseTypeProduct" }.All(n => !n.Contains(t.Name)))
                //    .Except(new[] { type });

                //var importer = new XmlReflectionImporter();
                //var mapping = importer.ImportTypeMapping(typeof(ophalenInschrijvingResponse));
                //var serializer = XmlSerializer.FromMappings(new[] { mapping }).First();

                //var serializer = new XmlSerializer(typeof(ophalenInschrijvingResponse),
                //    null,
                //    knownTypes.ToArray(),
                //    new XmlRootAttribute(typeof(ophalenInschrijvingResponse).Name),
                //    "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02");


                // TODO
                var ns = "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02";
                var importer = new SoapReflectionImporter(ns);
                var mapping = importer
                    .ImportTypeMapping(typeof(ophalenInschrijvingResponse));
                var serializer = new XmlSerializer(mapping, ns);
                
                //var reader = msg.GetReaderAtBodyContents();
                var reader = XmlReader.Create(new StringReader(message));
                if (serializer.CanDeserialize(reader))
                {
                    var result = serializer.Deserialize(reader);
                    return (ophalenInschrijvingResponse)result;
                }
                throw new Exception("cannot deserialize!");
            }

        }

        public static T GetBodyWithXmlSerializer<T>(Message msg)
        {
            var ser = new XmlSerializer(typeof(T));
            T o;
            using (var reader = msg.GetReaderAtBodyContents())
            {
                o = (T)ser.Deserialize(reader);
            }
            return o;
        }
    }
}

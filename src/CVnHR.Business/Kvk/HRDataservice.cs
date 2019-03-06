using CVnHR.Business.HrDataserviceHelpers;
using CVnHR.Business.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CVnHR.Business.Kvk
{
    //Example from:
    //https://stackoverflow.com/questions/47104618/how-do-i-call-xml-soap-service-that-requires-signature-from-net-core/48818293#48818293

    public class HrDataservice : IHrDataservice
    {
        private readonly IHrDataServiceHttpClient _httpClientFactory;
        private readonly IHRDataserviceMessageParser _hRDataserviceMessageParser;
        private readonly ISettingsService _settingsService;

        public HrDataservice(IHrDataServiceHttpClient httpClientFactory,
            IHRDataserviceMessageParser hRDataserviceMessageParser,
            ISettingsService settingsService)
        {
            _httpClientFactory = httpClientFactory;
            _hRDataserviceMessageParser = hRDataserviceMessageParser;
            _settingsService = settingsService;
        }

        public async Task<string> GetInschrijvingFromKvK(string kvkNummer)
        {
            var payload = _hRDataserviceMessageParser.SerializeOphalenInschrijvingRequest(new ophalenInschrijvingRequest
            {
                ophalenInschrijvingRequest1 = new InschrijvingRequestType
                {
                    Item = kvkNummer,
                    ItemElementName = ItemChoiceType.kvkNummer,
                    klantreferentie = _settingsService.GetKlantReferentie()
                }
            });
            var action = "ophalenInschrijving";
            return await CallHrDataservice(payload, action);
        }

        private async Task<string> CallHrDataservice(string payload, string action)
        {
            var certificate = _settingsService.GetCertificate();
            var responseMessage = string.Empty;
            var envelope = BuildEnvelope(certificate, payload, action);
            var client = _httpClientFactory.GetHttpClient(action);

            var content = new StringContent(envelope, Encoding.UTF8, "text/xml");
            var response = await client.PostAsync("https://webservices.kvk.nl/postbus1", content);

            if (response.IsSuccessStatusCode)
            {
                responseMessage = await response.Content.ReadAsStringAsync();
            }

            return responseMessage;
        }

        private string BuildEnvelope(X509Certificate2 certificate, string payload, string action)
        {
            string envelope = null;
            // note - lots of bits here specific to my thirdparty
            var cert_id = $"uuid-{Guid.NewGuid()}-2";
            var refTimestamp = $"uuid-{Guid.NewGuid()}-1";
            using (var stream = new MemoryStream())
            {
                var utf8 = new UTF8Encoding(false); // omit BOM
                using (var writer = new XmlTextWriter(stream, utf8))
                {
                    // timestamp
                    var dt = DateTime.UtcNow;
                    var now = dt.ToString("o").Substring(0, 23) + "Z";
                    var plus5 = dt.AddMinutes(5).ToString("o").Substring(0, 23) + "Z";

                    // soap envelope
                    // <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/" xmlns:a="http://www.w3.org/2005/08/addressing" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
                    writer.WriteStartDocument();
                    writer.WriteStartElement("s", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
                    writer.WriteAttributeString("xmlns", "a", null, "http://www.w3.org/2005/08/addressing");
                    writer.WriteAttributeString("xmlns", "u", null, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

                    writer.WriteStartElement("s", "Header", null);

                    /////////////////
                    //  saml guts  //
                    /////////////////

                    //<a:Action s:mustUnderstand="1" u:Id="_2">http://es.kvk.nl/ophalenInschrijving</a:Action>
                    writer.WriteStartElement("a", "Action", null);
                    writer.WriteAttributeString("s", "mustUnderstand", null, "1");
                    writer.WriteAttributeString("u", "Id", null, "_2");
                    writer.WriteString($"http://es.kvk.nl/{action}");
                    writer.WriteEndElement(); //Action

                    //<a:MessageID u:Id="_3">uuid:baae5ec4-cf09-4241-9a28-61128fa9aeef</a:MessageID>
                    writer.WriteStartElement("a", "MessageID", null);
                    writer.WriteAttributeString("u", "Id", null, "_3");
                    writer.WriteString($"uuid:{Guid.NewGuid()}");
                    writer.WriteEndElement(); //MessageID

                    //<a:To s:mustUnderstand="1" u:Id="_4">http://es.kvk.nl/kvk-Dataservice/2015/02</a:To>
                    writer.WriteStartElement("a", "To", null);
                    writer.WriteAttributeString("s", "mustUnderstand", null, "1");
                    writer.WriteAttributeString("u", "Id", null, "_4");
                    writer.WriteString("http://es.kvk.nl/kvk-Dataservice/2015/02");
                    writer.WriteEndElement(); //To

                    //<o:Security s:mustUnderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
                    writer.WriteStartElement("o", "Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                    writer.WriteAttributeString("s", "mustUnderstand", null, "1");

                    //<u:Timestamp u:Id="uuid-be9c1dee-6293-40bc-ab38-050cf4f93b4e-1">
                    writer.WriteStartElement("u", "Timestamp", null);
                    writer.WriteAttributeString("u", "Id", null, refTimestamp);

                    //<u:Created>2018-08-01T19:32:03.198Z</u:Created>
                    writer.WriteElementString("u", "Created", null, now);

                    //<u:Expires>2018-08-01T19:37:03.198Z</u:Expires>
                    writer.WriteElementString("u", "Expires", null, plus5);

                    writer.WriteEndElement(); //Timestamp

                    writer.WriteStartElement("o", "BinarySecurityToken", null);
                    writer.WriteAttributeString("u", "Id", null, cert_id);
                    writer.WriteAttributeString("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
                    writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
                    var rawData = certificate.GetRawCertData();
                    writer.WriteBase64(rawData, 0, rawData.Length);
                    writer.WriteEndElement(); //BinarySecurityToken

                    writer.WriteEndElement(); //Security
                    writer.WriteEndElement(); //Header

                    //<s:Body u:Id="_1" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                    writer.WriteStartElement("s", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
                    writer.WriteAttributeString("u", "Id", null, "_1");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");

                    // write the payload of the message.
                    writer.WriteRaw(payload);

                    writer.WriteEndElement(); // Body

                    writer.WriteEndElement(); //Envelope
                }

                // signing pass
                var signable = Encoding.UTF8.GetString(stream.ToArray());
                var doc = new XmlDocument();
                doc.LoadXml(signable);

                // see https://stackoverflow.com/a/6467877
                var signedXml = new SignedXmlWithId(doc);

                var key = certificate.GetRSAPrivateKey();
                signedXml.SigningKey = key;
                // these values may not be supported by your 3rd party - they may use e.g. SHA256 miniumum
                signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
                signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

                // create keyInfo
                var keyInfo = new KeyInfo();
                var x509data = new KeyInfoX509Data(certificate);
                keyInfo.AddClause(x509data);
                signedXml.KeyInfo = keyInfo;

                // sign all references
                for (var i = 1; i < 5; i++)
                {
                    var reference0 = new Reference
                    {
                        Uri = $"#_{i}",
                        DigestMethod = SignedXml.XmlDsigSHA1Url
                    };
                    reference0.AddTransform(new XmlDsigExcC14NTransform());
                    signedXml.AddReference(reference0);
                }

                // sign the timestamp fragment
                var refTimeStamp = new Reference()
                {
                    Uri = $"#{refTimestamp}",
                    DigestMethod = SignedXml.XmlDsigSHA1Url
                };
                refTimeStamp.AddTransform(new XmlDsigExcC14NTransform());
                signedXml.AddReference(refTimeStamp);

                // get the sig fragment
                signedXml.ComputeSignature();
                var xmlDigitalSignature = signedXml.GetXml();

                // modify the fragment so it points at BinarySecurityToken instead
                XmlNode info = null;
                for (int i = 0; i < xmlDigitalSignature.ChildNodes.Count; i++)
                {
                    var node = xmlDigitalSignature.ChildNodes[i];
                    if (node.Name == "KeyInfo")
                    {
                        info = node;
                        break;
                    }
                }
                info.RemoveAll();

                // get id for tokenreference
                var securityTokenReference = doc.CreateElement("o", "SecurityTokenReference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                var reference = doc.CreateElement("o", "Reference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                reference.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
                // cert id                
                reference.SetAttribute("URI", "#" + cert_id);
                securityTokenReference.AppendChild(reference);
                info.AppendChild(securityTokenReference);

                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("o", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                nsmgr.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
                var security_node = doc.SelectSingleNode("/s:Envelope/s:Header/o:Security", nsmgr);
                security_node.AppendChild(xmlDigitalSignature);

                var sb = new StringBuilder();
                var settings = new XmlWriterSettings
                {
                    Indent = false,
                    OmitXmlDeclaration = true,
                    WriteEndDocumentOnClose = true
                };
                using (var writer = XmlWriter.Create(sb, settings))
                {
                    doc.Save(writer);
                }
                envelope = sb.ToString();
            }

            return envelope;
        }
    }
}
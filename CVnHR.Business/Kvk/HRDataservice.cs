﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace CVnHR.Business.Kvk
{
    //Example from:
    //https://stackoverflow.com/questions/47104618/how-do-i-call-xml-soap-service-that-requires-signature-from-net-core/48818293#48818293


    public class HRDataservice
    {
        private readonly string _klantReferentie;
        private readonly IHttpClientFactory _httpClientFactory;

        public HRDataservice(string klantReferentie, IHttpClientFactory httpClientFactory)
        {
            _klantReferentie = klantReferentie;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<object> GetInschrijvingFromKvK(string kvkNummer)
        {

            // TODO: make this work without having the certificate in the store... (SSL, 100-continue)

            // Create a collection object and populate it using the PFX file
            var password = File.ReadAllText("Certificates/digilevering.drenthe.nl.txt");
            var certificate = new X509Certificate2("Certificates/digilevering.drenthe.nl.pfx",  password);
            var responseMessage = string.Empty; 
            var envelope = BuildEnvelope(certificate, kvkNummer);

            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
                CheckCertificateRevocationList = false,
                ClientCertificateOptions = ClientCertificateOption.Manual,
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = cookieContainer,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                //PreAuthenticate = true,
                
                UseProxy = true,
                Proxy = new WebProxy("http://127.0.0.1:8888", false),
                
            };

            handler.ClientCertificates.Add(certificate);
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => { return true; };

            //TODO: var client = _httpClientFactory.CreateClient();
            var client = new HttpClient(new LoggingHandler(handler));
            client.DefaultRequestHeaders.Connection.Add("Keep-Alive");
            client.DefaultRequestHeaders.ExpectContinue = true;
            client.DefaultRequestHeaders.Add("SOAPAction", "\"http://es.kvk.nl/ophalenInschrijving\"");

            var content = new StringContent(envelope, Encoding.UTF8, "text/xml");
            var response = await client.PostAsync("https://webservices.kvk.nl/postbus1", content);

            if (response.IsSuccessStatusCode)
            {
                responseMessage = await response.Content.ReadAsStringAsync();
            }

            Console.WriteLine(responseMessage);

            return responseMessage;
        }

        private string BuildEnvelope(X509Certificate2 certificate, string kvkNummer)
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
                    writer.WriteString("http://es.kvk.nl/ophalenInschrijving");
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


                    // TODO!!!

                    /*
                     <ophalenInschrijvingRequest xmlns="http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02">
                        <klantreferentie>ACC_I_002</klantreferentie>
                        <kvkNummer>53352009</kvkNummer>
                     </ophalenInschrijvingRequest>
                    */

                    // your 3rd-party soap payload goes here
                    writer.WriteStartElement("ophalenInschrijvingRequest", "http://schemas.kvk.nl/schemas/hrip/dataservice/2015/02");

                    writer.WriteStartElement("klantreferentie");
                    writer.WriteValue(_klantReferentie);
                    writer.WriteEndElement(); // klantreferentie

                    writer.WriteStartElement("kvkNummer");
                    writer.WriteValue(kvkNummer);
                    writer.WriteEndElement(); // kvkNummer

                    writer.WriteEndElement(); // ophalenInschrijvingRequest


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

//#error //TODO: fix this so we don't get any errors anymore.

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
                    //Indent = true,
                    //IndentChars = "  ",
                    //NewLineChars = "\r\n",
                    //NewLineHandling = NewLineHandling.Replace,
                    Indent = false,
                    OmitXmlDeclaration = true,
                    WriteEndDocumentOnClose = true
                };
                using (var writer = XmlWriter.Create(sb, settings))
                {
                    doc.Save(writer);
                }
                envelope = sb.ToString();

                //envelope = doc.OuterXml;
            }

            return envelope;
        }

        public class SignedXmlWithId : SignedXml
        {
            public SignedXmlWithId(XmlDocument xml) : base(xml)
            {
            }

            public SignedXmlWithId(XmlElement xmlElement)
                : base(xmlElement)
            {
            }

            public override XmlElement GetIdElement(XmlDocument doc, string id)
            {
                // check to see if it's a standard ID reference
                XmlElement idElem = base.GetIdElement(doc, id);

                if (idElem == null)
                {
                    XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                    nsManager.AddNamespace("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

                    idElem = doc.SelectSingleNode("//*[@wsu:Id=\"" + id + "\"]", nsManager) as XmlElement;
                }

                return idElem;
            }
        }
    }

    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request:");
            Console.WriteLine(request.ToString());
            if (request.Content != null)
            {
                Console.WriteLine(await request.Content.ReadAsStringAsync());
            }
            Console.WriteLine();

            try
            {
                var result = base.SendAsync(request, cancellationToken);
                var response = await result;

                Console.WriteLine("Response statuscode:");
                Console.WriteLine(response.StatusCode);
                if (response.Content != null)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                Console.WriteLine();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
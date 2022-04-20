using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace COAD.FISCAL.XmlUtils
{
    public class XmlUtil
    {

        public static XmlDocument AssinarXml<TSource>(TSource obj, X509Certificate2 certificate, string tagParaAssinar, string tagOndeSeraInserida, string namespaces = null)
        {
            try
            {
                if (certificate == null)
                    throw new ArgumentNullException("O certificado 'certificate' não foi informado");

                string namespacesSerialization = (!string.IsNullOrWhiteSpace(namespaces)) ? namespaces : "http://www.portalfiscal.inf.br/nfe";
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, namespacesSerialization);

                XmlDocument xml = null;
                var ser = new XmlSerializer(obj.GetType());

                using (MemoryStream memoryStr = new MemoryStream())
                {
                    ser.Serialize(memoryStr, obj, ns);
                    memoryStr.Position = 0;

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreWhitespace = true;

                    using (XmlReader xmlR = XmlReader.Create(memoryStr, settings))
                    {
                        xml = new XmlDocument();
                        xml.Load(xmlR);
                    }
                }

                if (xml != null)
                {
                    XmlUtil.AssinarXml(xml, certificate, tagParaAssinar, tagOndeSeraInserida);
                }
                return xml;
            }
            catch (Exception e)
            {
                var erro = string.Format("Erro ao gerar a nota fiscal.");
                throw new Exception(erro, e);
            }
        }

        /// <summary>
        /// Assina com um certificado digital um documento xml dado o nome do atributo que será assinada e onde a assinatura será colocada
        /// </summary>
        /// <param name="doc">Documento Xml</param>
        /// <param name="certificate">Certificado válido para assinar o Xml</param>
        /// <param name="tagParaAssinar">nome do elemento que será assinado pelo certificado</param>
        /// <param name="xPathParaAssinar">Em qual elemento a assinatura gerada será inserida no xml</param>
        /// <returns></returns>
        public static XmlDocument AssinarXml(XmlDocument doc, X509Certificate2 certificate, string tagParaAssinar = "infEvento", string xPathParaAssinar = "evento")
        {
            if (doc != null && certificate != null && doc.GetElementsByTagName(tagParaAssinar).Count > 0)
            {
                
                XmlNodeList elements = doc.GetElementsByTagName(tagParaAssinar);


                if (elements != null && elements.Count > 0)
                {
                    var index = 0;

                    foreach (XmlNode element in elements)
                    {
                        SignedXml signedXml = new SignedXml(doc);
                        signedXml.SigningKey = certificate.PrivateKey;
                        Reference reference = new Reference();
                        XmlAttributeCollection _Uri = element.Attributes;
                        reference.Uri = FindUrl(_Uri);

                        XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                        XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();

                        reference.AddTransform(env);
                        reference.AddTransform(c14);

                        signedXml.AddReference(reference);

                        KeyInfo keyInfo = new KeyInfo();
                        keyInfo.AddClause(new KeyInfoX509Data(certificate));
                        signedXml.KeyInfo = keyInfo;
                        signedXml.ComputeSignature();

                        XmlElement xmlDigitalSignatura = signedXml.GetXml();
                        var node = element.SelectSingleNode((string)xPathParaAssinar);

                        node.AppendChild(doc.ImportNode(xmlDigitalSignatura, true));

                        doc.PreserveWhitespace = false;

                        index++;
                    }
                }
                return doc;
            }

            return null;
        }

        static string FindUrl(XmlAttributeCollection collections)
        {
            foreach (XmlAttribute attr in collections)
            {
                if (attr.Name == "Id")
                {
                    return "#" + attr.InnerText;
                }
            }
            return null;
        }

        public static void SerializeXml(XmlDocument xml, string fileName, bool identado = true)
        {
            SerializeXml((XmlNode)xml, fileName, identado);
        }
        
        public static void SerializeXml(XmlNode xml, string fileName, bool identado = true)
        {
            try
            {
                if (xml != null)
                {


                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, "http://www.portalfiscal.inf.br/nfe");

                    XmlSerializer ser = new XmlSerializer(typeof(XmlDocument));

                    if (!identado)
                    {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = false;
                        settings.NewLineHandling = NewLineHandling.None;

                        XmlWriter writer = XmlTextWriter.Create(fileName, settings);

                        ser.Serialize(writer, xml, ns);
                        writer.Close();

                    }
                    else
                    {
                        TextWriter writer = new StreamWriter(fileName);
                        ser.Serialize(writer, xml, ns);
                        writer.Close();
                    }
                }
            }
            catch (Exception e)
            {
                var erro = string.Format("Erro ao gerar a nota fiscal. Verifique se o caminho {0} em que o arquivo será gerado  existe e se o sistema possui permissão para acessar.", fileName);
                throw new Exception(erro, e);
            }
        }

        public static string SerializeAsXml<TSource>(TSource obj, bool identado = true, bool omitXmlDeclaration = false)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, "http://www.portalfiscal.inf.br/nfe");
            string value = null;

            if (!identado)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.NewLineHandling = NewLineHandling.None;
                settings.Encoding = Encoding.UTF8;
                settings.OmitXmlDeclaration = omitXmlDeclaration;

                var ser = new XmlSerializer(obj.GetType());
                var txWriter = new StringWriter();
                XmlWriter writer = XmlTextWriter.Create(txWriter, settings);
                ser.Serialize(writer, obj, ns);

                value = txWriter.ToString();

                writer.Close();

            }
            else
            {

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.OmitXmlDeclaration = omitXmlDeclaration;

                var ser = new XmlSerializer(obj.GetType());
                var txWriter = new StringWriter();
                XmlWriter writer = XmlTextWriter.Create(txWriter, settings);
                ser.Serialize(writer, obj, ns);

                value = txWriter.ToString();

                writer.Close();

            }
            return value;
        }

        public static void SerializeAsXml<TSource>(TSource obj, string fileName, bool identado = false)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, "http://www.portalfiscal.inf.br/nfe");


            var ser = new XmlSerializer(obj.GetType());

            if (!identado)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.NewLineHandling = NewLineHandling.None;

                XmlWriter writer = XmlTextWriter.Create(fileName, settings);

                ser.Serialize(writer, obj, ns);
                writer.Close();

            }
            else
            {
                TextWriter writer = new StreamWriter(fileName);
                ser.Serialize(writer, obj, ns);
                writer.Close();
            }


        }

        public static byte[] SerializeAsXmlBinary<TSource>(TSource obj, string fileFullName, bool identado = false)
        {
            SerializeAsXml(obj, fileFullName, identado);

            if (File.Exists(fileFullName))
            {
                var binary = File.ReadAllBytes(fileFullName);
                return binary;
            }
            return null;
        }

        public static byte[] SerializeAsXmlBinary<TSource>(TSource obj, bool deleteFile, string fileName, bool identado = false)
        {
            var path = @"C:\xmlTemp";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var fullPath = path + @"\" + fileName;

            SerializeAsXml(obj, fullPath, identado);

            if (File.Exists(fullPath))
            {
                var binary = File.ReadAllBytes(fullPath);
                if (deleteFile)
                {
                    File.Delete(fullPath);
                }
                return binary;
            }
            return null;
        }
        public static void SerializeAsXmlWithSignature<TSource>(TSource obj, string fileName, string certPath, string password, string tagParaAssinar, string xPathParaAssinar, bool identado = true)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, "http://www.portalfiscal.inf.br/nfe");

                XmlDocument xml = null;
                var ser = new XmlSerializer(obj.GetType());

                using (MemoryStream memoryStr = new MemoryStream())
                {
                    ser.Serialize(memoryStr, obj, ns);
                    memoryStr.Position = 0;

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreWhitespace = true;

                    using (XmlReader xmlR = XmlReader.Create(memoryStr, settings))
                    {
                        xml = new XmlDocument();
                        xml.Load(xmlR);
                    }
                }

                if (xml != null)
                {
                    X509Certificate2 actualCert = new X509Certificate2(certPath, password, X509KeyStorageFlags.MachineKeySet);
                    XmlUtil.AssinarXml(xml, actualCert, tagParaAssinar, xPathParaAssinar);
                    XmlUtil.SerializeXml(xml, fileName, identado);
                }
            }
            catch (CryptographicException e)
            {
                var erro = string.Format("Erro ao gerar a nota fiscal. Verifique se o caminho do certificado {0} existe e se o sistema possui permissão para acessar. Caminho para salvar o arquivo {1}", certPath, fileName);
                throw new Exception(erro, e);
            }
            catch (Exception e)
            {
                var erro = string.Format("Erro ao gerar a nota fiscal. Verifique se o caminho {0} em que o arquivo será gerado  existe e se o sistema possui permissão para acessar. CertPath {1}", fileName, certPath);
                throw new Exception(erro, e);
            }
        }

        public static TDest LoadFromXMLString<TDest>(string xmlText)
        {
            var stringReader = new StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(TDest));
            TDest resp = (TDest) serializer.Deserialize(stringReader);
            stringReader.Close();
            return resp;
        }

        public static TDest LoadFromXMLBytes<TDest>(byte[] bytes)
        {

            var path = @"C:\xmlTemp\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var file = string.Format(@"{0}output-{1:yyyy-MM-ddThh-mm-ss}-{2}.xml", path, DateTime.Now, new Random().Next(10000));

            File.WriteAllBytes(file, bytes);
            var xmlContent = File.ReadAllText(file);

            File.Delete(file);
            TDest dest = XmlUtil.LoadFromXMLString<TDest>(xmlContent);
            return dest;
        }

        public static TDest LoadFromXMLDocument<TDest>(XmlNode xmlNode)
        {
            var xmlNodeReader = new XmlNodeReader(xmlNode);
            var serializer = new XmlSerializer(typeof(TDest));
            TDest resp = (TDest)serializer.Deserialize(xmlNodeReader);
            xmlNodeReader.Close();
            return resp;
        }

        public static TDest LoadFromStream<TDest>(StreamReader reader)
        {
            TDest resp = default(TDest);
            using (reader)
            {
                var serializer = new XmlSerializer(typeof(TDest));
                resp = (TDest)serializer.Deserialize(reader);                
            }

            return resp;
        }

        public static XmlDocument SerializeAsXmlDocumentWithSignature<TSource>(TSource obj, string fileName, string certPath, string password, string tagParaAssinar, string xPathParaAssinar, bool identado = true)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, "http://www.portalfiscal.inf.br/nfe");

                XmlDocument xml = null;
                var ser = new XmlSerializer(obj.GetType());

                using (MemoryStream memoryStr = new MemoryStream())
                {
                    ser.Serialize(memoryStr, obj, ns);
                    memoryStr.Position = 0;

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreWhitespace = true;

                    using (XmlReader xmlR = XmlReader.Create(memoryStr, settings))
                    {
                        xml = new XmlDocument();
                        xml.Load(xmlR);
                    }
                }

                if (xml != null)
                {
                    X509Certificate2 actualCert = new X509Certificate2(certPath, password, X509KeyStorageFlags.MachineKeySet);
                    XmlUtil.AssinarXml(xml, actualCert, tagParaAssinar, xPathParaAssinar);
                }

                return xml;
            }
            catch (CryptographicException e)
            {
                var erro = string.Format("Erro ao gerar a nota fiscal. Verifique se o caminho do certificado {0} existe e se o sistema possui permissão para acessar. Caminho para salvar o arquivo {1}", certPath, fileName);
                throw new Exception(erro, e);
            }
            catch (Exception e)
            {
                var erro = string.Format("Erro ao gerar a nota fiscal. Verifique se o caminho {0} em que o arquivo será gerado  existe e se o sistema possui permissão para acessar. CertPath {1}", fileName, certPath);
                throw new Exception(erro, e);
            }
        }

        public static XmlDocument SerializeAsXmlDocument<TSource>(TSource obj, string nameSpace = null)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            if (!string.IsNullOrWhiteSpace(nameSpace))
            {
                ns.Add(string.Empty, nameSpace);
            }
            else
            {
                ns.Add(string.Empty, "http://www.portalfiscal.inf.br/nfe");
            }

            XmlDocument xml = null;
            var ser = new XmlSerializer(obj.GetType());

            using (MemoryStream memoryStr = new MemoryStream())
            {
                ser.Serialize(memoryStr, obj, ns);
                memoryStr.Position = 0;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                using (XmlReader xmlR = XmlReader.Create(memoryStr, settings))
                {
                    xml = new XmlDocument();
                    xml.Load(xmlR);
                }
            }                

            return xml;
            
        }

        public static void SerializarObjetoTeste(XmlDocument xml)
        {
            if (!Directory.Exists(@"C:\teste_serializacao"))
                Directory.CreateDirectory(@"C:\teste_serializacao");

            string fileName = string.Format(@"\serializacao_{0:yyyy-MM-ddTH-mm}.xml", DateTime.Now);
            string path = Path.Combine(@"C:\teste_serializacao", Path.GetFileName(fileName));
            XmlUtil.SerializeXml(xml, path);
        }

        public static XElement SerializeAsXElement<TSource>(TSource obj)
        {
            var xmlDoc = SerializeAsXmlDocument(obj);
            if(xmlDoc != null)
            {
                XElement xmlElement = XElement.Load(new XmlNodeReader(xmlDoc));
                return xmlElement;
            }
            return null;
        }

        public static TSource LoadAsXElement<TSource>(XElement obj) where TSource : class
        {
            if (obj != null)
            {
                var xmlSerializer = new XmlSerializer(typeof(TSource));
                var resp = (TSource)xmlSerializer.Deserialize(obj.CreateReader());
                return resp;
            }
            return null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace COAD.CRYPT.XmlUtils
{
    public class XmlUtil
    {
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

        public static void SerializeXml(XmlDocument xml, string fileName)
        {
            SerializeXml((XmlNode)xml, fileName);
        }

        public static void SerializeXml(XmlNode xml, string fileName)
        {
            if (xml != null)
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                TextWriter writer = new StreamWriter(fileName);
                XmlSerializer ser = new XmlSerializer(typeof(XmlDocument));
                ser.Serialize(writer, xml, ns);
                writer.Close();
            }
        }
    }
}

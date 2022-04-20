using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace COAD.PAGAMENTO.API.Parsers
{
    public class XmlParse
    {
        public string ParseToString(XmlDocument doc)
        {
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

            doc.WriteTo(xmlTextWriter);

            return stringWriter.ToString();
        }
    }
}

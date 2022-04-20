using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace COAD.PAGAMENTO.API.Parsers
{
    public class InstrucaoParser : XmlParse
    {
        public static string XmlPath =   HttpRuntime.AppDomainAppPath + "Xml/teste.xml";

        public XmlDocument ParseInstrucao()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(XmlPath);
            return xml;
        }

      
    }
}

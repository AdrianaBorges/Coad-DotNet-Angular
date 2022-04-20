using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    [Serializable]
    [XmlRoot("cabecalho", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    public class CabecMsgWebSrv
    {
        [XmlAttribute]
        public string Versao { get; set; }
        public string versaoDados { get; set; }
    }
}

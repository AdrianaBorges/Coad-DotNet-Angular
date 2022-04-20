using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    [Serializable]
    [XmlRoot("ConsultarLoteRpsEnvio", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    public class ConsultarLoteRpsEnvio
    {
        public IdentificacaoPrestadorRps Prestador { get; set; }
        public string Protocolo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    [Serializable]
    [XmlRoot("ConsultarNfseRpsEnvio", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    public class ConsultarNfseRpsEnvio
    {
        public IdentificacaoRps IdentificacaoRps { get; set; }
        public IdentificacaoPrestadorRps Prestador { get; set; }
    }
}

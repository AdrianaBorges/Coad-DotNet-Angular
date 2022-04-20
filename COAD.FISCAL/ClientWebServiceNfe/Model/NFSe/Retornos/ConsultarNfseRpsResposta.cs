using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Retornos
{
    [Serializable]
    [XmlRoot("ConsultarNfseRpsResposta", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    public class ConsultarNfseRpsResposta
    {
        public CompNfse CompNfse { get; set; }
        public ListaMensagemRetornoNfse ListaMensagemRetorno { get; set; }
    }
}

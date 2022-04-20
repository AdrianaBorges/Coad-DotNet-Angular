using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{

    [XmlRoot("CancelarNfseResposta", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    [Serializable]
    public class CancelarNfseResposta
    {
        public CancelamentoNfse Cancelamento { get; set; }
        public ListaMensagemRetornoNfse ListaMensagemRetorno { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Retornos
{

    [Serializable]
    [XmlRoot("ConsultarLoteRpsResposta", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    public class ConsultarLoteRpsResposta
    {
        public ConsultarLoteRpsResposta()
        {
            ListaNfse = new List<CompNfse>();
        }
        public List<CompNfse> ListaNfse { get; set; }
        public ListaMensagemRetornoNfse ListaMensagemRetorno { get; set; }
    }
}

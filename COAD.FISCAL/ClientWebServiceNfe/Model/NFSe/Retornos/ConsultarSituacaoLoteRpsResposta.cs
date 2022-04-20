using COAD.FISCAL.Model.NFSe.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Retornos
{
    [Serializable]
    [XmlRoot("ConsultarSituacaoLoteRpsResposta", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    public class ConsultarSituacaoLoteRpsResposta
    {
        public int? NumeroLote { get; set; }
        public SituacaoLoteRpsEnum Situacao { get; set; }
        public ListaMensagemRetornoNfse ListaMensagemRetorno { get; set; }
    }
}

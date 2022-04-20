using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Retornos
{
    [Serializable]
    [XmlRoot("EnviarLoteRpsResposta", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    public class EnviarLoteRpsResposta
    {
        public string NumeroLote { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public string Protocolo { get; set; }
        public ListaMensagemRetornoNfse ListaMensagemRetorno { get; set; }
    }
}

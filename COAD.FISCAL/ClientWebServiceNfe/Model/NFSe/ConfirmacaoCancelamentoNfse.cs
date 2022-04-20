using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    public class ConfirmacaoCancelamentoNfse
    {
        [XmlAttribute]
        public string Id { get; set; }
        public PedidoCancelamentoNfse Pedido { get; set; }
        public InfConfirmacaoCancelamentoNfse InfConfirmacaoCancelamento { get; set; }
    }
}

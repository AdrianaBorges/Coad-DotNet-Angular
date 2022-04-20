using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{

    public class InfPedidoCancelamentoNfse
    {
        [XmlAttribute]
        public string Id { get; set; }
        public IdentificacaoNfse IdentificacaoNfse { get; set; }
        public string CodigoCancelamento { get; set; }
    }
}

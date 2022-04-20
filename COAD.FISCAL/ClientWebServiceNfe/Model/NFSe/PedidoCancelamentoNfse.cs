using COAD.FISCAL.Model.DTOCriptografia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    public class PedidoCancelamentoNfse
    {
        public InfPedidoCancelamentoNfse InfPedidoCancelamento { get; set; }
        public SignatureDTO Signature { get; set; }
    }
}

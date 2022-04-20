using COAD.FISCAL.Model.DTOCriptografia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class CancelamentoNfse
    {
        public ConfirmacaoCancelamentoNfse Confirmacao { get; set; }
        public SignatureDTO Signature { get; set; }
    }
}

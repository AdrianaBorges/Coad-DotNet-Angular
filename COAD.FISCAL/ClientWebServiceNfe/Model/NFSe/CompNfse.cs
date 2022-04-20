using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    [Serializable]

    public class CompNfse
    {
        public NFse Nfse { get; set; }
        public CancelamentoNfse NfseCancelamento { get; set; }
        public SubstituicaoNfse SubstituicaoNfse { get; set; }
    }
}

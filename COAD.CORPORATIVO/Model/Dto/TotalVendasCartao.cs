using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class TotalVendasCartaoDTO
    {

        public int EMP_ID { get; set; }
        public int FOR_ID { get; set; }
        public int TVC_MES { get; set; }
        public int TVC_ANO { get; set; }
        public Nullable<decimal> TVC_VLR_CARTAO_DEB { get; set; }
        public Nullable<decimal> TVC_VLR_CARTAO_CRE { get; set; }

        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }
        public virtual FornecedorDTO FORNECEDOR { get; set; }
    }
}

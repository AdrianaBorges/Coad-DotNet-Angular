using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class CFOPIcmsDTO
    {
        public string CFOP { get; set; }
        public string UF_EMPRESA { get; set; }
        public string UF_NOTA { get; set; }
        public Nullable<decimal> ICMS_ENTRADA { get; set; }
        public Nullable<decimal> ICMS_SAIDA { get; set; }

        public virtual CFOTableDTO CFOP_TABLE { get; set; }
        public virtual UFDTO UF { get; set; }
        public virtual UFDTO UF1 { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class CFOPReferenciaDTO
    {
        public string CFOP_ENT { get; set; }
        public string CFOP_SAI { get; set; }
        public Nullable<int> CFOP_ATIVO { get; set; }

        public virtual CFOTableDTO CFOP_TABLE { get; set; }
        public virtual CFOTableDTO CFOP_TABLE1 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Impostos
{
    public class RequisicaoConfigImpostoDTO
    {
        public int? CmpID { get; set; }
        public int? NfcId { get; set; }
        public int? TipoCliId { get; set; }
        public bool? EmpresaDoSimples { get; set; }
        public bool? ClienteRetem { get; set; }
        public bool? SobreTotal { get; set; }
    }
}

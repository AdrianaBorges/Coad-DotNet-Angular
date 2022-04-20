using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class CFOTableDTO
    {
        public CFOTableDTO()
        {
            this.CFOP_ICMS = new HashSet<CFOPIcmsDTO>();
            this.CFOP_REFERENCIA = new HashSet<CFOPReferenciaDTO>();
            this.CFOP_REFERENCIA1 = new HashSet<CFOPReferenciaDTO>();
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
            this.NOTA_FISCAL_ITEM = new HashSet<NotaFiscalItemDTO>();
        }

        public string CFOP { get; set; }
        public string CFOP_DESCRICAO { get; set; }
        public string CFOP_TIPO { get; set; }

        public virtual ICollection<CFOPIcmsDTO> CFOP_ICMS { get; set; }
        public virtual ICollection<CFOPReferenciaDTO> CFOP_REFERENCIA { get; set; }
        public virtual ICollection<CFOPReferenciaDTO> CFOP_REFERENCIA1 { get; set; }
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }
        public virtual ICollection<NotaFiscalItemDTO> NOTA_FISCAL_ITEM { get; set; }
    }
}

using COAD.RM.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.RM.Model.Dto
{
    public class ContraChequeCustomDTO
    {
        public ContraChequeCustomDTO()
        {
            this.Lista = new HashSet<COAD_EMISSAO_CONTRACHEQUES_Result>();
        }
        public Nullable<decimal> CCH_TOT_VLR_PROVENTO { get; set; }
        public Nullable<decimal> CCH_TOT_VLR_DESCONTOS { get; set; }

        public virtual ICollection<COAD_EMISSAO_CONTRACHEQUES_Result> Lista { get; set; }
    }
}

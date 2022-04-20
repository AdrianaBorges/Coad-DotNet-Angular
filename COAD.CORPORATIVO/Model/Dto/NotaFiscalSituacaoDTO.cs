using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{  
    public partial class NotaFiscalSituacaoDTO
    {
        public NotaFiscalSituacaoDTO()
        {
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
        }
    
        public string NF_COD_SIT { get; set; }
        public string NF_DESCRICAO_SIT { get; set; }
    
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }
    }
}

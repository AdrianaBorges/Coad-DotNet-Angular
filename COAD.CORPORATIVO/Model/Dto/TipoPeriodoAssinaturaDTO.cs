using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoPeriodoAssinaturaDTO
    {
        public TipoPeriodoAssinaturaDTO()
        {
            this.ASSINATURA = new HashSet<AssinaturaDTO>();
        }
    
        public int TP_ASS_ID { get; set; }
        public string TP_ASS_DESCRICAO { get; set; }
        public Nullable<int> TP_ASS_QTD_PERIODO { get; set; }
    
        public virtual ICollection<AssinaturaDTO> ASSINATURA { get; set; }
    }
}

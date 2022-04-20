using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class TipoCancelamentoDTO
    {
        public TipoCancelamentoDTO()
        {
            this.MOTIVO_CANCELAMENTO = new HashSet<MotivoCancelamentoDTO>();
        }
    
        public string TIP_CANC_ID { get; set; }
        public string TIP_CANC_DESCRICAO { get; set; }

        public virtual ICollection<MotivoCancelamentoDTO> MOTIVO_CANCELAMENTO { get; set; }
    }
}

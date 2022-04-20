using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class UraTPAtuDTO
    {
        public UraTPAtuDTO()
        {
            this.URA_LOG = new HashSet<UraLogDTO>();
        }
    
        public int URA_TP_ATU_ID { get; set; }
        public string URA_TP_ATU_DESCRICAO { get; set; }
    
        public virtual ICollection<UraLogDTO> URA_LOG { get; set; }
    }
}

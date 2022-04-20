using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class ManualDPModuloDTO
    {
        public ManualDPModuloDTO()
        {
            this.MANUAL_DP = new HashSet<ManualDPDTO>();
        }
    
        public int MOD_ID { get; set; }
        public string MOD_DESCRICAO { get; set; }

        public virtual ICollection<ManualDPDTO> MANUAL_DP { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class ManualDPDTO
    {
        public ManualDPDTO()
        {
            this.MANUAL_DP_ITEM = new HashSet<ManualDPItemDTO>();
            this.MENU_DOC = new HashSet<MenuDocDTO>();
        }
    
        public int MAN_ID { get; set; }
        public string MAN_ASSUNTO { get; set; }
        public Nullable<System.DateTime> MAN_DATA_PUBLICACAO { get; set; }
        public Nullable<int> MOD_ID { get; set; }
        public Nullable<int> MAN_INDEX { get; set; }

        public virtual ICollection<ManualDPItemDTO> MANUAL_DP_ITEM { get; set; }
        public virtual ManualDPModuloDTO MANUAL_DP_MODULO { get; set; }
        public virtual ICollection<MenuDocDTO> MENU_DOC { get; set; }


    }
}

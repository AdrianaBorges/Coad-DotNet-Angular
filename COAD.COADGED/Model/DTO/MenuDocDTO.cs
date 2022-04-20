using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    
    public partial class MenuDocDTO
    {
        public MenuDocDTO()
        {
            this.MENU_DOC_ITEM = new HashSet<MenuDocItemDTO>();
        }
    
        public Nullable<int> MND_ID { get; set; }
        public string MND_DESCRICAO { get; set; }
        public Nullable<int> MAN_ID { get; set; }
    
        public virtual ManualDPDTO MANUAL_DP { get; set; }
        public virtual ICollection<MenuDocItemDTO> MENU_DOC_ITEM { get; set; }
    }
}

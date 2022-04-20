using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class MenuDocItemDTO
    {
        public MenuDocItemDTO()
        {
            this.MENU_DOC_ITEM1 = new HashSet<MenuDocItemDTO>();
        }
    
        public Nullable<int> MNI_ID { get; set; }
        public int MND_ID { get; set; }
        public string MND_DESCRICAO { get; set; }
        public Nullable<int> MNI_ID_NODE { get; set; }
        public Nullable<int> MNI_MENU_SEQ { get; set; }
        public Nullable<int> MNI_MENU_NIVEL { get; set; }
        public Nullable<int> MNI_TIPO { get; set; }
        public Nullable<int> ITM_ATIVO { get; set; }

        public virtual MenuDocDTO MENU_DOC { get; set; }
        public virtual ICollection<MenuDocItemDTO> MENU_DOC_ITEM1 { get; set; }
        public virtual MenuDocItemDTO MENU_DOC_ITEM2 { get; set; }
    }
}

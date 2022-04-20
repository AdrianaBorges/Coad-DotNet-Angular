using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class ManualDPLinkDTO
    {
        public string LNK_TAG { get; set; }
        public Nullable<int> MAI_ID { get; set; }
        public string LNK_DESCRICAO { get; set; }
        public string LNK_LINK { get; set; }

        public virtual ManualDPItemDTO MANUAL_DP_ITEM { get; set; }
    }
}

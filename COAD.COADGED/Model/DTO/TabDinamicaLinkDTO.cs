using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class TabDinamicaLinkDTO
    {
        public string LNK_TAG { get; set; }
        public string TDC_ID { get; set; }
        public string LNK_DESCRICAO { get; set; }
        public string LNK_LINK { get; set; }

        public virtual TabDinamicaConfigDTO TAB_DINAMICA_CONFIG { get; set; }
    }
}

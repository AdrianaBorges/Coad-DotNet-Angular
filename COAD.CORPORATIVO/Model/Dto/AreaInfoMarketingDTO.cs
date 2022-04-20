using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class AreaInfoMarketingDTO
    {
        public int? MKT_CLI_ID { get; set; }
        public int? AREA_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }

        public virtual AreasCorpDTO AREAS { get; set; }
        public virtual InfoMarketingDTO INFO_MARKETING { get; set; }
    }
}

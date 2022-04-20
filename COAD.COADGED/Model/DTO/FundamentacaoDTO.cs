using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class FundamentacaoDTO
    {
        public int FUN_ID { get; set; }
        public Nullable<int>  MAI_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public System.DateTime DATA_INSERT { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<int> TIP_ATO_ID { get; set; }
        public Nullable<int> MAI_NUMERO_ATO { get; set; }
        public string MAI_DATA_ATO { get; set; }
        public Nullable<int> ORG_ID { get; set; }
        public string MAI_NUMERO_ARTIGO { get; set; }
        public Nullable<int> FUN_NUM_PARAGRAFO { get; set; }
        public string FUN_INCISO { get; set; }

        public virtual ManualDPItemDTO MANUAL_DP_ITEM { get; set; }
    }
}

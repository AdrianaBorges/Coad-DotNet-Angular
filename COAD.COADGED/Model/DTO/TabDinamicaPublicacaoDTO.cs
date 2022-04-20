using System;
using System.Collections.Generic;

namespace COAD.COADGED.Model.DTO
{
    public partial class TabDinamicaPublicacaoDTO
    {
        public int TPU_ID { get; set; }
        public string TDC_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public string USU_LOGIN_APROV { get; set; }
        public Nullable<System.DateTime> TPU_DATA_LANC { get; set; }
        public Nullable<System.DateTime> TPU_DATA_APROV { get; set; }
        public string TPU_OBS { get; set; }
        public string TPU_STATUS { get; set; }
        
        public virtual TabDinamicaConfigDTO TAB_DINAMICA_CONFIG { get; set; }
    }
}

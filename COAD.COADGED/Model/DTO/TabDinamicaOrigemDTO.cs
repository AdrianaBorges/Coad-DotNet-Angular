using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class TabDinamicaOrigemDTO
    {
        public int OAC_ID { get; set; }
        public string TDC_ID { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }

        public virtual OrigemAcessoRefDTO ORIGEM_ACESSO_REF { get; set; }
        public virtual TabDinamicaConfigDTO TAB_DINAMICA_CONFIG { get; set; }
    }
}

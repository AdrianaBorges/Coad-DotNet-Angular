using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class UraLogDTO
    {
        public int ULG_ID { get; set; }
        public string URA_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public System.DateTime ULG_DATA { get; set; }
        public int URA_TP_ATU_ID { get; set; }
        public string ULG_OBS { get; set; }
        public string USU_LOGIN { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual UraTPAtuDTO URA_TP_ATU { get; set; }
    }

}

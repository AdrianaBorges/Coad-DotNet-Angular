using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial  class ChequeDevolvidoDTO
    {
        public string ASN_NUM_ASSINATURA { get; set; }
        public string BAN_ID { get; set; }
        public string AGE_ID { get; set; }
        public string CHD_NUM_CHEQUE { get; set; }
        public string CHD_REF { get; set; }
        public Nullable<decimal> CHD_VALOR_CHEQUE { get; set; }
        public Nullable<System.DateTime> CHD_DATA_CHEQUE { get; set; }
        public string CHD_SOLICITANTE { get; set; }
        public Nullable<System.DateTime> CHD_DATA_LANCTO { get; set; }
        public Nullable<System.DateTime> CHD_DATA_IMP_FICHA { get; set; }
        public string CHD_CANCELADO { get; set; }
        public string CHD_EMITENTE { get; set; }
        public Nullable<System.DateTime> CHD_DATA_SUBST_REAP { get; set; }
        public Nullable<System.DateTime> CHD_DATA_1DEVOL { get; set; }
        public Nullable<System.DateTime> CHD_DATA_2DEVOL { get; set; }
        public string CHD_ATRIB_OPER_EM { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual BancosDTO BANCOS { get; set; }
    }
}

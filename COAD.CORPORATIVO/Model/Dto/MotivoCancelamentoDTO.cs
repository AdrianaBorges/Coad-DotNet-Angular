using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class MotivoCancelamentoDTO
    {
        public int MCA_ID { get; set; }
        public string MCA_DESCRICAO { get; set; }
        public string TIP_CANC_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual ContratoDTO CONTRATOS { get; set; }
        public virtual TipoCancelamentoDTO TIPO_CANCELAMENTO { get; set; }
    }
}

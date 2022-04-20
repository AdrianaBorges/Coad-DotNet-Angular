using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public class RelFaturamentoContratoDTO
    {
        public int? EMP_ID { get; set; }
        public string EMP_RAZAO_SOCIAL { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string PED_NUM_PEDIDO { get; set; }
        public Nullable<DateTime> CTR_DATA_FAT { get; set; }
        public Nullable<DateTime> CTR_DATA_INI_VIGENCIA { get; set; }
        public Nullable<DateTime> CTR_DATA_FIM_VIGENCIA { get; set; }
        public Nullable<DateTime> CTR_DATA_CANC { get; set; }
        public string SITUACAO { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string AEM_EMAIL { get; set; }
        public Nullable<decimal> CTR_VLR_CONTRATO { get; set; }
        public Nullable<int> CTR_NUMERO_NOTA { get; set; }
        public Nullable<int> IPE_ID { get; set; }
    }
}

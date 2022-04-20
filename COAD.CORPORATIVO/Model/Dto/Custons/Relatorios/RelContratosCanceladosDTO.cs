using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{    
    /// <summary>
    /// 
    /// </summary>
    public class RelContratosCanceladosDTO
    {
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public string REP_NOME { get; set; } 
        public int? CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string ASN_A_C { get; set; }
        public DateTime? CTR_DATA_FAT { get; set; }
        public DateTime? CTR_DATA_CANC { get; set; }
        public DateTime? CTR_DATA_INI_VIGENCIA { get; set; }
        public DateTime? CTR_DATA_FIM_VIGENCIA { get; set; }
        public decimal? CTR_VLR_CONTRATO { get; set; }
        public string CAR_ID { get; set; }
        public Nullable<decimal> CTR_VLR_BRUTO { get; set; }

    }
}

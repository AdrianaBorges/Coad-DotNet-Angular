using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ParcelaUpdateDTO
    {
        [Required]
        public string PAR_NUM_PARCELA { get; set; }
        [Required]
        public Nullable<System.Int32> CTA_ID { get; set; }
        [Required]
        public string BAN_ID { get; set; }
        [Required]
        public Nullable<System.DateTime> PAR_DATA_ALOC { get; set; }
        [Required]
        public string PAR_REMESSA { get; set; }
        [Required]
        public string PAR_TRANSMITIDO { get; set; }
        [Required]
        public string PAR_NOSSO_NUMERO { get; set; }
        [Required]
        public Nullable<System.Decimal> PAR_VLR_PAGO { get; set; }
        [Required]
        public Nullable<System.DateTime> PAR_DATA_PAGTO { get; set; }
        public bool STATUS { get; set; }
    }
}

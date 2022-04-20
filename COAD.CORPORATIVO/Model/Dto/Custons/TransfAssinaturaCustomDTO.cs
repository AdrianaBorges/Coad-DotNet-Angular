using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public partial class TransfAssinaturaCustomDTO
    {
        public string CLI_NOME { get; set; }
        [Required(ErrorMessage = "Informe o período de vigência")]
        public Nullable<System.DateTime> CTR_DATA_INI_VIGENCIA { get; set; }
        [Required(ErrorMessage = "Informe o período de vigência")]
        public Nullable<System.DateTime> CTR_DATA_FIM_VIGENCIA { get; set; }
        [Required(ErrorMessage = "Informe o numero do ultimo contrato")]
        public string CTR_NUM_CONTRATO { get; set; }
        public string ASN_NUM_ASSINATURA_ANT { get; set; }
        [Required(ErrorMessage = "Informe o numero da assinatura")]
        public string ASN_NUM_ASSINATURA { get; set; }
        public string SOLICITANTE { get; set; }
        public string USU_LOGIN { get; set; }
        [Required(ErrorMessage = "Informe o ano de vigência")]
        public string CTR_ANO_VIGENCIA { get; set; }
        [Required(ErrorMessage = "Informe o motívo da transferêcia")]
        public string MOTIVO { get; set; }
        public string DATA_TRANSF { get; set; }
        public int MES_REFERENCIA { get; set; }

    }
}

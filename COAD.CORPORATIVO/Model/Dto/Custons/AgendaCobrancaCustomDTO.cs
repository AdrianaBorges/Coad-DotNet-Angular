
using System;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public partial class AgendaCobrancaCustomDto
    {
        public int AGC_ID { get; set; }
        public DateTime? AGC_DATA_ATENDIMENTO { get; set; }
        public DateTime? AGC_DATA_AGENDA { get; set; }
        public string AGC_HORA_AGENDA { get; set; }
        public string AGC_ASSUNTO { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string USU_LOGIN { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public int CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public int? AGC_REAGENDAMENTO { get; set; }

    }
}
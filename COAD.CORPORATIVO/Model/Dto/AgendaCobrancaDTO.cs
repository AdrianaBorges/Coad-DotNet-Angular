using System;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class AgendaCobrancaDTO
    {
        public int AGC_ID { get; set; }
        public Nullable<System.DateTime> AGC_DATA_ATENDIMENTO { get; set; }
        public Nullable<System.DateTime> AGC_DATA_AGENDA { get; set; }
        public Nullable<System.DateTime> AGC_DATA_REGISTRO { get; set; }

        public string AGC_HORA_AGENDA { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string USU_LOGIN { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public string AGC_ASSUNTO { get; set; }
        public string AGC_RESUMO { get; set; }
        public string HAT_DESCRICAO { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> AGC_REAGENDAMENTO { get; set; }
        public Nullable<bool> AGC_GERAR_AGENDAMENTO { get; set; }
        public Nullable<int> TIP_ATEND_ID { get; set; }
        public Nullable<bool> STATUS { get; set; }
        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual ParcelasDTO PARCELAS { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.CORPORATIVO.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class VW_AGENDAMENTO_COBRANCA
    {
        public string ASN_NUM_ASSINATURA { get; set; }
        public int CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public int AGC_ID { get; set; }
        public Nullable<System.DateTime> AGC_DATA_ATENDIMENTO { get; set; }
        public Nullable<System.DateTime> AGC_DATA_AGENDA { get; set; }
        public string AGC_HORA_AGENDA { get; set; }
        public string AGC_ASSUNTO { get; set; }
        public Nullable<int> AGC_REAGENDAMENTO { get; set; }
    }
}
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
    
    public partial class NOTIFICACOES
    {
        public int NTF_ID { get; set; }
        public Nullable<int> TP_NTF_ID { get; set; }
        public string NTF_DESCRICAO { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public Nullable<int> AGE_ID { get; set; }
        public string URG_NTF_ID { get; set; }
        public bool NTF_VISUALIZADO { get; set; }
        public Nullable<System.DateTime> NTF_DATA { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> REP_QUE_ENCAMINHOU { get; set; }
        public Nullable<System.DateTime> DATA_AGENDAMENTO { get; set; }
        public Nullable<bool> NTF_EXIBIDO { get; set; }
        public Nullable<int> NTF_COD_REF_INT { get; set; }
        public string NTF_COD_REF_STR { get; set; }
    
        public virtual AGENDAMENTO AGENDAMENTO { get; set; }
        public virtual CLIENTES CLIENTES { get; set; }
        public virtual REPRESENTANTE REPRESENTANTE { get; set; }
        public virtual TIPO_NOTIFICACAO TIPO_NOTIFICACAO { get; set; }
        public virtual URGENCIA_NOTIFICACAO URGENCIA_NOTIFICACAO { get; set; }
    }
}
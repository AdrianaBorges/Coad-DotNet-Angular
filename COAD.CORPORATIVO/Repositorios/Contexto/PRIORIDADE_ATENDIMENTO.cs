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
    
    public partial class PRIORIDADE_ATENDIMENTO
    {
        public int PRI_ID { get; set; }
        public Nullable<System.DateTime> PRI_DATA { get; set; }
        public string PRI_NOTA { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> TP_PRI_ID { get; set; }
        public Nullable<System.DateTime> PRI_DATA_CONFIRMACAO { get; set; }
        public Nullable<int> REP_ID_DEMANDANTE { get; set; }
        public Nullable<int> RG_ID { get; set; }
    
        public virtual CLIENTES CLIENTES { get; set; }
        public virtual REGIAO REGIAO { get; set; }
        public virtual REPRESENTANTE REPRESENTANTE { get; set; }
        public virtual REPRESENTANTE REPRESENTANTE1 { get; set; }
        public virtual TIPO_PRIORIDADE_ATENDIMENTO TIPO_PRIORIDADE_ATENDIMENTO { get; set; }
    }
}

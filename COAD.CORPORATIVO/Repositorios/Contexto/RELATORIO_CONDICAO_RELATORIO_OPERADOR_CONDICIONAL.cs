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
    
    public partial class RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL
    {
        public int REO_ID { get; set; }
        public int REC_ID { get; set; }
        public int ROC_ID { get; set; }
    
        public virtual RELATORIO_CONDICAO RELATORIO_CONDICAO { get; set; }
        public virtual RELATORIO_OPERADOR_CONDICIONAL RELATORIO_OPERADOR_CONDICIONAL { get; set; }
    }
}

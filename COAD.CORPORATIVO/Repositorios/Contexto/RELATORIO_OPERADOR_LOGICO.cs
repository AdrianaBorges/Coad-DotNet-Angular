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
    
    public partial class RELATORIO_OPERADOR_LOGICO
    {
        public RELATORIO_OPERADOR_LOGICO()
        {
            this.RELATORIO_CONDICAO = new HashSet<RELATORIO_CONDICAO>();
        }
    
        public int ROL_ID { get; set; }
        public string ROL_DESCRICAO { get; set; }
    
        public virtual ICollection<RELATORIO_CONDICAO> RELATORIO_CONDICAO { get; set; }
    }
}
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
    
    public partial class TIPO_VENDA
    {
        public TIPO_VENDA()
        {
            this.PRODUTO_COMPOSICAO = new HashSet<PRODUTO_COMPOSICAO>();
        }
    
        public int TPV_ID { get; set; }
        public string TPV_DESCRICAO { get; set; }
    
        public virtual ICollection<PRODUTO_COMPOSICAO> PRODUTO_COMPOSICAO { get; set; }
    }
}
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
    
    public partial class TIPO_PRODUTO
    {
        public TIPO_PRODUTO()
        {
            this.PRODUTOS = new HashSet<PRODUTOS>();
        }
    
        public int TIPO_PRO { get; set; }
        public string TIPO_DESCRICAO { get; set; }
    
        public virtual ICollection<PRODUTOS> PRODUTOS { get; set; }
    }
}

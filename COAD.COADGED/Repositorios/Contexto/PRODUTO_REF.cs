//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.COADGED.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class PRODUTO_REF
    {
        public PRODUTO_REF()
        {
            this.FUNCIONALIDADE = new HashSet<FUNCIONALIDADE>();
        }
    
        public int PRO_ID { get; set; }
    
        public virtual ICollection<FUNCIONALIDADE> FUNCIONALIDADE { get; set; }
    }
}

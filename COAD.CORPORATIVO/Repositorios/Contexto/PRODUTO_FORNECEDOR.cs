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
    
    public partial class PRODUTO_FORNECEDOR
    {
        public int PRO_ID { get; set; }
        public int FOR_ID { get; set; }
        public Nullable<int> PFO_ATIVO { get; set; }
    
        public virtual FORNECEDOR FORNECEDOR { get; set; }
        public virtual PRODUTOS PRODUTOS { get; set; }
    }
}
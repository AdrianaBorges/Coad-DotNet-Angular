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
    
    public partial class NOTA_FISCAL_REFERENCIADA
    {
        public int NFR_ID { get; set; }
        public string NFR_CHAVE_NOTA { get; set; }
        public Nullable<int> NLI_ID { get; set; }
        public Nullable<int> NF_ID { get; set; }
    
        public virtual NOTA_FISCAL NOTA_FISCAL { get; set; }
        public virtual NOTA_FISCAL_LOTE_ITEM NOTA_FISCAL_LOTE_ITEM { get; set; }
    }
}
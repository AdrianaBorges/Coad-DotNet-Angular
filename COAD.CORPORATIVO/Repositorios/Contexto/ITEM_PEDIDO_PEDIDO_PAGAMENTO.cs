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
    
    public partial class ITEM_PEDIDO_PEDIDO_PAGAMENTO
    {
        public int IPE_ID { get; set; }
        public int PGT_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }
    
        public virtual ITEM_PEDIDO ITEM_PEDIDO { get; set; }
        public virtual PEDIDO_PAGAMENTO PEDIDO_PAGAMENTO { get; set; }
    }
}

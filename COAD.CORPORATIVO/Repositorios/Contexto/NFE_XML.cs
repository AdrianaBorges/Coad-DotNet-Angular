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
    
    public partial class NFE_XML
    {
        public NFE_XML()
        {
            this.CONTRATOS = new HashSet<CONTRATOS>();
        }
    
        public int NFX_ID { get; set; }
        public Nullable<int> NFX_TIPO { get; set; }
        public string NFX_CHAVE_NOTA { get; set; }
        public string NFX_PATH_NOTA { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public Nullable<int> NFX_NUMERO_NOTA { get; set; }
        public Nullable<System.DateTime> NFX_DATA_EMI_NOTA { get; set; }
        public Nullable<System.DateTime> NFX_DATA_EXCLUSAO { get; set; }
        public Nullable<bool> NFX_NUM_EXTORNADO { get; set; }
    
        public virtual ICollection<CONTRATOS> CONTRATOS { get; set; }
        public virtual ITEM_PEDIDO ITEM_PEDIDO { get; set; }
    }
}
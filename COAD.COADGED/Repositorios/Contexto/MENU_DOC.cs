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
    
    public partial class MENU_DOC
    {
        public MENU_DOC()
        {
            this.MENU_DOC_ITEM = new HashSet<MENU_DOC_ITEM>();
        }
    
        public int MND_ID { get; set; }
        public string MND_DESCRICAO { get; set; }
        public Nullable<int> MAN_ID { get; set; }
    
        public virtual MANUAL_DP MANUAL_DP { get; set; }
        public virtual ICollection<MENU_DOC_ITEM> MENU_DOC_ITEM { get; set; }
    }
}

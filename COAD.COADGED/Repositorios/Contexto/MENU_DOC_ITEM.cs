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
    
    public partial class MENU_DOC_ITEM
    {
        public MENU_DOC_ITEM()
        {
            this.MENU_DOC_ITEM1 = new HashSet<MENU_DOC_ITEM>();
        }
    
        public int MNI_ID { get; set; }
        public int MND_ID { get; set; }
        public string MND_DESCRICAO { get; set; }
        public Nullable<int> MNI_ID_NODE { get; set; }
        public Nullable<int> MNI_MENU_SEQ { get; set; }
        public Nullable<int> MNI_MENU_NIVEL { get; set; }
        public Nullable<int> MNI_TIPO { get; set; }
        public Nullable<int> ITM_ATIVO { get; set; }
    
        public virtual MENU_DOC MENU_DOC { get; set; }
        public virtual ICollection<MENU_DOC_ITEM> MENU_DOC_ITEM1 { get; set; }
        public virtual MENU_DOC_ITEM MENU_DOC_ITEM2 { get; set; }
    }
}
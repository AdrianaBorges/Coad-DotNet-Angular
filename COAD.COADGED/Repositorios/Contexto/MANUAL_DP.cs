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
    
    public partial class MANUAL_DP
    {
        public MANUAL_DP()
        {
            this.MANUAL_DP_ITEM = new HashSet<MANUAL_DP_ITEM>();
            this.MENU_DOC = new HashSet<MENU_DOC>();
        }
    
        public int MAN_ID { get; set; }
        public string MAN_ASSUNTO { get; set; }
        public Nullable<System.DateTime> MAN_DATA_PUBLICACAO { get; set; }
        public Nullable<int> MOD_ID { get; set; }
        public Nullable<int> MAN_INDEX { get; set; }
    
        public virtual ICollection<MANUAL_DP_ITEM> MANUAL_DP_ITEM { get; set; }
        public virtual MANUAL_DP_MODULO MANUAL_DP_MODULO { get; set; }
        public virtual ICollection<MENU_DOC> MENU_DOC { get; set; }
    }
}
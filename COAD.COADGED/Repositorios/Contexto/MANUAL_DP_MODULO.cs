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
    
    public partial class MANUAL_DP_MODULO
    {
        public MANUAL_DP_MODULO()
        {
            this.MANUAL_DP = new HashSet<MANUAL_DP>();
        }
    
        public int MOD_ID { get; set; }
        public string MOD_DESCRICAO { get; set; }
    
        public virtual ICollection<MANUAL_DP> MANUAL_DP { get; set; }
    }
}

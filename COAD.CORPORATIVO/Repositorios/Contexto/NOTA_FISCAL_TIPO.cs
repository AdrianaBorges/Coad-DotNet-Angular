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
    
    public partial class NOTA_FISCAL_TIPO
    {
        public NOTA_FISCAL_TIPO()
        {
            this.NOTA_FISCAL = new HashSet<NOTA_FISCAL>();
        }
    
        public int NTP_ID { get; set; }
        public string NTP_DESCRICAO { get; set; }
    
        public virtual ICollection<NOTA_FISCAL> NOTA_FISCAL { get; set; }
    }
}
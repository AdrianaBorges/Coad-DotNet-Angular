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
    
    public partial class ORIGEM_CADASTRO
    {
        public ORIGEM_CADASTRO()
        {
            this.IMPORTACAO_SUSPECT = new HashSet<IMPORTACAO_SUSPECT>();
            this.INFO_MARKETING = new HashSet<INFO_MARKETING>();
        }
    
        public int O_CAD_ID { get; set; }
        public string O_CAD_DESCRICAO { get; set; }
    
        public virtual ICollection<IMPORTACAO_SUSPECT> IMPORTACAO_SUSPECT { get; set; }
        public virtual ICollection<INFO_MARKETING> INFO_MARKETING { get; set; }
    }
}

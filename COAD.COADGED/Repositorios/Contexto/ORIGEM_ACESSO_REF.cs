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
    
    public partial class ORIGEM_ACESSO_REF
    {
        public ORIGEM_ACESSO_REF()
        {
            this.LOG_ACESSO_PORTAL = new HashSet<LOG_ACESSO_PORTAL>();
            this.ORIGEM_ACESSO_MENU = new HashSet<ORIGEM_ACESSO_MENU>();
            this.ORIGEM_FUNCIONALIDADE = new HashSet<ORIGEM_FUNCIONALIDADE>();
            this.TAB_DINAMICA_ORIGEM = new HashSet<TAB_DINAMICA_ORIGEM>();
        }
    
        public int OAC_ID { get; set; }
    
        public virtual ICollection<LOG_ACESSO_PORTAL> LOG_ACESSO_PORTAL { get; set; }
        public virtual ICollection<ORIGEM_ACESSO_MENU> ORIGEM_ACESSO_MENU { get; set; }
        public virtual ICollection<ORIGEM_FUNCIONALIDADE> ORIGEM_FUNCIONALIDADE { get; set; }
        public virtual ICollection<TAB_DINAMICA_ORIGEM> TAB_DINAMICA_ORIGEM { get; set; }
    }
}
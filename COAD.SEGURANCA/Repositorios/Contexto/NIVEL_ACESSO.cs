//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.SEGURANCA.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class NIVEL_ACESSO
    {
        public NIVEL_ACESSO()
        {
            this.PERFIL = new HashSet<PERFIL>();
        }
    
        public int NIV_ACE_ID { get; set; }
        public string NIV_ACE_DESCRICAO { get; set; }
        public Nullable<int> NIV_ACE_NIVEL { get; set; }
    
        public virtual ICollection<PERFIL> PERFIL { get; set; }
    }
}

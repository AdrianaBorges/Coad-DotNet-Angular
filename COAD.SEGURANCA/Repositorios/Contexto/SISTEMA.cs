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
    
    public partial class SISTEMA
    {
        public SISTEMA()
        {
            this.ITEM_MENU = new HashSet<ITEM_MENU>();
            this.PERFIL = new HashSet<PERFIL>();
        }
    
        public string SIS_ID { get; set; }
        public string SIS_DESCRICAO { get; set; }
        public string SIS_VERSAO { get; set; }
        public string SIS_URL { get; set; }
        public string SIS_URL_PRODUCAO { get; set; }
    
        public virtual ICollection<ITEM_MENU> ITEM_MENU { get; set; }
        public virtual ICollection<PERFIL> PERFIL { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.PORTAL.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class verbetes
    {
        public verbetes()
        {
            this.tab_1026_noticias = new HashSet<tab_1026_noticias>();
            this.tab_1026_noticias1 = new HashSet<tab_1026_noticias>();
            this.verbetes1 = new HashSet<verbetes>();
        }
    
        public int id { get; set; }
        public Nullable<int> id_area { get; set; }
        public Nullable<int> verbetes_id { get; set; }
        public string nome { get; set; }
        public Nullable<System.DateTime> data_cadastro { get; set; }
    
        public virtual ICollection<tab_1026_noticias> tab_1026_noticias { get; set; }
        public virtual ICollection<tab_1026_noticias> tab_1026_noticias1 { get; set; }
        public virtual ICollection<verbetes> verbetes1 { get; set; }
        public virtual verbetes verbetes2 { get; set; }
    }
}
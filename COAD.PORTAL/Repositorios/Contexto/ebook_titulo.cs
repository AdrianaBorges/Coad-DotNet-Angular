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
    
    public partial class ebook_titulo
    {
        public int id_ebook_titulo { get; set; }
        public int id_ebook_area { get; set; }
        public string no_ebook_titulo { get; set; }
        public System.DateTime dt_ultima_atualizacao { get; set; }
        public string url_link { get; set; }
        public string url_img { get; set; }
        public Nullable<sbyte> flag_perfil_tributario { get; set; }
        public Nullable<sbyte> flag_perfil_juridico { get; set; }
        public Nullable<sbyte> flag_perfil_trabalhista { get; set; }
        public Nullable<int> flag_ambiente_producao { get; set; }
        public Nullable<int> flag_ambiente_homologacao { get; set; }
    }
}

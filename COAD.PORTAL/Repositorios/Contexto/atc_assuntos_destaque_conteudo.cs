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
    
    public partial class atc_assuntos_destaque_conteudo
    {
        public int id { get; set; }
        public Nullable<int> atc_assuntos_destaque_id { get; set; }
        public string descricao { get; set; }
        public string link { get; set; }
        public string tp_conteudo { get; set; }
        public Nullable<System.DateTime> log_dt_cadastro { get; set; }
        public Nullable<int> log_usuario_id { get; set; }
        public Nullable<int> deletado { get; set; }
    }
}

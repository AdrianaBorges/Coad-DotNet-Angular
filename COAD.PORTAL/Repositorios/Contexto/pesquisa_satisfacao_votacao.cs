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
    
    public partial class pesquisa_satisfacao_votacao
    {
        public int id { get; set; }
        public int pesquisa_satisfacao_categorias_id { get; set; }
        public int pesquisa_satisfacao_id { get; set; }
        public Nullable<int> nota { get; set; }
        public string motivo { get; set; }
    
        public virtual pesquisa_satisfacao pesquisa_satisfacao { get; set; }
        public virtual pesquisa_satisfacao_categorias pesquisa_satisfacao_categorias { get; set; }
    }
}
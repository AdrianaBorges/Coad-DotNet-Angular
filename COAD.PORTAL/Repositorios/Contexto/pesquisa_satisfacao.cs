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
    
    public partial class pesquisa_satisfacao
    {
        public pesquisa_satisfacao()
        {
            this.pesquisa_satisfacao_votacao = new HashSet<pesquisa_satisfacao_votacao>();
        }
    
        public int id { get; set; }
        public Nullable<int> cliente_id { get; set; }
        public string id_sessao { get; set; }
        public string enderecoip { get; set; }
        public int finalizado { get; set; }
        public Nullable<System.DateTime> data_cadastro { get; set; }
    
        public virtual ICollection<pesquisa_satisfacao_votacao> pesquisa_satisfacao_votacao { get; set; }
    }
}

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
    
    public partial class clientes_atendimentos
    {
        public int id { get; set; }
        public Nullable<System.DateTime> dt_inicio { get; set; }
        public Nullable<System.DateTime> dt_fim { get; set; }
        public Nullable<int> filial_id { get; set; }
        public string ramal { get; set; }
        public Nullable<int> consultor_id { get; set; }
        public Nullable<int> area_id { get; set; }
        public Nullable<int> assunto_id { get; set; }
        public Nullable<int> cliente_id { get; set; }
        public string contato_nome { get; set; }
        public string contato_telefone { get; set; }
        public Nullable<int> consulta_id { get; set; }
        public string resposta { get; set; }
        public int id_canal { get; set; }
    }
}

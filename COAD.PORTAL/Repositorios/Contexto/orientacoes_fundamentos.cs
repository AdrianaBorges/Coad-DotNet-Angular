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
    
    public partial class orientacoes_fundamentos
    {
        public int id { get; set; }
        public int id_orientacao { get; set; }
        public int id_topico { get; set; }
        public string link { get; set; }
        public int id_tipo_ato { get; set; }
        public int id_orgao_publicador { get; set; }
        public int id_publicacao { get; set; }
        public string pagina { get; set; }
        public string complemento { get; set; }
        public string data_publicacao { get; set; }
        public string numero { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.PORTAL.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class noticias_busca
    {
        public int id_grupo { get; set; }
        public int id_noticia { get; set; }
        public string verbete { get; set; }
        public int id_prod { get; set; }
        public int id_tipo { get; set; }
        public string texto { get; set; }
        public System.DateTime data_cadastro { get; set; }
        public string publicar { get; set; }
        public string texto_integra { get; set; }
        public string verbete_integra { get; set; }
        public string descricao { get; set; }
    }
}

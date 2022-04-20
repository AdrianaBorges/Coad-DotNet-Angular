using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.PortalCoad
{
    [Mapping(Source = typeof(noticias), confRef = "portalCoad")]
    public class NoticiasPortalDTO
    {
        public int id { get; set; }
        public int id_prod { get; set; }
        public int id_tipo { get; set; }
        public string texto { get; set; }
        public string nota { get; set; }
        public string destaque { get; set; }
        public string newsletter { get; set; }
        public string link_file { get; set; }
        public System.DateTime data_cadastro { get; set; }
        public string destaque_home { get; set; }
        public string destaque_perfil { get; set; }
        public bool destaque_fixo_home { get; set; }
        public bool destaque_fixo_perfil { get; set; }
        public string publicar { get; set; }
    }
}

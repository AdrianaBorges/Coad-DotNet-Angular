using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.PortalCoad
{
    [Mapping(Source = typeof(noticias_busca), confRef = "portalCoad")]
    public class NoticiasPortalBuscaDTO
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

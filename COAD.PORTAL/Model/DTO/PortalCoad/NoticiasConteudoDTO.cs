using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.PortalCoad
{
    [Mapping(Source = typeof(noticias_conteudo), confRef = "portalCoad")]
    public class NoticiasConteudoDTO
    {
        public int id_grupo { get; set; }
        public int id_noticia { get; set; }
        public string verbete { get; set; }
        public string subverbete { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.PortalCoad
{
    [Mapping(Source = typeof(noticias_grupos), confRef = "portalCoad")]
    public class NoticiasGrupoDTO
    {
        public int id { get; set; }
        public int id_tipo { get; set; }
        public string descricao { get; set; }
    }
}

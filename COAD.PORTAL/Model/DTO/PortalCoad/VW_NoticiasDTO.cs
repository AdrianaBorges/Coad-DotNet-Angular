using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.PortalCoad
{
    [Mapping(Source = typeof(VW_BuscarNoticias), confRef = "portalCoad")]
    public class VW_NoticiasDTO
    {
        public int nmid { get; set; }
        public int nmid_prod { get; set; }
        public int nmid_tipo { get; set; }
        public string nmtexto { get; set; }
        public string nmnota { get; set; }
        public string nmdestaque { get; set; }
        public string nmnewsletter { get; set; }
        public string nmlink_file { get; set; }
        public System.DateTime nmdata_cadastro { get; set; }
        public string nmdestaque_home { get; set; }
        public string nmdestaque_perfil { get; set; }
        public bool nmdestaque_fixo_home { get; set; }
        public bool nmdestaque_fixo_perfil { get; set; }
        public int ncid_grupo { get; set; }
        public int ncid_noticia { get; set; }
        public string ncverbete { get; set; }
        public string ncsubverbete { get; set; }
        public int ngid { get; set; }
        public int ngid_tipo { get; set; }
        public string ngdescricao { get; set; }
    }
}

using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTO.PortalBusca
{
    [Mapping(Source = typeof(busca_completa_tributario), confRef = "portalBusca")]
    public class Busca_completa_tributarioDTO
    {
        public Nullable<int> id { get; set; }
        public int id_conteudo { get; set; }
        public int id_tipo_conteudo { get; set; }
        public Nullable<int> id_perfil { get; set; }
        public Nullable<System.DateTime> data_conteudo { get; set; }
        public string palavras_indexadas { get; set; }
        public Nullable<int> idGED { get; set; }
    }
}

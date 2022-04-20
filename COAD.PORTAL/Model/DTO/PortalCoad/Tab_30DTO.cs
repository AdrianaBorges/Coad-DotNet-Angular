using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTO.PortalCoad
{
    [Mapping(Source = typeof(tab_30), confRef = "portalCoad")]
    public class Tab_30DTO
    {
        public Nullable<int> id { get; set; }
        public string colec { get; set; }
        public string gg { get; set; }
        public string vb { get; set; }
        public string svb { get; set; }
        public string complemento { get; set; }
        public string tipo_materia { get; set; }
        public string expressao_ato { get; set; }
        public string num { get; set; }
        public string org { get; set; }
        public Nullable<int> pagina { get; set; }
        public string informativo { get; set; }
        public string ano { get; set; }
        public string modulo { get; set; }
        public string titulo { get; set; }
        public System.DateTime dataCadastro { get; set; }
        public Nullable<int> idGED { get; set; }
        public string GED_revoga_altera { get; set; }
    }
}

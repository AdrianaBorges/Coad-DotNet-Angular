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
    [Mapping(Source = typeof(tab_31_html), confRef = "portalCoad")]
    public class Tab_31_htmlDTO
    {
        public Nullable<int> id { get; set; }

        [MaxLength(255, ErrorMessage = "Por favor, informe no máximo 255 caracteres!")]
        public string colec { get; set; }

        public int modulo { get; set; }
        public string html { get; set; }
        public int bug { get; set; }
        public Nullable<int> idGED { get; set; }
    }
}

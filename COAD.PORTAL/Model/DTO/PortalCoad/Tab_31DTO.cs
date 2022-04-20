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
    [Mapping(Source = typeof(tab_31), confRef = "portalCoad")]
    public class Tab_31DTO
    {
        public Nullable<int> id { get; set; }

        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string colec { get; set; }

        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string gg { get; set; }

        [MaxLength(250, ErrorMessage = "Por favor, informe no máximo 250 caracteres!")]
        public string vb { get; set; }

        [MaxLength(250, ErrorMessage = "Por favor, informe no máximo 250 caracteres!")]
        public string svb { get; set; }

        public string complemento { get; set; }

        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string tipo_materia { get; set; }

        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string expressao_ato { get; set; }

        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string num { get; set; }

        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string org { get; set; }
        
        public Nullable<int> pagina { get; set; }
        public Nullable<int> informativo { get; set; }

        [MaxLength(10, ErrorMessage = "Por favor, informe no máximo 10 caracteres!")]
        public string ano { get; set; }

        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string modulo { get; set; }
        
        public string titulo { get; set; }
        public System.DateTime dataCadastro { get; set; }

        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string LABEL { get; set; }
        public string Destino { get; set; }
        public string Revisar { get; set; }
        public Nullable<System.DateTime> datadoato { get; set; }
        public Nullable<System.DateTime> publicacao { get; set; }

        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string diario { get; set; }

        [MaxLength(250, ErrorMessage = "Por favor, informe no máximo 250 caracteres!")]
        public string ementa { get; set; }
        public string status_colec { get; set; }
        public string status_vb { get; set; }
        public string newsletter { get; set; }
        public Nullable<int> secao { get; set; }

        [MaxLength(250, ErrorMessage = "Por favor, informe no máximo 250 caracteres!")]
        public string desc_news { get; set; }
        public Nullable<int> classiBuscaAvancada { get; set; }
        public Nullable<int> idGED { get; set; }
        public string GED_revoga_altera { get; set; }
    }
}

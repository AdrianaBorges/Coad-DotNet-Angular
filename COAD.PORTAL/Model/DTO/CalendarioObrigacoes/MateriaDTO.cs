using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTO.CalendarioObrigacoes
{
    public class MateriaDTO
    {
        public Nullable<int> id { get; set; }
        public string titulo { get; set; }
        public Nullable<System.DateTime> dataCadastro { get; set; }
        public string modulo { get; set; }
        public string colec { get; set; }
        public string ano { get; set; }
        public string num { get; set; }
        public Nullable<int> informativo { get; set; }
        public Nullable<int> pagina { get; set; }
        public string gg { get; set; }
        public string vb { get; set; }
        public string svb { get; set; }
        public string complemento { get; set; }
        public string tipo_materia { get; set; }
        public string expressao_ato { get; set; }
        public string org { get; set; }
        public string html { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTO.CalendarioObrigacoes
{
    public class MateriasDTO
    {
        public Nullable<int> id { get; set; }
        public string tipo { get; set; }
        public string numero { get; set; }
        public string ano { get; set; }
        public string orgao { get; set; }
        public string area { get; set; }
        public string assunto { get; set; }
        public string subGrupo { get; set; }
        public Nullable<int> informativo { get; set; }
        public Nullable<int> pagina { get; set; }
        public string grande_grupo { get; set; }
    }
}

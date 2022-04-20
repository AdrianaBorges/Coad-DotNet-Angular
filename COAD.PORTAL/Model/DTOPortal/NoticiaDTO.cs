using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTOPortal
{
    public class NoticiaDTO
    {
        public int id { get; set; }
        public int id_prod { get; set; }
        public int id_tipo { get; set; }
        public string texto { get; set; }
        public string nota { get; set; }
        public string destaque { get; set; }
        public string newsletter { get; set; }
        public string link_file { get; set; }
        public System.DateTime data_cadastro { get; set; }
        public string destaque_home { get; set; }
        public string destaque_perfil { get; set; }
        public bool destaque_fixo_home { get; set; }
        public bool destaque_fixo_perfil { get; set; }
    }
}

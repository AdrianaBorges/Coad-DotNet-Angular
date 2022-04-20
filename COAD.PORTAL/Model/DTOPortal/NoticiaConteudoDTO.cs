using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTOPortal
{
    public class NoticiaConteudoDTO
    {
        public int id_grupo { get; set; }
        public int id_noticia { get; set; }
        public string verbete { get; set; }
        public string subverbete { get; set; }
    }
}

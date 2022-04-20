using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    public partial class ParametroGrupoDTO
    {
        public ParametroGrupoDTO()
        {
            this.PARAMETROS = new HashSet<ParametrosDTO>();
        }

        public int PGR_ID { get; set; } 
        public string PGR_DESCRICAO { get; set; }

        public virtual HashSet<ParametrosDTO> PARAMETROS { get; set; }
    }
}

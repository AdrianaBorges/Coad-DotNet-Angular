using COAD.SEGURANCA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class RepresentanteUsuarioDTO
    {
        public RepresentanteDTO REPRESENTANTE { get; set; }
        public UsuarioModel USUARIO { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce
{
    public class UsuarioAutenticadoDTO : ClaimsIdentity
    {
        public string Login { get; set; }

        public int? CliId { get; set; }
        public bool EhUsuario { get; set; }
        public ClienteDto Cliente { get; set; }

        public override string Name => Login;

        public override string AuthenticationType => "Basic";

        public override bool IsAuthenticated => true;
    }
}

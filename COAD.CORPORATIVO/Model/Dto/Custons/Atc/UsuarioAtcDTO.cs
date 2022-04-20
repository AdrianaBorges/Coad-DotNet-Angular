using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Atc
{
    class UsuarioAtcDTO : ClaimsIdentity
    {
        public string Login { get; set; }

        public string Assinatura { get; set; }

        public bool EhUsuario { get; set; }

        public ContratoDTO ContratoDTO { get; set; }

        public override string Name => Login;

        public override string AuthenticationType => "Basic";

        public override bool IsAuthenticated => true;

    }
}

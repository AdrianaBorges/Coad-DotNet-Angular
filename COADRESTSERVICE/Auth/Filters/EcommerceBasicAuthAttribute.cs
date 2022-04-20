using COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce;
using COAD.CORPORATIVO.Service;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace COADRESTSERVICE.Auth.Filters
{
    public class EcommerceBasicAuthAttribute : IdentityBasicAuthenticationAttribute
    {
        public EcommerceBasicAuthAttribute()
        {

        }

        public EcommerceBasicAuthAttribute(bool Return401ForUnauthorized) : base(Return401ForUnauthorized)
        {
        }

        public override UsuarioAutenticadoDTO LogarCliente(string userName, string hashPassword)
        {
            var usuarioClienteService = ServiceFactory.RetornarServico<ClienteUsuarioSRV>();
            var user = usuarioClienteService.LogarCliente(userName, hashPassword);

            if (user != null)
            {
                var result = new UsuarioAutenticadoDTO
                {
                    Login = user.USC_LOGIN,
                    CliId = user.CLI_ID
                };
                return result;
            }
            return null;

        }
    }
}

using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce;
using COAD.CORPORATIVO.Service;
using GenericCrud.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace COADRESTSERVICE.Auth
{
    public class UserContext
    {
        private IHttpContextAccessor HttpContextAcessor { get; set; }
        private ClienteSRV ClienteSRV { get; set; }
        
        public UserContext(
            IHttpContextAccessor httpContextAcessor,
            ClienteSRV clienteSRV
            )
        {
            this.HttpContextAcessor = httpContextAcessor;
            this.ClienteSRV = clienteSRV;
        }

        public UsuarioAutenticadoDTO RetornarUsuario(ClaimsPrincipal claimsPrincipal)
        {
            if(claimsPrincipal != null && 
                claimsPrincipal.Identity != null && 
                claimsPrincipal.Identity.IsAuthenticated &&
                claimsPrincipal.Identity is UsuarioAutenticadoDTO)
            {
                var usuarioAutenticado = claimsPrincipal.Identity as UsuarioAutenticadoDTO;

                if(usuarioAutenticado != null && 
                    usuarioAutenticado.CliId != null)
                {
                    usuarioAutenticado.Cliente = ClienteSRV.FindById(usuarioAutenticado.CliId);
                }
                return usuarioAutenticado;
            }

            return null;
        }

        public UsuarioAutenticadoDTO Usuario {
            get {

                if(HttpContextAcessor != null && HttpContextAcessor.HttpContext != null)
                {
                    var usuario = HttpContextAcessor.HttpContext.User;
                    return RetornarUsuario(usuario);
                }

                return null;
            }
        }
    }
}

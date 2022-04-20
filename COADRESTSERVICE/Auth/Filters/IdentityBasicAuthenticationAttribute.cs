using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Service.Custons;
using GenericCrud.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using COAD.SEGURANCA.Model.Custons;
using System.Security.Claims;
using COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce;

namespace COADRESTSERVICE.Auth.Filters
{
    public abstract class IdentityBasicAuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        public IdentityBasicAuthenticationAttribute(bool Return401ForUnauthorized)
        {
            this.Return401ForUnauthorized = Return401ForUnauthorized;
        }

        public IdentityBasicAuthenticationAttribute()
        {
            this.Return401ForUnauthorized = true;
        }

        public bool Return401ForUnauthorized { get; set; } = true;
        public void OnAuthorization(AuthorizationFilterContext context)
        { 
            var authHeader = context.HttpContext.Request.Headers["Authorization"];

            if(!string.IsNullOrEmpty(authHeader))
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                if(authHeaderVal.Scheme == "Basic")
                {
                    var usuario = Login(authHeaderVal.Parameter);
                    
                    if (usuario != null)
                    {
                        context.HttpContext.User = new ClaimsPrincipal();
                        context.HttpContext.User.AddIdentity(usuario);
                        return;
                    }

                }
            }

            if(Return401ForUnauthorized)
                context.Result = new UnauthorizedResult();
        }

        public UsuarioAutenticadoDTO Login(string basicToken)
        {
            if (!string.IsNullOrWhiteSpace(basicToken))
            {
                var usernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(basicToken));

                if (!string.IsNullOrWhiteSpace(usernamePassword))
                {
                    var usernamePasswordArray = usernamePassword.Split(':');
                    if (usernamePasswordArray != null && usernamePasswordArray.Count() >= 2)
                    {
                        var userName = usernamePasswordArray[0];
                        var password = usernamePasswordArray[1];

                        var user = LogarCliente(userName, password);
                        return user;

                    }
                }
            }
            return null;
        }

        public abstract UsuarioAutenticadoDTO LogarCliente(string userName, string hashPassword);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Coad.GenericCrud.Security
{
    public class SecureAttribute : AuthorizeAttribute
    {
        public bool IsAjax { get; set; }

        /// <summary>
        /// Sempre que não estiver logado. O usuário sera redirecionado diretamente para a página de login
        /// </summary>
        public bool AlwaysRedirectToLogin { get; set; }
        protected string ErroMsg { get; set; }
        protected bool logado = false;


        public SecureAttribute()
        {
            if (logado)
            {
                ErroMsg = @"Seu usuário não possue permissão para acessar esta funcionalidade.";
            }
            else
            {
                ErroMsg = @"Erro de autenticação: Sua sessão expirou. Abra uma nova aba e logue novamente.";
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            
          var isAuthorized = base.AuthorizeCore(httpContext);

          if (!isAuthorized)
              return false;

          return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (IsAjax)
            {
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("area", "");
                redirectTargetDictionary.Add("action", "Error");
                redirectTargetDictionary.Add("controller", "Home");                
                filterContext.Controller.TempData["message"] = ErroMsg;
                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }
            else
            {
                if (logado && AlwaysRedirectToLogin == false)
                {
                    RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                    redirectTargetDictionary.Add("area", "");
                    redirectTargetDictionary.Add("action", "Info");
                    redirectTargetDictionary.Add("controller", "Home");
                    filterContext.Controller.TempData["message"] = ErroMsg;
                    filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
                }
                else
                {
                    filterContext.Controller.TempData["message"] = ErroMsg;
                    base.HandleUnauthorizedRequest(filterContext);
                }

               
            }
            
        }
    }
}
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using COAD.SEGURANCA.Repositorios.Base;
using COADMOBILE.Config;
using GenericCrud.Exceptions.ErrorHandling;
using GenericCrud.ActionResultTools;

namespace COADMOBILE
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new ExceptionFilterAttribute(), 6);
            GlobalFilters.Filters.Add(new OpenDbContextInViewAttribute(), 1);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            CoadMobileConfig.Configurar();
            //Bloco retirado para renovação da aplicação
            /*
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            CoadProxyConfig.Configurar();
            */
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            SysException.RegistrarLog(Server.GetLastError().Message, "", SessionContext.autenticado);
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        protected void Session_End(object sender, EventArgs e)
        {

        }
        public void Application_BeginRequest(object sender, EventArgs e)
        {


        }

        protected void Application_End()
        {
            CoadMobileConfig.FecharContainer();
        }
    }
}
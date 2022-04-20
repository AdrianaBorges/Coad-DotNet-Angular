using COAD.CORPORATIVO.Config;
using COAD.SEGURANCA.Repositorios.Base;
using COADPAG.Config;
using GenericCrud.Exceptions.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace COADPAG
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new ExceptionFilterAttribute(), 6);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            CoadPagConfig.Configurar();
            
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
    }
}
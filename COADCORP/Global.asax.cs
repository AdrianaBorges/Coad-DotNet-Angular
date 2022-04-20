using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.UTIL.Helpers;
using COADCORP.Config;
using GenericCrud.ActionResultTools;
using GenericCrud.Exceptions.ErrorHandling;
using GenericCrud.Util;

namespace COADCORP
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //private static CoadCorpLog logHelper = new CoadCorpLog(typeof(MvcApplication));

        protected void Application_Start()
        {
            //CoadCorpLog.Initialize();

            //logHelper.Info("Start");

            SysUtils.DefaultPath = Server.MapPath("/");
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new ExceptionFilterAttribute(), 6);
            GlobalFilters.Filters.Add(new OpenDbContextInViewAttribute(), 1);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            CoadCorpConfig.Configurar();

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

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
            CoadCorpConfig.FecharContainer();
            //logHelper.Info("End");

        }

    }
}
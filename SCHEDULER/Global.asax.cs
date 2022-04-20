using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using COAD.SEGURANCA.Config;
using SCHEDULER.Config;
using MundiPaggClientSDK.Configuration;
using GenericCrud.Util;

namespace SCHEDULER
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            
            //-- Chamada do mentodo de configuração AutoMapper (ConfigSis);

           SysUtils.DefaultPath = Server.MapPath("/");
           SchedulerConfig.Configurar();
        }
    }
}
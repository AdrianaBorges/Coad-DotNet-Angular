using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace COADCORE
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           
            routes.MapRoute(name: "Default",
                             url: "{controller}/{action}/{id}",
                             defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
            
            //routes.MapRoute(name: "Simuladores",
            //                 url: "{controller}/{action}/{id}/{interno}",
            //                 defaults: new
            //                 {
            //                     controller = "TabelaDinamica",
            //                     action = "Simulador",
            //                     id = UrlParameter.Optional,
            //                     interno = UrlParameter.Optional
            //                 }
            //);

            //routes.MapRoute(name: "Pagamento",
            //                 url: "{controller}/{action}/{id}/{interno}",
            //                 defaults: new
            //                 {
            //                     controller = "Checkout",
            //                     action = "Pagamento",
            //                     id = UrlParameter.Optional,
            //                     interno = UrlParameter.Optional
            //                 }
            //  );

            //routes.MapRoute(name: "Tabelas",
            //                 url: "{controller}/{action}/{id}/{interno}",
            //                 defaults: new
            //                 {
            //                     controller = "TabelaDinamica",
            //                     action = "Tabela",
            //                     id = UrlParameter.Optional,
            //                     interno = UrlParameter.Optional
            //                 }
            //);
            
            //routes.MapRoute(name: "SimuladorICMS",
            //                 url: "{controller}/{action}/{id}",
            //                 defaults: new
            //                 {
            //                     controller = "TabelaDinamica",
            //                     action = "SimuladorICMS",
            //                     id = UrlParameter.Optional
            //                 }
            //);

            //routes.MapRoute(name: "Referencia",
            //                 url: "{controller}/{action}/{id}",
            //                 defaults: new
            //                 {
            //                     controller = "Manualdp",
            //                     action = "Referencia",
            //                     id = UrlParameter.Optional
            //                 }
            //);

            // Ignorando por causa de exceptions lançados pelo IOCContainer
            //routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
        }
    }
}
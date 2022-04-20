using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace COADPAG
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(name: "Default",
                       url: "{controller}/{action}/{ipeid}/{ipehash}",
                       defaults: new
                       {
                           controller = "Checkout",
                           action = "Pagamento",
                           ipeid = UrlParameter.Optional,
                           ipehash = UrlParameter.Optional,
                       }
            );



            // Ignorando por causa de exceptions lançados pelo IOCContainer
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
        }
    }
}
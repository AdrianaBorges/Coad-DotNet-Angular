using System.Web.Mvc;

namespace COADCORP.Areas.franquia
{
    public class franquiaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "franquia";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "franquia_home",
               "Home/",
               new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "default" }
           );
            context.MapRoute(
                "franquia_default",
                "franquia/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            
        }
    }
}

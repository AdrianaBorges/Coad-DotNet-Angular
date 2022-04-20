using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Filter;

namespace COADMOBILE.Controllers
{
    public class HomeController : Controller
    {
        [Autorizar(PorMenu = false, IsAjax = false)]
        public ActionResult Index()
        {
            try
            {
                //ClientesSRV clientesrv = new ClientesSRV();
                //var cliente = clientesrv.LoginCliente("jpereira","1qaz2wsx");
                //string _retorno = new UsuarioSRV().RealizarLogin("MOBILEUSER", "123456", "COADMOBILE", System.Web.HttpContext.Current);

                //if (_retorno.Trim() != "")
                //TempData.Add("Resultado", _retorno);

                return Redirect("Tributario/Index");
            }
            catch (Exception ex)
            {
                TempData.Add("Resultado", ex.Message);

                return View();
            }
        }

        [Autorizar(PorMenu = false, IsAjax = false)]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        //[Autorizar(PorMenu = false, IsAjax = false)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Info()
        {
            //return View();
            return Redirect("~/Login/LimparCache");
        }


    }
}

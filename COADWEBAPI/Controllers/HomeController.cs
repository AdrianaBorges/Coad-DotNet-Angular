using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coad.GenericCrud.Service.Base;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;

namespace COADWEBAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (SessionContext.autenticado != null)
            {
                string _retorno = new UsuarioSRV().RealizarLogin("MOBILEUSER", "123456", "COADMOBILE", System.Web.HttpContext.Current);

                if (_retorno.Trim() != "")
                    TempData.Add("Resultado", _retorno);
            }
            else
            {
                //string _retorno = new UsuarioSRV().RealizarLogin("MOBILEUSER", "123456", "COADMOBILE", System.Web.HttpContext.Current);
                //if (_retorno.Trim() != "")
                    //TempData.Add("Resultado", _retorno);
            }

            return View();
        }
    }
}

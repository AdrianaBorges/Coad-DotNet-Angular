using System;
using System.Web;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace CONFIGSIS.Controllers
{
    public class SairController : Controller
    {
        //
        // GET: /Sair/

        //public ActionResult Index()
        //{
        //    SysException.RegistrarLog("LogOff Usuário (" + SessionContext.autenticado.USU_LOGIN + ")", "", SessionContext.autenticado);

        //    Response.Expires = 0;
        //    Response.ExpiresAbsolute = DateTime.Now;
        //    Response.AddHeader("pragma","no-cache");
        //    Response.AddHeader("cache-control","private");
        //    Response.CacheControl = "no-cache";
        //    Session.Contents.RemoveAll();
        //    Session.Abandon();

        //    return RedirectToAction("Login", "Login");
        //}

        public ActionResult Index()
        {
            if (SessionContext.autenticado != null)
            {
                SysException.RegistrarLog("LogOff Usuário (" + SessionContext.autenticado.USU_LOGIN + ")", "", SessionContext.autenticado);
            }

            SessionContext.RemoveSession(System.Web.HttpContext.Current);
            return RedirectToAction("Login", "Login");
        }
    }
}

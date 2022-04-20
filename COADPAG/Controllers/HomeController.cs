using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.GATEWAY.MUNDIPAGG.Model;
using COAD.GATEWAY.MUNDIPAGG.Service;
using COAD.SEGURANCA.Repositorios.Base;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADPAG.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
        {
            JSONResponse response = new JSONResponse()
            {
                message = Message.Fail(TempData["message"] as string)
            };
            response.success = false;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Info(string type)
        {
            if (type != "" && type != null)
                TempData["message"] = type;

            return View();
        }
        public ActionResult Falha(string type)
        {
            if (type != "" && type != null)
                TempData["message"] = type;

            return View();
        }
        public ActionResult Erro(string type)
        {
            return View();
        }

    }
}

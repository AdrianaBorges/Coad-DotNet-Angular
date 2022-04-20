using Coad.GenericCrud.ActionResultTools;
using COAD.COADGED.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADPORTAL.Controllers
{
    public class PortalController : Controller
    {
        //
        // GET: /Portal/
        //[Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            ViewBag.id = 8;

            return View();
        }
        //[Autorizar(IsAjax = true)]
        public ActionResult BuscarWidgets(int _origem)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var lstwidgetsd = new OrigemFuncionalidadeSRV().ListarFuncionalidades(_origem,"LDI");
                var lstwidgetse = new OrigemFuncionalidadeSRV().ListarFuncionalidades(_origem,"LES");
                var lstwidgetsc = new OrigemFuncionalidadeSRV().ListarFuncionalidades(_origem,"CPA");
                response.Add("lstwidgetsd", lstwidgetsd);
                response.Add("lstwidgetse", lstwidgetse);
                response.Add("lstwidgetsc", lstwidgetsc);
                response = this.CarregarWidgets(response);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {

            SessionContext.RemoveSessionGlobal(System.Web.HttpContext.Current, SessionContext.autenticado.SESSION_ID);

            SessionContext.RemoveSession(System.Web.HttpContext.Current);

            return RedirectToAction("Index", "Login");
            
        }
        public JSONResponse CarregarWidgets(JSONResponse response)
        {

            try
            {
                DateTime dataini = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                DateTime datafim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                var lstAcessoTabelas = new LogSimuladorSRV().BuscarTabelasPorGrupo(dataini, datafim, 0, null, 10);
                var lstAcessoSimulador = new LogSimuladorSRV().BuscarSimuladorPorGrupo(dataini, datafim, 0, null, 10);

                response.Add("lstAcessoTabelas", lstAcessoTabelas);
                response.Add("lstAcessoSimulador", lstAcessoTabelas);

                return response;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
  

    }
}

using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Util;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Areas.franquia.Controllers
{
    public class UENController : Controller
    {
        //
        // GET: /franquia/Fila/

        private UENSRV _service = new UENSRV();

        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarUENs()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstUEN = _service.FindAll();
                response.Add("lstUEN", lstUEN);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarUEN(int? uenId)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var uen = _service.FindById(uenId);
                response.Add("uen", uen);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }


        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult TrocarUen(int? UEN_ID)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                SessionUtil.SetUenId(UEN_ID);
                var uen = SessionUtil.GetUen();
                result.Add("uen", uen);

                return Json(result);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true)]
        public ActionResult RetornarUenAtual()
        {
            JSONResponse result = new JSONResponse();

            try
            {
                var uen = SessionUtil.GetUen();
                result.Add("uen", uen);
                return Json(result);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

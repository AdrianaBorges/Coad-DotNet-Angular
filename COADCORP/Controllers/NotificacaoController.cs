using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Dto;
using COAD.SEGURANCA.Model.Dto.Custons.Pesquisas;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class NotificacaoController : Controller
    {
        public FilaEmailSRV _service { get; set; }
        public NotificacaoSistemaSRV _notificacaoService { get; set; }
        //
        // GET: /Templates/
        
        public ActionResult Index()
        {
            return View();
        }
        
        [ActionName("cancelar-email")]
        public JsonResult CancelarEmail(string fleId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                _notificacaoService.CancelarEnvioDeNotificacoes(fleId, "E-Mail");               

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        [ActionName("pausar-erro-email")]
        public JsonResult PausarErroEmail(string fleId, int? qtdDias)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                _notificacaoService.PausarErroNotificacao(fleId, qtdDias, "E-Mail");

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}

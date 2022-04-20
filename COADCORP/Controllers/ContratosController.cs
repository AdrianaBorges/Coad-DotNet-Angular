using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class ContratosController : Controller
    {
        //
        // GET: /Contratos/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Cancelamento(ContratoDTO _ctr)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var _contrato = ServiceFactory.RetornarServico<ContratoSRV>();
                var _histcoanc = _ctr.HISTORICO_CANCELAMENTO;
                _ctr = _contrato.FindById(_ctr.CTR_NUM_CONTRATO);

                _ctr.HISTORICO_CANCELAMENTO = _histcoanc;

                _contrato.CancelarContrato(_ctr);

                result.message = Message.Info("Cancelamento realizado com sucesso");
                result.success = true;

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

    }
}

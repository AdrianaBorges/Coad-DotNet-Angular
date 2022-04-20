using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Plugins
{
    public class ComprovantesController : Controller
    {
        public PropostaItemComprovanteSRV _propostaItemComprovanteSRV { get; set; }
        //
        // GET: /Comprovantes/

        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ListarPropostaItemComprovante(int? ppiId = null, int? ipeId = null)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var lstItemPropostaPedidoComprovante = _propostaItemComprovanteSRV.ListarPropostaItemComprovante(ppiId, ipeId);
                result.Add("lstItemPropostaPedidoComprovante", lstItemPropostaPedidoComprovante);

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult SalvarPropostaItemComprovanteProposta(PropostaItemDTO propostaItem)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = SessionContext.login;
                    var repId = SessionContext.GetIdRepresentante();
                    _propostaItemComprovanteSRV.SalvarPropostaItemComprovanteComTransacao(propostaItem, usuario, repId);
                    SysException.RegistrarLog("Dados do comprovante atualizados com sucesso!!", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do comprovante atualizados com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult SalvarPropostaItemComprovantePedido(ItemPedidoDTO propostaItem)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = SessionContext.login;
                    var repId = SessionContext.GetIdRepresentante();
                    _propostaItemComprovanteSRV.SalvarPropostaItemComprovanteComTransacao(propostaItem, usuario, repId);
                    SysException.RegistrarLog("Dados do comprovante atualizados com sucesso!!", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do comprovante atualizados com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
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

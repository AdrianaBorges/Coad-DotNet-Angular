using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace COADCORP.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class NotificacoesController : Controller
    {
        public NotificacoesSRV _service { get; set; }
        public TipoNotificacaoSRV _tipoNotSRV { get; set; }
        public UrgenciaNotificacaoSRV _urgenciaSRV { get; set; }
        //
        // GET: /Notificacoes/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContadorTimeOut()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                
                SessionContext.ContadorTimeOut(System.Web.HttpContext.Current);
                result.success = true;
                result.message = Message.Info("Operação realizada com sucesso!!");
 
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
        public ActionResult ListarUsuariosLogados()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var lstusuariologado = SessionContext.autenticadoGlobal;
                result.Add("lstusuariologado", lstusuariologado);
                result.message = Message.Fail("Consulta realizada com sucesso!!");
                result.success = true;
 
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
        public ActionResult ListarQtdNotificacoesNaoLidas()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {                    
                    var qtdNotificacoes = _service.ChecaQuantidadeNotificacoesNaoLidas((int) REP_ID);
                    result.Add("qtdNotificacoes", qtdNotificacoes);
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }     

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
        public ActionResult ChecaResumoNotificacacoesNaoLidas()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var lstNotificacoes = _service.ResumoDeNotificacoes((int)REP_ID);
                    result.AddPage("lstResumoNotificacoes", lstNotificacoes);
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
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
        public ActionResult ListarNotificacoes(bool? lidas = null, 
            int? tipoNotificacaoId = null, string urgenciaNotificacaoId = null, int pagina = 1)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var lstNotificacoes = _service.Notificacoes((int) REP_ID, lidas, tipoNotificacaoId, urgenciaNotificacaoId, pagina, 7);
                    result.AddPage("lstNotificacoes", lstNotificacoes);
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult MostrarTudo()
        {
            try
            {
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    ViewBag.lstTipoNotificacao = _tipoNotSRV.FindAll();
                    ViewBag.lstUrgenciaNotificacao = _urgenciaSRV.FindAll();
                }
                else
                {
                    TempData["message"] = "O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.";
                    return RedirectToAction("falha", "home");
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
            }

            return View();

        }
        [Autorizar(IsAjax = true)]
        public ActionResult LerEMarcarNotificacaoComoLida(int NTF_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var notificacao = _service.LerEMarcarNotificacaoComoLida(NTF_ID);
                    result.Add("notify", notificacao);
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
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
        public ActionResult ListarNotificacoesNaoExibidas()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var lstNotificacoesPopup = _service.ListarNotificacoesNaoExibidas(REP_ID);
                    result.Add("lstNotificacoesPopup", lstNotificacoesPopup);
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
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
        public ActionResult MarcarTodasAsNotificacoesComoLidas(int? REP_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    _service.MarcarTodasAsNotificacoesComoLidas(REP_ID);
                    
                }
                else
                {
                    result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    result.success = false;
                }
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
        public ActionResult MarcarNotificacaoComoExibida(int? NTF_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.MarcarNotificacaoComoExibida(NTF_ID);                
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}

using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
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
    public class BackgroundController : Controller
    {
        public NotificacoesSRV _service { get; set; }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarQtdNotificacoesNaoLidas()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var qtdNotificacoes = _service.ChecaQuantidadeNotificacoesNaoLidas((int)REP_ID);
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
                //SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        ///// <summary>
        ///// Checa o SessionUtil com o método específico para validar autenticação
        ///// </summary>
        ///// <param name="methodName"></param>
        ///// <returns></returns>
        //public JsonResult ValidarPermissao(string methodName)
        //{
        //    JSONResponse response = new JSONResponse();
        //    try
        //    {
        //        var possuiPermissao = SessionUtil.ValidarPermissaPorNomeMetodo(methodName);
        //        response.Add("possuiPermissao", possuiPermissao);

        //    }
        //    catch (Exception e)
        //    {
        //        response.message = Message.Fail(e);
        //        response.success = false;
        //    }

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
    }
}

using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Filters;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    
    public class RegistroLiberacaoController : Controller
    {
        public RegistroLiberacaoSRV _service { get; set; }
        public RegistroLiberacaoItemSRV _registroLiberacaoItemSRV { get; set; }
        //
        // GET: /Templates/

        [AutorizarCustom(PorMenu = false,SessionUtilMethodName = "PossuiGerenciaVenda")]
        public ActionResult Index()
        {
            return View();
        }
        
        [AutorizarCustom(IsAjax = true, SessionUtilMethodName = "PossuiGerenciaVenda")]
        public JsonResult PesquisarRegistrosLiberacao(PesquisaRegistroLiberacaoDTO pesquisaDTO)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstRegistroLiberacao = _service.PesquisarRegistrosLiberacao(pesquisaDTO);
                response.AddPage("lstRegistroLiberacao", lstRegistroLiberacao);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AutorizarCustom(IsAjax = true, SessionUtilMethodName = "PossuiGerenciaVenda")]
        public JsonResult ListarRegistroLiberacaoItemAtivo(int? rliId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstRegistroLiberacaoItems = _registroLiberacaoItemSRV.ListarRegistroItemPorRegistroAtivo(rliId);
                response.Add("lstRegistroLiberacaoItems", lstRegistroLiberacaoItems);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        [AutorizarCustom(IsAjax = true, SessionUtilMethodName = "PossuiGerenciaVenda")]
        [HttpPost]
        public ActionResult AprovarPendencia(AlteracaoStatusDTO status)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;
                _registroLiberacaoItemSRV.AprovarPendencia(status.RLI_ID, usuario, repId, status.OBSERVACOES);

                result.success = true;
                result.message = Message.Info("Aprovado com sucesso!!");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
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

        [AutorizarCustom(IsAjax = true, SessionUtilMethodName = "PossuiGerenciaVenda")]
        [HttpPost]
        public ActionResult RejeitarPendencia(AlteracaoStatusDTO status)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;
                _registroLiberacaoItemSRV.RejeitarPendencia(status.RLI_ID, usuario, repId, status.OBSERVACOES);

                result.success = true;
                result.message = Message.Info("Rejeitado com sucesso!!");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
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

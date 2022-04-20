using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class NotaFiscalConfigController : Controller
    {
        public NotaFiscalConfigSRV Service { get; set; }
        public ImpostoSRV ImpostoSRV { get; set; }
        public NotaFiscalConfigTipoSRV NotaFiscalConfigTipoSRV { get; set; } 
        //
        // GET: /NotaFiscalConfig/

        public ActionResult Index()
        {
            return View();
        }


        [Autorizar(IsAjax = true)]
        public JsonResult ListarNotaFiscalConfigPorProduto(int? cmpId, int pagina = 1, int registrosPorPagina = 15)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstNotaFiscalConfig = Service.ListarNotaFiscalConfig(cmpId, prencherDados: true);
                response.Add("lstNotaFiscalConfig", lstNotaFiscalConfig);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public JsonResult ListarImpostos()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstImpostos = ImpostoSRV.FindAll();
                response.Add("lstImpostos", lstImpostos);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public JsonResult ListarNotaFiscalConfigTipo()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstNotaFiscalConfigTp = NotaFiscalConfigTipoSRV.FindAll();
                response.Add("lstNotaFiscalConfigTp", lstNotaFiscalConfigTp);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(NotaFiscalConfigSaveRequestDTO notaFiscalConfigSaveRequest)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    Service.SalvarNotaFiscalConfig(notaFiscalConfigSaveRequest);
                    result.message = Message.Success("Dados salvos com sucesso.");
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

            return Json(result);

        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ValidarNotaFiscalConfig(NotaFiscalConfigDTO notaFiscalConfig)
        {
            JSONResponse result = new JSONResponse();
            if (ModelState.IsValid)
            {
                return Json(result);

            }
            else
            {
                result.success = false;
                result.SetMessageFromModelState(ModelState);
                return Json(result);
            }
        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ValidarConfigImposto(ConfigImpostoDTO notaFiscalConfig)
        {
            JSONResponse result = new JSONResponse();
            if (ModelState.IsValid)
            {
                return Json(result);

            }
            else
            {
                result.success = false;
                result.SetMessageFromModelState(ModelState);
                return Json(result);
            }
        }


        [Autorizar(IsAjax = true)]
        public JsonResult ClonarConfiguracao(int? cmpIdOrigem, int? cmpIdDestino)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var nfConfigClonada = Service.ClonarConfiguracao(cmpIdOrigem, cmpIdDestino);
                response.Add("nfConfigClonada", nfConfigClonada);
                
                response.message = Message.Success("Configuração copiada com sucesso.");
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

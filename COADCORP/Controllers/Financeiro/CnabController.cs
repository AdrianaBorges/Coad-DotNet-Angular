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
using COAD.CORPORATIVO.Service.Custons;
using System.Threading.Tasks;
using System.Web.SessionState;
using COAD.SEGURANCA.Service.Custons;
using COAD.FISCAL.Service.Integracoes.Interfaces;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using GenericCrud.Service;
using Coad.GenericCrud.Exceptions;

namespace COADCORP.Areas.franquia.Controllers.Financeiro
{
    public class CnabController : Controller
    {
        private CnabSRV _cnabSRV { get; set; }
        private CnabConfigSRV _cnabConfigSRV { get; set; }
        private BancosSRV _bancosSRV { get; set; }
        public CnabTipoRegistroSRV _cnabTipoRegistroSRV { get; set; }
        public CnabTipoDadosSRV _cnabTipoDadosSRV { get; set; }

        public CnabController(
            CnabSRV _cnabSRV, 
            CnabConfigSRV CnabConfigSRV,
            BancosSRV _bancosSRV)
        {
            this._cnabSRV = _cnabSRV;
            this._cnabConfigSRV = CnabConfigSRV;
            this._bancosSRV = _bancosSRV;
        }

        [Autorizar(Departamento = "TI, franquiado, franquiador, controladoria", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Editar(int? cncID)
        {
            ViewBag.cncID = cncID;
            return View();
        }

        public JsonResult PesquisarCnabConfig(PesquisaCnabConfigDTO pesquisaCnabDTO)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstCnabConfig = _cnabConfigSRV.PesquisarCnabConfig(pesquisaCnabDTO);
                response.AddPage("lstCnabConfig", lstCnabConfig);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ReceberUploadPlanilhaCarga()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                HttpPostedFileBase arquivo = Request.Files[0];
                UploadUtil.ArmazenarArquivoTemporario(arquivo);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult InserirAtualizarPlanilhaCarga()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                var usuario = SessionContext.login;
                var serverPath = Server.MapPath("~/");
                HttpPostedFileBase arquivo = Request.Files[0];
                var lstCnabs = _cnabSRV.InserirAtualizarPlanilhaCarga(serverPath, arquivo);
                response.Add("lstCnabs", lstCnabs);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        public ActionResult ListarBancos()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstBancos = _bancosSRV.FindAll(); 
                response.Add("lstBancos", lstBancos);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDoCnabConfig(int? cncId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var cnabConfig = _cnabConfigSRV.FindByIdFullLoaded(cncId, true, true);
                response.Add("cnabConfig", cnabConfig);

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
        public ActionResult SalvarCnabConfig(CnabConfigDTO cnabConfig)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _cnabConfigSRV.SalvarCnabConfig(cnabConfig);
                    SysException.RegistrarLog("Dados da config cnab atualizados com sucesso!!", "", SessionContext.autenticado);
                    result.success = true;
                    result.message = Message.Info("Dados da config cnab atualizados com sucesso!!");

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

        public ActionResult ListarTipoRegistro()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstTipoRegistro = _cnabTipoRegistroSRV.FindAll();
                response.Add("lstTipoRegistro", lstTipoRegistro);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarTipoDados()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstTipoDados = _cnabTipoDadosSRV.FindAll();
                response.Add("lstTipoDados", lstTipoDados);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public FileResult DownloadPlanilhaCnab(int? ccaId)
        {
            var serverPath = Server.MapPath("~/");
            var downloadInfo = _cnabSRV.RetornarPlanilhaCnab(ccaId, serverPath);

            System.IO.File.Delete(downloadInfo.Path);

            return File(downloadInfo.Bytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadInfo.FileName);
        }
    }
}

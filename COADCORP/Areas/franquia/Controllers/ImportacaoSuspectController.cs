using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Util;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;

namespace COADCORP.Areas.franquia.Controllers
{
    
    public class ImportacaoSuspectController : Controller
    {
        //private const int uenId = 1;
        //
        // GET: /franquia/cliente
        public AssinaturaSRV _serviceAss { get; set; }
        public ClienteSRV _service { get; set; }
        public ImportacaoSuspectSRV _importacaoSuspectSRV { get; set; }
        public ImportacaoSRV _importacaoSRV { get; set; }
        public ImportacaoStatusSRV _importacaoStatusSRV { get; set; }
        public ImportacaoHistoricoSRV _importacaoHistoricoSRV { get; set; }
        public ImportacaoResultadoRodizioSRV _importacaoResultadoSRV { get; set; }

        [Autorizar(GerenteDepartamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Agendamento(int? impID)
        {
            ViewBag.impID = impID;
            return View();
        }


        [Autorizar(GerenteDepartamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Index()
        {

            return View();
        }


        [Autorizar]
        public ActionResult Novo(string cpf_cnpj, string nome, string email, 
                                                        string REP_ID, string dddTelefone, string telefone, int? AREA_ID, int? CMP_ID)
        {
            ViewBag.filtrosAInserir = new CadastraSuspectComBaseNoFiltroDTO()
            {
                cpf_cnpj = cpf_cnpj,
                nome = nome,
                email = email,
                dddTelefone = dddTelefone,
                telefone = telefone,
                AREA_ID = AREA_ID,
                CMP_ID = CMP_ID
            };
                
            ViewBag.RG_ID = SessionUtil.GetRegiao();

            return View();
        }

       
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(ClienteDto cliente)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    
                    int? rep_id = null;

                    if (AuthUtil.TryGetRepId(out rep_id))
                    {
                        _service.SalvarClienteRodizio(cliente, rep_id);
                    }
                    else {

                        throw new Exception("O representante não pode ser encontrado.");
                    }
                    SysException.RegistrarLog("Dados do cliente atualizados com sucesso!!", "", SessionContext.autenticado);
                    
                    result.success = true;
                    result.message = Message.Info("Dados do suspect atualizados com sucesso!!");
                    
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidacaoException ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.SetMessageFromValidacaoException(ex);
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

        [Autorizar(GerenteDepartamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult GerarPreviaDeImportacao()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                string sessionId = HttpContext.Session.SessionID;
                var previaImportacao = _importacaoSuspectSRV.GerarPreviaDeImportacao(sessionId);
                response.Add("previaImportacao", previaImportacao);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        
        public ActionResult ListarImportacaoStatus()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstImportacaoStatus = _importacaoStatusSRV.FindAll();
                response.Add("lstImportacaoStatus", lstImportacaoStatus);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult ReceberUploadPlanilhaSuspect()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                HttpPostedFileBase arquivo = Request.Files[0];
                var repId = SessionContext.GetIdRepresentante();
                var login = SessionContext.login;
                var path = Server.MapPath("~/");

                var sessionId = HttpContext.Session.SessionID;
                _importacaoSuspectSRV.ReceberUploadPlanilhaSuspectsEAgendarImportacao(arquivo, path, "temp", sessionId);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult AgendarImportacaoDeSuspect()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                var usuario = SessionContext.login;
                var sessionId = HttpContext.Session.SessionID;
                var path = Server.MapPath("~/");
                var importacao = _importacaoSRV.AgendarImportacao(sessionId, repId, usuario);
                
                response.Add("importacao", importacao);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        public JsonResult PesquisarImportacoes(PesquisaImportacaoDTO pesquisaImportacaoDTO)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstImportacoes = _importacaoSRV.PesquisarImportacoes(pesquisaImportacaoDTO);
                response.AddPage("lstImportacoes", lstImportacoes);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PesquisarHistorico(DateTime? dataInicial, DateTime? dataFinal, 
            int? impID, 
            int? ipsID,
            int? imsID, 
            int pagina = 1, 
            int registroPorPagina = 6)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstHistoricos = _importacaoHistoricoSRV.PesquisarHistorico(
                    dataInicial, 
                    dataFinal,
                    impID,
                    ipsID,
                    imsID, 
                    pagina, 
                    registroPorPagina);
                response.AddPage("lstHistoricos", lstHistoricos);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PesquisarImportacoesSuspect(PesquisaImportacaoSuspectDTO pesquisaImportacaoSuspectDTO)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstImportacoesSuspect = _importacaoSuspectSRV.PesquisarImportacaoSuspects(pesquisaImportacaoSuspectDTO);
                response.AddPage("lstImportacoesSuspect", lstImportacoesSuspect);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarResultadosDeRodizio(int? impID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var retorno = _importacaoResultadoSRV.RetornarResultado(impID);
                response.Add("retorno", retorno);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public FileResult DownloadPlanilhaSuspectComErro(int? impId)
        {
            var serverPath = Server.MapPath("~/");
            var downloadInfo = _importacaoSuspectSRV.RetornarPlanilhaImportacaoNaoProcessada(impId, serverPath);

            System.IO.File.Delete(downloadInfo.Path);

            return File(downloadInfo.Bytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadInfo.FileName);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ReceberUploadPlanilhaComErroImportacao()
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
        public ActionResult AtualizarSuspectsIncorretos(int? impID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                var usuario = SessionContext.login;
                var serverPath = Server.MapPath("~/");
                HttpPostedFileBase arquivo = UploadUtil.RetornarArquivoDeUpload();

                var context = _importacaoSRV.ExecutarAtualizacaoSuspectsIncorretos(impID, serverPath, repId, usuario);
                response.Add("context", context);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e); 
            }

            return Json(response);
        }

        public ActionResult RetonarProgressoDoJob(int? impID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var batchProgress = _importacaoSRV.RetornarProgressoDaImportacao(impID);
                response.Add("batchProgress", batchProgress);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RetonarDadosDaImportacao(int? impID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var importacao = _importacaoSRV.FindById(impID);
                response.Add("importacao", importacao);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult ReexecutarImportacao(int? impID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                var usuario = SessionContext.login;
                _importacaoSRV.ReexecutarImportacao(impID, repId, usuario);
                response.message = Message.Success("A importação foi agendanda para executar. Aguarde até 10 segundos para execução. Se não houver outra importação em andamento.");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CancelarImportacao(int? impID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                var usuario = SessionContext.login;

                _importacaoSRV.CancelarImportacao(impID, repId, usuario);
                response.message = Message.Success("Importação cancelada com sucesso.");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        public ActionResult RetornarCodigoDoCliente(int? ipsID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var cliID = _service.RetornarCodDoClienteDaImportacaoSuspect(ipsID);
                response.Add("cliID", cliID);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}

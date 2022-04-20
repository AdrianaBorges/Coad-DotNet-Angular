using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Util;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace COADCORP.Controllers
{
    public class JobsController : Controller
    {
        public JobAgendamentoSRV _service { get; set; }
        public JobNotificacaoSRV _jobNotificacao { get; set; }
        public SchedulerSRV schedulerSRV { get; set; }
        public NotificacaoSistemaSRV _notificacaoSistemaSRV { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        //[AutorizarCustom(IsAjax = true, SessionUtilMethodName = "PossuiGerenciaVenda")]
        public JsonResult PesquisarJobs(int pagina = 1, int registrosPorPagina = 8)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstJobs = _service.PesquisarJobAgendamento(pagina, registrosPorPagina);
                response.AddPage("lstJobs", lstJobs);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public JsonResult PesquisarNotificacoesJobsAtivos(int pagina = 1, int registrosPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = SessionContext.login;
                var lstJobs = _jobNotificacao.ListarNotificacaoAtivasPorJob(usuario, pagina, registrosPorPagina);
                response.AddPage("lstJobs", lstJobs);
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
        public ActionResult LigarDesligarJob(int? jobId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                    _service.LigarDesligarJob(jobId);
                    SysException.RegistrarLog("Job Ligado/Desligado com sucesso!!", "", SessionContext.autenticado);
                    result.success = true;
                    result.message = Message.Info("Job Ligado/Desligado com sucesso!!");

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


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ExecutarManualmenteJob(int? jobId, int? codRef, string codRefStr = null)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.ExecutarManualmenteJob(jobId, codRef, codRefStr);
                SysException.RegistrarLog("Execução agendada!!", "", SessionContext.autenticado);
                result.success = true;
                result.message = Message.Info("O JOB será executado em até 7 segundos.");

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

        [Autorizar(IsAjax = true)]
        public ActionResult StartJob()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                schedulerSRV.StartService();
                SysException.RegistrarLog("Service Iniciado!!", "", SessionContext.autenticado);
                result.success = true;
                result.message = Message.Info("O JOB foi iniciado com sucesso!");

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

        [Autorizar(IsAjax = true)]
        public ActionResult StopJob()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                schedulerSRV.StopService();
                SysException.RegistrarLog("Serviço Parado!!", "", SessionContext.autenticado);
                result.success = true;
                result.message = Message.Info("O JOB foi parado com sucesso!");

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

        
        [Autorizar(IsAjax = true)]
        public ActionResult CancelarJobNotificacao(int? jnfID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _jobNotificacao.CancelarJobNotificacao(jnfID);
                result.success = true;

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

        [Autorizar(IsAjax = true)]
        public JsonResult ListarJobNotificacaoMsgItem(int? jnf)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = SessionContext.login;
                var lstMsg = ServiceFactory.RetornarServico<JobNotificacaoMsgItemSRV>().ListarJobNotificacaoMsgItem(jnf);
                response.Add("lstMsg", lstMsg);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarNotificacaoSistema(int? tnsID, int? codRefInt = null, string codRefStr = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var notifySistema = _notificacaoSistemaSRV.RetornarNotificacaoSistema(tnsID, codRefInt, codRefStr);
                response.Add("notifySistema", notifySistema);
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

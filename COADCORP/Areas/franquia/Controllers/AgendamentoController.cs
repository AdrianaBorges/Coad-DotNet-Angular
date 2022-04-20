using System;
using System.Web.Mvc;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Util;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Util;

namespace COADCORP.Areas.franquia.Controllers
{
    public class AgendamentoController : Controller
    {
        //
        // GET: /franquia/Fila/

        private readonly AgendamentoSRV _agendamentoSRV = new AgendamentoSRV();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarHorarios()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstHorarios = AgendamentoUtil.SelectHorarios();
                response.Add("horarios", lstHorarios);
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
        public ActionResult CriarAgendamento(AgendamentoDTO agendamento)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? REP_ID = null;

                    if (AuthUtil.TryGetRepId(out REP_ID))
                    {
                        var login =  SessionContext.autenticado.USU_LOGIN;
                        var rgId = SessionUtil.GetRegiao();

                        agendamento.REP_ID = REP_ID;
                        agendamento.RG_ID = rgId;
                        _agendamentoSRV.SalvarAgendamento(agendamento, login);

                        SysException.RegistrarLog("Agendamento realizado com sucesso!", "", SessionContext.autenticado);

                        result.success = true;
                        result.message = Message.Info("Dados do cliente atualizados com sucesso!!");
                       
                    }
                    else
                    {
                        result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                        result.success = false;
                        
                    }                   

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    
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

        public ActionResult ListarAgendamentosPorClienteEOperadora(DateTime? dataInicial = null, DateTime? dataFinal = null, int? CLI_ID = null, int pagina = 1, int registrosPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;


                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var rgId = SessionUtil.GetRegiao();
                    var lstAgendamento = _agendamentoSRV.Agendamentos(dataInicial, dataFinal, REP_ID, CLI_ID, pagina, registrosPorPagina, rgId);
                    response.AddPage("lstAgendamento", lstAgendamento);
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ConfirmarAgendamento(AgendamentoDTO agendamento)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? REP_ID = null;

                    if (AuthUtil.TryGetRepId(out REP_ID))
                    {

                        var login = SessionContext.autenticado.USU_LOGIN;
                        agendamento.REP_ID = REP_ID;
                        _agendamentoSRV.ConfirmarAgendamento(agendamento, login);

                        SysException.RegistrarLog("Confirmação de agendamento realizado com sucesso!", "", SessionContext.autenticado);

                        result.success = true;

                    }
                    else
                    {
                        result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                        result.success = false;

                    }

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);

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
        [HttpPost]
        public ActionResult Reagendar(AgendamentoDTO agendamento)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? REP_ID = null;

                    if (AuthUtil.TryGetRepId(out REP_ID))
                    {
                        var login = SessionContext.autenticado.USU_LOGIN;
                        agendamento.REP_ID = REP_ID;
                        _agendamentoSRV.Reagendar(agendamento, login);

                        SysException.RegistrarLog("Reagendamento realizado com sucesso!", "", SessionContext.autenticado);
                        result.success = true;
                    }
                    else
                    {
                        result.message = Message.Fail("O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                        result.success = false;

                    }

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);

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
       

    }
}

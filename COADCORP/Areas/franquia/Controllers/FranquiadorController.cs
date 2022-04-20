using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Util;
using COAD.UTIL.Grafico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Areas.franquia.Controllers
{
    [Autorizar(GerenteDepartamento = "franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true)]
    public class FranquiadorController : Controller
    {
        private ClienteSRV _clienteSRV = new ClienteSRV();
        private RegiaoSRV _regiaoSRV = new RegiaoSRV();
        private RepresentanteSRV _representanteSRV = new RepresentanteSRV();
        private PrioridadeAtendimentoSRV _prioridadeAtendimentoSRV = new PrioridadeAtendimentoSRV();
        private AgendamentoSRV _agendamentoSRV = new AgendamentoSRV();

        //private const int uenId = 1;        
      
        //
        // GET: /franquia/Gerente/

        public ActionResult Index()
        {
            int? UEN_ID = null;
            UEN_ID = SessionUtil.GetUenId();
            
            if (UEN_ID == null)
            {
                UEN_ID = 1;
            }

            var lstRegiao = _regiaoSRV.ListarRegioes(UEN_ID);
            ViewBag.regioes = lstRegiao;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarDadosHome(int pagina = 1, int registrosPorPagina = 5, int? REP_ID = null, int? RG_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                //var RG_ID = _regiaoSRV.ObterRgIdDoRepresentante(REP_ID);

                var lstPrioridades = _prioridadeAtendimentoSRV.GetPrioridadesByRepresentante(REP_ID, pagina, registrosPorPagina, RG_ID);
                var quantidadeTipoCliente = _clienteSRV.QtdClientesRepresentante(REP_ID);

                response.AddPage("lstPrioridades", lstPrioridades);
                //response.Add("quantidadeTipoCliente", quantidadeTipoCliente);                

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult RepresentantesDaRegiao(int? RG_ID, string nome = null, string descricaoUf = null,int pagina = 1, int registroPorPagina = 5)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                int? REP_ID = null;
                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var uenId = SessionUtil.GetUenId();
                    var representantes = _representanteSRV.Representantes(nome, RG_ID, uenId, false, pagina, registroPorPagina,nivelRepresentanteId: 4);
                    response.AddPage("representantes", representantes);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                }
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public ActionResult QuantidadeClientesPorTipo(int? REP_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? GERENTE_REP_ID = null;
                if (REP_ID != null && AuthUtil.TryGetRepId(out GERENTE_REP_ID))
                {
                    var uenId = SessionUtil.GetUenId();
                    var quantidadeTipoCliente = _clienteSRV.QtdClientesRepresentante(REP_ID, uenId);
                    response.Add("resumoQuantidadeTipoCliente", quantidadeTipoCliente);
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
       
        public ActionResult ListarAgendamentoDoDia(DateTime? data = null, int pagina = 1, int registrosPorPagina = 5, int? REP_ID = null, int? RG_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? GERENTE_REP_ID = null;

                if (REP_ID != null && AuthUtil.TryGetRepId(out GERENTE_REP_ID))
                {
                    if (data == null)
                    {
                        data = DateTime.Now;
                    }
                    //int? rgId = SessionUtil.GetRegiao();
                    
                    var lstAgendamentoDoDia = _agendamentoSRV.ListarAgendamentosDoDia(data, REP_ID, pagina, registrosPorPagina, RG_ID);
                    response.AddPage("lstAgendamentoDoDia", lstAgendamentoDoDia);
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarAgendamentoAtrasado(DateTime? data = null, int pagina = 1, int registrosPorPagina = 5, int? REP_ID = null, int? RG_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? GERENTE_REP_ID = null;

                if (REP_ID != null && AuthUtil.TryGetRepId(out GERENTE_REP_ID))
                {
                    if (data == null)
                    {
                        data = DateTime.Now;
                    }

                    //int? rgId = SessionUtil.GetRegiao();
                    
                    var lstAgendamentoAtrasado = _agendamentoSRV.ListarAgendamentosAtrasados(data, REP_ID, pagina, registrosPorPagina, RG_ID);
                    response.AddPage("lstAgendamentoAtrasado", lstAgendamentoAtrasado);
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarAgendamentoVindouro(DateTime? data = null, int pagina = 1, int registrosPorPagina = 5, int? REP_ID = null, int? RG_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? GERENTE_REP_ID = null;

                if (REP_ID != null && AuthUtil.TryGetRepId(out GERENTE_REP_ID))
                {
                    if (data == null)
                    {
                        data = DateTime.Now;
                    }
                    //int? rgId = SessionUtil.GetRegiao();
                    
                    var lstAgendamentoVindouro = _agendamentoSRV.ListarAgendamentosVindouros(data, REP_ID, pagina, registrosPorPagina, RG_ID);
                    response.AddPage("lstAgendamentoVindouro", lstAgendamentoVindouro);
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
        public ActionResult TransferirClientes()
        {
            return View();
        }

        [Autorizar(IsAjax = true, GerenteDepartamento = "franquiador, franquiado, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult AtendimentosRealizadosNoMesPorRegiao(DateTime? data)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var uenId = SessionUtil.GetUenId();
                var atendimentosRealizadosPorRegiao = new HistAtendSRV().ListarAtendimentosRealizadosNoMesPorRegiao(data, uenId);
                response.Add("atendimentosRealizadosMesPorRegiao", atendimentosRealizadosPorRegiao);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true, GerenteDepartamento = "franquiador, franquiado, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult AtendimentosRealizadosNoMes(DateTime? data)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var uenId = SessionUtil.GetUenId();
                var atendimentosRealizados = new HistAtendSRV().ListarAtendimentosRealizadosNoMes(data, uenId);
                response.Add("atendimentosRealizadosMes", atendimentosRealizados);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }


        [Autorizar(IsAjax = true, GerenteDepartamento = "franquiador, franquiado, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult RelatorioFaturamentoFranquia(DateTime? data)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var uenId = SessionUtil.GetUenId();
                var relatorioFaturamentoFranquia = new ContratoSRV().RelatorioFaturamentoFranquia(data, uenId);
                response.Add("relatorioFaturamentoFranquia", relatorioFaturamentoFranquia);
              
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true, GerenteDepartamento = "franquiador, franquiado, TI")]
        public ActionResult RelatorioAtendimentoXVendasPorRegiao(DateTime? data)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var uenId = SessionUtil.GetUenId();
                var relatorioAtendimentoXVendasPorRegiao = new HistAtendSRV().GerarRelatorioAtendimentoXVendasPorRegiao(data, uenId);
                response.Add("relatorioAtendimentoXVendasPorRegiao", relatorioAtendimentoXVendasPorRegiao);

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

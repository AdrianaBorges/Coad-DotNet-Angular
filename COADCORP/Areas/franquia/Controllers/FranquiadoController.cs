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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericCrud.Controllers;

namespace COADCORP.Areas.franquia.Controllers
{
    [Autorizar(GerenteDepartamento = "franquiado")]
    public class FranquiadoController : Controller
    {
        private ClienteSRV _clienteSRV = new ClienteSRV();
        //private const int uenId = 1;
        
        private AssinaturaSRV _serviceAss = new AssinaturaSRV();
        private ProspectSRV _prospectSRV = new ProspectSRV();
        private TipoDeClienteSRVProxy _tipoDeCliente = new TipoDeClienteSRVProxy();
        private AgendaSRV _agenda = new AgendaSRV();
        private HistAtendSRV _atendimento = new HistAtendSRV();
        private OpcaoAtendimentoSRV _setor = new OpcaoAtendimentoSRV();
        private TipoTelefoneSRV _tipotelefone = new TipoTelefoneSRV();
        private TipoAtendimentoSRV _tipoAtendSRV = new TipoAtendimentoSRV();
        private AcaoAtendimentoSRV _acaoAtendSRV = new AcaoAtendimentoSRV();
        private RepresentanteSRV _representanteSRV = new RepresentanteSRV();
        private PrioridadeAtendimentoSRV _prioridadeAtendimentoSRV = new PrioridadeAtendimentoSRV();
        private AgendamentoSRV _agendamentoSRV = new AgendamentoSRV();
        private CarteiramentoSRV _carteiramentoSRV = new CarteiramentoSRV();

        private LinhaProdutoInformativoSRV _lproinfSRV = new LinhaProdutoInformativoSRV();
        //
        // GET: /franquia/Gerente/

        public ActionResult Index()
        {
            return View();
        }

        private void _preencherCombos()
        {
            
         var _SetorTelefones = _setor.BuscarSetorDeTelefones().ToList();
            var _SetorEmails = _setor.BuscarSetorDeEmails().ToList();
            var _tipotel = _tipotelefone.BuscarTodos().ToList(); 

            var ListaTipoAtendimento = _tipoAtendSRV.FindAll();
            var ListaAcaoAtendimento = _acaoAtendSRV.FindAll();

            var ufs = new UFSRV().BuscarSomenteUF();
            var _tipoLogradouro = new TipoLogradouroSRV().FindAll();
            var tipoEndereco = new TipoEnderecoSRV().FindAll();
            var municipio = new MunicipioSRV().FindAll();
            var ListaClassificacao = new ClassificacaoSRV().FindAll();

           
                        
            ViewBag.ufs = ufs;
            ViewBag.tiposDeCliente = _tipoDeCliente.RetornarTiposDeCliente(0);
            ViewBag.tipoLogradouro = _tipoLogradouro;
            ViewBag.tipoEndereco = tipoEndereco;           
            ViewBag.ListaTipoAtendimento = new SelectList(ListaTipoAtendimento, "TIP_ATEND_ID", "TIP_ATEND_DESCRICAO");
            ViewBag.ListaAcaoAtendimento = new SelectList(ListaAcaoAtendimento, "ACA_ID", "ACA_DESCRICAO");
            ViewBag.ListaUf = new SelectList(new UFSRV().FindAll().ToList(), "UF_SIGLA", "UF_DESCRICAO");

            ViewBag.ListaClassificacao = new SelectList(ListaClassificacao, "CLA_ID", "CLA_DESCRICAO");           
            ViewBag.ListaSetorTelefone = new SelectList(_SetorTelefones, "OPC_ID", "OPC_DESCRICAO");
            ViewBag.ListaSetorEmail = new SelectList(_SetorEmails, "OPC_ID", "OPC_DESCRICAO");
            ViewBag.Listatipotel = new SelectList(_tipotel, "TIPO_TEL_ID", "TIPO_TEL_ID");
            ViewBag.ListaTipoLogradouro = new SelectList(_tipoLogradouro, "TIPO_LOG_ID", "TIPO_LOG_DESCRICAO");           
        
        }

       
        public ActionResult CadastrarSuspect()
        {

            ViewBag.RG_ID = SessionUtil.GetRegiao();
            _preencherCombos();
            return View();
        }

        public ActionResult Home()
        {
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
                    int? REP_ID = null;

                    if (AuthUtil.TryGetRepId(out REP_ID))
                    {
                        _clienteSRV.SalvarClienteRodizio(cliente, REP_ID);
                        SysException.RegistrarLog("Dados do cliente atualizados com sucesso!!", "", SessionContext.autenticado);

                        result.success = true;
                        result.message = Message.Info("Dados do cliente atualizados com sucesso!!");
                    }                   

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ClientesPorFranquia(string cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 7)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    var representanteId = SessionContext.GetIdRepresentante();
                    int? uenId = SessionUtil.GetUenId();
                    var lstClientes = _clienteSRV.Clientes(cnpj, nome, pagina, registroPorPagina, representanteId, uenId);
                    response.AddPage("clientes", lstClientes);
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

        public ActionResult ListarDadosHome(int pagina = 1, int registrosPorPagina = 5, int? REP_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? GERENTE_REP_ID = null;

                if (REP_ID != null && AuthUtil.TryGetRepId(out GERENTE_REP_ID))
                {
                    var rgId = SessionUtil.GetRegiao();
                    var lstPrioridades = _prioridadeAtendimentoSRV.GetPrioridadesByRepresentante(GERENTE_REP_ID, REP_ID, pagina, registrosPorPagina, rgId);
                    var quantidadeTipoCliente = _clienteSRV.QtdClientesRepresentante(REP_ID);

                    response.AddPage("lstPrioridades", lstPrioridades);
                    //response.Add("quantidadeTipoCliente", quantidadeTipoCliente);
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
                    var quantidadeTipoCliente = _clienteSRV.QtdClientesRepresentanteGerente(GERENTE_REP_ID, REP_ID, uenId);
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

        public ActionResult ListarAgendamentoDoDia(DateTime? data = null, int pagina = 1, int registrosPorPagina = 5, int? REP_ID = null)
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
                    int? rgId = SessionUtil.GetRegiao();
                    
                    var lstAgendamentoDoDia = _agendamentoSRV.ListarAgendamentosDoDiaGerente(data,GERENTE_REP_ID, REP_ID, pagina, registrosPorPagina, rgId);
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

        public ActionResult ListarAgendamentoAtrasado(DateTime? data = null, int pagina = 1, int registrosPorPagina = 5, int? REP_ID = null)
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

                    int? rgId = SessionUtil.GetRegiao();
                    
                    var lstAgendamentoAtrasado = _agendamentoSRV.ListarAgendamentosAtrasadosGerente(data,GERENTE_REP_ID, REP_ID, pagina, registrosPorPagina, rgId);
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

        public ActionResult ListarAgendamentoVindouro(DateTime? data = null, int pagina = 1, int registrosPorPagina = 5, int? REP_ID = null)
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

                    int? rgId = SessionUtil.GetRegiao();
                    
                    var lstAgendamentoVindouro = _agendamentoSRV.ListarAgendamentosVindourosGerente(data, GERENTE_REP_ID, REP_ID, pagina, registrosPorPagina, rgId);
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

        [Autorizar(IsAjax = true, GerenteDepartamento = "Franquiador, Franquiado", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Representantes(string nome = null, int? RG_ID_REPRESENTANTE = null, int pagina = 1, int registroPorPagina = 5)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    if (RG_ID_REPRESENTANTE != null)
                    {
                        RG_ID_REPRESENTANTE = SessionUtil.GetRegiao();
                    }

                    var uenId = SessionUtil.GetUenId();
                    var representantes = _representanteSRV.Representantes(nome, RG_ID_REPRESENTANTE, uenId, false, pagina, registroPorPagina, nivelRepresentanteId: 4);
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

        [Autorizar(GerenteDepartamento = "franquiado", IsAjax = true)]
        public ActionResult ClientesPorRepresentante(int? REP_ID, string cnpj = null, string nome = null, int? classificacaoClienteId = null, string CAR_ID = null, int? RG_ID = null, int pagina = 1, int registroPorPagina = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (REP_ID != null)
                {
                    int? uenId = SessionUtil.GetUenId();
                    var lstClientes = _clienteSRV.Clientes(cnpj, nome, pagina, registroPorPagina, REP_ID, uenId, classificacaoClienteId, CAR_ID, RG_ID);
                    response.AddPage("clientes", lstClientes);
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

        [Autorizar(IsAjax = true)]
        public ActionResult CarteiramentosDaRepresentante(int REP_ID)
        {
            JSONResponse response = new JSONResponse();

            if (SessionContext.PossuiRepresentante())
            {
                var carteiramentos = _carteiramentoSRV.GetCarteirasDoRepresentante(REP_ID);
                response.Add("carteiramentos", carteiramentos);
            }
            else
            {
                response.success = false;
                response.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult TransferirClientes()
        {
            return View();
        }

       

       
    }
}

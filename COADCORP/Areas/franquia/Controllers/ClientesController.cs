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
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Filters;
using GenericCrud.Util;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using System.ComponentModel.DataAnnotations;

namespace COADCORP.Areas.franquia.Controllers
{
    public class ClientesController : Controller
    {
        //private const int uenId = 1;
        //
        // GET: /franquia/cliente
        public AssinaturaSRV _serviceAss { get; set; } // = new AssinaturaSRV();
        public AssinaturaEmailSRV _assinaturaEmailSRV { get; set; } //= new AssinaturaEmailSRV();
        public AssinaturaTelefoneSRV _assinaturaTelefoneSRV { get; set; } //= new AssinaturaTelefoneSRV();
        public ClienteSRV _service { get; set; } //= new ClienteSRV();
        public ProspectSRV _prospectSRV { get; set; } //= new ProspectSRV();
        public TipoClienteSRV _tipoDeCliente { get; set; } //= new TipoDeClienteSRV(); //alterado
        public AgendaSRV _agenda { get; set; } //= new AgendaSRV();
        public HistAtendSRV _atendimento { get; set; } //= new HistAtendSRV();
        public OpcaoAtendimentoSRV _setor { get; set; } //= new OpcaoAtendimentoSRV();
        public TipoTelefoneSRV _tipotelefone { get; set; } //= new TipoTelefoneSRV();
        public TipoAtendimentoSRV _tipoAtendSRV { get; set; } //= new TipoAtendimentoSRV();
        public AcaoAtendimentoSRV _acaoAtendSRV { get; set; } //= new AcaoAtendimentoSRV();
        public LinhaProdutoInformativoSRV _lproinfSRV { get; set; } //= new LinhaProdutoInformativoSRV();
        public ClassificacaoClienteSRV _classificacaoClienteSRV { get; set; } //= new ClassificacaoClienteSRV();
        public PrioridadeAtendimentoSRV _priAtendimentoSRV { get; set; } //= new PrioridadeAtendimentoSRV();
        public OrigemCadastroSRV _origemCadastro { get; set; } //= new OrigemCadastroSRV();

        public ActionResult Clientes(int cliente = 0, 
                                     string contrato = null, 
                                     string pedido = null, 
                                     string assinatura = null, 
                                     string logradouro = null, 
                                     string cnpj = null, 
                                     string nome = null, int pagina = 1, int registroPorPagina = 7)
        {

            JSONResponse response = new JSONResponse();
            
            try
            {
                
                var lstClientes = _service.ListaClientes(cliente, contrato, pedido, assinatura, logradouro, cnpj, nome, pagina, registroPorPagina);
                response.AddPage("clientes", lstClientes);
                response.success = true;

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Cliente/
        [Autorizar(IsAjax = true)]
        public ActionResult preencherGridTel(string _assinatura, int pagina = 1, int registroPorPagina = 20)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                TelefonesDeClienteSRV _srvTelefone = new TelefonesDeClienteSRV();

                var listatelefone = _srvTelefone.BuscarTelefonesDeClientePorAssinatura(_assinatura, pagina, registroPorPagina);

                response.AddPage("listatelefone", listatelefone);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private void _preencherCombos()
        {   
            
            var _SetorTelefones = _setor.BuscarSetorDeTelefones().ToList();
            var _SetorEmails = _setor.BuscarSetorDeEmails().ToList();
            var _tipotel = _tipotelefone.BuscarTodos().ToList(); 

            List<URA> ListaUras = new URASRV().BuscarTodos().ToList();
            List<SelectListItem> ListaPasta = new List<SelectListItem>();
            List<SelectListItem> ListaTipoPessoa = new List<SelectListItem>();
            List<SelectListItem> ListaMotivoTroca = new List<SelectListItem>();
            
            var ListaTipoAtendimento = _tipoAtendSRV.FindAll();
            var ListaAcaoAtendimento = _acaoAtendSRV.FindAll();

            var ufs = new UFSRV().BuscarSomenteUF();
            var _tipoLogradouro = new TipoLogradouroSRV().FindAll();
            var tipoEndereco = new TipoEnderecoSRV().FindAll();
            var municipio = new MunicipioSRV().FindAll();
            var ListaClassificacao = new ClassificacaoSRV().FindAll();

            ListaTipoPessoa.AddRange(new[]{
                            new SelectListItem() { Text = "Física",   Value = "F" },
                            new SelectListItem() { Text = "Jurídica", Value = "J" }
            });

            ListaMotivoTroca.AddRange(new[]{
                            new SelectListItem() { Text = "Uso por Terceiros",   Value = "UPT" },
                            new SelectListItem() { Text = "Extravio", Value = "EXT" },
                            new SelectListItem() { Text = "Cliente quis trocar", Value = "CQT" }
            }); 

            ListaPasta.AddRange(new[]{
                            new SelectListItem() { Text = "Vazia", Value = "VAZ" },
                            new SelectListItem() { Text = "Cheia", Value = "CHE" }
                            
            });
                        
            ViewBag.ufs = ufs;
            ViewBag.tiposDeCliente = _tipoDeCliente.BuscarTiposDeClientesAtivos();
            ViewBag.tipoLogradouro = _tipoLogradouro;
            ViewBag.tipoEndereco = tipoEndereco;
            ViewBag.ListaPasta = new SelectList(ListaPasta, "Value", "Text");
            ViewBag.ListaTipoPessoa = new SelectList(ListaTipoPessoa, "Value", "Text");
            ViewBag.ListaMotivoTroca = new SelectList(ListaMotivoTroca, "Value", "Text");
            ViewBag.ListaTipoAtendimento = new SelectList(ListaTipoAtendimento, "TIP_ATEND_ID", "TIP_ATEND_DESCRICAO");
            ViewBag.ListaAcaoAtendimento = new SelectList(ListaAcaoAtendimento, "ACA_ID", "ACA_DESCRICAO");
            ViewBag.ListaUf = new SelectList(new UFSRV().FindAll().ToList(), "UF_SIGLA", "UF_DESCRICAO");

            ViewBag.ListaClassificacao = new SelectList(ListaClassificacao, "CLA_ID", "CLA_DESCRICAO");
            ViewBag.ListaUras = new SelectList(ListaUras, "URA_ID ", "URA_ID ");
            ViewBag.ListaSetorTelefone = new SelectList(_SetorTelefones, "OPC_ID", "OPC_DESCRICAO");
            ViewBag.ListaSetorEmail = new SelectList(_SetorEmails, "OPC_ID", "OPC_DESCRICAO");
            ViewBag.Listatipotel = new SelectList(_tipotel, "TIPO_TEL_ID", "TIPO_TEL_ID");
            ViewBag.ListaTipoLogradouro = new SelectList(_tipoLogradouro, "TIPO_LOG_ID", "TIPO_LOG_DESCRICAO");
            ViewBag.ListaTipoEndereco = new SelectList(tipoEndereco, "TP_END_ID", "TP_END_DESCRICAO");
           
        }

        [Autorizar(Departamento = "Franquiado")]
        public ActionResult Index()
        {
            var lstClassificacaoCliente = _classificacaoClienteSRV.FindAll();
            ViewBag.lstClassificacaoCliente = lstClassificacaoCliente;

            ViewBag.lstAreasDeInteresse = new AreasSRV().FindAll();
            ViewBag.lstProdutoInteresse = new ProdutoComposicaoSRV().ProdutosDeInteresse();

            var lstOrigemCadastro = _origemCadastro.FindAll();
            ViewBag.lstOrigemCadastro = lstOrigemCadastro;
            
            return View();
        }


        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            ViewBag.validarCPF_CNPJ = 1;
            _preencherCombos();
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int? clienteId)
        {
            ViewBag.validarCPF_CNPJ = 1;
            _preencherCombos();
            ViewBag.clienteId = clienteId;
            ViewBag.abrirModal = false;
            return View();
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult EditarEVisualizar(int? clienteId)
        {
            _preencherCombos();
            ViewBag.clienteId = clienteId;
            ViewBag.abrirModal = true;
            return View("Editar");
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
                        _service.SalvarClienteRodizio(cliente, REP_ID);
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
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
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
        public ActionResult BuscarClientes(
            string cpf_cnpj = null, 
            string nome = null, 
            string email = null,
            int? REP_ID = null,
            string dddTelefone = null,
            string telefone = null,
            int? AREA_ID = null,
            int? CMP_ID = null,
            int? classificacaoClienteId = null,
            int? CLI_ID = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            bool buscarForaDaAgenda = false,
            int? origemId = null,
            int pagina = 1,
            int registroPorPagina = 50,
            bool? excluidosDaValidacao = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    var rgId = SessionUtil.GetRegiao();

                    if (REP_ID != null && (!SessionContext.IsGerenteDepartamento("FRANQUIADO", true) && !SessionContext.IsGerenteDepartamento("FRANQUIADOR", true) && !SessionContext.IsGerenteDepartamento("TI", true)))
                    {
                        throw new Exception("Seu nivelRepresentante não tem acesso a pesquisa por representante.");
                    }

                    var uenId = SessionUtil.GetUenId();

                    var lstClientes = _service.BuscarClientes(new PesquisaClienteDTO() {
                                                                cpf_cnpj = cpf_cnpj,
                                                                nome = nome,
                                                                uen_id = uenId, 
                                                                email = email, 
                                                                REP_ID = REP_ID,
                                                                RG_ID = rgId,
                                                                classificacaoClienteId = classificacaoClienteId,
                                                                dddTelefone = dddTelefone,
                                                                telefone = telefone,
                                                                AREA_ID = AREA_ID,
                                                                CMP_ID = CMP_ID,
                                                                codigoCliente = CLI_ID,
                                                                pesquisaCpfCnpjPorIqualdade = pesquisaCpfCnpjPorIqualdade,
                                                                buscarForaDaAgenda = buscarForaDaAgenda,
                                                                origemId = origemId,
                                                                pagina = pagina, 
                                                                registroPorPagina = registroPorPagina,
                                                                excluidosDaValidacao = excluidosDaValidacao});

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

        public ActionResult BuscarParcelas(string _numcontrato)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_numcontrato != null && _numcontrato != "")
                {
                    var listaparcelas = new ParcelasSRV().BuscarPorContrato(_numcontrato);
                    response.Add("listaparcelas", listaparcelas);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"Não foi possivel consultar as parcelas relacionadas a este contrato.");
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
        public JsonResult RecuperarDadosDoCliente(Int32 clienteId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var cliente = _service.FindByIdFullLoaded(clienteId, true, true, true, true);
                response.Add("cliente", cliente);

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
        public JsonResult RecuperarDadosDoClienteParaConfiguracao(int? clienteId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                bool podeEditar = false;

                var cliente = _service.findByIdFullLoadedECheca(clienteId, true, true, true);
                response.Add("cliente", cliente);
                

                int? REP_ID = null;
                int? uenId = SessionUtil.GetUenId();
                if(AuthUtil.TryGetRepId(out REP_ID)){

                    if (cliente != null && cliente.ClienteExisteNaAgenda)
                    {
                        if (_service.RepresentantePodeEditarCliente(clienteId, REP_ID, uenId))
                        {
                            podeEditar = true;
                        }
                    }
                }

                response.Add("podeEditar", podeEditar);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(SessionUtil.RecursiveShowExceptionsMessage(e));
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        [Autorizar(IsAjax = true)]
        [HttpPost]
        public JsonResult RecuperarDadosDoClientePorAssinatura(string codAssinatura)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var cliente = _service.BuscarPorAssinatura(codAssinatura);
                response.Add("cliente", cliente);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ProspectsPorRepresentante(string cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 7)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    var representanteId = SessionContext.GetIdRepresentante();
                    var lstProspects = _prospectSRV.Prospects(cnpj, nome, pagina, registroPorPagina, representanteId);
                    response.AddPage("prospects", lstProspects);
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
        public ActionResult BuscarAssinatura(int? _cli_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_cli_id != null)
                {
                    AssinaturaSRV _srvAssinatura = new AssinaturaSRV();

                    var listaassinatura = _srvAssinatura.BuscarPorCliente((int)_cli_id);

                    response.Add("listaassinatura", listaassinatura);

                    response.success = true;
                }
                else
                    throw new Exception("Cliente não informado!! Verifique!!");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Consultas para preencher os grids da tela
        /// </summary>
        /// <returns></returns>
        [Autorizar(IsAjax = true)]
        public ActionResult PreencherGrids(string _assinatura, int pagina = 1, int registroPorPagina = 20)
        {

            JSONResponse response = new JSONResponse();

            try
            {

                TelefonesDeClienteSRV _srvTelefone = new TelefonesDeClienteSRV();
                ContratoSRV _srvContrato = new ContratoSRV();
                EmailsDeClienteSRV _srvEmail = new EmailsDeClienteSRV();
                var listaatend = _atendimento.BuscarPorAssinatura(_assinatura);
                var listaemail = _srvEmail.BuscarEmailsDeCliente(_assinatura);
                var listacontrato = _srvContrato.BuscarPorAssinatura(_assinatura);
                var listatelefone = _srvTelefone.BuscarTelefonesDeClientePorAssinatura(_assinatura, pagina, registroPorPagina);
                //var listaQtdeConsEmail = _serviceAss.ConsultasPorPeriodo(_assinatura);

                //response.Add("listaQtdeConsEmail", listaQtdeConsEmail);
                response.Add("listacontrato", listacontrato);
                response.Add("listaemail", listaemail);
                response.AddPage("listatelefone", listatelefone);
                response.Add("listaatend", listaatend);

                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarHistorico(string _assinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                ///--- Busca do corporativo antigo.
                var listaagenda = _agenda.ListarPorAssinatura(_assinatura);
                ///--------------------------------

                var listaatend = _atendimento.BuscarPorAssinatura(_assinatura);

                response.Add("listaagenda", listaagenda);
                response.Add("listaatend", listaatend);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarInformativo(int _pro_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listainformativos = _lproinfSRV.ListarInformativo(_pro_id);

                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("listainformativos", listainformativos);
                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult GravarAtendimento(HistoricoAtendimentoDTO _histAtend)
        {

            JSONResponse result = new JSONResponse();
            try
            {
                _histAtend.HAT_DATA_HIST = DateTime.Now;

                _histAtend.HAT_IMP_ETIQUETA = _acaoAtendSRV.ImpEtiqueta((int)_histAtend.ACA_ID);

                if (!(bool)_histAtend.HAT_IMP_ETIQUETA)
                   _histAtend.HAT_DATA_RESOLUCAO = DateTime.Now;

                _histAtend.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
           
                new HistAtendSRV().Save(_histAtend);

                result.success = true;
                result.message = Message.Info("Atendimento realizado com sucesso!!");

                SysException.RegistrarLog("Atendimento realizado com sucesso!!", "", SessionContext.autenticado);
                
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(SysException.Show(ex));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
          
        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult InformarContato(ContatoFranquiaDTO contato)
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
                        _service.InformarContato(contato, (int)REP_ID, login);

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

        [Obsolete("A evolução do cliente agora é feita de forma automática.")]
        public ActionResult EvoluirStatusDoCliente(ClienteDto cliente)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.EvoluirTipoCliente(cliente);
                SysException.RegistrarLog("Dados do departamento atualizados com sucesso!!", "", SessionContext.autenticado);
                result.success = true;

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

        [Autorizar]
        public ActionResult BuscarClientes()
        {
            if (SessionUtil.FranquiadoOuGerenteOuTI())
            {
                var lstClassificacaoCliente = _classificacaoClienteSRV.FindAll();
                ViewBag.lstClassificacaoCliente = lstClassificacaoCliente;
            }

            var lstOrigemCadastro = _origemCadastro.FindAll();
            ViewBag.lstOrigemCadastro = lstOrigemCadastro;
            return View();
        }

        public ActionResult EncaminharCliente(EncaminhamentoDTO encaminhamento)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    string usuario = SessionContext.login;
                    int? REP_ID = null;
                    if (AuthUtil.TryGetRepId(out REP_ID))
                    {
                        var REP_PARA_ENCAMINHAR = encaminhamento.REP_ID;

                        if (REP_PARA_ENCAMINHAR == REP_ID)
                        {
                            throw new Exception("Não é possível encaminhar para você mesmo.");
                        }

                        _service.EncaminharCliente(encaminhamento, REP_ID, usuario);
                        result.success = true;                                        
                    }
                    else
                    {
                        result.success = false;
                        result.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    }

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                }

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

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult SuspectsCadastradosNoDia(DateTime data, int? RG_ID, int pagina = 1, int registroPorPagina = 100)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    if (SessionContext.IsGerenteDepartamento("Franquiado"))
                    {
                        if (RG_ID == null)
                        {
                            RG_ID = SessionUtil.GetRegiao();
                        }
                    }

                    var uenId = SessionUtil.GetUenId();
                    var lstSuspectsCadastradosNoDia = _service.SuspectsCadastradosNoDia(data, uenId, RG_ID, pagina, registroPorPagina);
                    response.AddPage("lstSuspectsCadastradosNoDia", lstSuspectsCadastradosNoDia);
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

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult ClientesComPrioridade(DateTime data, int? RG_ID, int pagina = 1, int registroPorPagina = 100)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    if (SessionContext.IsGerenteDepartamento("Franquiado"))
                    {
                        if (RG_ID == null)
                        {
                            RG_ID = SessionUtil.GetRegiao();
                        }
                    }

                    var uenId = SessionUtil.GetUenId();
                    var lstClientesComPrioridade = _priAtendimentoSRV.ClientesComPrioridadeEncaminhados(data, uenId, RG_ID, pagina, registroPorPagina);
                    response.AddPage("lstClientesComPrioridade", lstClientesComPrioridade);
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
        public ActionResult ClientesPorRepresentante(string cpf_cnpj = null,
                                                        string nome = null,
                                                        string email = null,
                                                        string dddTelefone = null,
                                                        string telefone = null,
                                                        int? classificacaoClienteId = null,
                                                        int? AREA_ID = null,
                                                        int? CMP_ID = null,
                                                        int? origemId = null,
                                                        bool pesquisaCpfCnpjPorIqualdade = true,            
                                                        int pagina = 1,
                                                        int registroPorPagina = 7)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    var representanteId = SessionContext.GetIdRepresentante();

                    var uenId = SessionUtil.GetUenId();
                    var lstClientes = _service.BuscarClientes(new PesquisaClienteDTO(){
                                                               cpf_cnpj = cpf_cnpj,
                                                               nome = nome,
                                                               uen_id = uenId,
                                                               REP_ID = representanteId,
                                                               email = email,
                                                               dddTelefone = dddTelefone,
                                                               telefone = telefone,
                                                               classificacaoClienteId = classificacaoClienteId,
                                                               AREA_ID = AREA_ID,
                                                               CMP_ID = CMP_ID,
                                                               pesquisaCpfCnpjPorIqualdade = pesquisaCpfCnpjPorIqualdade,
                                                               origemId = origemId,
                                                               pagina = pagina,
                                                               registroPorPagina = registroPorPagina});

                    //var lstClientes = _service.Clientes(cnpj, nome, pagina, registroPorPagina, representanteId, UEN_ID, classificacaoClienteId);
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
                response.message = Message.Fail(SessionUtil.RecursiveShowExceptionsMessage(e));
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarAssinaturas(int? cliId = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var lstAssinatura = _serviceAss.FindAssinaturaPorCliente((int) cliId);
                response.Add("lstAssinatura", lstAssinatura);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarEmailDasAssinaturas(string asnNumAssinatura = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var lstEmails = _assinaturaEmailSRV.FindByNumAssinatura(asnNumAssinatura);
                response.Add("lstEmails", lstEmails);
                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult ListarTelefoneDasAssinaturas(string asnNumAssinatura = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstTelefone = _assinaturaTelefoneSRV.FindByNumAssinatura(asnNumAssinatura);
                response.Add("lstTelefone", lstTelefone);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AutorizarCustomAttribute("FranquiadoOuGerente", IsAjax = true)]
        public ActionResult ImportarClienteParaAgenda(ImportarClienteAgendaDTO importDTO)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    string usuario = SessionContext.login;
                    int? REP_ID = null;
                    if (AuthUtil.TryGetRepId(out REP_ID))
                    {
                        if (SessionContext.IsGerenteDepartamentoOR(true, "TI", "FRANQUIADOR"))
                        {
                            importDTO.REP_ID_DEMANDANTE = REP_ID;

                            if (importDTO.RG_ID == null)
                            {
                                throw new NegocioException("É necessário escolher uma região para realizar o rodizio.");
                            }
                        }
                        else 
                        if (SessionContext.IsGerenteDepartamento("FRANQUIADO", true))
                        {
                            var rgId = SessionUtil.GetRegiao();

                            importDTO.REP_ID_DEMANDANTE = REP_ID;
                            importDTO.RG_ID = rgId;

                        }
                        else
                        {
                            importDTO.REP_ID = REP_ID;
                            importDTO.REP_ID_DEMANDANTE = REP_ID;
                        }
                        _service.ImportarClienteAgenda(importDTO);
                        result.success = true;
                    }
                    else
                    {
                        result.success = false;
                        result.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                    }

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                }

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
        public ActionResult ListarEmailsDoCliente(int? cliId)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstEmails = _assinaturaEmailSRV.FindByCliente(cliId);
                response.Add("lstEmails", lstEmails);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ExcluirClienteDaValidacao(int? cliId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.ExcluirClienteDaValidacao(cliId);
                SysException.RegistrarLog("Cliente excluído da validação com sucesso!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Cliente excluído da validação com sucesso!!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult BuscarClienteGlobal(
            PesquisaClienteDTO pesquisaDTO)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int? repIdCart = SessionUtil.FranquiadoOuGerenteOuTI() ? null : SessionContext.GetIdRepresentante();
                pesquisaDTO.buscarCarteiraAssinatara = true;
                pesquisaDTO.BuscarTodos = true;

                if(pesquisaDTO.uenLogada == true)
                {
                    pesquisaDTO.BuscarTodos = false;
                    pesquisaDTO.uen_id = SessionUtil.GetUenId();
                }

                var lstClientes = _service.BuscarClientes(pesquisaDTO);
                    response.AddPage("clientes", lstClientes);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarTiposCliente()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstTipoCliente = _tipoDeCliente.BuscarTiposDeClientesAtivos();
                response.Add("lstTipoCliente", lstTipoCliente);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ChecarCliente(AssinaturaEmailDTO email)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.ChecarEmailJaCadastrado(email.AEM_EMAIL);          
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                }

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
        public ActionResult ChecarInadimplencia(int? cliId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var validacao = _service.ExecutarValidacaoDeInadimplencia(cliId, 1, null);
                result.Add("ValidacaoInadimplencia", validacao);
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

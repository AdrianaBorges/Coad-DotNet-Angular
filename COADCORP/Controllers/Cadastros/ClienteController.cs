using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.PORTAL.Service.Uras;
using COAD.SEGURANCA.Exceptions;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericCrud.Service;
using COAD.CORPORATIVO.LEGADO.Service.CorporativoAntigo;
using COAD.CORPORATIVO.Service.Boleto;
using COAD.SEGURANCA.Config.Email;
using COAD.COBRANCA.Bancos.Service;
using COAD.COBRANCA.Bancos.Model.DTO;
using GenericCrud.Util;
using COAD.FISCAL.Service.Integracoes;

namespace COADCORP.Controllers.Cadastros
{
    public class ClienteController : Controller
    {
        public AssinaturaSRV _serviceAss { get; set; }
        public ClienteSRV _service { get; set; }
        public ProspectSRV _prospectSRV { get; set; }
        public TipoDeClienteSRVProxy _tipoDeCliente { get; set; }
        public AgendaSRV _agenda { get; set; }
        public HistAtendSRV _atendimento { get; set; }
        public HistAtendUraSRV _atendimentoUra { get; set; }
        public HistAtendEmailSRV _atendimentoEmail { get; set; }
        public OpcaoAtendimentoSRV _setor { get; set; }
        public TipoTelefoneSRV _tipotelefone { get; set; }
        public TipoAtendimentoSRV _tipoAtendSRV { get; set; }
        public AcaoAtendimentoSRV _acaoAtendSRV { get; set; }
        public LinhaProdutoInformativoSRV _lproinfSRV { get; set; }
        public ChequeDevolvidoSRV _chDevolvido { get; set; }
        public AssinaturaEmailSRV _assEmailSRV { get; set; }
        public AssinaturaTelefoneSRV _assTelefoneSRV { get; set; }
        public ClienteEnderecoSRV _cliEnderecoSRV { get; set; }
        public CarteiraRepresentanteSRV _CartRepSRV { get; set; }
        public ParcelasSRV _parcelaSRV { get; set; }

        private EmpresaSRV _serviceEmpresa = new EmpresaSRV();
        private ContaSRV _serviceConta = new ContaSRV();
        private ClienteEnderecoSRV _serviceClienteEndereco = new ClienteEnderecoSRV();


        [Autorizar(IsAjax = true)]
        public ActionResult Clientes(int cliente = 0, string contrato = null, string pedido = null, string assinatura = null, string cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 20, Boolean somenteativos=false )
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var lstClientes = _service.ListaClientesContrato(cliente, contrato, pedido, assinatura, cnpj, nome, pagina, registroPorPagina, somenteativos);

                if (lstClientes.lista.Count() > 0)
                {
                    response.AddPage("clientes", lstClientes);
                    response.success = true;
                }
                else
                {
                    throw new Exception("Nenhum registro encontrado para a consulta selecionada.");
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
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        private void _preencherCombos()
        {
            List<SelectListItem> ListaMeses = new List<SelectListItem>();
            List<SelectListItem> ListaAno = new List<SelectListItem>();

            ListaMeses.AddRange(new[]{
                            new SelectListItem() { Text = "Janeiro", Value = "1"},
                            new SelectListItem() { Text = "Fevereiro", Value = "2" },
                            new SelectListItem() { Text = "Março", Value = "3" },
                            new SelectListItem() { Text = "Abril", Value = "4" },
                            new SelectListItem() { Text = "Maio", Value = "5" },
                            new SelectListItem() { Text = "Junho", Value = "6" },
                            new SelectListItem() { Text = "Julho", Value = "7" },
                            new SelectListItem() { Text = "Agosto", Value = "8" },
                            new SelectListItem() { Text = "Setembro", Value = "9" },
                            new SelectListItem() { Text = "Outubro", Value = "10" },
                            new SelectListItem() { Text = "Novembro", Value = "11" },
                            new SelectListItem() { Text = "Dezembro", Value = "12" }
            });


            ViewBag.ListaMes = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.DataAtual = DateTime.Now;

            var _SetorTelefones = _setor.BuscarSetorDeTelefones().ToList();
            var _SetorEmails = _setor.BuscarSetorDeEmails().ToList();
            var _tipotel = _tipotelefone.BuscarTodos().ToList();
            var _tipoend = new TipoEnderecoSRV().FindAll().ToList();

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
                            new SelectListItem() { Text = "Orgão Publico",   Value = "G" },
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

            _tipoDeCliente = new TipoDeClienteSRVProxy();

            ViewBag.ufs = ufs;
            ViewBag.tiposDeCliente = _tipoDeCliente.RetornarTiposDeCliente(0);
            ViewBag.tipoLogradouro = _tipoLogradouro;
            ViewBag.tipoEndereco = tipoEndereco;
            ViewBag.ListaPasta = new SelectList(ListaPasta, "Value", "Text");
            ViewBag.ListaTipoPessoa = new SelectList(ListaTipoPessoa, "Value", "Text");
            ViewBag.ListaMotivoTroca = new SelectList(ListaMotivoTroca, "Value", "Text");
            ViewBag.ListaTipoAtendimento = new SelectList(ListaTipoAtendimento, "TIP_ATEND_ID", "TIP_ATEND_DESCRICAO");
            ViewBag.ListaAcaoAtendimento = new SelectList(ListaAcaoAtendimento, "ACA_ID", "ACA_DESCRICAO");
            ViewBag.ListaUf = new SelectList(new UFSRV().BuscarSomenteUF(), "UF_SIGLA", "UF_DESCRICAO");

            ViewBag.ListaClassificacao = new SelectList(ListaClassificacao, "CLA_ID", "CLA_DESCRICAO");
            ViewBag.ListaUras = new SelectList(ListaUras, "URA_ID ", "URA_ID ");
            ViewBag.ListaSetorTelefone = new SelectList(_SetorTelefones, "OPC_ID", "OPC_DESCRICAO");
            ViewBag.ListaSetorEmail = new SelectList(_SetorEmails, "OPC_ID", "OPC_DESCRICAO");
            ViewBag.Listatipotel = new SelectList(_tipotel, "TIPO_TEL_ID", "TIPO_TEL_DESCRICAO");
            ViewBag.ListaTipoLogradouro = new SelectList(_tipoLogradouro, "TIPO_LOG_ID", "TIPO_LOG_DESCRICAO");
            ViewBag.ListaTipoEndereco = new SelectList(_tipoend, "TP_END_ID", "TP_END_DESCRICAO");
        }
        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            return View();
        }
        [Autorizar(Acesso = "Editar")]
        public ActionResult Editar(int? clienteId, string assinatura)
        {
    
            ViewBag.Title = " Cliente (Editar) ";

            if (clienteId == null)
                ViewBag.Title = " Cliente (Novo)";

             _preencherCombos();

            ViewBag.assinatura = assinatura;
            ViewBag.clienteId = clienteId;

            return View();
        }

        [Autorizar(IsAjax = true, Acesso = "Incluir")]
        public ActionResult Salvar(ClienteDto _cliente, string _assinatura, List<AssinaturaTelefoneDTO> _telefones, List<AssinaturaEmailDTO> _emails)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                   
                    _service.SalvarCliente(_cliente, _assinatura); 
                    SysException.RegistrarLog("Dados do cliente atualizados com sucesso!!", "", SessionContext.autenticado);
                    
                    result.success = true;
                    result.message = Message.Info("Dados do cliente atualizados com sucesso!!");
                    
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
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true, GerenteDepartamento = "CONTROLADORIA,SAC,TI")]
        public ActionResult BuscarSequenciaProd(string _prodletra)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var _prodid = int.Parse(_prodletra.Substring(0, 2));
                var _letra = char.Parse(_prodletra.Substring(2, 1).ToUpper());
                var _mes = 0;

                switch (_letra)
                {
                    case 'A':
                        _mes = 1;
                        break;
                    case 'B':
                        _mes = 2;
                        break;
                    case 'C':
                        _mes = 3;
                        break;
                    case 'D':
                        _mes = 4;
                        break;
                    case 'E':
                        _mes = 5;
                        break;
                    case 'F':
                        _mes = 6;
                        break;
                    case 'G':
                        _mes = 7;
                        break;
                    case 'H':
                        _mes = 8;
                        break;
                    case 'I':
                        _mes = 9;
                        break;
                    case 'J':
                        _mes = 10;
                        break;
                    case 'K':
                        _mes = 11;
                        break;
                    case 'L':
                        _mes = 12;
                        break;
                    case 'M':
                        _mes = 12;
                        break;
                }

                if (_mes == 0)
                    throw new Exception("Sequência não encotrada para o produto selecionado!! Verifique o Produto e Letra !!");

                var novaassinatura = new  AssinaturaSRV().GerarCodAssinatura(_prodid, _mes);

                if (novaassinatura == null)
                    throw new Exception("Sequência não encotrada para o produto selecionado!! Verifique o Produto e Letra !!");

                result.Add("novaassinatura", novaassinatura);
                result.success = true;
                result.message = Message.Info("Realizada com sucesso !!");

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
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [Autorizar(IsAjax = true)]
        public ActionResult BuscarUltContrato(string _assinatura)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                
                var ultimocontrato = new ContratoSRV().BuscarUltimoContrato(_assinatura,false);

                if (ultimocontrato == null)
                    throw new Exception("Não existem contratos disponíveis para transferência nesta assinatura !");

                result.Add("ultimocontrato", ultimocontrato);
                result.success = true;
                result.message = Message.Info("ok !!");

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
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        

        [Autorizar(IsAjax = true, GerenteDepartamento = "CONTROLADORIA,SAC,TI")]
        public ActionResult TransferirVigencia(TransfAssinaturaCustomDTO _transf)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    if (_transf.SOLICITANTE == null || _transf.SOLICITANTE == "")
                        _transf.SOLICITANTE = SessionContext.autenticado.USU_LOGIN;

                    _transf.MOTIVO = _transf.MOTIVO + "  Transferêcia de vigência / Produto (" + _transf.ASN_NUM_ASSINATURA_ANT + " >>> " + _transf.ASN_NUM_ASSINATURA + ")";
                    _transf.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    _transf.DATA_TRANSF = DateTime.Now.Day.ToString()+"/"+ DateTime.Now.Month.ToString() + "/"+DateTime.Now.Year.ToString();

                    new AssinaturaSRV().TrasferirVigencia(_transf);
                    SysException.RegistrarLog("Transferêcia realizada com sucesso !! (" + _transf.ASN_NUM_ASSINATURA_ANT + " >>> " + _transf.ASN_NUM_ASSINATURA + ")", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Transferêcia realizada com sucesso !!");

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
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true, Acesso = "Incluir")]
        [HttpPost]
        public ActionResult SalvarEmail(AssinaturaEmailDTO _email)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarEmail(_email);
                    SysException.RegistrarLog("Dados do cliente atualizados com sucesso!! ("+_email.AEM_EMAIL+")", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do cliente atualizados com sucesso!!");

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
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true, Acesso = "Incluir")]
        [HttpPost]
        public ActionResult SalvarTelefone(AssinaturaTelefoneDTO _telefone)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    var _tel = _telefone.ATE_DDD + " - " +_telefone.ATE_TELEFONE;
                    _service.SalvarTelefone(_telefone);
                    SysException.RegistrarLog("Telefone atualizado com sucesso!! (" + _tel + ")", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do cliente atualizados com sucesso!!");

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
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        
        [Autorizar(IsAjax = true, GerenteDepartamento = "CONTROLADORIA,SAC,TI")]
        public ActionResult ExcluirEndereco(ClienteEnderecoDto _endereco)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                
                new ClienteSRV().ExcluirEndereco(_endereco);
                SysException.RegistrarLog("Endereço excluído com sucesso!! (" + _endereco.END_LOGRADOURO + ")", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Endereço excluído com sucesso!!");

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
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true, Acesso = "Excluir")]
        [HttpPost]
        public ActionResult ExcluirEmail(AssinaturaEmailDTO _email)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    new ClienteSRV().ExcluirEmail(_email);
                    SysException.RegistrarLog("Email excluído com sucesso!! (" + _email.AEM_EMAIL + ")", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Email excluído com sucesso!!");

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
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true, Acesso = "Excluir")]
        [HttpPost]
        public ActionResult ExcluirTelefone(AssinaturaTelefoneDTO _telefone)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    var _tel = _telefone.ATE_DDD + " - " + _telefone.ATE_TELEFONE;
                    new ClienteSRV().ExcluirTelefone(_telefone);
                    SysException.RegistrarLog("Telefone excluído com sucesso!! (" + _tel + ")", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Telefone excluído com sucesso!! (" + _tel + ")");

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
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }




        [Autorizar(IsAjax = true)]
        public ActionResult ClientesPorRepresentante(string cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 7, int? uen_id = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    var representanteId = SessionContext.GetIdRepresentante();
                    var lstClientes = _service.Clientes(cnpj, nome, pagina, registroPorPagina, representanteId, uen_id);
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
        public ActionResult BuscarSituacaoCliente(string _asn_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var SituacaoCliente = _service.BuscarSituacaoCliente(_asn_id);

                response.success = true;
                response.Add("SituacaoCliente", SituacaoCliente);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
 
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public JsonResult RecuperarDadosDoCliente(Int32 clienteId)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var cliente = _service.FindByIdFullLoaded(clienteId);
                var listaatend = _atendimento.BuscarPorCliente(clienteId);
                response.Add("cliente", cliente);
                response.Add("listaatend", listaatend);
                response.success = true;

                ///--- Busca do corporativo antigo.
                var listaagenda = _agenda.ListarPorAssinatura(clienteId.ToString());
                response.Add("listaagenda", listaagenda);
                ///--------------------------------


            }
            catch (Exception e)
            {
                
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public JsonResult BuscarUsuarioLogado()
        {
            JSONResponse response = new JSONResponse();

            response.Add("USU_LOGIN", SessionContext.autenticado.USU_LOGIN);
            response.Add("DATA_ALTERA", DateTime.Now);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
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
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarLiqParcelas(string _numparcela, int pagina = 1, int registroPorPagina = 7)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_numparcela != null && _numparcela != "")
                {
                    var listaliqparcelas = new ParcelaLiquidacaoSRV().BuscarPorParcela(_numparcela, pagina, registroPorPagina);
                    response.AddPage("listaliqparcelas", listaliqparcelas);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"Não foi possivel consultar os documentos de liquidação relacionados a este contrato.");
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
        public ActionResult BuscarCHDevolvido(string _numassinatura)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_numassinatura != null && _numassinatura != "")
                {
                    var listachdevolvido = new ParcelasSRV().BuscarPorContrato(_numassinatura);
                    response.Add("listachdevolvido", listachdevolvido);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"Não foi possivel consultar as cheques devolvidos relacionadas a este contrato.");
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
        public ActionResult GerarNovaSenha(string _assinatura, int _cli_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_assinatura != null && _assinatura != "")
                {
                    _serviceAss.GerarSenha(_assinatura, _cli_id);
                    response.success = true;
                    response.message = Message.Fail(@"Senha gerada com sucesso!");
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"Não foi possível gerar uma nova senha para este cliente.");
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
        public ActionResult BuscarTelefones(string _assinatura, int pagina = 1, int registroPorPagina = 10)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                TelefonesDeClienteSRV _srvTelefone = new TelefonesDeClienteSRV();
                EmailsDeClienteSRV _srvEmail = new EmailsDeClienteSRV();
                
                var listaemail = _srvEmail.BuscarEmailsDeCliente(_assinatura);
                var listatelefone = _srvTelefone.BuscarTelefonesDeClientePorAssinatura(_assinatura, pagina, registroPorPagina);

                response.Add("listaemail", listaemail);
                response.AddPage("listatelefone", listatelefone);

                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarEmailsNF(string _contrato, int _cli_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                EmailsDeClienteSRV _srvEmail = new EmailsDeClienteSRV();
                var listaemail = _srvEmail.BuscarEmailsContrato(_contrato, _cli_id);
                response.Add("listaemail", listaemail);
                response.success = true;
                response.message = Message.Info("Ok");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarEmails(string _assinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                EmailsDeClienteSRV _srvEmail = new EmailsDeClienteSRV();
                var listaemail = _srvEmail.BuscarEmailsDeCliente(_assinatura);
                response.Add("listaemail", listaemail);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuscarEmailPorBoleto(string _parcela)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                EmailsDeClienteSRV _srvEmail = new EmailsDeClienteSRV();
                var listaemail = _srvEmail.BuscarEmailPorBoleto(_parcela);
                response.Add("listaemail", listaemail);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarContratosAssinatura(string _assinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                ContratoSRV _srvContrato = new ContratoSRV();

                var listacontrato = _srvContrato.BuscarPorAssinatura(_assinatura);

                response.Add("listacontrato", listacontrato);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        [Autorizar(IsAjax = true)]
        public ActionResult PreencherGrids(string _assinatura, int pagina = 1, int registroPorPagina = 20)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                TelefonesDeClienteSRV _srvTelefone = new TelefonesDeClienteSRV(); 
                ContratoSRV _srvContrato = new ContratoSRV();
                EmailsDeClienteSRV _srvEmail = new EmailsDeClienteSRV();
                //var listaatend = _atendimento.BuscarPorAssinatura(_assinatura);
                var listaemail = _srvEmail.BuscarEmailsDeCliente(_assinatura);
                var listacontrato = _srvContrato.BuscarPorAssinatura(_assinatura);
                var listatelefone = _srvTelefone.BuscarTelefonesDeClientePorAssinatura(_assinatura, pagina, registroPorPagina);
                var listaQtdeConsEmail = _serviceAss.ConsultasPorPeriodo(_assinatura);
                var listaChequeDevolvido = _chDevolvido.BuscarPorAssinatura(_assinatura);
                var AssinaturaSenha = new AssinaturaSenhaSRV().BuscarSenhaAtiva(_assinatura);
                var listaRepresentante = _CartRepSRV.BuscarCarteiraAssinatura(_assinatura);
                var _ass = new AssinaturaSRV().FindByIdFullLoaded(_assinatura);

                if (_ass != null)
                    _ass.ASN_ATIVA = new ClienteSRV().VerificarAssinaturaAtiva(_assinatura);

                response.Add("assinatura", _ass);
                response.Add("listaChequeDevolvido", listaChequeDevolvido);
                response.Add("listaQtdeConsEmail", listaQtdeConsEmail);
                response.Add("listacontrato", listacontrato);
                response.Add("listaemail", listaemail);
                //response.Add("listaatend", listaatend);
                response.Add("listaRepresentante", listaRepresentante);
                response.Add("AssinaturaSenha", AssinaturaSenha);
                response.AddPage("listatelefone", listatelefone);
 
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarApuracaoConsulta(string _assinatura)
        {

            JSONResponse response = new JSONResponse();

            try
            {

                var listaQtdeConsEmail = _serviceAss.ConsultasPorPeriodo(_assinatura);
                var AssinaturaSenha = new AssinaturaSenhaSRV().BuscarSenhaAtiva(_assinatura);

                response.Add("listaQtdeConsEmail", listaQtdeConsEmail);
                response.Add("AssinaturaSenha", AssinaturaSenha);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarListaApuracaoConsulta(string _assinatura)
        {

            JSONResponse response = new JSONResponse();
 
            try
            {
                var _contrato = new ContratoSRV().BuscarUltimoContrato(_assinatura);
                    
                var listaQtdeConsEmail = _serviceAss.ConsultasPorPeriodo(_assinatura, _contrato);

                response.Add("listaQtdeConsEmail", listaQtdeConsEmail);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarHistorico(int _cli_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                ///--- Busca do corporativo antigo.
                var listaagenda = _agenda.ListarPorAssinatura(_cli_id.ToString());
                ///--------------------------------

                var listaatend = _atendimento.BuscarPorCliente(_cli_id);

                response.Add("listaagenda", listaagenda);
                response.Add("listaatend", listaatend);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
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
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarDetConsultaUra(string _asn_id, string _ura_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var detconsultas = _atendimentoUra.BuscarPorPeriodo(_asn_id,_ura_id);
                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("detconsultas", detconsultas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)] 
        public ActionResult BuscarDetConsultaEmail(string _asn_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var detconsultas = _atendimentoEmail.BuscarPorPeriodo(_asn_id);

                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("detconsultas", detconsultas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarHistAgenda(string _asn_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listaagenda = _agenda.ListarPorAssinatura(_asn_id);

                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("listaagenda", listaagenda);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true, GerenteDepartamento = "CONTROLADORIA,SAC,TI")]
        public ActionResult AlterarEndereco()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true, GerenteDepartamento = "SAC,TI")]
        public ActionResult AtualizarAssinatura(AssinaturaDTO _assinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                new AssinaturaSRV().SalvarAssinatura(_assinatura);

                response.success = true;
                response.message = Message.Info("Atualização das Assinaturas realizada com sucesso!");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true, GerenteDepartamento = "SAC,TI")]
        public ActionResult AtualizarAssinaturaURA(AssinaturaDTO _assinatura, Boolean? _ativo)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                new AssinaturaSRV().SalvarAssinatura(_assinatura);

                int? _qtde = _assinatura.ASN_QTDE_CONS_CONTRATO;

                if (_assinatura.ASN_QTDE_CONS_CONTRATO > 0)
                    new ClienteSRV().AtualizarUra(_assinatura.ASN_NUM_ASSINATURA, _qtde, _ativo);

                response.success = true;
                response.message = Message.Info("Atualização das Assinaturas realizada com sucesso!");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
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
                _histAtend.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
       //         _histAtend.HAT_SOLICITANTE = SessionContext.autenticado.USU_LOGIN;
                _histAtend.UEN_ID = 3;
           
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
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
          
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarEmail(string _email)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (_email.Length == 0)
                    throw new Exception("Nenhum registro encontrado.");

                IList<AssinaturaEmailDTO> _lstemail = _assEmailSRV.BuscarEmails(_email);

                result.success = true;
                result.Add("lstemail", _lstemail);

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
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarTelefone(string _telefone)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (_telefone.Length == 0)
                    throw new Exception("Nenhum registro encontrado.");

                IList<AssinaturaTelefoneDTO> _lsttelefone = _assTelefoneSRV.BuscarTelefone(_telefone);

                result.success = true;
                result.Add("lsttelefone", _lsttelefone);
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
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarLogradouro(string _logradouro)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (_logradouro.Length == 0)
                    throw new Exception("Nenhum registro encontrado.");

                List<PesquisaEnderecoDTO> _lstlogradouro = _cliEnderecoSRV.BuscarEndereco(_logradouro);

                result.success = true;
                result.Add("lstlogradouro", _lstlogradouro);
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
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [Autorizar(IsAjax = true)]
        public ActionResult BuscarTipoAtendimento(string _grupo = null, int _classificacao = 0)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                var lstTipoAtendimento = _tipoAtendSRV.BuscarTipoAtendimento(_grupo, _classificacao);

                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("lstTipoAtendimento", lstTipoAtendimento);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult VerificarEtiqueta(int _id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                TipoAtendimentoSRV _srvTipo = new TipoAtendimentoSRV();

                var _tipo = _srvTipo.FindById(_id);

                response.Add("TipoAtendimento", _id);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GerarBoleto(string hashkey)
        {
            byte[] fileBytes = ServiceFactory.RetornarServico<ItemPedidoSRV>().GerarPDFDoBoleto(hashkey);

            string fileName = string.Format(@"boleto_{0:yyyy-MM-ddTH-mm}.pdf", DateTime.Now);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        }

        public ActionResult GerarBoletoProposta(string hashkey)
        {
            string path = Server.MapPath("~/");
            string filePath = _parcelaSRV.GerarPDFDoBoleto(hashkey, path);
            

            string fileName = string.Format(@"boleto_{0:yyyy-MM-ddTH-mm}.pdf", DateTime.Now);
            return File(filePath, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        }

        public ActionResult ConsultarCadastroSefaz(string cpfCnpj, bool pessoaFisica = false)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<IntegrNfeSRV>().ConsultarCadastro(cpfCnpj, null, pessoaFisica);
                //result.Add("retornoLote", resp);
                result.success = true;
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

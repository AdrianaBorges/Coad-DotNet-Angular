using System;
using System.Web;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using Coad.GenericCrud.Repositorios.Base;
using COAD.UTIL.Grafico;
using Coad.GenericCrud.ActionResultTools;
//using System.Web.UI.DataVisualization.Charting;
using FusionCharts.Charts;
using COAD.SEGURANCA.Util;
using RTE;
using COAD.COADGED.Service;
using COAD.COADGED.Model.DTO;
using COAD.CORPORATIVO.SessionUtils;
using GenericCrud.Util;
using GenericCrud.Service;
using COAD.SEGURANCA.Service.Interfaces;
using COAD.SEGURANCA.Config.Email;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using COAD.CORPORATIVO.Service.Boleto;

namespace COADCORP.Controllers
{
    public class HomeController : Controller
    {
        private CnabSRV _serviceCnab = new CnabSRV();
        private BoletoSRV _serviceBoleto = new BoletoSRV();
        public PropostaSRV _proposta { get; set; }
        HistAtendUraSRV _srvUra = new HistAtendUraSRV();
        private Editor editor = new Editor(System.Web.HttpContext.Current, "editor");
        LogSimuladorSRV _srvLogSimulador = new LogSimuladorSRV();
        TabDinamicaConfigSRV _tabDinConfigSRV = new TabDinamicaConfigSRV();
        [Autorizar(PorMenu = false)]
        public ActionResult Index()
        {
            string _home_perfil = SessionContext.GetHomePerfil();

            this.CarregarCombo();

            if (_home_perfil != null && !string.IsNullOrWhiteSpace(_home_perfil))
                return Redirect(_home_perfil);
            else
                return View();
        }
        [Autorizar(PorMenu = false)]
        public ActionResult UsuariosLogados()
        {
            return View();
        }
        [Autorizar(PorMenu = false)]
        public ActionResult DeslogarUsuario(string _session_id)
        {
            JSONResponse _resultado = new JSONResponse();

            try
            {
                SessionContext.RemoveSessionGlobal(System.Web.HttpContext.Current, _session_id);

                _resultado.message = Message.Info("Usuário removido com sucesso!!");
                _resultado.success = true;

                return Json(_resultado, JsonRequestBehavior.AllowGet);
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

                _resultado.success = false;
                _resultado.message = Message.Fail(ex);
                return Json(_resultado, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult BuscarUsuariosLogados()
        {
            JSONResponse _resultado = new JSONResponse();

            try
            {
                var autenticado = SessionContext.autenticadoGlobal;

                _resultado.Add("listaautenticado", autenticado);
                _resultado.success = true;

                return Json(_resultado, JsonRequestBehavior.AllowGet);
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

                _resultado.success = false;
                _resultado.message = Message.Fail(ex);
                return Json(_resultado, JsonRequestBehavior.AllowGet);
            }
        }
        [Autorizar(PorMenu = false)]
        public ActionResult MudaPerfil(string _per_id)
        {
            this.CarregarCombo();

            SessionContext.autenticado.PER_ID = _per_id;
            SessionContext.menu_usuario = new UsuarioSRV().MontaMenu(SessionContext.autenticado.PER_ID, SessionContext.autenticado.EMP_ID, SessionContext.autenticado.SIS_ID, SessionContext.autenticado.ADMIN);


            int? REP_ID = null;
            if (SessionContext.HasDepartamento("Franquiado"))
            {
                if (AuthUtil.TryGetRepId(out REP_ID) && !SessionContext.IsGerenteDepartamento("Franquiado"))
                {
                    new RepresentanteSRV().ChecaEInsereFilaRepresentante(REP_ID);
                }
            }

            string _home_perfil = SessionContext.GetHomePerfil();
            if (_home_perfil != null && !string.IsNullOrWhiteSpace(_home_perfil))
                return Redirect(_home_perfil);
            else
                return View("Index");

        }
        [Autorizar(PorMenu = false)]
        public ActionResult TestaEditor()
        {
            return View();
        }
        [Autorizar(PorMenu = false)]
        public ActionResult Dashboard()
        {
            string _home_perfil = SessionContext.GetHomePerfil();

            this.CarregarCombo();

            return View();
        }
        [Autorizar(PorMenu = false)]
        public ActionResult DashboardVendas()
        {
            string _home_perfil = SessionContext.GetHomePerfil();

            this.CarregarCombo();

            return View();
        }
        [Autorizar(PorMenu = false)]
        public ActionResult DashboardPagamentos()
        {
            string _home_perfil = SessionContext.GetHomePerfil();

            this.CarregarCombo();

            return View();
        }
        [Autorizar(PorMenu = false)]
        public ActionResult DashboardRepresentante()
        {

            var _rep = new RepresentanteDTO();

            if (SessionContext.rep_id != null)
            {
                //_rep = new RepresentanteSRV().FindById(2734);
                _rep = new RepresentanteSRV().FindById(SessionContext.rep_id);

                ViewBag.REP_ID = _rep.REP_ID;
                ViewBag.REP_NOME = _rep.REP_NOME;
            }

            string _home_perfil = SessionContext.GetHomePerfil();

            this.CarregarCombo();

            return View();
        }


        [Autorizar(PorMenu = false)]
        public ActionResult Teste()
        {
            return View();
        }
        [Autorizar(PorMenu = false)]
        public JsonGraficoResponse ConsultarURAPorUF(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();

            try
            {
                _resultado.Titulo = "Consultas Por UF";
                _resultado.Descricao = "Apuração do Mês ( " + _dtini.Value.Month.ToString() + "/" + _dtini.Value.Year.ToString() + " )";
                _resultado.Dados = _srvUra.BuscarTotalPorUF(_dtini, _dtfim).ToList();

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Autorizar(IsAjax = true)]
        public JsonGraficoResponse ConsultarUraRamal(string _ura_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();

            try
            {
                _resultado.Titulo = "Consultas Por RAMAL";
                _resultado.Descricao = "Apuração do Mês ( " + _dtini.Value.Month.ToString() + "/" + _dtini.Value.Year.ToString() + " )";
                _resultado.Dados = _srvUra.BuscarTotalPorRamal(_ura_id, _dtini, _dtfim).ToList();

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Autorizar(IsAjax = true)]
        public JsonGraficoResponse ConsultarTabelasPorGrupo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _Tipo = null)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();

            try
            {

                DateTime _dataini = new DateTime(_dtini.Value.Year, _dtini.Value.Month, _dtini.Value.Day);
                DateTime _datafim;


                if (_Tipo == "M")
                {
                    _datafim = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, _dtfim.Value.Day);
                    _resultado.Titulo = "Tabelas Acessadas P/ Grupo ( Mensal )";
                    _resultado.Descricao = "Apuração do Mês ( " + _dtini.Value.Month.ToString() + "/" + _dtini.Value.Year.ToString() + " )";
                    _resultado.Dados = _srvLogSimulador.BuscarTabelasPorGrupo(_dataini, _datafim).ToList();

                }
                else
                {
                    _datafim = new DateTime(_dtfim.Value.Year, 12, _dtfim.Value.Day);
                    _resultado.Titulo = "Tabelas Acessadas P/ Grupo ( Anual )";
                    _resultado.Descricao = "Apuração do Ano ( " + _datafim.Year.ToString() + " )";
                    _resultado.Dados = _srvLogSimulador.BuscarTabelasPorGrupo(_dataini.Date.Year).ToList();

                }



                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Autorizar(IsAjax = true)]
        public JsonGraficoResponse ConsultarTabelasPorPeriodo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, int _grupo = 0)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();

            try
            {
                _resultado.Titulo = "Tabelas Acessadas Por Período";
                _resultado.Descricao = "Apuração do Mês ( " + _dtini.Value.Month.ToString() + "/" + _dtini.Value.Year.ToString() + " )";
                _resultado.Dados = _srvLogSimulador.BuscarTabelasPorPeriodo(_dtini, _dtfim, _grupo).ToList();

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Autorizar(IsAjax = true)]
        public JsonGraficoResponse ConsultarTotalPorProduto(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();

            try
            {
                _resultado.Titulo = "Consultas Por Produto";
                _resultado.Descricao = "Apuração do Mês ( " + _dtini.Value.Month.ToString() + "/" + _dtini.Value.Year.ToString() + " )";
                _resultado.Dados = _srvUra.BuscarTotalPorProduto(_dtini, _dtfim).ToList();

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Autorizar(IsAjax = true)]
        public JsonGraficoResponse ConsultarSimuladorUFMesCalc(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _tdc_id = null)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();

            try
            {
                _resultado.Titulo = "Calculos realizados por clientes (Por UF)";
                _resultado.Descricao = "Apuração Ano ( " + _dtini.Value.Year.ToString() + " )";
                _resultado.Dados = _srvLogSimulador.BuscarTotalPorUFCalc(_dtini, _dtfim, "C", _tdc_id).ToList();

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Autorizar(IsAjax = true)]
        public JsonGraficoResponse ConsultarSimuladorD(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _tdc_id = null)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();

            try
            {

                _resultado.Titulo = "Cálculos realizados no simulador (Diário)";
                _resultado.Descricao = "Apuração do Período ( " + _dtini.Value.Month.ToString() + "/" +
                                                                  _dtini.Value.Year.ToString() + " )";
                _resultado.Dados = _srvLogSimulador.BuscarTotalPorDia(_dtini, _dtfim, "C", _tdc_id).ToList();

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Autorizar(IsAjax = true)]
        public JsonGraficoResponse ConsultarSimuladorH(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _tdc_id = null)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();

            try
            {
                _resultado.Titulo = "Cálculos realizados no simulador ( P/ Hora )";
                _resultado.Descricao = "Apuração do Período ( " + _dtini.Value.Day.ToString() + "/" + _dtini.Value.Month.ToString() + "/" + _dtini.Value.Year.ToString() + " )";
                _resultado.Dados = _srvLogSimulador.BuscarTotalPorHora(_dtini, _dtfim, "C", _tdc_id).ToList();

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void CarregarCombo()
        {

            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            IList<TabDinamicaGrupoDTO> ListaTabGrupo = new TabDinamicaGrupoSRV().FindAll();
            IList<TabDinamicaConfigDTO> ListaTabRef = _tabDinConfigSRV.ListarTabDinamica(null, null, 2);
            List<SelectListItem> ListaMeses = new List<SelectListItem>();
            List<SelectListItem> ListaAno = new List<SelectListItem>();
            IList<GrupoDTO> grupos = new GrupoSRV().FindAll();
            var ufs = new UFSRV().FindAll();

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

            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
            ViewBag.grupos = grupos;
            ViewBag.ListaRegiao = new SelectList(ufs.ToList(), "UF_SIGLA", "UF_DESCRICAO");
            ViewBag.ListaMes = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.ListaTabRef = new SelectList(ListaTabRef, "TDC_ID", "TDC_NOME_TABELA");
            ViewBag.ListaTabGrupo = new SelectList(ListaTabGrupo, "TGR_ID", "TGR_DESCRICAO");
            ViewBag.DataAtual = DateTime.Now;

        }
        [Autorizar(PorMenu = false)]
        public ActionResult Sac()
        {
            return View();
        }
        public ActionResult Benvindo()
        {
            return View();
        }
        public ActionResult Sobre()
        {
            return View();
        }
        public ActionResult Error()
        {
            JSONResponse response = new JSONResponse()
            {
                message = Message.Fail(TempData["message"] as string)
            };
            response.success = false;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Info(string type)
        {
            if (type != "" && type != null)
                TempData["message"] = type;

            return View();
        }
        public ActionResult Falha(string type)
        {
            if (type != "" && type != null)
                TempData["message"] = type;

            return View();
        }
        public ActionResult Erro(string type)
        {
            // TempData["message"] = type;

            return View();
        }
        [Autorizar(PorMenu = false)]
        public ActionResult CarregarGrafico(string _numsimulador = null, Nullable<DateTime> _dataacesso = null, int _grupo = 0)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                //   _numsimulador = "1228f7e8-b4a0-45e3-bf67-099c7fda840b";

                var grafSimuladorUFMesCalc = this.ConsultarSimuladorUFMesCalc(_dataacesso, _dataacesso, _numsimulador);
                var grafSimuladorD = this.ConsultarSimuladorD(_dataacesso, _dataacesso, _numsimulador);
                var grafSimuladorH = this.ConsultarSimuladorH(_dataacesso, _dataacesso, _numsimulador);
                var grafUF = this.ConsultarURAPorUF(_dataacesso, _dataacesso);
                var grafProduto = this.ConsultarTotalPorProduto(_dataacesso, _dataacesso);
                var grafTabelas = this.ConsultarTabelasPorPeriodo(_dataacesso, _dataacesso, _grupo);
                var grafTabelasGruAno = this.ConsultarTabelasPorGrupo(_dataacesso, _dataacesso, "A");
                var grafTabelasGruMes = this.ConsultarTabelasPorGrupo(_dataacesso, _dataacesso, "M");
                var grafUraRamalRJ = this.ConsultarUraRamal("URARJ", _dataacesso, _dataacesso);
                var grafUraRamalMG = this.ConsultarUraRamal("URAMG", _dataacesso, _dataacesso);
                var grafUraRamalPR = this.ConsultarUraRamal("URAPR", _dataacesso, _dataacesso);


                response.Add("grafSimuladorUFMesCalc", grafSimuladorUFMesCalc);
                response.Add("grafSimuladorD", grafSimuladorD);
                response.Add("grafSimuladorH", grafSimuladorH);
                response.Add("grafUF", grafUF);
                response.Add("grafProduto", grafProduto);
                response.Add("grafTabelas", grafTabelas);
                response.Add("grafTabelasGruAno", grafTabelasGruAno);
                response.Add("grafTabelasGruMes", grafTabelasGruMes);
                response.Add("grafUraRamalRJ", grafUraRamalRJ);
                response.Add("grafUraRamalMG", grafUraRamalMG);
                response.Add("grafUraRamalPR", grafUraRamalPR);
                response.success = true;

                return Json(response, JsonRequestBehavior.AllowGet);
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

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ValidaPermissao(string _url, string _acesso)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                Boolean _possuiAcesso = false;

                ItemMenuPerfilModel _permissao = new ItemMenuSRV().TemAcessoFuncionalidade(SessionContext.sis_id, SessionContext.emp_id, SessionContext.autenticado.PER_ID, _url);

                if (_permissao != null)
                {
                    if (_acesso == "Acesso") _possuiAcesso = (_permissao.NIV_ACESSO > 0);
                    if (_acesso == "Editar") _possuiAcesso = (_permissao.NIV_EDIT > 0);
                    if (_acesso == "Excluir") _possuiAcesso = (_permissao.NIV_DELETE > 0);
                    if (_acesso == "Incluir") _possuiAcesso = (_permissao.NIV_INSERT > 0);
                }

                response.success = _possuiAcesso;

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(GerenteDepartamento = "TI, franquiado, franquiador", PermitirNiveisPrivilegiosSuperiores = true, IsAjax = true)]
        [ActionName("recarregar-config")]
        public ActionResult RecarregarConfig(string numeroParcela)
        {
            try
            {
                SysUtils.RecarregarConfiguracoes();
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                ViewBag.message = Message.Fail(ex);
            }

            string _home_perfil = SessionContext.GetHomePerfil();

            if (!string.IsNullOrWhiteSpace(_home_perfil))
                return Redirect(_home_perfil);
            else
                return View("Index");

        }
        public ActionResult ObterIp()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                string login = SessionContext.login;

                var teste = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                result.Add("request", teste);
                SysException.RegistrarLog("Status da proposta alterado para cancelado com sucesso!", "", SessionContext.autenticado);
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


        
        public ActionResult TestarBoleto2aViaDownload(string parId)
        {
            try
            {
                int _cta_id = 0;
                bool _avulso = false;

                var parametro = _serviceCnab.prepararParametro(parId, _cta_id, _avulso);

                if (parametro != null)
                {
                    parametro.segVia = true;
                    List<ParametroDTO> lstParametro = new List<ParametroDTO>();
                    lstParametro.Add(parametro);

                    return File(_serviceBoleto.GerarVariosBoletosPDF(lstParametro), "application/pdf", "Boletos.pdf");
                }
                else
                {
                    throw new Exception("O Cliente do título (" + parId + ") não foi localizado pelo gerador de [parâmetros] do Boleto!");
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

                ViewBag.erro = parId + " -- " + SysException.Show(ex);

                return View();

            }
        }

        public ActionResult TestarEnvioEmail(string parId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<PropostaItemSRV>().RetornarBytesDoBoleto(parId);
                
                string login = SessionContext.login;
        
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


        public ActionResult TestarEnvioFilaEmail()
        {
            JSONResponse result = new JSONResponse();
            try
            {

       
                EmailActionContainer.AddActions("emailPropostaBoleto", parId =>
                {
                    return ServiceFactory
                        .RetornarServico<PropostaItemSRV>()
                        .RetornarBytesDoBoleto(parId);
                });

                string login = SessionContext.login;
                ServiceFactory.RetornarServico<FilaEmailSRV>().ProcessarFilaDeEnvio();

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

        [Autorizar(PorMenu = false)]
        public ActionResult CarregarGraficoPagamentos(int _mes,  int _ano, int _emp_id, int _grupo_id = 0, int? _qtdeParcelas =null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _dtini = new DateTime(_ano, _mes, 1);
                var _dtfim = new DateTime(_ano, _mes, DateTime.DaysInMonth(_ano, _mes));


                var grafPagamentosPeriodo = this.BuscarContratoTipoPgto(_mes, _ano, _emp_id, _qtdeParcelas, _grupo_id);
                var grafPgtoProdutoPeriodo = this.BuscarProdutosTipoPgto(_dtini, _dtfim, _emp_id, _qtdeParcelas, _grupo_id);
                //------------
                var grafPgtoQtdeValor = new ContratoSRV().BuscarQtdeValor(_dtini, _dtfim, _emp_id, _qtdeParcelas, _grupo_id);

                grafPgtoQtdeValor.chart.caption = "Apuração ( " + _dtfim.Month.ToString() + "/" + _dtfim.Year.ToString() + " )";
                grafPgtoQtdeValor.chart.subCaption = "DEMONSTRATIVO POR FORMA DE PAGAMENTO (PARCELAS)";
                grafPgtoQtdeValor.chart.showlegend = "1";
                grafPgtoQtdeValor.chart.legendBgAlpha = "1";
                grafPgtoQtdeValor.chart.legendBorderAlpha = "1";
                grafPgtoQtdeValor.chart.legendShadow = "1";
                grafPgtoQtdeValor.chart.legendItemFontSize = "10";
                grafPgtoQtdeValor.chart.legendItemFontColor = "#666666";
                grafPgtoQtdeValor.chart.legendCaptionFontSize = "9";
                grafPgtoQtdeValor.chart.showValues = "1";
                grafPgtoQtdeValor.chart.numberPrefix = "R$ ";
                grafPgtoQtdeValor.chart.showpercentvalues = "1";
                grafPgtoQtdeValor.chart.formatNumber = "1";
                grafPgtoQtdeValor.chart.formatNumberScale = "0";
                //------------


                response.Add("grafPgtoQtdeValor", grafPgtoQtdeValor);
                response.Add("grafPagamentosPeriodo", grafPagamentosPeriodo);
                response.Add("grafPgtoProdutoPeriodo", grafPgtoProdutoPeriodo);
                response.success = true;

                return Json(response, JsonRequestBehavior.AllowGet);
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

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [Autorizar(PorMenu = false)]
        public ActionResult CarregarGraficoVendas(int _mes, int _ano, int? _emp_id, int? _rep_id, int? _grupo)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                var grafVendasPeriodo = this.BuscarVendasPeriodo(_mes, _ano, _emp_id, _grupo);
                var grafContratosPeriodo = this.BuscarContratosPeriodo(_mes, _ano, _emp_id, _rep_id, _grupo);
                var grafContratosGrupo = this.BuscarVendasGrupo(_mes,_ano, _emp_id, _grupo);
                var grafContratosRepresentante = this.BuscarVendasRepresentante(_mes, _ano, _emp_id, _grupo);
                var grafContratosVendasEvolucao = this.BuscarVendasEvolucao(_mes, _ano,_emp_id, _grupo);
                var grafContratosEvolucaoAnual = this.BuscarContratosEvolucaoAnual(_mes, _ano, _emp_id, _grupo);

                grafContratosPeriodo.chart.theme = "ocean";
                grafContratosPeriodo.chart.yaxisname = "VALOR";
                grafContratosPeriodo.chart.showlegend = "0";
                grafContratosPeriodo.chart.showpercentvalues = "0";
                grafContratosPeriodo.chart.formatNumber = "1";
                grafContratosPeriodo.chart.formatNumberScale = "0";
                grafContratosPeriodo.chart.xaxisname = "MESES";
                grafContratosPeriodo.chart.plottooltext = "Em $label foram faturados $datavalue ";
                grafContratosPeriodo.chart.captionFontSize = "14";
                grafContratosPeriodo.chart.subcaptionFontSize = "10";
                grafContratosPeriodo.chart.subcaptionFontBold = "1";
                grafContratosPeriodo.chart.numberPrefix = "R$ ";
                grafContratosPeriodo.chart.showValues = "1";
                grafContratosPeriodo.chart.showBorder = "1";
                grafContratosPeriodo.chart.showCanvasBorder = "0";

                var graficovendas = grafVendasPeriodo.DadosResumo.grafico;
                graficovendas.chart.caption = grafVendasPeriodo.Titulo;
                graficovendas.chart.subCaption = grafVendasPeriodo.Descricao;
                graficovendas.chart.showBorder = "1";
                graficovendas.chart.showCanvasBorder = "0";
                graficovendas.chart.numberPrefix = "R$ ";
                graficovendas.chart.showlegend = "1";
                graficovendas.chart.showpercentvalues = "1";
                graficovendas.chart.formatNumber = "1";
                graficovendas.chart.formatNumberScale = "0";
                graficovendas.chart.xaxisname = "DIAS";
                graficovendas.chart.plottooltext = "Dia $label foram emitidas $datavalue em propostas";
                graficovendas.chart.yaxisname = "VALOR";


                grafContratosGrupo.chart.showBorder = "1";
                grafContratosGrupo.chart.showCanvasBorder = "0";
                grafContratosGrupo.chart.numberPrefix = "R$ ";
                grafContratosGrupo.chart.showlegend = "1";
                grafContratosGrupo.chart.showpercentvalues = "1";
                grafContratosGrupo.chart.formatNumber = "1";
                grafContratosGrupo.chart.formatNumberScale = "0";

                grafContratosRepresentante.chart.showBorder = "1";
                grafContratosRepresentante.chart.showCanvasBorder = "0";
                grafContratosRepresentante.chart.numberPrefix = "R$ ";
                grafContratosRepresentante.chart.showlegend = "1";
                grafContratosRepresentante.chart.showpercentvalues = "1";
                grafContratosRepresentante.chart.formatNumber = "1";
                grafContratosRepresentante.chart.formatNumberScale = "0";

                grafContratosVendasEvolucao.chart.showBorder = "1";
                grafContratosVendasEvolucao.chart.showCanvasBorder = "0";
                grafContratosVendasEvolucao.chart.numberPrefix = "R$ ";
                grafContratosVendasEvolucao.chart.showlegend = "1";
                grafContratosVendasEvolucao.chart.showpercentvalues = "1";
                grafContratosVendasEvolucao.chart.formatNumber = "1";
                grafContratosVendasEvolucao.chart.formatNumberScale = "0";

                grafContratosEvolucaoAnual.chart.showBorder = "1";
                grafContratosEvolucaoAnual.chart.showCanvasBorder = "0";
                grafContratosEvolucaoAnual.chart.numberPrefix = "R$ ";
                grafContratosEvolucaoAnual.chart.showlegend = "1";
                grafContratosEvolucaoAnual.chart.showpercentvalues = "1";
                grafContratosEvolucaoAnual.chart.formatNumber = "1";
                grafContratosEvolucaoAnual.chart.formatNumberScale = "0";
                
                response.Add("grafContratosGrupo", grafContratosGrupo);
                response.Add("grafContratosRepresentante", grafContratosRepresentante);

                response.Add("grafContratosPeriodo", grafContratosPeriodo);
                response.Add("grafVendasPeriodo", graficovendas);
                response.Add("grafContratosVendasEvolucao", grafContratosVendasEvolucao);
                response.Add("grafContratosEvolucaoAnual", grafContratosEvolucaoAnual);

                response.Add("grafVendastotgeral", grafVendasPeriodo.DadosResumo.total);
                response.Add("grafVendastotcanc", grafVendasPeriodo.DadosResumo.cancelada);
                response.Add("grafVendastotpago", grafVendasPeriodo.DadosResumo.pagos);
                
                response.success = true;

                return Json(response, JsonRequestBehavior.AllowGet);
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

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true)]
        public JsonGraficoResponse BuscarVendasPeriodo(int _mes, int _ano, int? _emp_id, int? _grupo)
        {
            JsonGraficoResponse _resultado = new JsonGraficoResponse();

            try
            {
                _resultado.Descricao = "Apuração do Mês ( " + _mes.ToString() + "/" + _ano.ToString() + " )";

                if (_emp_id != null)
                {
                    var _emp = new EmpresaSRV().FindById(_emp_id);
                    _resultado.Descricao += " - " + _emp.EMP_NOME_FANTASIA;

                }
                if (_grupo != null)
                {
                    var _gru = new GrupoSRV().FindById(_grupo);
                    _resultado.Descricao += " - " + _gru.GRU_DESCRICAO;

                }

                _resultado.Titulo = "RESUMO DE VENDAS NO PERÍODO (PROPOSTAS)";
                _resultado.DadosResumo = _proposta.ListarPropostaPeriodo(_mes, _ano, _emp_id, _grupo);

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        [Autorizar(IsAjax = true)]
        public JsonGraficoDataSource BuscarVendasRepresentante(int _mes, int _ano, int? _emp_id, int? _gru_id)
        {
            JsonGraficoDataSource _resultado = new JsonGraficoDataSource();

            try
            {
                _resultado  = new ContratoSRV().BuscarVendasRepresentante(_mes, _ano, _emp_id, _gru_id);
                _resultado.chart.caption = "Apuração do Mês ( " + _mes.ToString() + "/" + _ano.ToString() + " )";

                var _emp = new EmpresaSRV().FindById(_emp_id);
                _resultado.chart.subCaption += " - " + ((_emp != null) ? _emp.EMP_NOME_FANTASIA : "Todas Empresas");


                if (_gru_id != null)
                {
                    var _gru = new GrupoSRV().FindById(_gru_id);
                    _resultado.chart.subCaption += " - " + _gru.GRU_DESCRICAO;
                }

                _resultado.chart.caption = "RESUMO DE VENDAS (REPRESENTANTE)";
                

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Autorizar(IsAjax = true)]
        public JsonGraficoDataSource BuscarVendasGrupo(int _mes, int _ano, int? _emp_id,  int? _grupo)
        {
              try
            {
                var _resultado = new ContratoSRV().BuscarVendasGrupo(_mes, _ano, _emp_id);
                var _emp = new EmpresaSRV().FindById(_emp_id);

                _resultado.chart.caption = "RESUMO DE VENDAS (GRUPO)";
                _resultado.chart.subCaption = ((_emp != null) ? _emp.EMP_NOME_FANTASIA : "Todas Empresas") + " - " 
                                            + "Apuração do Mês ( " + _mes.ToString() + "/" + _ano.ToString() + " )";

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Autorizar(IsAjax = true)]
        public JsonGraficoDataSource BuscarContratosEvolucaoAnual(int _mes, int _ano, int? _emp_id, int? _grupo)
        {
            try
            {
                var _anoini = _ano - 6;
                var _resultado = new ContratoSRV().BuscarContratosEvolucaoAnual(_mes, _anoini, _ano, _emp_id, _grupo);
                var _emp = new EmpresaSRV().FindById(_emp_id);

                _resultado.chart.caption = "APURAÇÃO DE VENDAS ";
                _resultado.chart.subCaption += ((_emp != null) ? _emp.EMP_NOME_FANTASIA : "Todas Empresas")
                                            + " - " + "Apuração do Período ( "+ _anoini.ToString() + " a " + _ano.ToString() + " )";
                if (_grupo != null)
                {
                    var _gru = new GrupoSRV().FindById(_grupo);
                    _resultado.chart.subCaption += " - " + _gru.GRU_DESCRICAO;

                }

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        [Autorizar(IsAjax = true)]
        public JsonGraficoDataSource BuscarVendasEvolucao(int _mes, int _ano, int? _emp_id, int? _grupo)
        {
            try
            {
                var _anoini = _ano - 6;
                var _resultado = new ContratoSRV().BuscarVendasEvolucao(_mes, _anoini, _ano, _emp_id, _grupo);
                var _emp = new EmpresaSRV().FindById(_emp_id);

                _resultado.chart.caption = "APURAÇÃO DE VENDAS ";
                _resultado.chart.subCaption += ((_emp != null) ? _emp.EMP_NOME_FANTASIA : "Todas Empresas" ) 
                                            + " - " + "Apuração do Período ( " + _mes.ToString() + "/" + _anoini.ToString() +" a "+ _ano.ToString() + " )";
                if (_grupo != null)
                {
                    var _gru = new GrupoSRV().FindById(_grupo);
                    _resultado.chart.subCaption += " - " + _gru.GRU_DESCRICAO;

                }

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [Autorizar(IsAjax = true)]
        public JsonGraficoDataSource BuscarContratosPeriodo(int _mes, int _ano, int? _emp_id, int? _rep_id, int? _grupo)
        {
            
            try
            {

                var _resultado = new ContratoSRV().BuscarFaturamentoAnualSint(_ano, _emp_id, _rep_id, _grupo);

                _resultado.chart.subCaption = "Apuração ( " + _ano.ToString() + " )";

                if (_emp_id != null)
                {
                    var _emp = new EmpresaSRV().FindById(_emp_id);
                    _resultado.chart.subCaption += " - " + _emp.EMP_NOME_FANTASIA;

                }
                if (_rep_id != null)
                {
                    var _rep = new RepresentanteSRV().FindById(_rep_id);
                    _resultado.chart.subCaption += " - " + _rep.REP_NOME;

                }
                if (_grupo != null)
                {
                    var _gru = new GrupoSRV().FindById(_grupo);
                    _resultado.chart.subCaption += " - " + _gru.GRU_DESCRICAO;

                }

                _resultado.chart.caption = "RESUMO DE FATURAMENTO";

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Autorizar(IsAjax = true)]
        public JsonGraficoDataSource BuscarContratoTipoPgto(int _mes, int _ano, int? _emp_id, int? _qtdeParcelas, int? _grupo_id)
        {

            try
            {

                var _resultado = new ContratoSRV().BuscarContratoTipoPgto(_mes, _ano, (int)_emp_id, (int)_qtdeParcelas, _grupo_id);

                _resultado.chart.caption = "Apuração ( "+ _mes.ToString() + "/" + _ano.ToString() + " )";
                _resultado.chart.subCaption = "DEMONSTRATIVO POR FORMA DE PAGAMENTO";

                _resultado.chart.paletteColors = "#0075c2,#1aaf5d,#333333,#666666,#d6d6d6,#g6g6g6,#f8f8f8";
                _resultado.chart.bgColor = "#ffffff";
                _resultado.chart.showAlternateHGridColor = "0";
                _resultado.chart.showBorder = "0";
                _resultado.chart.showCanvasBorder = "0";
                _resultado.chart.baseFontColor = "#333333";
                _resultado.chart.baseFont = "Helvetica Neue, Arial";
                _resultado.chart.captionFontSize = "14";
                _resultado.chart.subcaptionFontSize = "14";
                _resultado.chart.subcaptionFontBold = "0";
                _resultado.chart.usePlotGradientColor = "0";
                _resultado.chart.toolTipColor = "#ffffff";
                _resultado.chart.toolTipBorderThickness = "0";
                _resultado.chart.toolTipBgColor = "#000000";
                _resultado.chart.toolTipBgAlpha = "80";
                _resultado.chart.toolTipBorderRadius = "2";
                _resultado.chart.toolTipPadding = "5";
                _resultado.chart.legendBgAlpha = "0";
                _resultado.chart.legendBorderAlpha = "0";
                _resultado.chart.legendShadow = "0";
                _resultado.chart.legendItemFontSize = "10";
                _resultado.chart.legendItemFontColor = "#666666";
                _resultado.chart.legendCaptionFontSize = "9";
                _resultado.chart.divlineAlpha = "100";
                _resultado.chart.divlineColor = "#999999";
                _resultado.chart.divlineThickness = "1";
                _resultado.chart.divLineIsDashed = "1";
                _resultado.chart.divLineDashLen = "1";
                _resultado.chart.divLineGapLen = "1";
                _resultado.chart.showValues = "0";
                _resultado.chart.numberPrefix = "R$ ";
                _resultado.chart.showpercentvalues = "0";
                _resultado.chart.formatNumber = "1";
                _resultado.chart.formatNumberScale = "0";


                if (_emp_id != null)
                {
                    var _emp = new EmpresaSRV().FindById(_emp_id);
                    _resultado.chart.subCaption += " - " + _emp.EMP_NOME_FANTASIA;
                }

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [Autorizar(IsAjax = true)]
        public JsonGraficoDataSource BuscarProdutosTipoPgto(DateTime _dtini, DateTime _dtfim, int? _emp_id, int? _qtdeParcelas, int? _grupo_id)
        {

            try
            {

                var _resultado = new ContratoSRV().BuscarProdutosTipoPgto(_dtini, _dtfim, (int)_emp_id, (int)_qtdeParcelas, _grupo_id);

                _resultado.chart.caption = "Apuração ( " + _dtfim.Month.ToString() + "/" + _dtfim.Year.ToString() + " )";
                _resultado.chart.subCaption = "DEMONSTRATIVO POR FORMA DE PAGAMENTO (PRODUTO)";

                _resultado.chart.paletteColors = "#0075c2,#1aaf5d,#333333,#666666,#d6d6d6,#g6g6g6,#f8f8f8";
                _resultado.chart.bgColor = "#ffffff";
                _resultado.chart.showAlternateHGridColor = "0";
                _resultado.chart.showBorder = "0";
                _resultado.chart.showCanvasBorder = "0";
                _resultado.chart.baseFontColor = "#333333";
                _resultado.chart.baseFont = "Helvetica Neue, Arial";
                _resultado.chart.captionFontSize = "14";
                _resultado.chart.subcaptionFontSize = "14";
                _resultado.chart.subcaptionFontBold = "0";
                _resultado.chart.usePlotGradientColor = "0";
                _resultado.chart.toolTipColor = "#ffffff";
                _resultado.chart.toolTipBorderThickness = "0";
                _resultado.chart.toolTipBgColor = "#000000";
                _resultado.chart.toolTipBgAlpha = "80";
                _resultado.chart.toolTipBorderRadius = "2";
                _resultado.chart.toolTipPadding = "5";
                _resultado.chart.legendBgAlpha = "0";
                _resultado.chart.legendBorderAlpha = "0";
                _resultado.chart.legendShadow = "0";
                _resultado.chart.legendItemFontSize = "10";
                _resultado.chart.legendItemFontColor = "#666666";
                _resultado.chart.legendCaptionFontSize = "9";
                _resultado.chart.divlineAlpha = "100";
                _resultado.chart.divlineColor = "#999999";
                _resultado.chart.divlineThickness = "1";
                _resultado.chart.divLineIsDashed = "1";
                _resultado.chart.divLineDashLen = "1";
                _resultado.chart.divLineGapLen = "1";
                _resultado.chart.showValues = "0";
                _resultado.chart.numberPrefix = "R$ ";
                _resultado.chart.showpercentvalues = "0";
                _resultado.chart.formatNumber = "1";
                _resultado.chart.formatNumberScale = "0";


                if (_emp_id != null)
                {
                    var _emp = new EmpresaSRV().FindById(_emp_id);
                    _resultado.chart.subCaption += " - " + _emp.EMP_NOME_FANTASIA;
                }

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}

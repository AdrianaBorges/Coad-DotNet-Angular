using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Service.Boleto;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using Ionic.Zip;

namespace COADCORP.Controllers
{
    public class RemessaController : Controller
    {
        public ContaSRV _serviceConta { get; set; }
        public ParcelasSRV _serviceParcela { get; set; }
        public ParcelaLiquidacaoSRV _serviceLiquidacao { get; set; }
        public BoletoSRV _serviceBoleto { get; set; }
        public OcorrenciaRemessaSRV _serviceRemessa { get; set; }
        public ParcelasRemessaSRV _serviceParcelaRemessa { get; set; }
        public CnabSRV _serviceCnab { get; set; }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Index()
        {
            this.CarregarListas();

            return View();
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult CarregarContratos(string assinatura)
        {
            var _contratos = new ContratoSRV().BuscarPorAssinatura(assinatura);

            JSONResponse response = new JSONResponse();
            response.Add("contratos", _contratos);
            return Json(response);
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult CarregarParcelas(string contrato)
        {
            var _titulos = _serviceParcela.BuscarPorContrato(contrato).Where(x => x.PAR_DATA_ALOC == null && x.PAR_DATA_PAGTO == null);

            JSONResponse response = new JSONResponse();
            response.Add("titulos", _titulos);
            return Json(response);
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Editar(int cnbId)
        {
            ViewBag.cnbId = cnbId;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ProcessarArquivoRetornoNovo(HttpPostedFileBase upload)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (Request.Files.Count == 0 || Request.Files[0].FileName == "")
                    throw new Exception("Erro ao processar arquivo. Arquivo vazio, Verifique!!");

                StreamReader linhas = new StreamReader(Request.Files[0].InputStream);

                _serviceParcela.ProcessarArquivoRetornoNovo(Request.Files[0], Request.Files[0].FileName, linhas);

                response.success = true;
                response.message = Message.Info("Processada com sucesso !!");

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
        public void CarregarListas()
        {
            ViewBag.empresa = new EmpresaSRV().FindAll().Select(c => new SelectListItem() { Text = c.EMP_ID.ToString() + " - " + c.EMP_NOME_FANTASIA, Value = c.EMP_ID.ToString() });
            ViewBag.banco = new BancosSRV().BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });
            ViewBag.bco = new BancosSRV().FindAll().Select(c => new SelectListItem() { Text = c.BAN_ID + " " + c.BAN_NOME, Value = c.BAN_ID });

        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult GerarBoletosPDF(int? remessa = null, string msgPontual = "")
        {
            bool gerou = false;

            if (remessa != null)
            {
                // preparando para gerar...
                var lstTitulos = _serviceParcela.LerTitulosGerarBoletos((int)remessa);

                List<ParametroDTO> lstParametro = new List<ParametroDTO>();
                ParametroDTO parametro = new ParametroDTO();

                foreach (var tit in lstTitulos)
                {
                    var assinatura = new AssinaturaSRV().BuscarAssinaturaPorContrato(tit.CTR_NUM_CONTRATO);

                    parametro = new ParametroDTO();

                    parametro.idEmpresa = (int)tit.EMP_ID;
                    parametro.idConta = (int)tit.CTA_ID;
                    parametro.idCliente = (int)assinatura.CLI_ID;
                    parametro.idTitulo = tit.PAR_NUM_PARCELA;
                    parametro.preAlocado = false;
                    parametro.msg = msgPontual;

                    lstParametro.Add(parametro);
                }

                // gerando...
                try
                {   // salvando o boleto gerado \\
                    return File(_serviceBoleto.GerarVariosBoletosPDF(lstParametro), "application/pdf", "Boletos.pdf");
                }
                catch (Exception e)
                {   // salvando o erro em Boletos.txt \\
                    string texto;

                    if (e.InnerException == null)
                        texto = e.Message;
                    else if (e.InnerException.InnerException == null)
                        texto = e.InnerException.Message;
                    else if (e.InnerException.InnerException.InnerException == null)
                        texto = e.InnerException.InnerException.Message;
                    else if (e.InnerException.InnerException.InnerException.InnerException == null)
                        texto = e.InnerException.InnerException.InnerException.Message;
                    else
                        texto = e.InnerException.InnerException.InnerException.InnerException.Message;

                    return File(Encoding.UTF8.GetBytes(texto), "text/plain", "Boletos.txt");
                }
            }

            JSONResponse response = new JSONResponse();
            response.Add("boletoGerado", gerou);
            return Json(response);
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BoletoAvulso()
        {
            return View();
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult buscarContas(string idTitulo)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var parcela = _serviceParcela.FindById(idTitulo);
                if (parcela != null && parcela.EMP_ID != null)
                {
                    if (parcela.PAR_PODE_ALOCAR)
                    {
                        if (parcela.PAR_DATA_PAGTO == null && parcela.PAR_SITUACAO == "NOR")
                        {
                            var cta = _serviceConta.ListarPorEmpresa((int)parcela.EMP_ID).Where(x => x.BAN_ID != "604" && x.CTA_CEDENTE_EMITE_BOLETO);
                            if (cta.Count() > 0)
                            {
                                response.Add("contas", cta.Select(c => new SelectListItem()
                                {
                                    Text = "Empresa: " + c.EMP_ID.ToString() + " Bco: " + c.BAN_ID + " Age: " + c.CTA_AGENCIA + " Cta: " + c.CTA_CONTA + " " + c.CTA_TIPO,
                                    Value = c.CTA_ID.ToString()
                                }));
                            }
                            else
                            {
                                response.success = false;
                                response.message = Message.Warning("Não há Conta para Emissão de Boleto pelo Cedente cadastrada para a empresa deste título!");
                            }
                        }
                        else
                        {
                            response.success = false;
                            response.message = Message.Warning("O título informado já foi pago!");
                        }
                    }
                    else
                    {
                        response.success = false;
                        response.message = Message.Warning("O título informado está bloqueado para alocação e geração de boleto!");
                    }
                }
                else
                {
                    response.success = false;
                    response.message = Message.Warning("O título informado não foi encontrado!");
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = Message.Fail(ex);
            }
            return Json(response);
        }

        //[HttpPost]
        //public ActionResult ProcessarArquivoRetornoCNAB(HttpPostedFileBase upload, bool processar = false)
        //{
        //    JSONResponse response = new JSONResponse();
        //    try
        //    {
        //        if (Request.Files.Count > 0 && Request.Files[0].FileName != "")
        //        {
        //            StreamReader linhas = new StreamReader(Request.Files[0].InputStream);
        //            string retornar = _serviceParcela.ProcessarArquivoRetorno(Request.Files[0], Request.Files[0].FileName, linhas, processar);

        //            bool jaProcessado = (retornar.Substring(0, 1) == "1");

        //            if (retornar.Substring(0, 1) == "#")
        //            { // erro no BANCO DE DADOS \\
        //                response.success = false;
        //                response.message = Message.Fail("Erro de Banco de Dados durante a gravação!");
        //            }
        //            else
        //            {
        //                retornar = retornar.Substring(1, retornar.Length - 1);

        //                response.Add("jaProcessado", jaProcessado);
        //                response.Add("linhas", retornar);
        //                response.Add("status", (retornar.IndexOf("ERRO:") < 0));
        //                response.Add("processou", processar);
        //                response.success = true;
        //                response.message = Message.Info("Processada com sucesso !!");
        //            }
        //        }

        //        return Json(response, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
        //            SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
        //        else
        //        {
        //            Autenticado aut = new Autenticado();
        //            aut.USU_LOGIN = SessionContext.usu_login_desktop;

        //            SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
        //        }

        //        response.success = false;
        //        response.message = Message.Fail(ex);
        //        return Json(response, JsonRequestBehavior.AllowGet);
        //    }

        //}

        [Autorizar(IsAjax = true)]
        public ActionResult CarregarItensAvulsos(string _ASN_NUM_ASSINATURA, string _CLI_NOME)
        {

            JSONResponse response = new JSONResponse();
            try
            {
                if (String.IsNullOrWhiteSpace(_ASN_NUM_ASSINATURA) &&
                    String.IsNullOrWhiteSpace(_CLI_NOME))
                    throw new Exception("Informe ao menos um parametro.");


                if (String.IsNullOrWhiteSpace(_ASN_NUM_ASSINATURA))
                    _ASN_NUM_ASSINATURA = null;

                if (String.IsNullOrWhiteSpace(_CLI_NOME))
                    _CLI_NOME = null;

                var listaparcelas = new ParcelasSRV().CarregarItensAvulsos(_ASN_NUM_ASSINATURA, _CLI_NOME);

                response.success = true;
                response.Add("listaparcelas", listaparcelas);

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
        public ActionResult ListarParcelas(int? _remid, int _pagina = 1, int _numpaginas = 10)
        {

            JSONResponse response = new JSONResponse();
            try
            {
                if (_remid == null)
                    throw new Exception("Numero da remessa não informado. Verifique !!");

                var listaparcelas = new ParcelasSRV().BuscarParcelasRemessa((int)_remid, _pagina, _numpaginas);

                if (listaparcelas.lista.Count() == 0)
                    throw new Exception("Nenhum registro encontrado para o período selecionado!!");

                response.success = true;
                response.AddPage("listaparcelas", listaparcelas);

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
        public ActionResult ListarTipoRemessa()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _lstTipoRemessa = new TipoRemessaSRV().FindAll();

                response.Add("lstTipoRemessa", _lstTipoRemessa);

                return Json(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response);
            }

        }


        [Autorizar(IsAjax = true)]
        public ActionResult ListarRemessa(DateTime? _dtini, DateTime? _dtfinal, int? _emp_id, string _ban_id, int _pagina = 1)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (_dtini == null || _dtfinal == null)
                    throw new Exception("Data não informada !!");

                if (_emp_id == null)
                    _emp_id = -1;

                var listaremessa = _serviceParcelaRemessa.ListarRemessa((DateTime)_dtini, (DateTime)_dtfinal, (int) _emp_id, _ban_id, _pagina);
                var lstdisponivel = new ParcelasSRV().ListarTitulosParaAlocacao( (int) _emp_id);
                
                response.success = true;
                response.Add("lstdisponivel", lstdisponivel);
                response.AddPage("listaremessa", listaremessa);

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
        public ActionResult BuscarRemessa()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var listaremessa = _serviceParcelaRemessa.Listar();

                response.Add("listaremessa", listaremessa);

                return Json(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response);
            }

        }


        [Autorizar(IsAjax = true)]
        public ActionResult ListarContasBanco(string bco,int empid)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                var listacontas = _serviceConta.ListarContasBanco(bco, empid);

                response.success = true;
                response.message = Message.Info("ok");
                response.Add("listacontas", listacontas);
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
        public ActionResult BuscarContasBanco(string bco, bool cta_emite_boleto)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                var listacontas = _serviceConta.Listar(bco, cta_emite_boleto);

                response.success = true;
                response.message = Message.Info("ok");
                response.Add("listacontas", listacontas);
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
        public ActionResult lerCodigosRemessaDoBanco(string bco = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (String.IsNullOrWhiteSpace(bco))
                    throw new Exception("Erro ao carregar tipos de remessa !! ");

                var lstTipoRemessa = _serviceRemessa.LerOcorrenciaRemessa(bco);
                response.Add("lstTipoRemessa", lstTipoRemessa);
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
        public ActionResult SelecionarTitulos(int ctaId, decimal vlrT)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var cta = _serviceConta.FindById(ctaId);

                if (cta == null)
                    throw new Exception("Conta não cadastrada !!");

                var sel = _serviceParcela.SelecionarTitulos(cta, vlrT);

                if (sel.Count == 0)
                    throw new Exception(" Não existem registros para o período selecionado !!");

                response.success = true;
                response.Add("alocacao", sel);

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
        public ActionResult LerTitulosAlocacao(string ctaId, string bco, string titulo = null, DateTime? dvencimentoI = null, DateTime? dvencimentoF = null, Decimal vlrI = 0, Decimal vlrF = 0, Decimal vlrT = 0)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                List<ParcelasDTO> sel = null;

                var ctaTemp = _serviceConta.Dao.FindById( int.Parse( ctaId ) );

                var ctaUsada = _serviceConta.BuscarContaRemessa(ctaTemp.EMP_ID, bco, true );

                if (ctaId != null)
                    sel = _serviceParcela.SelecionarTitulos(titulo, ctaUsada, dvencimentoI, dvencimentoF, vlrI, vlrF, vlrT);
                if (sel.Count == 0)
                    throw new Exception(" Não existem registros para o período selecionado !!");

                response.success = true;
                response.Add("alocacao", sel);

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
        public ActionResult EfetuarAlocacaoAvulsa(List<ParcelasDTO> alocar, int _tre_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var sel = new List<ParcelasDTO>();

                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    sel = _serviceParcela.EfetuarAlocacaoAvulsa(alocar, _tre_id);
                    scope.Complete();
                }

                response.success = true;
                response.Add("alocacao", sel.Count());
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
        public ActionResult EfetuarAlocacao(List<ParcelasDTO> alocar , int _tre_id, bool? sacadorAvalista)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var sel = new List<ParcelasDTO>();

                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    sel = _serviceParcela.EfetuarAlocacao(alocar, _tre_id, sacadorAvalista);
                    scope.Complete();
                }

                response.success = true;
                response.Add("alocacao", sel.Count());
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
        public ActionResult DesAlocar(int? _rem_id)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                _serviceParcela.EfetuarDesAlocacao((int)_rem_id);
                response.success = true;
                return Json(response);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
                return Json(response);
            }

        }

         [Autorizar(IsAjax = true)]
        public ActionResult GerarArquivoCNAB(int? cta_id = null
                                            ,string leiaute = "400"
                                            ,int? remessa = null
                                            ,string tipoRemessa = null
                                            ,bool preAlocado = false)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                string conteudo = "Não há registros!";

                if (cta_id != null)
                {
                    _serviceParcela.SalvarRemessaParcelaAlocada((int)remessa, tipoRemessa);

                    var cta = _serviceConta.FindById(cta_id);

                    leiaute = ( cta.BAN_ID == "756" ? "240" : "400" );

                    conteudo = _serviceCnab.gerarArquivoCNAB(cta.EMP_ID, cta.CTA_ALOCAR_TITULO_DA_EMP_ID, cta.BAN_ID, leiaute, remessa, preAlocado, tipoRemessa);

                    DateTime d = DateTime.Now;
                    var dt = d.ToString("yyyy-MM-dd hh:mm:ss");
                }

                string _curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());
                string _arquivo = "Remessa" + remessa.ToString() + ".REM";
                string _Path = _curDir + "\\temp\\" + _arquivo;
                string _lnkPath = _arquivo;
                string _lnkLink = "Baixar o arquivo ( " + _arquivo + " )";

                System.IO.File.WriteAllText(_Path, conteudo);

                List<SelectListItem> path = new List<SelectListItem>();

                path.AddRange(new[] { new SelectListItem() { Text = _lnkLink, Value = _lnkPath } });

                response.Add("lnkPath", new SelectList(path, "Value", "Text"));
                response.success = true;
                response.message = Message.Info("Arquivo de Remessa gerado com sucesso!!");

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
        public ActionResult BaixarArquivoRemessa(string _arquivo)
        {
            FileContentResult result;
            try
            {
                string _curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                string _Path = _curDir + "\\temp\\" + _arquivo;
                string _lnkPath = "../temp/" + _arquivo;
                string _lnkLink = "Baixar o arquivo SPED ( " + _arquivo + " )";

                var conteudo = System.IO.File.ReadAllText(_Path, Encoding.UTF8);

                var contentType = "text/xml";
                var bytes = Encoding.UTF8.GetBytes(conteudo);
                result = new FileContentResult(bytes, contentType);
                result.FileDownloadName = _arquivo;

                return result;

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

                return null;
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult TransmitirArqCNAB(int _rem_id, int _cta_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                _serviceParcela.EfetuarTransmissao(_rem_id, _cta_id);

                response.message = Message.Info("Ok");
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
        public ActionResult DownloadNFesRemessa(int? REM_ID)
        {

            JSONResponse response = new JSONResponse();

            try
            {

                var _curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());
                string _arquivo = "ZipNFesRemessa" + REM_ID.ToString() + ".zip";
                string _Path = _curDir + "\\temp\\" + _arquivo;
                string _lnkPath = _arquivo;
                string _lnkLink = "Baixar o arquivo ( " + _arquivo + " )";

                IList<string> arquivos = _serviceParcela.ListarArquivosRemessaParaOZip(REM_ID).ToList();

                using (ZipFile zip = new ZipFile())
                {

                    if ( arquivos != null && arquivos.Count() > 0 )
                        // percorre todos os arquivos da lista
                        foreach (string arquivo in arquivos)
                        {

                            string item = arquivo.Split('|')[0].Replace("\\SCHEDULER\\", "\\SCHEDULER_HOMOL\\");
                            item = item.Replace("D:", "C:");

                            string contrato = arquivo.Split('|')[1];

                            string nomeArquivo = "\\REMESSA_" + REM_ID.ToString() + "\\" + contrato + "_" + item.Split('\\')[item.Split('\\').Count() - 1];

                            Stream stream = System.IO.File.Open(item, FileMode.Open);

                            // se o item é um arquivo
                            if (stream != null)
                                try
                                {
														 
                                    zip.AddEntry(nomeArquivo, stream); //.AddItem(item, contrato);   // .AddFile(item, "");

                                }
                                catch
                                {

                                    throw;

                                }

                        }

                    // Salva o arquivo zip para o destino
                    try
                    {
                        zip.Save(_Path);
                    }
                    catch
                    {
                        throw;
                    }

                }

                List<SelectListItem> path = new List<SelectListItem>();

                path.AddRange(new[] { new SelectListItem() { Text = _lnkLink, Value = _lnkPath } });

                response.Add("lnkPath", new SelectList(path, "Value", "Text"));
                response.success = true;
                response.message = Message.Info("Arquivo Zip da Remessa gerado com sucesso!!");

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
        public ActionResult BaixarArquivoZipNFeRemessa(string _arquivo)
        {
            FileContentResult result;
            try
            {

                string _curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());
                string _Path = _curDir + "\\temp\\" + _arquivo;
                string _lnkPath = "../temp/" + _arquivo;
                string _lnkLink = "Baixar o arquivo SPED ( " + _arquivo + " )";

                byte[] conteudo = System.IO.File.ReadAllBytes(_Path);

                var contentType = "application/zip";

                result = new FileContentResult(conteudo, contentType);
                result.FileDownloadName = _arquivo;

                return result;

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

                return null;
            }

        }


    }
}

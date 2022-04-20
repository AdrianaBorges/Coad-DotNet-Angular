using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Service;
using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Service;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Model;
using GenericCrud.Service;
using System.ComponentModel;
using System.Text;
using System.IO;
using Coad.GenericCrud.Exceptions;
using System.Globalization;
using System.Transactions;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Service.Boleto;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using System.Net;
using COAD.UTIL.Ferramentas;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using NReco.PdfGenerator;

namespace COADCORP.Controllers.Cobranca_Escritural
{
    /// <summary>
    /// ALT: 29/06/2016 - Cobrança Escritural - Padrão CNAB (Centro Nacional de Automação Bancária)
    /// </summary>
    [ValidateInput(false)]
    public class CobrancaEscrituralController : Controller
    {
        public CnabSRV _serviceCnab { get; set; }
        public BancosSRV _serviceBanco { get; set; }
        public ContaSRV _serviceConta { get; set; }
        public EmpresaSRV _serviceEmpresa { get; set; }
        public ClienteSRV _serviceCliente { get; set; }
        public ClienteEnderecoSRV _serviceClienteEndereco { get; set; }
        public ParcelasSRV _serviceParcela { get; set; }
        public ParcelaLiquidacaoSRV _serviceLiquidacao { get; set; }
        public AssinaturaSRV _serviceAssinatura { get; set; }
        public DocLiquidacaoSRV _serviceDocLiq { get; set; }
        public BoletoSRV _serviceBoleto { get; set; }
        public ContratoSRV _serviceContrato { get; set; }
        public ParcelaAlocadaSRV _serviceParcelaAlocada { get; set; }
        public OcorrenciaRemessaSRV _serviceRemessa { get; set; }
        public ParcelasRemessaSRV _serviceParcelaRemessa { get; set; }
        public ItemPedidoSRV _serviceItemPedido { get; set; }
        public PedidoCRMSRV _servicePedidoCRM { get; set; }
        

        [Autorizar(PorMenu = false)]
        public ActionResult Dashboard()
        {
            ViewBag.empresa = _serviceEmpresa.FindAll().OrderBy(x => x.EMP_NOME_FANTASIA).Select(c => new SelectListItem() { Text = c.EMP_ID.ToString() + " - " + c.EMP_NOME_FANTASIA, Value = c.EMP_ID.ToString() });
            return View();
        }

        [HttpGet]
       /// [Autorizar(PorMenu = false)]
        public ActionResult BoletoHtml(string idTitulo)
        {

            ViewBag.idTitulo = idTitulo;

            return View();

        }

        [Autorizar(PorMenu = false)]
        public ActionResult Remessa()
        {
            // empresas ordenados por código \\
            ViewBag.empresa = _serviceEmpresa.FindAll().OrderBy(x => x.EMP_NOME_FANTASIA).Select(c => new SelectListItem() { Text = c.EMP_ID.ToString() + " - " + c.EMP_NOME_FANTASIA, Value = c.EMP_ID.ToString() });

            // bancos ordenados por código \\
            ViewBag.banco = _serviceBanco.BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });

            return View();
        }


        public ActionResult CarregarBoletoHtml(string idparcela)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                var _boleto = this.GerarBoletoHtmlPdf(idparcela);

                response.success = true;
                response.Add("boleto", _boleto);
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

                TempData["message"] = ex.Message;

                return View();

            }

        }

        [Autorizar(PorMenu = false)]
        public ActionResult GerarBoletoHtmlPdf(string idTitulo)
        {

            var arq = DateTime.Now.Millisecond.ToString() +
                      DateTime.Now.Second.ToString() +
                      DateTime.Now.Minute.ToString();

        

            var outPdfBuffer = _serviceBoleto.GerarBoletosPDF(idTitulo);

            FileResult fileResult = new FileContentResult(outPdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "boleto" + arq + ".pdf";

            string _curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());
            
            _curDir = _curDir + "\\temp\\ET015" + arq + ".txt";
            
            System.IO.File.WriteAllBytes(_curDir, ((FileContentResult)fileResult).FileContents);
            
            this.ToFile(fileResult, _curDir);
            
            return fileResult;

        }

        public void ToFile(FileResult fileResult, string fileName)
        {
            if (fileResult is FileContentResult)
            {
                System.IO.File.WriteAllBytes(fileName, ((FileContentResult)fileResult).FileContents);
            }
            else if (fileResult is FilePathResult) 
            {
                System.IO.File.Copy(((FilePathResult)fileResult).FileName, fileName, true); //overwrite file if it already exists
            }
            else if (fileResult is FileStreamResult)
            {
                using (var fileStream = System.IO.File.Create(fileName))
                {
                    var fileStreamResult = (FileStreamResult)fileResult;
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
                    fileStreamResult.FileStream.CopyTo(fileStream);
                    fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin); //reset position to beginning. If there's any chance the FileResult will be used by a future method, this will ensure it gets left in a usable state - Suggestion by Steven Liekens
                }
            }
            else
            {
                throw new ArgumentException("Unsupported FileResult type");
            }
        }
        
        public ActionResult Boleto2aVia()
        {
            try
            {
                if (SessionContext.GetInSession<string>("LOGIN_PORTAL") == null)
                    return Content("<script>parent.window.open('http://www.coad.com.br/cadastre-se/');</script>");

                var code = SessionContext.GetInSession<string>("LOGIN_PORTAL");

                ViewBag.idAssinatura = code;
                //ViewBag.idAssinatura = "01j64179";

                return View();

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

                TempData["message"] = ex.Message;

                return View();

            }

        }




        [Autorizar(PorMenu = false, IsAjax = true)]
        [HttpPost]
        public ActionResult obterTitulosDaAssinatura(string idAssinatura)
        {
            var lstTitulos = _serviceParcela.obterTitulosDaAssinatura(idAssinatura);

            JSONResponse response = new JSONResponse();
            response.Add("titulos", lstTitulos);
            return Json(response);
        }

        [HttpPost]
        public ActionResult Boleto2aViaDownload(string idTitulo)
        {
            try
            {
                var bytes = _serviceCnab.GerarBoleto2aVia(idTitulo);
                if(bytes != null) {

                    return File(bytes, "application/pdf", "Boletos.pdf");
                }
                else
                {
                    throw new Exception("O Cliente do título (" + idTitulo + ") não foi localizado pelo gerador de [parâmetros] do Boleto!");
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

                ViewBag.erro = idTitulo + " -- " + SysException.Show(ex);

                return View();

            }
        }

        // boleto avulso \\
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BoletoAvulso(string idTitulo = "")
        {
            ViewBag.idTitulo = idTitulo;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarCNAB()
        {
            var _lstcnab = _serviceCnab.BuscarCNAB();



            JSONResponse response = new JSONResponse();
            response.Add("lstcnab", _lstcnab);
            return Json(response);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult BuscarDetalheCNAB(string _referencia)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _itemcnab = new CnabDTO();

                var _campos = _referencia.Split('-');

                _itemcnab.EMP_ID = Convert.ToInt32(_campos[0]);
                _itemcnab.CNB_REGISTRO = _campos[1];
                _itemcnab.BAN_ID = _campos[2];
                _itemcnab.CNB_CNAB = _campos[3];
                _itemcnab.CNB_ARQUIVO = _campos[4];

                var _lstdetalhecnab = _serviceCnab.BuscarDetalheCNAB(_itemcnab);
                response.Add("lstdetalhecnab", _lstdetalhecnab);
                return Json(response);

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        

        [Autorizar(IsAjax = true)]
        public ActionResult SalvarCnab(CnabDTO _CNAB)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                _CNAB.DATA_ALTERACAO = DateTime.Now;
                _serviceCnab.Merge(_CNAB, "CNB_ID");

                response.success = true;
                response.message = Message.Info("Registro atualizado com sucesso");
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult EnviarBoletoAvulso(string idTitulo, DateTime dtVencimento, Decimal vlrBoleto, string email)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
               _serviceParcela.EnviarBoletoAvulso(idTitulo, dtVencimento, vlrBoleto, 0, email, repId);
                
               response.success = true;
               response.message = Message.Info(@"Email enviado com sucesso !!");
               return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                response.success = false;
                response.SetMessageFromValidacaoException(e, false, true);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// ALT: 27/12/2016 - retornando as contas deste título para emitir boleto;
        ///                   verifica também toda a disponibilidade do título.
        /// </summary>
        /// <param name="idTitulo"></param>
        /// <returns></returns>
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult buscarContas(string idTitulo)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var parcela = _serviceParcela.FindById(idTitulo);
                if (parcela != null && parcela.EMP_ID != null)
                {
                    if (parcela.PAR_DATA_PAGTO == null)
                    {
                        var _clien = _serviceParcela.RetornarClienteDaParcela(idTitulo);
                        if (_clien == null)
                            throw new Exception("Não encontrei o Cliente do título informado!");

                        if (!_serviceCliente.ChecarCnpjValido(_clien.CLI_CPF_CNPJ) && !_serviceCliente.ChecarCpfValido(_clien.CLI_CPF_CNPJ))
                            throw new Exception("CPF / CNPJ (" + _clien.CLI_CPF_CNPJ + ") do cliente " + _clien.CLI_NOME + " é inválido!");

                        var cta = _serviceConta.BuscarContaBoletoAvuso();
                        if (cta != null)
                        {
                            List<SelectListItem> conta = new List<SelectListItem>();
                            conta.AddRange(new[]{
                                            new SelectListItem()
                                            { Text = "Empresa: " + cta.EMP_ID.ToString() + " Bco: " + cta.BAN_ID + " Age: " + cta.CTA_AGENCIA + " Cta: " + cta.CTA_CONTA + " " + cta.CTA_TIPO,
                                                Value = cta.CTA_ID.ToString() }
                            });

                            var _email = new AssinaturaEmailSRV();
                            var _lstEmails = _email.FindEmailsDoClienteEAssinatura(_clien.CLI_ID);

                            List<SelectListItem> emails = new List<SelectListItem>();
                            foreach (var lst in _lstEmails)
                            {
                                emails.AddRange(new[] { new SelectListItem() { Text = lst.AEM_EMAIL, Value = lst.AEM_EMAIL } });
                            }

                            response.Add("contas", new SelectList(conta, "Value", "Text"));
                            response.Add("emails", new SelectList(emails, "Value", "Text"));
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

        // boleto avulso \\
        [Autorizar(PorMenu = false, IsAjax = true)]
        [HttpPost]
        public ActionResult VisualizarBoleto(string idTitulo, DateTime dtVencimento, Decimal vlrBoleto, int idConta, string msg = "")
        {
            JSONResponse response = new JSONResponse();

            List<ParametroDTO> lstParametro = new List<ParametroDTO>();

            var parametro = _serviceCnab.prepararParametro(idTitulo, idConta, true, msg, dtVencimento, vlrBoleto);
            if (parametro != null)
            {
                lstParametro.Add(parametro);
                return File(_serviceBoleto.GerarVariosBoletosPDF(lstParametro), "application/pdf", "Boletos.pdf");
            }

            response.success = true;
            response.message = Message.Fail("Por favor, verifique o CNAB do Banco associado a esta Conta!");

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BaixarArquivo(string _arquivo)
        {
            FileContentResult result;
            try
            {
                string _curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                string _Path = _curDir + "\\temp\\" + _arquivo;
                string _lnkPath = "../temp/" + _arquivo;
                string _lnkLink = "Baixar o arquivo ( " + _arquivo + " )";

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

        // ------------------------------------------PROCESSAR BAIXA MANUAL E DE ARQUIVO CNAB -----------------------------------

        // tela...
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BaixaTitulos()
        {
            var ban = _serviceBanco.FindAll();
            @ViewBag.banco = ban.Select(c => new SelectListItem() { Text = c.BAN_ID + " " + c.BAN_NOME, Value = c.BAN_ID });

            var dli = _serviceDocLiq.FindAll();
            @ViewBag.docLiq = dli.Select(c => new SelectListItem() { Text = c.DLI_SIGLA + " " + c.DLI_DESCRICAO, Value = c.DLI_SIGLA });

            return View();
        }

        // tela...
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BaixaTitulosEstorno()
        {
            var ban = _serviceBanco.FindAll();
            @ViewBag.banco = ban.Select(c => new SelectListItem() { Text = c.BAN_ID + " " + c.BAN_NOME, Value = c.BAN_ID });

            var dli = _serviceDocLiq.FindAll();
            @ViewBag.docLiq = dli.Select(c => new SelectListItem() { Text = c.DLI_SIGLA + " " + c.DLI_DESCRICAO, Value = c.DLI_SIGLA });

            return View();
        }
        
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
        //            {
        //                response.success = false;
        //                response.message = Message.Fail(retornar.Substring(1, retornar.Length - 1));
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

        // impressão do CNAB retorno \\
        //[HttpPost]
        //public ActionResult ImprimirCNABRetorno(HttpPostedFileBase upload, bool processar = false)
        //{
        //    FileContentResult result;
        //    try
        //    {
        //        if (Request.Files.Count > 0 && Request.Files[0].FileName != "")
        //        {
        //            StreamReader linhas = new StreamReader(Request.Files[0].InputStream);

        //            string conteudo = _serviceParcela.ProcessarArquivoRetorno(Request.Files[0], Request.Files[0].FileName, linhas, processar);

        //            var contentType = "application/doc";
        //            var bytes = Encoding.UTF8.GetBytes(conteudo);
        //            result = new FileContentResult(bytes, contentType);

        //            result.FileDownloadName = "RetornoCNAB.RET";
        //        }
        //        else
        //            result = null;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    return result;
        //}

        // impressão Analitico \\
        [HttpPost]
        public ActionResult ImprimirAnalitico(string html, string arquivo)
        {
            FileContentResult result;
            try
            {
                if (!String.IsNullOrWhiteSpace(html))
                {
                    var contentType = "application/doc";
                    var bytes = Encoding.UTF8.GetBytes(html);
                    result = new FileContentResult(bytes, contentType);

                    result.FileDownloadName = arquivo + ".doc";
                }
                else
                    result = null;
            }
            catch
            {
                return null;
            }
            return result;
        }

        // buscar parcela pelo número e LIQUIDAÇÃO NÃO EXCLUÍDA \\
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BuscarParcela(string PAR_NUM_PARCELA = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (!String.IsNullOrWhiteSpace(PAR_NUM_PARCELA))
                {
                    var tit = _serviceParcela.FindById(PAR_NUM_PARCELA);
                    if (tit == null)
                        response.message = Message.Warning("Título não encontrado!");
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(tit.CTR_NUM_CONTRATO))
                        {
                            var ass = _serviceAssinatura.BuscarAssinaturaPorContrato(tit.CTR_NUM_CONTRATO);

                            var liq = _serviceLiquidacao.BuscarPorParcela(PAR_NUM_PARCELA);

                            response.Add("docLiq", liq);
                            response.Add("titulo", tit);
                            response.Add("nomeCliente", ass.CLIENTES.CLI_ID.ToString() + "-" + ass.CLIENTES.CLI_NOME);
                            response.Add("cnpjCliente", ass.CLIENTES.CLI_CPF_CNPJ);
                            response.Add("assiCliente", ass.ASN_NUM_ASSINATURA);
                        }
                        else
                        {
                            var itm = _serviceItemPedido.FindById(tit.IPE_ID);

                            var crm = _servicePedidoCRM.FindById(itm.PED_CRM_ID);

                            var cli = _serviceCliente.FindById(crm.CLI_ID);

                            var liq = _serviceLiquidacao.BuscarPorParcela(PAR_NUM_PARCELA);

                            response.Add("docLiq", liq);
                            response.Add("titulo", tit);
                            response.Add("nomeCliente", cli.CLI_ID.ToString() + "-" + cli.CLI_NOME);
                            response.Add("cnpjCliente", cli.CLI_CPF_CNPJ);
                            response.Add("assiCliente", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = Message.Fail(ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // Baixa manual \\
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BaixarManualmente(IList<ParcelaLiquidacaoDTO> liq)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                // ALT: 15/02/2017 - DTO de Parcelas para realizar UPDATE direto no BANCO.
                List<ParcelaUpdateDTO> listaDeParcelasUpdateDB = new List<ParcelaUpdateDTO>();
                List<ParcelasLegadoUpdateDTO> listaDeParcelasLegadoUpdateDB = new List<ParcelasLegadoUpdateDTO>();
                List<ParcelasDTO> listaDeParcelas = new List<ParcelasDTO>();
                List<ParcelaLiquidacaoDTO> listaDeLiquidacao = new List<ParcelaLiquidacaoDTO>();

                //
                foreach (var r in liq)
                {
                    if (r.PAR_NUM_PARCELA != null)
                    {
                        var parc = _serviceParcela.FindById(r.PAR_NUM_PARCELA);
                        if (parc != null)
                        {
                            if (parc.PAR_DATA_PAGTO == null)
                            {
                                parc.PAR_VLR_PAGO = r.PLI_VALOR;
                                parc.PAR_DATA_PAGTO = DateTime.Now;
                                parc.PAR_BAIXA_MANUAL = true;

                                listaDeParcelas.Add(parc);

                                // liquidando \\
                                var temLiquidacao = _serviceLiquidacao.FindById(r.PAR_NUM_PARCELA, r.PLI_TIPO_DOC, r.PLI_NUMERO);
                                if (temLiquidacao == null)
                                {
                                    r.PLI_DATA = DateTime.Now;
                                    r.PLI_ORIGEM_PGTO = "2";

                                    listaDeLiquidacao.Add(r);
                                }

                                // inicializando DTO PARCELAS para executar direto no BANCO \\
                                ParcelaUpdateDTO ParcelaUpdateDB = new ParcelaUpdateDTO();
                                ParcelaUpdateDB.PAR_NUM_PARCELA = parc.PAR_NUM_PARCELA;
                                ParcelaUpdateDB.PAR_DATA_PAGTO = parc.PAR_DATA_PAGTO;
                                ParcelaUpdateDB.PAR_VLR_PAGO = parc.PAR_VLR_PAGO;
                                ParcelaUpdateDB.PAR_DATA_ALOC = parc.PAR_DATA_ALOC;
                                ParcelaUpdateDB.PAR_NOSSO_NUMERO = parc.PAR_NOSSO_NUMERO;
                                ParcelaUpdateDB.PAR_REMESSA = parc.PAR_REMESSA;
                                ParcelaUpdateDB.PAR_TRANSMITIDO = parc.PAR_TRANSMITIDO;
                                ParcelaUpdateDB.CTA_ID = parc.CTA_ID;
                                ParcelaUpdateDB.BAN_ID = parc.BAN_ID;

                                listaDeParcelasUpdateDB.Add(ParcelaUpdateDB);

                                //
                                ParcelasLegadoUpdateDTO ParcelaLegadoUpdateDB = new ParcelasLegadoUpdateDTO();
                                var parcLegado = _serviceParcela.BuscarLegado(parc.PAR_NUM_PARCELA);
                                if (parcLegado != null)
                                {
                                    ParcelaLegadoUpdateDB.CONTRATO = parcLegado.CONTRATO;
                                    ParcelaLegadoUpdateDB.LETRA = parcLegado.LETRA;
                                    ParcelaLegadoUpdateDB.CD = parcLegado.CD;
                                    ParcelaLegadoUpdateDB.ALOC_BANCO = parcLegado.ALOC_BANCO;
                                    ParcelaLegadoUpdateDB.BCO_ALOC = parcLegado.BCO_ALOC;
                                    ParcelaLegadoUpdateDB.CART_ALOC = parcLegado.CART_ALOC;
                                    ParcelaLegadoUpdateDB.CART_ALOC_2 = parcLegado.CART_ALOC_2;
                                    ParcelaLegadoUpdateDB.CEDENTE = parcLegado.cedente;
                                    ParcelaLegadoUpdateDB.DT_ALOC = parcLegado.DT_ALOC;
                                    ParcelaLegadoUpdateDB.DT_EMISSAO_BLQ = parcLegado.DT_EMISSAO_BLQ;
                                    ParcelaLegadoUpdateDB.NOSSO_NUMERO = parcLegado.nosso_numero;
                                    ParcelaLegadoUpdateDB.DT_PAGTO = parcLegado.DT_PAGTO;
                                    ParcelaLegadoUpdateDB.VLR_PAGO = parcLegado.VLR_PAGO;

                                    listaDeParcelasLegadoUpdateDB.Add(ParcelaLegadoUpdateDB);
                                }
                            }
                        }
                    }
                }

                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    _serviceParcela.BaixarTitulos(listaDeParcelas, listaDeLiquidacao, null, true, listaDeParcelasUpdateDB, listaDeParcelasLegadoUpdateDB);

                    scope.Complete();
                }

                response.Add("bxManual", listaDeParcelas.Count() > 0);
            }
            catch (Exception ex)
            {
                response.Add("bxManual", false);
                response.success = false;
                response.message = Message.Fail(ex);
            }
            return Json(response);
        }

        // Baixa manual \\
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult EstornarBaixa(string PAR_NUM_PARCELA)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                response.Add("bxEstorno", _serviceParcela.EstornarBaixa(PAR_NUM_PARCELA));
            }
            catch (Exception ex)
            {
                response.Add("bxEstorno", false);
                response.success = false;
                response.message = Message.Fail(ex);
            }
            return Json(response);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult ListarRemessa(DateTime _dtini, DateTime _dtfinal, int _emp_id, string _ban_id, int _pagina = 1)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var listaremessa = _serviceParcelaRemessa.ListarRemessa(_dtini, _dtfinal, _emp_id, _ban_id, _pagina);

                if (listaremessa.lista.Count() == 0)
                    throw new Exception("Nenhum registro encontrado para o período selecionado!!");

                response.success = true;
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


        // ------------------------------------------GERÊNCIA DE TÍTULOS --------------------------------
        // tela...
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
        public ActionResult ResumoFinanceiro()
        {
            var _listabancos = _serviceBanco.FindAll().Select(c => new SelectListItem() { Text = c.BAN_ID + " " + c.BAN_NOME, Value = c.BAN_ID });
            ViewBag.bco = _listabancos;

            ViewBag.remessa = _serviceParcelaRemessa.Listar().Select(c => new SelectListItem() { Text = c.REM_REF, Value = c.REM_ID.ToString() });

            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult GerenciaTitulos()
        {
            var _listabancos = _serviceBanco.FindAll().Select(c => new SelectListItem() { Text = c.BAN_ID + " " + c.BAN_NOME, Value = c.BAN_ID });
            ViewBag.bco = _listabancos;

            ViewBag.remessa = _serviceParcelaRemessa.Listar().Select(c => new SelectListItem() { Text = c.REM_REF, Value = c.REM_ID.ToString() });

            return View();
        }
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
        public ActionResult DesAlocar(int? codRemessa)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                _serviceParcela.EfetuarDesAlocacao((int)codRemessa);
                response.Add("desAlocou", "true");
                return Json(response);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
                return Json(response);
            }

        }

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


        // processamento...
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult processarTitulos(int empresa = 2, string remessa = "teste", DateTime? de = null, DateTime? ate = null, DateTime? dataBase = null)
        {

            var expirando = _serviceParcela.LerTitulosExpirando(empresa, dataBase);

            JSONResponse response = new JSONResponse();

            response.Add("expirando", expirando);

            return Json(response);
        }
        
        public JsonResult lerCampos(Object DTO = null, string nomeModel = null)
        {
            ClassMetadata c = new ClassMetadata();

            JSONResponse response = new JSONResponse();
            response.Add(nomeModel, c.RetornaCampos(DTO, nomeModel));

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult lerCamposConta()
        {
            ContaDTO DTO = new ContaDTO();
            return this.lerCampos(DTO, "conta");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult lerCamposEmpresa()
        {
            EmpresaModel DTO = new EmpresaModel();
            return this.lerCampos(DTO, "empresa");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult lerCamposCliente()
        {
            ClienteDto DTO = new ClienteDto();
            return this.lerCampos(DTO, "cliente");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult lerCamposClienteEndereco()
        {
            ClienteEnderecoDto DTO = new ClienteEnderecoDto();
            return this.lerCampos(DTO, "clienteEndereco");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult lerCamposParcela()
        {
            ParcelasDTO DTO = new ParcelasDTO();
            return this.lerCampos(DTO, "parcela");
        }

        //-----------------------------------------------------CADASTRAMENTO DO CNAB - Tela inicial \\
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Index()
        {
            ViewBag.empresa = _serviceEmpresa.FindAll().Select(c => new SelectListItem() { Text = c.EMP_ID.ToString() + " - " + c.EMP_NOME_FANTASIA, Value = c.EMP_ID.ToString() });
            ViewBag.banco = _serviceBanco.BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });

            List<SelectListItem> cnab = new List<SelectListItem>();
            cnab.AddRange(new[]{
                          new SelectListItem() { Text = "400", Value = "400" },
                          new SelectListItem() { Text = "240", Value = "240" }
            });
            ViewBag.cnab = new SelectList(cnab, "Value", "Text");

            // arquivo: 1REMESSA ou 2RETORNO \\
            List<SelectListItem> arquivo = new List<SelectListItem>();
            arquivo.AddRange(new[]{
                          new SelectListItem() { Text = "1 - REMESSA", Value = "1REMESSA" },
                          new SelectListItem() { Text = "2 - RETORNO", Value = "2RETORNO" }
            });
            ViewBag.arquivo = new SelectList(arquivo, "Value", "Text");

            return View();
        }

        // novo \\
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Novo()
        {
            // empresas ordenados por código \\
            ViewBag.empresa = _serviceEmpresa.FindAll().OrderBy(x => x.EMP_NOME_FANTASIA).Select(c => new SelectListItem() { Text = c.EMP_ID.ToString() + " - " + c.EMP_NOME_FANTASIA, Value = c.EMP_ID.ToString() });

            // bancos ordenados por código \\
            ViewBag.banco = _serviceBanco.BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });

            // cnab 400 ou 240 \\
            List<SelectListItem> cnab = new List<SelectListItem>();
            cnab.AddRange(new[]{
                          new SelectListItem() { Text = "400", Value = "400" },
                          new SelectListItem() { Text = "240", Value = "240" }
            });
            ViewBag.cnab = new SelectList(cnab, "Value", "Text");

            // arquivo: 1REMESSA ou 2RETORNO \\
            List<SelectListItem> arquivo = new List<SelectListItem>();
            arquivo.AddRange(new[]{
                          new SelectListItem() { Text = "1 - REMESSA", Value = "1REMESSA" },
                          new SelectListItem() { Text = "2 - RETORNO", Value = "2RETORNO" }
            });
            ViewBag.arquivo = new SelectList(arquivo, "Value", "Text");

            // tipo: (T)exto (N)úmero (D)ata \\
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.AddRange(new[]{
                          new SelectListItem() { Text = "(T)exto", Value = "T" },
                          new SelectListItem() { Text = "(N)úmero", Value = "N" },
                          new SelectListItem() { Text = "(D)ata", Value = "D" }
            });
            ViewBag.tipo = new SelectList(tipo, "Value", "Text");

            return View("Edit");
        }

        // editar \\
        [Autorizar(PorMenu = false, IsAjax = true)]
        [HttpPost]
        public ActionResult Editar(int cnbId)
        {
            ViewBag.cnbId = cnbId;
            return View("Edit");
        }

        // JS: listar() \\
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult lerCNAB(int? empresa = null, string banco = null, string leiaute = null, string arquivo = null, string registro = null, string campo = null, int pagina = 1, int itensPorPagina = 10)
        {
            Pagina<CnabDTO> page = _serviceCnab.LerCNAB(empresa, banco, leiaute, arquivo, registro, campo, pagina, itensPorPagina);

            JSONResponse response = new JSONResponse();
            response.AddPage("cnb", page);

            return Json(response);
        }

        // lendo os dados para a tela...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Readcnab(int cnbId)
        {
            var cnab = _serviceCnab.FindById(cnbId);

            JSONResponse response = new JSONResponse();
            response.Add("cnab", cnab);

            return Json(response);
        }


        // remover registro \\
        [Autorizar(PorMenu = false, IsAjax = true)]
        [HttpPost]
        public ActionResult removerCampo(int cnabId)
        {
            JSONResponse result = new JSONResponse();
            result.Add("cnb", _serviceCnab.DeletarCNAB(cnabId));
            return Json(result);
        }
    }
}

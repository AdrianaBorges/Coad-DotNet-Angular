using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Excel;

namespace COADCORP.Controllers.Financeiro
{
    public class ParcelasController : Controller
    {
        //
        // GET: /Parcelas/
        private ParcelasSRV _service = new ParcelasSRV();

        private ClientePassivelCobrancaSRV _cobrancaSrv = new ClientePassivelCobrancaSRV();

        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            return View();
        }
        [Autorizar(PorMenu = true)]
        public ActionResult Prorrogacao()
        {
            return View();
        }
        //[Autorizar(PorMenu = true)]
        public ActionResult Liquidacao()
        {
            ViewBag.Title = " Nota Fiscal";

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            List<EmpresaModel> ListaEmpresa = new EmpresaSRV().Listar();

            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");

            ViewBag.ListaBancos =  new BancosSRV().BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });

            return View();
        }

        public ActionResult GerarBoletos(string _assinatura, int _cli_id)
        {
            ViewBag.cli_id = _cli_id;
            ViewBag.assinatura = _assinatura;
            ViewBag.idAssinatura = _assinatura;

            return View();
        }

        [Autorizar(IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true, GerenteDepartamento = "SAC,TI")]
        public ActionResult AlteraSituacaoDeParcelas(ICollection<ParcelasDTO> lst)
        {
            var result = new JSONResponse();
            try
            {
                _service.AlterarSituacaoParcela(lst);
                result.success = true;

            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true, PermitirNiveisPrivilegiosSuperiores = true, GerenteDepartamento = "SAC,TI")]
        public ActionResult Salvar(ParcelasDTO _parcela, string _tipo)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.SalvarParcela(_parcela, _tipo);

                SysException.RegistrarLog("Dados da parcela atualizados com sucesso!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Dados da parcela atualizados com sucesso!!");

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
        public ActionResult BuscarTitulosProrrogados(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _situacao = null, int _pagina = 1)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var listaparcelas = new ParcelasSRV().BuscarTitulosProrrogados(_dtini, _dtfim, _situacao, _pagina);
                response.success = true;
                response.AddPage("listaparcelas", listaparcelas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult EnviarEmail(string _email, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            try
            {

                if (String.IsNullOrEmpty(_email))
                    throw new Exception("Email não informado !");

      
                var listaparcelas = new ParcelasSRV().BuscarTitulosProrrogados(_dtini, _dtfim, "PRO");


                string _mensagem = "<DIV>Este email é gerado automaticamente pelo sistema - COADCORP.  </DIV></p>" +
                                   "<DIV>Segue abaixo a lista com as parcelas que foram prorrogadas no COADCORP</DIV></p>" +
                                   "<DIV>e estão aguardando a prorrogação na instituição bancária.</DIV>" +
                                   "<DIV>Periodo de Prorrogação  " + Convert.ToDateTime(_dtini).ToString("dd/MM/yyyy") + " a " + Convert.ToDateTime(_dtfim).ToString("dd/MM/yyyy") + " </DIV><hr/>";

                 _mensagem += "<table><thead><tr><th>Nº Contrato</th><th>Nº Parcela</th><th>Alocação</th><th>Vencimento</th>"+
                                  "<th style='text-align: right;'>Valor</th></tr></thead><tbody>";


                foreach (var _item in listaparcelas)
                {
                    _mensagem += "<tr><td>" + _item.CTR_NUM_CONTRATO + "</td>" +
                                     "<td>" + _item.PAR_NUM_PARCELA + "</td>" +
                                     "<td style='text-align: center;'>" + _item.BAN_ID +"</span></td>" +
                                     "<td>" + _item.PAR_DATA_VENCTO.ToString("dd/MM/yyyy") + "</td>" +
                                     "<td style='text-align: right;'>" + _item.PAR_VLR_PARCELA + "</td></tr>";

                }

                _mensagem += "</tbody></table>";


                SessionContext.EnviarEmail(_email, "Titulos Prorrogados", _mensagem);

                SysException.RegistrarLog("Parcelas Prorrogadas - Email enviado  (" + SessionContext.autenticado.USU_LOGIN + ")", "", SessionContext.autenticado);

                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {

                SysException.RegistrarLog("Parcelas Prorrogadas - Email enviado  (" + SessionContext.autenticado.USU_LOGIN + ") " + SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);

                TempData.Add("Resultado", ex.Message);

                return View();
            }

        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarParcelasConciliacao(Nullable<DateTime> _dtini = null, 
                                                       Nullable<DateTime> _dtfim = null, 
                                                       int? _emp_id = null, 
                                                       string _ban_id = null, 
                                                       string _parcela = null,
                                                       int _tipodata = 0,
                                                       int? _tipoBaixa = null,
                                                       int? _pagina = 1)  
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_dtini == null || _emp_id == null)
                    throw new Exception("Informe todos os parametros para realizar a consulta!! ");

                var listaparcelas = new ParcelasSRV().BuscarConciliacaoTitulos((DateTime)_dtini, (DateTime)_dtfim, (int)_emp_id, _ban_id, _parcela, _tipodata, _tipoBaixa,  (int)_pagina);

                response.message = Message.Info("ok !!");
                response.success = true;
                response.AddPage("listaparcelas", listaparcelas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarEndereco(int? _cli_id)  
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_cli_id == null)
                    throw new Exception("Informe todos os parametros para realizar a consulta!! ");

                var _endereco = new ClienteEnderecoSRV().BuscarEnderecoDeFaturamentoOuEnderecoPadrao(_cli_id);

                response.success = true;
                response.Add("endereco", _endereco);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ExportarXLS(Nullable<DateTime> _dtini = null,
                                        Nullable<DateTime> _dtfim = null,
                                        int? _emp_id = null,
                                        string _ban_id = null,
                                        string _parcela = null,
                                        int _tipodata = 0,
                                        int? _tipoBaixa = null)
        {

            JSONResponse response = new JSONResponse();
            try
            {
                string _nomearquivo = null;

                var _listaparcelas = new ParcelasSRV().BuscarConciliacao((DateTime)_dtini, (DateTime)_dtfim, (int)_emp_id, _ban_id, _parcela,  _tipodata, _tipoBaixa).ToList();

                if (_nomearquivo == null)
                    _nomearquivo = DateTime.Now.Day.ToString() +
                                   DateTime.Now.Month.ToString() +
                                   DateTime.Now.Year.ToString() +
                                   DateTime.Now.Millisecond.ToString() +
                                   DateTime.Now.Minute.ToString();
                
                var _retorno = new ExcelLoad().Export(_listaparcelas, _nomearquivo, System.Web.HttpContext.Current);

                SysException.RegistrarLog("Arquivo gerado com sucesso (" + _retorno + ")", "", SessionContext.autenticado);

                response.Add("retorno", _retorno);
                response.success = true;
                response.message = Message.Info("Arquivo gerado com sucesso" + _retorno);

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
        public ActionResult BuscarPordata(Nullable<DateTime> _dtini = null, int? _emp_id = null, string _ban_id = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_dtini == null || _emp_id == null)
                    throw new Exception("Informe todos os parametros para realizar a consulta!! ");

                var listaparcelas = new ParcelasSRV().BuscarConciliacaoTitulos((DateTime)_dtini, (DateTime)_dtini, (int)_emp_id, _ban_id,null);

                response.message = Message.Info("ok !!");
                response.success = true;
                response.Add("listaparcelas", listaparcelas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarParcela(string _parcela, bool _baixamanual = false)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_parcela == null || _parcela == "")
                    throw new Exception("Nº da parcela não informada");

                var parcelaProrrog = new ParcelasSRV().BuscarParcela(_parcela, _baixamanual);

                response.message = Message.Info("ok !!");
                response.success = true;
                response.Add("parcelaProrrog", parcelaProrrog);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuscarParcelaNossoNum(string _par_nosso_numero)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_par_nosso_numero == null || _par_nosso_numero   == "")
                    throw new Exception("Nº da parcela não informada");

                var parcelaProrrog = new ParcelasSRV().BuscarParcelaNossoNumero(_par_nosso_numero);

                response.message = Message.Info("ok !!");
                response.success = true;
                response.Add("parcelaProrrog", parcelaProrrog);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult BaixaManual(ParcelasDTO _parcela)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                new ParcelasSRV().BaixaManual(_parcela);
                new AgendaCobrancaSRV().BaixaManualDeAgendamento(_parcela.ASN_NUM_ASSINATURA);

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
                response.message = Message.Fail(SysException.Show(ex));
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

    }
}

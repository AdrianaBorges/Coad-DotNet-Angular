using Coad.GenericCrud.ActionResultTools;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Service;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Relatorios
{
    public class AcessoTabelasController : Controller
    {
        //
        // GET: /AcessoTabelas/
        public void CarregarCombo()
        {
            IList<TabDinamicaConfigDTO> ListaTabRef =  new TabDinamicaConfigSRV().ListarTabDinamica(null, null, 1);
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
            ViewBag.ListaTabRef = new SelectList(ListaTabRef, "TDC_ID", "TDC_NOME_TABELA");
            ViewBag.DataAtual = DateTime.Now;

        }
        public ActionResult Index()
        {
            this.CarregarCombo();
            
            return View();
        }
        public ActionResult Acesso()
        {
            this.CarregarCombo();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Pesquisar(int _mes, int _ano, string _tdc_id, int _tipo)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                switch (_tipo)
                {
                    case 0: 
                         var listaAcessoTabelas = new LogSimuladorSRV().BuscarAcessoClientesPorPeriodo(_mes, _ano, _tdc_id);
                         response.success = true;
                         response.message = Message.Info("Ok");
                         response.Add("listaAcessoTabelas", listaAcessoTabelas);
                         break;
                    case 1:
                         var listaAcessoTabelas1 = new LogSimuladorSRV().BuscarListaClientesPorPeriodo(_mes, _ano, _tdc_id);
                         response.success = true;
                         response.message = Message.Info("Ok");
                         response.Add("listaAcessoTabelas", listaAcessoTabelas1);
                         break;
                    case 2:
                         var listaAcessoTabelas2 = new LogSimuladorSRV().BuscarTabelasPorGrupo(_mes, _ano, 0, null);
                         response.success = true;
                         response.message = Message.Info("Ok");
                         response.Add("listaAcessoTabelas", listaAcessoTabelas2);
                         break;
                }

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult PesquisarPorAssinatura(int _mes, int _ano, string _tipo_acesso, string _assinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listaAcessoTabelas2 = new LogSimuladorSRV().BuscarAcessoClientesPorPeriodo(_mes, _ano, _tipo_acesso, _assinatura);
                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("listaAcessoTabelas", listaAcessoTabelas2);
                
      
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarProdutos()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _lstprodutos = new ProdutosSRV().ListarProdutosVenda();
                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("lstprodutos", _lstprodutos);


            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarAcessoPorAssinatura(DateTime _dataini, DateTime _datafim, string _assinatura, int? _pro_id = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _lstAcessoTabelas = new AssinaturaSRV().BuscarAcessoClientesPorPeriodo(_dataini, _datafim, _assinatura, _pro_id);
                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("lstAcessoTabelas", _lstAcessoTabelas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        [Autorizar(IsAjax = true)]
        public ActionResult ExportarResumo(int _mes, int _ano, string _tdc_id)
        {

            string _nome_arquivo = "";

            JSONResponse response = new JSONResponse();
            try
            {
                   
                string[,] _planilha = null;

                var _listaAcessoTabelas = new LogSimuladorSRV().BuscarAcessoClientesPorPeriodo(_mes, _ano, _tdc_id);

                _nome_arquivo = "Planilha_"+
                                DateTime.Now.Hour.ToString() +
                                DateTime.Now.Minute.ToString() +
                                DateTime.Now.Second.ToString();


                if (_listaAcessoTabelas.Count > 0)
                {
                    _planilha = new string[(_listaAcessoTabelas.Count + 1), 4];
                    _planilha[0, 0] = "ID";
                    _planilha[0, 1] = "Assinatura";
                    _planilha[0, 2] = "Nome";
                    _planilha[0, 3] = "Total";

                    for (int lin = 0; lin <= _listaAcessoTabelas.Count - 1; lin++)
                    {
                        _planilha[(lin + 1), 0] = _listaAcessoTabelas[lin].IDTABELA;
                        _planilha[(lin + 1), 1] = _listaAcessoTabelas[lin].ASSINATURA;
                        _planilha[(lin + 1), 2] = _listaAcessoTabelas[lin].NOME_CLIENTE;
                        _planilha[(lin + 1), 3] = _listaAcessoTabelas[lin].QTDE.ToString();

                    }

                }

                                
                var _retorno = new ExcelLoad().Export(_nome_arquivo, _planilha, System.Web.HttpContext.Current);

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
                response.message = Message.Fail(COAD.CORPORATIVO.SessionUtils.SessionUtil.RecursiveShowExceptionsMessage(ex));
                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ExportarLista(int _mes, int _ano, string _tdc_id)
        {

            string _nome_arquivo = "";

            JSONResponse response = new JSONResponse();
            try
            {

                string[,] _planilha = null;

                var _listaAcessoTabelas = new LogSimuladorSRV().BuscarListaClientesPorPeriodo(_mes, _ano, _tdc_id);

                _nome_arquivo = _listaAcessoTabelas[0].NOME_TABELA;

                if (_listaAcessoTabelas.Count > 0)
                {
                    _planilha = new string[(_listaAcessoTabelas.Count + 1), 7];
                    _planilha[0, 0] = "ID";
                    _planilha[0, 1] = "Tabela";
                    _planilha[0, 2] = "Assinatura";
                    _planilha[0, 3] = "Nome";
                    _planilha[0, 4] = "Tipo";
                    _planilha[0, 5] = "Tipo Acesso";
                    _planilha[0, 6] = "Data Acesso";
                    
                    for (int lin = 0; lin <= _listaAcessoTabelas.Count - 1; lin++)
                    {
                        _planilha[(lin + 1), 0] = _listaAcessoTabelas[lin].IDTABELA;
                        _planilha[(lin + 1), 1] = _listaAcessoTabelas[lin].NOME_TABELA;
                        _planilha[(lin + 1), 2] = _listaAcessoTabelas[lin].ASSINATURA;
                        _planilha[(lin + 1), 3] = _listaAcessoTabelas[lin].NOME_CLIENTE;
                        _planilha[(lin + 1), 4] = _listaAcessoTabelas[lin].TIPO.ToString();
                        _planilha[(lin + 1), 5] = _listaAcessoTabelas[lin].TIPO_ACESSO.ToString();
                        _planilha[(lin + 1), 6] = _listaAcessoTabelas[lin].DATA_ACESSO.ToString("dd/MM/yyyy");
                    }

                }

                _nome_arquivo = "Planilha_" +
                                DateTime.Now.Hour.ToString() +
                                DateTime.Now.Minute.ToString() +
                                DateTime.Now.Second.ToString();

                var _retorno = new ExcelLoad().Export(_nome_arquivo, _planilha, System.Web.HttpContext.Current);

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
                response.message = Message.Fail(COAD.CORPORATIVO.SessionUtils.SessionUtil.RecursiveShowExceptionsMessage(ex));
                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ExportarResumo(int _mes, int _ano)
        {

            string _nome_arquivo = "";

            JSONResponse response = new JSONResponse();
            try
            {

                string[,] _planilha = null;

                var _listaAcessoTabelas = new LogSimuladorSRV().BuscarTabelasPorGrupo(_mes, _ano, 0, null);

                _nome_arquivo = "Planilha_" +
                                DateTime.Now.Hour.ToString() +
                                DateTime.Now.Minute.ToString() +
                                DateTime.Now.Second.ToString();


                if (_listaAcessoTabelas.Count > 0)
                {
                    _planilha = new string[(_listaAcessoTabelas.Count + 1), 4];
                    _planilha[0, 0] = "Grupo";
                    _planilha[0, 1] = "Tabela";
                    _planilha[0, 2] = "Período";
                    _planilha[0, 3] = "Total";

                    for (int lin = 0; lin <= _listaAcessoTabelas.Count - 1; lin++)
                    {
                        _planilha[(lin + 1), 0] = _listaAcessoTabelas[lin].grupo;
                        _planilha[(lin + 1), 1] = _listaAcessoTabelas[lin].nome;
                        _planilha[(lin + 1), 2] = _listaAcessoTabelas[lin].mes.ToString() + '/' + _listaAcessoTabelas[lin].ano.ToString();
                        _planilha[(lin + 1), 3] = _listaAcessoTabelas[lin].dados.ToString();

                    }

                }

                var _retorno = new ExcelLoad().Export(_nome_arquivo, _planilha, System.Web.HttpContext.Current);

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
                response.message = Message.Fail(COAD.CORPORATIVO.SessionUtils.SessionUtil.RecursiveShowExceptionsMessage(ex));
                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }

    }
}

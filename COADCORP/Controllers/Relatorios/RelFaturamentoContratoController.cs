using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using GenericCrud.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Relatorios
{
    public class RelFaturamentoContratoController : Controller
    {

        public void CarregarCombo()
        {
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            List<SelectListItem> ListaMeses = new List<SelectListItem>();
            List<SelectListItem> ListaAno = new List<SelectListItem>();
            IList<AreasCorpDTO> areas = new AreasSRV().FindAll();
            IList<GrupoDTO> grupos = new GrupoSRV().FindAll();

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
            ViewBag.ListaMes = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.DataAtual = DateTime.Now;
            ViewBag.areas = areas;
            ViewBag.grupos = grupos;

        }

        public ActionResult Index()
        {
            this.CarregarCombo();

            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ExportarXLS(  DateTime? _dtini
                                        , DateTime? _dtfim
                                        , int? _emp_id
                                        , int? _grupo_id
                                        , bool _tipodata)
        {

            JSONResponse response = new JSONResponse();
            try
            {

                var _nomearquivo = DateTime.Now.Day.ToString() +
                                   DateTime.Now.Month.ToString() +
                                   DateTime.Now.Year.ToString() +
                                   DateTime.Now.Millisecond.ToString() +
                                   DateTime.Now.Minute.ToString();

                var _lista = new ContratoSRV().BuscarFaturamentoContrato(_dtini, _dtfim, _emp_id, _grupo_id, _tipodata);

                var _retorno = new ExcelLoad().Export(_lista, _nomearquivo, System.Web.HttpContext.Current);

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
        public ActionResult ListarFaturamentoContrato(DateTime? _dtini 
                                                     ,DateTime? _dtfim 
                                                     ,int? _emp_id
                                                     ,string _ctr
                                                     ,string _asn
                                                     ,int? _grupo_id
                                                     ,bool _analitico = false
                                                     ,bool _tipodata = false
                                                     , int _numpagina = 1, int _linhas = 200)
        {

            JSONResponse response = new JSONResponse();


            try
            {
                var _listaFaturamentoContrato = new Pagina<RelFaturamentoContratoDTO>();

                if (!_analitico)
                {
                    _listaFaturamentoContrato = new ContratoSRV().BuscarFaturamentoContratoSint(_dtini, _dtfim, _grupo_id, _tipodata, _numpagina, _linhas);
                }
                else
                {
                    if ((_ctr != null && _ctr != "") || (_asn != null && _asn != ""))
                        _listaFaturamentoContrato = new ContratoSRV().BuscarFaturamentoContrato(_ctr, _asn);
                    else
                        _listaFaturamentoContrato = new ContratoSRV().BuscarFaturamentoContrato(_dtini, _dtfim, _emp_id, _grupo_id, _tipodata, _numpagina, _linhas);
                }

                decimal _tolcontratos = 0;

                
                response.AddPage("listaFaturamentoContrato", _listaFaturamentoContrato);
                response.Add("tolcontratos", _tolcontratos);

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

    }
}

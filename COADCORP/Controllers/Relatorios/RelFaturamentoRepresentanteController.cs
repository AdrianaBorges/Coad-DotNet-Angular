using Coad.GenericCrud.ActionResultTools;
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
    public class RelFaturamentoRepresentanteController : Controller
    {
        //
        // GET: /ContratoRepresentante/
        //[Autorizar(IsAjax = true)]
        public void Carregarlistas()
        {
            IList<GrupoDTO> grupos = new GrupoSRV().FindAll();
            IList<AreasCorpDTO> areas = new AreasSRV().FindAll();
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            List<SelectListItem> ListaMeses = new List<SelectListItem>();
            
            ListaMeses.AddRange(new[]{
                            new SelectListItem() { Text = "Janeiro", Value = "1" },
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


            ViewBag.areas = areas;
            ViewBag.grupos = grupos;
            ViewBag.ListaMes = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
            ViewBag.ListaUf = new SelectList(new UFSRV().FindAll().ToList(), "UF_SIGLA", "UF_DESCRICAO");
            
        }

        public ActionResult Index()
        {
            this.Carregarlistas();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarListaRepresentantes()
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _lstrepresentante = new RepresentanteSRV().BuscarRepresentantes();

                response.success = true;
                response.Add("lstrepresentante", _lstrepresentante);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarListaEmpresa()
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _lstempresa = new EmpresaSRV().FindAll().ToList();

                response.success = true;
                response.Add("lstempresa", _lstempresa);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ExportarXLS(List<RelFaturamentoRepresentanteDTO> _lista,
                                        List<RelFaturamentoRepresentanteSintDTO> _lista1,
                                        string _nomearquivo = null)
        {

            JSONResponse response = new JSONResponse();
            try
            {

                if (_nomearquivo == null)
                    _nomearquivo = DateTime.Now.Day.ToString() +
                                   DateTime.Now.Month.ToString() +
                                   DateTime.Now.Year.ToString() +
                                   DateTime.Now.Millisecond.ToString() +
                                   DateTime.Now.Minute.ToString();
                //  throw new Exception("Nome do arquivo não informado!!");

                string _retorno = null;

                if (_lista.Count > 0)
                   _retorno = new ExcelLoad().Export(_lista, _nomearquivo, System.Web.HttpContext.Current);
                else
                    _retorno = new ExcelLoad().Export(_lista1, _nomearquivo, System.Web.HttpContext.Current);

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
        public ActionResult BuscarContratoRepData(DateTime _dtini, DateTime _dtfim, int? _emp_id, int? _rep_id, int? _grupo_id, bool _ordalfabetica = false, int _tipo = 0)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                IList<RelFaturamentoRepresentanteSintDTO> _listacontratorepSint = null;
                IList<RelFaturamentoRepresentanteDTO> _listacontratorep = null;

                if (_ordalfabetica == false)
                    _listacontratorep = new ContratoSRV().BuscarFaturamentoRepresentante(_dtini, _dtfim, _emp_id, _rep_id, _grupo_id, _tipo);
                else
                    _listacontratorepSint = new ContratoSRV().BuscarFaturamentoRepresentanteSint(_dtini, _dtfim, _emp_id, _rep_id, _grupo_id);

                decimal _tolcontratos = 0;
                decimal _tolvenda = 0;
                decimal _tolrenovacao = 0;

                if (_ordalfabetica == false)
                {
                    foreach (var item in _listacontratorep)
                    {
                        _tolcontratos += (decimal)item.VALOR_TOTAL;
                    }
                }
                else
                {
                    foreach (var item in _listacontratorepSint)
                    {
                        _tolcontratos += (decimal)item.VALOR_TOTAL;
                        _tolvenda += (decimal)item.VALOR_VENDA;
                        _tolrenovacao += (decimal)item.VALOR_RENOVACAO;
                    }
                }

                response.success = true;
                response.Add("listacontratorep", _listacontratorep);
                response.Add("listacontratorepSint", _listacontratorepSint);
                response.Add("tolcontratos", _tolcontratos);
                response.Add("tolvenda", _tolvenda);
                response.Add("tolrenovacao", _tolrenovacao);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarContratoRep(int? _mes, int? _ano, int? _emp_id, int? _rep_id, int? _grupo_id, bool _ordalfabetica = false, int _tipo= 0)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                IList<RelFaturamentoRepresentanteSintDTO> _listacontratorepSint = null;
                IList<RelFaturamentoRepresentanteDTO> _listacontratorep = null;

                if (_mes == null || _ano == null)
                    throw new Exception("Informe todos os parametros para realizar a consulta!! ");

                if (_ordalfabetica == false)
                    _listacontratorep = new ContratoSRV().BuscarFaturamentoRepresentante(_mes, _ano, _emp_id, _rep_id, _grupo_id, _tipo);
                else
                    _listacontratorepSint = new ContratoSRV().BuscarFaturamentoRepresentanteSint(_mes, _ano, _emp_id, _rep_id, _grupo_id);

                decimal _tolcontratos = 0;
                decimal _tolvenda = 0;
                decimal _tolrenovacao = 0;

                if (_ordalfabetica == false)
                {
                    foreach (var item in _listacontratorep)
                    {
                        _tolcontratos += (decimal)item.VALOR_TOTAL;
                    }
                }
                else
                {
                    foreach (var item in _listacontratorepSint)
                    {
                        _tolcontratos += (decimal)item.VALOR_TOTAL;
                        _tolvenda += (decimal)item.VALOR_VENDA;
                        _tolrenovacao += (decimal)item.VALOR_RENOVACAO;
                    }
                }

                response.success = true;
                response.Add("listacontratorep", _listacontratorep);
                response.Add("listacontratorepSint", _listacontratorepSint);
                response.Add("tolcontratos", _tolcontratos);
                response.Add("tolvenda", _tolvenda);
                response.Add("tolrenovacao", _tolrenovacao);

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

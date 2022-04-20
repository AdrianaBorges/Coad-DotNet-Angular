﻿using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
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
    public class RelFaturamentoUFController : Controller
    {
        public void CarregarCombo()
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
            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.ListaMes = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
            ViewBag.ListaUf = new SelectList(new UFSRV().FindAll().ToList(), "UF_SIGLA", "UF_DESCRICAO");
            
        }


        public ActionResult Produto()
        {
            this.CarregarCombo();

            return View();
        }

        public ActionResult Index()
        {
            this.CarregarCombo();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ExportarXLS(List<RPT_CONTRATOS_POR_REGIAO_Result> _lista, string _nomearquivo = null)
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
        public ActionResult BuscarFaturamentoUF( DateTime _dtini
                                               , DateTime _dtfim
                                               , int _emp_id = 0
                                               , int _grupo_id = 0
                                               , bool _ordalfabetica = false
                                               , bool _tipodata = false)
        {
            

            JSONResponse response = new JSONResponse();

            try
            {
                var _lstFaturamentoUF = new ContratoSRV().BuscarFaturamentoUF(_dtini, _dtfim, _emp_id, _grupo_id, _ordalfabetica, _tipodata);
                decimal? _total = 0;
                decimal? _totalcanc = 0;

                foreach (var item in _lstFaturamentoUF)
                {
                    _total += (item.VALOR == null) ? 0 : item.VALOR;
                    _totalcanc += (item.CANCELADOS == null) ? 0 : item.CANCELADOS;
                }


                response.Add("lstFaturamentoUF", _lstFaturamentoUF);
                response.Add("total", _total);
                response.Add("totalcanc", _totalcanc);


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
        


        public ActionResult BuscarFaturamentoProdutoUF( DateTime _dtini
                                                      , DateTime _dtfim
                                                      , int _emp_id = 0
                                                      , int _grupo_id = 0
                                                      , bool _tipodata = false)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _lstFaturamentoProdUF = new ContratoSRV().BuscarFaturamentoProdutoUF(_dtini, _dtfim, _emp_id, _grupo_id, _tipodata);
                decimal? _total = 0;
                decimal? _totalcanc = 0;

                foreach (var item in _lstFaturamentoProdUF)
                {
                    _total += (item.VALOR == null) ? 0 : item.VALOR;
                    _totalcanc += (item.CANCELADOS == null) ? 0 : item.CANCELADOS;
                }
                
                response.Add("lstFaturamentoProdUF", _lstFaturamentoProdUF);
                response.Add("total", _total);
                response.Add("totalcanc", _totalcanc);


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

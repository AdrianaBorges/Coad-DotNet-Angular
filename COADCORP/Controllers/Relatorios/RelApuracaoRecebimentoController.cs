using Coad.GenericCrud.ActionResultTools;
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
    public class RelApuracaoRecebimentoController : Controller
    {
        public void CarregarCombo()
        {
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            List<SelectListItem> ListaMeses = new List<SelectListItem>();
            List<SelectListItem> ListaAno = new List<SelectListItem>();
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

            ViewBag.banco = new BancosSRV().BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });
            ViewBag.bco = new BancosSRV().FindAll().Select(c => new SelectListItem() { Text = c.BAN_ID + " " + c.BAN_NOME, Value = c.BAN_ID });
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
            ViewBag.ListaMes = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.DataAtual = DateTime.Now;
            ViewBag.grupos = grupos;

        }

        [Autorizar(IsAjax = true)]
        public ActionResult Index()
        {
            this.CarregarCombo();
            
            return View();
        }


        [Autorizar(IsAjax = true)]
        public ActionResult ExportarXLS(List<APURAR_RECEBIMENTO_Result> _lista, string _nomearquivo = null)
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
        public ActionResult BuscarApuraRecebimento(Nullable<DateTime> _dtini, Nullable<DateTime> _dtfim
                                                  , int _emp_id = 0
                                                  , string _ban_id = null
                                                  , int _gru_id = 0
                                                  , int _tipobaixa = 0)
        {
            


            JSONResponse response = new JSONResponse();

            try
            {

                var _lstTotal = new ContratoSRV().BuscarApuraRecebimento(_dtini, _dtfim, _emp_id, _ban_id, _gru_id, _tipobaixa);

                var _lstGeral = _lstTotal.LISTA;

                response.Add("lstTotal", _lstTotal);
                response.Add("lstGeral", _lstGeral);

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

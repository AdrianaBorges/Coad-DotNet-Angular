using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Relatorios
{
    public class RelResumoCReceberController : Controller
    {
        public void CarregarCombo()
        {
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


            ViewBag.ListaMes = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
            ViewBag.ListaUf = new SelectList(new UFSRV().FindAll().ToList(), "UF_SIGLA", "UF_DESCRICAO");
            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.DataAtual = DateTime.Now;

        }

        [Autorizar(IsAjax = true)]
        public ActionResult Index()
        {
            this.CarregarCombo();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarResumoCReceber( DateTime _dtini
                                                , DateTime _dtfim
                                                , int _emp_id
                                                , bool _tipodata)
        {
           


            JSONResponse response = new JSONResponse();

            try
            {
                var _lstResumoCReceber = new ContratoSRV().BuscarResumoCReceber(_dtini, _dtfim, _emp_id, _tipodata);
                
                decimal? _totalcanc = 0;
                decimal? _totalfat = 0;
                decimal? _totalpago = 0;
                decimal? _totalreceber = 0;

                foreach (var item in _lstResumoCReceber)
                {
                    _totalcanc += item.VALOR_CANCELADO;
                    _totalfat += item.VALOR_FATURADO;
                    _totalpago += item.VALOR_PAGO;
                    _totalreceber += item.VALOR_RECEBER;
                }


                response.Add("lstResumoCReceber", _lstResumoCReceber);
                response.Add("totalcanc", _totalcanc);
                response.Add("totalfat", _totalfat);
                response.Add("totalpago", _totalpago);
                response.Add("totalreceber", _totalreceber);


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

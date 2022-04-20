using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
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

namespace COADCORP.Controllers
{
    public class RelAlocadosController : Controller
    {
        [Autorizar(IsAjax = true)]
        public ActionResult ExportarXLS(DateTime _dtini, DateTime _dtfim, string _banid = null, int _grupoid = 0)
        {

            JSONResponse response = new JSONResponse();
            try
            {
                string _nomearquivo = null;

                var _lstalocados = new List<ParcelasDTO>();

                if (_nomearquivo == null)
                    _nomearquivo = DateTime.Now.Day.ToString() +
                                   DateTime.Now.Month.ToString() +
                                   DateTime.Now.Year.ToString() +
                                   DateTime.Now.Millisecond.ToString() +
                                   DateTime.Now.Minute.ToString();

                var _retorno = new ExcelLoad().Export(_lstalocados, _nomearquivo, System.Web.HttpContext.Current);

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
        public ActionResult ListarTitulosAlocados(DateTime _dtini, DateTime _dtfim,  string _banid = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _lstalocados = new ParcelasSRV().ListarTitulosAlocados(_dtini, _dtfim, _banid);

                response.Add("lstalocados", _lstalocados);

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
        public void Carregarlistas()
        {
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();

            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");

            ViewBag.grupos = new GrupoSRV().FindAll();

            ViewBag.ListaBancos = new BancosSRV().BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });


        }
        public ActionResult Index()
        {
            this.Carregarlistas();

            return View();
        }


    }
}

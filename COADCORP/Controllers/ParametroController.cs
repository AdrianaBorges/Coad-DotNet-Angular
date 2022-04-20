using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class ParametroController : Controller
    {
        
        public ActionResult Index()
        {
            this.CarregarListas();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public void CarregarListas()
        {
            ViewBag.empresa = new EmpresaSRV().FindAll().Select(c => new SelectListItem() { Text = c.EMP_ID.ToString() + " - " + c.EMP_NOME_FANTASIA, Value = c.EMP_ID.ToString() });
            ViewBag.conta = new ContaSRV().Listar(null, true).OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.CTA_AGENCIA + "/" + c.CTA_CONTA, Value = c.CTA_ID.ToString() });
            
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarParametroGrupo()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _lstParametroGrupo = new ParametroGrupoSRV().FindAll();

                if (_lstParametroGrupo.Count() == 0)
                    throw new Exception("Nenhum registro encontrado para o período selecionado!!");

                response.success = true;
                response.Add("lstParametroGrupo", _lstParametroGrupo);

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
        public ActionResult BuscarParametros(int _pgr_id)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _lstParametros = new ParametrosSRV().Listar(_pgr_id);

                response.success = true;
                response.Add("lstParametros", _lstParametros);

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
        public ActionResult Salvar(List<ParametrosDTO> _lstParametros)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                new ParametrosSRV().MergeAll(_lstParametros);

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

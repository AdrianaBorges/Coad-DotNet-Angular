using Coad.GenericCrud.ActionResultTools;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class ModuloController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Editar(int? _mod_id)
        {
            ViewBag.modid = _mod_id;

            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult Init(int? _mod_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var item =  new ManualDPModuloSRV().FindById(_mod_id);

                response.success = true;
                response.Add("item", item);
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex) , SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex) , SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [Autorizar(IsAjax = true)]
        public ActionResult Salvar(ManualDPModuloDTO _modulo)
        {

            JSONResponse response = new JSONResponse();

            var itematualizado = _modulo.MOD_ID.ToString() + " - " + _modulo.MOD_DESCRICAO;

            try
            {
                new ManualDPModuloSRV().SaveOrUpdate(_modulo);

                SysException.RegistrarLog("Dados atualizados com sucesso!! (" + itematualizado + ")", "", SessionContext.autenticado);

                response.success = true;
                response.message = Message.Info("Dados atualizados com sucesso!!");
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex) + " Erro (" + itematualizado + ")", SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex) + " Erro (" + itematualizado + ")", SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [Autorizar(IsAjax = true)]
        public ActionResult Excluir(ManualDPModuloDTO _modulo)
        {

            JSONResponse response = new JSONResponse();

            var itematualizado = _modulo.MOD_ID.ToString() + " - " + _modulo.MOD_DESCRICAO;

            try
            {
                new ManualDPModuloSRV().Delete(_modulo);

                SysException.RegistrarLog("Módulo excluído com sucesso!! (" + itematualizado + ")", "", SessionContext.autenticado);

                response.success = true;
                response.message = Message.Info("Módulo excluído com sucesso!!");
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex) + " Erro (" + itematualizado + ")", SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex) + " Erro (" + itematualizado + ")", SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }


    }
}

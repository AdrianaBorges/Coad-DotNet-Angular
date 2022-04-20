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

namespace COADPORTAL.Controllers
{
    public class NoticiaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Geral(int? id)
        {
            if (id == null)
                id = 0;

            ViewBag.id = id;

            return View();
        }
     //   [Autorizar(IsAjax = true)]
        public ActionResult CarregarTela(int? _id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _noticia = new NoticiaDTO();

                if (_id != null && _id != 0)
                {
                    _noticia = new NoticiaSRV().FindById(_id);
                }

                response.Add("noticia", _noticia);
                response.success = true;
                response.message = Message.Info("Ok");

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

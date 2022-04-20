using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Repositorios.Base;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Model;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;

namespace CONFIGSIS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MudaPerfil(string _per_id)
        {
            SessionContext.autenticado.PER_ID = _per_id;
            SessionContext.menu_usuario = new UsuarioSRV().MontaMenu(SessionContext.autenticado.PER_ID, SessionContext.autenticado.EMP_ID, SessionContext.autenticado.SIS_ID, SessionContext.autenticado.ADMIN);

            return View("Index");

        }

        public ActionResult Error()
        {
            JSONResponse response = new JSONResponse()
            {
                message = Message.Fail(TempData["message"] as string)
            };
            response.success = false;
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Info()
        {
            return View();

        }

        public ActionResult Falha(string cod)
        {
            if (cod != null)
            {
                switch (cod)
                {
                    case "PRE_CART01":
                        {
                            ViewData["message"] = "O representante logado não possui nenhuma carteira. É necessario possuir ao menos uma carteira para utilizar essa funcionalidade.";
                            break;
                        }
                }
            }
            return View();
        }

        [Autorizar(Admin = true)]
        public ActionResult TestaEmail()
        {

            return View();
        }


        [Autorizar(Admin = true, IsAjax = true)]
        public ActionResult EnviarEmail(EmailRequestDTO email)
        {
            JSONResponse response = new JSONResponse();

            if (ModelState.IsValid)
            {

                try
                {
                    EmailSRV _mailSRV = new EmailSRV();
                    _mailSRV.EnviarEmail(email);

                    response.message = Message.Success("Email enviado com sucesso");
                }
                catch (Exception e)
                {
                    response.success = false;
                    response.message = Message.Fail(e);
                    response.Add("erro", SessionUtil.RecursiveShowExceptionsMessage(e));
                    SessionUtil.HandleException(e);
                }

            }
            else
            {
                response.SetMessageFromModelState(ModelState);
                response.success = false;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
         
    }
}

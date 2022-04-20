using Coad.GenericCrud.ActionResultTools;
using GenericCrud.Controllers;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Config;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Service.Custons;
using COAD.SEGURANCA.Filter;

namespace COADCORP.Controllers
{
    public class EmailController : Controller
    {

        CustomEmailSRV emailSRV = new CustomEmailSRV();
        EmailConfig emailConfig = new EmailConfig();

        public EmailController() : base()
        {

        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Editar()
        {

            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregarEmails()
        {

            JSONResponse response = new JSONResponse();

            try
            {

                var lstEmails = emailSRV.LerMails();
                response.Add("lstEmails", lstEmails);

            }
            catch (Exception e)
            {

                response.success = false;
                response.message = Message.Fail(e);

            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult SelecionarEmail(String id)
        {

            JSONResponse response = new JSONResponse();

            try
            {

                List<Email> lstEmails = emailSRV.LerMails();

                if ( !id.Equals("0") )
                { 

                    foreach (Email emailTemp in lstEmails)
                        if (emailTemp.Id.Replace("+", " ").Equals(id))
                            response.Add("emailSelecionado", emailTemp);
                }
                else
                    response.Add("emailSelecionado", lstEmails.First());

            }
            catch (Exception e)
            {

                response.success = false;
                response.message = Message.Fail(e);

            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult IniciarEmail()
        {

            JSONResponse response = new JSONResponse();

            try
            {

                Email email = new Email();

                response.Add("email", email);

            }
            catch (Exception e)
            {

                response.success = false;
                response.message = Message.Fail(e);

            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult EnviarEmail(Email email)
        {

            JSONResponse response = new JSONResponse();

            try
            {

                email.De = emailConfig._username.Split(':').Last();
                email.Data = DateTime.Now;

                emailSRV.EnviarEmail(email);                
                response.success = true;

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

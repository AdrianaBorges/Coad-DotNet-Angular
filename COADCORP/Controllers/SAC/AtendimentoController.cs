using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.SAC
{
    public class AtendimentoController : Controller
    {
        //
        // GET: /Atendimento/
        [Autorizar(IsAjax = false)]
        public ActionResult Index()
        {
            return View();
        }
        [Autorizar(IsAjax = false)]
        public ActionResult Email()
        {
            ViewBag.email = SessionContext.autenticado.EMAIL;

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarEmail(string _email, string _senha)
        {

            if (_email == null)
            {
                _email = SessionContext.autenticado.EMAIL;
               // _senha = SessionContext.Descriptografar(SessionContext.autenticado.EMAIL_SENHA);
                _senha = SessionContext.autenticado.EMAIL_SENHA;
            }

            JSONResponse response = new JSONResponse();

            try
            {

                if (String.IsNullOrEmpty(_email) || String.IsNullOrEmpty(_senha))
                    throw new Exception("Email não informado");

                EmailAtendSRV _srvEmail = new EmailAtendSRV();
                
                var _listaemails = _srvEmail.BuscarEmails(_email, _senha);
                
                response.AddPage("listaemails", _listaemails);
                response.success = true;
                response.message = Coad.GenericCrud.ActionResultTools.Message.Info("Ok");

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
                response.message = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }           
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarAnexo(int _eat_id)
        {

            if (_eat_id == 0)
                throw new Exception("Informe o id do email");
         
            JSONResponse response = new JSONResponse();

            try
            {

                EmailAtendAnexoSRV _srvEmailAnexo = new EmailAtendAnexoSRV();

                var _listaanexo = _srvEmailAnexo.BuscarPorEmail(_eat_id);

                foreach (var _item in _listaanexo)
                {
                    string _filePath = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + _item.ANX_ANEXO_NOMEARQ;
                    System.IO.File.WriteAllBytes(_filePath, _item.ANX_ANEXO);
                }

                response.Add("listaanexo", _listaanexo);
                response.success = true;
                response.message = Coad.GenericCrud.ActionResultTools.Message.Info("Ok");

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
                response.message = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

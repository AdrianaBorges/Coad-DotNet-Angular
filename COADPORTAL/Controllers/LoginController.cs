
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADPORTAL.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            ViewBag.TelaTopo = "LOGIN";
            ViewBag.Tela = "LOGIN";
            return View();
        }
        
        public ActionResult Login(string _login, string _senha)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                string _retorno = "";

                new ClienteSRV().RealizarLogin(_login, _senha, 8, System.Web.HttpContext.Current);

                if (_retorno.Trim() != "")
                    new CadastroGratuitoSRV().RealizarLogin(_login, _senha, "COADCORP", System.Web.HttpContext.Current);

                if (_retorno.Trim() != "")
                    throw new Exception(_retorno);

                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LembrarSenha()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LembrarSenha(string _login, string _email)
        {
            string url = HttpContext.Request.Url.AbsoluteUri;
            string path = HttpContext.Request.Url.AbsolutePath;
            Autenticado _autenticado = new Autenticado();

            Random randNum = new Random();

            try
            {
                if (_login.Length <= 0 && _email.Length <= 0)
                    throw new Exception("Login e/ou Email não informados !");

                ClienteDto _usuario = new ClienteSRV().ValidarLoginEmail(_email);

                if (_usuario == null)
                    throw new Exception("Email não encontrado!!");

                _autenticado.USU_LOGIN = _login;
                _autenticado.IP_ACESSO = SessionContext.GetIp();
                _autenticado.PATH = url;
                _autenticado.SESSION_ID = this.Session.SessionID;

                string _novasenha = randNum.Next(2113).ToString() + _login[1] + _login[2] + randNum.Next(2003).ToString();
                string _mensagem = "<DIV>Caro " + _login + ", este email é gerado automaticamente pelo sistema - COADCORP.  </DIV></p>" +
                                   "<DIV>Conforme a sua solicitação, o sistema gerou uma senha automatica, aleatória e provisória.</DIV></p>" +
                                   "<DIV>Voçe deve realizar o login com esta senha provisória e em seguida realizar o cadastramento da sua senha definitiva.</DIV>" +
                                   "<DIV>Senha Provisória => " + _novasenha + " </DIV>";


                new ClienteSRV().AlterarSenha(_usuario);

                SessionContext.EnviarEmail(_email, "Nova Senha", _mensagem);

                SysException.RegistrarLog("Solicitação de Senha - Usuário  (" + _autenticado.USU_LOGIN + ")", "", _autenticado);

                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                _autenticado.USU_LOGIN = _login;
                _autenticado = new Autenticado();
                _autenticado.PATH = url;
                _autenticado.IP_ACESSO = SessionContext.GetIp();

                SysException.RegistrarLog("Solicitação de Senha - Usuário (" + _login + ") " + SysException.Show(ex), SysException.ShowIdException(ex), _autenticado);

                TempData.Add("Resultado", ex.Message);

                return View();
            }

        }
        

    }
}

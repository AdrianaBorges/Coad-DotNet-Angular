using System;
using System.Web;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace COADCORP.Controllers.Seguranca
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string _login, string _senha)
        {
            try
            {   
                string _retorno = new UsuarioSRV().RealizarLogin(_login, _senha, "COADCORP", System.Web.HttpContext.Current);

                if (_retorno.Trim() != "")
                    TempData.Add("Resultado", _retorno);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                TempData.Add("Resultado", ex.Message);

                return View();
            }
            
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

                USUARIO _usuario = new UsuarioSRV().ValidarLoginEmail(_login, _email);

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
                                   "<DIV>Senha Provisória => "+_novasenha+" </DIV>";

                _usuario.USU_NOVA_SENHA = 1;
                _usuario.USU_SENHA = _novasenha;

                new UsuarioSRV().AlterarSenha(_usuario);

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
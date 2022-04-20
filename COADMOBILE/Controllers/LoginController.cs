using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using COAD.PORTAL.Service;
using System.Web.UI;
using System.Web.Mvc.Html;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
//using COAD.PORTAL.Service.SevicesPortalCoad;
using COAD.CORPORATIVO.Service;

namespace COADMOBILE.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string _login, string _senha)
        {
            try
            {
                ClienteSRV clientesrv = new ClienteSRV();
                //var _cliente = clientesrv.LoginCliente(_login, _senha);
                var _cliente = clientesrv.BuscarClientesGeral(cpf_cnpj: "12414386762"/*, email: email*/).lista.FirstOrDefault();
                string _retorno = new UsuarioSRV().RealizarLogin("USERPMOBILE", "tr0p4c04dc0rp", "COADMOBILE", System.Web.HttpContext.Current);

                if (_retorno.Trim() != "")
                    TempData.Add("Resultado", _retorno);


                HttpCookie myCookie = new HttpCookie("InfClientes");
                myCookie.Values["IdCliente"] = _cliente.CLI_ID.ToString();
                myCookie.Values["UserName"] = _cliente.CLI_NOME;
                myCookie.Expires = DateTime.Now.AddDays(365);

                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);

                TempData.Add("InfClientes", _cliente.CLI_NOME);

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
                _autenticado.IP_ACESSO = GetIp();
                _autenticado.PATH = url;
                _autenticado.SESSION_ID = this.Session.SessionID;

                string _novasenha = randNum.Next(2113).ToString() + _login[1] + _login[2] + randNum.Next(2003).ToString();
                string _mensagem = "<DIV>Caro " + _login + ", este email é gerado automaticamente pelo sistema - COADCORP.  </DIV></p>" +
                                   "<DIV>Conforme a sua solicitação, o sistema gerou uma senha automatica, aleatória e provisória.</DIV></p>" +
                                   "<DIV>Voçe deve realizar o login com esta senha provisória e em seguida realizar o cadastramento da sua senha definitiva.</DIV>" +
                                   "<DIV>Senha Provisória => " + _novasenha + " </DIV>";

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
                _autenticado.IP_ACESSO = GetIp();

                SysException.RegistrarLog("Solicitação de Senha - Usuário (" + _login + ") " + SysException.Show(ex), SysException.ShowIdException(ex), _autenticado);

                TempData.Add("Resultado", ex.Message);

                return View();
            }

        }
        
        public string GetIp()
        {
            string strHostName = Dns.GetHostName();
            string _retorno = "";

            IPAddress[] ip = Dns.GetHostAddresses(strHostName);

            for (int i = 0; i <= ip.Count() - 1; i++)
            {
                if (ip[1].ToString().Length <= 17)
                {
                    _retorno = ip[1].ToString();
                    break;
                }
            }

            return _retorno;

        }

        public ActionResult Logout()
        {
            if (SessionContext.autenticado != null)
            {
                SysException.RegistrarLog("LogOff Usuário (" + SessionContext.autenticado.USU_LOGIN + ")", "", SessionContext.autenticado);

                Response.Expires = 0;
                Response.ExpiresAbsolute = DateTime.Now;
                Response.AddHeader("pragma", "no-cache");
                Response.AddHeader("cache-control", "private");
                Response.CacheControl = "no-cache";
                Session.Contents.RemoveAll();
                Session.Abandon();

            }
            if (Request.Cookies["InfClientes"] != null)
            {
                HttpCookie myCookie = new HttpCookie("InfClientes");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult LimparCache()
        {
            //SysException.RegistrarLog("LogOff Usuário (" + SessionContext.autenticado.USU_LOGIN + ")", "", SessionContext.autenticado);

            //Response.Expires = 0;
            //Response.ExpiresAbsolute = DateTime.Now;
            //Response.AddHeader("pragma", "no-cache");
            //Response.AddHeader("cache-control", "private");
            //Response.CacheControl = "no-cache";
            //Session.Contents.RemoveAll();
            //Session.Abandon();

            if (Request.Cookies["InfClientes"] != null)
            {
                HttpCookie myCookie = new HttpCookie("InfClientes");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            return RedirectToAction("Login", "Login");
        }
    }
}

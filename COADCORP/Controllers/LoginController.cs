using System;
using System.Web.Mvc;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Util;
using COAD.UTIL.Helpers;

namespace COADCORP.Controllers
{
    public class LoginController : Controller
    {
        public UsuarioSRV _usuarioSRV { get; set; }

        //private static CoadCorpLog logHelper = new CoadCorpLog(typeof(LoginController));

        public ActionResult Login()
        {
            if (SessionContext.EhAutenticado())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
        public ActionResult ValidarLogin(string _login, string _senha)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                if (ModelState.IsValid)
                {
                    if (_usuarioSRV == null)
                        _usuarioSRV = new UsuarioSRV();

                    if (String.IsNullOrWhiteSpace(_login) ||
                        String.IsNullOrWhiteSpace(_senha))
                        throw new Exception("  Login ou Senha Inválidos. Verifique !!!");


                    string _retorno = "";

                    if (_login.IndexOf(Convert.ToChar("@")) > 0)
                        new CadastroGratuitoSRV().RealizarLogin(_login, _senha, "COADCORP", System.Web.HttpContext.Current);
                    else
                        _retorno = _usuarioSRV.RealizarLogin(_login, _senha, "COADCORP", System.Web.HttpContext.Current);

                    int? REP_ID = null;

                    if (SessionContext.HasDepartamento("Franquiado"))
                    {
                        if (AuthUtil.TryGetRepId(out REP_ID) && !SessionContext.IsGerenteDepartamento("Franquiado"))
                        {
                            new RepresentanteSRV().ChecaEInsereFilaRepresentante(REP_ID);
                        }
                    }

                    return Json(response, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    response.success = false;
                    response.SetMessageFromModelState(ModelState);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                //logHelper.Error("Erro ao validar usuário. ", ex);

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
        public ActionResult LembrarSenha()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GerarSenhaTemporaria(string _login, string _email)
        {
            string url = HttpContext.Request.Url.AbsoluteUri;
            string path = HttpContext.Request.Url.AbsolutePath;
            Autenticado _autenticado = new Autenticado();

            Random randNum = new Random();

            JSONResponse response = new JSONResponse();
            try
            {
                if (String.IsNullOrWhiteSpace(_login) ||
                    String.IsNullOrWhiteSpace(_email))
                    throw new Exception("  Login ou Email Inválidos. Verifique !!!");

                new EmailSRV().LembrarSenha(_login,_email,this.Session.SessionID,url);

                response.success = true;
                response.message = Message.Info("A senha foi enviada para o email ("+ _email +"). Em alguns instantes verifique seu email!");
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog("Solicitação de Senha - Usuário (" + _login + ") " + SysException.Show(ex), SysException.ShowIdException(ex), _autenticado);

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

        /// <summary>
        /// Checa o SessionUtil com o método específico para validar autenticação
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        [Autorizar(IsAjax = true)]
        public JsonResult ValidarPermissao(string methodName)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var possuiPermissao = SessionUtil.ValidarPermissaPorNomeMetodo(methodName);
                response.Add("possuiPermissao", possuiPermissao);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
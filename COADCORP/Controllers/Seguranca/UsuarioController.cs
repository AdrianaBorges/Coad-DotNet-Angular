using System;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COADCORP.Controllers.Seguranca
{
    public class UsuarioController : Controller
    {
    
        public ActionResult AlterarSenha()
        {
            AlterarSenha _novasenha = new AlterarSenha();

            _novasenha.Login =  SessionContext.autenticado.USU_LOGIN;

            ViewBag.Title = "Alterar Senha";

            return View(_novasenha);
        }

        [HttpPost]
        public ActionResult AlterarSenha(AlterarSenha _novasenha)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.Title = "Alterar Senha";

                    USUARIO _usuario = new USUARIO();

                    _usuario.USU_LOGIN = _novasenha.Login;
                    _usuario.USU_SENHA = _novasenha.Senha;
                    _usuario.USU_NOVA_SENHA = 0;

                    new UsuarioSRV().AlterarSenha(_usuario);

                    TempData.Add("Resultado", "Senha alterada com sucesso.");

                    SysException.RegistrarLog("Senha do usuário alterada com sucesso (" + _novasenha.Login + ")", "", SessionContext.autenticado);

                    return  View();
                }
                catch (Exception ex)
                {
                    ViewBag.Title = "Alterar Senha";

                    ModelState.AddModelError("error", SysException.Show(ex));

                    SysException.RegistrarLog("Erro ao alterar a senha do usuário " + _novasenha.Login + " (" + SysException.Show(ex) + ") ", SysException.ShowIdException(ex), SessionContext.autenticado);

                    return View();
                }
            }
            
            _novasenha.Login = SessionContext.autenticado.USU_LOGIN;

            ViewBag.Title = "Alterar Senha";

            return View();
        }
        public ActionResult Carregar(int emp_id)
        {
            try
            {
                string _per_id = "";
                string _sis_id = "";

                List<PerfilModel> a = new PerfilSRV().BuscarLista(emp_id, _per_id, _sis_id).ToList();

                return Json(a, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", SysException.Show(ex));

                return View("Index");
            }

        }
        public ActionResult CarregarPerfilUsuario(string usu_login)
        {
            try
            {
                List<PerfilUsuarioModel> a = new PerfilUsuarioSRV().BuscarLista(usu_login).ToList();

                return Json(a, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", SysException.Show(ex));

                return View("Index");
            }

        }

    }
}

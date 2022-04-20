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
using COAD.SEGURANCA.Filter;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;

namespace COADCORP.Controllers
{
    public class UsuarioController : Controller
    {

        private UsuarioSRV _service = new UsuarioSRV();

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

        [Autorizar(IsAjax = true)]
        public ActionResult ListaDeUsuarioSemRepresentante()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var listaUsuario = _service.UsuarioAutoCompleteDTO(true);
                response.Add("listaUsuario", listaUsuario);
             
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult RecuperarDadosDoUsuario(string usu_login)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = _service.FindByIdFullLoaded(usu_login);
                response.Add("usuario", usuario);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarUsuariosSemRepresentante(string login,
            string nome,
            string cpf,
            bool cpfExato,
            string email,
            int pagina = 1,
            int registrosPorPagina = 5)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var lstUsuarios = _service.BuscarUsuarios(login, nome, cpf, cpfExato, email, true, pagina, registrosPorPagina);
                result.AddPage("lstUsuarios", lstUsuarios);

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}

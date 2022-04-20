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
using CONFIGSIS.Models;


namespace CONFIGSIS.Controllers
{
    public class UsuariooldController : Controller
    {
        //
        // GET: /Usuario/

        public ActionResult Index(int? _emp_id, string _per_id, string _usu_login)
        {
            try
            {
                ViewBag.Title = "Usuários (Consultar)";

                List<PERFIL_USUARIO> a = new UsuarioSRV().ListarPorPerfil(_emp_id, _per_id, _usu_login);
                List<PERFIL> listaPerfil = new PerfilSRV().BuscarTodos().ToList();
                var listaemp = new EmpresaSRV().FindAll().ToList();

                ViewBag.listaPerfil = new SelectList(listaPerfil, "perId", "perId");
                ViewBag.listaemp = new SelectList(listaemp, "EMP_ID", "EMP_RAZAO_SOCIAL");

                if (a.Count == 0)
                    throw new Exception("Nenhum resultado encontrado para a consulta.");

                return View(a);
            }
            catch (Exception ex)
            {
                ViewBag.Title = "Usuários (Novo)";

                ModelState.AddModelError("error", SysException.Show(ex));

                return View("Index");
            }
        }

        // GET: /Usuario/Create

        public ActionResult Novo()
        {
            ViewBag.Title = "Usuários (Novo)";

            ViewBag.emp_id = SessionContext.emp_id;

            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
            
            return View();
        }

        [HttpPost]
        public ActionResult Novo(USUARIO usuario, List<string> PerfilUsuario, List<string> PerfilUsuarioDef)
        {
            try
            {
                usuario.EMP_ID = 1;

                List<PERFIL_USUARIO> ListaPerfil = new List<PERFIL_USUARIO>();

                if (PerfilUsuario == null)
                    throw new Exception("O usuário deve possuir ao menos 1 nivelRepresentante.");

                for (int i = 0; i < PerfilUsuario.Count; i++)
                {
                    PERFIL_USUARIO p = new PERFIL_USUARIO();

                    p.EMP_ID = usuario.EMP_ID;
                    p.PER_ID = PerfilUsuario[i];
                    p.PUS_ATIVO = usuario.USU_ATIVO;
                    p.USU_LOGIN = usuario.USU_LOGIN;
                    p.PUS_DEFAULT = 0;
                    if (PerfilUsuarioDef[i] != "")
                        p.PUS_DEFAULT = Convert.ToInt32(PerfilUsuarioDef[i]);

                    ListaPerfil.Add(p);

                    new UsuarioSRV().IncluirReg(usuario, ListaPerfil);
                }

                TempData.Add("Resultado", "Inclusão realizada com sucesso.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Title = "Usuários (Novo)";

                ModelState.AddModelError("error", SysException.Show(ex));

                SysException.RegistrarLog("Erro ao Incluir " + usuario.USU_LOGIN + " (" + SysException.Show(ex) + ") ", ex.HResult.ToString(), SessionContext.autenticado);

                //------------------
                ViewBag.EmpId = 1;
                var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
                ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
                //------------------

                return View("Novo");
            }
        }
        public ActionResult Editar(string _usu_login)
        {
            ViewBag.Title = "Usuários (Editar)";

            USUARIO a = new UsuarioSRV().BuscarPorId(_usu_login);
            
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();

            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
            
            ViewBag.usuario_login = a.USU_LOGIN;
            ViewBag.emp_id = a.EMP_ID;

            return View(a);
        }

        [HttpPost]
        public ActionResult Editar(USUARIO usuario, List<string> PerfilUsuario, List<string> PerfilUsuarioDef)
        {

            try
            {
                List<PERFIL_USUARIO> ListaPerfil = new List<PERFIL_USUARIO>();

                if (PerfilUsuario == null)
                    throw new Exception("O usuário deve possuir ao menos 1 nivelRepresentante.");

                for (int i = 0; i < PerfilUsuario.Count; i++)
                {
                    PERFIL_USUARIO p = new PERFIL_USUARIO();

                    p.EMP_ID = usuario.EMP_ID;
                    p.PER_ID = PerfilUsuario[i];
                    p.PUS_ATIVO = usuario.USU_ATIVO;
                    p.USU_LOGIN = usuario.USU_LOGIN;
                    p.PUS_DEFAULT = 0;
                    if (PerfilUsuarioDef[i] != "")
                        p.PUS_DEFAULT = Convert.ToInt32(PerfilUsuarioDef[i]);

                    ListaPerfil.Add(p);

                }

                new UsuarioSRV().SalvarReg(usuario, ListaPerfil);

                TempData.Add("Resultado","Alteração realizada com sucesso.");

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewBag.Title = "Usuários (Editar)";

                ModelState.AddModelError("error", SysException.Show(ex));

                SysException.RegistrarLog("Erro ao atualizar o usuário " + usuario.USU_LOGIN + " (" + SysException.Show(ex) + ") ", ex.HResult.ToString(), SessionContext.autenticado);

                //----------
                var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
                ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
                ViewBag.usuario_login = usuario.USU_LOGIN;
                ViewBag.emp_id = usuario.EMP_ID;
                //----------

                return View(usuario);
            }
        }
        public ActionResult Excluir(string _usu_login)
        {
            ViewBag.Title = "Deseja excluir este usuário ?";

            USUARIO a = new UsuarioSRV().BuscarPorId(_usu_login);

            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();

            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");

            ViewBag.usuario_login = a.USU_LOGIN;
            ViewBag.emp_id = a.EMP_ID;

            return View(a);
        }
        [HttpPost]
        public ActionResult Excluir(USUARIO usuario)
        {

            try
            {
                new UsuarioSRV().ExcluirReg(usuario);

                TempData.Add("Resultado", "Exclusão realizada com sucesso.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Title = "Deseja excluir este usuário ?";

                ModelState.AddModelError("error", SysException.Show(ex));

                SysException.RegistrarLog("Erro ao excluir o usuário " + usuario.USU_LOGIN + " (" + SysException.Show(ex) + ") ", ex.HResult.ToString(), SessionContext.autenticado);

                return View("Excluir");
            }
        }
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
                    USUARIO _usuario = new USUARIO();

                    _usuario.USU_LOGIN = _novasenha.Login;
                    _usuario.USU_SENHA = _novasenha.Senha;
                    _usuario.USU_NOVA_SENHA = 0;

                    new UsuarioSRV().AlterarSenha(_usuario);

                    TempData.Add("Resultado", "Senha alterada com sucesso.");

                    SysException.RegistrarLog("Senha do usuário alterada com sucesso (" + _novasenha.Login + ")", "", SessionContext.autenticado);

                    return RedirectToAction("Index", "Home");
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

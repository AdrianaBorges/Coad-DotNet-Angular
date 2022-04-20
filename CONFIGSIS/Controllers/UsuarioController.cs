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
using COAD.SEGURANCA.Filter;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Exceptions;
using COAD.CORPORATIVO.Service;

namespace CONFIGSIS.Controllers
{
    public class UsuarioController : Controller
    {

        private UsuarioSRV _service = new UsuarioSRV();
        //
        // GET: /Usuario/

        [Autorizar(PorMenu = true, AdminOuAdminDeLogins = true)]
        public ActionResult Index()
        {
            try
            {
                ViewBag.Title = "Usuários (Consultar)";

                
                List<PERFIL> listaPerfil = new PerfilSRV().BuscarTodos().ToList();
                var listaemp = new EmpresaSRV().FindAll().ToList();

                ViewBag.listaPerfil = new SelectList(listaPerfil, "PER_ID", "PER_ID");
                ViewBag.listaemp = new SelectList(listaemp, "EMP_ID", "EMP_RAZAO_SOCIAL");

                //if (a.Count == 0)
                //    throw new Exception("Nenhum resultado encontrado para a consulta.");

                return View();
            }
            catch (Exception ex)
            {
                 //ViewBag.Title = "Usuários (Novo)";

                ModelState.AddModelError("error", SysException.Show(ex));
                return View("Index");
            }
        }

        [Autorizar(IsAjax = true, AdminOuAdminDeLogins = true)]
        public ActionResult Usuarios(int? emp_id, string per_id, string usu_login, string nome = null, string cpf = null, int pagina = 1, int registroPorPagina = 7)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                Pagina<UsuarioModel> lstUsuarios = null;
                response.success = true;

                if (SessionContext.admin) // se for um admin ele pode ver tudo
                {
                    lstUsuarios = _service.Usuarios(emp_id, per_id, usu_login, nome: nome, cpf: cpf, pagina: pagina, registrosPorPagina: registroPorPagina, trazPerfilUsuario: true);
                }
                else if(SessionContext.administradorDeLogin) // se ele for um administrador de login ele só poderá ver os cadastrados por ele mesmo.
                {
                    var autendicado = SessionContext.autenticado;
                    var loginGerenteDepartamento = autendicado.USU_LOGIN;
                    lstUsuarios = _service.Usuarios(emp_id, per_id, usu_login, loginGerenteDepartamento,nome, cpf, pagina, registroPorPagina, true);
                }
                else{
                    response.success = false;
                    response.message = Message.Fail("O usuário logado não possui direitos para visualizar os dados requeridos.");

                }
             
                response.AddPage("lstUsuarios", lstUsuarios);
                

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(AdminOuAdminDeLogins = true, PorMenu = false)]
        public ActionResult Novo()
        {
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
            IList<DepartamentoDTO> lstDepartamento = new DepartamentoSRV().FindAll();
            ViewBag.Departamentos = new SelectList(lstDepartamento, "DP_ID", "DP_NOME");

            if (!SessionContext.admin && SessionContext.administradorDeLogin)
            {
                var autenticado = SessionContext.autenticado;
                ViewBag.adminDeLogin = autenticado.USU_LOGIN;
            }

            ViewBag.origem = "incluir";
            ViewBag.emp_id = SessionContext.emp_id;
            return View("Editar");
        }

        [Autorizar(AdminOuAdminDeLogins = true, PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(string USU_LOGIN)
        {

            ViewBag.ListaEmpresa = new List<string>();
            IList<DepartamentoDTO> lstDepartamento = new DepartamentoSRV().FindAll();
                ViewBag.Departamentos = new SelectList(lstDepartamento, "DP_ID", "DP_NOME");
            ViewBag.origem = "editar";
            ViewBag.emp_id = SessionContext.emp_id;
            ViewBag.USU_LOGIN = USU_LOGIN;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDoUsuario(string USU_LOGIN)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = _service.FindByIdFullLoaded(USU_LOGIN);
                response.Add("usuario", usuario);

            //if (!SessionContext.admin && SessionContext.administradorDeLogin)
            //{
            //    if(usuario != null){

            //        var autenticado = SessionContext.autenticado;
            //        if(autenticado.USU_LOGIN.Equals(usuario.CADASTRADO_POR)){
                        

            //        }
            //    }
               
                
            //}
                
                
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(UsuarioModel usuario)
        {
            return SalvarOuIncluir(usuario, true);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Incluir(UsuarioModel usuario)
        {
            return SalvarOuIncluir(usuario, false);
        }


        private ActionResult SalvarOuIncluir(UsuarioModel usuario, bool update = true)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarUsuario(usuario);
                    SysException.RegistrarLog("Dados do usuário atualizados com sucesso!!", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do cliente atualizados com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (EmailNaoEnviadoException ex)
            {
                SessionUtil.HandleException(ex);
                result.message = Message.Warning("O usuário foi salvo com successo. Porém não foi possível enviar o email.");
                result.success = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }



        // GET: /Usuario/Create

        //public ActionResult Novo()
        //{
        //    ViewBag.Title = "Usuários (Novo)";

        //    ViewBag.emp_id = SessionContext.emp_id;

        //    List<EMPRESA> ListaEmpresa = new EmpresaSRV().BuscarTodos().ToList();
            
        //    ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
            
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Novo(USUARIO usuario, List<string> PerfilUsuario, List<string> PerfilUsuarioDef)
        //{
        //    try
        //    {
        //        usuario.EMP_ID = 1;

        //        List<PERFIL_USUARIO> ListaPerfil = new List<PERFIL_USUARIO>();

        //        if (PerfilUsuario == null)
        //            throw new Exception("O usuário deve possuir ao menos 1 nivelRepresentante.");

        //        for (int i = 0; i < PerfilUsuario.Count; i++)
        //        {
        //            PERFIL_USUARIO p = new PERFIL_USUARIO();

        //            p.EMP_ID = usuario.EMP_ID;
        //            p.perId = PerfilUsuario[i];
        //            p.PUS_ATIVO = usuario.USU_ATIVO;
        //            p.USU_LOGIN = usuario.USU_LOGIN;
        //            p.PUS_DEFAULT = 0;
        //            if (PerfilUsuarioDef[i] != "")
        //                p.PUS_DEFAULT = Convert.ToInt32(PerfilUsuarioDef[i]);

        //            ListaPerfil.Add(p);

        //            new UsuarioSRV().IncluirReg(usuario, ListaPerfil);
        //        }

        //        TempData.Add("Resultado", "Inclusão realizada com sucesso.");

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Title = "Usuários (Novo)";

        //        ModelState.AddModelError("error", SysException.Show(ex));

        //        SysException.RegistrarLog("Erro ao Incluir " + usuario.USU_LOGIN + " (" + SysException.Show(ex) + ") ", ex.HResult.ToString(), SessionContext.autenticado);

        //        //------------------
        //        ViewBag.EmpId = 1;
        //        List<EMPRESA> ListaEmpresa = new EmpresaSRV().BuscarTodos().ToList();
        //        ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
        //        //------------------

        //        return View("Novo");
        //    }
        //}
       

        //[HttpPost]
        //public ActionResult Editar(USUARIO usuario, List<string> PerfilUsuario, List<string> PerfilUsuarioDef)
        //{

        //    try
        //    {
        //        List<PERFIL_USUARIO> ListaPerfil = new List<PERFIL_USUARIO>();

        //        if (PerfilUsuario == null)
        //            throw new Exception("O usuário deve possuir ao menos 1 nivelRepresentante.");

        //        for (int i = 0; i < PerfilUsuario.Count; i++)
        //        {
        //            PERFIL_USUARIO p = new PERFIL_USUARIO();

        //            p.EMP_ID = usuario.EMP_ID;
        //            p.perId = PerfilUsuario[i];
        //            p.PUS_ATIVO = usuario.USU_ATIVO;
        //            p.USU_LOGIN = usuario.USU_LOGIN;
        //            p.PUS_DEFAULT = 0;
        //            if (PerfilUsuarioDef[i] != "")
        //                p.PUS_DEFAULT = Convert.ToInt32(PerfilUsuarioDef[i]);

        //            ListaPerfil.Add(p);

        //        }

        //        new UsuarioSRV().SalvarReg(usuario, ListaPerfil);

        //        TempData.Add("Resultado","Alteração realizada com sucesso.");

        //        return RedirectToAction("Index");

        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Title = "Usuários (Editar)";

        //        ModelState.AddModelError("error", SysException.Show(ex));

        //        SysException.RegistrarLog("Erro ao atualizar o usuário " + usuario.USU_LOGIN + " (" + SysException.Show(ex) + ") ", ex.HResult.ToString(), SessionContext.autenticado);

        //        //----------
        //        List<EMPRESA> ListaEmpresa = new EmpresaSRV().BuscarTodos().ToList();
        //        ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
        //        ViewBag.usuario_login = usuario.USU_LOGIN;
        //        ViewBag.emp_id = usuario.EMP_ID;
        //        //----------

        //        return View(usuario);
        //    }
        //}
        public ActionResult Excluir(string _usu_login)
        {
            ViewBag.Title = "Deseja excluir este usuário ?";

            USUARIO a = new UsuarioSRV().BuscarPorId(_usu_login);

            var  ListaEmpresa = new EmpresaSRV().FindAll().ToList();

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
        
        public ActionResult ChecaLogin(string login)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var existe = (_service.FindById(login) != null);
                result.Add("existe", existe);               
                return Json(result, JsonRequestBehavior.AllowGet);                
               
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


    }
}

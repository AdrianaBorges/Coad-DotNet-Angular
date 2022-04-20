using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.CORPORATIVO.SessionUtils;

namespace CONFIGSIS.Controllers
{
    public class PerfilController : Controller
    {
        //
        // GET: /Perfil/

        private PerfilUsuarioSRV _perfilUsuarioService = new PerfilUsuarioSRV();
        private PerfilSRV _service = new PerfilSRV();
        private SistemaSRV _sistemaService = new SistemaSRV();
        private NivelAcessoSRV _nivelAcessoService = new NivelAcessoSRV();
        private DepartamentoSRV _departamentoService = new DepartamentoSRV();

        private void PreecherCombos()        {

            List<SelectListItem> horas = new List<SelectListItem>();

            for (int i = 0; i < 24; i++)
            {
                SelectListItem _item = new SelectListItem();

                if (i.ToString().Length > 1)
                {
                    _item.Text = i.ToString() + ":00";
                    _item.Value = i.ToString() + ":00";

                }
                else
                {
                    _item.Text = "0" + i.ToString() + ":00";
                    _item.Value = "0" + i.ToString() + ":00";
                }

                horas.Add(_item);
            }

            SelectListItem _item2 = new SelectListItem() { Text = "23:59", Value = "23:59" };

            horas.Add(_item2);

            ViewBag.horas = horas;
            ViewBag.lstNivelAcesso = _nivelAcessoService.FindAll();
            ViewBag.lstDepartamentos = _departamentoService.FindAll();
            ViewBag.lstSistemas = _sistemaService.FindAll();

        }

        [Autorizar]
        public ActionResult Index()
        {
            ViewBag.lstSistemas = _sistemaService.FindAll();
            ViewBag.lstNivelAcesso = _nivelAcessoService.FindAll();
            ViewBag.lstDepartamento = _departamentoService.FindAll();

            return View();
        }


        //public ActionResult Index(string _per_id, string _sis_id)
        //{
        //    try
        //    {
        //        ViewBag.Title = "Perfil (Consultar)";
        //        List<PerfilModel> a = new PerfilSRV().BuscarLista(SessionContext.emp_id, _per_id, _sis_id);
        //        List<String> Situacao = new List<String>();

        //        Situacao.Add("Inativo");
        //        Situacao.Add("Ativo");

        //        ViewBag.Situacao = new SelectList(Situacao);
        //        List<SISTEMA> ListaSistema = new SistemaSRV().BuscarTodos().ToList();

        //        ViewBag.ListaSistema = new SelectList(ListaSistema, "SIS_ID", "SIS_DESCRICAO");

        //        if (a.Count == 0)
        //            throw new Exception("Nenhum resultado encontrado para a consulta.");

        //        return View(a);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Title = "Perfil (Consultar)";

        //        ModelState.AddModelError("error", SysException.Show(ex));

        //        return View("Index");
        //    }
        //}

        //public ActionResult Novo()
        //{
        //    try
        //    {
        //        this.CarregaDadosNovo();            
        //        return View();

        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Title = "Perfil (Novo)";

        //        ModelState.AddModelError("error", SysException.Show(ex));

        //        this.CarregaDadosNovo();

        //        return View("Editar");
        //    }
            
        //}

        //[HttpPost]
        //public ActionResult Novo(PerfilModel nivelRepresentante)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            ViewBag.Title = "Perfil (Novo)";
                                     
        //            new PerfilSRV().IncluirReg(nivelRepresentante);
                    
        //            TempData.Add("Resultado", "Inclusão realizada com sucesso.");

        //            return RedirectToAction("Index");
                 
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Title = "Perfil (Novo)";

        //            ModelState.AddModelError("error", SysException.Show(ex));

        //            SysException.RegistrarLog("Erro ao Incluir nivelRepresentante " + nivelRepresentante.perId + " (" + SysException.Show(ex) + ") ", ex.HResult.ToString(), SessionContext.autenticado);

        //            this.CarregaDadosNovo();

        //            return View("Novo");
        //        }
        //    }

        //    this.CarregaDadosNovo();

        //    return View("Novo");
        //}
        //public PerfilModel CarregaDadosEditar(int _emp_id, string _per_id)
        //{

        //    PerfilModel nivelRepresentante = new PerfilSRV().BuscarPorId(_emp_id, _per_id);

        //    List<EMPRESA> ListaEmpresa = new EmpresaSRV().BuscarTodos().ToList();
        //    List<SISTEMA> ListaSistema = new SistemaSRV().BuscarTodos().ToList();
        //    IList<DepartamentoDTO> lstDepartamento = new DepartamentoSRV().FindAll();

        //    ViewBag.Departamentos = new SelectList(lstDepartamento, "DP_ID", "DP_NOME");
        //    ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
        //    ViewBag.ListaSistema = new SelectList(ListaSistema, "SIS_ID", "SIS_DESCRICAO");

        //    //List<String> horas = new List<String>();

        //    List<SelectListItem> horas = new List<SelectListItem>();

        //    for (int i = 0; i < 24; i++)
        //    {
        //        SelectListItem _item = new SelectListItem();

        //        if (i.ToString().Length > 1)
        //        {
        //            _item.Text = i.ToString() + ":00";
        //            _item.Value = i.ToString() + ":00";

        //        }
        //        else
        //        {
        //            _item.Text = "0" + i.ToString() + ":00";
        //            _item.Value = "0" + i.ToString() + ":00";
        //        }

        //        horas.Add(_item);
        //    }

        //    SelectListItem _item2 = new SelectListItem() { Text = "23:59", Value = "23:59" };

        //    horas.Add(_item2);

        //    ViewBag.horas = horas;

        //    return nivelRepresentante;
        //}

        //public ActionResult Excluir(int _emp_id, string _per_id)
        //{
        //    ViewBag.Title = "Confirma a exclusão dete nivelRepresentante ?";

        //    PerfilModel _perfil = this.CarregaDadosEditar(_emp_id, _per_id);

        //    return View(_perfil);

        //}

        //[HttpPost]
        //public ActionResult Excluir(PerfilModel nivelRepresentante)
        //{
        //    PerfilModel _perfil = new PerfilModel();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            new PerfilSRV().ExcluirReg(nivelRepresentante);

        //            TempData.Add("Resultado", "Exclusão realizada com sucesso.");

        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Title = "Confirma a exclusão deste nivelRepresentante ?";

        //            ModelState.AddModelError("error", SysException.Show(ex));

        //            _perfil = this.CarregaDadosEditar(nivelRepresentante.EMP_ID, nivelRepresentante.perId);

        //            return View("Excluir");
        //        }
        //    }
            
        //    _perfil = this.CarregaDadosEditar(nivelRepresentante.EMP_ID, nivelRepresentante.perId);

        //    return View("Excluir");
        //}

        //public ActionResult Editar(int _emp_id, string _per_id)
        //{
        //    try
        //    {
        //        ViewBag.Title = "Perfil (Editar)";

        //        PerfilModel _perfil = this.CarregaDadosEditar(_emp_id, _per_id);

        //        return View(_perfil);

        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Title = "Perfil (Editar)";

        //        ModelState.AddModelError("error", SysException.Show(ex));

        //        PerfilModel _perfil = this.CarregaDadosEditar(_emp_id, _per_id);

        //        return View("Editar");
        //    }

        //}

        ////[HttpPost]
        ////public ActionResult Editar(PerfilModel nivelRepresentante)
        ////{
        ////    PerfilModel _perfil = new PerfilModel();

        ////    if (ModelState.IsValid)
        ////    {
        ////        try
        ////        {
        ////            ViewBag.Title = "Perfil (Editar)";

        ////            new PerfilSRV().SalvarReg(nivelRepresentante);

        ////            TempData.Add("Resultado", "Alteração realizada com sucesso.");

        ////            return RedirectToAction("Index");
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            ViewBag.Title = "Perfil (Editar)";

        ////            ModelState.AddModelError("error", SysException.Show(ex));

        ////            SysException.RegistrarLog("Erro ao atualizar o nivelRepresentante " + nivelRepresentante.perId + " (" + SysException.Show(ex) + ") ", ex.HResult.ToString(), SessionContext.autenticado);

        ////            _perfil = this.CarregaDadosEditar(nivelRepresentante.EMP_ID, nivelRepresentante.perId);

        ////            return View("Editar");
        ////        }
        ////    }
            
        ////    _perfil = this.CarregaDadosEditar(nivelRepresentante.EMP_ID, nivelRepresentante.perId);

        ////    return View("Editar");
        ////}

        public void CarregaDadosNovo()
        {
            ViewBag.Title = "Perfil (Novo)";

            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            List<SISTEMA> ListaSistema = new SistemaSRV().BuscarTodos().ToList();
            IList<DepartamentoDTO> lstDepartamento = new DepartamentoSRV().FindAll();
            
            ViewBag.Departamentos = new SelectList(lstDepartamento, "DP_ID", "DP_NOME");
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
            ViewBag.ListaSistema = new SelectList(ListaSistema, "SIS_ID", "SIS_DESCRICAO");

            List<SelectListItem> horas = new List<SelectListItem>();

            for (int i = 0; i < 24; i++)
            {
                SelectListItem _item = new SelectListItem();

                if (i.ToString().Length > 1)
                {
                    _item.Text = i.ToString() + ":00";
                    _item.Value = i.ToString() + ":00";

                }
                else
                {
                    _item.Text = "0" + i.ToString() + ":00";
                    _item.Value = "0" + i.ToString() + ":00";
                }

                horas.Add(_item);
            }

            SelectListItem _item2 = new SelectListItem() { Text = "23:59", Value = "23:59" };

            horas.Add(_item2);

            ViewBag.horas = horas;
        }

        public ActionResult CarregarItens(string _per_id, string _sis_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                //List<Menu> listaitemmenu = new UsuarioSRV().MontaMenuManutencao(_per_id, SessionContext.emp_id, _sis_id, SessionContext.admin);
                IList<ItemMenuPerfilModel> listaitemmenu = new ItemMenuPerfilSRV().ListarPerfilMenuCompleto(_per_id, _sis_id);
                response.Add("listaitemmenu", listaitemmenu);
                response.success = true;
                response.message = Message.Info("Ok");

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        
        [Autorizar(PorMenu = false)]
        public ActionResult Configurar()
        {
            List<SISTEMA> ListaSistema = new SistemaSRV().BuscarTodos().ToList();

            ViewBag.ListaSistema = new SelectList(ListaSistema, "SIS_ID", "SIS_DESCRICAO");
            ViewBag.ListaPerfil = new SelectList(new PerfilSRV().BuscarLista(SessionContext.emp_id, "", SessionContext.sis_id), "perId", "perId");
            
            return View();
        }

        [HttpPost]
        public ActionResult Configurar(List<Menu> listaitemmenu)
        {
            try
            {
                ViewBag.Title = "Perfil (Editar)";

                new PerfilSRV().SalvarMenuPerfil(listaitemmenu);

                TempData.Add("Resultado", "Alteração realizada com sucesso.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Title = "Perfil (Editar)";

                ModelState.AddModelError("error", SysException.Show(ex));

                TempData.Add("Erro", SysException.Show(ex) );

                ViewBag.ListaPefil = new SelectList(new PerfilSRV().BuscarLista(SessionContext.emp_id, "", SessionContext.sis_id), "perId", "perId");

                return View("Configurar");
            }

        }

        public ActionResult CarregarPerfis(string _sis_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                int emp_id = 0;
                string _per_id = "";

                List<PerfilModel> listaperfis = new PerfilSRV().BuscarLista(emp_id, _per_id, _sis_id).ToList();

                response.Add("listaperfis", listaperfis);
                response.success = true;
//response.message = Message.Info("Ok");

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                response.success = false;
                response.message = Message.Fail(SysException.Show(ex));

                return Json(response, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult PerfilsDoUsuario(string usu_login)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                IList<PerfilUsuarioModel> perfilUsuario = _perfilUsuarioService.BuscarLista(usu_login);

                response.Add("perfilUsuario", perfilUsuario);
                response.success = true;

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true)]
        public ActionResult Perfis(string nome = null, string SIS_ID = null, int? DP_ID = null, int? NIV_ACE_ID = null, int pagina = 1, int registroPorPagina = 10)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var perfis = _service.Perfis(nome, SIS_ID, DP_ID, NIV_ACE_ID, pagina, registroPorPagina);
                response.AddPage("perfis", perfis);
             
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarPerfis(string nome = null, string SIS_ID = null, int? DP_ID = null, int? NIV_ACE_ID = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var perfis = _service.ListarPerfis(nome, SIS_ID, DP_ID, NIV_ACE_ID);
                response.Add("perfis", perfis);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            PreecherCombos();
            ViewBag.editar = 0;
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(string PER_ID)
        {
            PreecherCombos();
            ViewBag.editar = 1;
            ViewBag.PER_ID = PER_ID;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDoPerfil(string PER_ID)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var perfil = _service.FindById(1, PER_ID);
                response.Add("perfil", perfil);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult Salvar(PerfilModel perfil)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.Salvar(perfil);
                    SysException.RegistrarLog("Dados do representante atualizados com sucesso!!", "", SessionContext.autenticado);

                    result.success = true;
                    result.message = Message.Info("Dados do representante atualizados com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
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

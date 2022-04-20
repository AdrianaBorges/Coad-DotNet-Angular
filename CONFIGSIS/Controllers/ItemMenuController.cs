using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using CONFIGSIS.Models;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;

namespace CONFIGSIS.Controllers
{
    public class ItemMenuController : Controller
    {
        //
        // GET: /ItemMenu/
        public ItemMenuSRV _service = new ItemMenuSRV();
        public ItemMenuPerfilSRV _itemMenuPerfilService = new ItemMenuPerfilSRV();

        //[Autorizar]
        //public ActionResult Index(string ListaSistema)
        //{
        //    try
        //    {
        //        this.Carregar();
        //        this.CarregaDadosNovo();                
        //    }
        //    catch (Exception ex)
        //    {
        //        SessionUtil.HandleException(ex);
        //    }

        //    return View();
        //}


        //public ActionResult Novo()
        //{
        //    ViewBag.Title = "Itens de Menu (Novo)";

        //    this.CarregaDadosNovo();

        //    return View("Editar");
        //}
        //[HttpPost]
        //public ActionResult Novo(ItemMenuModel _item_menu)
        //{

        //    JsonResponse resultado = new JsonResponse();

        //    try
        //    {
        //        _item_menu.ITM_NODE_ID = _item_menu.ITM_NODE_ID == null ? 0 : Convert.ToInt32(_item_menu.ITM_NODE_ID);

        //        _item_menu.ITM_ID = _service.BuscarNextID();
        //        _service.IncluirRegistro(_item_menu);

        //        resultado.Success = true;
        //        resultado.Message = "Operação realizada com sucesso !!";

        //        SysException.RegistrarLog(resultado.Message, "", SessionContext.autenticado);

        //        return Json(resultado, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

        //        resultado.Message = SysException.Show(ex);

        //        return Json(resultado, JsonRequestBehavior.AllowGet);
        //    }

        //}
        //public ActionResult Excluir(int _itm_id)
        //{
        //    ViewBag.Title = "Deseja excluir este item ?";

        //    ItemMenuModel _item_menu = this.CarregaDadosEditar(_itm_id);

        //    return View(_item_menu);

        //}
        //[HttpPost]
        //public ActionResult Excluir(ItemMenuModel _item_menu)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            new ItemMenuSRV().ExcluirReg(_item_menu);
                    
        //            TempData.Add("Resultado", "Exclusão realizada com sucesso.");

        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Title = "Confirma a exclusão deste item ?";

        //            _item_menu = this.CarregaDadosEditar(_item_menu.ITM_ID);

        //            ModelState.AddModelError("error", SysException.Show(ex));

        //            return View(_item_menu);
        //        }
        //    }

        //    _item_menu = this.CarregaDadosEditar(_item_menu.ITM_ID);

        //    return View("Excluir");

        //}

      
        //public ActionResult Editar(int ITM_ID)
        //{
        //    try
        //    {
        //       ViewBag.Title = "Itens de Menu (Editar)";
        //       ViewBag.itm_id = ITM_ID;

        //       ItemMenuModel _item_menu = this.CarregaDadosEditar(ITM_ID);

        //       return View(_item_menu);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("error", SysException.Show(ex));

        //        this.CarregaDadosEditar(ITM_ID);

        //        return View(ITM_ID);
        //    }
        //}

        //[HttpPost]
        //public ActionResult EditarSalvar(ItemMenuModel _item_menu)
        //{
        //    JsonResponse resultado = new JsonResponse();

        //    try
        //    {
        //        new ItemMenuSRV().SalvarRegistro(_item_menu);

        //        resultado.Success = true;
        //        resultado.Message = "Operação realizada com sucesso !!";

        //        SysException.RegistrarLog(resultado.Message, "", SessionContext.autenticado);

        //        return Json(resultado, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

        //        resultado.Message = SysException.Show(ex);

        //        return Json(resultado, JsonRequestBehavior.AllowGet);
        //    }

        //}  
        
        //public ItemMenuModel CarregaDadosEditar(int _itm_id)
        //{
        //    ItemMenuModel _item_menu = new ItemMenuSRV().BuscarPorId(_itm_id);

        //    List<ITEM_MENU> listaItmMenu = new ItemMenuSRV().BuscarPorTipo(0, SessionContext.sis_id).ToList();
        //    List<SISTEMA> listasistema = new SistemaSRV().BuscarTodos().ToList();
        //    List<string> listanivel = new List<string>();

        //    listanivel.Add("0");
        //    listanivel.Add("1");
        //    listanivel.Add("2");

        //    ViewBag.ListaSistema = new SelectList(listasistema, "SIS_ID", "SIS_DESCRICAO");
        //    ViewBag.ListaItmMenu = new SelectList(listaItmMenu, "ITM_ID", "ITM_DESCRICAO");
        //    ViewBag.ListaNivel = new SelectList(listanivel);

        //    return _item_menu;
        //}


        //public void Carregar()
        //{
            
        //    List<EMPRESA> listaemp           = new EmpresaSRV().BuscarTodos().ToList();
        //    List<SISTEMA> listasis           = new SistemaSRV().BuscarTodos().ToList();
        //    List<ItemMenuModel> listaMenu    = new ItemMenuSRV().Listar(0, 0, SessionContext.sis_id, 0).ToList();
   
        //    ViewBag.ListaEmpresa = new SelectList(listaemp, "EMP_ID", "EMP_RAZAO_SOCIAL");
        //    ViewBag.ListaSistema = new SelectList(listasis, "SIS_ID", "SIS_DESCRICAO");
        //    ViewBag.ListaMenu    = new SelectList(listaMenu, "ITM_ID", "ITM_DESCRICAO");
            
        //}
        //public void CarregaDadosNovo()
        //{
        //    List<ITEM_MENU> listaItmMenu = new ItemMenuSRV().BuscarPorTipo(0, SessionContext.sis_id).ToList();
        //    List<SISTEMA> listasistema = new SistemaSRV().BuscarTodos().ToList();
        //    List<string> listanivel = new List<string>();

        //    listanivel.Add("0");
        //    listanivel.Add("1");
        //    listanivel.Add("2");

        //    ViewBag.ListaItmMenu = new SelectList(listaItmMenu, "ITM_ID", "ITM_DESCRICAO");
        //    ViewBag.ListaSistema = new SelectList(listasistema, "SIS_ID", "SIS_DESCRICAO");
        //    ViewBag.ListaNivel = new SelectList(listanivel);
        //}

        private void _carregaCombos()
        {

            var listaemp = new EmpresaSRV().FindAll().ToList();
            var listasis = new SistemaSRV().FindAll().ToList();
            List<ItemMenuModel> listaMenu = new ItemMenuSRV().Listar(0, 0, SessionContext.sis_id, 0).ToList();

            ViewBag.ListaEmpresa = new SelectList(listaemp, "EMP_ID", "EMP_RAZAO_SOCIAL");
            ViewBag.ListaSistema = new SelectList(listasis, "SIS_ID", "SIS_DESCRICAO");
            ViewBag.ListaMenu    = new SelectList(listaMenu, "ITM_ID", "ITM_DESCRICAO");

            List<ITEM_MENU> listaItmMenu = new ItemMenuSRV().BuscarPorTipo(0, SessionContext.sis_id).ToList();
            List<SISTEMA> listasistema = new SistemaSRV().BuscarTodos().ToList();
            List<string> listanivel = new List<string>();

            listanivel.Add("0");
            listanivel.Add("1");
            listanivel.Add("2");

            ViewBag.ListaItmMenu = new SelectList(listaItmMenu, "ITM_ID", "ITM_DESCRICAO");
            ViewBag.ListaSistema = new SelectList(listasistema, "SIS_ID", "SIS_DESCRICAO");
            ViewBag.ListaNivel = new SelectList(listanivel);
            
        }

        [Autorizar]
        public ActionResult Index()
        {
            _carregaCombos();
            return View();
        }


        [Autorizar(IsAjax = true)]
        public ActionResult BuscarMenu(string _sis_id = null, int? _itm_memu_nivel = null, int pagina = 1, int registrosPorPagina = 7)
        {
            JSONResponse resultado = new JSONResponse();
            
            try
            {
                Pagina<ItemMenuModel> listaitemmenu = new ItemMenuSRV().ListarMenu(_sis_id, _itm_memu_nivel, pagina, registrosPorPagina);
                resultado.AddPage("listaitemmenu", listaitemmenu);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                resultado.success = false;
                SessionUtil.HandleException(ex);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }


        [Autorizar(IsAjax = true)]
        public ActionResult BuscarMenuCombo(string _sis_id = null, int? _itm_memu_nivel = null, int pagina = 1, int registrosPorPagina = 7)
        {
            JSONResponse resultado = new JSONResponse();

            try
            {
                IList<ItemMenuModel> listaitemmenu = new ItemMenuSRV().ListarMenu(_sis_id, _itm_memu_nivel, null);
                resultado.Add("listaitemmenu", listaitemmenu);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                resultado.success = false;
                SessionUtil.HandleException(ex);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarListaItemMenu(int? _itm_nivel = null , string _sis_id = null, int? _itm_id_node = null, int pagina = 1, int registrosPorPagina = 15)
        {
            JSONResponse resultado = new JSONResponse();
            
            try
            {
                Pagina<ItemMenuModel> listaitemmenu = new ItemMenuSRV().ListarItemMenuPaginas(_itm_nivel, _sis_id, _itm_id_node, pagina, registrosPorPagina);
                resultado.AddPage("listaitemmenu", listaitemmenu);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                resultado.success = false;
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                resultado.message = Message.Fail(SysException.Show(ex));

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }


        [Autorizar(IsAjax = true)]
        public ActionResult BuscarItemMenu(int? _itm_id)
        {
            JsonResponse resultado = new JsonResponse();

            try
            {
                ItemMenuModel _item_menu = new ItemMenuSRV().BuscarPorId((int)_itm_id);

                return Json(_item_menu, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                resultado.Message = "Erro ao carregar os dados ( " + SysException.Show(ex) + " )";

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            int? ultimoCodigo = _service.GetSugestaoCodigo(); 
            _carregaCombos();

            ViewBag.editar = 0;
            ViewBag.ultimoCodigo = ultimoCodigo;

            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(string ITM_ID)
        {
            _carregaCombos();
            ViewBag.editar = 1;
            ViewBag.ITM_ID = ITM_ID;
            return View();
        }
        

        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDoItemMenu(int? ITM_ID)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var menu = _service.FindById(ITM_ID);
                response.Add("menu", menu);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ItensDeMenu(int? ITM_TIPO = null,
            int ITM_MENU_NIVEL = 0,
            string SIS_ID = null,
            string path = null,
            int? ITM_NODE_ID = null,
            string ITM_DESCRICAO = null,
            int pagina = 1,
            int registrosPorPagina = 15)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var itensDeMenu = _service.ItensDeMenu(ITM_TIPO, ITM_MENU_NIVEL, path, SIS_ID, ITM_NODE_ID, ITM_DESCRICAO, pagina, registrosPorPagina);
                response.AddPage("itensDeMenu", itensDeMenu);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult ObterSubMenus(int? ITM_ID = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var subMenus = _service.ObterListaSubmenus(ITM_ID);
                response.Add("subMenus", subMenus);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult Salvar(ItemMenuModel itemMenu)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarItemMenu(itemMenu);
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

        public ActionResult CarregarItensMenuPerfil(string _per_id, string _sis_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                //List<Menu> listaitemmenu = new UsuarioSRV().MontaMenuManutencao(_per_id, SessionContext.emp_id, _sis_id, SessionContext.admin);
                IList<ItemMenuPerfilModel> listaitemmenu = new ItemMenuPerfilSRV().ListarPerfilMenuCompleto(_per_id, _sis_id);
                response.Add("itemMenuPerfil", listaitemmenu);
                response.success = true;

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true)]
        public ActionResult SalvarItemMenuPerfil(IList<ItemMenuPerfilModel> itemMenuPerfil)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _itemMenuPerfilService.SalvarItemMenuPerfil(itemMenuPerfil);
               
                result.success = true;
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

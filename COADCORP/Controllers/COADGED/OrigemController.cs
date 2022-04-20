using Coad.GenericCrud.ActionResultTools;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.COADGED
{
    public class OrigemController : Controller
    {
        //
        // GET: /Origem/

        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Pesquisar(string _descricao, int _pagina = 1, int _registroPorPagina = 20)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listaorigem = new OrigemAcessoSRV().ListarPorNome(_descricao, _pagina, _registroPorPagina);

                if (listaorigem == null)
                    throw new Exception("Nenhum registro encontrado para a consulta");

                response.AddPage("listaorigem", listaorigem);
                response.success = true;
                response.message = Message.Info("Ok");

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
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

        [Autorizar(Acesso = "Editar")]
        public ActionResult Editar(int? _org_id)
        {
            ViewBag.Title = " Origem Acesso (Editar) ";

            if (_org_id == null)
                ViewBag.Title = " Origem Acesso (Novo)";

            ////_preencherCombos();

            ViewBag.origem = _org_id;

            return View();
        }
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult BuscarOrigem(int? org_id)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _origem = new OrigemAcessoSRV().FindById(org_id);

                response.success = true;
                response.Add("origem", _origem);
                return Json(response);

            }
            catch (Exception ex)
            {
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
        
        [Autorizar(IsAjax = true, Acesso = "Incluir")]
        [HttpPost]
        public ActionResult Salvar(OrigemAcessoDTO _origem, List<OrigemFuncionalidadeDTO> _origemfunc)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    new OrigemFuncionalidadeSRV().SalvarOrigemAcesso(_origem, _origemfunc);
                    SysException.RegistrarLog("Origem e funcionalidades atualizadas com sucesso!!", "", SessionContext.autenticado);
                    
                    result.success = true;
                    result.message = Message.Info("Origem e funcionalidades atualizadas com sucesso!!");
                    
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
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ListarFuncionalidadesSelec(int? _origem)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                IList<OrigemFuncionalidadeDTO> listafuncionalidade = new List<OrigemFuncionalidadeDTO>();

                if (_origem != null)
                    listafuncionalidade = new OrigemFuncionalidadeSRV().ListarFuncionalidades((int)_origem);

                response.success = true;
                response.Add("listafuncionalidade", listafuncionalidade);
                response.message = Message.Info("Ok");

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
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
        

    }
}

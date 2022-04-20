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
    public class NoticiaController : Controller
    {
        public void Carregarlistas()
        {
            IList<TitulacaoDTO> _listaTitulacao = new TitulacaoSRV().ListarGrandeGrupo();
            IList<NoticiaGrupoDTO> _listagrupo = new NoticiaGrupoSRV().FindAll();

            ViewBag.Listagrupo = new SelectList(_listagrupo, "NGR_ID", "NGR_DESCRICAO");
            ViewBag.ListaTitulacao = new SelectList(_listaTitulacao, "TIT_ID", "TIT_DESCRICAO");

        }
        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {           
            this.Carregarlistas();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Salvar(NoticiaDTO _noticia)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    if (_noticia.NOT_ID > 0)
                    {
                        _noticia.DATA_ALTERA = DateTime.Now;
                        _noticia.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                        new NoticiaSRV().Merge(_noticia, "NOT_ID");
                    }
                    else
                    {
                        _noticia.DATA_CADASTRO = DateTime.Now;
                        _noticia.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                        new NoticiaSRV().Save(_noticia);
                    }

                    SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                    response.success = true;
                    response.message = Message.Info("Dados atualizados com sucesso!!");

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string erro = "";
                    response.success = false;
                    response.SetMessageFromModelState(ModelState);
                    foreach (var _item in response.validationMessage)
                    {
                        for (var ind = 0; ind <= _item.Value.Count - 1; ind++)
                        {
                            erro += " --- " + _item.Value[ind] + " \n ";
                        }
                    }

                    response.message = Message.Fail(erro);

                    return Json(response, JsonRequestBehavior.AllowGet);
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

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [Autorizar(Acesso = "Editar")]
        public ActionResult Editar(int? _id)
        {
            if (_id == null)
                _id = 0;

            this.Carregarlistas();

            ViewBag.id = _id;

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Pesquisar(string _manchete, int? _grandegrupo, int? _class, int _pagina = 1, int _registroPorPagina = 20)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (_manchete != null)
                    _manchete = _manchete.Trim();

                var _listanoticias = new NoticiaSRV().ListarNoticias(_manchete, _grandegrupo, _class, _pagina, _registroPorPagina);

                if (_listanoticias == null)
                    throw new Exception("Nenhum registro encontrado para a consulta");

                response.AddPage("listanoticias", _listanoticias);
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
        [Autorizar(IsAjax = true)]
        public ActionResult Publicar(NoticiaDTO _noticia)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                new NoticiaSRV().Publicar(_noticia);

                SysException.RegistrarLog("Notícia =>" + _noticia.NOT_ID + ", publicada com sucesso!", "", SessionContext.autenticado);

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
        [Autorizar(IsAjax = true)]
        public ActionResult RemoverPublicacao(NoticiaDTO _noticia)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                new NoticiaSRV().RemoverPublicacao(_noticia);

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
        [Autorizar(IsAjax = true)]
        public ActionResult CarregarTela(int? _id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _noticia = new NoticiaDTO();

                if (_id != null && _id != 0)
                {
                    _noticia = new NoticiaSRV().FindById(_id);

                }

                response.Add("noticia", _noticia);
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
        [Autorizar(Acesso = "Excluir")]
        public ActionResult Excluir(NoticiaDTO _noticia)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                new NoticiaSRV().Delete(_noticia, "NOT_ID");

                result.success = true;
                result.message = Message.Info("Operação realizada com sucesso !!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                result.success = false;
                result.SetMessageFromModelState(ModelState);
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

    }
}

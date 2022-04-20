using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class ClassificacaoAtendimentoController : Controller
    {

        TipoAtendimentoSRV _tipoAtendsrv = new TipoAtendimentoSRV();
        public ActionResult Index()
        {
            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarTipoAtendimento(int? _classificacao)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (_classificacao == null)
                    throw new Exception("Classificação não informada.");


                var _listaTipoAtendimento = _tipoAtendsrv.BuscarTipoAtendimento((int)_classificacao);

                result.Add("listaTipoAtendimento", _listaTipoAtendimento);
                result.success = true;
                result.message = Message.Info("Ok");
                return Json(result, JsonRequestBehavior.AllowGet);

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
        public ActionResult Salvar(TipoAtendimentoDTO _tipo)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _tipoAtendsrv.Merge(_tipo, "TIP_ATEND_ID");

                result.success = true;
                result.message = Message.Info("Ok");
                return Json(result, JsonRequestBehavior.AllowGet);

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
        public ActionResult PreencherGrids()
        {

            JSONResponse response = new JSONResponse();

            try
            {

                ClassificacaoAtendimentoSRV _srvClassificacao = new ClassificacaoAtendimentoSRV();
                var _listaClassificacao = _srvClassificacao.FindAll();

                response.Add("listaClassificacao", _listaClassificacao);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}

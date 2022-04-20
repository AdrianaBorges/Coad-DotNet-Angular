using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class CEPController : Controller
    {
        //
        // GET: /CEP/
        private CepLogradouroSRV _cepSRV = new CepLogradouroSRV();

        [Autorizar(IsAjax = true)]
        
        private void _preencherCombos()
        {

            var _tipoend = new TipoEnderecoSRV().FindAll().ToList();

            var ufs = new UFSRV().BuscarSomenteUF();
            var _tipoLogradouro = new TipoLogradouroSRV().FindAll();
            var tipoEndereco = new TipoEnderecoSRV().FindAll();
            var municipio = new MunicipioSRV().FindAll();
            var ListaClassificacao = new ClassificacaoSRV().FindAll();

            ViewBag.ufs = ufs;
            ViewBag.tipoLogradouro = _tipoLogradouro;
            ViewBag.tipoEndereco = tipoEndereco;
            ViewBag.ListaUf = new SelectList(new UFSRV().BuscarSomenteUF(), "UF_SIGLA", "UF_DESCRICAO");

            ViewBag.ListaClassificacao = new SelectList(ListaClassificacao, "CLA_ID", "CLA_DESCRICAO");
            ViewBag.ListaTipoLogradouro = new SelectList(_tipoLogradouro, "TIPO_LOG_ID", "TIPO_LOG_DESCRICAO");
            ViewBag.ListaTipoEndereco = new SelectList(_tipoend, "TP_END_ID", "TP_END_DESCRICAO");
        }

        public ActionResult Index()
        {
            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarCep(string _cep)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                CepLogradouroDTO _cepdto = _cepSRV.BuscarCep(_cep);
                if (_cepdto == null)
                   result.success = false;
                else
                   result.success = true;

                result.Add("cep",_cepdto);
                //result.message = Message.Info("Ok");

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
        public ActionResult Pesquisar(string _cep, string _cep_log, int _pagina = 1)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                Pagina<CepLogradouroDTO> _listacep = _cepSRV.BuscarCep(_cep, _cep_log, _pagina);
                result.AddPage("listacep", _listacep);
                result.success = true;
                result.message = Message.Info("ok");

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


        [Autorizar(Acesso = "Editar")]
        public ActionResult Editar(string _cep_id)
        {
            ViewBag.Title = " CEP (Editar) ";

            if (_cep_id == null)
                ViewBag.Title = " CEP (Novo)";

            _preencherCombos();

            ViewBag.cepid = _cep_id;

            return View();
        }

        [Autorizar(IsAjax = true, Acesso = "Incluir")]
        [HttpPost]
        public ActionResult Salvar(CepLogradouroDTO _cep)
        {
            JSONResponse result = new JSONResponse();
            try
            {
    
                if (ModelState.IsValid)
                {
                    if (_cep.CEP_ID > 0)
                        _cepSRV.Merge(_cep, "CEP_ID");
                    else
                    {
                        _cep.CEP_ID = _cepSRV.BuscarUltimoID() + 1;
                        _cepSRV.Save(_cep);
                    }

                    SysException.RegistrarLog("Cep atualizado com sucesso!!", "", SessionContext.autenticado);

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
    

    }
}

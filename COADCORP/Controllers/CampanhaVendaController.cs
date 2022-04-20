using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Filters;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    [AutorizarCustom(PorMenu = false, SessionUtilMethodName = "PossuiGerenciaVenda")]
    public class CampanhaVendaController : Controller
    {
        public CampanhaVendaSRV _service { get; set; }
        public TipoPagamentoSRV _tipoPagamentoSRV { get; set; }
        //
        // GET: /Templates/

        [AutorizarCustom(PorMenu = false, SessionUtilMethodName = "PossuiGerenciaVenda")]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Editar(int? cveId)
        {
            ViewBag.cveId = cveId;
            return View();
        }

        [AutorizarCustom(IsAjax = true, SessionUtilMethodName = "PossuiGerenciaVenda")]
        public JsonResult PesquisarCampanhaVenda(PesquisaCampanhaVendaDTO pesquisaDTO)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstCampanhaVenda = _service.PesquisarCampanhaVenda(pesquisaDTO);
                response.AddPage("lstCampanhaVenda", lstCampanhaVenda);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDaCampanhaVenda(int? cveId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var campanhaVendas = _service.FindByIdFullLoaded(cveId, true, true, true);
                response.Add("campanhaVendas", campanhaVendas);

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
        public ActionResult SalvarCampanhaVenda(CampanhaVendaDTO campanhaVenda)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarCampanhaVenda(campanhaVenda);
                    SysException.RegistrarLog("Dados da campanha atualizados com sucesso!!", "", SessionContext.autenticado);
                    result.success = true;
                    result.message = Message.Info("Dados do campanha atualizados com sucesso!!");

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
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

        //[Autorizar(IsAjax = true)]
        //public JsonResult BuscarCampanhaVenda(int? tppId)
        //{
        //    JSONResponse response = new JSONResponse();
        //    try
        //    {
        //        var campanhaVenda = _service.BuscarCampanhaVenda(DateTime.Now, tppId);
        //        response.Add("campanhaVenda", campanhaVenda);
        //    }
        //    catch (Exception e)
        //    {
        //        response.message = Message.Fail(e);
        //        response.success = false;
        //    }

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        [Autorizar(IsAjax = true)]
        public JsonResult ListarTipoPagamentoSimples()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstTipoPagamento = _tipoPagamentoSRV.ListarTipoPagamentoSimples();
                response.Add("lstTipoPagamento", lstTipoPagamento);

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
        public ActionResult PausarOuAtivarCampanhaVenda(int? cveId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                    _service.PausarOuAtivarCampanhaVenda(cveId);
                    SysException.RegistrarLog("Dados da campanha ativa/pausada com sucesso!!", "", SessionContext.autenticado);
                    result.success = true;
                    result.message = Message.Info("Dados do campanha ativa/pausada com sucesso!!");

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

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ExcluirCampanhaVenda(int? cveId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.ExcluirCampanhaVenda(cveId);
                SysException.RegistrarLog("Dados da campanha excluida com sucesso!!", "", SessionContext.autenticado);
                result.success = true;
                result.message = Message.Info("Dados do campanha excluida com sucesso!!");

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

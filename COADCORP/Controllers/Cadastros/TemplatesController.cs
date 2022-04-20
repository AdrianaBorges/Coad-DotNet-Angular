using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Dto;
using COAD.SEGURANCA.Model.Dto.Custons.Pesquisas;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class TemplatesController : Controller
    {
        public TemplateHTMLSRV _service { get; set; }
        public FonteDadosTemplateSRV _fonteDadosTemplateSRV { get; set; }
        //
        // GET: /Templates/

        [Autorizar(PorMenu = false)]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        public JsonResult PesquisarTemplatesHTML(PesquisaTemplatesDTO pesquisaTemplateDTO)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstTemplateHTML = _service.PesquisarTemplatesHTML(pesquisaTemplateDTO);
                response.AddPage("lstTemplateHTML", lstTemplateHTML);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public JsonResult PesquisarFonteDadosTemplate(int? fdaId, string descricao, int pagina = 1, int registrosPorPagina = 6)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstFonteDadosHTML = _fonteDadosTemplateSRV.PesquisarFonteDadosTemplate(fdaId, descricao, pagina, registrosPorPagina);
                response.AddPage("lstFonteDadosHTML", lstFonteDadosHTML);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {            
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Editar(int? tplId)
        {
            ViewBag.tplId = tplId;
            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Prevew(int? tplId)
        {
            ViewBag.tplId = tplId;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarTemplateGrupo()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstTemplateGrupo = ServiceFactory.RetornarServico<TemplateGrupoSRV>().FindAll();
                response.Add("lstTemplateGrupo", lstTemplateGrupo);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDoTemplate(int? tplId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var templateHTML = _service.FindByIdFullLoaded(tplId, true);
                response.Add("templateHTML", templateHTML);

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
        public ActionResult SalvarTemplate(TemplateHTMLDTO template)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarTemplate(template);
                    SysException.RegistrarLog("Dados do template atualizados com sucesso!!", "", SessionContext.autenticado);
                    result.success = true;
                    result.message = Message.Info("Dados do template atualizados com sucesso!!");

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

    }
}

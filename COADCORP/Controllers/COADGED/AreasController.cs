using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.COADGED.Service;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Filter;
using COAD.COADGED.Model.DTO;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using System.IO;
using RTE;

namespace COADCORP.Controllers.COADGED
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...
    
    [ValidateInput(false)]
    public class AreasController : Controller
    {
        private AreasSRV _service = new AreasSRV();

        // preparando para acionar o editor...
        protected Editor PrepareEditor(string obj, Action<Editor> oninit)
        {
            Editor editor = new Editor(System.Web.HttpContext.Current, obj);

            editor.ClientFolder = "/richtexteditor/";
            editor.ContentCss = "/richtexteditor/styles/richtexteditor.css";
            editor.AjaxPostbackUrl = Url.Action("EditorAjaxHandler");

            if (oninit != null)
                oninit(editor);

            bool isajax = editor.MvcInit();
            if (isajax)
                return editor;

            if (this.Request.HttpMethod == "POST")
            {
                string formdata = this.Request.Form[editor.Name];
                if (formdata != null)
                    editor.LoadFormData(formdata);
            }

            return editor;
        }

        [ValidateInput(false)]
        public ActionResult EditorAjaxHandler()
        {
            PrepareEditor("CabecaMateria", delegate(Editor editor) { });
            return new EmptyResult();
        }

        [Autorizar]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Areas(int? areaId, string descricao, int pagina = 0)
        {
            Pagina<COAD.COADGED.Model.DTO.AreasDTO> page = _service.Areas(areaId, descricao: descricao, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("areas", page);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            // cabeça da matéria...
            var obj = 
            PrepareEditor("CabecaMateria", delegate(Editor CabecaMateria)
            {
                CabecaMateria.LoadFormData("");
                CabecaMateria.DisabledItems = "save";
            });

            ViewBag.ARE_CABECA_MATERIA = obj.MvcGetString();

            return View("Edit");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int? areaId = null)
        {
            if (areaId != null)
            {
                ViewBag.areaId = areaId;

                // buscando a área...
                var area = _service.FindById(areaId);

                // cabeça da matéria...
                var obj =
                PrepareEditor("CabecaMateria", delegate(Editor CabecaMateria)
                {
                    CabecaMateria.LoadFormData("");
                    CabecaMateria.Text = area.ARE_CABECA_MATERIA;
                    CabecaMateria.DisabledItems = "save";
                });

                ViewBag.ARE_CABECA_MATERIA = obj.MvcGetString();

                return View("Edit");
            }
            else 
            {
                return View("Index");
            }
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(COAD.COADGED.Model.DTO.AreasDTO areas)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarArea(areas);
                    return Json(result);
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result);
            }
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Remover(int areaId)
        {

            TempData["message"] = "A remoção deste registro não é permitida!";
            return RedirectToAction("Index");

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Readarea(int areaId)
        {
            var areas = _service.FindById(areaId);
            JSONResponse response = new JSONResponse();
            response.Add("areas", areas);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int areaId)
        {
            ViewBag.areaId = areaId;

            // buscando a área...
            var area = _service.FindById(areaId);

            // cabeça da matéria...
            var obj =
            PrepareEditor("CabecaMateria", delegate(Editor CabecaMateria)
            {
                CabecaMateria.LoadFormData("");
                CabecaMateria.Text = area.ARE_CABECA_MATERIA;
                CabecaMateria.DisabledItems = "save";
            });

            ViewBag.ARE_CABECA_MATERIA = obj.MvcGetString();

            return View("Detalhes");
        }
    }
}
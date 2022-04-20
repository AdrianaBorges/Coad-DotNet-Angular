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
using RTE;
using System.Web.UI.WebControls;

namespace COADCORP.Controllers.COADGED
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class TipoMateriaController : Controller
    {
        private TipoMateriaSRV _service = new TipoMateriaSRV();

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
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");
            
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult TiposMaterias(int? tipoMateriaId, string descricao = null, int ativoId = 1, int pagina = 0)
        {
            Pagina<TipoMateriaDTO> page = _service.TiposMaterias(tipoMateriaId, descricao: descricao, ativoId: ativoId, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("tiposMaterias", page);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            // cabeça da matéria...
            var obj =
            PrepareEditor("CabecaMateria", delegate(Editor CabecaMateria)
            {
                CabecaMateria.LoadFormData("");
                CabecaMateria.DisabledItems = "save";
                CabecaMateria.Width = Unit.Percentage(100);
            });

            ViewBag.ARE_CABECA_MATERIA = obj.MvcGetString();

            return View("Edit");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int tipoMateriaId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            ViewBag.tipoMateriaId = tipoMateriaId;

            // buscando...
            var mat = _service.FindById(tipoMateriaId);

            // cabeça da matéria...
            var obj =
            PrepareEditor("CabecaMateria", delegate(Editor CabecaMateria)
            {
                CabecaMateria.LoadFormData("");
                CabecaMateria.Text = mat.ARE_CABECA_MATERIA;
                CabecaMateria.DisabledItems = "save";
                CabecaMateria.Width = Unit.Percentage(100);
            });

            ViewBag.ARE_CABECA_MATERIA = obj.MvcGetString();

            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(TipoMateriaDTO tipoMateria)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarTipoMateria(tipoMateria);
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
        public ActionResult Remover(int tipoMateriaId)
        {

            TempData["message"] = "A remoção deste registro não é permitida!";
            return RedirectToAction("Index");

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReadtipoMateria(int tipoMateriaId)
        {
            var tipoMateria = _service.FindById(tipoMateriaId);
            JSONResponse response = new JSONResponse();
            response.Add("tipoMateria", tipoMateria);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int tipoMateriaId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            ViewBag.tipoMateriaId = tipoMateriaId;

            // buscando...
            var mat = _service.FindById(tipoMateriaId);

            // cabeça da matéria...
            var obj =
            PrepareEditor("CabecaMateria", delegate(Editor CabecaMateria)
            {
                CabecaMateria.LoadFormData("");
                CabecaMateria.Text = mat.ARE_CABECA_MATERIA;
                CabecaMateria.DisabledItems = "save";
                CabecaMateria.Width = Unit.Percentage(100);
                CabecaMateria.SetConfig("readonly", true);
            });

            ViewBag.ARE_CABECA_MATERIA = obj.MvcGetString();

            return View("Detalhes");
        }
    }
}
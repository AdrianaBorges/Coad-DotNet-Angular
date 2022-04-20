using RTE;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service.EditorRTE
{
    public class EditorRTEController : Controller
    {
        // atributo: nome do objeto editor...
        private string objEditor = null;

        // preparando para acionar o editor...
        public Editor PrepareEditor(string obj, Action<Editor> oninit)
        {
            this.objEditor = String.IsNullOrWhiteSpace(obj) ? "Editor1" : obj;
            var r = System.Web.HttpContext.Current;

            Editor editor = new Editor(System.Web.HttpContext.Current, this.objEditor);

            editor.ClientFolder = "/richtexteditor/";
            editor.ContentCss = "/richtexteditor/styles/richtexteditor.css";
            editor.AjaxPostbackUrl = "/Areas/EditorAjaxHandler"; // +r.Request.RequestContext.RouteData.Values["controller"].ToString() + "/EditorAjaxHandler"; //Url.Action("EditorAjaxHandler");

            if (oninit != null)
                oninit(editor);

            bool isajax = editor.MvcInit();
            if (isajax)
                return editor;

            if (r.Request.HttpMethod == "POST")
            {
                string formdata = r.Request.Form[editor.Name];
                if (formdata != null)
                    editor.LoadFormData(formdata);
            }

            return editor;
        }

        [ValidateInput(false)]
        public ActionResult EditorAjaxHandler()
        {
            PrepareEditor(this.objEditor, delegate(Editor editor) { });
            return new EmptyResult();
        }
    }
}

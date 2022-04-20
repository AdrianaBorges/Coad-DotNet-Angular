using Coad.GenericCrud.ActionResultTools;
using RTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;


namespace COAD.COADCORP.Extensions
{
    public static class HtmlExtensions
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="messageName"></param>
        /// <returns></returns>
        public static MvcHtmlString TextEditor(this HtmlHelper helper, string messageName)
        {
            var tempData = helper.ViewContext.Controller.TempData;
            var viewData = helper.ViewContext.Controller.ViewData;
            var editor = new Editor(System.Web.HttpContext.Current, "editor");

            editor.LoadFormData("");
            editor.MvcInit();
            string editorString = editor.MvcGetString();

            StringBuilder sb = new StringBuilder();

            sb.Append("<div app-modal='editor-modal' header='Editar Texto'>");
            sb.Append(editorString);
            sb.Append("</div>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}

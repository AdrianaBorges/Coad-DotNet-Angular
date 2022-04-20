
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GenericCrud.Exceptions.ErrorHandling
{
    public class AjaxExceptionFilterAttribute : HandleErrorAttribute
    {
        public AjaxExceptionFilterAttribute() 
        {
            this.Order = 5;
        }
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("area", "");
                redirectTargetDictionary.Add("action", "Error");
                redirectTargetDictionary.Add("controller", "Home");
                filterContext.ExceptionHandled = true;
                filterContext.Controller.TempData["message"] = ex.Message;
                filterContext.Controller.ViewData["stackTrace"] = ex.StackTrace;
                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);                
           
        }
    }
}
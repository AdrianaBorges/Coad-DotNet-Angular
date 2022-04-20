using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GenericCrud.Exceptions.ErrorHandling
{
    public class ExceptionFilterAttribute : HandleErrorAttribute
    {
        private string Action { get; set; }
        public override void OnException(ExceptionContext filterContext)
        {
            RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
            redirectTargetDictionary.Add("area", "");
            redirectTargetDictionary.Add("action", "Erro");
            redirectTargetDictionary.Add("controller", "Home");
            Exception ex = filterContext.Exception;

            Message msg = ExceptionFormatter.RecursiveShowExceptionsMessage(ex);

            if (ex is NegocioException)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Controller.TempData["exceptionMsg"] = msg;
                filterContext.Controller.TempData["subExceptions"] = msg.subMessages;

            }
            else
            {

                filterContext.ExceptionHandled = true;
                filterContext.Controller.TempData["exceptionMsg"] = msg;
                filterContext.Controller.TempData["subExceptions"] = msg.subMessages;
                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);  
            }                
           
        }
    }
}
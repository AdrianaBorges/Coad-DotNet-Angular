using Coad.GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GenericCrud.ActionResultTools
{
    public class OpenDbContextInViewAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //DbContextFactory.CriarTodosOsDbContexts();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            DbContextFactory.FecharTodosOsDbContexts();
        }
    }
}

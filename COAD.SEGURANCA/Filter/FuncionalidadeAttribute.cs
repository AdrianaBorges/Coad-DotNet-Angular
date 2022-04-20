using COAD.SEGURANCA.Service;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace COAD.SEGURANCA.Filter
{
    public class FuncionalidadeAttribute : ActionFilterAttribute
    {
        public int FuncionalidadeId { get; set; }
        public string DescricaoFuncionalidade { get; set; }
        

        public FuncionalidadeAttribute(int FuncionalidadeId, string DescricaoFuncionalidade)
        {
            this.FuncionalidadeId = FuncionalidadeId;
            this.DescricaoFuncionalidade = DescricaoFuncionalidade;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(filterContext.RequestContext != null 
                && filterContext.RequestContext.HttpContext != null 
                && FuncionalidadeId  > 0)
            {
                var funcionalidadeSistema = ServiceFactory.RetornarServico<FuncionalidadeSistemaSRV>().FindById(FuncionalidadeId);
                if(funcionalidadeSistema != null)
                {
                    filterContext.RequestContext.HttpContext.Items.Add("funcionalidadeSistema", funcionalidadeSistema);                    
                }                
            }
        }
    }
}

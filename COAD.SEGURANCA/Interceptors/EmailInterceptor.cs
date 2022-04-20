using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Interceptors
{
    public class EmailInterceptor : IInterceptor
    {

        public void Intercept(IInvocation invocation)
        {
            var nome = invocation.Method.Name;
            //if (invocation.Method.GetParameters().Where(x => x.Name == "metodoOrigem").Count() > 0)
            //{
            //    invocation.SetArgumentValue(5, nome);
            //}

            invocation.Proceed();
        }
    }
}

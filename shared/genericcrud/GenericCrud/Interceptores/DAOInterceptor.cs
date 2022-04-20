using Castle.DynamicProxy;
using Coad.GenericCrud.Dao.Base;
using GenericCrud.Dao.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Interceptores
{
    public class DAOInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "db")
            {
                var dao = invocation.InvocationTarget as IRepository;
                dao.RefrechDbContext();

            }
        }
    }
}

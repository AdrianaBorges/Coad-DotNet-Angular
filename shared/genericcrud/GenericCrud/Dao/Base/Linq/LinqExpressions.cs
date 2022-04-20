using Coad.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Dao.Base.Linq
{
    public class LinqExpressions
    {
        public object CriarExpressao<Tsource>(string propertyName, object value)
        {
            if (!string.IsNullOrWhiteSpace(propertyName) && value != null)
            {
                PropertyInfo propertyInf = ReflectionProvider.GetPropertyInfo<Tsource>(propertyName);
                var instanceExpression = Expression.Parameter(propertyInf.DeclaringType);
                var tes = Expression.Lambda(Expression.MakeMemberAccess(instanceExpression, propertyInf), instanceExpression);
                return tes;

            }
            return null;

        }
    }
}

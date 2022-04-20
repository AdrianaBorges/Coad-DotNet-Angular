using AutoMapper;
using Coad.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Dao.Base
{
    public class ResultExpression<S,Ds>
    {
        public S source { get; set; }
        public Ds destiny { get; set; }

        /**
        public ResultExpression<S,Ds> Include<O,D>(string path)
        {
            if (source is IEnumerable<object>)
            {
                foreach(var obj in (IEnumerable<object>)
            }
           
        }*/

        private void _Set<O, D>(object obj, object destiny, string path)
        {
            O property = ReflectionProvider.GetPropertyValue<O>(obj, path);
            if (property != null)
            {
                D newProperty = Mapper.Map<O, D>(property);
                ReflectionProvider.SetPropertyValue<D>(destiny, "path", newProperty);
            }
            
        }
    }
}
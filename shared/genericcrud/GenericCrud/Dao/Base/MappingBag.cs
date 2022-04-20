using Coad.GenericCrud.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Dao.Base
{
    public class MappingBag<T,S> : IQueryable<T>
    {
        public MappingBag(IEnumerable<S> queryable)
        {
            this.source = queryable;
        }    

        public IEnumerable<S> source { get; set; }

        private IList<T> Convert(){

            IList<T> list = MapperWrapper.Convert<IEnumerable<S>, List<T>>(source);
            return list;
        }
        public IEnumerator<T> GetEnumerator()
        {
            IList<T> list = Convert();          
            return list.GetEnumerator();
        
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            IList<T> list = Convert();
            return source.GetEnumerator();
        }

        public Type ElementType
        {
            get { return source.GetType(); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get {

                IList<T> lst = Convert();
                return lst.AsQueryable().Expression;
            }
        }

        public IQueryProvider Provider
        {
            get {
                IList<T> lst = Convert();
                return lst.AsQueryable().Provider; 
            }
        }

        
    }

    public static class MappingBagExtended
    {
        public static IQueryable<T> AsMappingBag<T, S>(this IEnumerable<S> source)
        {            
            return new MappingBag<T,S>(source);
        }
    }
}

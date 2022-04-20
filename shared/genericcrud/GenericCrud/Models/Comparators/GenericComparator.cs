using Coad.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Comparators
{
    public class GenericComparator<TSource> : IEqualityComparer<TSource>
    {
        private string[] Keys { get; set; }

        public GenericComparator()
        {
           
        }

        public GenericComparator(params string[] Keys)
        {
            this.Keys = Keys;
        }

        public bool Equals(TSource x, TSource y)
        {
            bool resp = true;

            if (Keys != null)
            {
                foreach (var key in Keys)
                {
                    if (!resp)
                    {
                        return false;
                    }

                    var valueX = ReflectionProvider.GetPropertyValue<object>(x, key);
                    var valueY = ReflectionProvider.GetPropertyValue<object>(y, key);

                    resp = (resp && (valueX == null && valueY == null) || (valueX != null && valueY != null && valueX.Equals(valueY)));
                }
            }

            return resp;
        }

        public int GetHashCode(TSource obj)
        {
            int hash = 0;

            if (Keys != null)
            {
                foreach (var key in Keys)
                {
                    var value = ReflectionProvider.GetPropertyValue<object>(obj, key);
                    hash += (value != null) ? value.GetHashCode() : 0;
                }
            }

            return hash;
        }
    }
}

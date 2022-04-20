using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProxyConfigAttribute : Attribute
    {
        public Type ProxyType { get; set; }        
        public ProxyConfigAttribute()
        {

        }
    }
}

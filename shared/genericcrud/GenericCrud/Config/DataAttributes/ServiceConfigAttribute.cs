using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceConfigAttribute : Attribute
    {
        public string[] Keys { get; set; }
        public string profileName { get; set; }        

        public ServiceConfigAttribute()
        {

        }

        public ServiceConfigAttribute(params string[] Keys)
        {
            this.Keys = Keys;
        }
    }
}

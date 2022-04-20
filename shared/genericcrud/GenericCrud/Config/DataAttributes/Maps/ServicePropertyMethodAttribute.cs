using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes.Maps
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServicePropertyMethodAttribute : Attribute
    {
        public string Name { get; set; }
        public ServicePropertyMethodAttribute()
        {

        }
    }

}

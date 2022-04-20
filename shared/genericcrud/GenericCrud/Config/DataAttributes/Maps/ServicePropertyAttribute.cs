using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes.Maps
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ServicePropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string[] Keys { get; set; }
        public bool FindById { get; set; }
        

        public ServicePropertyAttribute(params string[] Keys)
        {
            this.Keys = Keys;
        }
    }

}

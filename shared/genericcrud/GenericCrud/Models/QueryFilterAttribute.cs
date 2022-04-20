using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryFilterAttribute : Attribute
    {
        public QueryFilterAttribute()
        {

        }
        public QueryFilterAttribute(Type DTOType, string Name = null)
        {
            this.DTOType = DTOType;
            this.Name = Name;
        }

        public Type DTOType { get; set; }
        public string Name { get; set; }
    }
}

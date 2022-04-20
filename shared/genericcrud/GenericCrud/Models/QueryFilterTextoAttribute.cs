using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryFilterTextoAttribute : QueryFilterPropertyDescAttribute
    {
        public QueryFilterTextoAttribute(string PropertyPath, string Label) : base(PropertyPath, Label, TipoFiltroEnum.Texto)
        {
        
        }
    }
}

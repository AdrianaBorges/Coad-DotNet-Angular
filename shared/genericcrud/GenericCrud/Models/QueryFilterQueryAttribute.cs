using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryFilterQueryAttribute : QueryFilterPropertyDescAttribute
    {
        public QueryFilterQueryAttribute() : base(null, null, TipoFiltroEnum.Query)
        {
        
        }
    }
}

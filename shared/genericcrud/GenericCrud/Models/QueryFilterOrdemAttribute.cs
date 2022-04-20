using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryFilterOrdemAttribute : Attribute
    {
        public QueryFilterOrdemAttribute()
        {

        }
        public QueryFilterOrdemAttribute(int Ordem)
        {
            this.Ordem = Ordem;
        }
        
        public int Ordem { get; set; }
    }
}

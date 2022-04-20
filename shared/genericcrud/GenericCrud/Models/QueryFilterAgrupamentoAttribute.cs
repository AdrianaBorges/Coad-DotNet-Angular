using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryFilterAgrupamentoAttribute : Attribute
    {
        public QueryFilterAgrupamentoAttribute()
        {

        }
        public QueryFilterAgrupamentoAttribute(string NomeGrupo)
        {
            this.NomeGrupo = NomeGrupo;
        }
        
        public string NomeGrupo { get; set; }
    }
}

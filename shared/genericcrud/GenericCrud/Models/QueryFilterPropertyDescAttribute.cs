using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class QueryFilterPropertyDescAttribute : Attribute
    {
        public QueryFilterPropertyDescAttribute()
        {

        }
        public QueryFilterPropertyDescAttribute(string PropertyPath, string Label, TipoFiltroEnum Tipo)
        {
            this.PropertyPath = PropertyPath;
            this.Label = Label;
            this.Tipo = Tipo;
        }

        public string PropertyPath { get; set; }
        public string Label { get; set; }
        public int Size { get; set; }

        private TipoFiltroEnum Tipo { get; set; }

        public TipoFiltroEnum GetTipoFiltro()
        {
            return Tipo;
        }
    }
}

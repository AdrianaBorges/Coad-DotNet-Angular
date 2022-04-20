using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryFilterSelectAttribute : QueryFilterPropertyDescAttribute
    {
        public QueryFilterSelectAttribute(Type Target, string valueName, string labelName, string PropertyPath, string Label) : base(PropertyPath, Label, TipoFiltroEnum.Select)
        {
            this.Target = Target;
            this.valueName = valueName;
            this.labelName = labelName;
        }

        public Type Target { get; set; }
        public string valueName { get; set; }
        public string labelName { get; set; }

    }
}

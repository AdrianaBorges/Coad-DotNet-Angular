using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes.Maps
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MappingAttribute : Attribute
    {
        public Type Source { get; set; }
        public string confRef { get; set; }
        public bool ReverseMapping { get; set; }

        public MappingAttribute()
        {
            confRef = "padrao";
            ReverseMapping = true;
        }


        //Novo construtor
        public MappingAttribute(Type Source)
        {
            this.Source = Source;
            confRef = "padrao";
            ReverseMapping = true;
        }
    }

}

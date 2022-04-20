using GenericCrud.Config.DataAttributes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes.Maps
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreMemberMappingAttribute : Attribute
    {
        public MappingDirection Direction { get; set; }
        public string MappingRef { get; set; }
        
        public IgnoreMemberMappingAttribute()
        {
            MappingRef = "padrao";
        }

        public IgnoreMemberMappingAttribute(MappingDirection Direction)
        {
            MappingRef = "padrao";
            this.Direction = Direction;
        }

        public IgnoreMemberMappingAttribute(string MappingRef)
        {
            this.MappingRef = MappingRef;
        }

        public IgnoreMemberMappingAttribute(MappingDirection Direction, string MappingRef)
        {
            MappingRef = "padrao";
            this.Direction = Direction;
            this.MappingRef = MappingRef;
        }
    }

}

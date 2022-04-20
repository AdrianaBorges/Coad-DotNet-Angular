using GenericCrud.Config.DataAttributes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes.Maps
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingMemberAttribute : Attribute
    {      
        public MappingMemberAttribute()
        {
            
        }
    }

}

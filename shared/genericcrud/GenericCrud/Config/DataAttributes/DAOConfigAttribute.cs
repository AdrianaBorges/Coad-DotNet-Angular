using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.DataAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DAOConfigAttribute : Attribute
    {
        public string ProfileName { get; set; }

        public DAOConfigAttribute()
        {

        }

        public DAOConfigAttribute(string profileName)
        {
            this.ProfileName = profileName;
        }
    }
}

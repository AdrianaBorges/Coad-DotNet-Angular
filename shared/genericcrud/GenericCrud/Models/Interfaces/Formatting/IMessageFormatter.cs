using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Interfaces.Formatting
{
    
    public interface IMessageFormatter
    {
        string Format(object source);
    }
}

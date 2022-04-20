using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class NFeException : Exception
    {
        public NFeException()
        {

        }

        public NFeException(string message) : base(message)
        {
            
        }

        public NFeException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

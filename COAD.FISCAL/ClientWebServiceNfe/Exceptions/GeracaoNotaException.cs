using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Exceptions
{
    public class GeracaoNotaException : Exception
    {
        public GeracaoNotaException()
        {

        }

        public GeracaoNotaException(string message) : base(message)
        {

        }

        public GeracaoNotaException(string message, Exception e) : base(message, e)
        {

        }

    }
}

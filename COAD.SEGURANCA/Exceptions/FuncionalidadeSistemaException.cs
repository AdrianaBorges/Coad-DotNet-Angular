using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Exceptions
{
    public class FuncionalidadeSistemaException : Exception
    {
        public FuncionalidadeSistemaException()
        {

        }

        public FuncionalidadeSistemaException(string message) : base(message)
        {
            
        }

        public FuncionalidadeSistemaException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

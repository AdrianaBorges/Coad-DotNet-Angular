using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Exceptions
{
    public class UsuarioJaExisteException : Exception
    {
        public UsuarioJaExisteException()
        {

        }

        public UsuarioJaExisteException(string message) : base(message)
        {
            
        }

        public UsuarioJaExisteException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Exceptions
{
    public class LoginInvalidoException : Exception
    {
        public LoginInvalidoException()
        {

        }

        public LoginInvalidoException(string message) : base(message)
        {
            
        }

        public LoginInvalidoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

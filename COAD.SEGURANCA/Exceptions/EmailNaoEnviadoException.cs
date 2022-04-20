using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Exceptions
{
    public class EmailNaoEnviadoException : Exception
    {
        public EmailNaoEnviadoException()
        {

        }

        public EmailNaoEnviadoException(string message) : base(message)
        {
            
        }

        public EmailNaoEnviadoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

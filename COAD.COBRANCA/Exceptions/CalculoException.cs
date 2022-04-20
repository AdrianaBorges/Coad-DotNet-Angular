using System;

namespace COAD.COBRANCA.Exceptions
{
    public class CalculoException : Exception
    {
        public CalculoException()
        {

        }

        public CalculoException(string message) : base(message)
        {
            
        }

        public CalculoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

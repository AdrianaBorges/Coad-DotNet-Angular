using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Exceptions
{
    public class NegocioException : ValidacaoException
    {
        private string message;

        public NegocioException()
        {

        }

        public NegocioException(string message) : base(message)
        {
            
        }

        public NegocioException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Exceptions
{
    /// <summary>
    /// Exception Para indicar erro de permissão de acesso ou de login
    /// </summary>
    public class MappingException : ValidacaoException
    {
        private string message;

        public MappingException()
        {

        }

        public MappingException(string message) : base(message)
        {
            
        }

        public MappingException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
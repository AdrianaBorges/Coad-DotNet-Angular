using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Exceptions
{
    /// <summary>
    /// Exception Para indicar erro de permissão de acesso ou de login
    /// </summary>
    public class AcessoException : ValidacaoException
    {
        private string message;

        public AcessoException()
        {

        }

        public AcessoException(string message) : base(message)
        {
            
        }

        public AcessoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
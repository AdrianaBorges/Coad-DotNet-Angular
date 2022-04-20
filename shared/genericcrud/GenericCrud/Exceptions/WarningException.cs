using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Exceptions
{
    public class WarningException : Exception
    {
        private string message;

        public WarningException()
        {

        }

        public WarningException(string message) : base(message)
        {
            
        }

        public WarningException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
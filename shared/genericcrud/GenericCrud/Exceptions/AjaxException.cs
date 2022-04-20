using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Exceptions
{
    public class AjaxException : Exception
    {
        public AjaxException()
        {

        }

        public AjaxException(string message) : base(message)
        {
            
        }

        public AjaxException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
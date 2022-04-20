using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Exceptions
{
    public class SchedulerException : Exception
    {
        private string message;

        public SchedulerException()
        {

        }

        public SchedulerException(string message) : base(message)
        {
            
        }

        public SchedulerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
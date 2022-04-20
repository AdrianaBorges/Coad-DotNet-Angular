using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Exceptions
{
    public class UploadException : Exception
    {
        public UploadException()
        {

        }

        public UploadException(string message) : base(message)
        {
            
        }

        public UploadException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class NotificacaoException : Exception
    {
        public NotificacaoException()
        {

        }

        public NotificacaoException(string message) : base(message)
        {
            
        }

        public NotificacaoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

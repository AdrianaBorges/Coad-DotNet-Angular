using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class CarteiramentoException : Exception
    {
        public CarteiramentoException()
        {

        }

        public CarteiramentoException(string message) : base(message)
        {
            
        }

        public CarteiramentoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

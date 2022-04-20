using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class PagamentoException : FaturamentoException
    {
        public PagamentoException()
        {

        }

        public PagamentoException(string message) : base(message)
        {
            
        }

        public PagamentoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class FaturamentoException : Exception
    {
        public FaturamentoException()
        {

        }

        public FaturamentoException(string message) : base(message)
        {
            
        }

        public FaturamentoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

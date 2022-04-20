using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class PagamentoNaoRealizadoException : FaturamentoException
    {
        public PagamentoNaoRealizadoException()
        {

        }

        public PagamentoNaoRealizadoException(string message) : base(message)
        {
            
        }

        public PagamentoNaoRealizadoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

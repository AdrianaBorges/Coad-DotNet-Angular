using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class AcessoADadosNaoPermitidoException : Exception
    {
        public AcessoADadosNaoPermitidoException()
        {

        }

        public AcessoADadosNaoPermitidoException(string message) : base(message)
        {
            
        }

        public AcessoADadosNaoPermitidoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

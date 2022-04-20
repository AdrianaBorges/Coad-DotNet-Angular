using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class OperadoraNaoEncontradaPorRodizioException : Exception
    {
        public OperadoraNaoEncontradaPorRodizioException()
        {

        }

        public OperadoraNaoEncontradaPorRodizioException(string message) : base(message)
        {
            
        }

        public OperadoraNaoEncontradaPorRodizioException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

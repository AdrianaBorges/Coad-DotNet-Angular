using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    /// <summary>
    /// Quando ocorre problemas em alguma operação de uma chamada webservice.
    /// </summary>
    public class ServicoException : Exception
    {
        public ServicoException()
        {

        }

        public ServicoException(string message) : base(message)
        {
            
        }

        public ServicoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

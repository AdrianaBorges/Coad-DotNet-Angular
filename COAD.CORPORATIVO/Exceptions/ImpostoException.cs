using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    /// <summary>
    /// Quando ocorre problemas em alguma operação de cálculo de imposto
    /// </summary>
    public class ImpostoException : Exception
    {
        public ImpostoException()
        {

        }

        public ImpostoException(string message) : base(message)
        {
            
        }

        public ImpostoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

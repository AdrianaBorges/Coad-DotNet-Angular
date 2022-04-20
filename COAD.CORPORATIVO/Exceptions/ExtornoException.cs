using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    /// <summary>
    /// Essa exception é lançada quando há qualquer erro referente a operações de extorno.
    /// </summary>
    public class ExtornoException : Exception
    {
        public ExtornoException()
        {

        }

        public ExtornoException(string message) : base(message)
        {
            
        }

        public ExtornoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

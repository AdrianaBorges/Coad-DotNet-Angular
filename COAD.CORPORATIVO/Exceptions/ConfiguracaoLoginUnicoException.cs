using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class ConfiguracaoLoginUnicoException : Exception
    {
        public ConfiguracaoLoginUnicoException()
        {

        }

        public ConfiguracaoLoginUnicoException(string message) : base(message)
        {
            
        }

        public ConfiguracaoLoginUnicoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Exceptions
{
    public class RegistroLiberacaoException : FaturamentoException
    {
        public RegistroLiberacaoException()
        {

        }

        public RegistroLiberacaoException(string message) : base(message)
        {
            
        }

        public RegistroLiberacaoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

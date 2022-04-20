using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Exceptions
{
    /// <summary>
    /// Quando uma funcionalidade não está disponível para uso em produção
    /// </summary>
    public class NaoDispEmProdException : Exception
    {
        public NaoDispEmProdException()
        {

        }

        public NaoDispEmProdException(string message) : base(message)
        {
            
        }

        public NaoDispEmProdException(string message, Exception innerException)
            : base(message, innerException)
        {

        }


    }
}

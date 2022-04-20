using COAD.SEGURANCA.Model.Custons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Exceptions
{
    public class BatchJobException : Exception
    {
        public RegistroNotificacaoSistemaDTO reg;

        public BatchJobException(RegistroNotificacaoSistemaDTO reg)
        {
            this.reg = reg;
        }
        

        public BatchJobException(RegistroNotificacaoSistemaDTO reg, string message, Exception innerException)
            : base(message, innerException)
        {
            this.reg = reg;
        }
    }
}

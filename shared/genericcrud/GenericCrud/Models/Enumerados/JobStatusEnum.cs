using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Enumerados
{
    public enum JobStatusEnum
    {
        PENDENTE = 0,
        CONCLUIDO = 1,
        ERRO = 2,
        CONCLUIDO_COM_ERROS = 3,
        CONCLUINDO_OPERACAO = 4,
    }
}

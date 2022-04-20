using COAD.SEGURANCA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COBRANCA.Bancos.Model.DTO.Interfaces
{
    public interface INossoNumeroGenerator
    {
        string GerarNossoNumero(ContaDTO contaDTO, int? sequencial);
    }
}

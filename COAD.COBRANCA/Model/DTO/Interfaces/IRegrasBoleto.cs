using COAD.COBRANCA.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COBRANCA.Model.DTO.Interfaces
{
    public interface IRegrasBoleto
    {
        string CodigoCarteira { get; }
        BancoEnum BancoEnum { get; }
        string CalcularDigitoNossoNumero(string nossoNumero);
    }
}

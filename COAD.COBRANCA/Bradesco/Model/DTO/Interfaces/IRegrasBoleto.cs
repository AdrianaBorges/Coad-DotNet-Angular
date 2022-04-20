
using COAD.COBRANCA.Bradesco.Model.Enumerados;

namespace COAD.COBRANCA.Bradesco.Model.DTO.Interfaces
{
    public interface IRegrasBoleto
    {
        string CodigoCarteira { get; }
        BancoEnum BancoEnum { get; }
        string CalcularDigitoNossoNumero(string nossoNumero);
    }
}


using COAD.COBRANCA.Bancos.Model.Enumerados;

namespace COAD.COBRANCA.Bancos.Model.DTO.Interfaces
{
    public interface IRegrasBoleto
    {
        string CodigoCarteira { get; }
        BancoEnum BancoEnum { get; }
        string CalcularDigitoNossoNumero(string nossoNumero);
    }
}

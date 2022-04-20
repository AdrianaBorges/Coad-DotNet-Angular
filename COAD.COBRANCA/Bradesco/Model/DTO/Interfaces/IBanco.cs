using System;
using COAD.COBRANCA.Bradesco.Service;

namespace COAD.COBRANCA.Bradesco.Model.DTO.Interfaces
{
    public interface IBanco
    {
        string CodigoBanco { get; set; }
        string NomeBanco { get; set; }

        string CalcularDigitoNossoNumero(string nossoNumero, string CodigoCarteira);
        string CalcularDVCodigoBarras(string codigoBarras);
        string CalcularDVLinhaDigitavel(string campoLinhaDigitavel);
        
    }
}

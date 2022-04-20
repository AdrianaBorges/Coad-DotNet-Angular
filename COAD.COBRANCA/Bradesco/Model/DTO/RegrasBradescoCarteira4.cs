using COAD.COBRANCA.Exceptions;
using COAD.COBRANCA.Bradesco.Model.DTO.Interfaces;
using COAD.COBRANCA.Bradesco.Model.Enumerados;
using COAD.COBRANCA.Util;
using GenericCrud.Util;

namespace COAD.COBRANCA.Model.DTO
{
    public class RegrasBradescoCarteira4 : IRegrasBoleto
    {
        public string CodigoCarteira => "4";

        public BancoEnum BancoEnum => BancoEnum.BRADESCO;

        public string CalcularDigitoNossoNumero(string nossoNumero)
        {
            if (string.IsNullOrWhiteSpace(nossoNumero))
                throw new CalculoException("Nosso número não informado.");

            if (string.IsNullOrWhiteSpace(CodigoCarteira))
                throw new CalculoException("Código da Carteira não informada.");

            var codigoCarteira = StringUtil.PreencherZeroEsquerda(CodigoCarteira, 2);
            var nossoNumeroComposto = codigoCarteira + nossoNumero;

            var modulo = MathUtil.CalcularModuloDecrescente(nossoNumeroComposto, 13, 11, 7);

            if (modulo == 1)
                return "P";
            if (modulo == 0)
                return "0";

            var digito = 11 - modulo;
            
            if (digito != null)
            {
                return digito.ToString();
            }
            else
            {
                throw new CalculoException("O cálculo do dígito retornou um valor incorreto.");
            }
            
        }
        
    }
}

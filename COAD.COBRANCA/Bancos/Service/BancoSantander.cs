using COAD.COBRANCA.Bancos.Model.Constants;
using COAD.COBRANCA.Bancos.Model.DTO;
using COAD.COBRANCA.Bancos.Model.DTO.Interfaces;
using COAD.COBRANCA.Exceptions;
using COAD.COBRANCA.Util;
using COAD.SEGURANCA.Model;
using GenericCrud.Util;
using System;
using System.Linq;


namespace COAD.COBRANCA.Bancos.Service
{
    public class BancoSantander : IBanco
    {
        public string CodigoBanco { get; set; }
        public string NomeBanco { get; set; }
        public string Logo => "santander-logo.jpg";
        public string BancoDesc => "033-7";
        public bool UsarConfigDoBanco { get; set; }
        public bool ExcluirDigitoNossoNumero { get; }

        public BancoSantander()
        {
            this.NomeBanco = "Banco Santander S.A.";
            this.CodigoBanco = "033";
        }
        

        public string CalcularDVCodigoBarras(string codigoBarras)
        {
            var modulo = MathUtil.CalcularModuloCrescente(codigoBarras, 43, 11, 9);

            if(modulo != null)
            {
                var digito = 11 - modulo;
                if (digito == 0 || digito == 1 || digito > 9)
                    return "1";
                else
                    return digito.ToString();
            }
            return null;
        }

        public string CalcularDigitoNossoNumero(string nossoNumero, string trash)
        {

            if (string.IsNullOrWhiteSpace(nossoNumero))
                throw new CalculoException("Nosso número não informado.");

            var modulo = MathUtil.CalcularModuloDecrescente(nossoNumero, 12, 11, 9);

            if (modulo == 10)
                return "1";
            else if ((modulo == 0) || (modulo == 1))
                return "0";
            else
                return (11 - modulo).ToString();

        }

        public string CalcularDVLinhaDigitavel(string campoLinhaDigitavel)
        {
            var verificador = MathUtil.CalcularModulo10(campoLinhaDigitavel);

            if(verificador != null)
                return verificador.ToString();

            return null;
        }

        public CodigoBarrasDTO GerarCodigoBarras(DateTime? DataVencimento, string NossoNumero, decimal? Valor, ContaDTO Conta)
        {
            var cb = new CodigoBarrasDTO(Conta);
            cb.DataVencimento = DataVencimento;
            cb.NossoNumero = NossoNumero;
            cb.Valor = Valor;
            cb.CodBeneficiario = Conta.CTA_CONVENIO.Substring(0, 7);


            MathUtil.CalcularFatorVencimento(cb);
            cb.CodBeneficiario = StringUtil.PreencherZeroEsquerda(cb.CodBeneficiario, 7);
            //cb.Carteira = StringUtil.PreencherZeroEsquerda(cb.Carteira, 2);

            var contaStr = cb.NumeroConta;

            if (!string.IsNullOrWhiteSpace(contaStr))
            {
                contaStr = contaStr.Split('-').FirstOrDefault();
                cb.NumeroConta = StringUtil.PreencherZeroEsquerda(contaStr, 7);
            }
            if (!string.IsNullOrWhiteSpace(cb.NossoNumero))
            {
                cb.NossoNumero = StringUtil.PreencherZeroEsquerda(cb.NossoNumero, 12);
                cb.NossoNumero += CalcularDigitoNossoNumero(cb.NossoNumero, "007");
            }

            var CodigoBarra01 = $@"{cb.IdentificacaoBanco}{cb.CodigoMoeda}";

            var CodigoBarra02 = $@"{cb.FatorVencimento}{cb.ValorStr}9";

            var CodigoBarra03 = $@"{cb.CodBeneficiario}{cb.NossoNumero}0104";

            var codigoBarraProvisorio = CodigoBarra01 + CodigoBarra02 + CodigoBarra03;

            cb.DigitoVerificadorCodigoBarras = this.CalcularDVCodigoBarras(codigoBarraProvisorio);

            cb.CodigoBarras = $@"{CodigoBarra01}{cb.DigitoVerificadorCodigoBarras}{CodigoBarra02}{CodigoBarra03}";

            return cb;
        }

        public CamposLinhaDigitalDTO SepararCamposVariaveisLinhaDigitavel(CodigoBarrasDTO cb)
        {
            var campo1 = $@"{cb.IdentificacaoBanco}{cb.CodigoMoeda}9{cb.CodBeneficiario.Substring(0, 4)}";
            var campo2 = $@"{cb.CodBeneficiario.Substring(4, 3)}{cb.NossoNumero.Substring(0, 7)}";
            var campo3 = $@"{cb.NossoNumero.Substring(7, 6)}0104";


            var result = new CamposLinhaDigitalDTO()
            {
                Campo1 = campo1,
                Campo2 = campo2,
                Campo3 = campo3
            };

            return result;
        }

        public string FormatarContaAgencia(ContaDTO conta)
        {
            throw new NotImplementedException();
        }
    }
}

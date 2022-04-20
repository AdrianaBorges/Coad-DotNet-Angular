using COAD.COBRANCA.Bradesco.Model.Constants;
using COAD.COBRANCA.Bradesco.Model.DTO;
using COAD.COBRANCA.Bradesco.Model.DTO.Interfaces;
using COAD.COBRANCA.Exceptions;
using COAD.COBRANCA.Util;
using COAD.SEGURANCA.Model;
using GenericCrud.Util;
using System;
using System.Linq;


namespace COAD.COBRANCA.Bradesco.Service
{
    public class BancoSantander : IBanco
    {
        public string CodigoBanco { get; set; }
        public string NomeBanco { get; set; }

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

        public string CalcularDigitoNossoNumero(string nossoNumero, string CodigoCarteira)
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
            cb.CodBeneficiario = Conta.CTA_CONVENIO;


            MathUtil.CalcularFatorVencimento(cb);
            cb.CodBeneficiario = StringUtil.PreencherZeroEsquerda(cb.CodBeneficiario, 7);
            cb.Carteira = StringUtil.PreencherZeroEsquerda(cb.Carteira, 2);

            var contaStr = cb.NumeroConta;

            if (!string.IsNullOrWhiteSpace(contaStr))
            {
                contaStr = contaStr.Split('-').FirstOrDefault();
                cb.NumeroConta = StringUtil.PreencherZeroEsquerda(contaStr, 7);
            }
            if (!string.IsNullOrWhiteSpace(cb.NossoNumero))
            {
                cb.NossoNumero = StringUtil.PreencherZeroEsquerda(cb.NossoNumero, 13);
            }

            var CodigoBarra01 = $@"{cb.IdentificacaoBanco}{cb.CodigoMoeda}";

            var CodigoBarra02 = $@"{cb.FatorVencimento}{cb.ValorStr}9";

            var CodigoBarra03 = $@"{cb.CodBeneficiario}{cb.NossoNumero}0104";

            var codigoBarraProvisorio = CodigoBarra01 + CodigoBarra02 + CodigoBarra03;

            cb.DigitoVerificadorCodigoBarras = this.CalcularDVCodigoBarras(codigoBarraProvisorio);

            cb.CodigoBarras = $@"{CodigoBarra01}{cb.DigitoVerificadorCodigoBarras}{CodigoBarra02}{CodigoBarra03}";

            return cb;
        }


    }
}

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
    public class BancoItau : IBanco
    {
        public string CodigoBanco { get; set; }
        public string NomeBanco { get; set; }

        public BancoItau()
        {
            this.NomeBanco = "Banco Itaú S.A.";
            this.CodigoBanco = "341";
        }
        

        private void GerarCampoLivre(CodigoBarrasDTO cb)
        {
            if (cb != null)
            {
                cb.CampoLivre = $@"{cb.Carteira}{cb.NossoNumero}{cb.AgenciaBeneficiario}{cb.NumeroConta}000";
            }
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

            MathUtil.CalcularFatorVencimento(cb);
            cb.AgenciaBeneficiario = StringUtil.PreencherZeroEsquerda(cb.AgenciaBeneficiario, 4);
            cb.Carteira = StringUtil.PreencherZeroEsquerda(cb.Carteira, 2);

            var contaStr = cb.NumeroConta;

            if (!string.IsNullOrWhiteSpace(contaStr))
            {
                contaStr = contaStr.Replace("-", "");
                cb.NumeroConta = StringUtil.PreencherZeroEsquerda(contaStr, 6);
            }
            if (!string.IsNullOrWhiteSpace(cb.NossoNumero))
            {
                cb.NossoNumero = StringUtil.PreencherZeroEsquerda(cb.NossoNumero, 9);
            }

            this.GerarCampoLivre(cb);
            var primeiraMetadeCodigoBarra = $@"{cb.IdentificacaoBanco}{cb.CodigoMoeda}";
            var segundaMetadeCodigoBarra = $@"{cb.FatorVencimento}{cb.ValorStr}{cb.CampoLivre}";

            var codigoBarraProvisorio = primeiraMetadeCodigoBarra + segundaMetadeCodigoBarra;

            cb.DigitoVerificadorCodigoBarras = this.CalcularDVCodigoBarras(codigoBarraProvisorio);

            cb.CodigoBarras = $@"{primeiraMetadeCodigoBarra}{cb.DigitoVerificadorCodigoBarras}{segundaMetadeCodigoBarra}";

            return cb;
        }


    }
}

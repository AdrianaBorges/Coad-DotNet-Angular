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
    public class BancoBanrisul : IBanco
    {
        public string CodigoBanco { get; set; }
        public string NomeBanco { get; set; }

        public BancoBanrisul()
        {
            this.NomeBanco = "Banco Banrisul";
            this.CodigoBanco = "041";
        }      

        private void GerarCampoLivre(CodigoBarrasDTO cb)
        {

            if (cb != null)
            {

                var numeroconta = cb.NumeroConta.Substring(0, 7);

                cb.CampoLivre = $@"21{cb.AgenciaBeneficiario}{numeroconta}{cb.NossoNumero}40";

                var dig01 = this.CalcularDVCodigoBarrasMod10(cb.CampoLivre);
                cb.CampoLivre += dig01;

                var dig02 = this.CalcularDVCodigoBarrasMod11Lim7(cb.CampoLivre);
                cb.CampoLivre += dig02; 

            }

        }

        public string CalcularDVCodigoBarrasMod11Lim7(string codigoBarras)
        {
            var modulo = MathUtil.CalcularModuloCrescente(codigoBarras, codigoBarras.Length, 11, 7);

            if (modulo != null)
            {
                var digito = 11 - modulo;
                if (digito == 0 || digito == 1 || digito > 9)
                    return "1";
                else
                    return digito.ToString();
            }
            return null;
        }

        public string CalcularDVCodigoBarras(string codigoBarras, int numeroCaracteres)
        {
            var modulo = MathUtil.CalcularModuloCrescente(codigoBarras, numeroCaracteres, 11, 9);

            if (modulo != null)
            {
                var digito = 11 - modulo;
                if (digito == 0 || digito == 1 || digito > 9)
                    return "1";
                else
                    return digito.ToString();
            }
            return null;
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
        public string CalcularDVCodigoBarrasMod10(string codigoBarras)
        {
            var verificador = MathUtil.CalcularModulo10(codigoBarras);

            if (verificador != null)
                return verificador.ToString();

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
                contaStr = contaStr.Split('-').FirstOrDefault();
                cb.NumeroConta = StringUtil.PreencherZeroEsquerda(contaStr, 7);
            }
            if (!string.IsNullOrWhiteSpace(cb.NossoNumero))
            {               
                cb.NossoNumero = StringUtil.PreencherZeroEsquerda(cb.NossoNumero.Substring(0, 8), 8);
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

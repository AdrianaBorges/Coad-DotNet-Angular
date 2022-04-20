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
    public class BancoSICOOB : IBanco
    {
        public string CodigoBanco { get; set; }
        public string NomeBanco { get; set; }
        public string Logo => "sicoob-logo.jpg";
        public string BancoDesc => "237-2";
        public bool UsarConfigDoBanco { get; set; }
        public bool ExcluirDigitoNossoNumero { get; }

        public BancoSICOOB()
        {
            this.NomeBanco = "Sistema de Cooperativas de Crédito do Brasil";
            this.CodigoBanco = "756";
        }
        

        private void GerarCampoLivre(CodigoBarrasDTO cb)
        {
            if (cb != null)
            {
                cb.CampoLivre = $@"{cb.AgenciaBeneficiario.Substring(0, 4)}{cb.Carteira.Substring(1,1)}
                                    {StringUtil.PreencherZeroEsquerda(cb.NossoNumero, 10)}{StringUtil.PreencherZeroEsquerda(cb.CodBeneficiario, 10)}0";
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
            cb.AgenciaBeneficiario = StringUtil.PreencherZeroEsquerda(cb.AgenciaBeneficiario, 4).Substring(0, 4);
            cb.Carteira = StringUtil.PreencherZeroEsquerda(cb.Carteira, 2);

            var contaStr = cb.NumeroConta;

            if (!string.IsNullOrWhiteSpace(contaStr))
            {
                contaStr = contaStr.Replace("-", "");
                cb.NumeroConta = StringUtil.PreencherZeroEsquerda(contaStr, 7);
            }
            if (!string.IsNullOrWhiteSpace(cb.NossoNumero))
            {
                cb.NossoNumero = StringUtil.PreencherZeroEsquerda(cb.NossoNumero, 7);
            }

            cb.CodBeneficiario = "0263613";

            //this.GerarCampoLivre(cb);

            string dVNosssoNumero = this.RetornaDVGrupo3197(cb.AgenciaBeneficiario + StringUtil.PreencherZeroEsquerda(cb.CodBeneficiario, 10) + cb.NossoNumero);

            string primeiroGrupo = this.RetornaGrupo("75691" + cb.AgenciaBeneficiario);
            string segundoGrupo = this.RetornaGrupo("01" + cb.CodBeneficiario + cb.NossoNumero.Substring(0, 1));
            string terceiroGrupo = this.RetornaGrupo(cb.NossoNumero.Substring(1, 6) + dVNosssoNumero + "001");

            cb.CampoLivre = primeiroGrupo + segundoGrupo + terceiroGrupo;

            string codigoBarraProvisorio = "7569" + StringUtil.PreencherZeroEsquerda(cb.FatorVencimento, 4) + StringUtil.PreencherZeroEsquerda(cb.ValorStr, 10);
            codigoBarraProvisorio += "1404201" + cb.CodBeneficiario + cb.NossoNumero + dVNosssoNumero + "001";

            string dvCodigoBarras = this.CalcularDVCodigoBarras(codigoBarraProvisorio);

            string codigoBarras = $@"7569{dvCodigoBarras}{StringUtil.PreencherZeroEsquerda(cb.FatorVencimento, 4)}{StringUtil.PreencherZeroEsquerda(cb.ValorStr, 10)}";

            codigoBarras += $@"1404201{cb.CodBeneficiario}{cb.NossoNumero}{dVNosssoNumero}001";

            cb.CodigoBarras = codigoBarras;

            cb.DigitoVerificadorCodigoBarras = dvCodigoBarras;

            return cb;

        }

        public CamposLinhaDigitalDTO SepararCamposVariaveisLinhaDigitavel(CodigoBarrasDTO cb)
        {
            //return null;

            var campoLivre = cb.CampoLivre;

            // Processamento Padrão
            //var campoLivreCampo1 = campoLivre.Substring(0, 5);
            var campo1 = $@"{cb.IdentificacaoBanco}{cb.CodigoMoeda}{cb.Carteira.Substring(1, 1)}{cb.AgenciaBeneficiario}"; //{cb.CampoLivre.Substring(9, 1)}
            var campo2 = $@"01{cb.CodBeneficiario}{cb.NossoNumero.Substring(0, 1)}"; //{campoLivre.Substring(16, 1)}
            var campo3 = $@"{cb.NossoNumero.Substring(1, 6)}{campoLivre.Substring(campoLivre.Length - 5, 1)}001"; // {campoLivre.Substring(28, 1)}
            var campo4 = $@"{cb.DigitoVerificadorCodigoBarras}";
            var campo5 = $@"{StringUtil.PreencherZeroEsquerda(cb.FatorVencimento, 4)}{cb.ValorStr}";

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

        private String RetornaGrupo ( String dados )
        {

            int soma = 0;
            //2 1
            for (int contador = dados.Length - 1; contador >= 0 ; contador--)
            {

                if (((dados.Length - 1) - contador) % 2 == 0)
                    soma += 2 * int.Parse(dados[contador].ToString());
                else
                    soma += 1 * int.Parse(dados[contador].ToString());

            }

            int dv = soma % 10;

            return dados + dv.ToString();

        }

        private String RetornaDVGrupo3197 (String dados)
        {

            int soma = 0;
            //3197
            for (int contador = 0; contador < dados.Length; contador ++)
            {

                if (contador % 4 == 0)
                    soma += 3 *  int.Parse(dados[contador].ToString());
                else if (contador % 4 == 1)
                    soma += 1 * int.Parse(dados[contador].ToString());
                else if (contador % 4 == 2)
                    soma += 9 * int.Parse(dados[contador].ToString());
                else
                    soma += 7 * int.Parse(dados[contador].ToString());

            }

            int dv = (((soma % 11) == 0) || ((soma % 11) == 1) ? 0 : (11 - (soma % 11)));

            return dv.ToString();

        }
    }
}

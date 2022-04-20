using System;
using GenericCrud.Util;
using COAD.COBRANCA.Bradesco.Model.DTO.Interfaces;
using COAD.SEGURANCA.Model;


namespace COAD.COBRANCA.Bradesco.Model.DTO
{
    public class CodigoBarrasDTO
    {
        public IBanco Banco { get; set; }
        public CodigoBarrasDTO()
        {

        }

        public CodigoBarrasDTO(ContaDTO conta)
        {
            if(conta != null && conta.BAN_ID != null)
            {
                this.IdentificacaoBanco = conta.BAN_ID;
                this.AgenciaBeneficiario = conta.CTA_AGENCIA;
                this.Carteira = conta.CTA_CARTEIRA_BOLETO;
                this.CodigoMoeda = "9";
                this.NumeroConta = conta.CTA_CONTA;
                this.ModalidadeCobranca = "00";
            }
        }

        // Campos Padrão
        public string IdentificacaoBanco { get; set; }
        public string CodigoMoeda { get; set; }
        public string DigitoVerificadorCodigoBarras { get; set; }
        public string FatorVencimento { get; set; }
        public DateTime? DataVencimento { get; set; }        
        public decimal? Valor { get; set; }
        
        // Campos Livre
        public string AgenciaBeneficiario { get; set; }
        public string ContaBeneficiario { get; set; }
        public string CodBeneficiario { get; set; }
        public string Carteira { get; set; }
        public string NossoNumero { get; set; }
        public string NumeroConta { get; set; }
        public string ModalidadeCobranca { get; set; }
        
        public string CampoLivre { get; set; }
        public string CodigoBarras { get; set; }
        
        public string ValorStr
        {
            get
            {

                if (Valor == null)
                    return null;

                var valorStr = StringUtil.FormatarDinheiro((decimal) Valor, false);
                valorStr = valorStr.Replace(".", "").Replace(",", "");
                valorStr = StringUtil.PreencherZeroEsquerda(valorStr, 10);
                return valorStr;

            }
        }        

        public override string ToString()
        {
            return CodigoBarras;
        }
    }
}
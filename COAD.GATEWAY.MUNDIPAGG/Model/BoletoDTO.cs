using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.GATEWAY.MUNDIPAGG.Model
{
    public class BoletoDTO
    {
        /// <summary>
        /// (DaysToAddInBoletoExpirationDate) Dias para o vencimento
        /// </summary>
        public int dias { get; set; }

        /// <summary>
        /// (AmountInCents) Valor da transação em centavos. R$ 1,00 = 100
        /// </summary>
        public long valor { get; set; }

        /// <summary>
        /// (BankNumber) Número do Banco
        /// </summary>
        public string banco { get; set; }

        /// <summary>
        /// (City) Cidade
        /// </summary>
        public string cidade { get; set; }

        /// <summary>
        /// (Complement) Complemento do endereço
        /// </summary>
        public string complemento { get; set; }

        /// <summary>
        /// (Number) Número
        /// </summary>
        public string numero { get; set; }

        /// <summary>
        /// (District) Bairro
        /// </summary>
        public string bairro { get; set; }

        /// <summary>
        /// (State) Estado
        /// </summary>
        public string UF { get; set; }

        /// <summary>
        /// (Street) Logradouro/Endereço
        /// </summary>
        public string endereco { get; set; }

        /// <summary>
        /// (ZipCode) CEP
        /// </summary>
        public string CEP { get; set; }

        /// <summary>
        /// (DocumentNumber) Número do documento no boleto. O conteúdo desse campo é utilizado somente para impressão no boleto
        /// </summary>
        public string numeroDocumento { get; set; }

        /// <summary>
        /// (Instructions) Instruções que serão impressas no boleto. Esse campo é utilizado para instruir o caixa do banco ao receber o pagamento do boleto.
        /// Podem ser registradas por exemplo cobranças de multa e juros.
        /// </summary>
        public string instrucoes { get; set; }

        /// <summary>
        /// (TransactionReference) Identificador da transação na sua base.
        /// <para>Permite que seja informado um valor que será utilizado para identificar a transação na MundiPagg.</para>
        /// <para>Caso não seja especificado, a MundiPagg criará automaticamente um valor para este campo.</para>
        /// <para>Você pode, por exemplo, especificar a chave da transação no seu sistema interno e utilizá-lo para localizar a transação no portal da MundiPagg.</para>
        /// </summary>
        public string transacaoInterna { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{
    public class CartaoCreditoDTO
    {

        public CartaoCreditoDTO()
        {

        }

        /// <summary>
        /// (AmountInCents) Valor da transação em centavos. R$ 1,00 = 100
        /// </summary>
        public int valor { get; set; }

        /// <summary>
        /// (City) Cidade
        /// </summary>
        public string cidade { get; set; }

        /// <summary>
        /// (Complement) Complemento do endereço
        /// </summary>
        public string complemento { get; set; }

        /// <summary>
        /// (District) Bairro
        /// </summary>
        public string bairro { get; set; }

        /// <summary>
        /// (Number) Número
        /// </summary>
        public string numero { get; set; }

        /// <summary>
        /// (State) Estado - Unidade Federativa
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
        /// (CreditCardBrand) Bandeira do cartão do cliente [0=Visa, 1=MasterCard, 2=Hipercard, 3=Amex, 4=Diners, 5=Elo, 6=Aura, 7=Discover, 8=CasaShow, 9=HugCard, 10=AndarAki, 11=Havan, 12=LeaderCard]
        /// </summary>
        //        public Nullable<CreditCardBrandEnum> bandeira { get; set; }
        public string bandeira { get; set; }

        /// <summary>
        /// (CreditCardNumber) Número do cartão do cliente. Informar apenas números.
        /// </summary>
        public string numeroCartao { get; set; }

        /// <summary>
        /// (ExpMonth) Mês de expiração do cartão
        /// </summary>
        public int mesExpiracao { get; set; }

        /// <summary>
        /// (ExpYear) Ano de expiração do cartão
        /// </summary>
        public int anoExpiracao { get; set; }

        /// <summary>
        /// (HolderName) Nome do portador do cartão
        /// </summary>
        public string portador { get; set; }

        public string email { get; set; }

        /// <summary>
        /// (SecurityCode) Código de segurança do cartão
        /// </summary>
        public string codigoSeguranca { get; set; }

        /// <summary>
        /// (CreditCardOperation) Tipo da transação a ser realizada: 
        /// <para>(0)=Realiza a pré-autorização do valor no cartão do cliente. Você deve realizar a operação de captura /Capture para a confirmação da transação;</para>
        /// <para>(1)=Realiza a autorização seguida de captura autormaticamente na adquirente. Não necessita que você realize a transação de captura para a confirmação da transação;</para>
        /// (2)=Realiza a pré-autorização do valor no cartão do cliente. A MundiPagg será responsável por realizar a operação de captura (Capture) para a confirmação da transação. A loja deverá ter uma url configurada na MundiPagg para ser notificada do sucesso ou falha da operação de captura.
        /// </summary>
        public Nullable<CreditCardOperationEnum> tipoOperacao { get; set; }

        /// <summary>
        /// (InstallmentCount) Número de Parcelas
        /// </summary>
        public int numeroParcelas { get; set; }

        /// <summary>
        /// (PaymentMethodCode) Meio de pagamento que deve ser utilizado para a transação: 3=RedecardKomerci 5=Cielo 18=GetNetSitef 20=Stone 23=Elavon 32=eRede
        /// </summary>
        public int meioDePagamento { get; set; }

        /// <summary>
        /// (SoftDescriptorText) Texto da fatura do cartão
        /// </summary>
        public string textoCartao { get; set; }

        /// <summary>
        /// (TransactionReference) Identificador da transação na sua base.
        /// <para>Permite que seja informado um valor que será utilizado para identificar a transação na MundiPagg.</para>
        /// <para>Caso não seja especificado, a MundiPagg criará automaticamente um valor para este campo.</para>
        /// <para>Você pode, por exemplo, especificar a chave da transação no seu sistema interno e utilizá-lo para localizar a transação no portal da MundiPagg.</para>
        /// </summary>
        public string transacaoInterna { get; set; }

        /// <summary>
        /// (DateToStartBilling) Data do início da recorrência
        /// </summary>
        public Nullable<DateTime> dataInicioRecorrencia { get; set; }
        /// <summary>
        /// (Frequency) Frequência: [1]=Semanal [2]=Mensal [3]=Anual [4]=Diária
        /// </summary>
        public FrequencyEnum frequenciaDaRecorrencia { get; set; }
        /// <summary>
        /// (Interval) Intervalo entre a frequência (Nesse caso, se a frequência for mensal, e você enviar 2 no intervalo, será feita uma cobrança a cada dois meses.)
        /// </summary>
        public int intervaloDaRecorrencia { get; set; }
        /// <summary>
        /// (Recurrences) Número de recorrências que serão criadas; caso seja informado 0, será criada uma recorrência infinita.
        /// </summary>
        public int numeroDeRecorrencias { get; set; }

        /// <summary>
        /// (formapgto) formapgto: [1]=Boleto [2]=Cartão
        public int formapgto { get; set; }
        /// (formapgto) formapgto: [0]=Recorrente [1]=Normal
        public int recorrente { get; set; }

        public string numeroPedido { get; set; }
        public string cmpDescricao { get; set; }
        public int cmpQuantidade { get; set; }
        public long cmpVlrUnit { get; set; }
        public long cmpVlrTotal { get; set; }
        public decimal? cmpVlrUnittela { get; set; }
        public decimal? cmpVlrTotaltela { get; set; }
        public decimal? valortela { get; set; }
        public decimal? valorParcelas { get; set; }


        /// <summary>
        /// (Endpoint) sandbox - ambiente de testes e HOMOLOGAÇÃO na MundiPagg.
        /// </summary>
        public string homologacao = "https://sandbox.mundipaggone.com";

        /// <summary>
        /// (Endpoint) transactionv2 - ambiente de PRODUÇÃO na MundiPagg.
        /// </summary>
        public string producao = "https://transactionv2.mundipaggone.com";

        /// <summary>
        /// Seu ambiente de trabalho na MundiPagg. [P]rodução ou [H]omologação. O Padrão é "H".
        /// </summary>
        public string ambiente = "H";

        /// <summary>
        /// Url do ambiente escolhido.
        /// </summary>
        public string urlAmbiente;

        /// <summary>
        /// (merchantKey) Sua chave de acesso à MundiPagg. Ela é a autenticação necessária para todas as requisições enviadas aos nossos endpoints.
        /// </summary>
        public string merchantKey = "374b6325-9cbe-4f70-9476-65bd0dd8020b";
        //"380e7bd3-76c4-424e-92be-4d234b5ea371";          

        public string chave_secreta = "sk_test_MemOXpjhD9F2vO7o";

        public string chave_publica = "pk_test_4morLGaiK8CnjKbJ";

    }
}

using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PAGAMENTO.API.Service.GatewayMundiPagg
{
    /// <summary>
    /// ALT: 20/06/2016
    /// Este objeto cria as transações com cartões de crédito [Visa, MasterCard, Hipercard, Amex, Diners, Elo, Aura, Discover, CasaShow, HugCard]
    /// </summary>
    public class CartaoDeCredito
    {
        /// <summary>
        /// (AmountInCents) Valor da transação em centavos. R$ 1,00 = 100
        /// </summary>
        public long valor { get; set; }

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
        public int bandeira { get; set; }

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
        public int tipoOperacao { get; set; }

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
        public DateTime dataInicioRecorrencia { get; set; }

        /// <summary>
        /// (Frequency) Frequência: [0]=Diária [1]=Semanal [2]=Mensal [3]=Anual
        /// </summary>
        public int frequenciaDaRecorrencia { get; set; }

        /// <summary>
        /// (Interval) Intervalo entre a frequência (Nesse caso, se a frequência for mensal, e você enviar 2 no intervalo, será feita uma cobrança a cada dois meses.)
        /// </summary>
        public int intervaloDaRecorrencia { get; set; }

        /// <summary>
        /// (Recurrences) Número de recorrências que serão criadas; caso seja informado 0, será criada uma recorrência infinita.
        /// </summary>
        public int numeroDeRecorrencias { get; set; }

        /// <summary>
        /// Criar a transação de cartão de crédito.
        /// </summary>
        /// <returns></returns>
        public CreditCardTransaction Criar()
        {
            CreditCardBrandEnum[] Cartao = { CreditCardBrandEnum.Visa, 
                                             CreditCardBrandEnum.Mastercard, 
                                             CreditCardBrandEnum.Hipercard, 
                                             CreditCardBrandEnum.Amex, 
                                             CreditCardBrandEnum.Diners,
                                             CreditCardBrandEnum.Elo,
                                             CreditCardBrandEnum.Aura,
                                             CreditCardBrandEnum.Discover,
                                             CreditCardBrandEnum.CasaShow,
                                             CreditCardBrandEnum.HugCard, 
                                             CreditCardBrandEnum.AndarAki,
                                             CreditCardBrandEnum.Havan,
                                             CreditCardBrandEnum.LeaderCard
                                           };
            CreditCardOperationEnum[] Operacao = { CreditCardOperationEnum.AuthOnly, CreditCardOperationEnum.AuthAndCapture, CreditCardOperationEnum.AuthAndCaptureWithDelay };

            FrequencyEnum[] Frequencia = { FrequencyEnum.Daily, FrequencyEnum.Weekly, FrequencyEnum.Monthly, FrequencyEnum.Yearly };

            // Cria a transação de cartão de crédito com recorrência.
            if (this.dataInicioRecorrencia != null)
            {
                var creditCardTransaction = new CreditCardTransaction()
                {
                    AmountInCents = this.valor,
                    CreditCard = new CreditCard()
                    {
                        BillingAddress = new BillingAddress()
                        {
                            City = this.cidade,
                            Complement = this.complemento,
                            Country = CountryEnum.Brazil.ToString(),
                            District = this.bairro,
                            Number = this.numero,
                            State = this.UF,
                            Street = this.endereco,
                            ZipCode = this.CEP
                        },
                        CreditCardBrand = Cartao[this.bandeira],
                        CreditCardNumber = this.numeroCartao,
                        ExpMonth = this.mesExpiracao,
                        ExpYear = this.anoExpiracao,
                        HolderName = this.portador,
                        SecurityCode = this.codigoSeguranca
                    },
                    CreditCardOperation = Operacao[this.tipoOperacao],
                    InstallmentCount = this.numeroParcelas,
                    Options = new CreditCardTransactionOptions()
                    {
                        CurrencyIso = CurrencyIsoEnum.BRL,
                        PaymentMethodCode = this.meioDePagamento,
                        SoftDescriptorText = this.textoCartao
                    },
                    Recurrency = new Recurrency()
                    {
                        DateToStartBilling = this.dataInicioRecorrencia,
                        Frequency = Frequencia[this.frequenciaDaRecorrencia],
                        Interval = this.intervaloDaRecorrencia,
                        Recurrences = this.numeroDeRecorrencias
                    },
                    TransactionReference = this.transacaoInterna
                };

                return creditCardTransaction;
            }
            else 
            {
                var creditCardTransaction = new CreditCardTransaction()
                {
                    AmountInCents = this.valor,
                    CreditCard = new CreditCard()
                    {
                        BillingAddress = new BillingAddress()
                        {
                            City = this.cidade,
                            Complement = this.complemento,
                            Country = CountryEnum.Brazil.ToString(),
                            District = this.bairro,
                            Number = this.numero,
                            State = this.UF,
                            Street = this.endereco,
                            ZipCode = this.CEP
                        },
                        CreditCardBrand = Cartao[this.bandeira],
                        CreditCardNumber = this.numeroCartao,
                        ExpMonth = this.mesExpiracao,
                        ExpYear = this.anoExpiracao,
                        HolderName = this.portador,
                        SecurityCode = this.codigoSeguranca
                    },
                    CreditCardOperation = Operacao[this.tipoOperacao],
                    InstallmentCount = this.numeroParcelas,
                    Options = new CreditCardTransactionOptions()
                    {
                        CurrencyIso = CurrencyIsoEnum.BRL,
                        PaymentMethodCode = this.meioDePagamento,
                        SoftDescriptorText = this.textoCartao
                    },
                    TransactionReference = this.transacaoInterna
                };

                return creditCardTransaction;
            }
        }
    }
}

using COAD.GATEWAY.MUNDIPAGG.Model;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.GATEWAY.MUNDIPAGG.Service
{
     public class CartaoCreditoSRV
    {
        /// <summary>
        /// Criar a transação de cartão de crédito.
        /// </summary>
        /// <returns></returns>
        public CreditCardTransaction CriarTransacaoCartao(CartaoCreditoDTO cartao)
        {
            // Cria a transação de cartão de crédito com recorrência.
            if (cartao.dataInicioRecorrencia != null)
            {
                var creditCardTransaction = new CreditCardTransaction()
                {
                    AmountInCents = cartao.valor,
                    CreditCard = new CreditCard()
                    {
                        BillingAddress = new BillingAddress()
                        {
                            City = cartao.cidade,
                            Complement = cartao.complemento,
                            Country = CountryEnum.Brazil.ToString(),
                            District = cartao.bairro,
                            Number = cartao.numero,
                            State = cartao.UF,
                            Street = cartao.endereco,
                            ZipCode = cartao.CEP
                        },
                        CreditCardBrand = cartao.bandeira,
                        CreditCardNumber = cartao.numeroCartao,
                        ExpMonth = cartao.mesExpiracao,
                        ExpYear = cartao.anoExpiracao,
                        HolderName = cartao.portador,
                        SecurityCode = cartao.codigoSeguranca
                    },
                    CreditCardOperation = cartao.tipoOperacao,
                    InstallmentCount = cartao.numeroParcelas,
                    Options = new CreditCardTransactionOptions()
                    {
                        CurrencyIso = CurrencyIsoEnum.BRL,
                        PaymentMethodCode = cartao.meioDePagamento,
                        SoftDescriptorText = cartao.textoCartao
                    },
                    Recurrency = new Recurrency()
                    {
                        DateToStartBilling = cartao.dataInicioRecorrencia,
                        Frequency = cartao.frequenciaDaRecorrencia,
                        Interval = cartao.intervaloDaRecorrencia,
                        Recurrences = cartao.numeroDeRecorrencias
                    },
                    TransactionReference = cartao.transacaoInterna
                };

                return creditCardTransaction;
            }
            else 
            {
                var creditCardTransaction = new CreditCardTransaction()
                {
                    AmountInCents = cartao.valor,
                    CreditCard = new CreditCard()
                    {
                        BillingAddress = new BillingAddress()
                        {
                            City = cartao.cidade,
                            Complement = cartao.complemento,
                            Country = CountryEnum.Brazil.ToString(),
                            District = cartao.bairro,
                            Number = cartao.numero,
                            State = cartao.UF,
                            Street = cartao.endereco,
                            ZipCode = cartao.CEP
                        },
                        CreditCardBrand = cartao.bandeira,
                        CreditCardNumber = cartao.numeroCartao,
                        ExpMonth = cartao.mesExpiracao,
                        ExpYear = cartao.anoExpiracao,
                        HolderName = cartao.portador,
                        SecurityCode = cartao.codigoSeguranca
                    },
                    CreditCardOperation = cartao.tipoOperacao,
                    InstallmentCount = cartao.numeroParcelas,
                    Options = new CreditCardTransactionOptions()
                    {
                        CurrencyIso = CurrencyIsoEnum.BRL,
                        PaymentMethodCode = cartao.meioDePagamento,
                        SoftDescriptorText = cartao.textoCartao
                    },
                    TransactionReference = cartao.transacaoInterna
                };

                return creditCardTransaction;
            }
        }
        /// <summary>
        ///BoletoTransactionCollection[AmountInCents] -- Valor do boleto em centavos. R$ 1,00 = 100
        ///BoletoTransactionCollection[BankNumber] -- Número do banco
        ///BoletoTransactionCollection[DocumentNumber] -- Número do documento no boleto
        ///BoletoTransactionCollection[Instructions] -- Instruções que serão impressas no boleto. Esse campo é utilizado para instruir o caixa do banco ao receber o pagamento do boleto. Podem ser registradas por exemplo cobranças de multa e juros
        ///BoletoTransactionCollection[Options[TransactionDateInMerchant] -- Identificador do pedido na sua base
        ///BoletoTransactionCollectionOptions[DaysToAddInBoletoExpirationDate] -- Dias para o vencimento do boleto
        ///Order[OrderReference] -- Identificador do pedido na sua base
        /// </summary>
        /// <param name="CartaoCreditoDTO"></param>
        /// <returns></returns>
        public BoletoTransaction CriarTransacaoBoleto(CartaoCreditoDTO cartao)
        {

            var _boleto = new BoletoTransaction()
            {
                AmountInCents = cartao.valor,
                BankNumber = "341",
                DocumentNumber = cartao.numeroPedido,
                TransactionDateInMerchant = DateTime.Now,
                TransactionReference = cartao.numeroPedido,
                Instructions = "Receber em toda rede bancária ate o vencimento",


                BillingAddress = new BillingAddress()
                {
                    City = cartao.cidade,
                    Complement = cartao.complemento,
                    Country = CountryEnum.Brazil.ToString(),
                    District = cartao.bairro,
                    Number = cartao.numero,
                    State = cartao.UF,
                    Street = cartao.endereco,
                    ZipCode = cartao.CEP,
                    
                },

                Options = new BoletoTransactionOptions()
                {
                    CurrencyIso = CurrencyIsoEnum.BRL,
                    DaysToAddInBoletoExpirationDate = 2,
                    IsNotificationEnabled = true,
                    NotificationUrl = "",
                }
            };

            return _boleto;
            
        }
    }
}

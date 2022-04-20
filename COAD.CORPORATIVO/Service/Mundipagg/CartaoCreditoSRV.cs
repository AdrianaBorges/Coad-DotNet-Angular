using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using MundiAPI.PCL;
using MundiAPI.PCL.Models;

/*
using GatewayApiClient;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
*/

namespace COAD.CORPORATIVO.Service.Mundipagg
{
    public class CartaoCreditoSRV
    {

        /// <summary>
        /// Criar a transação de cartão de crédito.
        /// </summary>
        /// <returns></returns>
        public bool CriarTransacaoCartao(CartaoCreditoDTO cartao)
        {

            string basicAuthUserName = cartao.chave_secreta; // The username to use with basic authentication
            string basicAuthPassword = ""; // The password to use with basic authentication

            MundiAPIClient cliente = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

            CreateAddressRequest endereco = new CreateAddressRequest()
            {
                Street = cartao.endereco,
                Complement = cartao.complemento,
                Number = cartao.numero,
                Neighborhood = cartao.bairro,
                City = cartao.cidade,
                State = cartao.UF,
                Country = "BR",
                ZipCode = cartao.CEP
            };

            CreateCustomerRequest customer = new CreateCustomerRequest()
            {
                Name = cartao.portador,
                Email = cartao.email,
                Address = endereco
            };

            CreateCardRequest card = new CreateCardRequest()
            {

                HolderName = cartao.portador,
                Number = cartao.numeroCartao,
                BillingAddress = endereco,
                Brand = cartao.bandeira,
                Cvv = cartao.codigoSeguranca,
                ExpMonth = cartao.mesExpiracao,
                ExpYear = cartao.anoExpiracao
                
            };

            CreateChargeRequest request = new CreateChargeRequest()
            {
                Amount = cartao.valor,
                Customer = customer,
                DueAt = DateTime.Now.AddDays(7),
                Payment = new CreatePaymentRequest()
                {
                    PaymentMethod = "credit_card",
                    CreditCard = new CreateCreditCardPaymentRequest()
                    {

                        Card = card
                        
                    }

                }

            };

            GetChargeResponse response = cliente.Charges.CreateCharge(request);

            if (response.Code != null)
                return true;

            return false;


            /*
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

                cartao.urlAmbiente = (cartao.ambiente == "P") ? cartao.producao : cartao.homologacao;

                IGatewayServiceClient client = new GatewayServiceClient(Guid.Parse(cartao.merchantKey), new Uri(cartao.urlAmbiente));

                var httpResponse = client.Sale.Create(creditCardTransaction);

                return (httpResponse.HttpStatusCode == HttpStatusCode.Created);


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

                cartao.urlAmbiente = (cartao.ambiente == "P") ? cartao.producao : cartao.homologacao;

                IGatewayServiceClient client = new GatewayServiceClient(Guid.Parse(cartao.merchantKey), new Uri(cartao.urlAmbiente));


                var httpResponse = client.Sale.Create(creditCardTransaction);


                return (httpResponse.HttpStatusCode == HttpStatusCode.Created);

            }
            */
        }

        public bool TesteCartaoCredito ()
        {

            CartaoCreditoDTO cartao = new CartaoCreditoDTO
            {
                valor = 7000,
                cidade = "Rio",
                complemento = "Próximo ao Prezunic",
                bairro = "Pechincha",
                numero = "455",
                UF = "RJ",
                endereco = "Estrada do Tindiba",
                CEP = "22740360",
                bandeira = "Mastercard",
                numeroCartao = "5237 8820 9040 1907",
                mesExpiracao = 8,
                anoExpiracao = 2019,
                portador = "COAD",
                codigoSeguranca = "983",
                numeroParcelas = 1,
                textoCartao = "COAD",
                formapgto = 1,
                numeroPedido = "10101010-1010-1010-1010-101010101010",
                cmpVlrUnit = 7000,
                cmpVlrTotal = 7000,
                cmpVlrUnittela = 7000,
                cmpVlrTotaltela = 7000,
                valortela = 7000,
                valorParcelas = 7000
            };


            return CriarTransacaoCartao(cartao);


        }


    }
}

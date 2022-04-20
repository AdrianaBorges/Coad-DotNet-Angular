using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Net;

using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;

using MundiAPI.PCL;
using MundiAPI.PCL.Models;

/*
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using GatewayApiClient;
*/


namespace COAD.CORPORATIVO.Service.Mundipagg
{


    public class BoletoSRV 
    {

        public BoletoSRV()
        {


        }

        /*
        public static Task<HttpResponseMessage> RunAsync<HttpResponseMessage>()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("asad");



            HttpResponseMessage httpResponse;
            httpResponse.Headers.Add("Authorization", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("Basic " + Encoding.UTF8.GetBytes("sk_test_tra6ezsW3BtPPXQa:").ToString())));
            httpResponse.Headers.Add("Content-Type", "application/json");

            httpClient.PostAsync()
            return null;
        }
        */

        public bool EnviarBoleto(BoletoDTO boleto)
        {

            if (boleto != null)
            {

                string basicAuthUserName = boleto.chave_secreta; // The username to use with basic authentication
                string basicAuthPassword = ""; // The password to use with basic authentication

                MundiAPIClient cliente = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                CreateCustomerRequest customer = new CreateCustomerRequest()
                {
                    Name = boleto.nome,
                    Email = boleto.email,
                    //Document = boleto.numeroDocumento,
                    Address = new CreateAddressRequest()
                    {
                        Street = boleto.endereco,
                        Complement = boleto.complemento,
                        Number = boleto.numero,
                        Neighborhood = boleto.bairro,
                        City = boleto.cidade,
                        State = boleto.UF,
                        Country = "BR",
                        ZipCode = boleto.CEP
                    }
                };

                Dictionary<string, string> metaData = new Dictionary<string, string>();
                metaData.Add("Instructions", boleto.instrucoes);

                CreateChargeRequest request = new CreateChargeRequest()
                {
                    Amount = boleto.valor,
                    Customer = customer,
                    DueAt = DateTime.Now.AddDays(7),
                    Payment = new CreatePaymentRequest()
                    {
                        PaymentMethod = "boleto",
                        Boleto = new CreateBoletoPaymentRequest()
                        {
                            Bank = boleto.banco,
                            DueAt = DateTime.Now.AddDays(7)
                        }

                    },
                    Metadata = metaData

                };

                GetChargeResponse response = cliente.Charges.CreateCharge(request);

                if ( response.Code != null )
                    return true;

                /*
                // Configuration parameters and credentials
                string basicAuthUserName = "sk_test_MemOXpjhD9F2vO7o"; // The username to use with basic authentication
                string basicAuthPassword = ""; // The password to use with basic authentication

                 MundiAPIClient client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                MundiPaggClientSDK.Service.BoletoTransaction boletoTransaction = new MundiPaggClientSDK.Service.BoletoTransaction()
                {
                    AmountInCents = boleto.valor,
                    BankNumber = boleto.banco, 
                    BillingAddress = new BillingAddress()
                    {
                        City = boleto.cidade,
                        Complement = boleto.complemento,
                        Country = CountryEnum.Brazil.ToString(),
                        Number = boleto.numero,
                        District = boleto.bairro,
                        State = boleto.UF,
                        Street = boleto.endereco,
                        ZipCode = boleto.CEP
                    },
                    NossoNumero = boleto.numeroDocumento,
                    Instructions = boleto.instrucoes,
                    DaysToAddInBoletoExpirationDate = 4
                    ExtensionData
                    Options = new BoletoTransactionOptions()
                    {
                        CurrencyIso = CurrencyIsoEnum.BRL,
                        DaysToAddInBoletoExpirationDate = boleto.dias
                    },
                    TransactionReference = boleto.transacaoInterna 
                };

                MundiPaggClientSDK.Service.CreateOrderRequest orderRequest = new MundiPaggClientSDK.Service.CreateOrderRequest
                {
                    AmountInCents = boleto.valor,
                    AmountInCentsToConsiderPaid = boleto.valor,
                    MerchantKey = Guid.Parse ( boleto.merchantKey ),
                    OrderReference = boleto.numeroDocumento,
                    CurrencyIsoEnum = MundiPaggClientSDK.Service.CurrencyIsoEnum.BRL
                };

                orderRequest.BoletoTransactionCollection.Add(boletoTransaction);

                orderRequest.AmountInCents = boleto.valor;

                MundiPaggClientSDK.EnvironmentEnum sand = MundiPaggClientSDK.EnvironmentEnum.Sandbox
                {
                    
                };

                MundiPaggClientSDK.EnvironmentEnum.

                MundiPaggClientSDK.GatewayClient gateway = new MundiPaggClientSDK.GatewayClient();


                HttpWebRequest http = WebRequest.Create("https://api.mundipagg.com/core/v1/orders") as HttpWebRequest;
                http.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("pk_test_4morLGaiK8CnjKbJ")); //Autenticação
                http.Method = "POST";
                http.ContentType = "application/json";

                MundiPaggClientSDK.Service.BoletoTransaction.EnvironmentEnum environment = new MundiPaggClientSDK.EnvironmentEnum
                MundiPaggClientSDK.GatewayClient gatewayClient = new MundiPaggClientSDK.GatewayClient((environmnent);

                MundiAPIClient client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);


                boleto.urlAmbiente = (boleto.ambiente == "P") ? boleto.producao : boleto.homologacao;

                IGatewayServiceClient client = new GatewayServiceClient(Guid.Parse(boleto.merchantKey), new Uri(boleto.urlAmbiente));


                var httpResponse = client.Sale.Create(boletoTransaction);


                return (httpResponse.HttpStatusCode == HttpStatusCode.Created);

    */

                /*
                BoletoTransaction boletoTransaction = new BoletoTransaction()
                {
                    AmountInCents = boleto.valor,
                    BankNumber = boleto.banco,
                    BillingAddress = new BillingAddress()
                    {
                        City = boleto.cidade,
                        Complement = boleto.complemento,
                        Country = CountryEnum.Brazil.ToString(),
                        Number = boleto.numero,
                        District = boleto.bairro,
                        State = boleto.UF,
                        Street = boleto.endereco,
                        ZipCode = boleto.CEP
                    },
                    DocumentNumber = boleto.numeroDocumento,
                    Instructions = boleto.instrucoes,
                    TransactionDateInMerchant = DateTime.Now,

                    Options = new BoletoTransactionOptions()
                    {
                        CurrencyIso = CurrencyIsoEnum.BRL,
                        DaysToAddInBoletoExpirationDate = boleto.dias
                    },
                    TransactionReference = boleto.transacaoInterna
                };

                boleto.urlAmbiente = (boleto.ambiente == "P") ? boleto.producao : boleto.homologacao;

                IGatewayServiceClient client = new GatewayServiceClient(Guid.Parse(boleto.merchantKey), new Uri(boleto.urlAmbiente));

                var httpResponse = client.Sale.Create(boletoTransaction);

                return (httpResponse.HttpStatusCode == HttpStatusCode.Created);
                */

            }

            return false;

        }

        public bool TesteEnviarBoleto ()
        {

            BoletoDTO boleto = new BoletoDTO();

            boleto.dias = 7;
            boleto.valor = 7000;
            boleto.banco = "341";
            boleto.nome = "Teste";
            boleto.cidade = "Rio de Janeiro";
            boleto.complemento = "";
            boleto.numero = "455";
            boleto.bairro = "Pechincha";
            boleto.UF = "RJ";
            boleto.endereco = "Estrada do Tindiba";
            boleto.CEP = "22740360";
            boleto.numeroDocumento = "476346I9";
            boleto.instrucoes = "Receber mesmo após o vencimento";
            boleto.transacaoInterna = "não";
            boleto.email = "ti@coad.com.br";


            return EnviarBoleto(boleto);

        }


    }
}

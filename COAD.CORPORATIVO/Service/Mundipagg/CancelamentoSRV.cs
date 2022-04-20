using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

using MundiAPI.PCL;
using MundiAPI.PCL.Models;

/*
using GatewayApiClient;
using GatewayApiClient.DataContracts;
using GatewayApiClient.EnumTypes;
using GatewayApiClient.Utility;
*/

namespace COAD.CORPORATIVO.Service.Mundipagg
{
    public class CancelamentoSRV
    {

        public bool Executar(CancelamentoDTO canc)
        {

            string basicAuthUserName = canc.chave_secreta; // The username to use with basic authentication
            string basicAuthPassword = ""; // The password to use with basic authentication

            MundiAPIClient cliente = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

            GetChargeResponse response;

            try
            {

                response = cliente.Charges.CancelCharge(canc.chaveDoPedido);

            }
            catch
            {

                response = new GetChargeResponse();

            }
            

            if (response.Code != null)
                return true;

            return false;

            /*
            canc.urlAmbiente = (canc.ambiente == "P") ? canc.producao : canc.homologacao;

            Guid merchantKey = Guid.Parse(canc.merchantKey);

            // Chave do pedido
            Guid orderKey = Guid.Parse(canc.chaveDoPedido);

            // Cria o cliente para cancelar as transações.
            IGatewayServiceClient client = new GatewayServiceClient(merchantKey, new Uri(canc.urlAmbiente));

            // Transação específica que será cancelada.
            if (!String.IsNullOrWhiteSpace(canc.chaveTransacaoCartao))
            {
                var transactionToCancel = new ManageCreditCardTransaction()
                {
                    AmountInCents = canc.valorCancelar,
                    TransactionKey = Guid.Parse(canc.chaveTransacaoCartao)
                };

                // Cancela transação de cartão de crédito do pedido.
                var httpResponse = client.Sale.Manage(ManageOperationEnum.Cancel, orderKey, transactionToCancel);

                canc.retornouOK = (httpResponse.HttpStatusCode == HttpStatusCode.OK
                        && httpResponse.Response.CreditCardTransactionResultCollection.Any()
                        && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true));

                return httpResponse;
            }
            else
            {
                // Cancela toda transação de cartão de crédito do pedido.
                var httpResponse = client.Sale.Manage(ManageOperationEnum.Cancel, orderKey);

                canc.retornouOK = ( httpResponse.HttpStatusCode == HttpStatusCode.OK );
//                        && httpResponse.Response.CreditCardTransactionResultCollection.Any()
//                        && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true));


                return httpResponse;
            }
            */
        }

        public bool TesteCancelamento ( string chavePedidoGet, string chaveTransacaoGet )
        {

            CancelamentoDTO cancelamento = new CancelamentoDTO
            {
                ambiente = "H",
                chaveDoPedido = (chavePedidoGet != null ? chavePedidoGet : "10101010-1010-1010-1010-101010101010"),
                chaveTransacaoCartao = (chaveTransacaoGet != null ? chaveTransacaoGet : null),
                valorCancelar = 7000
            };
            
            return Executar(cancelamento);

        }


    }
}


using COAD.GATEWAY.MUNDIPAGG.Model;
using GatewayApiClient;
using GatewayApiClient.DataContracts;
using GatewayApiClient.EnumTypes;
using GatewayApiClient.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COAD.GATEWAY.MUNDIPAGG.Service
{
    /// <summary>
    /// ALT: 20/06/2016
    /// Este objeto cria uma requisição de Cancelamento de Transação.
    /// <para>...</para>
    /// <para>Cancelamentos só podem ser efetuados para transações de cartões de crédito.</para>
    /// <para>Boletos e débito online não podem ser cancelados pela API da MundiPagg.</para>
    /// <para>Para esses casos, a sua área financeira deve retornar os fundos do consumidor final via depósito bancário.</para>
    /// <para>...</para>
    /// <para>Bandeiras que permitem cancelamento: Cielo / Rede / Stone / Elavon / GetNet</para>
    /// </summary>
    public class CancelamentoSRV
    {
        public HttpResponse Executar(CancelamentoDTO canc)
        {
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

                canc.retornouOK = (httpResponse.HttpStatusCode == HttpStatusCode.OK
                        && httpResponse.Response.CreditCardTransactionResultCollection.Any()
                        && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true));

                return httpResponse;
            }
        }
    }
}

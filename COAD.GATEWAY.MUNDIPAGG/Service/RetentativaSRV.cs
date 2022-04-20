using COAD.GATEWAY.MUNDIPAGG.Model;
using GatewayApiClient;
using GatewayApiClient.DataContracts;
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
    /// Este objeto cria uma requisição de retentativas de transação.
    /// </summary>
    public class RetentativaSRV
    {

        /// <summary>
        /// Executar a reexecutar a tentativa de transação parametrizada. Retorna os resultados da operação.
        /// </summary>
        /// <returns></returns>
        public HttpResponse Executar(RetentativaDTO retentativa)
        {
            retentativa.urlAmbiente = (retentativa.ambiente == "P") ? retentativa.producao : retentativa.homologacao;

            Guid merchantKey = Guid.Parse(retentativa.merchantKey);

            // Chave do pedido
            Guid orderKey = Guid.Parse(retentativa.chaveDoPedido);

            // Cria o cliente para retentar as transações.
            IGatewayServiceClient client = new GatewayServiceClient(merchantKey, new Uri(retentativa.urlAmbiente));

            // Transação específica que será retentada.
            if (!String.IsNullOrWhiteSpace(retentativa.chaveTransacaoCartao))
            {
                RetrySaleCreditCardTransaction transactionToRetry = new RetrySaleCreditCardTransaction()
                {
                    SecurityCode = retentativa.codigoSeguranca,
                    TransactionKey = Guid.Parse(retentativa.chaveTransacaoCartao),
                };

                var httpResponse = client.Sale.Retry(orderKey, transactionToRetry);

                retentativa.retornouOK = (httpResponse.HttpStatusCode == HttpStatusCode.OK
                        && httpResponse.Response.CreditCardTransactionResultCollection.Any()
                        && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true));

                return httpResponse;
            }
            else 
            {
                var httpResponse = client.Sale.Retry(orderKey);

                retentativa.retornouOK = (httpResponse.HttpStatusCode == HttpStatusCode.OK
                        && httpResponse.Response.CreditCardTransactionResultCollection.Any()
                        && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true));

                return httpResponse;
            }
        }
    }
}

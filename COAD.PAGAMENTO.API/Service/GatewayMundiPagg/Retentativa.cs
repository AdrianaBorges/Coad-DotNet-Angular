using GatewayApiClient;
using GatewayApiClient.DataContracts;
using GatewayApiClient.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PAGAMENTO.API.Service.GatewayMundiPagg
{
    /// <summary>
    /// ALT: 20/06/2016
    /// Este objeto cria uma requisição de retentativas de transação.
    /// </summary>
    public class Retentativa
    {
        /// <summary>
        /// (Endpoint) sandbox - ambiente de testes e HOMOLOGAÇÃO na MundiPagg.
        /// </summary>
        private string homologacao = "https://sandbox.mundipaggone.com";

        /// <summary>
        /// (Endpoint) transactionv2 - ambiente de PRODUÇÃO na MundiPagg.
        /// </summary>
        private string producao = "https://transactionv2.mundipaggone.com";

        /// <summary>
        /// Seu ambiente de trabalho na MundiPagg. [P]rodução ou [H]omologação. O Padrão é "H".
        /// </summary>
        public string ambiente = "H";

        /// <summary>
        /// Url do ambiente escolhido.
        /// </summary>
        private string urlAmbiente;

        /// <summary>
        /// (merchantKey) Sua chave de acesso à MundiPagg. Ela é a autenticação necessária para todas as requisições enviadas aos nossos endpoints.
        /// </summary>
        public string merchantKey = "85328786-8BA6-420F-9948-5352F5A183EB";

        /// <summary>
        /// (orderKey) Chave do pedido gerada pela MundiPagg para a requisição.
        /// </summary>
        public string chaveDoPedido { get; set; }

        /// <summary>
        /// (TransactionKey) Chave da transação com cartões de crédito gerada pela MundiPagg para a requisição.
        /// </summary>
        public string chaveTransacaoCartao { get; set; }

        /// <summary>
        /// (SecurityCode) Codigo de segurança.
        /// </summary>
        public string codigoSeguranca { get; set; }

        /// <summary>
        /// Resultado da operação.
        /// </summary>
        public bool retornouOK { get; private set; }

        /// <summary>
        /// Executar a reexecutar a tentativa de transação parametrizada. Retorna os resultados da operação.
        /// </summary>
        /// <returns></returns>
        public HttpResponse Executar()
        {
            this.urlAmbiente = (this.ambiente == "P") ? this.producao : this.homologacao;

            Guid merchantKey = Guid.Parse(this.merchantKey);

            // Chave do pedido
            Guid orderKey = Guid.Parse(this.chaveDoPedido);

            // Cria o cliente para retentar as transações.
            IGatewayServiceClient client = new GatewayServiceClient(merchantKey, new Uri(this.urlAmbiente));

            // Transação específica que será retentada.
            if (!String.IsNullOrWhiteSpace(this.chaveTransacaoCartao))
            {
                RetrySaleCreditCardTransaction transactionToRetry = new RetrySaleCreditCardTransaction()
                {
                    SecurityCode = this.codigoSeguranca,
                    TransactionKey = Guid.Parse(this.chaveTransacaoCartao),
                };

                var httpResponse = client.Sale.Retry(orderKey, transactionToRetry);
                
                this.retornouOK = (httpResponse.HttpStatusCode == HttpStatusCode.OK
                        && httpResponse.Response.CreditCardTransactionResultCollection.Any()
                        && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true));

                return httpResponse;
            }
            else 
            {
                var httpResponse = client.Sale.Retry(orderKey);
                
                this.retornouOK = (httpResponse.HttpStatusCode == HttpStatusCode.OK
                        && httpResponse.Response.CreditCardTransactionResultCollection.Any()
                        && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true));

                return httpResponse;
            }
        }
    }
}

using GatewayApiClient;
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
    /// Este objeto cria uma consulta para determinada transação.
    /// </summary>
    public class Consulta
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
        /// Retorna o número do seu pedido.
        /// </summary>
        public string retornaSeuPedido { get; private set; }

        /// <summary>
        /// Resultado da operação.
        /// </summary>
        public bool retornouOK { get; private set; }

        /// <summary>
        /// Executar a consulta parametrizada. Retorna os resultados da operação.
        /// </summary>
        /// <returns></returns>
        public HttpResponse Executar()
        {
            this.urlAmbiente = (this.ambiente == "P") ? this.producao : this.homologacao;

            Guid merchantKey = Guid.Parse(this.merchantKey);
            Guid orderKey = Guid.Parse(this.chaveDoPedido);

            // Cria o cliente para consultar o pedido no gateway.
            IGatewayServiceClient client = new GatewayServiceClient(merchantKey, new Uri(this.urlAmbiente));

            // Consulta o pedido.
            var httpResponse = client.Sale.QueryOrder(orderKey);

            this.retornouOK = (httpResponse.HttpStatusCode == HttpStatusCode.OK);

            if (httpResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                foreach (var sale in httpResponse.Response.SaleDataCollection)
                {
                    this.retornaSeuPedido = sale.OrderData.OrderReference;
                }
            }

            return httpResponse;
        }
    }
}

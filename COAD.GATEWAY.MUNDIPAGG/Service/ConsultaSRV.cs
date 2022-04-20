using COAD.GATEWAY.MUNDIPAGG.Model;
using GatewayApiClient;
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
    /// Este objeto cria uma consulta para determinada transação.
    /// </summary>
    public class ConsultaSRV
    {
        /// <summary>
        /// Executar a consulta parametrizada. Retorna os resultados da operação.
        /// </summary>
        /// <returns></returns>
        public HttpResponse Executar(ConsultaDTO consulta)
        {
            consulta.urlAmbiente = (consulta.ambiente == "P") ? consulta.producao : consulta.homologacao;

            Guid merchantKey = Guid.Parse(consulta.merchantKey);
            Guid orderKey = Guid.Parse(consulta.chaveDoPedido);

            // Cria o cliente para consultar o pedido no gateway.
            IGatewayServiceClient client = new GatewayServiceClient(merchantKey, new Uri(consulta.urlAmbiente));

            // Consulta o pedido.
            var httpResponse = client.Sale.QueryOrder(orderKey);

            consulta.retornouOK = (httpResponse.HttpStatusCode == HttpStatusCode.OK);

            if (httpResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                foreach (var sale in httpResponse.Response.SaleDataCollection)
                {
                    consulta.retornaSeuPedido = sale.OrderData.OrderReference;
                }
            }

            return httpResponse;
        }
    }
}

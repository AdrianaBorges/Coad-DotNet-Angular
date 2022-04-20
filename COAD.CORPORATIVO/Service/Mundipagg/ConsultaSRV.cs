using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using MundiAPI.PCL;
using MundiAPI.PCL.Models;

/*
using GatewayApiClient;
using GatewayApiClient.Utility;
*/

namespace COAD.CORPORATIVO.Service.Mundipagg
{
    public class ConsultaSRV
    {

        /// <summary>
        /// Executar a consulta parametrizada. Retorna os resultados da operação.
        /// </summary>
        /// <returns></returns>
        public GetChargeResponse Executar(ConsultaDTO consulta)
        {

            string basicAuthUserName = consulta.chave_secreta; // The username to use with basic authentication
            string basicAuthPassword = ""; // The password to use with basic authentication

            MundiAPIClient cliente = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

            GetChargeResponse response;

            try
            {

                response = cliente.Charges.GetCharge(consulta.chaveDoPedido);

            }
            catch
            {

                response = new GetChargeResponse();

            }
            

            return response;

            /*
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
            */
        }

        public bool TesteConsulta ( string chavePedido )
        {

            ConsultaDTO consulta = new ConsultaDTO
            {
                ambiente = "H",
                chaveDoPedido = ( chavePedido != null ? chavePedido : "10101010-1010-1010-1010-101010101010"),
            };

            GetChargeResponse response = this.Executar(consulta);

            return ( response.Code != null ) ;


        }


    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.GATEWAY.MUNDIPAGG.Model
{
    public class CancelamentoDTO
    {
        /// <summary>
        /// (Endpoint) sandbox - ambiente de testes e HOMOLOGAÇÃO na MundiPagg.
        /// </summary>
        public string homologacao = "https://sandbox.mundipaggone.com";

        /// <summary>
        /// (Endpoint) transactionv2 - ambiente de PRODUÇÃO na MundiPagg.
        /// </summary>
        public string producao = "https://transactionv2.mundipaggone.com";

        /// <summary>
        /// Seu ambiente de trabalho na MundiPagg. [P]rodução ou [H]omologação. O Padrão é "H".
        /// </summary>
        public string ambiente = "H";

        /// <summary>
        /// Url do ambiente escolhido.
        /// </summary>
        public string urlAmbiente;

        /// <summary>
        /// (merchantKey) Sua chave de acesso à MundiPagg. Ela é a autenticação necessária para todas as requisições enviadas aos nossos endpoints.
        /// </summary>
        public string merchantKey = "85328786-8BA6-420F-9948-5352F5A183EB";

        /// <summary>
        /// (orderKey) Chave do pedido gerada pela MundiPagg para a requisição a cancelar.
        /// </summary>
        public string chaveDoPedido { get; set; }

        /// <summary>
        /// (TransactionKey) Chave da transação com cartões de crédito gerada pela MundiPagg para a requisição a cancelar.
        /// </summary>
        public string chaveTransacaoCartao { get; set; }

        /// <summary>
        /// (AmountInCents) Valor a cancelar.
        /// </summary>
        public int valorCancelar { get; set; }

        /// <summary>
        /// Resultado da operação.
        /// </summary>
        public bool retornouOK { get; set; }

        /// <summary>
        /// Executar o cancelamento parametrizado. Retorna os resultados da operação.
        /// </summary>
        /// <returns></returns>
    }
}

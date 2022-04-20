using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{
    public class ConsultaDTO
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
        public string merchantKey = "374b6325-9cbe-4f70-9476-65bd0dd8020b";
            //"380e7bd3-76c4-424e-92be-4d234b5ea371";
            //"f36f4057-c3d9-4809-a4b7-15a363846a7b";
            //"85328786-8BA6-420F-9948-5352F5A183EB";

        /// <summary>
        /// (orderKey) Chave do pedido gerada pela MundiPagg para a requisição.
        /// </summary>
        public string chaveDoPedido { get; set; }

        /// <summary>
        /// Retorna o número do seu pedido.
        /// </summary>
        public string retornaSeuPedido { get; set; }

        /// <summary>
        /// Resultado da operação.
        /// </summary>
        public bool retornouOK { get; set; }

        public string chave_secreta = "sk_test_MemOXpjhD9F2vO7o";

        public string chave_publica = "pk_test_4morLGaiK8CnjKbJ";

    }
}

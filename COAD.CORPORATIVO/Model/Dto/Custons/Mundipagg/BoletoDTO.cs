using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{

    public class BoletoDTO
    {

        public BoletoDTO()
        {


        }

        public int bol_id { get; set; }
        public int dias { get; set; }
        public int valor { get; set; }
        public string banco { get; set; }
        public string nome { get; set; } // ou CLIENTE ( CLI_ID )
        public string email { get; set; } // ou CLIENTE ( CLI_ID )
        public string cidade { get; set; }
        public string complemento { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public string UF { get; set; }
        public string endereco { get; set; }
        public string CEP { get; set; }
        public string numeroDocumento { get; set; }
        public string instrucoes { get; set; }
        public string transacaoInterna { get; set; }

        public DateTime DATA_CANCELAMENTO { get; set; }

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
        //"pk_test_4morLGaiK8CnjKbJ";
        //"374b6325-9cbe-4f70-9476-65bd0dd8020b";
        //"380e7bd3-76c4-424e-92be-4d234b5ea371";

        public string chave_secreta = "sk_test_tra6ezsW3BtPPXQa";

        public string chave_publica = "pk_test_gaa5xzfz7CfPPZAv";


    }
}

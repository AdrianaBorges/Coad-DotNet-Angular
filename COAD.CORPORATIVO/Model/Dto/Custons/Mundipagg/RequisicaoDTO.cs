using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using GatewayApiClient;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using System.Collections.ObjectModel;
using System.Net;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg
{
    public class RequisicaoDTO
    {

        public RequisicaoDTO()
        {
            this.cartaoCredito = new Collection<CreditCardTransaction>();
            this.boleto = new Collection<BoletoTransaction>();
        }

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
        public string ambiente;

        /// <summary>
        /// Url do ambiente escolhido.
        /// </summary>
        public string urlAmbiente;

        public Collection<BoletoTransaction> boleto { get; set; }
        public Buyer comprador { get; set; }
        public Collection<CreditCardTransaction> cartaoCredito { get; set; }
        public Merchant loja { get; set; }
        public ShoppingCart carrinho { get; set; }

        /// <summary>
        /// (Retries) Quantidade tentativas nos cartões de crédito.
        /// </summary>
        public int qtdTentativasCartaoCredito { get; set; }

        /// <summary>
        /// (MerchantReference) Identificação da Loja na Plataforma.
        /// </summary>
        public string identificacaoLoja { get; set; }

        /// <summary>
        /// (IpAddress) Endereço IP da loja de compras.
        /// </summary>
        public string ipLoja { get; set; }

        /// <summary>
        /// (Origin) Site de compras.
        /// </summary>
        public string siteDeCompra { get; set; }

        /// <summary>
        /// (SessionId) Sessão do site de compras.
        /// </summary>
        public string sessaoNoSiteDeCompra { get; set; }

        /// <summary>
        /// (OrderReference) Número do pedido em seu sistema.
        /// </summary>
        public string numeroDoPedido { get; set; }

        /// <summary>
        /// (merchantKey) Sua chave de acesso à MundiPagg. Ela é a autenticação necessária para todas as requisições enviadas aos nossos endpoints.
        /// </summary>
        public string merchantKey = "380e7bd3-76c4-424e-92be-4d234b5ea371";
            //"a950ebd1-49f2-4f7b-9a6d-5e221eb2c1f1"; //"85328786-8BA6-420F-9948-5352F5A183EB";


        /// <summary>
        /// Respostas à sua requisição. Todos os retornos necessários para verificar se suas transações foram aceitas ou recusadas; motivos etc.
        /// </summary>
        public GatewayApiClient.Utility.HttpResponse resposta { get; set; }

        /// <summary>
        /// Código do retorno da requisição. 
        /// <para>Códigos 2xx indicam sucesso, 4xx indicam erro por algum dado informado incorretamente (por exemplo, algum campo obrigatório não enviado ou um cartão sem data de validade)</para>
        /// e 5xx indicando erro nos servidores da MundiPagg.
        /// </summary>
        public HttpStatusCode retornoCodigo { get; set; }

        /// <summary>
        /// Chave do pedido gerada pela MundiPagg para a requisição executada.
        /// </summary>
        public Guid retornoChaveDoPedido { get; set; }

        /// <summary>
        /// Situação ou status da operação com cartões de crédito devolvido para a requisição executada.
        /// </summary>
        public CreditCardTransactionStatusEnum retornoStatusCartaoDeCredito { get; set; }

        /// <summary>
        /// Situação ou status da operação com boletos devolvido para a requisição executada.
        /// </summary>
        public BoletoTransactionStatusEnum retornoStatusBoleto { get; set; }

        /// <summary>
        /// URL gerada para o boleto da requisição executada.
        /// </summary>
        public string retornoUrlBoleto { get; set; }

        /// <summary>
        /// Código de Barras gerado para o boleto da requisição executada.
        /// </summary>
        public string retornoBarraBoleto { get; set; }

        /// <summary>
        /// Chave da transação com cartões de crédito gerada pela MundiPagg para a requisição executada.
        /// </summary>
        public Guid retornoChaveTransacaoCartao { get; set; }

        /// <summary>
        /// Chave da transação com boleto gerada pela MundiPagg para a requisição executada.
        /// </summary>
        public Guid retornoChaveTransacaoBoleto { get; set; }

        public int formaPagamento { get; set; }

        public string OrderKey { get; set; }
        public string OrderReference { get; set; }
        public string AuthorizationCode { get; set; }



    }
}

using GatewayApiClient;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PAGAMENTO.API.Service.GatewayMundiPagg
{
    /// <summary>
    /// ALT: 20/06/2016
    /// Este objeto cria uma requisição de comunicação com a API da MundiPagg
    /// </summary>
    public class Requisicao
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
        /// Passe aqui o retorno do método Criar() do objeto Boleto. Antes, prepare-o devidamente. Veja exemplos em [GatewayMundipaggController.ALTTeste()]
        /// </summary>
        public BoletoTransaction boleto { get; set; }
        
        /// <summary>
        /// Passe aqui o retorno do método Criar() do objeto Comprador. Antes, prepare-o devidamente. Veja exemplos em [GatewayMundipaggController.ALTTeste()]
        /// </summary>
        public Buyer comprador { get; set; }
        
        /// <summary>
        /// Passe aqui a coleção de objetos CartaoDeCredito. Antes, prepare-o devidamente. Veja exemplos em [GatewayMundipaggController.ALTTeste()]
        /// </summary>
        public Collection<CreditCardTransaction> cartaoDeCredito { get; set; }

        /// <summary>
        /// Passe aqui o retorno do método Criar() do objeto Loja. Veja exemplos em [GatewayMundipaggController.ALTTeste()]
        /// </summary>
        public Merchant loja { get; set; }
        
        /// <summary>
        /// Passe aqui o retorno do método Criar() do objeto Carrinho. Use o método AdicionarProduto() para incluir os ítens no carrinho. Veja exemplos em [GatewayMundipaggController.ALTTeste()]
        /// </summary>
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
        public string merchantKey = "85328786-8BA6-420F-9948-5352F5A183EB";

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

        /// <summary>
        /// Criar a requisição com a API da MundiPagg.
        /// </summary>
        /// <returns></returns>
        public CreateSaleRequest Criar()
        {
            // Dados da loja.
            Merchant merchant = new Merchant()
            {
                MerchantReference = this.identificacaoLoja
            };

            // Opções da requisição.
            SaleOptions saleOptions = new SaleOptions()
            {
                AntiFraudServiceCode = 0,
                CurrencyIso = CurrencyIsoEnum.BRL,
                IsAntiFraudEnabled = true, // Habilita a integração com o serviço de anti fraude
                Retries = this.qtdTentativasCartaoCredito // Quantidade máxima de retentativas para o cartão de crédito.
            };

            // Dados da requisição no site da loja.
            RequestData requestData = new RequestData()
            {
                EcommerceCategory = EcommerceCategoryEnum.B2C,
                IpAddress = this.ipLoja,
                Origin = this.siteDeCompra,
                SessionId = this.sessaoNoSiteDeCompra
            };

            // Cria requisição.
            var createSaleRequest = new CreateSaleRequest()
            {
                // Adiciona o boleto na requisição.
                BoletoTransactionCollection = new Collection<BoletoTransaction>(new BoletoTransaction[] { this.boleto }),

                Buyer = this.comprador,
                
                // Adiciona as transações de cartão de crédito na requisição.
                CreditCardTransactionCollection = this.cartaoDeCredito, // múltiplos cartões.
                //CreditCardTransactionCollection = new Collection<CreditCardTransaction>(new CreditCardTransaction[] { this.cartaoDeCredito }), // único cartão.

                Merchant = merchant,
                Options = saleOptions,
                Order = new Order()
                {
                    OrderReference = this.numeroDoPedido
                },
                RequestData = requestData,
                ShoppingCartCollection = new Collection<ShoppingCart>(new ShoppingCart[] { this.carrinho })
            };

            try
            {
                this.urlAmbiente = (this.ambiente == "P") ? this.producao : this.homologacao;

                // Coloque a sua MerchantKey aqui.
                Guid merchantKey = Guid.Parse(this.merchantKey);

                // Cria o client que enviará a transação.
                var serviceClient = new GatewayServiceClient(merchantKey, new Uri(this.urlAmbiente));

                // Autoriza a transação e recebe a resposta do gateway.
                var httpResponse = serviceClient.Sale.Create(createSaleRequest);

                // Preenchendo os atributos para simplificar para o Desenvolvedor.
                this.resposta = httpResponse;

                this.retornoCodigo = httpResponse.HttpStatusCode;

                // chaves usadas para cancelamentos, etc.
                if (httpResponse.Response.CreditCardTransactionResultCollection != null)
                {
                    this.retornoChaveTransacaoCartao = httpResponse.Response.CreditCardTransactionResultCollection.FirstOrDefault().TransactionKey;
                    this.retornoChaveTransacaoBoleto = httpResponse.Response.BoletoTransactionResultCollection.FirstOrDefault().TransactionKey;
                }
                this.retornoChaveDoPedido = httpResponse.Response.OrderResult.OrderKey;

                // status de retorno do boleto e das operadoras de cartão.
                if (httpResponse.Response.CreditCardTransactionResultCollection != null)
                {
                    this.retornoStatusCartaoDeCredito = httpResponse.Response.CreditCardTransactionResultCollection.FirstOrDefault().CreditCardTransactionStatus;
                    this.retornoStatusBoleto = httpResponse.Response.BoletoTransactionResultCollection.FirstOrDefault().BoletoTransactionStatus;
                }

                // url e barra do boleto gerado.
                if (httpResponse.Response.CreditCardTransactionResultCollection != null)
                {
                    this.retornoUrlBoleto = httpResponse.Response.BoletoTransactionResultCollection.FirstOrDefault().BoletoUrl;
                    this.retornoBarraBoleto = httpResponse.Response.BoletoTransactionResultCollection.FirstOrDefault().Barcode;
                }
            }
            catch 
            {
                createSaleRequest = null;
            }

            // Retornando a requisição.
            return createSaleRequest;
        }
    }
}

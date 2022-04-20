using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using GatewayApiClient;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using GatewayApiClient.EnumTypes;
using System.Collections.ObjectModel;
using System.Net;
using System.Transactions;

namespace COAD.CORPORATIVO.Service.Mundipagg
{
    public class RequisicaoSRV
    {

        public CreateSaleRequest CriarRequest(RequisicaoDTO requisicao)
        {
            try
            {

                // Dados da loja.
                Merchant merchant = new Merchant()
                {
                    MerchantReference = requisicao.identificacaoLoja
                };

                // Opções da requisição.
                SaleOptions saleOptions = new SaleOptions()
                {
                    AntiFraudServiceCode = 0,
                    CurrencyIso = CurrencyIsoEnum.BRL,
                    IsAntiFraudEnabled = true, // Habilita a integração com o serviço de anti fraude
                    Retries = requisicao.qtdTentativasCartaoCredito // Quantidade máxima de retentativas para o cartão de crédito.
                };

                // Dados da requisição no site da loja.
                RequestData requestData = new RequestData()
                {
                    EcommerceCategory = EcommerceCategoryEnum.B2C,
                    IpAddress = requisicao.ipLoja,
                    Origin = requisicao.siteDeCompra,
                    SessionId = requisicao.sessaoNoSiteDeCompra
                };

                var createSaleRequest = new CreateSaleRequest();
                var order = new Order();
                order.OrderReference = requisicao.numeroDoPedido;

                if (requisicao.formaPagamento == 1)
                {
                    createSaleRequest.BoletoTransactionCollection = requisicao.boleto;
                    createSaleRequest.Buyer = requisicao.comprador;
                    createSaleRequest.RequestData = requestData;
                    createSaleRequest.Order = order;
                }
                else
                {
                    createSaleRequest.CreditCardTransactionCollection = requisicao.cartaoCredito;
                    createSaleRequest.Options = saleOptions;
                    createSaleRequest.ShoppingCartCollection = new Collection<ShoppingCart>(new ShoppingCart[] { requisicao.carrinho });
                    createSaleRequest.RequestData = requestData;
                    createSaleRequest.Buyer = requisicao.comprador;
                    createSaleRequest.Merchant = merchant;

                }

                return createSaleRequest;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// Criar a requisição com a API da MundiPagg.
        /// </summary>
        /// <returns></returns>       
        public RequisicaoDTO Criar(RequisicaoDTO requisicao)
        {

            try
            {

                var createSaleRequest = this.CriarRequest(requisicao);

                requisicao.urlAmbiente = (requisicao.ambiente == "P") ? requisicao.producao : requisicao.homologacao;

                // Coloque a sua MerchantKey aqui.
                Guid merchantKey = Guid.Parse(requisicao.merchantKey);

                // Cria o client que enviará a transação.
                var serviceClient = new GatewayServiceClient(merchantKey, new Uri(requisicao.urlAmbiente));

                // Autoriza a transação e recebe a resposta do gateway.
                var httpResponse = serviceClient.Sale.Create(createSaleRequest);

                if (httpResponse.Response.ErrorReport != null)
                {
                    if (httpResponse.Response.ErrorReport.ErrorItemCollection.Count() > 0)
                    {

                        string _erro = "<ul>";

                        foreach (var _item in httpResponse.Response.ErrorReport.ErrorItemCollection)
                        {
                            _erro += "<li>" + _item.Description + "</li>";

                        }

                        _erro += "</ul>";

                        throw new Exception(_erro);
                    }
                }


                requisicao.resposta = httpResponse;
                requisicao.retornoCodigo = httpResponse.HttpStatusCode;

                // chaves usadas para cancelamentos, etc.
                if (httpResponse.Response.CreditCardTransactionResultCollection != null)
                {
                    var _ChaveTransacaoCartao = httpResponse.Response.CreditCardTransactionResultCollection.FirstOrDefault();

                    if (_ChaveTransacaoCartao != null)
                        requisicao.retornoChaveTransacaoCartao = _ChaveTransacaoCartao.TransactionKey;

                }

                if (httpResponse.Response.BoletoTransactionResultCollection != null)
                {
                    var _ChaveTransacaoBoleto = httpResponse.Response.BoletoTransactionResultCollection.FirstOrDefault();

                    if (_ChaveTransacaoBoleto != null)
                        requisicao.retornoChaveTransacaoBoleto = _ChaveTransacaoBoleto.TransactionKey;
                }

                if (httpResponse.Response.OrderResult != null)
                    requisicao.retornoChaveDoPedido = httpResponse.Response.OrderResult.OrderKey;

                // status de retorno do boleto e das operadoras de cartão.
                if (httpResponse.Response.CreditCardTransactionResultCollection != null)
                {
                    if (httpResponse.Response.CreditCardTransactionResultCollection.Count > 0)
                    {
                        var _StatusCartaoDeCredito = httpResponse.Response.CreditCardTransactionResultCollection.FirstOrDefault();

                        if (_StatusCartaoDeCredito.CreditCardTransactionStatus != CreditCardTransactionStatusEnum.AuthorizedPendingCapture &&
                            _StatusCartaoDeCredito.CreditCardTransactionStatus != CreditCardTransactionStatusEnum.Captured)
                        {
                            throw new Exception(_StatusCartaoDeCredito.AcquirerMessage);
                        }

                        if (_StatusCartaoDeCredito != null)
                        {
                            requisicao.retornoStatusCartaoDeCredito = _StatusCartaoDeCredito.CreditCardTransactionStatus;
                            requisicao.AuthorizationCode = _StatusCartaoDeCredito.AuthorizationCode;

                        }
                    }

                }

                // url e barra do boleto gerado.
                if (httpResponse.Response.BoletoTransactionResultCollection != null)
                {
                    if (httpResponse.Response.BoletoTransactionResultCollection.Count > 0)
                    {
                        var _StatusBoleto = httpResponse.Response.BoletoTransactionResultCollection.FirstOrDefault();

                        if (_StatusBoleto.BoletoTransactionStatus != BoletoTransactionStatusEnum.Generated &&
                            _StatusBoleto.BoletoTransactionStatus != BoletoTransactionStatusEnum.Viewed)
                        {
                            throw new Exception("Não foi possível gerar o boleto. - " + _StatusBoleto.BoletoTransactionStatus);
                        }

                        if (_StatusBoleto != null)
                        {
                            requisicao.retornoStatusBoleto = _StatusBoleto.BoletoTransactionStatus;
                            requisicao.retornoUrlBoleto = _StatusBoleto.BoletoUrl;
                            requisicao.retornoBarraBoleto = _StatusBoleto.Barcode;

                        }
                    }
                }

                requisicao.OrderKey = httpResponse.Response.OrderResult.OrderKey.ToString();
                requisicao.OrderReference = httpResponse.Response.OrderResult.OrderReference;

                return requisicao;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public RequisicaoDTO Cancelar(RequisicaoDTO requisicao)
        {

            try
            {

                // Coloque a sua MerchantKey aqui.
                Guid merchantKey = Guid.Parse(requisicao.merchantKey);

                Guid orderKey = Guid.Parse(requisicao.OrderKey);

                var serviceClient = new GatewayServiceClient(merchantKey, new Uri(requisicao.urlAmbiente));

                var httpResponse = serviceClient.Sale.Manage(ManageOperationEnum.Cancel, orderKey);

                if (httpResponse.Response.ErrorReport != null)
                {
                    if (httpResponse.Response.ErrorReport.ErrorItemCollection.Count() > 0)
                    {

                        string _erro = "";

                        foreach (var _item in httpResponse.Response.ErrorReport.ErrorItemCollection)
                        {
                            _erro += _item.ErrorField + " => " + _item.Description + " \n ";

                        }

                        throw new Exception(_erro);
                    }
                }


                requisicao.resposta = httpResponse;
                requisicao.retornoCodigo = httpResponse.HttpStatusCode;

                // chaves usadas para cancelamentos, etc.
                if (httpResponse.Response.CreditCardTransactionResultCollection != null)
                {
                    var _ChaveTransacaoCartao = httpResponse.Response.CreditCardTransactionResultCollection.FirstOrDefault();

                    if (_ChaveTransacaoCartao != null)
                        requisicao.retornoChaveTransacaoCartao = _ChaveTransacaoCartao.TransactionKey;

                }


                // status de retorno do boleto e das operadoras de cartão.
                if (httpResponse.Response.CreditCardTransactionResultCollection != null)
                {
                    var _StatusCartaoDeCredito = httpResponse.Response.CreditCardTransactionResultCollection.FirstOrDefault();

                    if (_StatusCartaoDeCredito != null)
                        requisicao.retornoStatusCartaoDeCredito = _StatusCartaoDeCredito.CreditCardTransactionStatus;

                }



                return requisicao;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public RequisicaoDTO Capturar(RequisicaoDTO requisicao)
        {

            try
            {

                // Coloque a sua MerchantKey aqui.
                Guid merchantKey = Guid.Parse(requisicao.merchantKey);

                Guid orderKey = Guid.Parse(requisicao.OrderKey);

                var serviceClient = new GatewayServiceClient(merchantKey, new Uri(requisicao.urlAmbiente));

                var httpResponse = serviceClient.Sale.Manage(ManageOperationEnum.Capture, orderKey);

                if (httpResponse.Response.ErrorReport != null)
                {
                    if (httpResponse.Response.ErrorReport.ErrorItemCollection.Count() > 0)
                    {

                        string _erro = "";

                        foreach (var _item in httpResponse.Response.ErrorReport.ErrorItemCollection)
                        {
                            _erro += _item.ErrorField + " => " + _item.Description + " \n ";

                        }

                        throw new Exception(_erro);
                    }
                }


                requisicao.resposta = httpResponse;
                requisicao.retornoCodigo = httpResponse.HttpStatusCode;

                // chaves usadas para cancelamentos, etc.
                if (httpResponse.Response.CreditCardTransactionResultCollection != null)
                {
                    var _ChaveTransacaoCartao = httpResponse.Response.CreditCardTransactionResultCollection.FirstOrDefault();

                    if (_ChaveTransacaoCartao != null)
                        requisicao.retornoChaveTransacaoCartao = _ChaveTransacaoCartao.TransactionKey;

                }


                // status de retorno do boleto e das operadoras de cartão.
                if (httpResponse.Response.CreditCardTransactionResultCollection != null)
                {
                    var _StatusCartaoDeCredito = httpResponse.Response.CreditCardTransactionResultCollection.FirstOrDefault();

                    if (_StatusCartaoDeCredito != null)
                        requisicao.retornoStatusCartaoDeCredito = _StatusCartaoDeCredito.CreditCardTransactionStatus;

                }


                return requisicao;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        /*
        public RequisicaoDTO CheckOut(CompradorDTO _comprador, CartaoCreditoDTO _cartao, CarrinhoDTO _carrinho)
        {
            try
            {
                var _transacaoCartao = new CreditCardTransaction();
                var _transacaoBoleto = new BoletoTransaction();

                RequisicaoDTO requisicao = new RequisicaoDTO();
                requisicao.comprador = new CompradorSRV().Criar(_comprador);
                requisicao.carrinho = new CarrinhoSRV().Criar(_carrinho);
                if (_cartao.formapgto == 1)
                    _transacaoBoleto = new CartaoCreditoSRV().CriarTransacaoBoleto(_cartao);
                else
                    _transacaoCartao = new CartaoCreditoSRV().CriarTransacaoCartao(_cartao);

                requisicao.numeroDoPedido = _cartao.numeroPedido;
                requisicao.formaPagamento = _cartao.formapgto;
                requisicao.cartaoCredito.Add(_transacaoCartao);
                requisicao.boleto.Add(_transacaoBoleto);
                requisicao.ambiente = "P";
                return this.Criar(requisicao);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        */


    }
}

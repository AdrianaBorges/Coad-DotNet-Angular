using COAD.CORPORATIVO.Service;
using COAD.GATEWAY.MUNDIPAGG.Model;
using COAD.GATEWAY.MUNDIPAGG.Service;
using COAD.PROXY.Model.DTO;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service.Interfaces;

namespace COAD.PROXY.Service
{
    public class CheckOutProxySRV
    {
        public IEmailSRV _emailSRV { get; set; }
        public string Pagamento(CompradorDTO _comprador, CartaoCreditoDTO _cartao, CarrinhoDTO _carrinho)
        {

            RequisicaoDTO _retorno = new RequisicaoDTO();

            RequisicaoSRV _reqsrv = new RequisicaoSRV();

            Boolean _realizouTransacao = false;

            string _urlretorno = "";

            try
            {
                TransactionOptions txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {

                    var _alteracao = new ItemPedidoSRV().RetornarDadosDePagamento(Convert.ToInt32(_cartao.numeroPedido));

                    if ((bool)_alteracao.Recorrente == true)
                    {
                        _cartao.dataInicioRecorrencia = DateTime.Now;
                        _cartao.frequenciaDaRecorrencia = FrequencyEnum.Monthly;
                        _cartao.intervaloDaRecorrencia = 1;
                        _cartao.numeroDeRecorrencias = 0;
                    }

                    _cartao.tipoOperacao = CreditCardOperationEnum.AuthOnly;
                    _cartao.numeroParcelas = (int)_alteracao.qtdParcelas;
                    
                    _retorno = _reqsrv.CheckOut(_comprador, _cartao, _carrinho);

                    if (_retorno.retornoCodigo != HttpStatusCode.OK &&
                        _retorno.retornoCodigo != HttpStatusCode.Created &&
                        _retorno.retornoCodigo != HttpStatusCode.Accepted)
                        throw new Exception("Não foi posível realizar o pagamento (" + _retorno.retornoCodigo.ToString() + ")");

                    _realizouTransacao = true;

                    if (_cartao.formapgto == 1)
                    {
                        _alteracao.StatusTransacaoBoleto = _retorno.retornoStatusBoleto;
                        _alteracao.ChaveTransacaoBoleto = _retorno.retornoChaveTransacaoBoleto;
                    }
                    else
                    {
                        _alteracao.StatusTransacaoCC = _retorno.retornoStatusCartaoDeCredito;
                        _alteracao.ChaveTransacaoCartao = _retorno.retornoChaveTransacaoCartao;
                    }

                    _alteracao.UrlBoleto = _retorno.retornoUrlBoleto;
                    _alteracao.CodigoBarras = _retorno.retornoBarraBoleto;
                    _alteracao.USU_LOGIN = "COADPAG";
                    _alteracao.REP_ID = 2601;
                    _alteracao.CLI_ID = _comprador.cli_id;
                    _alteracao.OrderKey = _retorno.OrderKey;
                    _alteracao.OrderReference = _retorno.OrderReference;
                    _alteracao.AuthorizationCode = _retorno.AuthorizationCode;
                    _alteracao.darBaixar = (_cartao.formapgto != 1);
                    _alteracao.marcarPedidoPagamentoPago = (_cartao.formapgto != 1);

                //    if (_cartao.formapgto != 1)
                        new ItemPedidoSRV().PagarPedido(_alteracao);
                   
                    _reqsrv.Capturar(_retorno);

                    _urlretorno = _alteracao.UrlBoleto;

                   // _emailSRV.EnviarEmail(_email, "COAD", _mensagem);

                    scope.Complete();

                }

                
            }
            catch (Exception e)
            {
                if (_realizouTransacao)
                    _reqsrv.Cancelar(_retorno);

                throw new Exception(SysException.Show(e));
            }

            return _urlretorno;

        }
        public DadosPedidoIntegracaoDTO SalvarPedido(CompradorDTO _comprador, CartaoCreditoDTO _cartao)
        {

            try
            {
                DadosPedidoIntegracaoDTO _pedido = new DadosPedidoIntegracaoDTO();

                _pedido.numeroDocumento = _comprador.numeroDocumento;
                _pedido.cli_id = _comprador.cli_id;

                _pedido.nome = _comprador.nome;
                _pedido.email = _comprador.email;
                _pedido.senha = _comprador.senha;
                _pedido.tipoDocumento = (_comprador.tipoDocumento == DocumentTypeEnum.CPF) ? 0 : 1;
                _pedido.email = _comprador.email;
                _pedido.numeroDocumento = _comprador.numeroDocumento;
                _pedido.formapgto = _cartao.formapgto;
                _pedido.numeroparcelas = _cartao.numeroParcelas;

                //-----------

                _pedido.CEP = _comprador.CEP;
                _pedido.bairro = _comprador.bairro;
                _pedido.complemento = _comprador.complemento;
                _pedido.endereco = _comprador.endereco;
                _pedido.cidade = _comprador.cidade;
                _pedido.numero = _comprador.numero;
                _pedido.UF = _comprador.UF;

                _pedido.dddComercial = _comprador.dddComercial;
                _pedido.foneComercial = _comprador.foneComercial;
                _pedido.dddCelular = _comprador.dddCelular;
                _pedido.foneCelular = _comprador.foneCelular;
                _pedido.email = _comprador.email;
                _pedido.numeroparcelas = _cartao.numeroParcelas;
                _pedido.cmp_id = _comprador.cmp_id;
                // TTP_ID (_pedido.recorrente) = 1 -- Mensal Recorrente
                // TTP_ID (_pedido.recorrente) = 6 -- Anual
                _pedido.recorrente = _cartao.recorrente;

                ServiceReference1.CoadServiceClient proxy = new ServiceReference1.CoadServiceClient("BasicHttpBinding_ICoadService");

                var pedidoSalvo = proxy.GerarPedido(_pedido);

                if (pedidoSalvo.erro > 0)
                    throw new Exception(pedidoSalvo.mensagem);

                return pedidoSalvo;

            }
            catch (Exception e)
            {
                throw new Exception(SysException.Show(e));
            }

        }
        public CartaoIntegracaoDTO CarregaPedidoPagamento(CartaoIntegracaoDTO _cartaoPagamento)
        {
         
            try
            {

                var _ipevalidahash = SessionContext.HashMD5(_cartaoPagamento.ipeId.ToString());

                if (_cartaoPagamento.ipeHash != _ipevalidahash)
                    throw new Exception("Erro de validação. Dados do pedido inválidos!");


                var _item = new ItemPedidoSRV().RetornarDadosDePagamento(_cartaoPagamento.ipeId);

                CartaoCreditoDTO _cartao = new CartaoCreditoDTO();
                CompradorDTO _comprador = new CompradorDTO();
                ItemPedidoSRV _pedidosrv = new ItemPedidoSRV();

                var _pedido = _item.ITEM_PEDIDO; // _pedidosrv.FindById(_ipe_id);

             //   if (_item.ITEM_PEDIDO.RG_ID != 11)
                if (_pedido.PST_ID == 7 || _pedido.PST_ID == 3)
                    throw new Exception("Pedido já faturado!!");

                //--------------------------

                var _cliente = _item.CLIENTE;

                _comprador.tipoPessoa = (_cliente.CLI_CPF_CNPJ.Length == 11) ? PersonTypeEnum.Person : PersonTypeEnum.Company;
                _comprador.tipoDocumento = (_cliente.CLI_CPF_CNPJ.Length <= 11) ? DocumentTypeEnum.CPF : DocumentTypeEnum.CNPJ;
                _comprador.categoria = BuyerCategoryEnum.Normal;
                _comprador.email = _cliente.CLI_EMAIL;
                _comprador.nome = _cliente.CLI_NOME;
                _comprador.cli_id = _cliente.CLI_ID;
                _comprador.numeroDocumento = _cliente.CLI_CPF_CNPJ;

                var _cliente_email = _cliente.ASSINATURA_EMAIL.FirstOrDefault();

                var endereco = _item.CLIENTE_ENDERECO;  // new ClienteEnderecoSRV().FindEnderecoCliente((int)_cliente.CLI_ID, 2);

                if (endereco != null)
                {
                    _comprador.cidade = endereco.END_MUNICIPIO;
                    _comprador.complemento = endereco.END_COMPLEMENTO;
                    _comprador.bairro = endereco.END_BAIRRO;
                    _comprador.numero = endereco.END_NUMERO;
                    _comprador.UF = endereco.END_UF;
                    _comprador.endereco = endereco.END_LOGRADOURO;
                    _comprador.CEP = endereco.END_CEP;

                    if (_cliente_email != null)
                        _comprador.email = _cliente_email.AEM_EMAIL;

                    //-----------------------------------------------

                    _cartao.valor = _comprador.valor;
                    _cartao.endereco = _comprador.endereco;
                    _cartao.numero = _comprador.numero;
                    _cartao.complemento = _comprador.complemento;
                    _cartao.bairro = _comprador.bairro;
                    _cartao.CEP = _comprador.CEP;
                    _cartao.cidade = _comprador.cidade;
                    _cartao.UF = _comprador.UF;


                }

                //-------------------------
                _cartao.portador = _cartaoPagamento.portador;
                _cartao.bandeira = _cartaoPagamento.bandeira;
                _cartao.numeroCartao = _cartaoPagamento.numeroCartao;
                _cartao.mesExpiracao = _cartaoPagamento.mesExpiracao;
                _cartao.anoExpiracao = _cartaoPagamento.anoExpiracao;
                _cartao.codigoSeguranca = _cartaoPagamento.codigoSeguranca;
                _cartao.meioDePagamento = _cartaoPagamento.meioDePagamento;
                //-------------------------

                _cartao.valorParcelas = _item.ITEM_PEDIDO.IPE_VALOR_PARCELA;
                _cartao.numeroParcelas = (int)_item.qtdParcelas;
                _cartao.formapgto = (_item.TPG_ID == 9) ? 2 : 1;
                _cartao.numeroPedido = _pedido.IPE_ID.ToString();
                _cartao.cmpDescricao = _pedido.PRODUTO_COMPOSICAO.CMP_DESCRICAO;
                _cartao.cmpQuantidade = (int)_pedido.IPE_QTD;
                _cartao.cmpVlrUnit = (long)(_pedido.IPE_PRECO_UNITARIO * 100);
                _cartao.cmpVlrTotal = (long)(_item.ValorPagamento * 100);
                _cartao.valor = (long)(_item.ValorPagamento * 100);

                _cartao.cmpVlrUnittela = _pedido.IPE_PRECO_UNITARIO;
                _cartao.cmpVlrTotaltela = _item.ValorPagamento;
                _cartao.valortela = _item.ValorPagamento;

                //--------------------------
                CarrinhoDTO _carrinho = new CarrinhoDTO();

                _carrinho.cidade = _comprador.cidade;
                _carrinho.complemento = _comprador.complemento;
                _carrinho.bairro = _comprador.bairro;
                _carrinho.numero = _comprador.numero;
                _carrinho.UF = _comprador.UF;
                _carrinho.endereco = _comprador.UF;
                _carrinho.CEP = _comprador.CEP;
                _carrinho.prazoMaximoEntrega = 2;
                _carrinho.prazoEstimadoEntrega = 2;
                _carrinho.valorFrete = 0;
                _carrinho.transportadora = "";
                //--------------------------

                this.Pagamento(_comprador, _cartao, _carrinho);

                _cartaoPagamento.erro = 0;
                _cartaoPagamento.mensagem = "Operação realizada com sucesso !!";

                return _cartaoPagamento;
            }
            catch (Exception e)
            {
                _cartaoPagamento.erro = 1;
                _cartaoPagamento.mensagem = e.Message;

                return _cartaoPagamento;
            }

        }
    }
}

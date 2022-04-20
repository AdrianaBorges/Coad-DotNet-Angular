using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.GATEWAY.MUNDIPAGG.Model;
using COAD.GATEWAY.MUNDIPAGG.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Coad.GenericCrud.Service;
using GenericCrud.Validations;
using COAD.PROXY.Service;
using GenericCrud.Util;

namespace COADPAG.Controllers
{
    public class CheckoutController : Controller
    {
 
        public ActionResult Pagamento(int? ipeid, string ipehash)
        {
            ViewBag.TelaTopo = "PAGAMENTO";
            ViewBag.Tela = "PAGAMENTO";
            ViewBag.IpeId = ipeid;
            ViewBag.IpeHash = ipehash;

            
            Autenticado _autenticado = new Autenticado();
            _autenticado.IP_ACESSO = SessionContext.GetIp();
            _autenticado.USU_LOGIN = "COADPAG";
            SessionContext.autenticado = _autenticado;

            return View();
        }
        public ActionResult DadosPagamentoInit(int _ipe_id, string _ipe_hash)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                var _ipevalidahash = SessionContext.HashMD5(_ipe_id.ToString());

                if (_ipe_hash != _ipevalidahash)
                    throw new Exception("Erro de validação. Dados do pedido inválidos!");


                var _item = new ItemPedidoSRV().RetornarDadosDePagamento(_ipe_id);

                CartaoCreditoDTO _cartao = new CartaoCreditoDTO();
                CompradorDTO _comprador = new CompradorDTO();
                ItemPedidoSRV _pedidosrv = new ItemPedidoSRV();

                var _pedido = _item.ITEM_PEDIDO; // _pedidosrv.FindById(_ipe_id);
                
                if (_pedido.PST_ID == 7 || _pedido.PST_ID == 3)
                    throw new Exception("Este pedido ja foi pago !!");

                if (_pedido.IPE_DATA_VALIDADE != null)
                    if (DateTime.Now > _pedido.IPE_DATA_VALIDADE)
                        throw new Exception("Pedido selecionado vencido !! Entre em contato com o SAC ou acesse o portal COAD para emissão de um novo pedido.");

                // --------------------------

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
                _cartao.valorParcelas = _item.ITEM_PEDIDO.IPE_VALOR_PARCELA;
                _cartao.numeroParcelas = (int)_item.qtdParcelas;
                _cartao.formapgto = (_item.TPG_ID == 9) ? 2 : 1;
                _cartao.numeroPedido = _pedido.IPE_ID.ToString();
                _cartao.cmpDescricao = _pedido.PRODUTO_COMPOSICAO.CMP_DESCRICAO;
                _cartao.cmpQuantidade = (int)_pedido.IPE_QTD;
                _cartao.cmpVlrUnit = (long)(_pedido.IPE_PRECO_UNITARIO *100);
                _cartao.cmpVlrTotal = (long)(_item.ValorPagamento * 100);
                _cartao.valor = (long)(_item.ValorPagamento * 100);

                _cartao.cmpVlrUnittela = _pedido.IPE_PRECO_UNITARIO;
                _cartao.cmpVlrTotaltela = _item.ValorPagamento;
                _cartao.valortela = _item.ValorPagamento;


                //--------------------------
                response.Add("comprador", _comprador);
                response.Add("cartao", _cartao);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public ActionResult RealizarPagamento(CartaoCreditoDTO _cartao, CompradorDTO _comprador)
        {
            JSONResponse response = new JSONResponse();

            try
            {
         
                //var bandeira = ValidadorCartao.Validar(_cartao.numeroCartao);

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
                    _cartao.meioDePagamento = SysUtils.BuscarCodAdiquirente();


                    var _urlboleto = new CheckOutProxySRV().Pagamento(_comprador, _cartao, _carrinho);

                    response.Add("urlboleto", _urlboleto);
               

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public void EmailConfirmaVenda(CompradorDTO _comprador, CartaoCreditoDTO _cartao, CarrinhoDTO _carrinho)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                EmailSRV _emailSRV = new EmailSRV();
                
                var _numcartao = "#### #### #### " + _cartao.numeroCartao.Substring(12, 4);

                var url = "https://ci4.googleusercontent.com/proxy/GgWnRPBud6_dbgT5a4AZGD1cXJaq7heSiSI6uRSLpqrbeRczzyf8rGzRft8ARSffAAjCKNryW9c1grWR6aZ4DfbBnsH6SAPgdbI5SsEUK5ISOjmLsiZKwAW0iJfwmKPQF_ufrNjh0VNiRRastLGv7F1SB7KA=s0-d-e1-ft#http://emkt.coad.com.br/emkt/dados/10268/10767/Image/Cursos_Novo/Header_Contabilidade_Geral.png";

                var templateEmail = @"<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""400px"">
                                        <tr>
                                            <td>
                                                <br />
                                                <p style=""text-align: left;"">
                                                    <strong>Caro Cliente</strong> , <p> Obrigado por adquirir um produto COAD. Seguem abaixo os dados da sua compra.</p>
                                                </p>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <ol style=""list-style-type: none;"">
                                                    <li>NUMERO:" + _cartao.numeroPedido + @"</li>
                                                    <li>DATA: " + DateTime.Now.ToString("dd/MM/yyyy") + @"</li>
                                                    <li>NOME: " + _comprador.nome + @"</li>
                                                    <li>MEIO PAGAMENTO: Visa (" + _numcartao + @")</li>
                                                    <li>TOTAL: R$ " + String.Format("{0:F}", _cartao.valortela) + @"</li>
                                                </ol>
                                            </td>
                                        </tr>
                                      </table>";


                _emailSRV.EnviarEmailParaCliente(_comprador.email, "Confirmação de Pagamento", templateEmail, url);

                SysException.RegistrarLog("Comprovante de venda enviado (" + _comprador.email + ")", "", null);

            }
            catch (Exception e)
            {
                SysException.RegistrarLog("Comprovante de venda não enviado (" + _comprador.email + " -- " + SysException.Show(e) + ")", "", null);
            }

        }
        public void EnviarEmailBoletoPagamento(CompradorDTO _comprador, string _urlpagamento, string _linhadigitavel)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                EmailSRV _emailSRV = new EmailSRV();
          
                var url = "https://ci4.googleusercontent.com/proxy/GgWnRPBud6_dbgT5a4AZGD1cXJaq7heSiSI6uRSLpqrbeRczzyf8rGzRft8ARSffAAjCKNryW9c1grWR6aZ4DfbBnsH6SAPgdbI5SsEUK5ISOjmLsiZKwAW0iJfwmKPQF_ufrNjh0VNiRRastLGv7F1SB7KA=s0-d-e1-ft#http://emkt.coad.com.br/emkt/dados/10268/10767/Image/Cursos_Novo/Header_Contabilidade_Geral.png";

                var templateEmail =  @"<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""400px"">
                                           <tr>
                                                <td><H2><strong>Agora falta pouco !!!</strong></H2>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style=""text-align: left;"">
                                                        Caro(a) <strong>" + _comprador.nome + @"</strong>,
                                                    </p>
                                                    <p style=""text-align: left;"">
                                                        Para confirmar a sua compra, clique no link abaixo ou copie a URL para visualizar o boleto.
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p style=""text-align: left;"">
                                                       <a href=" + _urlpagamento + @">" + _urlpagamento + @"</a>
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style=""padding: 25px 0 0 0;"">
                                                   <p style=""text-align: left;"">
                                                          Você também pode realizar o pagamento utilizando a linha digitável informada abaixo.
                                                   </p>
                                                   <p style=""text-align: left;"">"+_linhadigitavel+ @"</p>
                                                </td>
                                            </tr>
                                        <tr>
                                        <tr>
                                            <td>
                                                <p>
                                                    <center>Seja bem vido a COAD!</center>
                                                </p>
                                                <p>
                                                    <center>Time COAD</center>
                                                </p>
                                            </td>
                                        </tr>
                                     </table>";


                _emailSRV.EnviarEmailParaCliente(_comprador.email, "COAD - Pagamento", templateEmail, url);

                SysException.RegistrarLog("Comprovante de venda enviado (" + _comprador.email + ")", "", null);

            }
            catch (Exception e)
            {
                SysException.RegistrarLog("Comprovante de venda não enviado (" + _comprador.email + " -- " + SysException.Show(e) + ")", "", null);
            }

        }
   

    }
}

using GatewayApiClient.DataContracts.EnumTypes;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.GATEWAY.MUNDIPAGG.Model;
using COAD.GATEWAY.MUNDIPAGG.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coad.GenericCrud.Exceptions;
using COAD.PROXY.Model.DTO;
using GenericCrud.Validations;
using COAD.PROXY.Service;
using GenericCrud.Util;

namespace COADPORTAL.Controllers
{
    public class CadastroController : Controller
    {

        public ActionResult Conclusao(string _nome, int? _formapgto)
        {
            ViewBag.TelaTopo = "CONCLUSÃO";
            ViewBag.Tela = "CONCLUSÃO";
            ViewBag.Nome = _nome;
            ViewBag.Boleto = _formapgto;

            return View();
        }
        public ActionResult Pagamento()
        {
            ViewBag.TelaTopo = "NOSSOS PLANOS";
            ViewBag.Tela = "PLANOS";
            return View();
        }
        public ActionResult Planos()
        {
            ViewBag.TelaTopo = "NOSSOS PLANOS";
            ViewBag.Tela = "PLANOS";
            return View();
        }
        public ActionResult Identificacao(int? _CMP_ID, int? _CMP_ORIGEM, int? _TTP_ID)
        {
            if (_CMP_ID != null)
                SessionContext.PutInSession("_CMP_ID", _CMP_ID);

            if (_CMP_ORIGEM != null)
                SessionContext.PutInSession("_CMP_ID_ORIGEM", _CMP_ORIGEM);

            if (_TTP_ID != null)
                SessionContext.PutInSession("_TTP_ID", _TTP_ID);
            
            ViewBag.TelaTopo = "IDENTIFICAÇÃO DO CLIENTE";
            ViewBag.Tela = "IDENTIFICAÇÃO";
            return View();
        }
        public ActionResult DadosFaturamento()
        {
            ViewBag.TelaTopo = "DADOS PARA FATURAMENTO";
            ViewBag.Tela = "DADOS PARA FATURAMENTO";
            return View();
        }
        public ActionResult Cadastrar(string _cliportaldto)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                string _retorno = null;

                new ClienteSRV().Cadastrar(_cliportaldto);

                if (_retorno.Trim() != "")
                    new CadastroGratuitoSRV().Cadastrar(_cliportaldto);

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
        public ActionResult Login(string _login, string _senha)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                new ClienteSRV().RealizarLogin(_login, _senha, 8, System.Web.HttpContext.Current);

                CompradorDTO comprador = new CompradorDTO();

                var cliente = new ClienteSRV().FindById(SessionContext.autenticado.CLI_ID);
                comprador.tipoPessoa = (cliente.CLI_TP_PESSOA == "F") ? PersonTypeEnum.Person : PersonTypeEnum.Company;
                comprador.numeroDocumento = cliente.CLI_CPF_CNPJ;
                comprador.tipoDocumento = (cliente.CLI_CPF_CNPJ.Length <= 11) ? DocumentTypeEnum.CPF : DocumentTypeEnum.CNPJ;
                comprador.categoria = BuyerCategoryEnum.Normal;
                comprador.email = SessionContext.autenticado.EMAIL;
                comprador.nome = SessionContext.autenticado.USU_NOME;
                comprador.cli_id = SessionContext.autenticado.CLI_ID;
                comprador.ac = cliente.CLI_A_C;

                ClienteEnderecoSRV _endsrv = new ClienteEnderecoSRV();

                ClienteEnderecoDto endereco = _endsrv.FindEnderecoCliente((int)SessionContext.autenticado.CLI_ID, 2);
                
                if (endereco == null)
                    endereco = _endsrv.FindEnderecoCliente((int)SessionContext.autenticado.CLI_ID, 1);
                   
                if (endereco != null)
                {
                    comprador.cidade = endereco.END_MUNICIPIO;
                    comprador.complemento = endereco.END_COMPLEMENTO;
                    comprador.bairro = endereco.END_BAIRRO;
                    comprador.numero = endereco.END_NUMERO;
                    comprador.UF = endereco.MUNICIPIO.UF;
                    comprador.endereco = endereco.END_LOGRADOURO;
                    comprador.CEP = endereco.END_CEP;
                }

                var clienteEmail = new AssinaturaEmailSRV().FindByNumAssinatura(_login);
                var email = clienteEmail.FirstOrDefault();

                if (email != null)
                    comprador.email = email.AEM_EMAIL;


                var clienteTelefone = new AssinaturaTelefoneSRV().FindByNumAssinatura(_login);
                if (clienteTelefone != null)
                {
                    var telefone = clienteTelefone.Where(x => x.TIPO_TEL_ID == 4).FirstOrDefault();
                    if (telefone != null)
                    {
                        comprador.dddComercial = telefone.ATE_DDD;
                        comprador.foneComercial = telefone.ATE_TELEFONE;
                    }

                    var celular = clienteTelefone.Where(x => x.TIPO_TEL_ID == 1).FirstOrDefault();
                    if (celular != null)
                    {
                        comprador.dddCelular = celular.ATE_DDD;
                        comprador.foneCelular = celular.ATE_TELEFONE;
                    }

                }


                SessionContext.PutInSession("comprador", comprador);

                //if (_retorno.Trim() != "")
                //    new CadastroGratuitoSRV().RealizarLogin(_login, _senha, "COADCORP", System.Web.HttpContext.Current);

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
        public ActionResult VerificaEmail(string _email)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                if (String.IsNullOrEmpty(_email))
                    throw new Exception("Email não informado.");

                var _clientedto = new ClienteSRV().ValidarLoginEmail(_email);

                if (_clientedto != null)
                    throw new Exception("Email já cadastrado");

                CompradorDTO comprador = new CompradorDTO();

                comprador.email = _email;
                                
                SessionContext.PutInSession("comprador", comprador);
                
                response.success = (_clientedto == null);
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LembrarSenha()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LembrarSenha(string _login, string _email)
        {
            string url = HttpContext.Request.Url.AbsoluteUri;
            string path = HttpContext.Request.Url.AbsolutePath;
            Autenticado _autenticado = new Autenticado();

            Random randNum = new Random();

            try
            {
                if (_login.Length <= 0 && _email.Length <= 0)
                    throw new Exception("Login e/ou Email não informados !");

                ClienteDto _usuario = new ClienteSRV().ValidarLoginEmail(_email);

                if (_usuario == null)
                    throw new Exception("Email não encontrado!!");

                _autenticado.USU_LOGIN = _login;
                _autenticado.IP_ACESSO = SessionContext.GetIp();
                _autenticado.PATH = url;
                _autenticado.SESSION_ID = this.Session.SessionID;

                string _novasenha = randNum.Next(2113).ToString() + _login[1] + _login[2] + randNum.Next(2003).ToString();
                string _mensagem = "<DIV>Caro " + _login + ", este email é gerado automaticamente pelo sistema - COADCORP.  </DIV></p>" +
                                   "<DIV>Conforme a sua solicitação, o sistema gerou uma senha automatica, aleatória e provisória.</DIV></p>" +
                                   "<DIV>Voçe deve realizar o login com esta senha provisória e em seguida realizar o cadastramento da sua senha definitiva.</DIV>" +
                                   "<DIV>Senha Provisória => " + _novasenha + " </DIV>";


                new ClienteSRV().AlterarSenha(_usuario);

                SessionContext.EnviarEmail(_email, "Nova Senha", _mensagem);

                SysException.RegistrarLog("Solicitação de Senha - Usuário  (" + _autenticado.USU_LOGIN + ")", "", _autenticado);

                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                _autenticado.USU_LOGIN = _login;
                _autenticado = new Autenticado();
                _autenticado.PATH = url;
                _autenticado.IP_ACESSO = SessionContext.GetIp();

                SysException.RegistrarLog("Solicitação de Senha - Usuário (" + _login + ") " + SysException.Show(ex), SysException.ShowIdException(ex), _autenticado);

                TempData.Add("Resultado", ex.Message);

                return View();
            }

        }
        public ActionResult BuscarTabelaPreco(int _ttp_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                // _ttp_id => 1 Mensal Recorrente
                // _ttp_id => 6 Anual


                var _tabelapreco = new ProdutoComposicaoSRV();

                var _tabela01 = _tabelapreco.FindByProdOrigem(352,_ttp_id, 9);
                var _tabela02 = _tabelapreco.FindByProdOrigem(356,_ttp_id, 9);
                var _tabela03 = _tabelapreco.FindByProdOrigem(363,_ttp_id, 9);

                response.Add("tabela01", _tabela01);
                response.Add("tabela02", _tabela02);
                response.Add("tabela03", _tabela03);
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
        public ActionResult BuscarTipoPagamento(int cmp_id, int tpg_id)
        {
            
            JSONResponse response = new JSONResponse();
            try
            {
                // TTP_ID = 1 -- Mensal Recorrente
                // TTP_ID = 6 -- Anual
                    
                int _ttp_id = SessionContext.GetInSession<int>("_TTP_ID");

                var _tipopagamento = new RegiaoTabelaPrecoSRV().ListarResumoDeParcelamento(11, cmp_id, tpg_id, 1, _ttp_id);
                
                response.Add("tipopagamento", _tipopagamento);
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
        public ActionResult DadosPagamentoInit()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _cmp_id = SessionContext.GetInSession<int>("_CMP_ID");
                var _cmp_origem = SessionContext.GetInSession<int>("_CMP_ID_ORIGEM");

                var _tabelapreco = new ProdutoComposicaoSRV();
                var _tabelaSelecionada = _tabelapreco.FindByProdOrigem(_cmp_origem);
                var comprador = SessionContext.GetInSession<CompradorDTO>("comprador");
                var cartao = SessionContext.GetInSession<CartaoCreditoDTO>("cartao");
                cartao.recorrente = SessionContext.GetInSession<int>("_TTP_ID");
                comprador.cmp_id = _cmp_id;
                

                response.Add("comprador", comprador);
                response.Add("cartao", cartao);
                response.Add("tabelaSelecionada", _tabelaSelecionada);
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
        public ActionResult DadosFaturamentoInit()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _cmp_id = SessionContext.GetInSession<int>("_CMP_ID");
                var _cmp_origem = SessionContext.GetInSession<int>("_CMP_ID_ORIGEM");
                var _ttp_id = SessionContext.GetInSession<int>("_TTP_ID");

                var _tabelapreco = new ProdutoComposicaoSRV();
                var _tabelaSelecionada = _tabelapreco.FindByProdOrigem(_cmp_origem, _ttp_id);
                var comprador = SessionContext.GetInSession<CompradorDTO>("comprador");
                comprador.cmp_id = _cmp_id;

                response.Add("comprador", comprador);
                response.Add("tabelaSelecionada", _tabelaSelecionada);
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
        public ActionResult BuscarCep(string _cep_numero)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _cepdto = new CepLogradouroSRV().BuscarCep(_cep_numero);

                response.Add("cep", _cepdto);
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
        public ActionResult EnviarDadosPagamento(CompradorDTO _comprador, CartaoCreditoDTO _cartao)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _erro = "";

                if (_comprador.nome == null)
                    _erro = "<li>Informe o nome </li>";

                if (_comprador.cidade == null)
                    _erro = "<li>Informe a Cidade </li>";

                if (_comprador.endereco == null)
                    _erro += "<li>Informe o endereço </li>";

                if (_comprador.numero == null)
                    _erro += "<li>Informe o numero </li>";
                
                if (_comprador.CEP == null)
                    _erro += "<li>Informe o CEP </li>";

                if (_comprador.email == null)
                    _erro += "<li>Informe o email </li>";
                
                if (_comprador.numeroDocumento == null)
                    _erro += "<li>Informe o CPF/CNPJ </li>";

                if (_comprador.foneComercial == null)
                    _erro += "<li>Informe o telefone </li>";

                if (!String.IsNullOrWhiteSpace(_erro))
                    throw new Exception("<ul>"+_erro+"</ul>");

                SessionContext.PutInSession("comprador", _comprador);
                SessionContext.PutInSession("cartao", _cartao);

                response.message = Message.Info("OK");
                response.success = true;

                //var _pedido = this.SalvarPedido(_comprador, _cartao);

                //var _ipe_id = _pedido.idpedido;

                //var _ipevalidahash = _pedido.chavepedido;
                 
                //var _urlpagamento =  new PedidoCRMSRV().RetornarURLPagamento((int)_ipe_id);

                //response.Add("urlpagamento", _urlpagamento);
                //response.success = true;

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
                if (_cartao.numeroParcelas < 1)
                    _cartao.numeroParcelas = 1;


                var _retorno = new CheckOutProxySRV().SalvarPedido(_comprador, _cartao);

                CartaoIntegracaoDTO _cIntegracao = new CartaoIntegracaoDTO();

                _cIntegracao.ipeId = _retorno.idpedido;
                _cIntegracao.ipeHash = _retorno.chavepedido;
                _cIntegracao.valor = _cartao.valor;
                _cIntegracao.bandeira = _cartao.bandeira;
                _cIntegracao.numeroCartao = _cartao.numeroCartao;
                _cIntegracao.mesExpiracao = _cartao.mesExpiracao;
                _cIntegracao.anoExpiracao = _cartao.anoExpiracao;
                _cIntegracao.portador = _cartao.portador;
                _cIntegracao.codigoSeguranca = _cartao.codigoSeguranca;
                _cIntegracao.meioDePagamento = SysUtils.BuscarCodAdiquirente();
                
                ServiceReference1.CoadServiceClient proxy = new ServiceReference1.CoadServiceClient("BasicHttpBinding_ICoadService");

                var pedidoSalvo = proxy.CheckoutPagamento(_cIntegracao);

                if (pedidoSalvo != null)
                    new Exception(pedidoSalvo);


            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

    }
}

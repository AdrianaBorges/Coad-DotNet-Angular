using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ImportacaoSuspect;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.SessionUtils;
using COAD.GATEWAY.MUNDIPAGG.Model;
using COAD.PROXY.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Exceptions;
using GenericCrud.Service;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

namespace COADSERVICE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CoadService : ICoadService, ICoadServiceRest
    {
        public PedidoCRMSRV PedidoSRV { get; set; }
        public ClienteUsuarioSRV ClienteUsuario { get; set; }
        public ClienteIntegracaoSRV ClienteIntegr { get; set; }
        public DadosClientePortalSRV dadosClientePortalSRV { get; set; }
        public ClienteSRV _clienteSRV { get; set; }

        public string ValidarCliente(string usuario, string senha)
        {
            //cont - Portal Coad
            //adv  
            // permissao:00| ---> Produto
            // tipo_usuario -->> COAD
            // Contador -- > 0
            // Pesquisa --> 0

            string _retorno = null;
            try
            {
                AutenticacaoLoginUnicoDTO loginRequest = this.LoginPortal(usuario, senha);
                AssinaturaDTO _assinatura = loginRequest.AssinaturaPrincipal;

                DateTime _datacadastro;
                string _email = null;

                if (_assinatura.CLIENTES != null)
                {
                    var cliente = _assinatura.CLIENTES;

                    _datacadastro = (DateTime)cliente.DATA_CADASTRO;

                    var _emailObj = ServiceFactory.RetornarServico<AssinaturaEmailSRV>()
                        .RetornarEmailDeContato(cliente.CLI_ID);

                    if (_emailObj != null)
                        _email = _emailObj.AEM_EMAIL;

                    var _endereco = new ClienteEnderecoDto();

                    foreach (var _item in _assinatura.CLIENTES.CLIENTES_ENDERECO.OrderByDescending(x => x.END_TIPO))
                    {
                        _endereco = _item;
                    }


                    _retorno = "erro:0|id:" + _assinatura.CLIENTES.CLI_ID.ToString() + "|" +
                               "perfil:" + _assinatura.ASN_PERFIL + "|" +
                               "usuario:" + usuario + "|" +
                               "senha:" + senha + "|" +
                               "permissao:" + _assinatura.ASN_PERMISSAO_PORTAL + "|" +
                               "status:L|" +
                               "cpf:" + _assinatura.CLIENTES.CLI_CPF_CNPJ + "|" +
                               "nome:" + _assinatura.CLIENTES.CLI_NOME + "|" +
                               "sobrenome:" + _assinatura.CLIENTES.CLI_NOME + "|" +
                               "empresa:0|" +
                               "email:" + _email + "|" +
                               "endereco:" + _endereco.END_LOGRADOURO + "|" +
                               "numero:" + _endereco.END_NUMERO + "|" +
                               "complemento:" + _endereco.END_COMPLEMENTO + "|" +
                               "bairro:" + _endereco.END_BAIRRO + "|" +
                               "cep:" + _endereco.END_CEP + "|" +
                               "cidade:" + _endereco.END_MUNICIPIO + "|" +
                               "estado:" + _endereco.END_UF + "|" +
                               "telefone:|" +
                               "conhecimento:|" +
                               "public:|" +
                               "vigencia:|" +
                               "trab:|profissao:|sexo:|data_nascimento:|area_atuacao:|" +
                               "receber_novidades:1|cadastrado:0|" +
                               "data1:|expiracao:" + _datacadastro.ToString("yyyy-MM-dd") + "|" +
                               "tipo_usuario:" + _assinatura.ASN_TIPO + "|" +
                               "contador:0|dataCadastro:" + _datacadastro.ToString("yyyy-MM-dd") + "|data_atualizacao:|cnpj:|id_estacio:0|oab_flag:0|" +
                               "oab_nr_inscricao:|oab_status:|dt_ultimo_login:|" +
                               "qtd_sessoes:1|oab_nr_ficha:|data_repositorio:2014-01-01 01:01:01|pesquisa:1";

                    if (loginRequest.LogadoPorLoginUnico &&
                        loginRequest.Assinaturas != null &&
                        loginRequest.Assinaturas.Count() > 0)
                    {
                        StringBuilder sb = new StringBuilder(_retorno);
                        sb.Append("|loginUnico:1");
                        sb.Append("|Assinaturas:");

                        int index = 0;
                        int count = loginRequest.Assinaturas.Count();

                        foreach (var assinatura in loginRequest.Assinaturas)
                        {
                            if (index > 0 && index <= count)
                            {
                                sb.Append(";");
                            }
                            sb.Append(assinatura.CodAssinatura);
                            sb.Append(",");
                            sb.Append(assinatura.SenhaAssinatura);

                            index++;
                        }

                        _retorno = sb.ToString();
                    }
                    else
                    {
                        _retorno = "|loginUnico:0";
                    }
                }
                else
                {
                    _retorno = "erro:Cliente não encontrado !!";
                }

                return _retorno;
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                return "erro:" + SysException.Show(ex);
            }
        }

        public LoginUnicoResultDTO RealizarLogin(string code, string encodecode)
        {
            //cont - Portal Coad
            //adv  
            // permissao:00| ---> Produto
            // tipo_usuario -->> COAD
            // Contador -- > 0
            // Pesquisa --> 0

            try
            {
                AutenticacaoLoginUnicoDTO loginRequest = this.LoginPortal(code, encodecode);
                AssinaturaDTO _assinatura = loginRequest.AssinaturaPrincipal;

                DateTime _datacadastro;
                string _email = null;

                if (_assinatura.CLIENTES != null)
                {
                    var cliente = _assinatura.CLIENTES;

                    _datacadastro = (DateTime)cliente.DATA_CADASTRO;

                    var _emailObj = ServiceFactory.RetornarServico<AssinaturaEmailSRV>()
                        .RetornarEmailDeContato(cliente.CLI_ID);

                    if (_emailObj != null)
                        _email = _emailObj.AEM_EMAIL;

                    var _endereco = new ClienteEnderecoDto();

                    foreach (var _item in _assinatura.CLIENTES.CLIENTES_ENDERECO.OrderByDescending(x => x.END_TIPO))
                    {
                        _endereco = _item;
                    }

                    LoginUnicoResultDTO result = new LoginUnicoResultDTO()
                    {
                        Cliente = new LoginUnicoClienteDTO()
                        {
                            Id = _assinatura.CLIENTES.CLI_ID,
                            CPF = _assinatura.CLIENTES.CLI_CPF_CNPJ,
                            Nome = _assinatura.CLIENTES.CLI_NOME,
                            Email = _email,
                            Endereco = new LoginUnicoEnderecoClienteDTO()
                            {
                                Logradouro = _endereco.END_LOGRADOURO,
                                Numero = _endereco.END_NUMERO,
                                Complemento = _endereco.END_COMPLEMENTO,
                                Bairro = _endereco.END_BAIRRO,
                                CEP = _endereco.END_CEP,
                                Cidade = _endereco.END_MUNICIPIO,
                                UF = _endereco.END_UF
                            }

                        },
                        Perfil = _assinatura.ASN_PERFIL,
                        Usuario = code,
                        Senha = encodecode,
                        Permisao = _assinatura.ASN_PERMISSAO_PORTAL,
                        Status = "L",
                        Empresa = 0,
                        DataCadastro = _datacadastro,
                        TipoUsuario = _assinatura.ASN_TIPO,
                        DataRepositorio = new DateTime(2014, 01, 01),
                        Pesquisa = true

                    };

                    if (loginRequest.LogadoPorLoginUnico &&
                        loginRequest.Assinaturas != null &&
                        loginRequest.Assinaturas.Count() > 0)
                    {

                        result.LoginUnico = true;
                        result.Assinaturas =
                            loginRequest.Assinaturas
                                .Select(op => new LoginUnicoInfoAssinaturaDTO()
                                {
                                    CodAssinatura = op.CodAssinatura,
                                    Senha = op.SenhaAssinatura
                                }).ToList();
                    }

                    return result;
                }
                else
                {
                    return new LoginUnicoResultDTO()
                    {
                        Sucesso = false,
                        Mensagem = new LoginUnicoMensagemDTO()
                        {

                            Tipo = "Erro",
                            Mensagem = "Cliente não encontrado !!"
                        }
                    };
                }

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                return new LoginUnicoResultDTO()
                {
                    Sucesso = false,
                    Mensagem = new LoginUnicoMensagemDTO()
                    {

                        Tipo = "Erro",
                        Mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(ex)
                    }
                };
            }
        }

        public AutenticacaoLoginUnicoDTO LoginPortal(string code, string encodecod, IList<AssinaturaDTO> lstAssinaturas = null)
        {
            AssinaturaSenhaDTO _senha = new AssinaturaSenhaDTO();
            AssinaturaDTO _assinatura = new AssinaturaDTO();
            lstAssinaturas = new List<AssinaturaDTO>();

            AutenticacaoLoginUnicoDTO result = new AutenticacaoLoginUnicoDTO();
            try
            {
                result.Success = true;
                ClienteUsuarioDTO clienteUsuario = ClienteUsuario.LogarCliente(code, encodecod);

                if (clienteUsuario != null)
                {
                    ClienteDto cliente = clienteUsuario.CLIENTES;
                    _assinatura = clienteUsuario.ASSINATURA;
                    _preencherAssinaturaELogar(_assinatura, cliente, code);

                    result.LogadoPorLoginUnico = true;
                    result.AssinaturaPrincipal = _assinatura;
                    result.Assinaturas = clienteUsuario.Assinaturas;


                    return result;
                }
                else
                {
                    AssinaturaSenhaSRV _assSRV = new AssinaturaSenhaSRV();


                    _senha = _assSRV.BuscarSenhaAtiva(code);

                    if (_senha != null)
                    {

                        string _senhaHash = SessionContext.HashMD5(_senha.ASN_SENHA);

                        if (_senha.ASN_NUM_ASSINATURA != code || _senhaHash != encodecod)
                            throw new Exception("Usuário não autorizado ou não logado no sistema. Atualize a tela para continuar acessando esta funcionalidade.");

                        if (code == null || encodecod == null)
                            throw new Exception("Usuário não autorizado ou não logado no sistema. Verifique seu login e senha.");

                        _assinatura = new AssinaturaSRV().FindByIdFullLoaded(_senha.ASN_NUM_ASSINATURA, true, true);

                        var _cliente = new ClienteSRV().FindByIdFullLoaded((int)_assinatura.CLI_ID, false, false, false, true);

                        _preencherAssinaturaELogar(_assinatura, _cliente, code);

                    }
                    else
                    {
                        var _gratuito = new CadastroGratuitoSRV().RealizarLogin(code, encodecod, "COADCORP", System.Web.HttpContext.Current);

                        var _assinaturatel = new AssinaturaTelefoneDTO();
                        _assinaturatel.ATE_TELEFONE = _gratuito.CGR_TELEFONE;

                        var _assinaturaemail = new AssinaturaEmailDTO();
                        _assinaturaemail.AEM_EMAIL = _gratuito.CGR_EMAIL;

                        var _endereco = new ClienteEnderecoDto();
                        _endereco.END_LOGRADOURO = _gratuito.CGR_ENDERECO;
                        _endereco.END_NUMERO = _gratuito.CGR_NUMERO;
                        _endereco.END_COMPLEMENTO = _gratuito.CGR_COMPLEMENTO;
                        _endereco.END_BAIRRO = _gratuito.CGR_BAIRRO;
                        _endereco.END_CEP = _gratuito.CGR_CEP;
                        _endereco.END_MUNICIPIO = _gratuito.CGR_MUNICIPIO;
                        _endereco.END_UF = _gratuito.UF_SIGLA;

                        var _clientes = new ClienteDto();
                        _clientes.CLI_ID = 0;
                        _clientes.CLI_CPF_CNPJ = _gratuito.CGR_CPF_CNPJ;
                        _clientes.CLI_NOME = _gratuito.CGR_NOME;
                        _clientes.CLIENTES_ENDERECO.Add(_endereco);
                        _clientes.DATA_CADASTRO = DateTime.Now;

                        _clientes.CLIENTES_ENDERECO = new List<ClienteEnderecoDto>();
                        _assinatura.ASSINATURA_TELEFONE = new List<AssinaturaTelefoneDTO>();
                        _assinatura.ASSINATURA_EMAIL = new List<AssinaturaEmailDTO>();

                        _assinatura.ASSINATURA_TELEFONE.Add(_assinaturatel);
                        _assinatura.ASSINATURA_EMAIL.Add(_assinaturaemail);
                        _assinatura.CLIENTES = _clientes;
                        _assinatura.ASN_TIPO = _gratuito.CRG_TIPO_USUARIO;
                        _assinatura.ASN_PERFIL = _gratuito.CGR_PERFIL;
                        _assinatura.ASN_DATA_EXPIRA = _gratuito.CGR_DATA_EXPIRA;
                        _assinatura.ASN_PERMISSAO_PORTAL = _gratuito.CGR_PERMISSAO;

                    }
                    result.Success = true;
                    result.AssinaturaPrincipal = _assinatura;
                    return result;
                }

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado, true);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut, true);
                }
                result.Message = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);

                return result;

            }

        }

        /// <summary>
        /// Preenche a assinatura com os dados necessários para o login e realiza a autenticação.
        /// </summary>
        /// <param name="_assinatura"></param>
        /// <param name="_cliente"></param>
        /// <param name="code"></param>
        private void _preencherAssinaturaELogar(AssinaturaDTO _assinatura, ClienteDto _cliente, string code)
        {
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;

            _assinatura.CLIENTES = _cliente;
            _assinatura.ASN_TIPO = "COAD";
            _assinatura.ASN_DATA_EXPIRA = null;
            _assinatura.ASN_PERMISSAO_PORTAL = code.Substring(0, 2);

            var _perfil = new ProdutosSRV().ListarPerfil((int)_assinatura.PRO_ID);

            _assinatura.ASN_PERFIL = _perfil.PER_ID;

            //-----------------
            Autenticado _autenticado = new Autenticado();

            _autenticado.USU_LOGIN = code;
            _autenticado.IP_ACESSO = SessionContext.GetIp();
            _autenticado.PATH = url;
            _autenticado.SESSION_ID = System.Web.HttpContext.Current.Session.SessionID;
            _autenticado.SESSION_TIMEOUT = System.Web.HttpContext.Current.Session.Timeout;
            _autenticado.SESSION_TIMEOUT_RESTANTE = System.Web.HttpContext.Current.Session.Timeout;
            _autenticado.SIS_ID = "COADCORP";
            _autenticado.EMP_ID = 1;
            _autenticado.ADMIN = false;
            _autenticado.EMAIL = code;

            _autenticado.DATA_LOGIN = DateTime.Now;
            _autenticado.MEIO_ACESSO = "SERVICO";

            SessionContext.autenticado = _autenticado;
            SessionContext.sistemas_coad = new SistemaSRV().Listar();

            SessionContext.AddSessionGlobal(System.Web.HttpContext.Current); ;

            SysException.RegistrarLog("LogIn Usuário (" + _autenticado.USU_LOGIN + ")", "", _autenticado, true);

        }

        public DadosPedidoIntegracaoDTO GerarPedido(DadosPedidoIntegracaoDTO dados)
        {
            try
            {
                if (dados == null)
                {
                    throw new ArgumentNullException("Dados não informados corretamente.");
                }

                var _retorno = PedidoSRV.GerarPedido(dados);

                if (_retorno.erro > 0)
                {
                    dados.erro = _retorno.erro;
                    dados.mensagem = _retorno.mensagem;

                    throw new Exception(dados.mensagem);
                }

                dados.idpedido = (int)_retorno.ITEM_PEDIDO.FirstOrDefault().IPE_ID;
                dados.chavepedido = SessionContext.HashMD5(dados.idpedido.ToString());

                return dados;
            }
            catch
            {
                return dados;
            }
        }

        public string CheckoutPagamento(CartaoIntegracaoDTO dadospagamento)
        {
            try
            {
                if ((dadospagamento.ipeId == 0 || dadospagamento.ipeId == null) || String.IsNullOrWhiteSpace(dadospagamento.ipeHash))
                {
                    throw new ArgumentNullException("Dados não informados corretamente.");
                }

                var _retorno = new CheckOutProxySRV().CarregaPedidoPagamento(dadospagamento);

                if (_retorno.erro > 0)
                {
                    throw new Exception(_retorno.mensagem);
                }

                return _retorno.mensagem;

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                return ex.Message;
            }

        }

        public string LembrarSenha(string numAssinatura, int cliId, string email)
        {
            try
            {
                Autenticado _autenticado = new Autenticado();
                _autenticado.IP_ACESSO = SessionContext.GetIp();
                _autenticado.USU_LOGIN = "COADSERVICE";
                SessionContext.autenticado = _autenticado;

                new AssinaturaSRV().GerarSenhaEEnviarEmail(numAssinatura, cliId, email);

                return "Nova senha enviada com sucesso!!";

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                throw new Exception(ex.Message);

            }
        }

        public RenovacaoMalaResult EmitirPedidoRenovacaoDaMala(string numeroAssinatura, decimal valorPedido, int qtdParcelas)
        {
            RenovacaoMalaResult result = new RenovacaoMalaResult();
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var lstUrls = this.PedidoSRV.EmitirPedidoRenovacaoDaMala(numeroAssinatura, valorPedido, qtdParcelas);
                result.Result = lstUrls as List<string>;
                result.Success = true;

                watch.Stop();
                result.TempoDeExecucao = watch.ElapsedMilliseconds;

            }
            catch (Exception ex)
            {
                var msg = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);
                result.Success = false;
                result.Message = msg;
            }

            return result;
        }

        public ClienteIntegrResult RetornarDadosDoCliente(string numeroAssinatura, string senha)
        {
            ClienteIntegrResult result = new ClienteIntegrResult();

            var watch = new Stopwatch();
            watch.Start();

            try
            {
                var dadosCli = this.ClienteIntegr.RetornarDadosDoCliente(numeroAssinatura, senha);

                result.Cliente = dadosCli;
                result.Success = true;

            }
            catch (Exception ex)
            {
                var msg = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);
                result.Success = false;
                result.Message = msg;
            }
            finally
            {
                watch.Stop();
                result.TempoDeExecucao = watch.ElapsedMilliseconds;
            }

            return result;
        }

        public ValidatorWebServiceResult SalvarDadosDoCliente(ClienteIntegrDTO cliente)
        {
            ValidatorWebServiceResult result = new ValidatorWebServiceResult();
            try
            {
                var modelState = ValidatorProxy.RecursiveValidate(cliente);
                if (modelState.IsValid)
                {
                    ClienteIntegr.SalvarClienteIntegracao(cliente);
                    result.Success = true;
                    result.Message = Coad.GenericCrud.ActionResultTools.Message.Info("Dados do cliente atualizado com sucesso!!");
                    SysException.RegistrarLog("Dados do cliente atualizados com sucesso!!", "", SessionContext.autenticado);

                    return result;
                }
                else
                {
                    result.SetMessageFromModelState(modelState);
                    result.Success = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.Success = false;
                result.Message = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);
                return result;
            }
        }

        public BuscarCEPResult BuscarCep(string cep)
        {
            BuscarCEPResult result = new BuscarCEPResult();
            try
            {
                result.Endereco = ServiceFactory.RetornarServico<CepLogradouroSRV>().BuscarCepIntegracao(cep);
                result.Success = true;
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.Success = false;
                result.Message = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);
            }
            return result;
        }

        public BuscarMunicipioResult BuscarMunicipios(string uf, string descricao = null)
        {
            BuscarMunicipioResult result = new BuscarMunicipioResult();
            try
            {
                result.Municipios = ServiceFactory.RetornarServico<MunicipioSRV>().BuscarMunicipiosIntegracao(uf, descricao);
                result.Success = true;
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.Success = false;
                result.Message = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);
            }
            return result;
        }

        public BuscarMunicipioResult BuscarMunicipiosPorCodIBGE(string codIBGE)
        {
            BuscarMunicipioResult result = new BuscarMunicipioResult();
            try
            {
                result.Municipios =
                    new List<ClienteIntegrMunicipioDTO>(){
                        ServiceFactory.RetornarServico<MunicipioSRV>().BuscarMunicipiosIntegracaoPorCodIBGE(codIBGE)
                    };

                result.Success = true;
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.Success = false;
                result.Message = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);
            }
            return result;
        }

        public TipoClienteResult ListarTiposDeCliente()
        {
            TipoClienteResult result = new TipoClienteResult();
            try
            {
                result.TiposClientes =
                        ServiceFactory.RetornarServico<TipoClienteSRV>().BuscarTiposDeClienteIntegracao();

                result.Success = true;
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.Success = false;
                result.Message = Coad.GenericCrud.ActionResultTools.Message.Fail(ex);
            }
            return result;
        }

        public BuscarDadosClienteStResult BuscarDadosClienteSt(string assinatura, string senha)
        {

            BuscarDadosClienteStResult result = new BuscarDadosClienteStResult();
            try
            {
                result.dadosClienteStDTO = ServiceFactory.RetornarServico<DadosClientePortalSRV>().BuscarDadosClienteSt(assinatura, senha);
                result.Success = true;
            }
            catch (Exception e)
            {
                SessionUtil.HandleException(e);
                result.Success = false;
                result.Message = Coad.GenericCrud.ActionResultTools.Message.Fail(e);

            }
            return result;
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "teste")]
        public string[] teste()
        {
            return new string[] { "aieaji" };
        }

    }
}
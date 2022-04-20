using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Security;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.LEGADO.Model;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.AcaoSalvamento;
using COAD.CORPORATIVO.Model.Dto.Custons.Buscas;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using COAD.CORPORATIVO.Model.Dto.Custons.Validacoes;
using COAD.CORPORATIVO.Model.Dto.Transferencia;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Util;
using COAD.PORTAL.Model.DTO.Uras;
using COAD.PORTAL.Service.Uras;
using COAD.PROSPECTADOS.Model.Dto;
using COAD.PROSPECTADOS.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;
using GenericCrud.Service.Formatting;
using GenericCrud.Util;
using GenericCrud.Validations;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CLI_ID")]
    public class ClienteSRV : ServiceAdapter<CLIENTES, ClienteDto>
    {
        public ClienteDAO _dao { get; set; }
        public ClienteEnderecoSRV _endSRV { get; set; } 
        public InfoMarketingSRV _infMarketingSRV { get; set; } 
        public ClienteTelefoneSRV _clienteTelSRV { get; set; } 
        public CarteiramentoSRV _carteiramentoSRV { get; set; }
        public CarteiraRepresentanteSRV _carteiramentoRepresentanteSRV { get; set; } 
        public ContratoSRV _contratoSRV { get; set; }
        public NotificacoesSRV _notificacaoSRV { get; set; } 
        public RepresentanteSRV _representanteSRV { get; set; } 
        public PrioridadeAtendimentoSRV _prioridadeAtendimento { get; set; } 
        public HistAtendSRV _histAtendSRV { get; set; }         
        public CarteiraClienteSRV _carteiraClienteSRV { get; set; } 
        public AssinaturaTelefoneSRV _assinaturaTelefone { get; set; } 
        public AssinaturaEmailSRV _assinaturaEmail { get; set; } 
        public AssinaturaSRV _assinaturaSRV { get; set; } 
        public AgendamentoSRV _agendamentoSRV { get; set; } 
        public CartCoadSRV _cartCoadSRV { get; set; } 
        public cart_coadSRV _cartCoadLegadoSRV { get; set; } 
        public MessageFormatterService formatterService { get; set; }
        public ClienteSRV(ClienteDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public ClienteSRV()
        {
            _dao = new ClienteDAO();
            _endSRV = new ClienteEnderecoSRV();
            _infMarketingSRV = new InfoMarketingSRV();
            _clienteTelSRV = new ClienteTelefoneSRV();
            _carteiramentoSRV = new CarteiramentoSRV();
            _carteiramentoRepresentanteSRV = new CarteiraRepresentanteSRV();
            _notificacaoSRV = new NotificacoesSRV();
            _representanteSRV = new RepresentanteSRV();
            _prioridadeAtendimento = new PrioridadeAtendimentoSRV();
            _histAtendSRV = new HistAtendSRV();
            _carteiraClienteSRV = new CarteiraClienteSRV();
            _assinaturaTelefone = new AssinaturaTelefoneSRV();
            _assinaturaEmail = new AssinaturaEmailSRV();
            _assinaturaSRV = new AssinaturaSRV();
            _agendamentoSRV = new AgendamentoSRV();
            _cartCoadSRV = new CartCoadSRV(); //--- CartCoad do propectado -- Atende a agenda
            _cartCoadLegadoSRV = new cart_coadSRV();
            MessageFormatterService formatterService = new MessageFormatterService();
            _contratoSRV = new ContratoSRV();
            SetDao(_dao);
        }

        /// <summary>
        /// ALT: 26/07/2017 - Validar CNPJ
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public bool ChecarCnpjValido(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// ALT: 26/07/2017 - Validar CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public bool ChecarCpfValido(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        //
        public IList<RelContratosClienteAreaDTO> ListarClientesProduto(int _mes, int _ano)
        {
            return _dao.ListarClientesProduto(_mes, _ano);
        }
        public void AlterarSenha(ClienteDto usuario)
        {
            new ClienteDAO().AlterarSenha(usuario);
        }
        public ClienteDto BuscarPorCNPJ(string _cli_cnpj)
        {
            return _dao.BuscarPorCNPJ(_cli_cnpj);
        }
        public ClienteDto BuscarClienteComContratoPorCpfCnpj(string cpfCnpj)
        {
            var clienteContratoValido = _dao.BuscarClienteUltimoContratoValidoPorCpfCnpj(cpfCnpj);
            if (clienteContratoValido == null)
            {
                clienteContratoValido = _dao.BuscarClienteUltimoContratoPorCpfCnpj(cpfCnpj);
            }
            if (clienteContratoValido == null)
            {
                throw new Exception("Cliente sem contrato");
            }

            if(clienteContratoValido.CLI_ID != null)
            {
               var enderecoEntrega = _endSRV.FindEnderecoCliente(clienteContratoValido.CLI_ID.GetValueOrDefault(), 1);

               var enderecoFaturamento = _endSRV.FindEnderecoCliente(clienteContratoValido.CLI_ID.GetValueOrDefault(), 2);

                clienteContratoValido.ENDERECO_ENTREGA = enderecoEntrega;
                clienteContratoValido.ENDERECO_FATURAMENTO = enderecoFaturamento;
            }

            return clienteContratoValido;
        }
        public IList<ClienteDto> BuscarClientesPorCpfCnpj(string _cpf_cnpj)
        {
            var clientes = _dao.BuscarUltimoContratoPorCpfCnpj(_cpf_cnpj);
            
            if (clientes == null)
            {
                throw new Exception("Não foi encontrado cliente para este dado informado.");
            }
            if (clientes.Count == 0)
            {
                throw new Exception("Não foi encontrado cliente para este dado informado.");
            }
            return clientes;
        }
        public string Cadastrar(string _cliportaldto)
        {
            return "";
        }
        public Pagina<ClienteCustomDTO> ClientesAtivos(  int? _grupo_id
                                                       , int _vigencia = 0
                                                       , int _atraso = 0
                                                       , bool _quitado = false
                                                       , int? _qtdecontratos = null
                                                       , string _anocoad = null
                                                       , string _uf = null
                                                       , int pagina = 1
                                                       , int registroPorPagina = 10)
        {
            return _dao.ClientesAtivos(_grupo_id, _vigencia, _atraso,  _quitado, _qtdecontratos, _anocoad, _uf, pagina, registroPorPagina);
        }
        public List<ClienteCustomDTO> ClientesAtivosLista(int? _grupo_id
                                                        , int _vigencia = 0
                                                        , int _atraso = 0
                                                        , bool _quitado = false
                                                        , int? _qtdecontratos = null
                                                        , string _anocoad = null
                                                        , string _uf = null)
        {
            return _dao.ClientesAtivosLista(_grupo_id, _vigencia, _atraso, _quitado, _qtdecontratos, _anocoad, _uf);
        }

        public ClienteDto BuscarClientePorBoleto(string _parcela)
        {
            return _dao.BuscarClientePorBoleto(_parcela).FirstOrDefault();
        }

        public string BuscarEmailSistema(int _cli_id)
        {
            try
            {
                return _dao.BuscarEmailSistema(_cli_id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            } 

        }
        public Boolean VerificarAssinaturaAtiva(string _assinatura)
        {
            return _dao.VerificarAssinaturaAtiva(_assinatura);
        }
        public void RealizarLogin(string _login, string _senha, int _origem, HttpContext _url)
        {
            string url = _url.Request.Url.AbsoluteUri;
            string path = _url.Request.Url.AbsolutePath;

            Autenticado _autenticado = new Autenticado();

            try
            {
                if (String.IsNullOrWhiteSpace(_login) || String.IsNullOrWhiteSpace(_senha))
                    throw new Exception("Login e/ou senha inválida !!");

                _autenticado = _dao.ValidarLogin(_login, _senha, _origem);

                if (_autenticado == null)
                    throw new Exception("Login e/ou senha inválida !!");

                var _outrasessao = SessionContext.FindSessionGlobal(_url, _login);

                if (_outrasessao != null)
                    SessionContext.RemoveSessionGlobal(_url, _outrasessao.SESSION_ID);

                _url.Session.Timeout = 240;

                _autenticado.IP_ACESSO = SessionContext.GetIp();
                _autenticado.PATH = url;
                _autenticado.SESSION_ID = _url.Session.SessionID;
                _autenticado.SESSION_TIMEOUT = _url.Session.Timeout;
                _autenticado.SESSION_TIMEOUT_RESTANTE = _url.Session.Timeout;
      

                SessionContext.autenticado = _autenticado;

                SessionContext.AddSessionGlobal(_url);

                FormsAuthentication.SetAuthCookie(_autenticado.USU_LOGIN, false);

                SysException.RegistrarLog("LogIn Usuário (" + _autenticado.USU_LOGIN + ")", "", _autenticado, true);

            }
            catch (DbEntityValidationException dbEx)
            {
                string _erro = "";

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _erro += String.Format("Erro ao realizar login: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                SysException.RegistrarLog(_erro, "", SessionContext.autenticado, true);

                throw new Exception(_erro);
            }
            catch (DbUpdateException e)
            {
                SysException.RegistrarLog(SysException.Show(e), SysException.ShowIdException(e), SessionContext.autenticado, true);

                throw new Exception(SysException.Show(e));
            }
            catch (EntityException ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado, true);

                throw new Exception(SysException.Show(ex));
            }
            catch (Exception ex)
            {
                _autenticado = new Autenticado();

                _autenticado.PATH = url;
                _autenticado.IP_ACESSO = SessionContext.GetIp();

                var msgerro = "Tentativa de Login (" + _login + ") " + SysException.Show(ex);

                SysException.RegistrarLog(msgerro, SysException.ShowIdException(ex), _autenticado, true);

                throw new Exception(msgerro);
            }
            
        }
        public void ExcluirEndereco (ClienteEnderecoDto _endereco)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    new ClienteEnderecoSRV().Delete(_endereco);

                    var _mensagem = "Exclusão de Endereço ("+ _endereco.END_LOGRADOURO + " - " + _endereco.END_NUMERO + " / " + _endereco.END_UF+" - "+_endereco.MUN_ID.ToString()+" )"; 

                    this.GravarHistorico(3, _endereco.CLI_ID, null, _mensagem, 100);

                    scope.Complete();

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

                throw new Exception(SysException.Show(ex));
            }
            
        }
        public ClienteDto ValidarLoginEmail(string _email)
        {
            try
            {
                return new ClienteDAO().ValidarLoginEmail(_email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ClienteDto VerificarCPFCNPJ(string _cpfcnpj)
        {
            try
            {
                return new ClienteDAO().VerificarCPFCNPJ(_cpfcnpj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Pagina<ClienteDto> Clientes(
            string cpf_cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 7, int? representanteId = null, int? uen_id = null, int? classificacaoClienteId = null,
            string CAR_ID = null,
            int? RG_ID = null,
            string email = null,
            string representante = null)
        {
            return _dao.Clientes(cpf_cnpj, nome, pagina, registroPorPagina, representanteId, uen_id, classificacaoClienteId, CAR_ID, RG_ID, email, representante);
        }

        public Pagina<ClienteDto> ListaClientesContrato(int cliente = 0,
                                                     string contrato = null,
                                                     string pedido = null,
                                                     string assinatura = null,
                                                     string cpf_cnpj = null,
                                                     string nome = null, int pagina = 1, int registroPorPagina = 7, Boolean somenteativos = false)
        {
            return _dao.ListaClientesContrato(cliente, contrato, pedido, assinatura, cpf_cnpj, nome, pagina, registroPorPagina, somenteativos);
        }

        public Pagina<BuscarClienteDTO> BuscarClientes2(
            PesquisaClienteDTO pesquisaDTO)
        {
            return null;
        }

            /// <summary>
            /// Buscar todos os cliente de acordo com os Filtros
            /// </summary>
            /// <param name="cpf_cnpj">Filtro cpf_cnpj.</param>
            /// <param name="nome"></param>
            /// <param name="uen_id"></param>
            /// <param name="classificacaoClienteId"></param>
            /// <param name="CAR_ID"></param>
            /// <param name="RG_ID"></param>
            /// <param name="email"></param>
            /// <param name="representante"></param>
            /// <param name="pagina"></param>
            /// <param name="registroPorPagina"></param>
            /// <returns></returns>
            public Pagina<BuscarClienteDTO> BuscarClientes(
            PesquisaClienteDTO pesquisaDTO)
        {

            if (!string.IsNullOrWhiteSpace(pesquisaDTO.nome) && pesquisaDTO.nome.Trim().Length < 3)
            {
                throw new NegocioException("Não é possível filtrar por nome com menos de 3 letras");
            }
            Pagina<BuscarClienteDTO> clientes = _dao.BuscarClientes(pesquisaDTO);

            if (pesquisaDTO.buscarForaDaAgenda)
            {
                MarcarClienteComoForaDaAgenda(clientes.lista);
            }
            return clientes;
        }       

        public Pagina<ClienteDto> ListaClientes(int cliente = 0, string contrato = null, string pedido = null, string assinatura = null, string logradouro = null, string cpf_cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 7)
        {
            return _dao.ListaClientes(cliente, contrato, pedido, assinatura, logradouro, cpf_cnpj, nome, pagina, registroPorPagina);
        }
        
        /// <summary>
        /// Salva os dados do Cliente,Endereço, Email e Telefones 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public void SalvarCliente(ClienteDto _cliente, string _assinatura)
        {
            try
            {
                this.SincronizarTipoPessoa(_cliente);

                AssinaturaTelefoneSRV _srvAssTel = new AssinaturaTelefoneSRV();
              
                ClienteEnderecoSRV _srvClienteEndereco = new ClienteEnderecoSRV();

                using (TransactionScope scope = new TransactionScope())
                {

                     //-----------
                     //--- Atualiza os dados do Cliente
                     //-----------

                    ClienteDto _cli = this.FindById(_cliente.CLI_ID);

                    //----
                    this.GravarHistorico<ClienteDto>(_cli, _cliente, 3, _cliente.CLI_ID, 4, _assinatura);
                    //----

                   if (SessionContext.autenticado.USU_LOGIN != "" && SessionContext.autenticado.USU_LOGIN != null)
                        _cliente.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    else
                        _cliente.USU_LOGIN = SessionContext.usu_login_desktop;

                    if (_cli == null)
                    {
                        _cliente.CLA_CLI_ID = 3;
                        _cli = this.Save(_cliente);
                    }
                    else
                        _cli = this.Merge(_cliente, "CLI_ID");


                    //-----------
                    //--- Realiza a inclusão ou alteração dados do cliente (Corpotativo Legado)
                    //-----------
                    
                    var _cli_idLegado = _cli.CLI_ID.ToString().PadLeft(8, '0');
                    
                    Boolean _existe = false;

                    if (_cli_idLegado.Length < 9)
                    {
                        clienteLegDTO _clienteLeg = new clienteLegSRV().FindById(_cli_idLegado);

                        _existe = (_clienteLeg != null);

                        if (!_existe)
                            _clienteLeg = new clienteLegDTO();

                        _clienteLeg.CODIGO = _cli_idLegado;
                        _clienteLeg.CGC = _cli.CLI_CPF_CNPJ;
                        _clienteLeg.INSCRICAO = _cli.CLI_INSCRICAO;


                        if (_existe)
                        {
                            _clienteLeg.DATA_INSERT = DateTime.Now;
                            new clienteLegSRV().Merge(_clienteLeg, "CODIGO");
                        }
                        else
                        {
                            _clienteLeg.DATA_ALTERA = DateTime.Now;
                            new clienteLegSRV().Save(_clienteLeg);
                        }
                    }

                    //-----------
                    //--- Realiza a inclusão ou alteração dos endereços
                    //-----------
               
                    foreach (ClienteEnderecoDto _item in _cliente.CLIENTES_ENDERECO)
                    {
                        if (_item.CLI_ID == null)
                            _item.CLI_ID = _cli.CLI_ID;
                                   
                        _existe = false;

                        switch (_item.END_TIPO)
                        {
                            case 1:
                                cart_coadDTO _cart_coad = new cart_coadSRV().FindById(_cli_idLegado);
                                
                                _existe = (_cart_coad != null);

                                if (_cli_idLegado.Length < 9)
                                {
                                    if (!_existe)
                                        _cart_coad = new cart_coadDTO();

                                    _cart_coad.CODIGO = _cli_idLegado;
                                    _cart_coad.NOME = _cliente.CLI_NOME;
                                    _cart_coad.TIPO = null;
                                    _cart_coad.TIPO_COMPL = null;
                                    _cart_coad.TIPO_COMPL2 = null;
                                    _cart_coad.TIPO_COMPL3 = null;
                                    _cart_coad.COMPL2 = null;
                                    _cart_coad.COMPL3 = null;

                                    _cart_coad.IDENTIFICACAO = "C";
                                    _cart_coad.TP_PESSOA = _cliente.CLI_TP_PESSOA;
                                    _cart_coad.LOGRAD = _item.END_LOGRADOURO;
                                    _cart_coad.NUMERO = _item.END_NUMERO;
                                    _cart_coad.COMPL = _item.END_COMPLEMENTO;
                                    _cart_coad.BAIRRO = _item.END_BAIRRO;
                                    _cart_coad.MUNIC = _item.END_MUNICIPIO;
                                    _cart_coad.UF = _item.END_UF;
                                    _cart_coad.CEP = _item.END_CEP;

                                    if (_existe)
                                    {
                                        _cart_coad.DATA_INSERT = DateTime.Now;
                                        new cart_coadSRV().Merge(_cart_coad, "CODIGO");
                                    }
                                    else
                                    {
                                        _cart_coad.DATA_ALTERA = DateTime.Now;
                                        new cart_coadSRV().Save(_cart_coad);
                                    }
                                }

                                break;

                            case 2:
                                ender_fatDTO _end_legado = new ender_fatSRV().FindById(_cli_idLegado);
                              
                                _existe = (_end_legado !=  null);

                                if (_cli_idLegado.Length < 9)
                                {

                                    if (!_existe)
                                        _end_legado = new ender_fatDTO();

                                    _end_legado.CODIGO = _cli_idLegado;
                                    _end_legado.TIPO_FAT = null;
                                    _end_legado.END_FAT = _item.END_LOGRADOURO;
                                    _end_legado.NUM_FAT = _item.END_NUMERO;
                                    _end_legado.COMPL_FAT = _item.END_COMPLEMENTO;
                                    _end_legado.BAIRRO_FAT = _item.END_BAIRRO;
                                    _end_legado.MUNIC_FAT = _item.END_MUNICIPIO;
                                    _end_legado.UF_FAT = _item.END_UF;
                                    _end_legado.CEP_FAT = _item.END_CEP;

                                    if (_existe)
                                        new ender_fatSRV().Merge(_end_legado, "CODIGO");
                                    else
                                        new ender_fatSRV().Save(_end_legado);

                                }

                                break;
                        }

                        //-------------------

                    }

                    _srvClienteEndereco.SalvarEnderecos(_cliente.CLIENTES_ENDERECO.AsQueryable(), _cliente.CLI_ID, _assinatura);

                    scope.Complete();
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

                throw new Exception(SysException.Show(ex));
            }
        }
        public void SalvarEmail(AssinaturaEmailDTO _email)
        {
            AssinaturaEmailSRV _srvAssEmail = new AssinaturaEmailSRV();

            try
            {
                AssinaturaTelefoneSRV _srvAssTel = new AssinaturaTelefoneSRV();

                using (TransactionScope scope = new TransactionScope())
                {
                    EmailsDTO _email_leg_grav = new EmailsDTO();
                    LEGADO.Service.EmailsSRV _email_legSRV = new LEGADO.Service.EmailsSRV();

                    if (_email != null)
                    {
                        var _assnatura = new AssinaturaSRV().FindById(_email.ASN_NUM_ASSINATURA);

                        EmailsDTO email_leg = new LEGADO.Service.EmailsSRV().FindById(_email.EMAIL_ID_LEGADO);
                        var _emailHistorico = "Email atualizado ( " + _email.AEM_EMAIL + ")";
                        this.GravarHistorico(3, _assnatura.CLI_ID, _email.ASN_NUM_ASSINATURA, _emailHistorico, 102);

                        if (email_leg == null)
                            email_leg = new EmailsDTO();

                        email_leg.ASSINATURA = _email.ASN_NUM_ASSINATURA;
                        email_leg.E_MAIL = _email.AEM_EMAIL;
                        if (_email.OPC_ID != null)
                            email_leg.SETOR = new OpcaoAtendimentoSRV().FindById(_email.OPC_ID).OPC_DESCRICAO;

                        if (email_leg.AUTOID > 0)
                        {
                            //---Grava informações no corporativo legado
                            email_leg.AUTOID = (int)_email.EMAIL_ID_LEGADO;
                            email_leg.DATA_ALTERA = DateTime.Now;
                            new LEGADO.Service.EmailsSRV().Merge(email_leg, "AUTOID");
                            //------------------------------------------                            
                        }
                        else
                        {
                            //---Grava informações no corporativo legado
                            email_leg.DATA_INSERT = DateTime.Now;
                            _email_leg_grav = new LEGADO.Service.EmailsSRV().Save(email_leg);
                            //------------------------------------------
                        }

                        _email.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                        _email.DATA_ALTERA = DateTime.Now;
                        _email.EMAIL_ID_LEGADO = _email_leg_grav.AUTOID;
                        _email = _srvAssEmail.SaveOrUpdate(_email);

                        
                    }

                    scope.Complete();
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

                throw new Exception(SysException.Show(ex));

            }
        }
        public void SalvarTelefone(AssinaturaTelefoneDTO _telefone)
        {
            try
            {
                AssinaturaTelefoneSRV _srvAssTel = new AssinaturaTelefoneSRV();

                using (TransactionScope scope = new TransactionScope())
                {

                //    AssinaturaTelefoneDTO _asstel = _srvAssTel.FindById(_telefone.ATE_ID);

                    if (_telefone != null)
                    {
                        var _assnatura = new AssinaturaSRV().FindById(_telefone.ASN_NUM_ASSINATURA);

                        Telefones2DTO tel_leg = new Telefones2SRV().FindById(_telefone.TEL_ID_LEGADO);
                        var _telHistorico = "Telefone atualizado ( "+ _telefone.ATE_DDD + " - " +_telefone.ATE_TELEFONE+")";
                        this.GravarHistorico(3, _assnatura.CLI_ID, _telefone.ASN_NUM_ASSINATURA, _telHistorico, 20);

                        if (tel_leg == null)
                            tel_leg = new Telefones2DTO();

                        tel_leg.ASSINATURA = _telefone.ASN_NUM_ASSINATURA;
                        tel_leg.DDD_TEL = _telefone.ATE_DDD;
                        tel_leg.TELEFONE = _telefone.ATE_TELEFONE;

                        if (_telefone.TIPO_TEL_ID != null)
                            tel_leg.TIPO = new TipoTelefoneSRV().FindById(_telefone.TIPO_TEL_ID).TIPO_TEL_DESCRICAO;

                        if (_telefone.OPC_ID != null)
                            tel_leg.SETOR = new OpcaoAtendimentoSRV().FindById(_telefone.OPC_ID).OPC_DESCRICAO;

                        if (tel_leg.id > 0)
                        {
                            //---Grava informações no corporativo legado
                            tel_leg.id = (int)_telefone.TEL_ID_LEGADO;
                            tel_leg.DATA_ALTERA = DateTime.Now;
                            tel_leg = new Telefones2SRV().Merge(tel_leg, "id");
                            //------------------------------------------
                        }
                        else
                        {
                            //---Grava informações no corporativo legado
                            tel_leg.DATA_INSERT = DateTime.Now;
                            tel_leg = new Telefones2SRV().Save(tel_leg);
                            //------------------------------------------
                        }

                        _telefone.TEL_ID_LEGADO = tel_leg.id;
                        _telefone.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                        _telefone.DATA_ALTERA = DateTime.Now;
                        _telefone = _srvAssTel.SaveOrUpdate(_telefone);

                    }

                    scope.Complete();
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

                throw new Exception(SysException.Show(ex));

            }
        }
        public void ExcluirTelefone(AssinaturaTelefoneDTO _telefone)
        {
            try
            {
                AssinaturaTelefoneSRV _srvAssTel = new AssinaturaTelefoneSRV();
                Telefones2SRV _srvAssTel2 = new Telefones2SRV();

                var _assinatura = new AssinaturaSRV().FindById(_telefone.ASN_NUM_ASSINATURA);
                var _cliente = new ClienteSRV().FindById(_assinatura.CLI_ID);

                using (TransactionScope scope = new TransactionScope())
                {

                    _srvAssTel.Delete(_telefone);
                    
                    var _tel_leg = _srvAssTel2.FindById(_telefone.TEL_ID_LEGADO);
                    
                    _srvAssTel2.Delete(_tel_leg);

                    var _telHistorico = "Telefone Excluido ( " + _telefone.ATE_DDD + " - " + _telefone.ATE_TELEFONE + ")";
                    this.GravarHistorico(3, _cliente.CLI_ID, _telefone.ASN_NUM_ASSINATURA, _telHistorico, 20);

                    scope.Complete();
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

                throw new Exception(SysException.Show(ex));

            }
        }
        public void ExcluirEmail(AssinaturaEmailDTO _email)
        {
            try
            {
             
                AssinaturaEmailSRV _srvAssEmail = new AssinaturaEmailSRV();
                LEGADO.Service.EmailsSRV _srvAssEmail2 = new LEGADO.Service.EmailsSRV();

                var _assinatura = new AssinaturaSRV().FindById(_email.ASN_NUM_ASSINATURA);
                var _cliente = new ClienteSRV().FindById(_assinatura.CLI_ID);

                using (TransactionScope scope = new TransactionScope())
                {

                    _srvAssEmail.Delete(_email);

                    var _email_leg = _srvAssEmail2.FindById(_email.EMAIL_ID_LEGADO);

                    _srvAssEmail2.Delete(_email_leg);

                    var _emailHistorico = "Email Excluído ( " + _email.AEM_EMAIL + ")";
                    this.GravarHistorico(3, _cliente.CLI_ID, _email.ASN_NUM_ASSINATURA, _emailHistorico, 102);

                    scope.Complete();
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

                throw new Exception(SysException.Show(ex));

            }
        }


        public void GravarHistorico<T>(T _clienteAtu, T _clienteAnt, int? _uen_id, int? _cli_id = null, int _tipo = 0, string _assinatura = null)
        {

            var _tabelaAnt = _clienteAnt.GetType();
            PropertyInfo[] _listacamposAnt = _tabelaAnt.GetProperties();

            if (_clienteAtu != null)
            {
                foreach (var _camposAnt in _listacamposAnt)
                {

                    var _campo = typeof(T).GetProperty(_camposAnt.Name);
                    var _ant = _campo.GetValue(_clienteAnt, null) as IComparable;
                    var _atu = _campo.GetValue(_clienteAtu, null) as IComparable;

                    if (_camposAnt.Name != "TIPO_COMP_ID" && _camposAnt.Name != "ALTERADO")
                    {
                        if (_atu != null)
                        {
                            if (!_atu.Equals(_ant))
                            {

                                var _descricao = "Alteração do Campo " + _camposAnt.Name;

                                HistoricoAtendimentoDTO _hist = new HistoricoAtendimentoDTO();
                                _hist.HAT_DATA_HIST = DateTime.Now;
                                _hist.ACA_ID = 1;
                                _hist.HAT_DATA_RESOLUCAO = DateTime.Now;
                                _hist.TIP_ATEND_ID = _tipo;
                                _hist.USU_LOGIN = SessionContext.login;
                                _hist.HAT_IMP_ETIQUETA = false;
                                _hist.HAT_DESCRICAO = _descricao;
                                _hist.UEN_ID = _uen_id;
                                _hist.CLI_ID = _cli_id;
                                _hist.ASN_NUM_ASSINATURA = _assinatura;
                                _hist.HAT_ORIGEM_ATEND = "TEL";
                                _hist.HAT_SOLICITANTE = SessionContext.login;

                                _histAtendSRV.Save(_hist);

                            }
                        }
                    }

                }
            }
            else
            {

                var _descricao = "Inclusão de registro " + _tabelaAnt.Name;

                HistoricoAtendimentoDTO _hist = new HistoricoAtendimentoDTO();
                _hist.HAT_DATA_HIST = DateTime.Now;
                _hist.ACA_ID = 1;
                _hist.HAT_DATA_RESOLUCAO = DateTime.Now;
                _hist.TIP_ATEND_ID = _tipo;
                _hist.USU_LOGIN = SessionContext.login;
                _hist.HAT_IMP_ETIQUETA = false;
                _hist.HAT_DESCRICAO = _descricao;
                _hist.UEN_ID = _uen_id;
                _hist.CLI_ID = _cli_id;
                _hist.ASN_NUM_ASSINATURA = _assinatura;
                _hist.HAT_ORIGEM_ATEND = "TEL";
                _hist.HAT_SOLICITANTE = SessionContext.login;

                _histAtendSRV.Save(_hist);

            }
        }

        public void GravarHistorico(int? _uen_id, int? _cli_id = null, string _asn_id = null, string _descricao = null, int _tipo = 0)
        {
            //var _usu_login = "COADSERVICE";

            string _usu_login = null;
            if (SessionContext.PossuiSessao())
            {
                if (SessionContext.login == "" || SessionContext.login == null)
                    throw new ValidacaoException("Aplicações não possui sessão.");//SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.login;
                    _usu_login = SessionContext.login;
                    //SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

            }
            HistoricoAtendimentoDTO _hist = new HistoricoAtendimentoDTO();
            _hist.HAT_DATA_HIST = DateTime.Now;
            _hist.ACA_ID = 1;
            _hist.HAT_DATA_RESOLUCAO = DateTime.Now;
            _hist.TIP_ATEND_ID = _tipo;
            _hist.USU_LOGIN =_usu_login;
            _hist.HAT_IMP_ETIQUETA = false;
            _hist.HAT_DESCRICAO = _descricao;
            _hist.UEN_ID = _uen_id;
            _hist.CLI_ID = _cli_id;
            _hist.HAT_ORIGEM_ATEND = "TEL";
            _hist.ASN_NUM_ASSINATURA = _asn_id;
            _hist.HAT_SOLICITANTE = _usu_login;

            new HistAtendSRV().Save(_hist);
            
        }

        
        /// <summary>
        /// Salva um suspect por rodizio
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        public ClienteDto SalvarClienteRodizio(ClienteDto cliente, int? REP_ID = null, bool pularValidacao = false, bool ignorarRodizio = false)
        {
            ClienteDto cli = null;

            int? regId = null;

            if (cliente != null)
            {
                if (cliente.RegiaoIdParaRodizio != null)
                {
                    regId = cliente.RegiaoIdParaRodizio;
                }
                else
                {
                    RepresentanteDTO representante = _representanteSRV.FindById(REP_ID);

                    if (representante != null)
                    {
                        regId = representante.RG_ID;
                    }
                }

                cli = SalvarClienteEInformacoesDeMarketing(cliente, REP_ID, regId, pularValidacao, ignorarRodizio);
            
            }
            return cli;
        }

        /// <returns></returns>
        public ClienteDto SalvarClienteEInformacoesDeMarketing(ClienteDto cliente, int? REP_ID = null, int? REGIAO_ID = null, bool pularValidacao = false, bool ignorarRodizio = false)
        {
            SalvarClienteResultDTO result = null;
            ClienteDto clienteResposta = null;
            using (TransactionScope scope = new TransactionScope())
            {
                result = SalvarClienteAgenda(cliente, REP_ID, REGIAO_ID, null, pularValidacao, ignorarRodizio);
                if (result.ResultadoValidacaoDuplicidade != null &&
                    result.ResultadoValidacaoDuplicidade.HasDuplication)
                {
                    throw new Exception(result.ResultadoValidacaoDuplicidade.ErrorMessage);
                }

                scope.Complete();

                if (result != null && result.Cliente != null)
                    clienteResposta = result.Cliente;
            }
            return clienteResposta;
        }

        public SalvarClienteResultDTO SalvarClienteAgenda(ClienteDto cliente, bool pularValidacao = false, bool ignorarRodizio = false)
        {
            var clienteResposta = SalvarClienteAgenda(cliente,null, null, null, pularValidacao, ignorarRodizio);
            return clienteResposta;
            
        }
        public SalvarClienteResultDTO SalvarClienteAgenda(
         ClienteDto cliente,
         int? REP_ID_DEMANDANTE = null,
         int? REGIAO_ID = null,
         int? REP_ID_ENCARTEIRAR = null,
         bool pularValidacao = false,
         bool ignorarRodizio = false)
        {
            SalvarClienteParamDTO param = new SalvarClienteParamDTO()
            {
                RepIdDemandante = REP_ID_DEMANDANTE,
                RegiaoId = REGIAO_ID,
                RepIdEncarteirar = REP_ID_ENCARTEIRAR,
                PularValidacao = pularValidacao,
                IgnorarRodizio = ignorarRodizio
            };

            return SalvarClienteAgenda(cliente, param);
        }



            /// <summary>
            /// Salva o cliente junto com suas informações de marketing
            /// </summary>
            /// <param name="cliente"></param>
            /// <returns></returns>
            public SalvarClienteResultDTO SalvarClienteAgenda(
            ClienteDto cliente, SalvarClienteParamDTO param)
        {
            if (cliente != null)
            {
                SalvarClienteResultDTO result = new SalvarClienteResultDTO();

                int? REP_ID_DEMANDANTE = null;
                int? REGIAO_ID = null;
                int? REP_ID_ENCARTEIRAR = null;
                bool pularValidacao = false;
                bool ignorarRodizio = false;
                bool atualizarEncarteiramentos = false;

                if(param != null)
                {
                    REP_ID_DEMANDANTE = param.RepIdDemandante;
                    REGIAO_ID = param.RegiaoId;
                    REP_ID_ENCARTEIRAR = param.RegiaoId;
                    pularValidacao = param.PularValidacao;
                    ignorarRodizio = param.IgnorarRodizio;
                    atualizarEncarteiramentos = param.AtualizarEncarteiramentos;
                }
                
                    ClienteDto cli = null;
                //ChecaEmailTelefone(cliente);

                if (!pularValidacao && (cliente.CLI_EXCLUIDO_VALIDACAO == null || cliente.CLI_EXCLUIDO_VALIDACAO == false))
                {
                    ChecaDuplicacaoClienteComErro(cliente, result, param);

                    if (result.ResultadoValidacaoDuplicidade != null && result.ResultadoValidacaoDuplicidade.HasDuplication)
                    {
                        PreencherClientesConflitoReferencia(result);
                        return result;
                    }
                }

                AtribuirTipoDeCliente(cliente);

                if (cliente.CLI_ID != null)
                {
                    cliente.DATA_ALTERA = DateTime.Now;
                    cli = Merge(cliente, "CLI_ID"); // salvo o cliente                        
                }
                else
                {
                    cliente.DATA_CADASTRO = DateTime.Now;
                    cli = Save(cliente); //insiro no banco
                    cliente.CLI_ID = cli.CLI_ID; // pego o id do cliente para utili-lo como referência para a inserção das outras entidades

                    //if (REP_ID != null) // se existir o id de representante a mesma é diretamente encarteirada para o cliente
                    //{
                    //    int repId = (int)REP_ID;
                    //    _carteiramentoSRV.EncarteirarSuspectCriandoAssinaturaFranquia(cliente, repId); 
                    //}
                    //else 
                    if (REGIAO_ID != null && REP_ID_DEMANDANTE != null) // roda o rodizio
                    {
                        int regiaoId = (int)REGIAO_ID;
                        int representante = (int)REP_ID_DEMANDANTE;
                        _carteiramentoSRV.EncarteirarSuspectPorRodizio(cliente, regiaoId, representante);
                    }
                    else
                    {
                        if (!ignorarRodizio)
                        {
                            throw new Exception("Não é possível salvar o suspect. O representante não foi encontrado");
                        }
                    }

                }

                SincronizarTipoDeCliente(cliente);
                _endSRV.SalvarEnderecos(cliente.CLIENTES_ENDERECO.AsQueryable(), cliente.CLI_ID, null, cliente); // salva os endereços
                _infMarketingSRV.SalvarInfoMarketing(cliente); // salva a informação de marketing
                _assinaturaTelefone.SalvarEExcluirTelefones(cliente);
                _assinaturaEmail.SalvarEExcluirEmails(cliente);

                if (atualizarEncarteiramentos)
                    _carteiraClienteSRV.SalvarEExcluirCarteiraCliente(cliente);

                result.Cliente = cli;
                return result;
                

            }
            return null;
            
        }

        
        /// <summary>
        /// Checa se o cliente já está cadastrado no banco por nome, cpf/cpf_cnpj, email e telefone.
        /// Se for encontrado, então é lançado uma exceção. 
        /// </summary>
        /// <param name="cliente"></param>
        public void ChecaDuplicacaoClienteComErro(ClienteDto cliente, SalvarClienteResultDTO result, SalvarClienteParamDTO param = null)
        {
            var resp = ClienteJaExiste(cliente, param);

            if(resp != null && resp.HasDuplication){
                
                result.ResultadoValidacaoDuplicidade = resp;                
            }

            //if (resp != null && resp.HasDuplicationWarnings)
            //{
            //    throw new WarningException(resp.ErrorMessage);
            //}
        }

        /// <summary>
        /// Checa se o cliente possui pelo menos email ou telefone
        /// </summary>
        /// <param name="cliente"></param>
        public void ChecaEmailTelefone(ClienteDto cliente)
        {
            if (cliente != null)
            {
                var lstTelefone = cliente.ASSINATURA_TELEFONE;
                var lstEmail = cliente.ASSINATURA_EMAIL;

                    if ((lstTelefone == null || lstTelefone.Count() < 1) && (lstEmail == null || lstEmail.Count() < 1))
                    {
                        throw new ValidacaoException("email_telefone", new string[1]{
                                              
                            "Informe um telefone ou email. É necessário informar pelo menos uma maneira de contatar o cliente."                                            
                        });
                    }                
            }
        }

        public void ChecarEmailJaCadastrado(string email)
        {
            if (!string.IsNullOrWhiteSpace(email)) {

                var lstEmail = new List<AssinaturaEmailDTO>(){

                    new AssinaturaEmailDTO()
                    {
                        AEM_EMAIL = email
                    }
                };

                var resp = ClienteJaExiste(null, null, null, null, lstEmail);
                if (resp.HasDuplication)
                    throw new Exception(string.Format("O E-Email que você está tentando '{0}' já existe.", email));
            }
        }

        /// <summary>
        /// Checa se o cliente já está cadastrado no banco por nome, cpf/cpf_cnpj, email e telefone.
        /// Se for encontrado, Devolve um sumário dos campos que já existe em outro cliente        
        public ResultClienteDuplicadoDTO ClienteJaExiste(ClienteDto cliente, SalvarClienteParamDTO param)
        {
            if (cliente != null)
            {
                bool validacaoNaoRestritiva = false;
                bool Force = false;
                if(param != null)
                {
                    validacaoNaoRestritiva = param.ValidacaoEmailTelCPFNaoRestritiva;
                    Force = param.ForcarSalvamento;
                }

                int? CLI_ID = cliente.CLI_ID;
                string nome = cliente.CLI_NOME;
                string cpf_cnpj = cliente.CLI_CPF_CNPJ;

                
                var lstTelefones = cliente.ASSINATURA_TELEFONE;
                var lstEmails = cliente.ASSINATURA_EMAIL;

                var resp = ClienteJaExiste(CLI_ID, nome, cpf_cnpj, lstTelefones, lstEmails);
                
                if (resp != null)
                {

                    if(resp.HasDuplication)
                    {
                        
                        StringBuilder sb = new StringBuilder("{0}: O cliente que você está tentando cadastrar já existe: \r\n");
                        
                        resp.Force = Force;
                        var clienteBanco = FindByIdFullLoaded(CLI_ID, false, true, true);

                        string template = "\t * O {0} informado já existe. \r\n";

                        if(resp.HasDuplicationNome.Falhou){
                            sb.Append(string.Format(template, "nome"));

                            resp.HasDuplicationNome.Tipo = TipoValidacao.ERRO;
                            if (clienteBanco != null)
                            {
                                resp.HasDuplicationNome.Tipo = 
                                    (cliente.CLI_NOME == clienteBanco.CLI_NOME) ? 
                                    TipoValidacao.WARNING : 
                                TipoValidacao.ERRO;
                            }
                        }

                        if(resp.HasDuplicationCPF_CNPJ.Falhou)
                        {
                            resp.HasDuplicationCPF_CNPJ.Tipo = TipoValidacao.ERRO;
                            if (clienteBanco != null)
                            {
                                resp.HasDuplicationCPF_CNPJ.Tipo =
                                    (cliente.CLI_CPF_CNPJ == clienteBanco.CLI_CPF_CNPJ) ?
                                    TipoValidacao.WARNING :
                                TipoValidacao.ERRO;
                            }
                            sb.Append(string.Format(template, "cpf/cpf_cnpj"));
                        }

                        if(resp.HasDuplicationTelefone.Falhou)
                        {
                            resp.HasDuplicationTelefone.Tipo = TipoValidacao.ERRO;
                            sb.Append(string.Format(template, "telefone"));

                            if (clienteBanco != null)
                            {
                                if(
                                    (resp.ListDuplicationTelefone != null && 
                                    clienteBanco.ASSINATURA_TELEFONE == null) ||
                                    (resp.ListDuplicationTelefone == null &&
                                    clienteBanco.ASSINATURA_TELEFONE != null)
                                )
                                {
                                    resp.HasDuplicationTelefone.Tipo = TipoValidacao.ERRO;
                                }
                                else
                                {                               
                                    var lstTelefone = resp.ListDuplicationTelefone.Select(x => x.ATE_TELEFONE);
                                    var lstTelefoneBanco = clienteBanco.ASSINATURA_TELEFONE.Select(x => x.ATE_TELEFONE);

                                    var existe = (lstTelefone.Where(x => lstTelefoneBanco.Contains(x)).Count() > 0);
                                    resp.HasDuplicationTelefone.Tipo = (existe) ? TipoValidacao.WARNING : TipoValidacao.ERRO;
                                }
                            }

                        }
                        if(resp.HasDuplicationEmail.Falhou)
                        {
                            resp.HasDuplicationEmail.Tipo = TipoValidacao.ERRO;
                            sb.Append(string.Format(template, "email"));

                            if (clienteBanco != null)
                            {
                                if (
                                (resp.ListDuplicationEmail != null &&
                                    clienteBanco.ASSINATURA_EMAIL == null) ||
                                    (resp.ListDuplicationEmail == null &&
                                    clienteBanco.ASSINATURA_EMAIL != null)
                              )
                                {
                                    resp.HasDuplicationEmail.Tipo = TipoValidacao.ERRO;
                                }
                                else
                                {
                                    var lstEmail = resp.ListDuplicationEmail.Select(x => x.AEM_EMAIL);
                                    var lstEmailBanco = clienteBanco.ASSINATURA_EMAIL.Select(x => x.AEM_EMAIL);

                                    var existe = (lstEmail.Where(x => lstEmailBanco.Contains(x)).Count() > 0);
                                    resp.HasDuplicationEmail.Tipo = (existe) ? TipoValidacao.WARNING : TipoValidacao.ERRO;
                                }
                            }
                        }

                        string tipoErro = (!resp.HasDuplicationErrors && resp.HasDuplicationWarnings) ? "Aviso!" : "Erro!";
                        resp.ErrorMessage = string.Format(sb.ToString(), tipoErro);

                    }
                }
                return resp;
            }
            return null;
        }

        public ResultClienteDuplicadoDTO ClienteJaExiste(int? CLI_ID, string nome, string cpf_cnpj, 
            IEnumerable<AssinaturaTelefoneDTO> assinaturaTelefones, 
            IEnumerable<AssinaturaEmailDTO> assinaturaEmails)
        {
            return _dao.ClienteJaExiste(CLI_ID, nome, cpf_cnpj, assinaturaTelefones, assinaturaEmails);
        }

        public ResultClienteDuplicadoDTO ClienteJaExiste(string nome, string cpf_cnpj, IEnumerable<AssinaturaTelefoneDTO> assinaturaTelefones, IEnumerable<AssinaturaEmailDTO> assinaturaEmails)
        {
            return ClienteJaExiste(null, nome, cpf_cnpj, assinaturaTelefones, assinaturaEmails);
        }

        /// <summary>
        /// Ao passar como argumento o resultado do salvamento mal sucedido por conta de uma validação que falhou, preenche quais clientes conflitaram.
        /// </summary>
        /// <param name="result"></param>
        private void PreencherClientesConflitoReferencia(SalvarClienteResultDTO result)
        {
            if (result != null && result.ResultadoValidacaoDuplicidade != null && result.ResultadoValidacaoDuplicidade.HasDuplication)
            {
                result.ResultadoValidacaoDuplicidade.CriarListasQuandoNulla();
                var lstDuplicationNome = result.ResultadoValidacaoDuplicidade.ListDuplicationNome;
                var lstDuplicationCPF_CNPJ = result.ResultadoValidacaoDuplicidade.ListDuplicationCPF_CNPJ;
                var lstDuplicationEmail = result.ResultadoValidacaoDuplicidade.ListDuplicationEmail.Select(x => x.CLI_ID);
                var lstDuplicationTelefone = result.ResultadoValidacaoDuplicidade.ListDuplicationTelefone.Select(x => x.CLI_ID);

                ICollection<int?> lstCliIds =
                    lstDuplicationNome
                    .Concat(lstDuplicationCPF_CNPJ)
                    .Concat(lstDuplicationEmail)
                    .Concat(lstDuplicationTelefone)
                    .ToList();

                var lstClientes = ListarClientesPorIds(lstCliIds);

                foreach(var cli in lstClientes)
                {
                    result.ItensReferencia.Add(new ItemReferenciaValidacaoClienteDTO()
                    {
                        Cliente = cli,
                        CPFDuplicado = lstDuplicationCPF_CNPJ.Contains(cli.CLI_ID),
                        EmailDuplicado = lstDuplicationEmail.Contains(cli.CLI_ID),
                        NomeDuplicado = lstDuplicationNome.Contains(cli.CLI_ID),
                        TelefoneDuplicado = lstDuplicationTelefone.Contains(cli.CLI_ID)
                    });
                }

            }
        }
        
        /// <summary>
        /// Pega o cliente com todas as informações relativa a eles Franquia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClienteDto FindByIdFullLoaded(int? id, bool trazInfoMarketing = false, bool trazClienteTelefone = false, bool trazAssinaturaEmail = false, bool validaEnderecoBasica = false)
        {
            var buscaDTO = new BuscarClientePorIdDTO()
            {
                trazAssinaturaEmail = trazAssinaturaEmail,
                trazClienteTelefone = trazClienteTelefone,
                trazInfoMarketing = trazInfoMarketing,
                validaEnderecoBasica = validaEnderecoBasica
            };

            return FindByIdFullLoaded(id, buscaDTO);
        }

        /// <summary>
        /// Pega o cliente com todas as informações relativa a eles Franquia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClienteDto FindByIdFullLoaded(int? id, BuscarClientePorIdDTO buscaDTO)
        {
            
            var cliente = FindById(id);
            bool validaEnderecoBasica = false;
            bool trazInfoMarketing = false;
            bool trazClienteTelefone = false;
            bool trazAssinaturaEmail = false;
            bool trazCarteiraCliente = false;


            if(buscaDTO != null)
            {
                validaEnderecoBasica = buscaDTO.validaEnderecoBasica;
                trazInfoMarketing = buscaDTO.trazInfoMarketing;
                trazClienteTelefone = buscaDTO.trazClienteTelefone;
                trazAssinaturaEmail = buscaDTO.trazAssinaturaEmail;
                trazCarteiraCliente = buscaDTO.trazCarteiraCliente;
            }

            if (cliente != null)
            {
                _endSRV.PreencherEnderecoCliente(cliente, validaEnderecoBasica);

                if (trazInfoMarketing)
                {
                    _infMarketingSRV.PreencherInformacoesDeMarketing(cliente);
                }

                if (trazClienteTelefone)
                {
                    _assinaturaTelefone.PreencherTelefoneAssinaturaNoCliente(cliente);
                }

                if (trazAssinaturaEmail)
                {
                    _assinaturaEmail.PreencherEmailAssinaturaNoCliente(cliente);
                }

                if (trazCarteiraCliente)
                {
                    _carteiraClienteSRV.PreencherCarteiraCliente(cliente);
                }
            }
            return cliente;

        }

        /// <summary>
        /// Pega o cliente com todas as informações escolhidas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClienteDto FindByIdFullLoadedGeneral(int id, int? PROD_ID = null, bool trazInfoMarketing = false, bool trazAssinaturaTelefone = false, bool trazAssinaturaEmail = false, bool trazLstAssinatura = false)
        {
            var cliente = FindById(id);
            _endSRV.PreencherEnderecoCliente(cliente);

            if (trazInfoMarketing)
            {
                _infMarketingSRV.PreencherInformacoesDeMarketing(cliente);
            }

            if (trazAssinaturaTelefone || trazAssinaturaEmail)
            {
                _assinaturaSRV.PreencherAssinatura(cliente, PROD_ID, trazAssinaturaTelefone, trazAssinaturaEmail);
            }

            return cliente;

        }
        public ResumoQuantidadeTipoCliente QtdClientesRepresentante(int? REP_ID, int? UEN_ID = 1)
        {
            var lista =  _dao.QtdClientesRepresentante(REP_ID, UEN_ID);

            ResumoQuantidadeTipoCliente resumoQuantidade = new ResumoQuantidadeTipoCliente()
            {
                QuantidadeClassificacaoClienteDTO = lista
            };

            return resumoQuantidade;
        }

        public ResumoQuantidadeTipoCliente QtdClientesRepresentanteGerente(int? REP_GERENTE_ID, int? REP_ID, int? UEN_ID = 1)
        {

            if (_representanteSRV.RepresentantesExistemNaMesmaRegiao(REP_GERENTE_ID, REP_ID))
            {
                var lista = _dao.QtdClientesRepresentante(REP_ID, UEN_ID);

                ResumoQuantidadeTipoCliente resumoQuantidade = new ResumoQuantidadeTipoCliente()
                {
                    QuantidadeClassificacaoClienteDTO = lista
                };

                return resumoQuantidade;
            }
            return null;
        }

        public void InformarContato(ContatoFranquiaDTO contato, int REP_ID, string usuario)
        {
            using (var scope = new TransactionScope())
            {
                if (contato != null)
                {
                    //Foi requerido que data fosse automática.
                    //DateTime? data = contato.Data;
                    DateTime? data = DateTime.Now;

                    int CLI_ID = contato.CLI_ID;
                    string observacao = contato.Observacoes;

                    _prioridadeAtendimento.ConfirmarAtendimentoDePrioridade(REP_ID, CLI_ID);
                    _histAtendSRV.InserirHistoricoAtendimento(data, observacao, usuario, REP_ID, CLI_ID, 7);
                    //_agendamentoSRV.ConfirmarAgendamentosAteData(data, REP_ID);
                }

                scope.Complete();
            }           
        }

        /// <summary>
        /// Transfere os suspects de uma operadora de acordo com as regras passadas <br />
        /// Tipos: {<br /><br />
        ///   
        ///  representante = Transfere todos os suspects de uma operadora diretamente para a outra operadora, <br />
        ///  rodizioLogado = Transfere os suspects para o rodizio de operadoras, da região selecionada, que estão logadas, <br />
        ///  rodizioGeral = Distribui os suspects para todas as operadoras da região selecionada <br /><br />
        ///  
        /// }
        /// </summary>
        /// <param name="dto"></param>
        public IList<RepresentanteTransferenciaDTO> TransferirSuspects(TransferirSuspectDTO dto)
        {

            TransactionOptions txOpt = new TransactionOptions();
            txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
            {
                var tipo = dto.Tipo;

                switch (tipo)
                {
                    case "representante":
                        {
                            _TransferirClienteParaRepresentante(dto.CAR_ID_ORIGEM, dto.CAR_ID_DESTINO);
                            scope.Complete();
                            return null;
                        }

                    case "rodizioLogado":
                        {
                            var resp = _TransferirSuspectsParaRodizioLogado(dto.CAR_ID_ORIGEM, dto.RG_ID);
                            scope.Complete();
                            return resp;
                        }
                    case "rodizioGeral":
                        {
                            var resp = _TransferirSuspectsParaRodizioGeral(dto.CAR_ID_ORIGEM, dto.RG_ID);
                            scope.Complete();
                            return resp;

                        }
                    default:
                        return null;                        
                }

            }         

        }

       
        private void _TransferirClientes(IEnumerable<CarteiraClienteDTO> carteiramentos, string CAR_ID)
        {
            IEnumerable<CarteiraClienteDTO> carteiramentoParaDeletar = new List<CarteiraClienteDTO>(carteiramentos);

            _carteiraClienteSRV.DeleteAll(carteiramentoParaDeletar);
            foreach (var cart in carteiramentos) // atribuo os encarteirametos para um novo operador
            {
                cart.CAR_ID = CAR_ID;
                cart.CARTEIRA = null;
            }
            _carteiraClienteSRV.SaveOrUpdateNonIdentityKeyEntity(carteiramentos);
        }

        /// <summary>
        ///  Executa a transferencia de suspect do tipo 'operadora' que é:
        ///  Transfere todos os suspects de uma operadora diretamente para a outra operadora
        ///  
        /// </summary>
        /// <param name="carIdOrigem"></param>
        /// <param name="carIdDestino"></param>
        private void _TransferirClienteParaRepresentante(string carIdOrigem, string carIdDestino)
        {
            if (carIdOrigem == null)
            {
                throw new ArgumentNullException("O representante de origem não foi selecionado");
            }
            if (carIdDestino == null)
            {   
                throw new ArgumentNullException("O representante que receberá os suspects não foi selecionado");
            }

            if (carIdOrigem == carIdDestino)
            {
                throw new NegocioException("Não é possível transferir, o operador de origem e destino não podem ser os mesmos");
            }

            IList<CarteiraClienteDTO> carteiraAssinatura = _carteiraClienteSRV.FindByCarId(carIdOrigem);
            
            _TransferirClientes(carteiraAssinatura, carIdDestino);
        }

        /// <summary>
        ///  Executa a transferencia de suspect do tipo 'rodizioGeral' que é:
        ///  Distribui todos os suspects de uma operadora para todas as operadoras da região
        /// </summary>
        /// <param name="slpCodeOrigem"></param>
        /// <param name="regiaoId"></param>
        private IList<RepresentanteTransferenciaDTO> _TransferirSuspectsParaRodizioGeral(string carIdOrigem, int? regiaoId)
        {
            IList<CarteiraClienteDTO> carteiramentos = _carteiraClienteSRV.FindByCarId(carIdOrigem);
            IList<RepresentanteDTO> representante = _representanteSRV.RepresentantesDaRegiao(regiaoId, carIdOrigem);
            return _DistribuirClientes(carteiramentos, representante);
        }

        /// <summary>
        ///  Executa a transferencia de suspect do tipo 'rodizioLogado' que é:
        ///  Distribui todos os suspects de uma operadora para todas as operadoras "LOGADAS" da região 
        /// </summary>
        /// <param name="slpCodeOrigem"></param>
        /// <param name="regiaoId"></param>
        /// <returns></returns>
        private IList<RepresentanteTransferenciaDTO> _TransferirSuspectsParaRodizioLogado(string CAR_ID_ORIGEM, int? regiaoId)
        {
            IList<CarteiraClienteDTO> carteiramentos = _carteiraClienteSRV.FindByCarId(CAR_ID_ORIGEM);

            DateTime dateTime = DateTime.Today;
            IList<RepresentanteDTO> representante = _representanteSRV.GetRepresentantesLogados(dateTime, regiaoId, CAR_ID_ORIGEM);

            if (representante == null || representante.Count <= 0)
            {
                throw new NegocioException("Não existe representantes logados para essa região. Possível causa: O operador deslogou durante esse processo.");
            }
            return _DistribuirClientes(carteiramentos, representante);
        }

        /// <summary>
        /// Distribui os suspects
        /// </summary>
        /// <param name="carteiramentos"></param>
        /// <param name="representantes"></param>
        /// <param name="update">Determina se o carteiramento deve ser atualizado ou salvo</param>
        private IList<RepresentanteTransferenciaDTO> _DistribuirClientes(IEnumerable<CarteiraClienteDTO> carteiramentos, IEnumerable<RepresentanteDTO> representantes)
        {


            int quantidadeCarteiramento = carteiramentos.Count();
            int quantidadeRepresentantes = representantes.Count();

            if (quantidadeRepresentantes == 0)
            {
                throw new NegocioException("Por algum motivo não foi possível achar nenhum operador de destino. Por favor, refaça o processo.");
            }
            int quantidadeDistribuicao = quantidadeCarteiramento / quantidadeRepresentantes; // calcula quantos carteiramentos por operador será realizado
            int excedente = quantidadeCarteiramento % quantidadeRepresentantes; // calcula quantos carteiramentos sobraram

            IList<IEnumerable<CarteiraClienteDTO>> listaDistribuida = new List<IEnumerable<CarteiraClienteDTO>>();


            if (quantidadeDistribuicao > 0) // distribui os carteiramentos em partes iguals para cada operadoras
            {
                int index = 0;

                IList<RepresentanteDTO> reps = representantes.ToList();

                representantes = null; // induzir carbage collection----
                RepresentanteDTO rep0 = (excedente > 0) ? reps[0] : null; // se existe carteiramento sobrando separo o primeiro representante para recebe-los
                IList<RepresentanteTransferenciaDTO> resp = new List<RepresentanteTransferenciaDTO>();
                IEnumerable<CarteiraClienteDTO> cartParaSalvar = new List<CarteiraClienteDTO>();

                for (int i = 0; i < quantidadeRepresentantes; i++)
                {
                    // var inicio = (i * quantidadeDistribuicao);
                    IEnumerable<CarteiraClienteDTO> lista = null;

                    lista = carteiramentos.Take(quantidadeDistribuicao);
                    // elimita os encarteiramentos associados ao operador para a lista ser subtraida a cada iteração
                    carteiramentos = carteiramentos.AsQueryable().Skip(quantidadeDistribuicao);

                    RepresentanteDTO rep = reps[index];
                    CarteiraDTO car = _carteiramentoSRV.GetPrimeiraCarteiraDeFranquiaDoRepresentante(rep);

                    _TransferirClientes(lista, car.CAR_ID);

                    RepresentanteTransferenciaDTO dto = new RepresentanteTransferenciaDTO()
                    {
                        REP_ID = rep.REP_ID,
                        NOME = rep.REP_NOME,
                        QtdClientesTransferidos = lista.Count()
                    };

                    cartParaSalvar = cartParaSalvar.Concat(lista);
                    resp.Add(dto);
                    index++;

                }

                if (excedente > 0)
                {
                    //carteiramentos = carteiramentos.Skip(quantidadeDistribuicao); // elimina os ultimos carteiramentos que foram passados para sobrar apenas o execente
                    CarteiraDTO car = _carteiramentoSRV.GetPrimeiraCarteiraDeFranquiaDoRepresentante(rep0);

                    if (car != null)
                    {
                        _TransferirClientes(carteiramentos, car.CAR_ID); // entrega o excedente para o primeiro operador encontrado
                        resp[0].QtdClientesTransferidos += carteiramentos.Count();
                        cartParaSalvar = cartParaSalvar.Concat(carteiramentos);
                    }
                }

                try
                {
                    //if (update)
                    //    _carteiramentoSRV.MergeAll(cartParaSalvar, saveChanges: saveChanges);
                    //else
                    //    _carteiramentoSRV.SaveAll(cartParaSalvar, saveChanges: saveChanges, batchSize: 200);
                    _carteiraClienteSRV.SaveOrUpdateNonIdentityKeyEntity(cartParaSalvar);
                }
                catch (Exception e)
                {
                    throw new Exception("Ocorreu um erro ao tentar transferir os suspects: " + e.Message, e);
                }

                return resp;
            }
            else
            {
                throw new Exception("Não é possível distribuir os clientes. O número de representantes é superior ao número de clientes");
            }
            
        }

        public void MudarTipoCliente(ClienteDto cliente, int? CLA_CLI_ID)
        {
            if (cliente != null)
            {
                if (CLA_CLI_ID > 1)
                {
                    if (string.IsNullOrEmpty(cliente.CLI_NOME))
                    {
                        throw new ValidacaoException("Não é possível evoluir o status do cliente. O nome é obrigatório");
                    }
                    if (string.IsNullOrEmpty(cliente.CLI_CPF_CNPJ))
                    {
                        throw new ValidacaoException("Não é possível evoluir o status do cliente. O CPF é obrigatório");
                    }
                    
                }

                cliente.CLA_CLI_ID = CLA_CLI_ID;
                Merge(cliente);
            }
        }

        public void MudarTipoCliente(int CLI_ID, int? CLA_CLI_ID)
        {
            ClienteDto cliente = FindById(CLI_ID);
            MudarTipoCliente(cliente, CLA_CLI_ID);
        }

        public void EvoluirTipoCliente(int CLI_ID)
        {
            ClienteDto cliente = FindById(CLI_ID);

            EvoluirTipoCliente(cliente);
        }

        public void EvoluirTipoCliente(ClienteDto cliente)
        {
            if (cliente != null)
            {
                switch (cliente.CLA_CLI_ID)
                {
                    case 1:
                        {
                            MudarTipoCliente(cliente, 2);
                            break;
                        }
                    case 2:
                        {
                            MudarTipoCliente(cliente, 3);
                            break;
                        }
                    default:
                        {
                            MudarTipoCliente(cliente, 1);
                            break;
                        }
                }
            }
        }

        public bool RepresentantePodeEditarCliente(int? CLI_ID, int? representanteId, int? uenId = 1)
        {
            return _dao.RepresentantePodeEditarCliente(CLI_ID, representanteId, uenId);
        }

        public void EncaminharCliente(EncaminhamentoDTO encaminhamento, int? REP_ID_QUE_ENCAMINHOU, string usuario = null)
        {
            if (encaminhamento != null)
            {
                using (var scope = new TransactionScope())
                {
                    var CLI_ID = encaminhamento.CLI_ID;
                    var REP_ID = encaminhamento.REP_ID;
                    var observacoes = encaminhamento.observacao;
                    ClienteDto cliente = FindById(CLI_ID);
                    RepresentanteDTO representante = _representanteSRV.FindById(REP_ID_QUE_ENCAMINHOU);

                    if (cliente != null && representante != null)
                    {
                        string mensagem = "A operadora '{0}' acaba de encaminhar um cliente para sua carteira. Nome do Cliente '{1}' Observações da operadora que encaminhou: '{2}'";
                        mensagem = string.Format(mensagem, representante.REP_NOME, cliente.CLI_NOME, encaminhamento.observacao);

                        _notificacaoSRV.InserirNotificacao(4, "PRIORITY", mensagem, CLI_ID, REP_ID, REP_ID_QUE_ENCAMINHOU);
                        _prioridadeAtendimento.RegistrarPrioridade((int)REP_ID, (int)CLI_ID, 4, observacoes, REP_ID_QUE_ENCAMINHOU);
                        _histAtendSRV.RegistraHistoricoEncaminhamento(usuario, REP_ID_QUE_ENCAMINHOU,  REP_ID, CLI_ID, observacoes);
                    }
                    scope.Complete();
                }

            }
        }

        /// <summary>
        /// Atualiza o campo data de ultimo histórico e o status do cliente.
        /// Ação para ser disparada ao ser inserido um novo histórico
        /// </summary>
        /// <param name="CLI_ID">Id do cliente</param>
        /// <param name="data">Data do cadastro</param>
        public void AtualizarDataAlteracao(int? CLI_ID, DateTime? data)
        {
            if (CLI_ID != null && data != null)
            {
                var cliente = FindById(CLI_ID);
                cliente.DATA_ULTIMO_HISTORICO = data;
                //AtribuirTipoDeCliente(cliente, true, true);
                Merge(cliente);
            }
        }

        /// <summary>
        /// Verifica se o status do cliente pode ser alterado
        /// de acordo com as regras.
        /// </summary>
        /// <param name="cliente">Objeto do cliente</param>
        /// <param name="naoTestaHistorico">O histórico do cliente não será testado. Se ele se classifica como prospect, nesse caso ele será classificado como cliente.</param>
        /// <param name="carregarTelefoneEEmailDoBanco">Se for true, carrega o telefone e email do banco se eles não forem válidos no cliente</param>
        public void AtribuirTipoDeCliente(ClienteDto cliente, bool naoTestaHistorico = false, bool carregarTelefoneEEmailDoBanco = false)
        {
            // se ele não for nullo e já for classificado como um cliente não é necessário classifica-lo mais.
            if (cliente != null && cliente.CLA_CLI_ID != 3) 
            {
                cliente.CLA_CLI_ID = 1;

                var lstTelefone = cliente.ASSINATURA_TELEFONE; //_assinaturaTelefone.ExtrairTelefoneDaAssinaturaFranquiaCliente(cliente, carregarTelefoneEEmailDoBanco);
                var lstEmails = cliente.ASSINATURA_EMAIL;//_assinaturaEmail.ExtrairEmailDaAssinaturaFranquiaCliente(cliente, carregarTelefoneEEmailDoBanco);

                if (!string.IsNullOrWhiteSpace(cliente.CLI_NOME) && ((lstTelefone != null && lstTelefone.Count > 0) || (lstEmails != null && lstEmails.Count() > 0)))
                {
                    var cliEndereco = _endSRV.ExtrairEnderecosDoCliente(cliente);
                    int countEnd = (cliEndereco != null) ? cliEndereco.Count() : 0;

                    if (!string.IsNullOrWhiteSpace(cliente.CLI_CPF_CNPJ))
                    {
                        cliente.CLA_CLI_ID = 2;
                        //if (countEnd > 0 && (naoTestaHistorico || _histAtendSRV.VerificarClientePossuiHistorico(cliente)))
                        //{
                        //    cliente.CLA_CLI_ID = 3;
                        //}
                    }

                }
            }
        }

        public Pagina<RelatorioClientesRecebidosDTO> SuspectsCadastradosNoDia(DateTime data, int? UEN_ID, int? RG_ID, int pagina = 1, int registrosPorPagina = 100)
        {
            return _dao.SuspectsCadastradosNoDia(data, UEN_ID, RG_ID, pagina, registrosPorPagina);
        }
        public ContratoSituacaoDTO BuscarSituacaoCliente(string _asn_id)
        {
            return _dao.BuscarSituacaoCliente(_asn_id);
        }

        /// <summary>
        /// Salva o cliente junto com suas informações de marketing
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public ClienteDto SalvarClienteEInformacoesDeMarketingSemRodizio(ClienteDto cliente, int? PROD_ID)
        {
            bool retornarMerge = true;
            if (cliente != null)
            {
                ClienteDto cli = null;
                //ChecaEmailTelefone(cliente);
                //ChecaDuplicacaoClienteComErro(cliente);

                if (cliente.CLI_ID != null)
                {
                    cliente.DATA_ALTERA = DateTime.Now;
                    cli = Merge(cliente, "CLI_ID"); // salvo o cliente
                        
                }
                else
                {
                    cliente.DATA_CADASTRO = DateTime.Now;
                    var clienteNovo = Save(cliente); //insiro no banco
                    cliente.CLI_ID = clienteNovo.CLI_ID; // pego o id do cliente para utili-lo como referência para a inserção das outras entidades

                    if (PROD_ID != null)
                    {
                        _assinaturaSRV.GerarAssinatura(cliente, PROD_ID);
                    }
                    retornarMerge = false;

                }

                SincronizarTipoDeCliente(cliente);
                _endSRV.SalvarEnderecos(cliente.CLIENTES_ENDERECO.AsQueryable(), cliente.CLI_ID); // salva os endereços
                _infMarketingSRV.SalvarInfoMarketing(cliente); // salva a informação de marketing
                _assinaturaTelefone.SalvarEExcluirTelefonesAssinatura(cliente, PROD_ID);
                _assinaturaEmail.SalvarEExcluirEmailsAssinatura(cliente, PROD_ID);

                if (!retornarMerge)
                    return cliente;
                return cli;
            }

            return cliente;
        }


        public Pagina<BuscarClienteDTO> BuscarClientesGeral(
            string cpf_cnpj = null,
            string nome = null,
            int? classificacaoClienteId = null,
            string email = null,
            string dddTelefone = null,
            string telefone = null,
            int? AREA_ID = null,
            int? CMP_ID = null,
            int? CLI_ID = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            int pagina = 1,
            int registroPorPagina = 7,
            bool? excluidosDaValidacao = null)
        {
            return _dao.BuscarClientesGeral(cpf_cnpj, nome, classificacaoClienteId, email, dddTelefone, telefone, AREA_ID, CMP_ID, CLI_ID, pesquisaCpfCnpjPorIqualdade, pagina, registroPorPagina, excluidosDaValidacao); 
        }


        internal void VerificarDadosCliente(ClienteDto cliente, string acao)
        {

            string msg = "Não é possível {0} por que é necessário que o cliente possua os seguintes dados: {1}";
            StringBuilder campos = new StringBuilder();
            bool faltaCampo = false;

            if (cliente != null)
            {
                var cliEndereco = _endSRV.ExtrairEnderecosDoCliente(cliente);
                int countEnd = (cliEndereco != null) ? cliEndereco.Count() : 0;

                if (cliente.CLA_CLI_ID != 3)
                {
                    var lstTelefone = cliente.ASSINATURA_TELEFONE;//_assinaturaTelefone.ExtrairTelefoneDaAssinaturaFranquiaCliente(cliente, true);
                    var lstEmails = cliente.ASSINATURA_EMAIL;//_assinaturaEmail.ExtrairEmailDaAssinaturaFranquiaCliente(cliente, true);

                    if (string.IsNullOrWhiteSpace(cliente.CLI_NOME))
                    {
                        campos.Append("Nome");
                        faltaCampo = true;
                    }


                    if (string.IsNullOrWhiteSpace(cliente.CLI_CPF_CNPJ))
                    {
                        if (faltaCampo)
                            campos.Append(", ");
                        campos.Append("CPF/CNPJ");
                        faltaCampo = true;
                    }

                    if (lstTelefone == null || lstTelefone.Count <= 0)
                    {
                        if (faltaCampo)
                            campos.Append(", ");
                        campos.Append("Telefone");
                        faltaCampo = true;
                    }

                    if (lstEmails == null || lstEmails.Count() <= 0)
                    {
                        if (faltaCampo)
                            campos.Append(", ");
                        campos.Append("Email");
                        faltaCampo = true;
                    }

                }

                if (countEnd <= 0)
                {
                    if (faltaCampo)
                        campos.Append(", ");
                    campos.Append("Endereço");
                    faltaCampo = true;
                }
                else
                {

                    var result = ValidatorProxy.RecursiveValidate<ClienteEnderecoDto>(cliEndereco);
                    if (!result.IsValid)
                    {
                        if (faltaCampo)
                            campos.Append(", ");
                        campos.Append("Campos do Endereço: ");
                        campos.Append(result);

                        faltaCampo = true;
                    }

                }                

              }

            if (faltaCampo)
            {
                string msgMontada = string.Format(msg, acao, campos.ToString());
                throw new Exception(msgMontada);
            }
        }


        //public void InformarVendaEfetuada(ObservacaoDTO observacaoDTO)
        //{
        //    using (var scope = new TransactionScope())
        //    {

        //        if(observacaoDTO != null)
        //        {
        //            var CLI_ID = observacaoDTO.CLI_ID;
        //            var REP_ID = observacaoDTO.REP_ID;
        //            var usuario = observacaoDTO.USUARIO;

        //            var cliente = findByIdFullLoaded((int) CLI_ID, false, true, true);
        //            _verificarDadosCliente(cliente, "informar venda");
        //            cliente.CLA_CLI_ID = 3;

        //            if (!_cartCoadSRV.HasCartCoadByCliId(CLI_ID))
        //            {
        //                var cartCoad = SalvarComoProspectado(cliente, REP_ID);

        //                if (cartCoad != null)
        //                {
        //                    var codigo = cartCoad.CODIGO;

        //                    int codigoInt;
        //                    if (int.TryParse(codigo, out codigoInt))
        //                    {
        //                        cliente.PROSP_ID = codigoInt;
        //                    }
        //                }
        //            }

        //            SaveOrUpdate(cliente);
                    
        //            _histAtendSRV.RegistrarHistoricoVendaEfetuada(usuario, REP_ID, CLI_ID, observacaoDTO.Observacoes);
        //            _prioridadeAtendimento.ConfirmarAtendimentoDePrioridade((int) REP_ID, (int) CLI_ID);
                                      
                    
        //        }
        //        scope.Complete();
        //    }
        //}


        public CartCoadDTO SalvarComoProspectado(ClienteDto cliente, int? REP_ID, string carId = null, bool inserirNovo = false)
        {
            if (cliente != null && cliente.CLI_ID  != null && REP_ID != null)
            {
                string CAR_ID_ANTIGA = null;
                
                RepresentanteDTO rep = _representanteSRV.FindById(REP_ID);
                
                CAR_ID_ANTIGA = (!string.IsNullOrWhiteSpace(carId)) ? carId : rep.REP_COD_CARTEIRA_ANTIGO;

                CartCoadDTO cartCoad = null;

                if (!inserirNovo)
                {
                    cartCoad =
                        (string.IsNullOrWhiteSpace(cliente.CodigoProspect)) ?
                         _cartCoadSRV.ObterPorCliente(cliente.CLI_ID) :
                         _cartCoadSRV.FindById(cliente.CodigoProspect);

                }
                if (cartCoad == null)
                    cartCoad = new CartCoadDTO();
                
                cartCoad.CLI_ID = cliente.CLI_ID;
                cartCoad.NOME = StringUtil.Truncate(cliente.CLI_NOME, 35);
                cartCoad.A_C = StringUtil.Truncate(cliente.CLI_A_C, 35);

                MunicipioSRV _munSRV = null;

                if(cliente.CLIENTES_ENDERECO != null && cliente.CLIENTES_ENDERECO.Count() > 0)
                {
                    var end = _endSRV.BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliente);

                    if (end != null)
                    {
                        cartCoad.TIPO = _cartCoadSRV.PreencherTipoLogradouro(end.END_TIPO);
                        cartCoad.LOGRAD = StringUtil.Truncate(end.END_LOGRADOURO, 50);
                        cartCoad.NUMERO = end.END_NUMERO;
                        cartCoad.TIPO_COMPL = end.TIPO_COMP_ID;
                        cartCoad.COMPL = StringUtil.Truncate(end.END_COMPLEMENTO, 15);
                        cartCoad.BAIRRO = StringUtil.Truncate(end.END_BAIRRO, 20);
                        cartCoad.UF = end.END_UF;
                        cartCoad.CEP = end.END_CEP;
                        cartCoad.IDENTIFICACAO = "P";

                        if(cartCoad.DATA_CADASTRO == null)
                            cartCoad.DATA_CADASTRO = DateTime.Now.ToString("dd/MM/yyyy");

                        if (end.MUN_ID != null && end.MUNICIPIO == null)
                        {
                            if (_munSRV == null)
                                _munSRV = new MunicipioSRV();

                            end.MUNICIPIO = _munSRV.FindById(end.MUN_ID);
                        }

                        if (end.MUNICIPIO != null)
                        {
                            cartCoad.MUNIC = end.MUNICIPIO.MUN_DESCRICAO;
                        }
                    }
                }

                ProspectsDTO prop = new ProspectsDTO();

                string area = "1";
                string regiao = null;
                string codRep = CAR_ID_ANTIGA;

                if (!string.IsNullOrWhiteSpace(CAR_ID_ANTIGA) && CAR_ID_ANTIGA.Length > 4)
                {
                    regiao = CAR_ID_ANTIGA.Substring(0, 2);
                    area = CAR_ID_ANTIGA.Substring(2, 1);
                    codRep = CAR_ID_ANTIGA.Substring(3, 4);
                }
                else
                {
                    regiao = rep.REGIAO_UF;
                }

                prop.CART = codRep;
                prop.DRIVE_CDROM = "0";
                prop.AREA = area;
                prop.NUM_ENVIOS_ADNRJ = 0;
                prop.INTERNET = "0";

                switch(cliente.TIPO_CLI_ID){

                    case 1 : prop.PFIS_PJUR = "J";
                        break;
                    case 2 : prop.PFIS_PJUR = "F";
                        break;
                    case 3 : prop.PFIS_PJUR = "J";
                        break;
                    default : prop.PFIS_PJUR = "F";
                        break;                
                }

                prop.CPF_CNPJ = cliente.CLI_CPF_CNPJ;
                prop.REGIAO = regiao;

                cartCoad.prospects = prop;

                var lstTelefones = _processarCriacaoTelefoneProspect(cliente);
                var lstEmails = _processarCriacaoEmailProspect(cliente);
                
                cartCoad.TELEFONES_PROSP = lstTelefones;
                cartCoad.EMAILS_PROSP = lstEmails;
                
                return _cartCoadSRV.SalvarCartCoad(cartCoad);
                
            }

            return null;          

        }

        public cart_coadDTO SalvarComoClienteLegado(ClienteDto cliente)
        {
            if (cliente != null && cliente.CLI_ID != null)
            {
                //RepresentanteDTO rep = _representanteSRV.FindById(REP_ID);
                cart_coadDTO cartCoad = new cart_coadDTO();

                cartCoad.CLI_ID = cliente.CLI_ID;
                cartCoad.NOME = StringUtil.Truncate(cliente.CLI_NOME, 35);
                cartCoad.A_C = StringUtil.Truncate(cliente.CLI_A_C, 35);

                if (cliente.CLIENTES_ENDERECO != null && cliente.CLIENTES_ENDERECO.Count() > 0)
                {
                    var end = _endSRV.BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliente);
                    //var end = cliente.CLIENTES_ENDERECO.Where(x => x.END_TIPO == 1).FirstOrDefault();

                    if (end != null)
                    {
                        cartCoad.TIPO = _cartCoadSRV.PreencherTipoLogradouro(end.TIPO_LOG_ID);
                        cartCoad.LOGRAD = StringUtil.Truncate(end.END_LOGRADOURO, 50);
                        cartCoad.NUMERO = end.END_NUMERO;
                        cartCoad.TIPO_COMPL = end.TIPO_COMP_ID;
                        cartCoad.COMPL = StringUtil.Truncate(end.END_COMPLEMENTO, 15);
                        cartCoad.BAIRRO = StringUtil.Truncate(end.END_BAIRRO, 20);
                        cartCoad.UF = end.END_UF;
                        cartCoad.CEP = end.END_CEP;
                        cartCoad.IDENTIFICACAO = "P";
                        cartCoad.DATA_CADASTRO = DateTime.Now.ToString("dd/MM/yyyy");

                        switch (cliente.TIPO_CLI_ID)
                        {

                            case 1: cartCoad.TP_PESSOA = "J";
                                break;
                            case 2: cartCoad.TP_PESSOA = "F";
                                break;
                            case 3: cartCoad.TP_PESSOA = "J";
                                break;
                            default: cartCoad.TP_PESSOA = "F";
                                break;
                        }

                        if (end.MUNICIPIO != null)
                        {
                            cartCoad.MUNIC = end.MUNICIPIO.MUN_DESCRICAO;
                        }
                    }
                }

                clienteLegDTO cli = new clienteLegDTO();            
                cli.CGC = cliente.CLI_CPF_CNPJ;
                cli.DATA_INSERT = DateTime.Now;

                cartCoad.CLIENTES = cli;

                var cartCoadRetorno = _cartCoadLegadoSRV.SalvarCartCoad(cartCoad);

                if (cartCoadRetorno != null)
                {
                    int codigoInt = 0;

                    if (int.TryParse(cartCoadRetorno.CODIGO, out codigoInt))
                    {
                        cliente.CODIGO_ANTIGO = codigoInt;
                        Merge(cliente);
                    }
                }
                return cartCoad;
            }

            return null;

        }

        private IList<TelefoneProspectDTO> _processarCriacaoTelefoneProspect(ClienteDto cliente)
        {

            IList<TelefoneProspectDTO> listaRetorno = new List<TelefoneProspectDTO>();

            TipoTelefoneSRV tipoTelefoneSRV = null;

            if (cliente != null)
            {

                var lstTelefone = cliente.ASSINATURA_TELEFONE;

                lstTelefone = removerTelefonesDuplicados(lstTelefone);
                
                if (lstTelefone != null)
                {

                    foreach (var tel in lstTelefone)
                    {

                        TelefoneProspectDTO telProp = new TelefoneProspectDTO();
                        telProp.DDD_TEL = tel.ATE_DDD;
                        telProp.TELEFONE = StringUtil.Truncate(tel.ATE_TELEFONE, 8, true);

                        if (tel.TIPO_TELEFONE == null)
                        {
                            if (tipoTelefoneSRV == null)
                                tipoTelefoneSRV = new TipoTelefoneSRV();

                            tel.TIPO_TELEFONE = tipoTelefoneSRV.FindById(tel.TIPO_TEL_ID);
                        }

                        if (tel.TIPO_TELEFONE != null)
                            telProp.TIPO = tel.TIPO_TELEFONE.TIPO_TEL_DESCRICAO;
                        else
                        {
                            telProp.TIPO = "TELEFONE";
                        }

                        listaRetorno.Add(telProp);

                    }

                }

            }

            return listaRetorno;

        }

        private IList<EmailsProspDTO> _processarCriacaoEmailProspect(ClienteDto cliente)
        {
            IList<EmailsProspDTO> listaRetorno = new List<EmailsProspDTO>();

            if (cliente != null)
            {
                var lstEmail = cliente.ASSINATURA_EMAIL;

                if (lstEmail != null)
                {
                    foreach (var email in lstEmail)
                    {
                        EmailsProspDTO emailProp = new EmailsProspDTO();
                        emailProp.E_MAIL = StringUtil.Truncate(email.AEM_EMAIL, 50);

                        if (email.OPC_ID == 2)
                        {
                            emailProp.TIPO = "P";
                        }
                        else if (email.OPC_ID == 3)
                        {
                            emailProp.TIPO = "C";
                        }
                        else
                        {
                            emailProp.TIPO = "N";
                        }

                        listaRetorno.Add(emailProp);
                    }
                }
            }
            return listaRetorno;
        }
        public void AtualizarUra(string _assinatura, int? _qtde, Boolean? _ativo)
        {
            try
            {
                coadSRV _coad = new coadSRV();
                AssinaturaSRV _ass = new AssinaturaSRV();
                AssinaturaDTO _assdto = _ass.FindById(_assinatura);
                ClienteSRV _clisrv = new ClienteSRV();
                
                var _listaura = _coad.BuscarPorAssinatura(_assinatura);

                //using (TransactionScope scope = new TransactionScope())
                //{
                // Retirei a transação porque o mysql não aceita transação distribuida.

                    if (_listaura.Count > 0)
                    {
                        foreach (var _item in _listaura)
                        {
                            if (_qtde != null)
                                _item.qte_cons = (_qtde * 3);

                            if (_ativo != null)
                                _item.pode = _ativo == true ? 1 : 0;

                            //----- Atualizando bancos de dados das uras (RJ,MG e (PR - Não esta ativa))
                            _coad.Merge(_item, "id");

                            _clisrv.GravarHistorico(3, _assdto.CLI_ID, _assdto.ASN_NUM_ASSINATURA, "Atualização da URA - " + _item.telefone.ToString(), 4);

                            //new coadSRV("URAMG").Merge(_item, "id");
                            //new coadSRV("URAPR").Merge(_item, "id");
                        }
                    }
                    else
                    {
                        AssinaturaSenhaDTO _assdtosenha = new AssinaturaSenhaSRV().FindById(_assinatura);
                    
                        ClienteDto _cliente = new ClienteSRV().FindById(_assdto.CLI_ID);
                        ProdutosDTO _produto =  new ProdutosSRV().FindById(_assdto.PRO_ID);
                        var _ende = new ClienteEnderecoSRV().BuscarEnderecoDeFaturamentoOuEnderecoPadrao(_cliente);

                        IList<UraProdutoAreaDTO> _listaacesso = new List<UraProdutoAreaDTO>();

                        if (_ende.END_UF == "RJ")
                            _listaacesso = new UraProdutoAreaSRV().BuscarAreas("URARJ", (int)_assdto.PRO_ID, _ende.END_UF);
                        else
                            _listaacesso = new UraProdutoAreaSRV().BuscarAreas("URAMG", (int)_assdto.PRO_ID, _ende.END_UF);

                        var _acesso = "";

                        foreach (var _item in _listaacesso)
                        {
                            _acesso += _item.ACO_ID.ToString();
                        }

                        if (_acesso == "" || _acesso == null)
                            _acesso = "123";


                        var _telefones = new AssinaturaTelefoneSRV().BuscarTelefonesURA(_assinatura);

                        foreach (var _item in _telefones)
                        {
                            coadDTO _uraitem = new coadDTO();

                            _uraitem.vip = 1;
                            _uraitem.ddd = int.Parse(_item.ATE_DDD);
                            _uraitem.telefone = int.Parse(_item.ATE_TELEFONE);
                            _uraitem.senha = int.Parse(_assdtosenha.ASN_SENHA);
                            _uraitem.codigo = _assdto.ASN_NUM_ASSINATURA;
                            _uraitem.nome = _cliente.CLI_NOME;
                            _uraitem.pode = _ativo == true ? 1 : 0; 
                            _uraitem.qte_cons = (_qtde * 3);
                            _uraitem.acesso = int.Parse(_acesso);
                            _uraitem.qte_realiz = 0;
                            _uraitem.grupo = _produto.FAM_ID.ToString();

                            _coad.Save(_uraitem);

                            _clisrv.GravarHistorico(3, _assdto.CLI_ID, _assdto.ASN_NUM_ASSINATURA, "Inclusão na URA - " + _item.ATE_TELEFONE, 4);

                        }
                    }

                //    scope.Complete();
                //}


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

                throw new Exception(SysException.Show(ex));
            }


        }

        private void MarcarClienteComoForaDaAgenda(IEnumerable<BuscarClienteDTO> lstClientes)
        {
            if (lstClientes != null)
            {
                foreach (var cli in lstClientes)
                {
                    cli.ClienteNaAgenda = false;
                }
            }
        }


        public bool ChecaClienteExisteDentroDaAgenda(int? CLI_ID)
        {
            return _dao.ChecaClienteExisteDentroDaAgenda(CLI_ID);
        }

        /// <summary>
        /// Busca o cliente e checa se ele existe dentro da agenda.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trazInfoMarketing"></param>
        /// <param name="trazClienteTelefone"></param>
        /// <param name="trazAssinaturaEmail"></param>
        /// <param name="validaEnderecoBasica"></param>
        /// <returns></returns>
        public ClienteDto findByIdFullLoadedECheca(int? id, 
            bool trazInfoMarketing = false, 
            bool trazClienteTelefone = false, 
            bool trazAssinaturaEmail = false, 
            bool validaEnderecoBasica = false)
        {
            var obj = FindByIdFullLoaded(id, true, true, true, false);

            if(obj != null)
                obj.ClienteExisteNaAgenda = ChecaClienteExisteDentroDaAgenda(id);

            return obj;
        }
     

        /// <summary>
        /// Importa um cliente fora da agenda para a mesma.
        /// </summary>
        /// <param name="importacaoDTO"></param>
        public void ImportarClienteAgenda(ImportarClienteAgendaDTO importacaoDTO)
        {
            if (importacaoDTO != null && importacaoDTO.CLI_ID != null)
            {
                using (var scope = new TransactionScope())
                {
                    var cliente = FindById(importacaoDTO.CLI_ID);

                    var lstTelefones = importacaoDTO.ASSINATURA_TELEFONE;
                    var lstEmails = importacaoDTO.ASSINATURA_EMAIL;

                    _assinaturaTelefone.CopiarTelefonesESalvar(cliente, lstTelefones);
                    _assinaturaEmail.CopiarEmailsESalvar(cliente, lstEmails);

                    if (importacaoDTO.RG_ID == null)
                    {
                        var repId = importacaoDTO.REP_ID;
                        var repIdDemandante = importacaoDTO.REP_ID_DEMANDANTE;
                        _carteiramentoSRV.EncarteirarSuspectPorRepresentante(cliente, (int)repId, (int) repIdDemandante);
                    }
                    else if (importacaoDTO.RG_ID != null)
                    {
                        var rgId = importacaoDTO.RG_ID;
                        var repIdDemandante = importacaoDTO.REP_ID_DEMANDANTE;

                        _carteiramentoSRV.EncarteirarSuspectPorRodizio(cliente, rgId, (int)repIdDemandante);
                    }

                    scope.Complete();
                }
            }
        }

        
        /// <summary>
        /// Tenta achar o cliente no banco corporativo antigo.
        /// Primeiro tentar achar pelo CLI_ID. 
        /// Se não achar, tenta buscar pelo cpf_cnpj e cria um vínculo com o cliente normal pelo CLI_ID.
        /// Se não achar, Insere um novo cliente no banco legado e cria um vínculo com o cliente normal pelo CLI_ID
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public cart_coadDTO TentarBuscarClienteLegado(ClienteDto cliente)
        {
            if (cliente != null && cliente.CLI_ID != null)
            {
                cart_coadDTO cart_coad = null;
                var cliId = cliente.CLI_ID;
                
                // Tenta por cliId
                cart_coad = _cartCoadLegadoSRV.BuscarPrimeiroCartCoadPorCliId(cliId);

                if (cart_coad != null)
                {
                    return cart_coad;
                }

                // Tenta por cpf/cnpj
                var cnpjCpf = cliente.CLI_CPF_CNPJ;

                if(!string.IsNullOrWhiteSpace(cnpjCpf)){

                    cart_coad = _cartCoadLegadoSRV.BuscarPrimeiroCartCoadPorCnpjCpf(cnpjCpf);

                    if (cart_coad != null)
                    {
                        var codigoCartCoad = cart_coad.CODIGO;

                        int codigoInt = 0;

                        if (int.TryParse(codigoCartCoad, out codigoInt))
                        {
                            cliente.CODIGO_ANTIGO = codigoInt;
                            cart_coad.CLI_ID = cliId;

                            Merge(cliente);
                         cart_coad = _cartCoadLegadoSRV.Merge(cart_coad);
                        }
                    
                        return cart_coad;
                    }
                }
                
                // Insere e retorna
                cart_coad = SalvarComoClienteLegado(cliente);

                return cart_coad;
            }

            return null;
        }

        public void ChecaClientePodeEmitirPedido(int? cliId)
        {
            if (cliId == null)
            {
                throw new PedidoException("Código do cliente não informado.");
            }

            var cliente = FindByIdFullLoaded((int) cliId, true, true, true);

            AtribuirTipoDeCliente(cliente);

            if (cliente.CLA_CLI_ID == 1)
            {
                throw new PedidoException("Não é possível emitir um pedido. O cliente ainda é um suspect.");
            }

            VerificarDadosCliente(cliente, "emitir pedido");


        }

        public void InsersaoEmMassaDeClientes(IList<ClienteDto> lstCliente, ContextoImportacaoDTO contextoImportacao,int? REP_ID_DEMANDANTE = null)
        {
            //if (contextoImportacao != null)
            //{
            //    contextoImportacao.IniciarTempoImportacao();
            //}

            //var lstClientesNovos = new List<ClienteDto>();

            //if (lstCliente != null && lstCliente.Count() > 0 && contextoImportacao != null)
            //{
            //    DateTime data = DateTime.Now;

            //    foreach (var cli in lstCliente)
            //    {
            //        if (cli.CLI_ID != null)
            //        {
            //            cli.DATA_ALTERA = data;
            //            contextoImportacao.QuantidadeClienteEncontrados++;
            //        }
            //        else
            //        {
            //            cli.DATA_CADASTRO = data;
            //            AtribuirTipoDeCliente(cli);
            //            lstClientesNovos.Add(cli);
            //            cli.CLI_IMPORTADO = true;

            //            if (contextoImportacao != null)
            //            {
            //                contextoImportacao.QuantidadeDeClientesNovos++;
            //            }
            //        }
            //    }


            //    var lstClienteRetorno = AtualizarClienteImportacao(lstCliente, contextoImportacao);
            //    //------------ Criando prioridades para os clientes-----------------------------------------
            //    int index = 0;

            //    if (lstClienteRetorno != null && lstCliente.Count() == lstClienteRetorno.Count())
            //    {
            //        var listClienteRetorno = lstClienteRetorno.ToList();
            //        foreach (var cli in lstCliente)
            //        {
            //            if (cli.CLI_ID == null)
            //            {
            //                cli.CLI_ID = listClienteRetorno[index].CLI_ID;
            //            }                        
            //            index++;
            //        }

            //         contextoImportacao.IniciarPassoBatch("Salvando dados complementares do cliente", true);
            //         contextoImportacao.BatchStatus.TotalItens = 4;

            //        // ----------- Salvando informações complementares do cliente -----------------
            //        _endSRV.SalvarEnderecosVariosClientes(lstCliente);
            //        contextoImportacao.BatchStatus.ProcessedItens++;
                                        
            //        _assinaturaTelefone.SalvarEExcluirTelefonesDeVariosClientes(lstCliente);
            //        contextoImportacao.BatchStatus.ProcessedItens++;
                    
            //        _assinaturaEmail.SalvarEExcluirEmailsDeVariosClientes(lstCliente);
            //        contextoImportacao.BatchStatus.ProcessedItens++;
                    
            //        _infMarketingSRV.SalvarVariasInfoMarketing(lstCliente);
            //        contextoImportacao.BatchStatus.ProcessedItens++;


            //        //------------ Encarteirando clientes ------------------------------------------
                    
            //        // agrupando as regioes
            //        var lstRegioes = lstClientesNovos
            //            .OrderBy(x => x.RegiaoIdParaRodizio)
            //            .Select(sel => sel.RegiaoIdParaRodizio)
            //            .Distinct();

            //        foreach (var rgId in lstRegioes)
            //        {
            //            var lstClientesDaRegiao = lstClientesNovos.Where(x => x.RegiaoIdParaRodizio == rgId);
            //            _carteiramentoSRV.EncarteirarVariosClientes(lstClientesDaRegiao, rgId, contextoImportacao);
            //        }

            //        _prioridadeAtendimento.CriarPrioridadesAtendimento(lstCliente, contextoImportacao, REP_ID_DEMANDANTE);               
                    
            //    }
            //}

            //if (contextoImportacao != null)
            //{
            //    contextoImportacao.PararTempoImportacao();
            //    contextoImportacao.BatchStatus.IsRunning = false;
            //}
        }

        public IEnumerable<ClienteDto> AtualizarClienteImportacao(IEnumerable<ClienteDto> lstCliente, ContextoImportacaoDTO contextoImportacao)
        {
            //Dictionary<int?, ICollection<ClienteEnderecoDto>> lstEnde = new Dictionary<int?, ICollection<ClienteEnderecoDto>>();
            //Dictionary<int?, ICollection<AssinaturaTelefoneDTO>> lstTel = new Dictionary<int?, ICollection<AssinaturaTelefoneDTO>>();
            //Dictionary<int?, ICollection<AssinaturaEmailDTO>> lstEmail = new Dictionary<int?, ICollection<AssinaturaEmailDTO>>();
            //Dictionary<int?, InfoMarketingDTO> lstInfoMkt = new Dictionary<int?, InfoMarketingDTO>();

            //IList<ClienteDto> clientesParaProcessar = null;

            //if (lstCliente is List<ClienteDto>)
            //{
            //    clientesParaProcessar = lstCliente as List<ClienteDto>;
            //}
            //else
            //{
            //    clientesParaProcessar = lstCliente.ToList();
            //}

            //var index = 0;
            //foreach (var cli in clientesParaProcessar)
            //{

            //    if (cli.CLIENTES_ENDERECO != null)
            //    {
            //        lstEnde.Add(index, cli.CLIENTES_ENDERECO);
            //        cli.CLIENTES_ENDERECO = null;
            //    }

            //    if (cli.ASSINATURA_TELEFONE != null)
            //    {
            //        lstTel.Add(index, cli.ASSINATURA_TELEFONE);
            //        cli.ASSINATURA_TELEFONE = null;
            //    }

            //    if (cli.ASSINATURA_EMAIL != null)
            //    {
            //        lstEmail.Add(index, cli.ASSINATURA_EMAIL);
            //        cli.ASSINATURA_EMAIL = null;
            //    }

            //    if (cli.INFO_MARKETING != null)
            //    {
            //        lstInfoMkt.Add(index, cli.INFO_MARKETING);
            //        cli.INFO_MARKETING = null;
            //    }
            //    index++;

            //}

            if (contextoImportacao != null && contextoImportacao.BatchContext != null)
            {
                contextoImportacao.IniciarPassoBatch("Salvando os dados do cliente...", true);
            }

            var lstClientesRetorno = BulkInsertOrMerge(lstCliente);

            //index = 0;
            //foreach (var cli in lstClientesRetorno)
            //{
            //    if (cli.CLI_ID != null)
            //    {
            //        if (lstEnde.Keys.Contains(index))
            //        {
            //            cli.CLIENTES_ENDERECO = lstEnde[index];
            //        }

            //        if (lstTel.Keys.Contains(index))
            //        {
            //            cli.ASSINATURA_TELEFONE = lstTel[index];
            //        }

            //        if (lstEmail.Keys.Contains(index))
            //        {
            //            cli.ASSINATURA_EMAIL = lstEmail[index];
            //        }

            //        if (lstInfoMkt.Keys.Contains(index))
            //        {
            //            cli.INFO_MARKETING = lstInfoMkt[index];
            //        }
            //    }

            //    index++;
            //}

            return lstClientesRetorno;

        }

        //public void EncarteirarESalvarVariosClientes(IEnumerable<ClienteDto> lstCliente, ContextoImportacaoDTO contextoImportacao = null, int? REP_ID_DEMANDANTE = null)
        //{
        //    if (contextoImportacao != null)
        //    {
        //        contextoImportacao.IniciarTempoImportacao();
        //    }

        //    if (lstCliente != null && lstCliente.Count() > 0)
        //    {
        //        DateTime data = DateTime.Now;

        //        foreach (var cli in lstCliente)
        //        {
        //            if (cli.CLI_ID != null)
        //            {
        //                cli.DATA_ALTERA = data;
        //                cli.CLI_IMPORTADO = true;
        //            }
        //            else
        //            {
        //                cli.DATA_CADASTRO = data;
        //            }
        //        }

        //        AtualizarClienteImportacao(lstCliente, contextoImportacao);                
        //            //------------ Criando prioridades para os clientes-----------------------------------------

        //            _prioridadeAtendimento.CriarPrioridadesAtendimento(lstCliente, contextoImportacao, REP_ID_DEMANDANTE);
                    
        //        }            


        //    if (contextoImportacao != null)
        //    {
        //        contextoImportacao.PararTempoImportacao();
        //    }
        //}


        public ClienteDto BuscarClientesJaExistentes(IList<string> listaCNPJ_CPF, IList<string> listaTelefones, IList<string> listaEmails)
        {
            return _dao.BuscarClientesJaExistentes(listaCNPJ_CPF, listaTelefones, listaEmails);
        }

        public void ExcluirClienteDaValidacao(int? cliId)
        {
            var cliente = FindById(cliId);

            if (cliente != null)
            {
                cliente.CLI_EXCLUIDO_VALIDACAO = true;
            }

            SaveOrUpdate(cliente);
        }

        public Pagina<BuscarClienteDTO> BuscarProspects(
              string cpf_cnpj = null,
            string nome = null,
            string email = null,
            string dddTelefone = null,
            string telefone = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            int pagina = 1,
            int registroPorPagina = 30,
            int? codigoCliente = null)
        {
            return _dao.BuscarProspects(
                cpf_cnpj, 
                nome, 
                email, 
                dddTelefone, 
                telefone, 
                pesquisaCpfCnpjPorIqualdade,
                pagina, 
                registroPorPagina,
                codigoCliente);
        }

        public ClienteDto BuscarPorAssinatura(string codAssinatura)
        
        {
            if (!string.IsNullOrWhiteSpace(codAssinatura))
            {
                var assinatura = _assinaturaSRV.FindById(codAssinatura);
                
                if (assinatura != null && assinatura.CLI_ID != null)
                {
                    var cliId = assinatura.CLI_ID;
                    return FindById(cliId);
                }
            }

            return null;
        }

        /// <summary>
        /// Busca todos os clientes do sistema, independente de carteira ou UEN.
        /// </summary>
        /// <param name="cpf_cnpj"></param>
        /// <param name="nome"></param>
        /// <param name="email"></param>
        /// <param name="dddTelefone"></param>
        /// <param name="telefone"></param>
        /// <param name="pesquisaCpfCnpjPorIqualdade"></param>
        /// <param name="pagina"></param>
        /// <param name="registroPorPagina"></param>
        /// <param name="codigoCliente"></param>
        /// <returns></returns>
        //public Pagina<BuscarClienteDTO> BuscarClienteGlobal(PesquisaClienteDTO pesquisaDTO)
        //{
        //    return _dao.BuscarClienteGlobal(pesquisaDTO);
        //}

        public bool ChecarProspectDoRepresentante(int? cliId, int? repId)
        {
            return _dao.ChecarProspectDoRepresentante(cliId, repId);
        }

        public ClienteDto ChecarERetornarDadosDeCliente(int? cliId, int? repId = null,
         bool trazInfoMarketing = false,
         bool trazClienteTelefone = false,
         bool trazAssinaturaEmail = false,
         bool validaEnderecoBasica = false)
        {
            bool pertence = true;

            if (repId != null)
                pertence = ChecarProspectDoRepresentante(cliId, repId);

            if (pertence)
            {
                return findByIdFullLoadedECheca(cliId,
                    trazInfoMarketing,
                    trazClienteTelefone,
                    trazAssinaturaEmail,
                    validaEnderecoBasica);
            }
            else
            {
                throw new CarteiramentoException("O cliente não pertence à esse representante.");
            }
        }
        

        public void ValidacaoTotalCliente(ClienteDto cliente, string contextoValidacao)
        {
            cliente.ValidarCPF_CNPJ = true;
            cliente.ValidacaoTotal = true;
            if(cliente.CLIENTES_ENDERECO == null ||
                cliente.CLIENTES_ENDERECO.Count() <= 0)
            {
                _endSRV.PreencherEnderecoCliente(cliente);
            }
            
            if(cliente.CLIENTES_ENDERECO != null && cliente.CLIENTES_ENDERECO.Count() > 0)
            {
                foreach(var end in cliente.CLIENTES_ENDERECO)
                {
                    end.validacaoTotal = true;
                }
            }

            if(cliente.ASSINATURA_EMAIL == null ||
                cliente.ASSINATURA_EMAIL.Count() <= 0)
            {
                _assinaturaEmail.PreencherEmailAssinaturaNoCliente(cliente);
            }

            if(cliente.ASSINATURA_TELEFONE == null ||
                cliente.ASSINATURA_TELEFONE.Count() <= 0)
            {
                _assinaturaTelefone.PreencherTelefoneAssinaturaNoCliente(cliente);
            }

            var result = ValidatorProxy.RecursiveValidate<ClienteDto>(cliente);
            var emailValido = _assinaturaEmail.RetornarEmailDeContato(cliente.CLI_ID);

            if(emailValido == null)
            {
                result.AddModelError("Emails", "Não existe nenhum E-Mail ou E-Mail Válido.");
            }

            if (!result.IsValid)
            {
                throw new ValidacaoException(string.Format("Ocorreu um erro de validação o(a) {0}", contextoValidacao), result);
            }
        }

        /// <summary>
        /// Utiliza o campo TIPO_CLI_ID e faz o DE-PARA para converter o tipo equivalente e inserir no campo CLI_TP_PESSOA
        /// </summary>
        /// <param name="cliente"></param>
        private void SincronizarTipoDeCliente(ClienteDto cliente)
        {
            if(cliente != null)
            {
                switch (cliente.TIPO_CLI_ID)
                {

                    case 1:
                        cliente.CLI_TP_PESSOA = "G";
                        break;
                    case 2:
                        cliente.CLI_TP_PESSOA = "F";
                        break;
                    case 3:
                        cliente.CLI_TP_PESSOA = "J";
                        break;
                    case 4:
                        cliente.CLI_TP_PESSOA = "G";
                        break;
                    default:
                        cliente.CLI_TP_PESSOA = "F";
                        break;
                }
            }
        }
        private void SincronizarTipoPessoa(ClienteDto cliente)
        {
            if (cliente != null)
            {
                switch (cliente.CLI_TP_PESSOA)
                {

                    case "G":
                        cliente.TIPO_CLI_ID = 1;
                        break;
                    case "F":
                        cliente.TIPO_CLI_ID = 2;
                        break;
                    case "J":
                        cliente.TIPO_CLI_ID = 3;
                        break;
                    default:
                        cliente.TIPO_CLI_ID = 2;
                        break;

                }
            }
        }

        public IList<ClienteDto> ListarClientesPorIds(ICollection<int?> lstIds)
        {
            return _dao.ListarClientesPorIds(lstIds);
        }

        public IList<ClienteDto> EncontrarClientesSimilares(int? cliId)
       {
           if(cliId != null)
           {
               var cliente = this.FindByIdFullLoaded(cliId, new BuscarClientePorIdDTO()
               {
                   trazAssinaturaEmail = true,
                   trazClienteTelefone = true,                    
               });

               ICollection<AssinaturaTelefoneDTO> lstTelefone = null;
               ICollection<AssinaturaEmailDTO> lstEmail = null;

               var cpf_cnpj = cliente.CLI_CPF_CNPJ;
               lstTelefone = cliente.ASSINATURA_TELEFONE;
               lstEmail = cliente.ASSINATURA_EMAIL;

               var telefonesAssinatura = _assinaturaTelefone.ListarDeTodasAsAssinaturas(cliId);
               var emailsAssinatura = _assinaturaEmail.ListarDeTodasAsAssinaturas(cliId);

               if(telefonesAssinatura != null)
               {
                   lstTelefone = ((lstTelefone != null) ? lstTelefone.Concat(telefonesAssinatura) : telefonesAssinatura).ToList();
               }

               if (emailsAssinatura != null)
               {
                   lstEmail = ((lstEmail != null) ? lstEmail.Concat(emailsAssinatura) : emailsAssinatura).ToList();
               }

               return ListarClientesSimilares(cliId, cpf_cnpj, lstTelefone, lstEmail);
           }

           return null;
       }
        public IList<ClienteDto> ListarClientesSimilares(int? cliIdExcluir, string cpf_cnpj, ICollection<AssinaturaTelefoneDTO> lstTelefones = null, ICollection<AssinaturaEmailDTO> lstEmails = null)
        {
            return _dao.ListarClientesSimilares(cliIdExcluir, cpf_cnpj, lstTelefones, lstEmails);
        }

        public ValidacaoClienteInadimplenteDTO ExecutarValidacaoDeInadimplencia(int? cliId, int? tppId, string assinatura, int? prtIdExcluir = null)
        {
            if (cliId != null)
            {
                var cliente = FindById(cliId);
                var dadosInadimplencia = ObterDadosDeInadimplencia(cliente, tppId, assinatura, prtIdExcluir);

                IList<ClienteDto> lstCliente = null;
                ICollection<ClienteInadimplenteItemDTO> lstClientesInadimplentes = null;
                if (tppId == 1)
                {
                    lstClientesInadimplentes = new HashSet<ClienteInadimplenteItemDTO>();
                    lstCliente = EncontrarClientesSimilares(cliId);

                    if (lstCliente != null && lstCliente.Count() > 0)
                    {
                        foreach (var cli in lstCliente)
                        {
                            var dadosInadimplente = ObterDadosDeInadimplencia(cli, tppId, null, prtIdExcluir);

                            if (dadosInadimplente.ExisteInadimplencia)
                                lstClientesInadimplentes.Add(dadosInadimplente);
                        }
                    }
                }
                ValidacaoClienteInadimplenteDTO validacaoInadimplente = new ValidacaoClienteInadimplenteDTO()
                {
                    ClienteInadimplente = dadosInadimplencia,
                    ClientesSimilaresInadimplentes = lstClientesInadimplentes
                };

                return validacaoInadimplente;
            }

            return null;
        }

        private ClienteInadimplenteItemDTO ObterDadosDeInadimplencia(ClienteDto cliente, int? tppId, string assinatura = null, int? prtIdExcluir = null)
        {
            if (cliente != null)
            {
                DateTime? data = DateTime.Now;
                data = DateUtil.AdicionaDia(data, -1);
                IList<ParcelasDTO> lstParcelas = null;

                if (tppId != null && tppId == 2 && !string.IsNullOrWhiteSpace(assinatura))
                {
                    lstParcelas = ServiceFactory.RetornarServico<ParcelasSRV>().ListarParcelasEmAbertoDaAssinatura(assinatura, data.Value);
                }
                else
                {
                    lstParcelas = ServiceFactory.RetornarServico<ParcelasSRV>().ListarParcelasFaturadasEmAberto(cliente.CLI_ID.Value, data.Value, prtIdExcluir);
                }

                var dadosInadimplencia = new ClienteInadimplenteItemDTO()
                {
                    cpfCnpj = cliente.CLI_CPF_CNPJ,
                    NomeCliente = cliente.CLI_NOME,
                    CodigoCliente = cliente.CLI_ID,
                    Parcelas = lstParcelas,
                    assinatura = assinatura
                };
                return dadosInadimplencia;
            }
            return null;
        }

        
        public void SalvarClientesImportacao(ClienteDto cliente, ContextoImportacaoDTO context, int? REP_ID_DEMANDANTE = null)
        {
            SalvarClientesImportacao(new List<ClienteDto>() { cliente }, context, REP_ID_DEMANDANTE);
        }

        public void SalvarClientesImportacao(IList<ClienteDto> lstCliente, ContextoImportacaoDTO context, int? REP_ID_DEMANDANTE = null)
        {

            if (lstCliente != null && lstCliente.Count() > 0)
            {
                if(context == null)
                {
                    throw new Exception("As informações contextuais da importação não forma encotradas");
                }

                if(context.ImportacaoSuspect == null)
                {
                    throw new Exception("As informações da importação do suspect não foram encontradas");
                }

                ImportacaoSuspectDTO importacaoSusp = context.ImportacaoSuspect;

                DateTime data = DateTime.Now;

                foreach (var cli in lstCliente)
                {
                    if (cli.CLI_ID != null)
                    {
                        cli.DATA_ALTERA = data;
                                               
                    }
                    else
                    {
                        cli.DATA_CADASTRO = data;
                        AtribuirTipoDeCliente(cli);
                        cli.CLI_IMPORTADO = true;

                        if (context != null)
                        {
                            //context.QuantidadeDeClientesNovos++;
                        }
                    }
                }


                var lstClienteRetorno = BulkInsertOrMerge(lstCliente);
                //------------ Criando prioridades para os clientes-----------------------------------------
                int index = 0;

                if (lstClienteRetorno != null && lstCliente.Count() == lstClienteRetorno.Count())
                {
                    var listClienteRetorno = lstClienteRetorno.ToList();
                    foreach (var cli in lstCliente)
                    {
                        if (cli.CLI_ID == null)
                        {
                            cli.CLI_ID = listClienteRetorno[index].CLI_ID;
                        }
                        index++;
                    }

                    // ----------- Salvando informações complementares do cliente -----------------
                    _endSRV.SalvarEnderecosVariosClientes(lstCliente);

                    _assinaturaTelefone.SalvarEExcluirTelefonesDeVariosClientes(lstCliente);

                    _assinaturaEmail.SalvarEExcluirEmailsDeVariosClientes(lstCliente);

                    _infMarketingSRV.SalvarVariasInfoMarketing(lstCliente);

                    //------------ Encarteirando clientes ------------------------------------------

                    var importacao = context.Importacao;
                    string usuario = "sistema";
                    int? repID = 1;

                    if (importacao != null)
                    {
                        if (importacao.REP_ID != null)
                            repID = importacao.REP_ID;
                        if (!string.IsNullOrWhiteSpace(importacao.USU_LOGIN))
                            usuario = importacao.USU_LOGIN;
                    }

                    foreach (var cli in lstCliente)
                    {
                        if(cli.DATA_ALTERA == null && cli.DATA_CADASTRO == data)
                        {
                            _histAtendSRV.RegistraHistoricoNovoClienteImportacao(usuario, repID, cli.CLI_ID);
                        }
                    }

                    // agrupando as regioes
                    var lstRegioes = lstCliente
                        .OrderBy(x => x.RegiaoIdParaRodizio)
                        .Select(sel => sel.RegiaoIdParaRodizio)
                        .Distinct();

                    foreach (var rgId in lstRegioes)
                    {
                        var lstClientesDaRegiao = lstCliente.Where(x => x.RegiaoIdParaRodizio == rgId);
                        
                        foreach (var cli in lstClientesDaRegiao)
                        {
                            _carteiramentoSRV.EncarteirarClienteDaImportacao(cli, rgId, context, importacaoSusp.IPS_REP_ID);
                        }
                        
                        _prioridadeAtendimento.CriarPrioridadesAtendimento(lstClientesDaRegiao, rgId, context, REP_ID_DEMANDANTE);
                        
                    }  
                    

                }
            }

            
            //if (context != null)
            //{
            //    context.PararTempoImportacao();
            //    context.BatchStatus.IsRunning = false;
            //}
        }
        public ClienteDto FindByIDImportacaoSuspect(int? ipsID)
        {
            return _dao.FindByIDImportacaoSuspect(ipsID);
        }

        public int? RetornarCodDoClienteDaImportacaoSuspect(int? ipsID)
        {
            var cli  = FindByIDImportacaoSuspect(ipsID);

            if (cli != null)
                return cli.CLI_ID;
            return null;
        }

        public ClienteDto RetornarClienteDaParcela(string codParcela)
        {
            return _dao.RetornarClienteDaParcela(codParcela);
        }

        public ICollection<AssinaturaTelefoneDTO> removerTelefonesDuplicados(ICollection<AssinaturaTelefoneDTO>  lstTelefone)
        {

            ICollection<AssinaturaTelefoneDTO> telefones = new List<AssinaturaTelefoneDTO>();

            string jaCadastrado = "";

            foreach ( AssinaturaTelefoneDTO telefone in lstTelefone )
            {

                string tupla = telefone.ATE_DDD + telefone.ATE_TELEFONE + telefone.TIPO_TEL_ID.ToString();

                if (jaCadastrado.IndexOf(tupla) < 0)
                {

                    telefones.Add(telefone);
                    jaCadastrado += (jaCadastrado.Length < 1 ? tupla : ";" + tupla);

                }

            }

            return telefones;

        }


    }
}
    
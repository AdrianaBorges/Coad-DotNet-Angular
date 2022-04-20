using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes.Maps;
using System.Transactions;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service.Custons;
using GenericCrud.Service;
using COAD.CORPORATIVO.Exceptions;
using GenericCrud.Util;
using COAD.SEGURANCA.Service.Interfaces;
using COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("USC_ID")]
    public class ClienteUsuarioSRV : GenericService<CLIENTE_USUARIO, ClienteUsuarioDTO, int>
    {
        private ClienteUsuarioDAO _dao;
        private AssinaturaSRV _assinaturaSRV { get; set; }
        public ClienteSRV _clienteSRV { get; set; }
        public SenhaSRV _senhaSRV { get; set; }
        public HistAtendSRV _histAted { get; set; }
        public IEmailSRV EmailSRV { get; set; }

        public ClienteUsuarioSRV()
        {
            this._dao = new ClienteUsuarioDAO();
            this._assinaturaSRV = new AssinaturaSRV();
            this._senhaSRV = new SenhaSRV();
        }

        public ClienteUsuarioSRV(
            ClienteUsuarioDAO _dao,
            AssinaturaSRV assinaturaSRV)
        {
            this._dao = _dao;
            Dao = _dao;
            _assinaturaSRV = assinaturaSRV;
        }

        private void RealizarValidacoes(LoginUnicoRequestDTO loginUnicoRequest)
        {
            if (loginUnicoRequest != null)
            {
                if (loginUnicoRequest.AssinaturaPrincipal == null)
                {
                    throw new ConfiguracaoLoginUnicoException("A assinatura principal do cliente não foi informada. Por favor, refaça o processo.");
                }
                var codAssinatura = loginUnicoRequest.AssinaturaPrincipal.ASN_NUM_ASSINATURA;
                var senha = loginUnicoRequest.SenhaAssinatura;

                if (!_assinaturaSRV.TestarSenhaDaAssinatura(codAssinatura, senha))
                {
                    string exMsg = string.Format("A assinatura principal {0} não possuí uma senha válida. Por favor, refaça o processo.", codAssinatura);
                    throw new ConfiguracaoLoginUnicoException(exMsg);
                }

                if (loginUnicoRequest.Assinaturas != null && loginUnicoRequest.Assinaturas.Count() > 0)
                {
                    int index = 0;
                    int tamanho = loginUnicoRequest.Assinaturas.Count();
                    bool erro = false;

                    StringBuilder sb = new StringBuilder();

                    foreach (var ass in loginUnicoRequest.Assinaturas)
                    {

                        if (!_assinaturaSRV.TestarSenhaDaAssinatura(ass.CodAssinatura, ass.SenhaAssinatura))
                        {
                            if (index > 0 && index < (tamanho))
                            {
                                sb.Append(",");
                            }
                            erro = true;
                            sb.Append(ass.CodAssinatura);

                            index++;
                        }
                    }

                    if (erro)
                    {
                        string erroMsg = string.Format("A(s) assinatura(s) {0} possuem a senha incorreta. Por favor, refaça o processo.", sb);
                        throw new ConfiguracaoLoginUnicoException(erroMsg);
                    }
                }
            }
        }

        public void CriarLoginUnico(LoginUnicoRequestDTO loginUnicoRequest)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (loginUnicoRequest != null)
                    {
                        RealizarValidacoes(loginUnicoRequest);

                        IList<string> lstAssinaturas = new List<string>();

                        if (loginUnicoRequest.Assinaturas != null && loginUnicoRequest.Assinaturas.Count() > 0)
                        {
                            lstAssinaturas = loginUnicoRequest
                                .Assinaturas
                                .Select(x => x.CodAssinatura)
                                .ToList();
                        }

                        var cliente = loginUnicoRequest.AssinaturaPrincipal.CLIENTES;
                        _clienteSRV.SalvarClienteAgenda(cliente, true, true);


                        ClienteUsuarioDTO clienteUsuario = new ClienteUsuarioDTO()
                        {
                            CLI_ID = cliente.CLI_ID,
                            CLIENTES = cliente,
                            USC_ATIVO = true,
                            USC_LOGIN = loginUnicoRequest.Login,
                            COD_ASSINATURA_PRINCIPAL = loginUnicoRequest.AssinaturaPrincipal.ASN_NUM_ASSINATURA,
                            USC_SENHA = _senhaSRV.HashMD5(loginUnicoRequest.Senha)
                        };

                        Save(clienteUsuario);

                        _assinaturaSRV.TransferirAssinatura(cliente, lstAssinaturas, loginUnicoRequest);

                        var email = ServiceFactory.RetornarServico<AssinaturaEmailSRV>()
                            .RetornarEmailDeContato(cliente.CLI_ID);

                        if (email != null)
                        {
                            EnviarEmailLoginUnicoCadastrado(cliente.CLI_NOME, clienteUsuario.USC_LOGIN, email.AEM_EMAIL, lstAssinaturas);
                        }

                        _histAted.RegistrarHistoricoCriacaoLoginUnico(cliente.CLI_ID, loginUnicoRequest.Login, lstAssinaturas, loginUnicoRequest.Rastreamentos);

                        scope.Complete();
                    }
                }
                catch (Exception e)
                {
                    throw new ConfiguracaoLoginUnicoException("Ocorreu um problema ao tentar configurar seu novo login único", e);
                }

            }
        }


        public void EnviarEmailLoginUnicoCadastrado(string nomeCliente, string loginCliente, string endEmail, IEnumerable<string> lstCodAssinaturas)
        {
            endEmail = SysUtils.DecidirEnderecoDeEmail(endEmail);
            string lstAssinaturas = null;

            if (lstCodAssinaturas != null)
            {
                StringBuilder sb = new StringBuilder();
                int index = 0;
                int tamanho = lstCodAssinaturas.Count();

                foreach (var ass in lstCodAssinaturas)
                {
                    if (index > 0 && index < (tamanho))
                    {
                        sb.Append(", ");
                    }
                    sb.Append("<strong>");
                    sb.Append(ass);
                    sb.Append("</strong>");

                    index++;
                }

                lstAssinaturas = sb.ToString();

            }
            if (endEmail != null)
            {
                var url = "https://ci4.googleusercontent.com/proxy/GgWnRPBud6_dbgT5a4AZGD1cXJaq7heSiSI6uRSLpqrbeRczzyf8rGzRft8ARSffAAjCKNryW9c1grWR6aZ4DfbBnsH6SAPgdbI5SsEUK5ISOjmLsiZKwAW0iJfwmKPQF_ufrNjh0VNiRRastLGv7F1SB7KA=s0-d-e1-ft#http://emkt.coad.com.br/emkt/dados/10268/10767/Image/Cursos_Novo/Header_Contabilidade_Geral.png";

                var templateEmail =
                    @"<div style='padding:15px;'>
                        <fieldset style='border:none;'>
                            <legend style='font-size:16px; color: #0970a3;'><strong>Configuração do Login Único!!!</strong></legend>
                            <form>
                                <br />
                                <p style='font-size:14px'>
                                    Prezado(a) <h2>{0}</h2>,
                                    Seu O login único foi configurado com êxito. 
                                    <div><label>Login</label> <h3>{1}</h3>.</div>
                                    <br />
                                    Agora você poderá ter acesso a todos os seus produtos em um único login!!
                                    
                                    <div>
                                        <label>Assinaturas associadas a esse login: </label>
                                        <div>{2}</div>
                                    </div>
                                                                       
                                </p>
                                <p>Esse E-Mail é automático. Por Favor, não responder.</p>        
                                <br />
                            </form>
                        </fieldset>                    
                    </div>";


                templateEmail = string.Format(templateEmail, nomeCliente, loginCliente, lstAssinaturas);
                EmailSRV.EnviarEmailParaCliente(endEmail, "(COAD) - Login Único Gerado com Sucesso!!", templateEmail, url);
            }

        }

        public bool ChecarUsuarioJaExiste(string usuario)
        {
            return _dao.ChecarUsuarioJaExiste(usuario);
        }

        public string SugerirUsuario(string codAssinatura)
        {
            var assinatura = _assinaturaSRV.FindByIdFullLoaded(codAssinatura, false, true, false, true);

            if (assinatura != null)
            {
                if (assinatura.ASSINATURA_EMAIL != null && assinatura.ASSINATURA_EMAIL.Count() > 0)
                {
                    var lstEmail = assinatura.ASSINATURA_EMAIL.Select(sel => sel.AEM_EMAIL);

                    foreach (var email in lstEmail)
                    {
                        if (!ChecarUsuarioJaExiste(email))
                        {
                            return email;
                        }
                    }
                }
                else if (assinatura.CLIENTES != null &&
                    assinatura.CLIENTES.ASSINATURA_EMAIL != null &&
                    assinatura.CLIENTES.ASSINATURA_EMAIL.Count() > 0)
                {
                    var lstEmail = assinatura.CLIENTES.ASSINATURA_EMAIL.Select(sel => sel.AEM_EMAIL);

                    foreach (var email in lstEmail)
                    {
                        if (!ChecarUsuarioJaExiste(email))
                        {
                            return email;
                        }
                    }
                }

            }

            return null;

        }

        /// <summary>
        /// Verifica se o cliente já possui algum login cadastrado.
        /// </summary>
        /// <param name="cliId"></param>
        /// <returns></returns>
        public bool ChecarClienteJaPossuiLogin(int? cliId)
        {
            return _dao.ChecarClienteJaPossuiLogin(cliId);
        }

        /// <summary>
        /// Verifica se o cliente já possui algum login cadastrado.
        /// </summary>
        /// <param name="asnAssinatura"></param>
        /// <returns></returns>
        public bool ChecarClienteJaPossuiLogin(string asnAssinatura)
        {
            return _dao.ChecarClienteJaPossuiLogin(asnAssinatura);
        }


        public ClienteUsuarioDTO LogarCliente(string usuario, string senha)
        {
            if (!string.IsNullOrWhiteSpace(senha))
            {
                //var hash = _senhaSRV.HashMD5(senha);
                var cliUsuario = _dao.LogarCliente(usuario, senha);

                if (cliUsuario != null)
                {

                    cliUsuario.Assinaturas = _assinaturaSRV.BuscarResumosDeAssinaturasDoCliente(cliUsuario.CLI_ID);
                }

                return cliUsuario;
            }
            return null;
        }

        public bool LogarClienteEcommerce(UsuarioEcommerce usuario)
        {
            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.senha))
            {
                var hash = _senhaSRV.HashMD5(usuario.senha);
                var clienteUsuario = LogarCliente(usuario.login, hash);
                if(clienteUsuario != null)
                {
                    usuario.Id = clienteUsuario.CLI_ID;
                    usuario.senha = hash;

                    return true;
                }
            }

            throw new Exception("Falha no login. O Usuário/Senha não estão corretos.");
        }
        public bool AdicionarUsuarioEcommerce(UsuarioEcommerce usuario)
        {

            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.senha))
            {

                if ( !ChecarUsuarioJaExiste(usuario.login) )
                {

                    var hash = _senhaSRV.HashMD5(usuario.senha);

                    if (_dao.AdicionarClienteUsuario(usuario.login, hash))
                    {
                        var clienteUsuario = LogarCliente(usuario.login, hash);

                        usuario.Id = clienteUsuario.CLI_ID;
                        usuario.senha = hash;

                        return true;

                    }
                }
                else
                    throw new Exception("Falha na criação. O Usuário já esta cadastrado.");

            }

            return false;
        }
    }
}
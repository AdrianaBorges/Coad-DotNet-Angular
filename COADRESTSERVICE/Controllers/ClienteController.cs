using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using Newtonsoft.Json;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce;
using Newtonsoft.Json.Linq;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;

namespace COADRESTSERVICE.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        public ClienteController(DadosClientePortalSRV dadosClientePortalSRV,
            TipoClienteSRV tipoClienteSRV, ClienteSRV clienteSRV, ClienteEcommerceSRV clienteEcommerceSRV,
            ClienteEnderecoSRV clienteEnderecoSRV, ClienteTelefoneSRV clienteTelefoneSRV,
            TipoEnderecoSRV tipoEnderecoSRV,
            ClienteUsuarioSRV ClienteUsuarioSRV, AssinaturaSenhaSRV AssinaturaSenhaSRV, ClienteIntegracaoSRV clienteIntegracaoSRV)
        {
            this.dadosClientePortalSRV = dadosClientePortalSRV;
            this._tipoCliente = tipoClienteSRV;
            this._clienteSRV = clienteSRV;
            this._clienteEcommerceSRV = clienteEcommerceSRV;
            this._clienteEnderecoSRV = clienteEnderecoSRV;
            this._clienteTelefoneSRV = clienteTelefoneSRV;
            this._tipoEnderecoSRV = tipoEnderecoSRV;
            this.ClienteUsuarioSRV = ClienteUsuarioSRV;
            this.AssinaturaSenhaSRV = AssinaturaSenhaSRV;
            this._loginController = new LoginController(ClienteUsuarioSRV, AssinaturaSenhaSRV);
            this._clienteIntegracaoSRV = clienteIntegracaoSRV;
        }

        private DadosClientePortalSRV dadosClientePortalSRV { get; set; }
        private TipoClienteSRV _tipoCliente { get; set; }
        private ClienteSRV _clienteSRV { get; set; }
        private ClienteEcommerceSRV _clienteEcommerceSRV { get; set; }

        private ClienteEnderecoSRV _clienteEnderecoSRV { get; set; }

        private ClienteTelefoneSRV _clienteTelefoneSRV { get; set; }

        private TipoEnderecoSRV _tipoEnderecoSRV { get; set; }

        private LoginController _loginController { get; set; }

        private ClienteUsuarioSRV ClienteUsuarioSRV { get; set; }
        private AssinaturaSenhaSRV AssinaturaSenhaSRV { get; set; }
        private ClienteIntegracaoSRV _clienteIntegracaoSRV { get; set; }

        [HttpGet("buscar-dados-cliente-st")]
        public JSONResponse BuscarDadosClienteSt(string login, string senha)
        {

            var result = new JSONResponse();

            try
            {
                var dadosCliente = dadosClientePortalSRV.BuscarDadosClienteSt(login, senha);
                result.success = true;
                result.Add("dadosCliente", dadosCliente);
                return result;
            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
                return result;
            }
        }



        [HttpGet("buscar-dados-cliente-contrato")]
        public JSONResponse BuscarDadosClienteContrato(string login, string senha, string produtosId)
        {
            var result = new JSONResponse();

            try
            {
                var dadosCliente = dadosClientePortalSRV.BuscarDadosClienteContrato(login, senha, produtosId);
                result.success = true;
                result.Add("dadosCliente", dadosCliente);
                return result;
            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
                return result;
            }
        }

        [HttpGet("buscar-dados-cliente-cpf-cnpj")]
        public JSONResponse BuscarDadosClientePorCpfCnpj(string cpfCnpj)
        {
            var result = new JSONResponse();
            try
            {

                var dadosCliente = _clienteSRV.BuscarClientesPorCpfCnpj(cpfCnpj);
                var cliIntegr = _clienteIntegracaoSRV.CarregarDadosDoClienteParaIntegracao(dadosCliente[0].CLI_ID);

                result.success = true;
                result.Add("dadosCliente", cliIntegr);

                return result;
            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
                return result;
            }
        }

        [HttpGet("buscar-tipos-ecommerce")]
        public JSONResponse BuscarTiposClienteEcommerce()
        {

            var result = new JSONResponse();

            try
            {
                var lstTipoCliente = _tipoCliente.ListarTipoClienteEcommerce();
                result.success = true;
                result.Add("lstTipoCliente", lstTipoCliente);
                return result;
            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
                return result;
            }
        }

        [HttpGet("buscar-cliente-cliid")]
        public JSONResponse BuscarClientePorCLIIDEcommerce(int cLIID)
        {

            var result = new JSONResponse();

            try
            {

                ClienteDto clienteDto = _clienteEcommerceSRV.ObterClientePorCLIID(cLIID);
                ClienteEnderecoDto clienteEndereco = _clienteEnderecoSRV.BuscarEnderecosDeClienteDto((int)clienteDto.CLI_ID, 1);
                ClienteEnderecoDto clienteEnderecoEntreganto = _clienteEnderecoSRV.BuscarEnderecosDeClienteDto((int)clienteDto.CLI_ID, 2);

                //Dictionary<string, int> dict = new Dictionary<string, int>();
                //dict.Add("CLI_ID", cLIID);

                //ClienteTelefoneDTO clienteTelefone = _clienteTelefoneSRV.Dao.ToDTO(_clienteTelefoneSRV.Dao.FindById(dict));

                _clienteEcommerceSRV.clienteSRV._clienteTelSRV.PreecherClienteTelefone(clienteDto);

                ClienteTelefoneDTO clienteTelefoneDto = clienteDto.CLIENTES_TELEFONE.FirstOrDefault();












                result.success = true;
                result.Add("cliente", clienteDto);
                result.Add("clienteEndereco", clienteEndereco);
                result.Add("clienteEnderecoEntrega", clienteEnderecoEntreganto);
                result.Add("clienteTelefone", clienteTelefoneDto);

                return result;

            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
                return result;
            }
        }


        [HttpGet("buscar-tipo-cliente-cliid")]
        public JSONResponse BuscarTipoPorCLIIDEcommerce(int cLIID)
        {

            var result = new JSONResponse();

            try
            {

                int? tipo = _clienteEcommerceSRV.ObterTipoPorCLIID(cLIID);

                result.success = tipo != null;
                result.Add("tipo", tipo);

                return result;

            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
                return result;
            }
        }


        [HttpPost("cadastrar-cliente")]
        public JSONResponse CadastrarCliente(ClienteIntegrDTO clienteIntegrDTO)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var cliente = _clienteIntegracaoSRV.SalvarClienteIntegracao(clienteIntegrDTO);
                var data = _clienteIntegracaoSRV.CarregarDadosDoClienteParaIntegracao(cliente.CLI_ID);
                result.Add("data", data);
                result.success = true;
            }
            catch(Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
            }
            return result;
        }
        // POST: api/cliente
        [HttpPost("adicionar-cliente-ecommerce")]
        public JSONResponse AdicionarCliente([FromBody] Object clienteJson)
        {

            var result = new JSONResponse();

            try
            {

                ClienteEnderecoDto clienteEnderecoCadastro = new ClienteEnderecoDto();
                ClienteTelefoneDTO clienteTelefoneCadastro = new ClienteTelefoneDTO();
                ClienteEnderecoDto clienteEnderecoEntrega = new ClienteEnderecoDto();
                ClienteTelefoneDTO clienteTelefoneEntrega = new ClienteTelefoneDTO();
                UsuarioEcommerce usuario = new UsuarioEcommerce();

                TipoEnderecoDTO tipoCadastro = _tipoEnderecoSRV._dao.ToDTO(_tipoEnderecoSRV._dao.FindById(2));
                TipoEnderecoDTO tipoEntrega = _tipoEnderecoSRV._dao.ToDTO(_tipoEnderecoSRV._dao.FindById(1));

                var o = JObject.Parse(clienteJson.ToString());

                ClienteDto cliente = new ClienteDto();
                cliente.CLI_NOME = o["clI_NOME"].ToString();
                cliente.CLI_CPF_CNPJ = o["clI_CPF_CNPJ"].ToString().Replace(".", "").Replace("-", "").Replace("/", "");
                cliente.TIPO_CLI_ID = int.Parse(o["tipO_CLI_ID"].ToString());
                cliente.DATA_CADASTRO = DateTime.Now;

                cliente = _clienteEcommerceSRV.CadastrarCliente(cliente);

                var oEnderecoCadastro = o["enderecoCadastro"];


                clienteEnderecoCadastro.CLI_ID = cliente.CLI_ID;
                clienteEnderecoCadastro.CLIENTES = cliente;
                clienteEnderecoCadastro.END_TIPO = 2;
                clienteEnderecoCadastro.TIPO_ENDERECO = tipoCadastro;
                clienteEnderecoCadastro.END_TIPO = tipoCadastro.TP_END_ID;
                clienteEnderecoCadastro.END_LOGRADOURO = oEnderecoCadastro["enD_LOGRADOURO"].ToString();
                clienteEnderecoCadastro.END_NUMERO = oEnderecoCadastro["enD_NUMERO"].ToString();
                clienteEnderecoCadastro.END_COMPLEMENTO = (oEnderecoCadastro["enD_COMPLEMENTO"] != null ? oEnderecoCadastro["enD_COMPLEMENTO"].ToString() : "");
                clienteEnderecoCadastro.END_BAIRRO = oEnderecoCadastro["enD_BAIRRO"].ToString();
                clienteEnderecoCadastro.END_MUNICIPIO = oEnderecoCadastro["enD_MUNICIPIO"].ToString();
                clienteEnderecoCadastro.END_UF = oEnderecoCadastro["enD_UF"].ToString();
                clienteEnderecoCadastro.END_CEP = oEnderecoCadastro["enD_CEP"].ToString().Replace("-", "");

                _clienteEnderecoSRV.SalvarEndereco(clienteEnderecoCadastro, cliente.CLI_ID);

                cliente.CLIENTES_ENDERECO.Add(clienteEnderecoCadastro);

                string tel = (oEnderecoCadastro["telefone"] != null ? oEnderecoCadastro["telefone"].ToString().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : "");

                if (tel.Length > 0)
                {

                    clienteTelefoneCadastro.CLI_ID = (int)cliente.CLI_ID;
                    clienteTelefoneCadastro.CLIENTES = cliente;
                    clienteTelefoneCadastro.TIPO_TEL_ID = "4";
                    clienteTelefoneCadastro.CLI_TEL_DDD = tel.Substring(0, 2);
                    clienteTelefoneCadastro.CLI_TEL_TELEFONE = tel.Substring(2, tel.Length - 2);

                    //_clienteTelefoneSRV.SalvarEExcluirTelefones(clienteTelefoneCadastro);
                    cliente.CLIENTES_TELEFONE.Add(clienteTelefoneCadastro);

                }

                var oEnderecoEntrega = o["enderecoEntrega"];

                if (oEnderecoEntrega.Children().Count() > 0)
                {


                    clienteEnderecoEntrega.CLI_ID = cliente.CLI_ID;
                    clienteEnderecoEntrega.CLIENTES = cliente;
                    clienteEnderecoEntrega.END_TIPO = 1;
                    clienteEnderecoEntrega.TIPO_ENDERECO = tipoEntrega;
                    clienteEnderecoEntrega.END_TIPO = tipoEntrega.TP_END_ID;
                    clienteEnderecoEntrega.END_LOGRADOURO = oEnderecoEntrega["enD_LOGRADOURO"].ToString();
                    clienteEnderecoEntrega.END_NUMERO = oEnderecoEntrega["enD_NUMERO"].ToString();
                    clienteEnderecoEntrega.END_COMPLEMENTO = (oEnderecoEntrega["enD_COMPLEMENTO"] == null ? "" : oEnderecoEntrega["enD_COMPLEMENTO"].ToString());
                    clienteEnderecoEntrega.END_BAIRRO = oEnderecoEntrega["enD_BAIRRO"].ToString();
                    clienteEnderecoEntrega.END_MUNICIPIO = oEnderecoEntrega["enD_MUNICIPIO"].ToString();
                    clienteEnderecoEntrega.END_UF = oEnderecoEntrega["enD_UF"].ToString();
                    clienteEnderecoEntrega.END_CEP = oEnderecoEntrega["enD_CEP"].ToString().Replace("-", "");

                    _clienteEnderecoSRV.SalvarEndereco(clienteEnderecoEntrega);
                    cliente.CLIENTES_ENDERECO.Add(clienteEnderecoEntrega);

                    tel = (oEnderecoEntrega["telefone"] != null ? oEnderecoEntrega["telefone"].ToString().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : "");

                    if (tel.Length > 0)
                    {

                        clienteTelefoneEntrega.CLI_ID = (int)cliente.CLI_ID;
                        clienteTelefoneEntrega.CLIENTES = cliente;
                        clienteTelefoneEntrega.TIPO_TEL_ID = "4";
                        clienteTelefoneEntrega.CLI_TEL_DDD = tel.Substring(0, 2);
                        clienteTelefoneEntrega.CLI_TEL_TELEFONE = tel.Substring(2, tel.Length - 2);

                        //_clienteTelefoneSRV.Salvar(clienteTelefoneEntrega);
                        cliente.CLIENTES_TELEFONE.Add(clienteTelefoneEntrega);

                    }

                }

                cliente.ENDERECO_FATURAMENTO = clienteEnderecoCadastro;
                cliente.ENDERECO_ENTREGA = clienteEnderecoEntrega;

                _clienteTelefoneSRV.SalvarEExcluirTelefones(cliente);

                //var usuarioArray = o["usuario"];

                usuario.login = o["email"].ToString();
                usuario.senha = o["senha"].ToString();
                usuario.cli_id = cliente.CLI_ID;

                _loginController.AdicionarUsuario(usuario.login, usuario.senha, usuario.cli_id);

                //_clienteEnderecoSRV.SalvarEndereco(clienteEnderecoCadastro, cliente.CLI_ID);

                //clienteEnderecoCadastro = _clienteEnderecoSRV.Salvar(clienteEnderecoCadastro);
                //clienteEnderecoEntrega = _clienteEnderecoSRV.Salvar(clienteEnderecoEntrega);


                //clienteEnderecoCadastro.CLI_ID = cliente.CLI_ID;
                //clienteEnderecoEntrega.CLI_ID = cliente.CLI_ID;

                //IEnumerable<ClienteEnderecoDto> lstEnd = new ClienteEnderecoDto[] { clienteEnderecoCadastro, clienteEnderecoEntrega };

                //lstEnd[0] = .Add(clienteEnderecoCadastro);
                //lstEnd.Add(clienteEnderecoEntrega);

                //_clienteEnderecoSRV.BulkInsertOrMerge(lstEnd);

                //cliente = _clienteEcommerceSRV.ObterClientePorCLIID((int) cliente.CLI_ID);

                result.success = cliente != null;

                return result;

            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
                return result;
            }

        }
    }
}

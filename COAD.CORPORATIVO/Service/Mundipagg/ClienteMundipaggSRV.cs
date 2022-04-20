using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using COAD.CORPORATIVO.Service.Base;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Settings.Mundipagg;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace COAD.CORPORATIVO.Service.Mundipagg
{
    public class ClienteMundipaggSRV : MundipaggSRV
    {

        public override MundipaggStt MundipaggSTT { get; set; }
        public ClienteSRV _clienteSRV { get; }
        public MundipaggClienteSRV _mundipaggClienteSRV { get; set; }
        public ClienteIntegracaoSRV _clienteIntegracaoSRV { get; set; }
        public AssinaturaEmailSRV _assinaturaEmailSRV { get; set; }

        public ClienteMundipaggSRV(ClienteSRV clienteSRV)
        {
            this._clienteSRV = clienteSRV;
        }

        public BuscarClienteMundipaggResponseDTO BuscarClienteMundipagg(int? coadCliId)
        {
            if(coadCliId==null)
            {
                throw new Exception("Informar cliente.");
            }

            var mundipaggClienteDTO = _mundipaggClienteSRV.BuscarMundipaggClientePorCliId(coadCliId);
            if (mundipaggClienteDTO == null)
            {
                throw new Exception("Cliente não cadastrado.");
            }

            var mundipaggApi = new MundiAPIClient(MundipaggSTT.SecretKey, "");

            BuscarClienteMundipaggResponseDTO buscarClienteMundipaggDTO = new BuscarClienteMundipaggResponseDTO();
            try
            {
                buscarClienteMundipaggDTO.GetCustomerResponse = mundipaggApi.Customers.GetCustomer(mundipaggClienteDTO.MPG_CLI_CODE);
            }
            catch (Exception)
            {
                throw new Exception("O cliente informado não foi encontrado. ");
            }

            return buscarClienteMundipaggDTO;
        }

        public BuscarClienteMundipaggResponseDTO CadastrarClienteMundipagg(CriarClienteMundipaggRequestDTO criarClienteRequestMundipaggDTO)
        {

            ClienteDto clienteDto = null;
            if (criarClienteRequestMundipaggDTO.ClienteMundipaggDTO.ClientId != null)
            {
                clienteDto = _clienteSRV.FindById(criarClienteRequestMundipaggDTO.ClienteMundipaggDTO.ClientId);
                if (clienteDto == null)
                {
                    throw new Exception("Cliente não cadastrado.");
                }

                bool atualizarClienteDto = false;
                if (string.IsNullOrWhiteSpace(clienteDto.CLI_CPF_CNPJ))
                {
                    if (string.IsNullOrWhiteSpace(criarClienteRequestMundipaggDTO.ClienteMundipaggDTO.Document))
                    {
                        throw new Exception("Informar cpf ou cnpj");
                    }
                    clienteDto.CLI_CPF_CNPJ = criarClienteRequestMundipaggDTO.ClienteMundipaggDTO.Document;
                    atualizarClienteDto = true;
                }
                if (string.IsNullOrEmpty(clienteDto.CLI_NOME))
                {
                    if (string.IsNullOrEmpty(criarClienteRequestMundipaggDTO.ClienteMundipaggDTO.Name))
                    {
                        throw new Exception("Informar nome do cliente");
                    }
                    clienteDto.CLI_NOME = criarClienteRequestMundipaggDTO.ClienteMundipaggDTO.Name;
                    atualizarClienteDto = true;
                }
                if (atualizarClienteDto)
                {
                    clienteDto = _clienteSRV.Update(clienteDto);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(criarClienteRequestMundipaggDTO.ClienteMundipaggDTO.Name))
                {
                    throw new Exception("Informar nome do cliente");
                }
                if (string.IsNullOrWhiteSpace(criarClienteRequestMundipaggDTO.ClienteMundipaggDTO.Document))
                {
                    throw new Exception("Informar cpf ou cnpj");
                }
                var clienteConversaoMundipagg = ConverteClienteMundipaggParaCliente(criarClienteRequestMundipaggDTO.ClienteMundipaggDTO);
                clienteDto = _clienteSRV.Save(clienteConversaoMundipagg);

                var asnEmailCollec = clienteConversaoMundipagg.ASSINATURA_EMAIL.GetEnumerator();
                if (asnEmailCollec.MoveNext())
                {
                    var assinaturaEmailDTO = asnEmailCollec.Current;
                    assinaturaEmailDTO.CLI_ID = clienteDto.CLI_ID;
                    _assinaturaEmailSRV.Salvar(assinaturaEmailDTO);
                }
            }

            var mundipaggCliente = _mundipaggClienteSRV.BuscarMundipaggClientePorCliId(clienteDto.CLI_ID);

            criarClienteRequestMundipaggDTO.CreateCustomerRequest = new CreateCustomerRequest();
            if (mundipaggCliente != null)
            {
                var buscarClienteMundipaggResponseDTO = new BuscarClienteMundipaggResponseDTO();
                buscarClienteMundipaggResponseDTO.GetCustomerResponse = new GetCustomerResponse();
                buscarClienteMundipaggResponseDTO.GetCustomerResponse.Code = mundipaggCliente.MPG_CLI_CODE;
                buscarClienteMundipaggResponseDTO.GetCustomerResponse.Name = mundipaggCliente.MPG_CLI_NAME;
                buscarClienteMundipaggResponseDTO.GetCustomerResponse.Type = mundipaggCliente.MPG_CLI_TYPE;
                buscarClienteMundipaggResponseDTO.GetCustomerResponse.Id = mundipaggCliente.MPG_CLI_CUS_ID;
                buscarClienteMundipaggResponseDTO.GetCustomerResponse.Document = mundipaggCliente.MPG_CLI_DOCUMENT;
                return buscarClienteMundipaggResponseDTO;
            }

            try
            {

                criarClienteRequestMundipaggDTO.CreateCustomerRequest.Name = clienteDto.CLI_NOME;
                criarClienteRequestMundipaggDTO.CreateCustomerRequest.Document =  clienteDto.CLI_CPF_CNPJ;
                if(clienteDto.CLI_CPF_CNPJ.Length > 11)
                {
                    criarClienteRequestMundipaggDTO.CreateCustomerRequest.Type = "company";
                }
                else
                {
                    criarClienteRequestMundipaggDTO.CreateCustomerRequest.Type = "individual";
                }

                var mundipaggApiCliente = new MundiAPIClient(MundipaggSTT.SecretKey, "");
                var getCustomerResponse = mundipaggApiCliente.Customers.CreateCustomer(criarClienteRequestMundipaggDTO.CreateCustomerRequest);


                var mundipaggClienteDTO = new MundipaggClienteDTO();
                mundipaggClienteDTO.MPG_CLI_CUS_ID = getCustomerResponse.Id;
                mundipaggClienteDTO.MPG_CLI_CODE = getCustomerResponse.Code;
                mundipaggClienteDTO.MPG_CLI_DOCUMENT = getCustomerResponse.Document;
                mundipaggClienteDTO.MPG_CLI_NAME = getCustomerResponse.Name;
                mundipaggClienteDTO.MPG_CLI_TYPE = getCustomerResponse.Type;
                mundipaggClienteDTO.CLI_ID = (int)clienteDto.CLI_ID;
                _mundipaggClienteSRV.Save(mundipaggClienteDTO);
                
                var buscarClienteMundipaggResponseDTO = new BuscarClienteMundipaggResponseDTO();
                buscarClienteMundipaggResponseDTO.GetCustomerResponse = getCustomerResponse;

                return buscarClienteMundipaggResponseDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        private ClienteDto ConverteClienteMundipaggParaCliente(ClienteMundipaggDTO clienteMundipaggDTO)
        {

            var clienteDTO = new ClienteDto();
            clienteDTO.CLI_CPF_CNPJ = clienteMundipaggDTO.Document;
            clienteDTO.CLI_NOME = clienteMundipaggDTO.Name;
            clienteDTO.DATA_CADASTRO = DateTime.Now;
            clienteDTO.CLI_EMAIL = clienteMundipaggDTO.Email;
            if (clienteDTO.CLI_CPF_CNPJ.Length < 12)
            {
                clienteDTO.CLI_TP_PESSOA = "F";
            }
            else
            {
                clienteDTO.CLI_TP_PESSOA = "J";
            }

            var assinaturaEmailDTO = new AssinaturaEmailDTO();
            assinaturaEmailDTO.AEM_EMAIL = clienteMundipaggDTO.Email;
            clienteDTO.ASSINATURA_EMAIL = new List<AssinaturaEmailDTO>() { assinaturaEmailDTO };

            var clienteEnderecoDTO = new ClienteEnderecoDto();
            clienteEnderecoDTO.TIPO_ENDERECO = new TipoEnderecoDTO() { TP_END_ID = 2 };
            clienteEnderecoDTO.END_LOGRADOURO = clienteMundipaggDTO.EnderecoMundipaggDTO.Street;
            clienteEnderecoDTO.END_COMPLEMENTO = clienteMundipaggDTO.EnderecoMundipaggDTO.LineTwo;
            clienteEnderecoDTO.END_NUMERO = clienteMundipaggDTO.EnderecoMundipaggDTO.Number;
            clienteEnderecoDTO.END_BAIRRO = clienteMundipaggDTO.EnderecoMundipaggDTO.Neighborhood;
            clienteEnderecoDTO.END_CEP = clienteMundipaggDTO.EnderecoMundipaggDTO.ZipCode;
            clienteEnderecoDTO.END_MUNICIPIO = clienteMundipaggDTO.EnderecoMundipaggDTO.City;



            return clienteDTO;
        }

    }
}

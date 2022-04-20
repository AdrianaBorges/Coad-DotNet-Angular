using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect;
using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.CORPORATIVO.Service.Custons
{
    public class ClienteIntegracaoSRV
    {
        public ClienteSRV _clienteSRV { get; set; }
        public AssinaturaSenhaSRV _assinaturaSenha { get; set; }
        public AssinaturaSRV _assinaturaSRV { get; set; }
        public MunicipioSRV _municipioSRV { get; set; }
        public TipoClienteSRV _tipoClienteSRV { get; set; }

        public ClienteIntegrDTO RetornarDadosDoCliente(string assinatura, string senha)
        {
            if (_assinaturaSenha.TestarSenhaDaAssinatura(assinatura, senha))
            {
                var codCliente = _assinaturaSRV.RetornarIdClienteDaAssinatura(assinatura);
                var clienteIntegr = CarregarDadosDoClienteParaIntegracao(codCliente);

                return clienteIntegr;
            }
            else
            {
                throw new ServicoException("Código de assinatura ou senha não estão corretos.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliId"></param>
        /// <returns></returns>
        public ClienteIntegrDTO CarregarDadosDoClienteParaIntegracao(int? cliId)
        {
            var cliente = _clienteSRV.FindByIdFullLoaded((int)cliId, true, true, true, true);
            ClienteIntegrDTO prosp = new ClienteIntegrDTO();

            prosp.ClienteId = cliente.CLI_ID;
            prosp.Nome = cliente.CLI_NOME;
            prosp.CNPJ_CPF = cliente.CLI_CPF_CNPJ;
            prosp.A_C = cliente.CLI_A_C;
            prosp.TipoCliente = _tipoClienteSRV.RetornarTipoClienteIntegracao(cliente.TIPO_CLI_ID);

            prosp.EmailPagamento = RetornarEmailPorTipo(cliente, 2);
            prosp.EmailConsulta = RetornarEmailPorTipo(cliente, 3);
            prosp.EmailNewsLetter = RetornarEmailPorTipo(cliente, 4);

            InserirTelefones(cliente, prosp);
            InserirEnderecos(cliente, prosp);

            return prosp;

        }

        public void InserirEnderecos(ClienteDto cliente, ClienteIntegrDTO cliIntegr)
        {

            if (cliIntegr != null &&
                cliente != null &&
                cliente.CLIENTES_ENDERECO != null)
            {
                var endFat = cliente.CLIENTES_ENDERECO.Where(x => x.END_TIPO == 2).FirstOrDefault();
                var endEnt = cliente.CLIENTES_ENDERECO.Where(x => x.END_TIPO == 1).FirstOrDefault();

                if (endFat != null)
                {
                    var prostEnd = new ClienteIntegrEnderecoDTO()
                    {
                        Bairro = endFat.END_BAIRRO,
                        CEP = endFat.END_CEP,
                        Complemento = endFat.END_COMPLEMENTO,
                        Logradouro = endFat.END_LOGRADOURO,
                        Municipio = _municipioSRV.RetornarMunicipioIntegracao(endFat.MUN_ID),
                        Numero = endFat.END_NUMERO,
                        UF = endFat.END_UF,
                        //MunId = endFat.MUN_ID,
                        //Munic = endFat.MUNICIPIO
                    };

                    cliIntegr.EnderecoFaturamento = prostEnd;
                }

                if (endEnt != null)
                {
                    var prostEndEntr = new ClienteIntegrEnderecoDTO()
                    {
                        Bairro = endEnt.END_BAIRRO,
                        CEP = endEnt.END_CEP,
                        Complemento = endEnt.END_COMPLEMENTO,
                        Logradouro = endEnt.END_LOGRADOURO,
                        Municipio = _municipioSRV.RetornarMunicipioIntegracao(endEnt.MUN_ID),
                        Numero = endEnt.END_NUMERO,
                        UF = endEnt.END_UF,
                        //MunId = endEnt.MUN_ID,
                        //Munic = endEnt.MUNICIPIO
                    };
                    cliIntegr.EnderecoEntrega = prostEndEntr;
                }

            }
        }

        public string RetornarEmailPorTipo(ClienteDto cliente, int? tipo)
        {
            if (cliente != null &&
                cliente.ASSINATURA_EMAIL != null &&
                tipo != null)
            {
                var email = cliente.ASSINATURA_EMAIL
                .Where(x => x.OPC_ID == tipo)
                .Select(x => x.AEM_EMAIL)
                .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(email) ||
                    cliente.ASSINATURA_EMAIL.All(x => x.OPC_ID == null))
                {
                    email = cliente
                        .ASSINATURA_EMAIL
                        .Select(sel => sel.AEM_EMAIL)
                        .FirstOrDefault();
                }

                return email;
            }
            return null;
        }

        public void InserirTelefones(ClienteDto cliente, ClienteIntegrDTO cliIntegr)
        {
            if (cliente != null &&
                cliente.ASSINATURA_TELEFONE != null)
            {
                var telefone = cliente.ASSINATURA_TELEFONE
                .Where(x => x.TIPO_TEL_ID == 4)
                .FirstOrDefault();

                var celular = cliente.ASSINATURA_TELEFONE
                .Where(x => x.TIPO_TEL_ID == 1)
                .FirstOrDefault();

                var fax = cliente.ASSINATURA_TELEFONE
                .Where(x => x.TIPO_TEL_ID == 2)
                .FirstOrDefault();

                // --------------------- telefone ----------------

                if (telefone != null)
                {
                    var clienteProspectTel = new ClienteIntegrTelefoneDTO();
                    clienteProspectTel.CodigoTelefone = telefone.ATE_ID;
                    clienteProspectTel.Contato = telefone.ATE_CONTATO;
                    clienteProspectTel.DDD = telefone.ATE_DDD;
                    clienteProspectTel.Ramal = telefone.ATE_RAMAL;
                    clienteProspectTel.Telefone = telefone.ATE_TELEFONE;

                    cliIntegr.Telefone = clienteProspectTel;

                }

                // --------------------- celular ----------------
                if (celular != null)
                {
                    var clienteProspectCel = new ClienteIntegrTelefoneDTO();
                    clienteProspectCel.CodigoTelefone = celular.ATE_ID;
                    clienteProspectCel.Contato = celular.ATE_CONTATO;
                    clienteProspectCel.DDD = celular.ATE_DDD;
                    clienteProspectCel.Ramal = celular.ATE_RAMAL;
                    clienteProspectCel.Telefone = celular.ATE_TELEFONE;

                    cliIntegr.Celular = clienteProspectCel;

                }


                // --------------------- fax ----------------


                if (fax != null)
                {
                    var clienteProspectFax = new ClienteIntegrTelefoneDTO();
                    clienteProspectFax.CodigoTelefone = fax.ATE_ID;
                    clienteProspectFax.Contato = fax.ATE_CONTATO;
                    clienteProspectFax.DDD = fax.ATE_DDD;
                    clienteProspectFax.Ramal = fax.ATE_RAMAL;
                    clienteProspectFax.Telefone = fax.ATE_TELEFONE;

                    cliIntegr.Fax = clienteProspectFax;

                }

            }
        }

        public ClienteDto ConverterClienteProspectParaCliente(ClienteIntegrDTO clienteIntegr)
        {
            ClienteDto cliente = null;

            if (clienteIntegr != null)
            {
                if (clienteIntegr.ClienteId != null)
                    cliente = _clienteSRV.FindByIdFullLoaded((int)clienteIntegr.ClienteId, true, true, true, true);
                else
                {
                    cliente = new ClienteDto();
                }

                cliente.CLI_NOME = clienteIntegr.Nome;
                cliente.CLI_CPF_CNPJ = clienteIntegr.CNPJ_CPF;
                cliente.CLI_A_C = clienteIntegr.A_C;

                if (clienteIntegr.TipoCliente != null)
                {
                    cliente.TIPO_CLI_ID = clienteIntegr.TipoCliente.CodigoTipoCliente;                
                }
                
                AtualizarEndereco(cliente, clienteIntegr);

                AtualizarEmail(cliente, 2, clienteIntegr.EmailPagamento);
                AtualizarEmail(cliente, 3, clienteIntegr.EmailConsulta);
                AtualizarEmail(cliente, 4, clienteIntegr.EmailNewsLetter);

                AtualizarTelefones(cliente, clienteIntegr);
            }

            return cliente;
        }

        public void AtualizarEmail(ClienteDto cliente, int tipo, string email)
        {
            if (cliente != null)
            {
                if (cliente.ASSINATURA_EMAIL == null)
                    cliente.ASSINATURA_EMAIL = new HashSet<AssinaturaEmailDTO>();

                var lstEmail = cliente.ASSINATURA_EMAIL;
                AssinaturaEmailDTO objEmail = null;

                objEmail = lstEmail.Where(x => x.OPC_ID == tipo).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(email))
                {
                    if (objEmail != null)
                        lstEmail.Remove(objEmail);
                }
                else
                {
                    if (objEmail == null)
                    {
                        objEmail = new AssinaturaEmailDTO();
                        lstEmail.Add(objEmail);
                    }

                    objEmail.OPC_ID = tipo;
                    objEmail.AEM_EMAIL = email;
                }
            }
        }

        public void AtualizarTelefones(ClienteDto cliente, ClienteIntegrDTO clienteIntegr)
        {
            if (cliente != null && clienteIntegr != null)
            {
                if (cliente.ASSINATURA_TELEFONE == null)
                    cliente.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();

                var lstTelefone = cliente.ASSINATURA_TELEFONE;

                // ---------------------------- Telefone ---------------------------------------
                var tel = clienteIntegr.Telefone;

                if (tel != null)
                {
                    var tipoTelefone = 4;
                    AssinaturaTelefoneDTO assiTelefone = null;

                    if (tel.CodigoTelefone != null)
                    {
                        assiTelefone = lstTelefone.Where(x => x.ATE_ID == tel.CodigoTelefone).FirstOrDefault();
                    }
                    else
                    {
                        assiTelefone = lstTelefone.Where(x => x.TIPO_TEL_ID == tipoTelefone).FirstOrDefault();
                    }

                    if (!tel.IsEmpty)
                    {
                        if (assiTelefone == null)
                        {
                            assiTelefone = new AssinaturaTelefoneDTO();
                            lstTelefone.Add(assiTelefone);
                        }

                        assiTelefone.TIPO_TEL_ID = tipoTelefone;
                        assiTelefone.ATE_DDD = tel.DDD;
                        assiTelefone.ATE_TELEFONE = tel.Telefone;
                        assiTelefone.ATE_RAMAL = tel.Ramal;
                        assiTelefone.ATE_CONTATO = tel.Contato;
                    }
                    else
                    {
                        if (assiTelefone != null)
                        {
                            lstTelefone.Remove(assiTelefone);
                        }
                    }
                }

                // ---------------------------- Celular ---------------------------------------
                var cel = clienteIntegr.Celular;

                if (cel != null)
                {
                    var tipoTelefone = 1;
                    AssinaturaTelefoneDTO assiTelefone = null;

                    if (cel.CodigoTelefone != null)
                    {
                        assiTelefone = lstTelefone.Where(x => x.ATE_ID == cel.CodigoTelefone).FirstOrDefault();
                    }
                    else
                    {
                        assiTelefone = lstTelefone.Where(x => x.TIPO_TEL_ID == tipoTelefone).FirstOrDefault();
                    }

                    if (!cel.IsEmpty)
                    {
                        if (assiTelefone == null)
                        {
                            assiTelefone = new AssinaturaTelefoneDTO();
                            lstTelefone.Add(assiTelefone);
                        }

                        assiTelefone.TIPO_TEL_ID = tipoTelefone;
                        assiTelefone.ATE_DDD = cel.DDD;
                        assiTelefone.ATE_TELEFONE = cel.Telefone;
                        assiTelefone.ATE_RAMAL = cel.Ramal;
                        assiTelefone.ATE_CONTATO = cel.Contato;
                    }
                    else
                    {
                        if (assiTelefone != null)
                        {
                            lstTelefone.Remove(assiTelefone);
                        }
                    }
                }

                // ---------------------------- Telefone ---------------------------------------
                var fax = clienteIntegr.Fax;

                if (fax != null)
                {
                    var tipoTelefone = 2;
                    AssinaturaTelefoneDTO assiTelefone = null;

                    if (fax.CodigoTelefone != null)
                    {
                        assiTelefone = lstTelefone.Where(x => x.ATE_ID == fax.CodigoTelefone).FirstOrDefault();
                    }
                    else
                    {
                        assiTelefone = lstTelefone.Where(x => x.TIPO_TEL_ID == tipoTelefone).FirstOrDefault();
                    }

                    if (!fax.IsEmpty)
                    {

                        if (assiTelefone == null)
                        {
                            assiTelefone = new AssinaturaTelefoneDTO();
                            lstTelefone.Add(assiTelefone);
                        }

                        assiTelefone.TIPO_TEL_ID = tipoTelefone;
                        assiTelefone.ATE_DDD = fax.DDD;
                        assiTelefone.ATE_TELEFONE = fax.Telefone;
                        assiTelefone.ATE_RAMAL = fax.Ramal;
                        assiTelefone.ATE_CONTATO = fax.Contato;
                    }
                    else
                    {
                        if (assiTelefone != null)
                        {
                            lstTelefone.Remove(assiTelefone);
                        }
                    }
                }

            }
        }

        public void AtualizarEndereco(ClienteDto cliente, ClienteIntegrDTO clientIntegr)
        {
            if (cliente != null && clientIntegr != null)
            {
                if (cliente.CLIENTES_ENDERECO == null)
                    cliente.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();

                var lstEnd = cliente.CLIENTES_ENDERECO;
                var endFat = clientIntegr.EnderecoFaturamento;

                if (endFat != null)
                {
                    ClienteEnderecoDto clienteEnd = null;

                    clienteEnd = lstEnd.Where(x => x.END_TIPO == 2).FirstOrDefault();

                    if (clienteEnd == null)
                    {
                        clienteEnd = new ClienteEnderecoDto();
                        lstEnd.Add(clienteEnd);
                    }

                    clienteEnd.END_TIPO = 2;
                    clienteEnd.END_CEP = endFat.CEP;
                    clienteEnd.END_UF = endFat.UF;
                    clienteEnd.END_BAIRRO = endFat.Bairro;
                    clienteEnd.END_LOGRADOURO = endFat.Logradouro;
                    clienteEnd.END_NUMERO = endFat.Numero;
                    clienteEnd.END_COMPLEMENTO = endFat.Complemento;

                    if (endFat.Municipio != null)
                    {
                        clienteEnd.END_MUNICIPIO = endFat.Municipio.DescricaoMunicipio;
                        clienteEnd.MUN_ID = endFat.Municipio.CodigoMunicipio;
                    }
                }

                var endEntr = clientIntegr.EnderecoEntrega;

                if (endEntr != null)
                {
                    ClienteEnderecoDto clienteEnd = null;

                    clienteEnd = lstEnd.Where(x => x.END_TIPO == 1).FirstOrDefault();

                    if (clienteEnd == null)
                    {
                        clienteEnd = new ClienteEnderecoDto();
                        lstEnd.Add(clienteEnd);
                    }

                    clienteEnd.END_TIPO = 1;
                    clienteEnd.END_CEP = endEntr.CEP;
                    clienteEnd.END_UF = endEntr.UF;
                    clienteEnd.END_BAIRRO = endEntr.Bairro;
                    clienteEnd.END_LOGRADOURO = endEntr.Logradouro;
                    clienteEnd.END_NUMERO = endEntr.Numero;
                    clienteEnd.END_COMPLEMENTO = endEntr.Complemento;

                    if (endEntr.Municipio != null)
                    {
                        clienteEnd.END_MUNICIPIO = endEntr.Municipio.DescricaoMunicipio;
                        clienteEnd.MUN_ID = endEntr.Municipio.CodigoMunicipio;
                    }
                }
            }
        }

        public ClienteDto SalvarClienteIntegracao(
           ClienteIntegrDTO clienteProspect)
        {
            ClienteDto clienteResposta = null;
            ClienteDto cliente = ConverterClienteProspectParaCliente(clienteProspect);

            if (clienteProspect != null)
            {
                using (var scope = new TransactionScope())
                {
                    var result = _clienteSRV.SalvarClienteAgenda(cliente, null, null, null, false, true);
                    if(result.Cliente != null)
                    {
                        clienteResposta = result.Cliente;
                    }
                    scope.Complete();
                }
            }
            return clienteResposta;
        }

    }
}

using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.AcaoSalvamento;
using COAD.CORPORATIVO.Model.Dto.Custons.Buscas;
using COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect;
using COAD.PROSPECTADOS.Model.Dto;
using COAD.PROSPECTADOS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.CORPORATIVO.Service.Custons
{
    public class ClienteProspectSRV
    {

        public ClienteSRV _clienteSRV { get; set; } //= new ClienteSRV();
        public ProspectSRV _prospectSRV { get; set; } //= new ClienteSRV();= new ProspectSRV();
        public CartCoadSRV _cartCoadSRV { get; set; } //= new ClienteSRV();= new CartCoadSRV();
        public ClienteEnderecoSRV _clienteEndereco { get; set; } //= new ClienteSRV();= new ClienteEnderecoSRV();
        public CarteiramentoSRV _carteiramentoSRV { get; set; } //= new ClienteSRV();= new CarteiramentoSRV();
        public RepresentanteSRV _representanteSRV { get; set; } //= new ClienteSRV();= new RepresentanteSRV();
        public MunicipioSRV _municipioSRV { get; set; } //= new ClienteSRV();= new MunicipioSRV();

        public ClienteProspectSRV()
        {
            _clienteSRV = new ClienteSRV();
            _prospectSRV = new ProspectSRV();
            _cartCoadSRV = new CartCoadSRV();
            _clienteEndereco = new ClienteEnderecoSRV();
            _carteiramentoSRV = new CarteiramentoSRV();
            _representanteSRV = new RepresentanteSRV();
            _municipioSRV = new MunicipioSRV();
        }

        public ClienteProspectSRV(ClienteSRV _clienteSRV, ProspectSRV _prospectSRV)
        {
            this._clienteSRV = _clienteSRV;
            this._prospectSRV = _prospectSRV;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliId"></param>
        /// <returns></returns>
        public ClienteProspectDTO CarregarDadosDoProspectCorp(int? cliId)
        {
            var buscaDTO = new BuscarClientePorIdDTO()
            {
                trazAssinaturaEmail = true,
                trazCarteiraCliente = true,
                trazClienteTelefone = true,
                validaEnderecoBasica = true
            };

            var cliente = _clienteSRV.FindByIdFullLoaded((int) cliId, buscaDTO);
            ClienteProspectDTO prosp = new ClienteProspectDTO();

            prosp.ClienteId = cliente.CLI_ID;
            prosp.OrigemCliente = true;
            prosp.Nome = cliente.CLI_NOME;
            prosp.CNPJ_CPF = cliente.CLI_CPF_CNPJ;
            prosp.A_C = cliente.CLI_A_C;

            prosp.InscricaoEstadual = cliente.CLI_INSCRICAO;

            if (cliente.CLI_ISENTO_INSCRICAO.Value)
            {
                prosp.EhIsentoDeInscricaoEstadual = true;

            }
            else prosp.EhIsentoDeInscricaoEstadual = false;

            if (cliente.TIPO_CLI_ID.Equals(2))
                prosp.EhIsentoDeInscricaoEstadual = true;


            prosp.TipoClienteId = cliente.TIPO_CLI_ID;
            prosp.CAR_ID = cliente.CLI_CAR_ID_PROSPECT;
            prosp.carIdStr = cliente.CLI_CAR_ID_PROSPECT;

            PreencherEmailNoClienteProspect(prosp, cliente);

            PreencherTelefoneNoClienteProspect(prosp, cliente);
            PreencherEnderecoNoClienteProspect(prosp, cliente);
            PreencherCarteiraClienteNoClienteProspect(prosp, cliente);

            return prosp;

        }

        /// <summary>
        /// Recupera aos dados de prospect do cooporativo
        /// </summary>
        /// <param name="cliId"></param>
        /// <returns></returns>
        public ClienteProspectDTO CarregarDadosDoProspectOriginal(string codigo)
        {
            var prospect = _cartCoadSRV.FindByIdFullLoaded(codigo, true, true);
            ClienteProspectDTO clienteProspect = new ClienteProspectDTO();

            clienteProspect.Codigo = codigo;
            clienteProspect.Nome = prospect.NOME;
            clienteProspect.CNPJ_CPF = prospect.prospects.CPF_CNPJ;
            clienteProspect.A_C = prospect.A_C;
            clienteProspect.TipoClienteId = 2;
            clienteProspect.InscricaoEstadual = prospect.prospects.INSCRICAO;

            if (prospect.prospects != null)
            {
                var objProspects = prospect.prospects;

                var carteiraCliProsp = new CarteiraClienteProspectDTO();
                carteiraCliProsp.CarId = (objProspects.REGIAO + objProspects.AREA + objProspects.CART);
                clienteProspect.CarteirasCliente.Add(carteiraCliProsp);
            }

            PreencherEmailNoProspect(clienteProspect, prospect);
            PreencherTelefoneNoProspect(clienteProspect, prospect);
            PreencherEnderecoNoProspect(clienteProspect, prospect);

            return clienteProspect;

        }

        public ClienteDto ConverterClienteProspectParaCliente(ClienteProspectDTO clienteProspect)
        {
            ClienteDto cliente = null;

            if (clienteProspect != null)
            {
                if (clienteProspect.ClienteId != null)
                {
                    var buscaDTO = new BuscarClientePorIdDTO()
                    {
                        trazAssinaturaEmail = true,
                        trazCarteiraCliente = true,
                        trazClienteTelefone = true,
                        validaEnderecoBasica = true
                    };

                    cliente = _clienteSRV.FindByIdFullLoaded((int)clienteProspect.ClienteId, buscaDTO);                    
                }
                else
                {
                    cliente = new ClienteDto();
                }

                if (!string.IsNullOrWhiteSpace(clienteProspect.Codigo))
                {
                    cliente.CodigoProspect = clienteProspect.Codigo;
                }

                cliente.CLI_NOME = clienteProspect.Nome;
                cliente.CLI_CPF_CNPJ = clienteProspect.CNPJ_CPF;
                cliente.CLI_A_C = clienteProspect.A_C;
                cliente.TIPO_CLI_ID = clienteProspect.TipoClienteId;
                cliente.CLI_CAR_ID_PROSPECT = clienteProspect.CAR_ID;

                cliente.CLI_INSCRICAO = clienteProspect.InscricaoEstadual;

                cliente.CLI_ISENTO_INSCRICAO = clienteProspect.EhIsentoDeInscricaoEstadual;

                AtualizarEndereco(cliente, clienteProspect);

                AtualizarEmailNoCliente(cliente, clienteProspect);
                AtualizarTelefones(cliente, clienteProspect);
                AtualizarCarteiramentos(cliente, clienteProspect);

            }

            return cliente;
        }

        public void AtualizarEmailNoCliente(ClienteDto cliente, ClienteProspectDTO clienteProspect)
        {
            if (cliente != null)
            {
                if (cliente.ASSINATURA_EMAIL == null)
                    cliente.ASSINATURA_EMAIL = new HashSet<AssinaturaEmailDTO>();

                var lstEmail = cliente.ASSINATURA_EMAIL;
                var lstEmailsProsp = clienteProspect.Emails;
                

                // Apagando a referencia e criando uma nova garanto que os emails excluídos não permanecerão na lista.
                cliente.ASSINATURA_EMAIL = new HashSet<AssinaturaEmailDTO>();

                // Emails Novos
                var lstEmailsNovos = lstEmailsProsp
                    .Where(x => x.CodEmail == null)
                    .Select(x => new AssinaturaEmailDTO() {
                        AEM_EMAIL = x.Email,
                        OPC_ID = x.TipoAtendimento,
                        DATA_ALTERA = DateTime.Now,                        
                    });

                cliente.ASSINATURA_EMAIL = cliente.ASSINATURA_EMAIL.Concat(lstEmailsNovos).ToList();
                
                foreach(var email in lstEmailsProsp) // Atualizar os que já existem
                {
                    var emailAtualizar = lstEmail.Where(x => x.AEM_ID == email.CodEmail).FirstOrDefault();
                    if(emailAtualizar != null)
                    {
                        emailAtualizar.AEM_EMAIL = email.Email;
                        emailAtualizar.OPC_ID = email.TipoAtendimento;
                        emailAtualizar.DATA_ALTERA = DateTime.Now;
                        cliente.ASSINATURA_EMAIL.Add(emailAtualizar);
                    }
                }                
            }
        }

        public void AtualizarTelefones(ClienteDto cliente, ClienteProspectDTO clienteProspect)
        {
            if (cliente != null && clienteProspect != null)
            {
                if (cliente.ASSINATURA_TELEFONE == null)
                    cliente.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();

                var lstTelefone = cliente.ASSINATURA_TELEFONE;
                var lstTelefoneProsp = clienteProspect.Telefones;

                cliente.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();

                var lstTelefonesNovos = lstTelefoneProsp
                    .Where(x => x.CodigoTelefone == null)
                    .Select(x => new AssinaturaTelefoneDTO() {
                        TIPO_TEL_ID = x.TipoTelefone,
                        ATE_DDD = x.DDD,
                        ATE_TELEFONE = x.Telefone,
                        ATE_RAMAL = x.Ramal,
                        ATE_CONTATO = x.Contato,
                        OPC_ID = x.TipoAtendimento,
                        DATA_ALTERA = DateTime.Now
                    });

                cliente.ASSINATURA_TELEFONE = cliente
                    .ASSINATURA_TELEFONE
                    .Concat(lstTelefonesNovos)
                    .ToList();

                foreach(var tel in lstTelefoneProsp)
                {
                    var telefoneAtualizar = lstTelefone
                        .Where(x => x.ATE_ID == tel.CodigoTelefone)
                        .FirstOrDefault();

                    if(telefoneAtualizar != null)
                    {
                        telefoneAtualizar.TIPO_TEL_ID = tel.TipoTelefone;
                        telefoneAtualizar.ATE_DDD = tel.DDD;
                        telefoneAtualizar.ATE_TELEFONE = tel.Telefone;
                        telefoneAtualizar.ATE_RAMAL = tel.Ramal;
                        telefoneAtualizar.ATE_CONTATO = tel.Contato;
                        telefoneAtualizar.OPC_ID = tel.TipoAtendimento;
                        telefoneAtualizar.DATA_ALTERA = DateTime.Now;

                        cliente.ASSINATURA_TELEFONE.Add(telefoneAtualizar);
                    }
                }
            }
        }

        public void AtualizarCarteiramentos(ClienteDto cliente, ClienteProspectDTO clienteProspect)
        {
            if (cliente != null && clienteProspect != null)
            {
                if (cliente.CARTEIRA_CLIENTE == null)
                    cliente.CARTEIRA_CLIENTE = new HashSet<CarteiraClienteDTO>();

                var lstCarteiraCliente = cliente.CARTEIRA_CLIENTE;
                var lstCarteiraProsp = clienteProspect.CarteirasCliente;
                cliente.CARTEIRA_CLIENTE = new HashSet<CarteiraClienteDTO>();
                
                foreach (var carProsp in lstCarteiraProsp)
                {
                    var carteiraCliente = lstCarteiraCliente
                        .Where(x => x.CAR_ID == carProsp.CarId && x.CLI_ID == carProsp.CliId)
                        .FirstOrDefault();

                    if (carteiraCliente != null)
                    {
                        carteiraCliente.Deletar = carProsp.Deletar;
                        cliente.CARTEIRA_CLIENTE.Add(carteiraCliente);
                    }
                    else
                    {
                        cliente.CARTEIRA_CLIENTE.Add(new CarteiraClienteDTO() {

                            CLI_ID = carProsp.CliId,
                            CAR_ID = carProsp.CarId,
                            Deletar = carProsp.Deletar,
                            DATA_ASSOCIACAO = DateTime.Now
                        });
                    }
                }
            }
        }

        public void AtualizarEndereco(ClienteDto cliente, ClienteProspectDTO prospect)
        {
            if (cliente != null && prospect != null)
            {
                if (cliente.CLIENTES_ENDERECO == null)
                    cliente.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();

                var lstEnd = cliente.CLIENTES_ENDERECO;
                cliente.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();

                var endFat = prospect.EnderecoFaturamento;

                if (endFat != null)
                {
                //    if (endFat.IsEmpty())
                //    {
                //        endFat.MunId = null;
                //        endFat.Municipio = null;
                //    }

                    if (endFat.TipoEnd == null)
                        endFat.TipoEnd = 2;

                    var tipoEnd = endFat.TipoEnd;
                    ClienteEnderecoDto clienteEnd = null;

                    clienteEnd = lstEnd.Where(x => x.END_TIPO == tipoEnd).FirstOrDefault();

                    if (clienteEnd == null)
                    {
                        clienteEnd = new ClienteEnderecoDto();
                        lstEnd.Add(clienteEnd);
                    }

                    clienteEnd.END_TIPO = endFat.TipoEnd;
                    clienteEnd.END_CEP = endFat.CEP;
                    clienteEnd.END_MUNICIPIO = endFat.Municipio;
                    clienteEnd.END_UF = endFat.UF;
                    clienteEnd.END_BAIRRO = endFat.Bairro;
                    clienteEnd.END_LOGRADOURO = endFat.Logradouro;
                    clienteEnd.END_NUMERO = endFat.Numero;
                    clienteEnd.END_COMPLEMENTO = endFat.Complemento;
                    clienteEnd.TIPO_LOGRADOURO = endFat.TipoLogradouro;
                    clienteEnd.Excluir = endFat.Excluir;
                    

                    if (endFat.MunId != null)
                    {
                        clienteEnd.MUN_ID = endFat.MunId;
                    }
                    else if(!string.IsNullOrWhiteSpace(endFat.Municipio) 
                        && !string.IsNullOrWhiteSpace(endFat.UF))
                    {
                        clienteEnd.MUN_ID = _municipioSRV.RetornarMunIdPorDescricao(endFat.Municipio, endFat.UF);
                    }

                    cliente.CLIENTES_ENDERECO.Add(clienteEnd);
                }

                var endEntr = prospect.EnderecoEntrega;

                if (endEntr != null)
                {
                    //if (endEntr.IsEmpty())
                    //{
                    //    endFat.MunId = null;
                    //    endFat.Municipio = null;
                    //}
                    if (endEntr.TipoEnd == null)
                        endEntr.TipoEnd = 1;

                    var tipoEnd = endEntr.TipoEnd;
                    ClienteEnderecoDto clienteEnd = null;

                    clienteEnd = lstEnd.Where(x => x.END_TIPO == tipoEnd).FirstOrDefault();

                    if (clienteEnd == null)
                    {
                        clienteEnd = new ClienteEnderecoDto();
                    }

                    clienteEnd.END_TIPO = endEntr.TipoEnd;
                    clienteEnd.END_CEP = endEntr.CEP;
                    clienteEnd.END_MUNICIPIO = endEntr.Municipio;
                    clienteEnd.END_UF = endEntr.UF;
                    clienteEnd.END_BAIRRO = endEntr.Bairro;
                    clienteEnd.END_LOGRADOURO = endEntr.Logradouro;
                    clienteEnd.END_NUMERO = endEntr.Numero;
                    clienteEnd.END_COMPLEMENTO = endEntr.Complemento;
                    clienteEnd.TIPO_LOGRADOURO = endEntr.TipoLogradouro;
                    clienteEnd.Excluir = endEntr.Excluir;

                    if (endEntr.MunId != null)
                    {
                        clienteEnd.MUN_ID = endEntr.MunId;
                    }
                    else if (!string.IsNullOrWhiteSpace(endEntr.Municipio)
                        && !string.IsNullOrWhiteSpace(endEntr.UF))
                    {
                        clienteEnd.MUN_ID = _municipioSRV.RetornarMunIdPorDescricao(endEntr.Municipio, endEntr.UF);
                    }

                    cliente.CLIENTES_ENDERECO.Add(clienteEnd);
                }
            }
        }

        public string PreencherEmailNoClienteProspect(ClienteProspectDTO clienteProspect, ClienteDto cliente)
        {
            if(clienteProspect != null &&
                cliente != null && 
                cliente.ASSINATURA_EMAIL != null)
            {
                var emails = cliente.ASSINATURA_EMAIL;
                
                foreach(var email in emails)
                {
                    ClienteProspectEmailDTO clienteProspectEmail = new ClienteProspectEmailDTO()
                    {
                        CodEmail = email.AEM_ID,
                        Email = email.AEM_EMAIL,
                        TipoAtendimento = email.OPC_ID
                    };

                    clienteProspect.Emails.Add(clienteProspectEmail);
                    
                }                
            }
            return null;
        }

        public void PreencherTelefoneNoClienteProspect(ClienteProspectDTO prosp, ClienteDto cliente)
        {
            if (prosp != null &&
                cliente != null &&
                cliente.ASSINATURA_TELEFONE != null)
            {
                var lstTelefones = cliente.ASSINATURA_TELEFONE;

                foreach(var tel in lstTelefones)
                {
                    ClienteProspectTelefoneDTO clienteProspectTel = new ClienteProspectTelefoneDTO();
                    clienteProspectTel.CodigoTelefone = tel.ATE_ID;
                    clienteProspectTel.Contato = tel.ATE_CONTATO;
                    clienteProspectTel.DDD = tel.ATE_DDD;
                    clienteProspectTel.Ramal = tel.ATE_RAMAL;
                    clienteProspectTel.Telefone = tel.ATE_TELEFONE;
                    clienteProspectTel.TipoTelefone = tel.TIPO_TEL_ID;
                    clienteProspectTel.TipoAtendimento = tel.OPC_ID;

                    prosp.Telefones.Add(clienteProspectTel);
                }                
            }
        }

        public void PreencherCarteiraClienteNoClienteProspect(ClienteProspectDTO prosp, ClienteDto cliente)
        {
            if (prosp != null &&
                cliente != null &&
                cliente.CARTEIRA_CLIENTE != null)
            {
                var lstCartCli = cliente.CARTEIRA_CLIENTE;

                foreach (var carCli in lstCartCli)
                {
                    CarteiraClienteProspectDTO carCliPros = new CarteiraClienteProspectDTO();
                    carCliPros.CarId = carCli.CAR_ID;
                    carCliPros.CliId = carCli.CLI_ID;
                   
                    prosp.CarteirasCliente.Add(carCliPros);
                }
            }
        }
        public void PreencherEnderecoNoClienteProspect(ClienteProspectDTO prosp, ClienteDto cliente)
        {

            if (prosp != null && 
                cliente != null &&
                cliente.CLIENTES_ENDERECO != null)
            {
                var endFat = cliente.CLIENTES_ENDERECO.Where(x => x.END_TIPO == 2).FirstOrDefault();
                var endEnt = cliente.CLIENTES_ENDERECO.Where(x => x.END_TIPO == 1).FirstOrDefault();

                if (endFat != null)
                {
                    var prostEnd = new ClienteProspectEnderecoDTO()
                    {
                        Bairro = endFat.END_BAIRRO,
                        CEP = endFat.END_CEP,
                        Complemento = endFat.END_COMPLEMENTO,
                        Logradouro = endFat.END_LOGRADOURO,
                        Municipio = endFat.END_MUNICIPIO,
                        Numero = endFat.END_NUMERO,
                        TipoEnd = endFat.END_TIPO,
                        UF = endFat.END_UF,
                        MunId = endFat.MUN_ID,
                        Munic = endFat.MUNICIPIO,
                        TipoLogradouro = endFat.TIPO_LOGRADOURO
                    };

                    prosp.EnderecoFaturamento = prostEnd;
                }

                if (endEnt != null)
                {
                    var prostEndEntr = new ClienteProspectEnderecoDTO()
                    {
                        Bairro = endEnt.END_BAIRRO,
                        CEP = endEnt.END_CEP,
                        Complemento = endEnt.END_COMPLEMENTO,
                        Logradouro = endEnt.END_LOGRADOURO,
                        Municipio = endEnt.END_MUNICIPIO,
                        Numero = endEnt.END_NUMERO,
                        TipoEnd = endEnt.END_TIPO,
                        UF = endEnt.END_UF,
                        MunId = endEnt.MUN_ID,
                        Munic = endEnt.MUNICIPIO,
                        TipoLogradouro = endEnt.TIPO_LOGRADOURO
                    };
                    prosp.EnderecoEntrega = prostEndEntr;
                }

            }
        }

        /// <summary>
        /// Busca os prospects do banco do COADCORP
        /// </summary>
        /// <param name="cpf_cnpj"></param>
        /// <param name="nome"></param>
        /// <param name="uen_id"></param>
        /// <param name="CAR_ID"></param>
        /// <param name="RG_ID"></param>
        /// <param name="email"></param>
        /// <param name="REP_ID"></param>
        /// <param name="dddTelefone"></param>
        /// <param name="telefone"></param>
        /// <param name="AREA_ID"></param>
        /// <param name="CMP_ID"></param>
        /// <param name="CLI_ID"></param>
        /// <param name="pagina"></param>
        /// <param name="registroPorPagina"></param>
        /// <param name="excluidosDaValidacao"></param>
        /// <returns></returns>
        public Pagina<BuscarClienteDTO> BuscarProspectsCorp(
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
            var listProspectCoadCorp =
                _clienteSRV.BuscarProspects(cpf_cnpj,
                nome,
                email,
                dddTelefone,
                telefone,
                pesquisaCpfCnpjPorIqualdade,
                pagina,
                registroPorPagina,
                codigoCliente);

            return listProspectCoadCorp;
        }


        /// <summary>
        /// Buscar prospects no banco de prospects
        /// </summary>
        /// <param name="cpf_cnpj"></param>
        /// <param name="nome"></param>
        /// <param name="uen_id"></param>
        /// <param name="CAR_ID"></param>
        /// <param name="RG_ID"></param>
        /// <param name="email"></param>
        /// <param name="REP_ID"></param>
        /// <param name="dddTelefone"></param>
        /// <param name="telefone"></param>
        /// <param name="AREA_ID"></param>
        /// <param name="CMP_ID"></param>
        /// <param name="CLI_ID"></param>
        /// <param name="pagina"></param>
        /// <param name="registroPorPagina"></param>
        /// <param name="excluidosDaValidacao"></param>
        /// <returns></returns>
        public Pagina<BuscarClienteDTO> BuscarProspects(
            string cpf_cnpj = null,
            string nome = null,
            string email = null,
            string dddTelefone = null,
            string telefone = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            int pagina = 1,
            int registroPorPagina = 7)
        {

            if(
                !string.IsNullOrWhiteSpace(cpf_cnpj) ||
                !string.IsNullOrWhiteSpace(nome) ||
                !string.IsNullOrWhiteSpace(email) ||
                !string.IsNullOrWhiteSpace(telefone)
                )
            {
            var listProspectCoadCorp =
                _clienteSRV.BuscarProspects(cpf_cnpj,
                nome,
                email,
                dddTelefone,
                telefone,
                pesquisaCpfCnpjPorIqualdade,
                pagina,
                registroPorPagina);

            IList<string> lstIds = null;

            if(listProspectCoadCorp.numeroRegistros > 0){

                lstIds = listProspectCoadCorp.lista.Select(x => x.CLI_ID.ToString()).ToList();
            }

            var listProspect = _cartCoadSRV.BuscarProspects(
                nome, 
                cpf_cnpj, 
                email, 
                dddTelefone, 
                telefone, 
                lstIds, 
                pesquisaCpfCnpjPorIqualdade, 
                pagina, registroPorPagina);
            
            var listProspectBusca = listProspect.lista.Select(sel => new BuscarClienteDTO() { 
                Codigo = sel.CODIGO,
                CLI_NOME = sel.NOME,

                ASSINATURA_EMAIL = sel.EMAILS_PROSP.Select(x => new AssinaturaEmailDTO(){
                    
                    AEM_EMAIL = x.E_MAIL                    
                }).ToList(),

                ASSINATURA_TELEFONE = sel.TELEFONES_PROSP.Select(x => new AssinaturaTelefoneDTO(){
                
                    ATE_DDD = x.DDD_TEL,
                    ATE_TELEFONE = x.TELEFONE
                }).ToList()
            
            });


            var paginaProspect = new Pagina<BuscarClienteDTO>()
            {
                itensPorPagina = listProspect.itensPorPagina,
                lista = listProspectBusca,
                numeroPaginas = listProspect.numeroPaginas,
                numeroRegistros = listProspect.numeroRegistros,
                pagina = listProspect.pagina
            };


                return paginaProspect;
            }
            return new Pagina<BuscarClienteDTO>();
        }

        public void PreencherEmailNoProspect(ClienteProspectDTO clienteProspect, CartCoadDTO prospect)
        {
            if (clienteProspect != null && prospect != null && prospect.EMAILS_PROSP != null)
            {
                var lstEmail = prospect.EMAILS_PROSP.ToList();
                foreach (var email in lstEmail)
                {
                    ClienteProspectEmailDTO emailPro = new ClienteProspectEmailDTO();
                    emailPro.Email = email.E_MAIL;

                    int? tipoAtendimento = null;
                    switch (email.TIPO)
                    {
                        case "P":
                            tipoAtendimento = 2;
                            break;
                        case "C":
                            tipoAtendimento = 3;
                            break;
                        case "N":
                            tipoAtendimento = 4;
                            break;
                        case "R":
                            tipoAtendimento = 5;
                            break;
                    }
                    emailPro.TipoAtendimento = tipoAtendimento;
                    clienteProspect.Emails.Add(emailPro);
                }     

            }
        }

        public void PreencherTelefoneNoProspect(ClienteProspectDTO clienteProspect, CartCoadDTO prospect)
        {
            if (clienteProspect != null && prospect != null && prospect.TELEFONES_PROSP != null)
            {
                var lstTelefones = prospect.TELEFONES_PROSP;
                foreach(var tel in lstTelefones)
                {
                    int? tipoTelefone = 0;
                    switch (tel.TIPO)
                    {
                        case "TELEFONE":
                            tipoTelefone = 4;
                            break;
                        case "CELULAR":
                            tipoTelefone = 1;
                            break;
                        case "FAX":
                            tipoTelefone = 2;
                            break;
                    }

                    var telefoneProspect = new ClienteProspectTelefoneDTO()
                    {
                        DDD = tel.DDD_TEL,
                        Telefone = tel.TELEFONE,
                        TipoTelefone = tipoTelefone
                    };
                    clienteProspect.Telefones.Add(telefoneProspect);
                }
            }
        }

        public void PreencherEnderecoNoProspect(ClienteProspectDTO clienteProspect, CartCoadDTO prospect)
        {
            if (clienteProspect != null && prospect != null)
            {
                if (!string.IsNullOrWhiteSpace(prospect.UF) ||
                    !string.IsNullOrWhiteSpace(prospect.BAIRRO) ||
                    !string.IsNullOrWhiteSpace(prospect.CEP) ||
                    !string.IsNullOrWhiteSpace(prospect.COMPL) ||
                    !string.IsNullOrWhiteSpace(prospect.LOGRAD) ||
                    !string.IsNullOrWhiteSpace(prospect.MUNIC) ||
                    !string.IsNullOrWhiteSpace(prospect.NUMERO))
                {

                    MunicipioDTO mun = null;
                    int? munId = null;

                    if (!string.IsNullOrWhiteSpace(prospect.UF) &&
                        !string.IsNullOrWhiteSpace(prospect.MUNIC))
                    {
                        mun = _municipioSRV.BuscarPorUFDescricao(prospect.MUNIC, prospect.UF);

                        if (mun != null)
                            munId = mun.MUN_ID;
                    }

                    var prospEndereco = new ClienteProspectEnderecoDTO()
                    {
                        Bairro = prospect.BAIRRO,
                        CEP = prospect.CEP,
                        Complemento = prospect.COMPL,
                        Logradouro = prospect.LOGRAD,
                        Municipio = prospect.MUNIC,
                        Numero = prospect.NUMERO,
                        TipoEnd = 2,
                        UF = prospect.UF,
                        MunId = munId,
                        Munic = mun
                    };

                    clienteProspect.EnderecoFaturamento = prospEndereco;
                }
            }
        }

        public SalvarClienteResultDTO SalvarClienteEProspect(
           ClienteProspectDTO clienteProspect, int? repIdDemandante = null)
        {
            SalvarClienteResultDTO clienteResposta = null;
            ClienteDto cliente = ConverterClienteProspectParaCliente(clienteProspect);

            if (clienteProspect != null)
            {
                using (var scope = new TransactionScope())
                {
                    var salvamentoParam = new SalvarClienteParamDTO()
                    {
                        IgnorarRodizio = true,
                        AtualizarEncarteiramentos = true,
                        ValidacaoEmailTelCPFNaoRestritiva = true,
                        ForcarSalvamento = clienteProspect.ForcarSalvamento
                    };

                    clienteResposta = _clienteSRV.SalvarClienteAgenda(cliente, salvamentoParam);

                    if (clienteResposta.ResultadoValidacaoDuplicidade == null || 
                        !clienteResposta.ResultadoValidacaoDuplicidade.HasDuplication)
                        _carteiramentoSRV.ResolverEncarteiramento(cliente);                    

                    scope.Complete();
                }
            }
            return clienteResposta;
        }
    }
}

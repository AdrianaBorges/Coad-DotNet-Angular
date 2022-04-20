using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CLI_ID", "END_TIPO")]
    public class ClienteEnderecoSRV : ServiceAdapter<CLIENTES_ENDERECO, ClienteEnderecoDto, object>
    {
        private  ClienteEnderecoDAO _dao = new ClienteEnderecoDAO();

        public ClienteEnderecoSRV(){

            SetDao(_dao);
        }

        public ClienteEnderecoDto FindEnderecoCliente(int? cliId, int tipoEnd)
        {
            return _dao.FindEnderecoCliente(cliId, tipoEnd);
        }

        public bool HasEndereco(int? cliId, int tipoEnd)
        {
            return _dao.HasEndereco(cliId, tipoEnd);
        }

        public CLIENTES_ENDERECO BuscarEnderecosDeCliente(int idCliente, int tipoEnd)
        {
            return  _dao.BuscarEnderecoCliente(idCliente, tipoEnd);
        }

        public IList<ClienteEnderecoDto> FindEnderecoCliente(int? cliId)
        {
            return _dao.FindEnderecoCliente(cliId);
        }

        /// <summary>
        /// Pega no banco o endereço do cliente e atribui ao cliente passado
        /// </summary>
        /// <param name="cliente">Cliente que terá o endereço passado</param>
        /// <param name="marcarValidacaoBasica">Indica se a validação do endereço será básica ou completa</param>
        public void PreencherEnderecoCliente(ClienteDto cliente, bool marcarValidacaoBasica = false)
        {
            if (cliente != null && cliente.CLI_ID != null)
            {
                var cliId = cliente.CLI_ID;
                var lstClienteEndereco = FindEnderecoCliente((int) cliId);

                if(marcarValidacaoBasica && lstClienteEndereco != null && lstClienteEndereco.Count() > 0)
                {
                    foreach (var end in lstClienteEndereco)
                    {
                        end.validacaoTotal = false;
                    }
                }

                cliente.CLIENTES_ENDERECO = lstClienteEndereco;
            }
        }

        private void ChecarEDeletarEnderecoAusente(int? cliId, ICollection<ClienteEnderecoDto> enderecos)
        {
            if(cliId != null && enderecos != null)
            {
                var lstEnderecosDoBanco = FindEnderecoCliente(cliId);
                var lstParaExcluir = GetMissinList(lstEnderecosDoBanco, enderecos);

                DeleteAll(lstParaExcluir);
            }
        }
        public void SalvarEnderecos(IQueryable<ClienteEnderecoDto> enderecos, int? CLI_ID = null, string _assinatura = null, ClienteDto cliente = null)
        {
            if (enderecos != null)
            {
                ChecarEDeletarEnderecoAusente(CLI_ID, enderecos.ToList());

                if (cliente != null)
                {
                    CheckAndAssignKeyFromParentToChildsList(cliente, enderecos, "CLI_ID");
                }

                foreach (var end in enderecos)
                {
                    if (CLI_ID != null)
                    {
                        var _endAtu = this.FindById(end.CLI_ID, end.END_TIPO);

                        if (_endAtu != null)
                        {
                            ServiceFactory.RetornarServico<ClienteSRV>().GravarHistorico<ClienteEnderecoDto>(_endAtu, end, 3, CLI_ID, 100, _assinatura);
                        }
                    }
                    
                    _processarSalvamento(end, CLI_ID);
                }
            }
        }


        public void SalvarEndereco(ClienteEnderecoDto endereco, int? CLI_ID = null)
        {
            SalvarEnderecos(new List<ClienteEnderecoDto>() {endereco }.AsQueryable(), CLI_ID);
        }

        private void _processarSalvamento(ClienteEnderecoDto endereco, int? CLI_ID)
        {
            if (endereco != null && endereco.CLI_ID != null && endereco.END_TIPO != null) // verifico se o endereço já existe
            {
                _salvar(endereco);               
            }
            else // senão incluo a referência do cliente no endereço
            {

                if (CLI_ID == null)
                {
                    throw new Exception("O endereço precisa do CLI_ID para ser incluído");
                }
                endereco.CLI_ID = CLI_ID;
                endereco.MUNICIPIO = null;

                _salvar(endereco);
                        
            }                                    
            
        }

        private void _salvar(ClienteEnderecoDto endereco)
        {
            if (HasEndereco((int)endereco.CLI_ID, (int)endereco.END_TIPO)) // verifico mais uma vez, só que agora no banco, se o endereço existe
            {
                Merge(endereco, "CLI_ID", "END_TIPO"); // se existir eu atualizo
            }
            else
            {
                Save(endereco); // senão eu insiro
            }
        }

        /// <summary>
        /// Pega o endereço do cliente.
        /// Porém se ele não existir no objeto carrega do banco de dados.
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public ICollection<ClienteEnderecoDto> ExtrairEnderecosDoCliente (ClienteDto cliente)
        {
            if (cliente != null)
            {
                if (cliente.CLIENTES_ENDERECO != null && cliente.CLIENTES_ENDERECO.Count() > 0)
                {
                    return cliente.CLIENTES_ENDERECO;
                }

                PreencherEnderecoCliente(cliente);
                return cliente.CLIENTES_ENDERECO;
            }

            return null;
        }
        public List<PesquisaEnderecoDTO> BuscarEndereco(string _logradouro)
        {
            var endereco = _dao.BuscarEndereco(_logradouro);

            return endereco;
        }

        /// <summary>
        /// Procura o endereço de faturamento. Se não ouver traz o endereço padrão
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public ClienteEnderecoDto BuscarEnderecoDeFaturamentoOuEnderecoPadrao(ClienteDto cliente)
        {
            if (cliente != null)
            {
                if (cliente.CLIENTES_ENDERECO == null || cliente.CLIENTES_ENDERECO.Count() < 1)
                {
                    PreencherEnderecoCliente(cliente);
                }

                var endFat = (cliente.CLIENTES_ENDERECO.Where(x => x.END_TIPO == 2).Count() == 1) 
                    ? cliente.CLIENTES_ENDERECO.Where(x => x.END_TIPO == 2).FirstOrDefault() : 
                    cliente.CLIENTES_ENDERECO.FirstOrDefault();
                
                return endFat;
            }
            return null;
        }

        /// <summary>
        /// Procura o endereço de faturamento. Se não ouver traz o endereço padrão
        /// </summary>
        /// <param name="cliId"></param>
        /// <returns></returns>
        public ClienteEnderecoDto BuscarEnderecoDeFaturamentoOuEnderecoPadrao(int? cliId)
        {
            var endereco = _dao.BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliId);
            if (endereco != null)
                return endereco;

            var endFat = FindEnderecoCliente(cliId, 2);
            var endEnt = FindEnderecoCliente(cliId, 1);

            if (endFat != null)
                return endFat;
            if (endEnt != null)
                return endEnt;

            return null;
        }


        public IList<ClienteEnderecoDto> ProcessarEConcatenarClienteEndereco(ClienteDto cliente, IList<ClienteEnderecoDto> listaAcumulada)
        {
            var listaEndereco = cliente.CLIENTES_ENDERECO;
            if (listaEndereco != null)
            {
                foreach (var cliEnd in listaEndereco)
                {
                    if (cliEnd.CLI_ID == null)
                    {
                        cliEnd.CLI_ID = cliente.CLI_ID;
                    }
                }

                listaAcumulada = listaAcumulada.Concat(listaEndereco).ToList();
            }
            return listaAcumulada;
        }

        public void SalvarEnderecosVariosClientes(IEnumerable<ClienteDto> lstClientes)
        {
            IList<ClienteEnderecoDto> lstClienteEndereco = new List<ClienteEnderecoDto>();
 
            if (lstClientes != null)
            {
                foreach (var cli in lstClientes)
                {
                   lstClienteEndereco = ProcessarEConcatenarClienteEndereco(cli, lstClienteEndereco);
                }

                SaveOrUpdateNonIdentityKeyEntity(lstClienteEndereco, null, null, true);
            }
        }

        public ClienteEnderecoDto BuscarEnderecosDeClienteDto(int idCliente, int tipoEnd)
        {
            return  _dao.ToDTO(_dao.BuscarEnderecoCliente(idCliente, tipoEnd));
        }
    }
}

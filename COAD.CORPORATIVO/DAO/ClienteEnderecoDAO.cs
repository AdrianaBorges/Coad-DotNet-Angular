using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto.Custons;


namespace COAD.CORPORATIVO.DAO
{
    public class ClienteEnderecoDAO : DAOAdapter<CLIENTES_ENDERECO, ClienteEnderecoDto, object>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ClienteEnderecoDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        private IQueryable<CLIENTES_ENDERECO> templateQueryBusca(int? cliId, int tipoEnd)
        {
            return db.CLIENTES_ENDERECO.Where(t => t.CLI_ID == cliId && t.END_TIPO == tipoEnd);
        }

        public CLIENTES_ENDERECO BuscarEnderecoCliente(int? cliId, int tipoEnd)
        {
            var endereco = templateQueryBusca(cliId, tipoEnd).FirstOrDefault();
            return endereco;
        }

        public IList<ClienteEnderecoDto> FindEnderecoCliente(int? cliId)
        {
              var endereco = db.CLIENTES_ENDERECO.Where(t => t.CLI_ID == cliId);
            return ToDTO(endereco);
        }

        public ClienteEnderecoDto FindEnderecoCliente(int? cliId, int tipoEnd)
        {
            var endereco = BuscarEnderecoCliente(cliId, tipoEnd);
            return ToDTO(endereco);
        }

        public bool HasEndereco(int? cliId, int tipoEnd)
        {
            bool resp = (templateQueryBusca(cliId, tipoEnd).Count() > 0); // Preferi jogar o resultado em uma variável ao devolver direto por fins de debug.
            return resp;
        }

        public List<PesquisaEnderecoDTO> BuscarEndereco(string _logradouro)
        {
            var endereco = (from e in db.CLIENTES_ENDERECO
                            join c in db.CLIENTES on e.CLI_ID equals c.CLI_ID
                            join m in db.ASSINATURA on e.CLI_ID equals m.CLI_ID
                            where e.END_LOGRADOURO.Contains(_logradouro)
                            select new PesquisaEnderecoDTO()
                            {
                                ASN_NUM_ASSINATURA = m.ASN_NUM_ASSINATURA,
                                CLI_NOME = c.CLI_NOME,
                                END_LOGRADOURO = e.END_LOGRADOURO,
                                END_COMPLEMENTO = e.END_COMPLEMENTO,
                                END_NUMERO = e.END_NUMERO,
                                END_BAIRRO = e.END_BAIRRO,
                                END_CEP = e.END_CEP,
                                END_MUNICIPIO = e.END_MUNICIPIO,
                                END_UF = e.END_UF

                            }).ToList();
            
            return endereco;
        }

        public ClienteEnderecoDto BuscarEnderecoDeFaturamentoOuEnderecoPadrao(int? cliId)
        {
            var query = (from e in db.CLIENTES_ENDERECO where e.CLI_ID == cliId && 
                             (e.END_TIPO == 1  ||                             
                                (from subE in db.CLIENTES_ENDERECO where e.CLI_ID == cliId select subE).Count() == 1
                             ) 
                         select e);

            return ToDTO(query.FirstOrDefault());
        }

    }
}

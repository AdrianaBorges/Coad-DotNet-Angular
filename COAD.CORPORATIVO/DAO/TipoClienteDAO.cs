using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;

namespace COAD.CORPORATIVO.DAO
{
    public class TipoClienteDAO : DAOAdapter<TIPO_CLIENTE, TipoClienteDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public IList<TipoClienteDTO> BuscarTipoClienteAtivos()
        {            
            var listaTipoDeClienteAtivo = (from x in db.TIPO_CLIENTE where x.TIPO_CLI_ATIVO == 1 select x);
            return ToDTO(listaTipoDeClienteAtivo);
        }

        public IList<ClienteIntegrTipoClienteDTO> BuscarTiposDeClienteIntegracao()
        {
            var query = (from tpCli in db.TIPO_CLIENTE
                         where tpCli.TIPO_CLI_ATIVO == 1
                         select new ClienteIntegrTipoClienteDTO() { 
                            CodigoTipoCliente = tpCli.TIPO_CLI_ID,
                            DescricaoTipoCliente = tpCli.TIPO_CLI_DESCRICAO
                         });

            return query.ToList();
        }

        public IList<TipoClienteDTO> ListarTipoClienteEcommerce()
        {
            var query = (from tpCli in db.TIPO_CLIENTE
                         where
                            tpCli.TIPO_CLI_ATIVO == 1 &&
                            (tpCli.TIPO_CLI_ID == 2 || tpCli.TIPO_CLI_ID == 3)
                         select tpCli);

            return ToDTO(query);
        }
    }
}

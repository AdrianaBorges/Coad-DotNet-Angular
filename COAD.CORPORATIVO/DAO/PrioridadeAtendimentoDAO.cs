
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Data.Objects;


namespace COAD.CORPORATIVO.DAO
{

    public class PrioridadeAtendimentoDAO : AbstractGenericDao<PRIORIDADE_ATENDIMENTO, PrioridadeAtendimentoDTO, int>
    {
        public Pagina<PrioridadeAtendimentoDTO> GetPrioridadesByRepresentante(int? REP_ID, int pagina = 1, int registrosPorPagina = 5, int? RG_ID = null)
        {
            IQueryable<PRIORIDADE_ATENDIMENTO> query = GetDbSet()
                .Where(x => x.REP_ID == REP_ID && x.PRI_DATA_CONFIRMACAO == null && x.RG_ID == RG_ID)
                .OrderByDescending(or => or.PRI_DATA);

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public IList<PrioridadeAtendimentoDTO> GetPrioridadesByRepresentanteEOperadoras(int? REP_ID, int? CLI_ID, int? RG_ID = null)
        {
            IQueryable<PRIORIDADE_ATENDIMENTO> query = GetDbSet()
                .Where(x => x.REP_ID == REP_ID && x.CLI_ID == CLI_ID && x.RG_ID == RG_ID)
                .OrderByDescending(or => or.PRI_DATA);

            return ToDTO(query);
        }

        public IList<PrioridadeAtendimentoDTO> GetPrioridadesDoClienteByRegiaoDoRepresentante(int? RG_ID, int? CLI_ID)
        {
            IQueryable<PRIORIDADE_ATENDIMENTO> query = GetDbSet()
                .Where(x => x.REPRESENTANTE1.RG_ID == RG_ID && x.CLI_ID == CLI_ID && x.RG_ID == RG_ID)
                .OrderByDescending(or => or.PRI_DATA);

            return ToDTO(query);
        }

        public IList<PrioridadeAtendimentoDTO> GetPrioridadesDoCliente(int? CLI_ID, int? RG_ID = null)
        {
            IQueryable<PRIORIDADE_ATENDIMENTO> query = GetDbSet()
                .Where(x =>x.CLI_ID == CLI_ID && x.RG_ID == RG_ID)
                .OrderByDescending(or => or.PRI_DATA);

            return ToDTO(query);
        }

        public Pagina<PrioridadeAtendimentoDTO> ClientesComPrioridadeEncaminhados(DateTime data, int? UEN_ID = null, int? RG_ID = null, int pagina = 1, int registrosPorPagina = 100)
        {
            var db = GetDb<COADCORPEntities>(false);
            var query = (from pri_ate in db.PRIORIDADE_ATENDIMENTO
                         where EntityFunctions.TruncateTime(pri_ate.PRI_DATA) == EntityFunctions.TruncateTime(data) && pri_ate.TP_PRI_ID == 4
                         select pri_ate);

            if (RG_ID != null || UEN_ID != null)
            {
                query = (from car_cli in db.CARTEIRA_CLIENTE
                         join pri_ate in query on car_cli.CLI_ID equals pri_ate.CLI_ID
                         where (RG_ID == null || (car_cli.CARTEIRA.RG_ID == RG_ID && pri_ate.RG_ID == RG_ID))
                                && (UEN_ID == null || car_cli.CARTEIRA.UEN_ID == UEN_ID)
                         select pri_ate);
            }

            return ToDTOPage(query, pagina, registrosPorPagina);
        }
    }
}


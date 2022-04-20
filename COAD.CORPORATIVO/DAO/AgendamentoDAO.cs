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
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Data.Objects.SqlClient;
using System.Data.Objects;


namespace COAD.CORPORATIVO.DAO
{
    public class AgendamentoDAO : AbstractGenericDao<AGENDAMENTO, AgendamentoDTO,int>
    {
        private IQueryable<AGENDAMENTO> TemplateAgendamento(int? REP_ID, int? CLI_ID = null, DateTime? dataInicial = null, DateTime? dataFinal = null, 
            bool trazConfirmados = true, int? RG_ID = null)
        {
            IQueryable<AGENDAMENTO> query = GetDbSet().Where(
                x => x.REP_ID == REP_ID);

            if (dataInicial != null)
            {
                DateTime dataIniComparacao = ((DateTime)dataInicial).Date;
                query = query.Where(x => EntityFunctions.TruncateTime(x.AGE_DATA_AGENDAMENTO) >= dataIniComparacao);
            }

            if (dataFinal != null)
            {
                DateTime dataFimComparacao = ((DateTime)dataFinal).Date;
                query = query.Where(x => EntityFunctions.TruncateTime(x.AGE_DATA_AGENDAMENTO) <= dataFimComparacao);
            }

            if (!trazConfirmados)
            {
                query = query.Where(x => x.AGE_DATA_CONFIRMACAO == null);
            }

            if (RG_ID != null)
            {
                query = query.Where(x => x.RG_ID == RG_ID);
            }

            query = query.OrderByDescending(x => x.AGE_DATA_AGENDAMENTO);

            if (CLI_ID != null)
            {
                query = query.Where(x => x.CLI_ID == CLI_ID);
            }

            return query.OrderByDescending(or => or.AGE_DATA_AGENDAMENTO);
        }

        public Pagina<AgendamentoDTO> ListarAgendamentosDoDia(DateTime? data, int? REP_ID, int? pagina = 1, int? registrosPorPagina = 7, int? RG_ID = null)
        {
            IQueryable<AGENDAMENTO> query = TemplateAgendamento(REP_ID, trazConfirmados: false, RG_ID: RG_ID);
            query = query.Where(x => EntityFunctions.TruncateTime(x.AGE_DATA_AGENDAMENTO) == EntityFunctions.TruncateTime(data));

            return ToDTOPage(query, (int) pagina, (int) registrosPorPagina);
        }

        public Pagina<AgendamentoDTO> ListarAgendamentosAtrasados(DateTime? data, int? REP_ID, int? pagina = 1, int? registrosPorPagina = 7, int? RG_ID = null)
        {
            if (data != null)
            {
                IQueryable<AGENDAMENTO> query = TemplateAgendamento(REP_ID, trazConfirmados: false, RG_ID: RG_ID);
                query = query.Where(x => EntityFunctions.TruncateTime(x.AGE_DATA_AGENDAMENTO) < EntityFunctions.TruncateTime(data));

                return ToDTOPage(query, (int)pagina, (int)registrosPorPagina);
            }

            return new Pagina<AgendamentoDTO>();
            
        }

        public Pagina<AgendamentoDTO> ListarAgendamentosVindouros(DateTime? data, int? REP_ID, int? pagina = 1, int? registrosPorPagina = 7, int? RG_ID = null)
        {
            if (data != null)
            {
                IQueryable<AGENDAMENTO> query = TemplateAgendamento(REP_ID, trazConfirmados: false, RG_ID: RG_ID);
                query = query.Where(x => EntityFunctions.TruncateTime(x.AGE_DATA_AGENDAMENTO) > EntityFunctions.TruncateTime(data));

                return ToDTOPage(query, (int)pagina, (int)registrosPorPagina);
            }

            return new Pagina<AgendamentoDTO>();

        }

        public IQueryable<AGENDAMENTO> TemplateAgendamentosAteData(DateTime? data, int? REP_ID, int? RG_ID = null)
        {
            if (data != null)
            {
                IQueryable<AGENDAMENTO> query = TemplateAgendamento(REP_ID, trazConfirmados: false, RG_ID: RG_ID);
                query = query.Where(x => EntityFunctions.TruncateTime(x.AGE_DATA_AGENDAMENTO) <= EntityFunctions.TruncateTime(data));

                return query;
            }
            return new List<AGENDAMENTO>().AsQueryable();
        }

        public Pagina<AgendamentoDTO> Agendamentos(DateTime? dataInicial, DateTime? dataFinal, int? REP_ID, int? CLI_ID, 
            int pagina = 1, 
            int registrosPorPagina = 7,
            int? RG_ID = null)
        {
            var query = TemplateAgendamento(REP_ID, CLI_ID, dataInicial, dataFinal, RG_ID: RG_ID);
            return ToDTOPage(query, pagina, registrosPorPagina);            
        }

        public IList<AgendamentoDTO> GetAgendamentosAteData(DateTime? data, int? REP_ID, int? RG_ID = null)
        {
            var query = TemplateAgendamentosAteData(data, REP_ID, RG_ID);
            return ToDTO(query);
        }
    }
}

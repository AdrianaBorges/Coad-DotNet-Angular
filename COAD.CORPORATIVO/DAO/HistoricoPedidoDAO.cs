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
using System.Data.Objects;
using Coad.GenericCrud.Dao.Base.Pagination;


namespace COAD.CORPORATIVO.DAO
{
    public class HistoricoPedidoDAO : AbstractGenericDao<HISTORICO_PEDIDO, HistoricoPedidoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public HistoricoPedidoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public IQueryable<HISTORICO_PEDIDO> TemplateHistoricoDoCliente(int? IPE_ID, 
            DateTime? dataInicial = null, 
            DateTime? dataFinal = null, 
            int? PST_ID = null,
            int? PPI_ID = null)
        {
            var query = GetDbSet().Where(x =>
                (IPE_ID == null || x.IPE_ID == IPE_ID) &&
                (PPI_ID == null || x.PPI_ID == PPI_ID) &&
                (PST_ID == null || x.PST_ID == PST_ID));


            if (dataInicial != null)
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.HIP_DATA) >= EntityFunctions.TruncateTime(dataInicial));
            }

            if (dataFinal != null)
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.HIP_DATA) <= EntityFunctions.TruncateTime(dataFinal));
            }

            query = query.OrderByDescending(x => x.HIP_DATA);
            return query;

        }

        public Pagina<HistoricoPedidoDTO> ListarHistoricoPorItemPedido(int? IPE_ID, DateTime? dataInicial = null, DateTime? dataFinal = null,
            int? PST_ID = null, 
            int pagina = 1, int registroPorPagina = 10, int? PPI_ID = null)
        {
            var query = TemplateHistoricoDoCliente(IPE_ID, dataInicial, dataFinal, PST_ID, PPI_ID);

            return ToDTOPage(query, pagina, registroPorPagina);
        }

        public IList<HistoricoPedidoDTO> ListarHistoricoPorItemPedidoSemPaginacao(int CLI_ID, DateTime? dataInicial = null, 
            DateTime? dataFinal = null, 
            int? PST_ID = null,
            int? PPI_ID = null)
        {
            var query = TemplateHistoricoDoCliente(CLI_ID, dataInicial, dataFinal, PST_ID, PPI_ID);
            return ToDTO(query);
        }

    }
}

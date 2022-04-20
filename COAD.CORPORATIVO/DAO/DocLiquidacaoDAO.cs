using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class DocLiquidacaoDAO : AbstractGenericDao<DOC_LIQUIDACAO, DocLiquidacaoDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public DocLiquidacaoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public Pagina<DocLiquidacaoDTO> DocLiquidacao(string sigla = null, string descricao = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<DOC_LIQUIDACAO> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(sigla))
            {
                query = query.Where(x => x.DLI_SIGLA == sigla);
            }
            if (!String.IsNullOrWhiteSpace(descricao))
            {
                query = query.Where(x => x.DLI_DESCRICAO == descricao);
            }
            
            query = query.Where(x => x.DLI_DATA_EXCLUSAO == null);

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

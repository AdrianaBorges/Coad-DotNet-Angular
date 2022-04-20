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
using System.Configuration;
using System.Web.Configuration;

namespace COAD.CORPORATIVO.DAO
{
    public class OcorrenciaRemessaDAO : AbstractGenericDao<OCORRENCIA_REMESSA, OcorrenciaRemessaDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public OcorrenciaRemessaDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }
        public IList<OcorrenciaRemessaDTO> LerOcorrenciaRemessa(string bco)
        {
            var query = (from or in db.OCORRENCIA_REMESSA
                         where or.BAN_ID == bco
                         select or);

            return ToDTO(query);
        }

        public Pagina<OcorrenciaRemessaDTO> LerOcorrenciaRemessa(string bco = null, string cod = null, string rem = null, int numeroPagina = 1, int linhasPorPaginas = 7)
        {
            IQueryable<OCORRENCIA_REMESSA> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(bco))
            {
                query = query.Where(x => x.BAN_ID == bco);
            }
            if (!String.IsNullOrWhiteSpace(cod))
            {
                query = query.Where(x => x.OCM_CODIGO == cod);
            }
            if (!String.IsNullOrWhiteSpace(rem))
            {
                query = query.Where(x => x.OCM_CODIGO.Contains(rem));
            }
            
            return ToDTOPage(query, numeroPagina, linhasPorPaginas);
        }
    }
}

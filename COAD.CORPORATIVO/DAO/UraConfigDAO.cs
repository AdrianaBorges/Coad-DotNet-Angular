using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.CORPORATIVO.DAO
{
    public class UraConfigDAO : DAOAdapter<URA_CONFIG, UraConfigDTO>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public UraConfigDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }
        public IList<UraConfigDTO> BuscarConfiguracao(string _ura_id, int _pro_id)
        {
            IQueryable<URA_CONFIG> query = GetDbSet();
            query = query.Where(x => x.URA_ID == _ura_id && x.PRO_ID == _pro_id);

            return ToDTO(query);
        }
        public Pagina<UraConfigDTO> BuscarConfiguracaoPaginas(string _ura_id, int _pro_id, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<URA_CONFIG> query = GetDbSet();

            query = query.Where(x => x.URA_ID == _ura_id && x.PRO_ID == _pro_id);

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
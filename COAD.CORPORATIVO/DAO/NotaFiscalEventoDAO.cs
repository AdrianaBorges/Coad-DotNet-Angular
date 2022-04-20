

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COAD.CORPORATIVO.DAO
{
    public class NotaFiscalEventoDAO : AbstractGenericDao<NOTA_FISCAL_EVENTO, NotaFiscalEventoDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public NotaFiscalEventoDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public Pagina<NotaFiscalEventoDTO> ListarEventosNotaFiscal(int? nfID, int pagina = 1, int registrosPorPagina = 4)
        {
            var query = (from ntEv in db.NOTA_FISCAL_EVENTO
                         where ntEv.NF_ID == nfID
                         orderby ntEv.NEV_DATA descending
                         select ntEv);

            return ToDTOPage(query, pagina, registrosPorPagina);
        } 

    }
}

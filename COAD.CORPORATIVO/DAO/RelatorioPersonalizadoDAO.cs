
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


namespace COAD.CORPORATIVO.DAO
{
    public class RelatorioPersonalizadoDAO : AbstractGenericDao<RELATORIO_PERSONALIZADO, RelatorioPersonalizadoDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RelatorioPersonalizadoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public Pagina<RelatorioPersonalizadoDTO> ListarRelatorioPersonalizadoBase(string usuario = null, int pagina = 1, int registrosPorPagina = 5)
        {
            var query =  (from reP in db.RELATORIO_PERSONALIZADO 
                          where
                            (usuario == null || reP.USU_LOGIN == usuario) &&
                            reP.RET_RELATORIO_BASE == true &&
                            reP.DATA_EXCLUSAO == null
                              select reP);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public Pagina<RelatorioPersonalizadoDTO> ListarRelatorioPersonalizado(string usuario = null, int pagina = 1, int registrosPorPagina = 5)
        {
            var query = (from reP in db.RELATORIO_PERSONALIZADO
                         where
                            (usuario == null || reP.USU_LOGIN == usuario) &&
                            reP.RET_RELATORIO_BASE == null ||
                            reP.RET_RELATORIO_BASE == false &&
                            reP.DATA_EXCLUSAO == null
                         select reP);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

    }
}

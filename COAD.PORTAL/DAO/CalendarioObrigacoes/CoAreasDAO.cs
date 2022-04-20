using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PORTAL.Model.DTO;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;

namespace COAD.PORTAL.DAO.CalendarioObrigacoes
{
    public class CoAreasDAO : AbstractGenericDao<CO_AREAS, CoAreasDTO, string>
    {
        public CoAreasDAO()
        {
            SetProfileName("portal");
        }

        public Pagina<CoAreasDTO> Areas(string codigoArea, string nomeArea, int pagina = 1 , int nLinha = 7)
        {
            IQueryable<CO_AREAS> query = GetDbSet();

            if (!string.IsNullOrEmpty(codigoArea))
            {
                query = query.Where(x => x.COD_AREA.Contains(codigoArea));
            }

            if (!string.IsNullOrEmpty(nomeArea))
            {
                query = query.Where(x => x.NOME_AREA.Contains(nomeArea));
            }

            var repPagina = ToDTOPage(query, pagina, nLinha);

            return repPagina;
        }
   }
}

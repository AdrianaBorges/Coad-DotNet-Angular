using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class AreasDAO : AbstractGenericDao<AREAS_CONSULTORIA, AreasDTO, int>
    {

        public AreasDAO() : base()
        {
            SetProfileName( "GED" );
           
        }

        public Pagina<AreasDTO> Areas(int? areaId, string descricao = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<AREAS_CONSULTORIA> query = GetDbSet();

            if (descricao != null)
            {
                query = query.Where(x => x.ARE_CONS_DESCRICAO.Contains(descricao));
            }

            if (areaId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == areaId);
            }

            //query = query.Where(p => p.DATA_EXCLUSAO == null); ALT: 23/06/2015 - o campo de exclusão lógica não foi criado...

            return ToDTOPage(query, pagina, itensPorPagina);
        }
               
    }
}

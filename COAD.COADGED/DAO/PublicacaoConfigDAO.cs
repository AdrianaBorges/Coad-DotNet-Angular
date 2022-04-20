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

    public class PublicacaoConfigDAO : AbstractGenericDao<PUBLICACAO_CONFIG, PublicacaoConfigDTO, int>
    {
        public PublicacaoConfigDAO() : base()
        {
            SetProfileName( "GED" );
        }

        public Pagina<PublicacaoConfigDTO> PublicacaoConfig(int? publicacaoId, int? areaId, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_CONFIG> query = GetDbSet();

            if (publicacaoId != null)
            {
                query = query.Where(x => x.PUB_ID == publicacaoId);
            }

            if (areaId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == areaId);
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

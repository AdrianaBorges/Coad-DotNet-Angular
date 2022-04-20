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

    public class PublicacaoTitulacaoDAO : AbstractGenericDao<PUBLICACAO_TITULACAO, PublicacaoTitulacaoDTO, int>
    {
        public PublicacaoTitulacaoDAO() : base()
        {
            SetProfileName( "GED" );
        }

        public Pagina<PublicacaoTitulacaoDTO> PublicacaoTitulacao(int? publicacaoId, int? areaId, int? principal = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_TITULACAO> query = GetDbSet();

            if (publicacaoId != null)
            {
                query = query.Where(x => x.PUB_ID == publicacaoId);
            }

            if (areaId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == areaId);
            }

            if (principal != null)
            {
                var sim = (principal == 1);
                query = query.Where(x => x.PTI_PRINCIPAL == sim);
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

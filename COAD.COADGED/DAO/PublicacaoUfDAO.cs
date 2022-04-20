using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.UTIL.Grafico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PublicacaoUfDAO : AbstractGenericDao<PUBLICACAO_UF, PublicacaoUfDTO, object>
    {
        public PublicacaoUfDAO() : base()
        {
            SetProfileName( "GED" );
        }

        public Pagina<PublicacaoUfDTO> PublicacaoUf(int? publicacaoId=null, int? areaId=null, string ufId = null, string ano=null, int? numero=null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_UF> query = GetDbSet();

            if (publicacaoId != null)
            {
                query = query.Where(x => x.PUB_ID == publicacaoId);
            }
            if (areaId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == areaId);
            }
            if (ufId != null)
            {
                query = query.Where(x => x.UF_ID == ufId);
            }
            if (!String.IsNullOrWhiteSpace(ano)) {
                query = query.Where(x => x.INF_ANO == ano);
            }
            if (numero != null) {
                query = query.Where(x => x.INF_NUMERO == numero);
            }
            
            query = query.OrderBy( x => x.ARE_CONS_ID );

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

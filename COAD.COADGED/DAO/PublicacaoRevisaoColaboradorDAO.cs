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

    public class PublicacaoRevisaoColaboradorDAO : AbstractGenericDao<PUBLICACAO_REVISAO_COLABORADOR, PublicacaoRevisaoColaboradorDTO, int>
    {
        public PublicacaoRevisaoColaboradorDAO() : base()
        {
            SetProfileName( "GED" );
        }

        public Pagina<PublicacaoRevisaoColaboradorDTO> PublicacaoRevisaoColaborador(int? pubId = null, int? colecionadorId = null, int? colId = null, DateTime? data = null, string revisao = null, string editada = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_REVISAO_COLABORADOR> query = GetDbSet();

            if (pubId != null)
            {
                query = query.Where(x => x.PUB_ID == pubId);
            }
            if (colecionadorId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == colecionadorId);
            }
            if (colId != null)
            {
                query = query.Where(x => x.COL_ID == colId);
            }
            if (data != null)
            {
                query = query.Where(x => x.DATA == data);
            }
            if (!String.IsNullOrWhiteSpace(revisao))
            {
                query = query.Where(x => x.REVISAO.Contains(revisao));
            }
            if (!String.IsNullOrWhiteSpace(editada))
            {
                query = query.Where(x => x.EDITOU.Contains(editada));
            }
            query = query.OrderByDescending(x => x.PUB_ID);

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

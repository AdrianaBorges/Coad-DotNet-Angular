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

    public class PublicacaoBuscaDAO : AbstractGenericDao<PUBLICACAO_BUSCA, PublicacaoBuscaDTO, int>
    {

        public PublicacaoBuscaDAO() : base()
        {
            SetProfileName( "GED" );
           
        }

        public Pagina<PublicacaoBuscaDTO> PublicacaoBusca(int? pubId, int? colecionadorId, string palavra = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_BUSCA> query = GetDbSet();

            if (pubId != null)
            {
                query = query.Where(x => x.PUB_ID == pubId);
            }

            if (colecionadorId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == colecionadorId);
            }

            if (palavra != null)
            {
                palavra = palavra.ToString();
                query = query.Where(x => x.PBU_PALAVRA.Contains(palavra)).OrderBy(x => x.ARE_CONS_ID).ThenBy(x => x.PUB_ID);
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

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

    public class SecoesDAO : AbstractGenericDao<SECOES, SecoesDTO, int>
    {

        public SecoesDAO() : base()
        {
            SetProfileName( "GED" );
           
        }

        public Pagina<SecoesDTO> Secoes(int? secaoId, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<SECOES> query = GetDbSet();

            if (descricao != null)
            {
                descricao = descricao.ToString();
                query = query.Where(x => x.SEC_DESCRICAO.Contains(descricao));
            }

            query = query.Where(x => x.SEC_ATIVO == ativoId);
            
            query = query.Where(p => p.DATA_EXCLUSAO == null);

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

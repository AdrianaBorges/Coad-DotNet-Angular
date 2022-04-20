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

    public class PublicacaoRevisaoDAO : AbstractGenericDao<PUBLICACAO_REVISAO, PublicacaoRevisaoDTO, int>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 

        public PublicacaoRevisaoDAO() : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);
        }

        public Pagina<PublicacaoRevisaoDTO> RevisaoTecnica(int? informativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_REVISAO> query = GetDbSet();
            if (informativo != null)
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_NUMERO == informativo select uf.PUB_ID).Contains(x.PUB_ID));
            }
            if (!String.IsNullOrWhiteSpace(anoInformativo))
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_ANO == anoInformativo select uf.PUB_ID).Contains(x.PUB_ID));
            }
            query = query.Where(x => x.REV_TC == "L" && x.PUBLICACAO.PUB_ATIVO == 1);
            return ToDTOPage(query, pagina, itensPorPagina);
        }

        public Pagina<PublicacaoRevisaoDTO> Digitacao(int? informativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_REVISAO> query = GetDbSet();
            if (informativo != null)
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_NUMERO == informativo select uf.PUB_ID).Contains(x.PUB_ID));
            }
            if (!String.IsNullOrWhiteSpace(anoInformativo))
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_ANO == anoInformativo select uf.PUB_ID).Contains(x.PUB_ID));
            }
            query = query.Where(x => x.DIG_TC == "L" && x.PUBLICACAO.PUB_ATIVO == 1);
            return ToDTOPage(query, pagina, itensPorPagina);
        }

        public Pagina<PublicacaoRevisaoDTO> RevisaoOrtografica(int? informativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_REVISAO> query = GetDbSet();
            if (informativo != null)
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_NUMERO == informativo select uf.PUB_ID).Contains(x.PUB_ID));
            }
            if (!String.IsNullOrWhiteSpace(anoInformativo))
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_ANO == anoInformativo select uf.PUB_ID).Contains(x.PUB_ID));
            }
            query = query.Where(x => x.REV_OR == "L" && x.PUBLICACAO.PUB_ATIVO == 1);
            return ToDTOPage(query, pagina, itensPorPagina);
        }

        public Pagina<PublicacaoRevisaoDTO> PublicacaoRevisao(int? pubId = null, int? colecionadorId = null, string revisaoTecnica = null, string digitacao = null, string revisaoOrtografica = null,
                                                              int? informativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_REVISAO> query = GetDbSet();

            if (pubId != null)
            {
                query = query.Where(x => x.PUB_ID == pubId);
            }
            if (colecionadorId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == colecionadorId);
            }
            if (!String.IsNullOrWhiteSpace(revisaoTecnica))
            {
                query = query.Where(x => x.REV_TC.Contains(revisaoTecnica));
            }
            if (!String.IsNullOrWhiteSpace(digitacao))
            {
                query = query.Where(x => x.DIG_TC.Contains(digitacao));
            }
            if (!String.IsNullOrWhiteSpace(revisaoOrtografica))
            {
                query = query.Where(x => x.REV_OR.Contains(revisaoOrtografica));
            }
            if (informativo != null)
            { 
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_NUMERO == informativo select uf.PUB_ID).Contains(x.PUB_ID));
            }
            if (!String.IsNullOrWhiteSpace(anoInformativo))
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_ANO == anoInformativo select uf.PUB_ID).Contains(x.PUB_ID));
            }

            query = query.Where(x => x.PUBLICACAO.PUB_ATIVO == 1 && x.PUBLICACAO.PUB_ID == x.PUB_ID).OrderByDescending(x => x.PUB_ID);

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

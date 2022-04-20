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

    public class PublicacaoAlteracaoRevogacaoDAO : AbstractGenericDao<PUBLICACAO_ALTERACAO_REVOGACAO, PublicacaoAlteracaoRevogacaoDTO, int>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 

        public PublicacaoAlteracaoRevogacaoDAO() : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);
        }

        public Pagina<PublicacaoAlteracaoRevogacaoDTO> PublicacaoAlteracaoRevogacao(int? id, int? publicacaoId, string tipo=null, int? tipoAtoId=null, string nrAto=null, DateTime? dtAto=null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_ALTERACAO_REVOGACAO> query = GetDbSet();

            if (id != null)
            {
                query = query.Where(x => x.PAR_ID == id);
            }

            if (publicacaoId != null)
            {
                query = query.Where(x => x.PUB_ID == publicacaoId);
            }

            if (tipo != null)
            {
                tipo = tipo.ToString();
                query = query.Where(x => x.PAR_TIPO.Contains(tipo));
            }

            if (nrAto != null)
            {
                nrAto = nrAto.ToString();
                query = query.Where(x => x.PUB_NUMERO_ATO.Contains(nrAto));
            }

            if (tipoAtoId != null)
            {
                query = query.Where(x => x.TIP_ATO_ID == tipoAtoId);
            }

            if (dtAto != null)
            {
                query = query.Where(x => x.PUB_DATA_ATO.Value == dtAto.Value);
                //query = query.Where(x => (from pub in db.PUBLICACAO
                //                          where pub.PUB_DATA_ATO != null &&
                //                                pub.PUB_DATA_ATO.Value.Day == dtAto.Value.Day &&
                //                                pub.PUB_DATA_ATO.Value.Month == dtAto.Value.Month &&
                //                                pub.PUB_DATA_ATO.Value.Year == dtAto.Value.Year
                //                          select pub.PUB_ID).Contains(x.PUB_ID));
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

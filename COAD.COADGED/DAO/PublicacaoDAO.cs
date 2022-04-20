using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PublicacaoDAO : AbstractGenericDao<PUBLICACAO, PublicacaoDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 

        public PublicacaoDAO() : base()
        {
            SetProfileName("GED");
            db = GetDb<COADGEDEntities>(false);
        }

        public Pagina<PublicacaoDTO> Publicacoes(int? tpAto=null, string nrAto=null, DateTime? dtAto=null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO> query = GetDbSet();

            if (tpAto != null)
            {
                query = query.Where(x => x.TIP_ATO_ID == tpAto);
            }
            if (!String.IsNullOrWhiteSpace(nrAto))
            {
                query = query.Where(x => x.PUB_NUMERO_ATO == nrAto);
            }
            if (dtAto != null)
            {
                query = query.Where(x => x.PUB_DATA_ATO == dtAto);
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

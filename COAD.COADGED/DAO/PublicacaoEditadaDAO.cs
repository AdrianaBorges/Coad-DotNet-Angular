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

    public class PublicacaoEditadaDAO : AbstractGenericDao<PUBLICACAO_EDITADA, PublicacaoEditadaDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 

        public PublicacaoEditadaDAO() : base()
        {
            SetProfileName("GED");
            db = GetDb<COADGEDEntities>(false);
        }

        public IList<PublicacaoEditadaDTO> BuscarPublicacaoSendoEditada(int pub_id, string usuario)
        {
            IQueryable<PUBLICACAO_EDITADA> query = GetDbSet();

            query = query.Where(x => x.PUB_ID == pub_id && x.USU_LOGIN == usuario && x.EDT_LIBERADA == null);

            return ToDTO(query);
        }

        public IList<PublicacaoEditadaDTO> BuscarSomenteLeitura(int pub_id)
        {
            IQueryable<PUBLICACAO_EDITADA> query = GetDbSet();

            query = query.Where(x => x.PUB_ID == pub_id && x.EDT_EDITANDO == true && x.EDT_LIBERADA == null);
            
            return ToDTO(query);
        }

        public Pagina<PublicacaoEditadaDTO> Buscar(int? pub_id = null, string usuario = null, DateTime? acessouAposHora = null, DateTime? liberouAposHora = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_EDITADA> query = GetDbSet();

            if (pub_id != null)
            {
                query = query.Where(x => x.PUB_ID == pub_id);
            }
            if (!String.IsNullOrWhiteSpace(usuario))
            {
                query = query.Where(x => x.USU_LOGIN == usuario);
            }
            if (acessouAposHora != null)
            {
                query = query.Where(x => x.EDT_HORARIO >= acessouAposHora);
            }
            if (liberouAposHora != null)
            {
                query = query.Where(x => x.EDT_LIBERADA >= liberouAposHora);
            }

            query = query.OrderByDescending(x => x.EDT_ID);

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

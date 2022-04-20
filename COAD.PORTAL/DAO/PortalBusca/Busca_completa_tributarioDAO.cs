using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PORTAL.Model.DTO.PortalBusca;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.DAO.PortalBusca
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...
    public class Busca_completa_tributarioDAO : AbstractGenericDao<busca_completa_tributario, Busca_completa_tributarioDTO, int>
    {
        public Busca_completa_tributarioDAO() : base()
        {
            SetProfileName("portalBusca");
        }

        public Pagina<Busca_completa_tributarioDTO> Busca(int? idGED=null, int? id=null, int? id_conteudo=null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<busca_completa_tributario> query = GetDbSet();

            if (idGED != null)
            {
                query = query.Where(x => x.idGED == idGED);
            }

            if (id != null)
            {
                query = query.Where(x => x.id == id);
            }

            if (id_conteudo != null)
            {
                query = query.Where(x => x.id_conteudo == id_conteudo);
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

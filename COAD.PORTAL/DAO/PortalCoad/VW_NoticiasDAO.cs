using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.DAO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.PortalCoad
{
    public class VW_NoticiasDAO : AbstractGenericDao<VW_BuscarNoticias, VW_NoticiasDTO, int>
    {
        private coadEntities db { get; set; }

        public VW_NoticiasDAO()
            : base()
        {
            SetProfileName("portalCoad");
            db = GetDb<coadEntities>(false);
        }

        public List<VW_NoticiasDTO> NoticiasEmOrdemDescendente(int pagina = 0, int itensPorPagina = 10)
        {
            List<VW_BuscarNoticias> query = GetDbSet().SqlQuery("SELECT * FROM VW_BuscarNoticias ORDER BY nmdata_cadastro DESC LIMIT " + pagina + "," + itensPorPagina + "").ToList();

            List<VW_NoticiasDTO> noticiasDTO = new List<VW_NoticiasDTO>();
            if (query != null)
            {
                foreach (var noticia in query)
                {
                    noticiasDTO.Add(ToDTO(noticia));
                }
            }
            return noticiasDTO;
        }

        public VW_NoticiasDTO UltimaNoticia()
        {
            VW_BuscarNoticias query = GetDbSet().OrderByDescending(x => x.nmdata_cadastro).FirstOrDefault();
            return ToDTO(query);
        }

        public VW_NoticiasDTO BuscarNoticiaPorId(int id)
        {
            VW_BuscarNoticias query = GetDbSet().Where(x => x.nmid == id).FirstOrDefault();
            return ToDTO(query);
        }

        public Pagina<VW_NoticiasDTO> NoticiasFiltro(string titulo, string texto, string descricao, int pagina = 1, int nLinha = 7)
        {
            IQueryable<VW_BuscarNoticias> query = GetDbSet().Where(x => x.ncverbete.Contains(titulo) && x.nmtexto.Contains(texto) && x.ngdescricao.Contains(descricao)).OrderByDescending(x => x.nmdata_cadastro);
            return ToDTOPage(query, pagina, nLinha);
        }
    }
}

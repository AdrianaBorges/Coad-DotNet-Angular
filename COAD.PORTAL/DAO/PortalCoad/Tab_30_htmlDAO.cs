using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.DAO.PortalCoad
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...
    public class Tab_30_htmlDAO : AbstractGenericDao<tab_30_html, Tab_30_htmlDTO, int>
    {
        public coadEntities db { get { return GetDb<coadEntities>(); } set { } }

        public Tab_30_htmlDAO() : base()
        {
            SetProfileName("portalCoad");
            db = GetDb<coadEntities>(false);
        }

        public Pagina<Tab_30_htmlDTO> Coad(int? idGED = null, int? id = null, int? modulo = null, string html = null, int? carregarMais = null, int? apartirDe = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<tab_30_html> query = GetDbSet();

            if (id != null)
            {
                query = query.Where(x => x.id == id);
            }

            if (idGED != null)
            {
                query = query.Where(x => x.idGED == idGED);
            }

            if (modulo != null)
            {
                query = query.Where(x => x.modulo == modulo);
            }

            if (!String.IsNullOrWhiteSpace(html))
            {
                if (apartirDe == null)
                    apartirDe = 0;

                string _SQL = "select * from tab_30_html where MATCH(html) AGAINST ('" + html + "') limit " + apartirDe.ToString() + ", 100";

                query = GetDbSet().SqlQuery(_SQL).AsQueryable();
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
        public IQueryable<Tab_30_htmlDTO> Totalizar(string html = null, int? apartirDe = null)
        {
            if (!String.IsNullOrWhiteSpace(html))
            {
                if (apartirDe == null)
                    apartirDe = 0;

                string _SQL = "select count(1) as total from tab_30_html where MATCH(html) AGAINST ('" + html + "') limit " + apartirDe.ToString() + ", 100";

                return db.Database.SqlQuery<Tab_30_htmlDTO>(_SQL).AsQueryable();
            }
            return null;
        }
    }
}

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
    public class Tab_31DAO : AbstractGenericDao<tab_31, Tab_31DTO, int>
    {
        private coadEntities db { get; set; }

        public Tab_31DAO() : base()
        {
            SetProfileName("portalCoad");
            db = GetDb<coadEntities>(false);
        }

        public Pagina<Tab_31DTO> Coad(int? idGED = null,
                                      int? id = null,
                                      string tipo_materia = null,
                                      string expressao_ato = null,
                                      string num = null,
                                      DateTime? datadoato = null,
                                      string anoAto = null,
                                      string ano = null,
                                      string informativo = null,
                                      string uf = null,
                                      string gg = null,
                                      string vb = null,
                                      string svb = null,
                                      int? indice = null,
                                      DateTime? dtCadastro = null,
                                      int? colecionadorId = null,
                                      string html = null,
                                      int? carregarMais = null,
                                      int? apartirDe = null,
                                      int pagina = 1,
                                      int itensPorPagina = 10)
        {
            IQueryable<tab_31> query = GetDbSet();

            if (idGED != null)
            {
                query = query.Where(x => x.idGED == idGED);
            }
            if (id != null)
            {
                query = query.Where(x => x.id == id);
            }
            if (!String.IsNullOrWhiteSpace(tipo_materia))
            {
                query = query.Where(x => x.tipo_materia == tipo_materia);
            }
            if (!String.IsNullOrWhiteSpace(expressao_ato))
            {
                query = query.Where(x => x.expressao_ato == expressao_ato);
            }
            if (!String.IsNullOrWhiteSpace(num))
            {
                query = query.Where(x => x.num == num);
            }
            if (datadoato != null)
            {
                DateTime dt = (DateTime)datadoato;

                int dd = System.Convert.ToInt32(dt.ToString("dd"));
                int mm = System.Convert.ToInt32(dt.ToString("MM"));
                int yyyy = System.Convert.ToInt32(dt.ToString("yyyy"));

                query = query.Where(x => (from pub in db.tab_31
                                          where pub.datadoato != null &&
                                              pub.datadoato.Value.Day == dd &&
                                              pub.datadoato.Value.Month == mm &&
                                              pub.datadoato.Value.Year == yyyy
                                          select pub.id).Contains(x.id));
            }
            if (anoAto != null)
            {
                DateTime dt = (DateTime)datadoato;

                int yyyy = System.Convert.ToInt32(dt.ToString("yyyy"));

                query = query.Where(x => (from pub in db.tab_31
                                          where pub.datadoato != null &&
                                                pub.datadoato.Value.Year == yyyy
                                          select pub.id).Contains(x.id));
            }
            if (!String.IsNullOrWhiteSpace(ano))
            {
                query = query.Where(x => x.ano == ano);
            }
            if (!String.IsNullOrWhiteSpace(informativo))
            {
                int inf = System.Convert.ToInt32(informativo);
                query = query.Where(x => x.informativo == inf);
            }
            if (!String.IsNullOrWhiteSpace(uf))
            {
                if (colecionadorId == 2)
                {
                    query = query.Where(x => x.colec.Contains(uf) || x.colec == "TODOS OS ESTADOS");
                }
                else
                {
                    query = query.Where(x => x.colec.Contains(uf));
                }
            }
            if (!String.IsNullOrWhiteSpace(gg))
            {
                query = query.Where(x => x.gg == gg);
            }
            if (!String.IsNullOrWhiteSpace(vb))
            {
                query = query.Where(x => x.vb == vb);
            }
            if (!String.IsNullOrWhiteSpace(svb))
            {
                query = query.Where(x => x.svb == svb);
            }
            if (dtCadastro != null)
            {
                DateTime dt = (DateTime)dtCadastro;

                int dd = System.Convert.ToInt32(dt.ToString("dd"));
                int mm = System.Convert.ToInt32(dt.ToString("MM"));
                int yyyy = System.Convert.ToInt32(dt.ToString("yyyy"));

                query = query.Where(x => (from pub in db.tab_31
                                          where pub.dataCadastro != null &&
                                              pub.dataCadastro.Day == dd &&
                                              pub.dataCadastro.Month == mm &&
                                              pub.dataCadastro.Year == yyyy
                                          select pub.id).Contains(x.id));
            }

            if (indice != null)
            {
                query = query.Where(x => x.informativo >= 1 && x.informativo <= 52);
                query = query.Where(x => (x.ano != null && x.ano != "") && (x.informativo != null));
                if (indice >= 4)
                {
                    query = query.Where(x => (from pub in db.tab_31
                                              orderby pub.expressao_ato, pub.num, pub.datadoato, pub.gg, pub.vb
                                              select pub.id).Contains(x.id));
                }
                else
                    query = query.OrderBy(x => x.gg).ThenBy(x => x.vb).ThenBy(x => x.svb);
            }
            else
            {
                query = query.OrderBy(x => x.gg).ThenBy(x => x.vb).ThenBy(x => x.svb);
            }

            if (!String.IsNullOrWhiteSpace(html))
            {
                if (apartirDe == null)
                    apartirDe = 0;

                string _SQL = "select t.*, h.html FROM tab_31 t INNER JOIN tab_31_html h ON t.modulo=h.modulo where MATCH(html) AGAINST ('" + html + "') limit " + apartirDe.ToString() + ", 100";

                query = GetDbSet().SqlQuery(_SQL).AsQueryable();
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }

        public IQueryable<Tab_31DTO> Totalizar(string html = null, int? apartirDe = null)
        {
            if (!String.IsNullOrWhiteSpace(html))
            {
                if (apartirDe == null)
                    apartirDe = 0;

                string _SQL = "select count(1) as total FROM tab_31 t INNER JOIN tab_31_html h ON t.modulo=h.modulo where MATCH(html) AGAINST ('" + html + "') limit " + apartirDe.ToString() + ", 100";

                return db.Database.SqlQuery<Tab_31DTO>(_SQL).AsQueryable();
            }
            return null;
        }





        public IList<string> RecuperarVerbetesSemRepeticao(string label)
        {
            IQueryable<string> query = GetDbSet().Where(x => x.colec == label &&
                x.status_vb == "S" &&
                string.IsNullOrEmpty(x.vb.Replace(" ", "")) != true &&
                x.LABEL == "Orientacao" &&
                x.Revisar == "N").OrderBy(x => x.vb).Select(x => x.vb).Distinct();

            return query.ToList();
        }
    }
}

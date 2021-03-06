using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.PortalCoad;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Service.PortalCoad
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...
    public class Tab_31_htmlSRV : GenericService<tab_31_html, Tab_31_htmlDTO, int>
    {
        private Tab_31_htmlDAO _dao = new Tab_31_htmlDAO();

        public Tab_31_htmlSRV()
        {
            Dao = _dao;
        }

        public Pagina<Tab_31_htmlDTO> Coad(int? idGED, int? id = null, int? modulo = null, string html = null, int? carregarMais = null, int? apartirDe = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(idGED, id, modulo, html, carregarMais, apartirDe, pagina, itensPorPagina);
            return resp;
        }
        public IQueryable<Tab_31_htmlDTO> Totalizar(string html = null, int? apartirDe = null)
        {
            return _dao.Totalizar(html, apartirDe);
        }

        public void SalvarTab_31_html(Tab_31_htmlDTO coad)
        {
            try
            {
                if (coad.idGED != null)
                {
                    Merge(coad, "idGED");
                }
                else
                {
                    Save(coad);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var _erro = new FormattedDbEntityValidationException(dbEx);

                SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                throw _erro;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }

    }
}

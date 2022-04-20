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
//using MySql.Data.MySqlClient;
using System.Configuration;

namespace COAD.PORTAL.Service.PortalCoad
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...
    public class Tab_30SRV : GenericService<tab_30, Tab_30DTO, int>
    {
        private Tab_30DAO _dao = new Tab_30DAO();

        public Tab_30SRV()
        {
            Dao = _dao;
        }

        public Pagina<Tab_30DTO> Sumario(string ano, string informativo, string uf = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, informativo, uf, null, null, null, 1, null, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_30DTO> IndiceOrientacoesLembretes(string ano, string uf = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, null, uf, null, null, null, 2, null, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_30DTO> IndiceAlfabeticoRemissivo(string ano, string uf = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, null, uf, null, null, null, 3, null, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_30DTO> IndiceNumericoAtos(string ano, string uf = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, null, uf, null, null, null, 4, null, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_30DTO> IndiceNumericoAlteracoesRevogacoes(string ano, string uf = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, null, uf, null, null, null, 5, null, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_30DTO> Coad(int? idGED = null, int? id = null, string expressao_ato = null, string num = null, DateTime? datadoato = null, string anoAto = null,
            string ano = null, string informativo = null, string uf = null, string gg = null, string vb = null, string svb = null, DateTime? dataCadastro = null, string html = null,
            int? carregarMais = null, int? apartirDe = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(idGED, id, null, expressao_ato, num, datadoato, anoAto, ano, informativo, uf, gg, vb, svb, 1, dataCadastro, html, carregarMais, apartirDe, pagina, itensPorPagina);
            return resp;
        }

        public IQueryable<Tab_30DTO> Totalizar(string html = null, int? apartirDe = null)
        {
            return _dao.Totalizar(html, apartirDe);
        }

        public void SalvarTab_30(Tab_30DTO coad)
        {
            try
            {
                if (coad.id != null)
                {
                    Merge(coad, "id");
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

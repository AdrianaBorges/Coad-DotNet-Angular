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
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;

namespace COAD.PORTAL.Service.PortalCoad
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...
    public class Tab_31SRV : GenericService<tab_31, Tab_31DTO, int>
    {
        private Tab_31DAO _dao = new Tab_31DAO();

        public Tab_31SRV()
        {
            Dao = _dao;
        }

        public Pagina<Tab_31DTO> Sumario(string ano, string informativo, string uf = null, int? colecionadorId = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, informativo, uf, null, null, null, 1, null, colecionadorId, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_31DTO> IndiceOrientacoesLembretes(string ano, string uf = null, int? colecionadorId = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, null, uf, null, null, null, 2, null, colecionadorId, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_31DTO> IndiceAlfabeticoRemissivo(string ano, string uf = null, int? colecionadorId = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, null, uf, null, null, null, 3, null, colecionadorId, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_31DTO> IndiceNumericoAtos(string ano, string uf = null, int? colecionadorId = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, null, uf, null, null, null, 4, null, colecionadorId, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_31DTO> IndiceNumericoAlteracoesRevogacoes(string ano, string uf = null, int? colecionadorId = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(null, null, null, null, null, null, null, ano, null, uf, null, null, null, 5, null, colecionadorId, null, null, null, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<Tab_31DTO> Coad(int? idGED = null, int? id = null, string expressao_ato = null, string num = null, DateTime? datadoato = null, string anoAto = null,
            string ano = null, string informativo = null, string uf = null, string gg = null, string vb = null, string svb = null, DateTime? dataCadastro = null, int? colecionadorId = null,
            string html = null, int? carregarMais = null, int? apartirDe = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Coad(idGED, id, null, expressao_ato, num, datadoato, anoAto, ano, informativo, uf, gg, vb, svb, 1, dataCadastro, colecionadorId, html, carregarMais,
                apartirDe, pagina, itensPorPagina);
            return resp;
        }

        public IQueryable<Tab_31DTO> Totalizar(string html = null, int? apartirDe = null)
        {
            return _dao.Totalizar(html, apartirDe);
        }


        public void SalvarTab_31(Tab_31DTO coad)
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

        public IList<ComboDTO> RecuperarVerbetesSemRepeticao(string label)
        {
            var verbetes = _dao.RecuperarVerbetesSemRepeticao(label);
            IList<ComboDTO> combo = new List<ComboDTO>();
            foreach (var verbete in verbetes)
            {
                ComboDTO cbo = new ComboDTO();
                cbo.texto = verbete;
                cbo.valor = verbete;
                combo.Add(cbo);
            }

            return combo;
        }
    }
}

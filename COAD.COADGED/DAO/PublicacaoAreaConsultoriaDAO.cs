using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PublicacaoAreaConsultoriaDAO : AbstractGenericDao<PUBLICACAO_AREAS_CONSULTORIA, PublicacaoAreaConsultoriaDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } }

        public PublicacaoAreaConsultoriaDAO() : base()
        {
            SetProfileName("GED");
            db = GetDb<COADGEDEntities>(false);
        }

        // busca para o Painel...
        public IQueryable<PublicacaoAreaConsultoriaDTO> Painel(int? colaboradorId = null, int? colecionadorId = null, int? nrInformativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 999999)
        {
            string reprovadaPeloColaborador = "RP_COL=0, ";
            string where = "";
            if (colaboradorId != null)
            {
                reprovadaPeloColaborador = "RP_COL=case when exists(select REV_OR from PUBLICACAO_REVISAO r where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID and " +
                                           "((REV_TC='R' or DIG_TC='R' or REV_OR='R') and r.COL_ID=" + colaboradorId.ToString() + ")) then 1 else 0 end, ";
            }
            if (colaboradorId != null || colecionadorId != null || !String.IsNullOrWhiteSpace(anoInformativo) || nrInformativo != null)
            {
                where = "where ";
                where += colecionadorId != null ? "a.ARE_CONS_ID=" + colecionadorId.ToString() + " and " : "";
                where += colaboradorId != null ? "p.PUB_ID=a.PUB_ID and p.USU_LOGIN in(select USU_LOGIN from COLABORADOR where COL_ID=" + colaboradorId.ToString() + ") and " : "";
                where += !String.IsNullOrWhiteSpace(anoInformativo) ? "u.INF_ANO=" + anoInformativo.ToString() + " and " : "";
                where += nrInformativo != null ? "u.INF_NUMERO=" + nrInformativo.ToString() + " and " : "";
                where = where.Substring(0, where.LastIndexOf(" and "));
            }
            var q = "select " + (colecionadorId != null ? "ARE_CONS_ID, " : "") +
                    "rdc=SUM(rdc),rvt=SUM(rvt),dgt=SUM(dgt),rvo=SUM(rvo),dia=SUM(dia),rp_rvt=SUM(rp_rvt),rp_dgt=SUM(rp_dgt),rp_rvo=SUM(rp_rvo), " +
                    "rp_col=SUM(rp_col),ed_rvt=SUM(ed_rvt),ed_dgt=SUM(ed_dgt),ed_rvo=SUM(ed_rvo) " +
                    "from( " +
                    "select distinct a.ARE_CONS_ID, a.PUB_ID, " +

                    "RDC=case when coalesce((select REV_TC from PUBLICACAO_REVISAO r where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID),'')in('R','') then 1 else 0 end, " +
                    "RVT=case when ((select REV_TC from PUBLICACAO_REVISAO r where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID)in('L')) then 1 else 0 end, " +
                    "DGT=case when ((select DIG_TC from PUBLICACAO_REVISAO r where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID)in('L')) then 1 else 0 end, " +
                    "RVO=case when ((select REV_OR from PUBLICACAO_REVISAO r where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID)in('L')) then 1 else 0 end, " +
                    "DIA=case when ((select REV_OR from PUBLICACAO_REVISAO r where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID)in('A')) then 1 else 0 end, " +

                    "RP_RVT=case when ((select REV_TC from PUBLICACAO_REVISAO r where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID)in('R')) then 1 else 0 end, " +
                    "RP_DGT=case when ((select DIG_TC from PUBLICACAO_REVISAO r where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID)in('R')) then 1 else 0 end, " +
                    "RP_RVO=case when ((select REV_OR from PUBLICACAO_REVISAO r where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID)in('R')) then 1 else 0 end, " +
                    reprovadaPeloColaborador +

                    "ED_RVT=case when exists(select EDITOU from PUBLICACAO_REVISAO_COLABORADOR r " +
                    "                        where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID and r.EDITOU='S' and r.REVISAO='T') then 1 else 0 end, " +
                    "ED_DGT=case when exists(select EDITOU from PUBLICACAO_REVISAO_COLABORADOR r " +
                    "                        where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID and r.EDITOU='S' and r.REVISAO='D') then 1 else 0 end, " +
                    "ED_RVO=case when exists(select EDITOU from PUBLICACAO_REVISAO_COLABORADOR r " +
                    "                        where r.ARE_CONS_ID=a.ARE_CONS_ID and r.PUB_ID=a.PUB_ID and r.EDITOU='S' and r.REVISAO='O') then 1 else 0 end " +
                    "from " +
                    "PUBLICACAO_AREAS_CONSULTORIA a inner join " +
                    "PUBLICACAO p on p.PUB_ID=a.PUB_ID inner join " +
                    "PUBLICACAO_UF u on a.ARE_CONS_ID=u.ARE_CONS_ID and a.PUB_ID=u.PUB_ID " +
                    where +
                    ")l1 " +
                    (colecionadorId != null ? "group by ARE_CONS_ID" : "");

            IQueryable<PublicacaoAreaConsultoriaDTO> query = db.Database.SqlQuery<PublicacaoAreaConsultoriaDTO>(q).AsQueryable();

            return query;
        }

        // busca para indice numérico dos atos...
        public Pagina<PublicacaoAreaConsultoriaDTO> RodarIndiceNumericoAtos(string anoInformativo, int colecionadorId, int pagina = 1, int itensPorPagina = 3)
        {
            IQueryable<PUBLICACAO_AREAS_CONSULTORIA> query = GetDbSet();

            query = GetDbSet().SqlQuery("select a.*, t.TIP_ATO_DESCRICAO, p.PUB_NUMERO_ATO, p.PUB_DATA_ATO " +
                                        "from PUBLICACAO p " +
                                        "inner join TIPO_ATO t on (t.TIP_ATO_ID = p.TIP_ATO_ID) " +
                                        "inner join PUBLICACAO_AREAS_CONSULTORIA a on (p.PUB_ID = a.PUB_ID and a.ARE_CONS_ID = " + colecionadorId.ToString() + ") " +
                                        "inner join PUBLICACAO_UF u on (u.PUB_ID = a.PUB_ID and u.ARE_CONS_ID = a.ARE_CONS_ID and u.INF_ANO = '" + anoInformativo.ToString() + "') " +
                                        "inner join PUBLICACAO_TITULACAO pt on (pt.PUB_ID = a.PUB_ID and pt.ARE_CONS_ID = a.ARE_CONS_ID and pt.PTI_PRINCIPAL = 1) " +
                                        "order by t.TIP_ATO_DESCRICAO, convert(int,p.PUB_NUMERO_ATO), p.PUB_DATA_ATO").AsQueryable();

            return ToDTOPage(query, pagina, itensPorPagina);
        }

        // busca para indice numérico das alterações e revogações...
        public Pagina<PublicacaoAreaConsultoriaDTO> RodarIndiceNumericoAlteracoesRevogacoes(string anoInformativo, int colecionadorId, int pagina = 1, int itensPorPagina = 3)
        {
            IQueryable<PUBLICACAO_AREAS_CONSULTORIA> query = GetDbSet();

            query = GetDbSet().SqlQuery("select a.*, t.TIP_ATO_DESCRICAO, p.PUB_NUMERO_ATO, p.PUB_DATA_ATO " +
                                        "from PUBLICACAO p " +
                                        "inner join TIPO_ATO t on (t.TIP_ATO_ID = p.TIP_ATO_ID) " +
                                        "inner join PUBLICACAO_AREAS_CONSULTORIA a on (p.PUB_ID = a.PUB_ID and a.ARE_CONS_ID = " + colecionadorId.ToString() + ") " +
                                        "inner join PUBLICACAO_UF u on (u.PUB_ID = a.PUB_ID and u.ARE_CONS_ID = a.ARE_CONS_ID and u.INF_ANO = '" + anoInformativo.ToString() + "') " +
                                        "inner join PUBLICACAO_TITULACAO pt on (pt.PUB_ID = a.PUB_ID and pt.ARE_CONS_ID = a.ARE_CONS_ID and pt.PTI_PRINCIPAL = 1) " +
                                        "inner join PUBLICACAO_ALTERACAO_REVOGACAO r on (r.PUB_ID = p.PUB_ID) " +
                                        "order by t.TIP_ATO_DESCRICAO, convert(int,p.PUB_NUMERO_ATO), p.PUB_DATA_ATO").AsQueryable();

            return ToDTOPage(query, pagina, itensPorPagina);
        }

        // busca para index...
        public Pagina<PublicacaoAreaConsultoriaDTO> PublicacoesAreaConsultoria(string UF = null, string faseMateria = null, DateTime? dtCadastro = null, string coadgedBI = null,
                                                                               int?[] tpMateria = null, int? nrMateria = null, string anoAto = null, int? tpAto = null,
                                                                               string nrAto = null, int? nrInformativo = null, string anoInformativo = null, int? colecionadorId = null,
                                                                               int? colaboradorId = null, int? gg = null, int? vb = null, int? svb = null, DateTime? dtAto = null,
                                                                               int? ativoId = null, int pagina = 1, int itensPorPagina = 3)
        {
            IQueryable<PUBLICACAO_AREAS_CONSULTORIA> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(coadgedBI))
            {
                query = GetDbSet().SqlQuery("select * " +
                                            "from PUBLICACAO p " +
                                            "inner join PUBLICACAO_AREAS_CONSULTORIA a on p.PUB_ID=a.PUB_ID " +
                                            "where " +
                                            "FREETEXT(CABECALHO, 'FORMSOF(THESAURUS," + '"' + coadgedBI.ToString() + '"' + ")') or " +
                                            "FREETEXT(PUB_CONTEUDO, 'FORMSOF(THESAURUS," + '"' + coadgedBI.ToString() + '"' + ")') or " +
                                            "FREETEXT(PUB_CONTEUDO_RESENHA, 'FORMSOF(THESAURUS," + '"' + coadgedBI.ToString() + '"' + ")')").AsQueryable();
            }
            if (!String.IsNullOrWhiteSpace(faseMateria))
            {
                if (faseMateria == "1") // redação...
                {
                    query = query.Where(x => !(from pub in db.PUBLICACAO_REVISAO where pub.ARE_CONS_ID == x.ARE_CONS_ID select pub.PUB_ID).Contains(x.PUB_ID) ||
                                              (from pub in db.PUBLICACAO_REVISAO where pub.ARE_CONS_ID == x.ARE_CONS_ID && (pub.REV_TC == "R") select pub.PUB_ID).Contains(x.PUB_ID));
                }
                if (faseMateria == "2") // revisão técnica...
                {
                    query = query.Where(x => (from pub in db.PUBLICACAO_REVISAO where pub.ARE_CONS_ID == x.ARE_CONS_ID && (pub.REV_TC == "L") select pub.PUB_ID).Contains(x.PUB_ID));
                }
                if (faseMateria == "3") // digitação...
                {
                    query = query.Where(x => (from pub in db.PUBLICACAO_REVISAO where pub.ARE_CONS_ID == x.ARE_CONS_ID && (pub.DIG_TC == "L") select pub.PUB_ID).Contains(x.PUB_ID));
                }
                if (faseMateria == "4") // revisão ortográfica...
                {
                    query = query.Where(x => (from pub in db.PUBLICACAO_REVISAO where pub.ARE_CONS_ID == x.ARE_CONS_ID && (pub.REV_OR == "L") select pub.PUB_ID).Contains(x.PUB_ID));
                }
                if (faseMateria == "5") // diagramação...
                {
                    query = query.Where(x => (from pub in db.PUBLICACAO_REVISAO where pub.ARE_CONS_ID == x.ARE_CONS_ID && (pub.REV_OR == "A") select pub.PUB_ID).Contains(x.PUB_ID));
                }
            }
            if (!String.IsNullOrWhiteSpace(anoAto))
            {
                Int32 busqueAnoAto = 0;
                if (Int32.TryParse(anoAto, out busqueAnoAto))
                {
                    query = query.Where(x => (from pub in db.PUBLICACAO where pub.PUB_DATA_ATO != null && ((DateTime)pub.PUB_DATA_ATO).Year == busqueAnoAto select pub.PUB_ID).Contains(x.PUB_ID));
                }
            }
            if (!String.IsNullOrWhiteSpace(nrAto))
            {
                query = query.Where(x => (from pub in db.PUBLICACAO where pub.PUB_NUMERO_ATO == nrAto select pub.PUB_ID).Contains(x.PUB_ID));
            }
            if (dtCadastro != null)
            {
                query = query.Where(x => (from pub in db.PUBLICACAO
                                          where pub.DATA_CADASTRO != null &&
                     pub.DATA_CADASTRO.Value.Day == dtCadastro.Value.Day &&
                     pub.DATA_CADASTRO.Value.Month == dtCadastro.Value.Month &&
                     pub.DATA_CADASTRO.Value.Year == dtCadastro.Value.Year
                                          select pub.PUB_ID).Contains(x.PUB_ID));
            }
            if (dtAto != null)
            {
                query = query.Where(x => (from pub in db.PUBLICACAO
                                          where pub.PUB_DATA_ATO != null &&
                                                pub.PUB_DATA_ATO.Value.Day == dtAto.Value.Day &&
                                                pub.PUB_DATA_ATO.Value.Month == dtAto.Value.Month &&
                                                pub.PUB_DATA_ATO.Value.Year == dtAto.Value.Year
                                          select pub.PUB_ID).Contains(x.PUB_ID));
            }
            if (nrInformativo != null)
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_NUMERO == nrInformativo select uf.PUB_ID).Contains(x.PUB_ID));
            }
            if (!String.IsNullOrWhiteSpace(anoInformativo))
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.INF_ANO == anoInformativo select uf.PUB_ID).Contains(x.PUB_ID));
            }
            if (!String.IsNullOrWhiteSpace(UF))
            {
                query = query.Where(x => (from uf in db.PUBLICACAO_UF where uf.UF_ID == UF select uf.PUB_ID).Contains(x.PUB_ID));
            }
            if (gg != null)
            {
                query = query.Where(x => (from tit in db.PUBLICACAO_TITULACAO where tit.TIT_ID == gg select tit.PUB_ID).Contains(x.PUB_ID));
            }
            if (vb != null)
            {
                query = query.Where(x => (from tit in db.PUBLICACAO_TITULACAO where tit.TIT_ID_VERBETE == vb select tit.PUB_ID).Contains(x.PUB_ID));
            }
            if (svb != null)
            {
                query = query.Where(x => (from tit in db.PUBLICACAO_TITULACAO where tit.TIT_ID_SUBVERBETE == svb select tit.PUB_ID).Contains(x.PUB_ID));
            }
            if (ativoId != null)
            {
                query = query.Where(x => (from pub in db.PUBLICACAO where pub.PUB_ATIVO == ativoId select pub.PUB_ID).Contains(x.PUB_ID));
            }
            /*if (tpMateria != null)
            {
                query = query.Where(o => o.PUBLICACAO.PUB_ID == o.PUB_ID && tpMateria.Contains(o.PUBLICACAO.TIP_MAT_ID));
            }
            if (tpAto != null)
            {
                query = query.Where(o => o.PUBLICACAO.PUB_ID == o.PUB_ID && o.PUBLICACAO.TIP_ATO_ID == tpAto);
            }*/
            if (nrMateria != null)
            {
                query = query.Where(x => x.PUB_ID == nrMateria);
            }
            if (colecionadorId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == colecionadorId);
            }
            if (colaboradorId != null)
            {
                query = query.Where(x => (from pub in db.PUBLICACAO join col in db.COLABORADOR on pub.USU_LOGIN equals col.USU_LOGIN where col.COL_ID == colaboradorId select pub.PUB_ID).Contains(x.PUB_ID));
            }

            // ordenação por data do cadastro decrescente...
            /*query = query.OrderByDescending(x => x.PUBLICACAO.DATA_CADASTRO);*/

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}

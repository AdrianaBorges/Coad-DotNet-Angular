using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class TitulacaoDAO : AbstractGenericDao<TITULACAO, TitulacaoDTO, int>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 

        public TitulacaoDAO() : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);
        }

        public Pagina<TitulacaoDTO> Gg(int? colecionadorId = null, string[] ufs = null)
        {
            IQueryable<TITULACAO> query = GetDbSet();
            if (colecionadorId != null)
            {
                query = GetDbSet().SqlQuery("select TIT_ID, TIT_DESCRICAO=case when UF_ID is NOT NULL then TIT_DESCRICAO+' - '+UF_ID else TIT_DESCRICAO end, " +
                                            "TIT_ATIVO, TIT_TIPO, TIT_ID_REFERENCIA, ARE_CONS_ID, UF_ID " +
                                            "from TITULACAO " +
                                            "where ARE_CONS_ID=" + colecionadorId.ToString() +
                                            " and TIT_TIPO='G'").AsQueryable();
                if (ufs != null)
                {
                    query = query.Where(x => ufs.Contains(x.UF_ID));
                }
            }
            return ToDTOPage(query, 1, 999999);
        }

        public Pagina<TitulacaoDTO> Verbetes(int? ggId = null, bool trazSimples = true)
        {
            IQueryable<TITULACAO> query = GetDbSet();
            if (ggId != null)
            {
                query = query.Where(x => x.TIT_ID_REFERENCIA == ggId && x.TIT_TIPO == "V");
            }
            return ToDTOPage(query, 1, 999999);
        }

        public Pagina<TitulacaoDTO> Subverbetes(int? vbId = null)
        {
            IQueryable<TITULACAO> query = GetDbSet();
            if (vbId != null)
            {
                query = query.Where(x => x.TIT_ID_REFERENCIA == vbId && x.TIT_TIPO == "S");
            }
            return ToDTOPage(query, 1, 999999);
        }

        public Pagina<TitulacaoDTO> TitulacoesInferiores(string tipo)
        {
            IQueryable<TITULACAO> query = GetDbSet();

            if (tipo == "G")
            {
                query = query.Where(x => x.TIT_TIPO == "V");
            }
            else if (tipo == "V") {
                query = query.Where(x => x.TIT_TIPO == "S");
            }
            
            return ToDTOPage(query, 1, 10);
        }

        public Pagina<TitulacaoDTO> FiltrarTitulacoes(int? areaId, int? ggId, int? vbId, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<TITULACAO> query = GetDbSet();

            if (areaId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == areaId);
            }
            if (vbId != null)
            {
                query = query.Where(x => x.TIT_ID_REFERENCIA == vbId && x.TIT_TIPO == "S");
            } 
            else if (ggId != null)
            {
                query = query.Where(x => x.TIT_ID_REFERENCIA == ggId && x.TIT_TIPO == "V");
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }

        public Pagina<TitulacaoDTO> Titulacoes(int? titulacaoId = null, int? areaId = null, string descricao = null, int? ativo = 1, string tipo = null, int? superiorId = null, string uf = null, bool nomeExato = false, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<TITULACAO> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(uf))
            {
                query = query.Where(x => x.UF_ID.Contains(uf));
            }

            if (!String.IsNullOrWhiteSpace(descricao))
            {
                descricao = descricao.ToString();
                if (nomeExato)
                    query = query.Where(x => x.TIT_DESCRICAO.ToUpper() == descricao.ToUpper());
                else
                    query = query.Where(x => x.TIT_DESCRICAO.Contains(descricao));
            }

            if (tipo != null)
            {
                tipo = tipo.ToString();
                query = query.Where(x => x.TIT_TIPO.Contains(tipo));
            }

            if (superiorId != null)
            {
                query = query.Where(x => x.TIT_ID_REFERENCIA == superiorId);
            }

            if (areaId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == areaId);
            }

            query = query.Where(x => x.TIT_ATIVO == ativo);

            return ToDTOPage(query, pagina, itensPorPagina);
        }

        public IList<TitulacaoDTO> ListarGrandeGrupo(int? colecionadorId = null, string nome = null)
        {
            IQueryable<TITULACAO> query = GetDbSet();
            
            if (colecionadorId != null)
            {
                query = query.Where(x => x.ARE_CONS_ID == colecionadorId);
            }

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.TIT_DESCRICAO.Contains(nome));
            }

            query = query.Where(x => x.TIT_TIPO == "G");
            return ToDTO(query);
        }
    }
}

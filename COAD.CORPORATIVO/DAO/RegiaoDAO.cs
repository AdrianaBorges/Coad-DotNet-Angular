using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;


namespace COAD.CORPORATIVO.DAO
{
    public class RegiaoDAO : AbstractGenericDao<REGIAO, RegiaoDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RegiaoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public IList<RegiaoDTO> FindAllByUen(int REP_ID_PARA_EXCLUIR, int? uenId = 1)
        {
            
            var rg = GetDbSet().Where(x => 
                
                    (from rep in db.REPRESENTANTE where rep.REP_ID == REP_ID_PARA_EXCLUIR select rep.RG_ID).FirstOrDefault() != x.RG_ID &&
                    x.UEN_ID == uenId
                );

            return ToDTO(rg);
        }

        public IList<RegiaoDTO> FindAllByUen(int? uenId = 1)
        {

            var rg = GetDbSet().Where(x => x.UEN_ID == uenId);
            return ToDTO(rg);
        }

        public IQueryable<REGIAO> TemplateRegioesDoCliente(int? CLI_ID, int? uenId = 1)
        {
            var query = (from car_rep in db.CARTEIRA_REPRESENTANTE
                         join car_cli in db.CARTEIRA_CLIENTE
                            on car_rep.CAR_ID equals car_cli.CAR_ID
                         where car_rep.REPRESENTANTE.REP_ATIVO == 1
                         && car_cli.CARTEIRA.DATA_EXCLUSAO == null
                         && (uenId == null || (car_rep.REPRESENTANTE.UEN_ID == uenId && car_rep.CARTEIRA.UEN_ID == uenId)
                         && car_cli.CLI_ID == CLI_ID)
                         select car_rep.REPRESENTANTE.REGIAO).Distinct().OrderBy(or => or.RG_DESCRICAO);
            return query;
        }

        /// <summary>
        /// Traz todas das regiões onde a operadora está encarteirada
        /// </summary>
        /// <param name="CLI_ID"></param>
        /// <param name="uenId"></param>
        /// <returns></returns>
        public IList<RegiaoDTO> FindRegioesDoCliente(int? CLI_ID, int? uenId = 1)
        {
            var query = TemplateRegioesDoCliente(CLI_ID, uenId);
            return ToDTO(query);
        }

        /// <summary>
        /// Traz todas das regiões onde a operadora está encarteirada
        /// </summary>
        /// <param name="CLI_ID"></param>
        /// <param name="uenId"></param>
        /// <returns></returns>
        public bool ClientePossuiRegiao(int? CLI_ID, int? RG_ID, int? uenId = 1)
        {
            var query = TemplateRegioesDoCliente(CLI_ID, uenId);
            var count = query.Where(x => x.RG_ID == RG_ID).Count();

            return (count > 0);
        }

        public IList<RegiaoDTO> ListarRegioes(int? UEN_ID = 1)
        {
            var lstRegioes = GetDbSet().Where(x => x.UEN_ID == UEN_ID);
            return ToDTO(lstRegioes);
        }

        public IList<RegiaoDTO> ListarRegioesCombo(int? uenId = null)
        {
            var query = (from rg in db.REGIAO
                         where 1 == 1 &&
                            (uenId == null || rg.UEN_ID == uenId)
                         select rg);
            return ToDTO(query);
        }
        public int? ObterRgIdDoRepresentante(int? REP_ID)
        {
            var query = (from rep in db.REPRESENTANTE where rep.REP_ID == REP_ID select rep.RG_ID).FirstOrDefault();
            return query;
        }

        public Pagina<RegiaoDTO> ListarRegiao(string descricao, int? empId, string uf, int? munId, IList<int> lstIds = null, int pagina = 1, int registrosPorPagina = 8, int? uenId = null)
        {
            if (string.IsNullOrWhiteSpace(uf))
            {
                uf = null;
            }
            var query = (from rg in db.REGIAO 
                        where
                            (empId == null || rg.EMP_ID == empId) &&
                            (uf == null || (from objUf in db.UF where objUf.UF_SIGLA == uf select objUf.RG_ID).Contains(rg.RG_ID)) &&
                            (munId == null || (from objMun in db.MUNICIPIO where objMun.MUN_ID == munId select objMun.RG_ID).Contains(rg.RG_ID)) &
                            (uenId == null || rg.UEN_ID == uenId)
                         select rg);

            if (!string.IsNullOrWhiteSpace(descricao))
            {
                query = query.Where(x => x.RG_DESCRICAO.Contains(descricao));
            }

            if (lstIds != null)
            {
                query = query.Where(x => lstIds.Contains((int) x.EMP_ID));
            }

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public RegiaoDTO EncontrarRegiaoPorNome(string descricaoRg, bool usarLike = false)
        {
            var query = (from rg in db.REGIAO 
                        where                         
                        (usarLike == false && rg.RG_DESCRICAO.ToUpper() == descricaoRg.ToUpper()) ||
                        (usarLike == true && rg.RG_DESCRICAO.Contains(descricaoRg))
                        select rg);

            return ToDTO(query.FirstOrDefault());
        }

        public IList<RegiaoDTO> ListarRegioesPorNome(string nome, int? uenId)
        {
            var query = (from rg in db.REGIAO 
                         where rg.RG_DESCRICAO.Contains(nome) &&
                         (from pro in db.PROPOSTA where pro.CLI_ID > 20
                              select pro.RG_ID).Contains(rg.RG_ID) && 
                         rg.UEN_ID == uenId
                             orderby rg.RG_DESCRICAO ascending
                             select rg);

            return ToDTO(query);
        }

        public RegiaoDTO PesquisarRegiao(string cidade, string uf)
        {
            var query = (from 
                            rg in db.REGIAO join
                            ufObj in db.UF on rg.RG_ID equals ufObj.RG_ID join 
                            mun in db.MUNICIPIO on ufObj.UF_SIGLA equals mun.UF
                        where 
                            //mun.MUN_DESCRICAO.Contains(cidade) &&
                            mun.UF == uf &&
                            ufObj.UF_VALIDA == true
                        select rg);
            var  regiao = query.FirstOrDefault();
            

            return ToDTO(regiao);
        }

    }
}

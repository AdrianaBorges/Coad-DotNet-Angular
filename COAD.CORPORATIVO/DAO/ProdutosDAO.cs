using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Extensions;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto.Custons;

namespace COAD.CORPORATIVO.DAO
{
    public class ProdutosDAO : DAOAdapter<PRODUTOS, ProdutosDTO>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ProdutosDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public string ObterUltimaRemessaEnviada()
        {
            return db.ASSINATURA.Where(x => !(x.ASN_ANO_REMESSA == null || x.ASN_ANO_REMESSA.Trim() == string.Empty || x.ASN_ANO_REMESSA == "") &&
                                            !(x.ASN_REMESSA == null || x.ASN_REMESSA.Trim() == string.Empty || x.ASN_REMESSA == "")).
                                            Max(x => x.ASN_ANO_REMESSA + x.ASN_REMESSA);
        }

        public IList<ProdutosDTO> ObterProdutosInformativoSemanal(bool MDP, int? tipoEnvio = null)
        {
            var query = new List<PRODUTOS>();

            if (tipoEnvio == 2)
                query = (from p in db.PRODUTOS
                        where (p.PRO_TIPO_REMESSA == 1      && 
                               p.PRO_CODIGO_CORREIO != null && 
                                    p.PRO_REMESSA_SEMANAL == true) || (MDP ? p.PRO_ID == 16 : p.PRO_ID != p.PRO_ID)
                        select p).ToList();
            else
                query = (from p in db.PRODUTOS
                        where (p.PRO_TIPO_REMESSA == 1)
                       select p).ToList();


            return ToDTO(query);

        }
       

        public List<Produto> BuscarPorNCM(string _ncm_id)
        {
            var _produto = (from d in db.PRODUTOS
                            where d.NCM_ID == _ncm_id && d.DATA_EXCLUSAO == null
                            select new Produto
                            {
                                PRO_ID = d.PRO_ID,
                                PRO_SIGLA = d.PRO_SIGLA,
                                PRO_NOME = d.PRO_NOME,
                                PRO_ID_DERVADO = d.PRO_ID_DERVADO,
                                PRO_MOD_CARTA_URA = d.PRO_MOD_CARTA_URA,
                                PRO_TIPO_REMESSA = d.PRO_TIPO_REMESSA,
                                PRO_RECEBE_MALA = d.PRO_RECEBE_MALA,
                                PRO_RECEBE_PASTA_SN = d.PRO_RECEBE_PASTA_SN,
                                PRO_PRODUTO_ACABADO = d.PRO_PRODUTO_ACABADO,
                                PRO_STATUS = d.PRO_STATUS,
                                PRO_EMITE_NF = d.PRO_EMITE_NF,
                                NCM_ID = d.NCM_ID,
                                GRUPO_ID = d.GRUPO_ID,
                                DATA_CADASTRO = d.DATA_CADASTRO,
                                DATA_ALTERA = d.DATA_ALTERA,
                                PRO_UN_COMPRA = d.PRO_UN_COMPRA,
                                PRO_UN_VEND = d.PRO_UN_VEND,
                                PRO_PRECO_COMPRA = d.PRO_PRECO_COMPRA,
                                PRO_PRECO_CUSTO = d.PRO_PRECO_CUSTO,
                                AREA_ID = d.AREA_ID

                            }).ToList();

            if (_produto == null)
            {
                _produto = new List<Produto>();
            }

            return _produto;

        }
        public ProdutosDTO BuscarPorNCMDescricao(string _ncm_id, string _descricao)
        {
            var _produto = (from d in db.PRODUTOS
                                where (d.NCM_ID == _ncm_id) &&
                                      (d.PRO_NOME == _descricao) &&
                                      (d.DATA_EXCLUSAO == null)
                               select d).FirstOrDefault();           

            return ToDTO(_produto);

        }
        public List<Produto> BuscarPorDescricao(string _pro_nome)
        {
            List<Produto> _produto = (from d in db.PRODUTOS
                                      where (d.PRO_NOME.StartsWith(_pro_nome)) && (d.DATA_EXCLUSAO == null)
                                       select new Produto
                                       {
                                           PRO_ID = d.PRO_ID,
                                           PRO_SIGLA = d.PRO_SIGLA,
                                           PRO_NOME = d.PRO_NOME,
                                           PRO_ID_DERVADO = d.PRO_ID_DERVADO,
                                           PRO_MOD_CARTA_URA = d.PRO_MOD_CARTA_URA,
                                           PRO_TIPO_REMESSA = d.PRO_TIPO_REMESSA,
                                           PRO_RECEBE_MALA = d.PRO_RECEBE_MALA,
                                           PRO_RECEBE_PASTA_SN = d.PRO_RECEBE_PASTA_SN,
                                           PRO_PRODUTO_ACABADO = d.PRO_PRODUTO_ACABADO,
                                           PRO_STATUS = d.PRO_STATUS,
                                           PRO_EMITE_NF = d.PRO_EMITE_NF,
                                           NCM_ID = d.NCM_ID,
                                           GRUPO_ID = d.GRUPO_ID,
                                           DATA_CADASTRO = d.DATA_CADASTRO,
                                           DATA_ALTERA = d.DATA_ALTERA,
                                           PRO_UN_COMPRA = d.PRO_UN_COMPRA,
                                           PRO_UN_VEND = d.PRO_UN_VEND,
                                           PRO_PRECO_COMPRA = d.PRO_PRECO_COMPRA,
                                           PRO_PRECO_CUSTO = d.PRO_PRECO_CUSTO,
                                           AREA_ID = d.AREA_ID
                                       }).ToList();


            return _produto;

        }
        public Produto BuscarPorID(int _pro_id)
        {
            Produto _produto = (from d in db.PRODUTOS
                                      where (d.PRO_ID == _pro_id) && (d.DATA_EXCLUSAO == null)
                                      select new Produto
                                      {
                                          PRO_ID = d.PRO_ID,
                                          PRO_SIGLA = d.PRO_SIGLA,
                                          PRO_NOME = d.PRO_NOME,
                                          PRO_ID_DERVADO = d.PRO_ID_DERVADO,
                                          PRO_MOD_CARTA_URA = d.PRO_MOD_CARTA_URA,
                                          PRO_TIPO_REMESSA = d.PRO_TIPO_REMESSA,
                                          PRO_RECEBE_MALA = d.PRO_RECEBE_MALA,
                                          PRO_RECEBE_PASTA_SN = d.PRO_RECEBE_PASTA_SN,
                                          PRO_PRODUTO_ACABADO = d.PRO_PRODUTO_ACABADO,
                                          PRO_STATUS = d.PRO_STATUS,
                                          PRO_EMITE_NF = d.PRO_EMITE_NF,
                                          NCM_ID = d.NCM_ID,
                                          GRUPO_ID = d.GRUPO_ID,
                                          DATA_CADASTRO = d.DATA_CADASTRO,
                                          DATA_ALTERA = d.DATA_ALTERA,
                                          PRO_UN_COMPRA = d.PRO_UN_COMPRA,
                                          PRO_UN_VEND = d.PRO_UN_VEND,
                                          PRO_PRECO_COMPRA = d.PRO_PRECO_COMPRA,
                                          PRO_PRECO_CUSTO = d.PRO_PRECO_CUSTO,
                                          AREA_ID = d.AREA_ID
                                      }).FirstOrDefault();


            return _produto;

        }
        public virtual void InsertSemIdentity(int? _pro_id,string _pro_sigla, string _pro_nome, string _ncm_id) 
        {
            try
            {
                db.INSERT_PRODUTO_INDIVIDUAL(_pro_id, _pro_sigla, _pro_nome, _ncm_id);
            }
            catch (Exception ex)
            {
                
                throw new Exception(SysException.Show(ex));
            }
        }

        /// <summary>
        /// Retorna todos os produtos com data exclusão diferente de null, descrição diferente de null e respeitando o tipo de produto informado.
        /// </summary>
        /// <returns></returns>
        public IList<ProdutosDTO> BuscarPorTipoProduto(int _tipo_pro)
        {
            IQueryable<PRODUTOS> query = GetDbSet();
            query = query.Where(x => x.DATA_EXCLUSAO == null);
            query = query.Where(x => x.TIPO_PRO == _tipo_pro && x.PRO_NOME != null);

            return ToDTO(query);
        }

        /// <summary>
        /// Busca todos os produtos não marcados como excluido, possui o tipo do produto, tipo de comportamento e uma descrição
        /// </summary>
        /// <returns></returns>
        public IList<ProdutosDTO> FindAllValid(
            bool? prodVenda = null,
            bool? prodUra = null,
            bool? prodPortal = null,
            bool? prodPortalST = null)
        {
            IQueryable<PRODUTOS> query = GetDbSet();


            //if (prodVenda != null)
            //{
            //    query = query.Where(x => x.PRO_VENDA);
            //}
            if (prodUra != null)
            {
                query = query.Where(x => x.PRO_URA);
            }
            if (prodPortal != null)
            {
                query = query.Where(x => x.PRO_PORTAL);
            }
            if (prodPortalST != null)
            {
                query = query.Where(x => x.PRO_PORTAL_ST);
            }

            query = query.Where(x => x.DATA_EXCLUSAO == null);
            //query = query.Where(x => x.TIPO_PRO != null && x.TIPO_PROD_COMPORTAMENTO != null);

            return ToDTO(query);
        }

        /// <summary>
        /// Busca todos os produtos exceto os que possuiem o ID iqual a um dos passado pela lista de lstParams
        /// </summary>
        /// <param name="lstId"></param>
        /// <returns></returns>
        public IList<ProdutosDTO> FindAllExcept(ICollection<int> lstId)
        {

            IQueryable<PRODUTOS> prod = GetDbSet();

            if (lstId != null && lstId.Count() > 0)
            {
                prod = prod.Where(p => lstId.Contains(p.PRO_ID));
            }

            prod = prod.Where(p => p.DATA_EXCLUSAO != null);
            return ToDTO(prod);
        }

        public IList<ProdutosDTO> FindAllExcept(int? lstId)
        {

            if (lstId != null)
            {
                return FindAllExcept(new List<int> { (int)lstId });
            }

            return FindAllConverted();
        }


        public Pagina<ProdutosDTO> Produtos(Boolean? ativos, 
            int? grupoId, 
            int? tipoProdutoId, 
            int? areaId, 
            string sigla = null,
            string nome = null, 
            int pagina = 1,
            int itensPorPagina = 10,
            bool? prodVenda = null,
            bool? prodUra = null,
            bool? prodPortal = null,
            bool? prodPortalST = null)
        {

            IQueryable<PRODUTOS> query = GetDbSet();

            if(sigla != null){

                sigla = sigla.ToString();
                query = query.Where(x => x.PRO_SIGLA.Contains(sigla));
            }            
            if(nome != null){

                nome = nome.ToString();
                query = query.Where(x => x.PRO_NOME.Contains(nome));
            }
            if (grupoId != null)
            {
                query = query.Where(x => x.GRUPO_ID == grupoId);
            }
            if (tipoProdutoId != null)
            {
                query = query.Where(x => x.TIPO_PRO == tipoProdutoId);
            }
            if (areaId != null)
            {
                query = query.Where(x => x.AREA_ID == areaId);
            }
            if (prodVenda != null)
            {
                query = query.Where(x => x.PRO_VENDA);
            }
            if (prodUra != null)
            {
                query = query.Where(x => x.PRO_URA);
            }
            if (prodPortal != null)
            {
                query = query.Where(x => x.PRO_PORTAL);
            }
            if (prodPortalST != null)
            {
                query = query.Where(x => x.PRO_PORTAL_ST);
            }

            if (ativos != null)
                if ((Boolean)ativos == true)
                    query = query.Where(p => p.DATA_EXCLUSAO == null);
                else
                    query = query.Where(p => p.DATA_EXCLUSAO != null);
             

            return ToDTOPage(query, pagina, itensPorPagina);
        }


        /// <summary>
        /// Traz uma lista baseado no nome do produto informado.
        /// Usado geralmente para autocompletes.
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public IList<ProdutosDTO> ListarPorNome(string nome)
        {
            IQueryable<PRODUTOS> query = GetDbSet();
            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.PRO_NOME.Contains(nome));
            }
            
            return ToDTO(query);
        }

        /// <summary>
        /// Traz uma lista produtos anexos tipo (livros, apostilas, pastas) baseado no nome do produto informado.
        /// Usado geralmente para autocompletes.
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public IList<ProdutosDTO> ListarProdutosAnexosPorNome(string nome)
        {
            IList<int> lstIds = new List<int>(){ // livros
            
                3, 
                4,
                5
            };

            IQueryable<PRODUTOS> query = GetDbSet().Where(x => lstIds.Contains(x.GRUPO_ID));


            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.PRO_NOME.Contains(nome));
            }

            return ToDTO(query);
        }

        public bool ChecaProdutoEhCurso(int? PRO_ID)
        {
            var query = (from pro in db.PRODUTOS 
                         where pro.GRUPO_ID == 2 &&
                         pro.PRO_ID == PRO_ID
                         select pro);

            return (query.Count() > 0);
        }

        public bool ChecarGeraSenha(int? PRO_ID)
        {
            var query = (from pro in db.PRODUTOS
                         where pro.GRUPO_ID == 1 &&
                         pro.PRO_ID == PRO_ID
                         select pro);

            return (query.Count() > 0);
        }

        public ProdutoPerfilDTO ListarPerfil(int pro_id)
        {
            var query = (from p in db.PRODUTO_PERFIL
                         where p.PRO_ID == pro_id
                         select new ProdutoPerfilDTO
                         {
                             PER_ID = p.PER_ID
                            ,
                             PPR_QTDE_LOGIN = p.PPR_QTDE_LOGIN
                            ,
                             PRO_ID = p.PRO_ID

                         }).FirstOrDefault();

            return query;
        }
        public IList<ProdutoPerfilDTO> ListarPerfis(int pro_id)
        {
            var query = (from p in db.PRODUTO_PERFIL
                         where p.PRO_ID == pro_id
                         select new ProdutoPerfilDTO
                         {
                             PER_ID = p.PER_ID
                            ,
                             PPR_QTDE_LOGIN = p.PPR_QTDE_LOGIN
                            ,
                             PRO_ID = p.PRO_ID

                         }).ToList();

            return query;
        }
        public IList<ProdutoPerfilDTO> ListarPerfis()
        {
            var query = (from p in db.PRODUTO_PERFIL
                         select new ProdutoPerfilDTO
                         {
                             PER_ID = p.PER_ID
                            ,
                             PPR_QTDE_LOGIN = p.PPR_QTDE_LOGIN
                            ,
                             PRO_ID = p.PRO_ID

                         }).ToList();

            return query;
        }

        public IList<ProdutosDTO> ListarProdutosVenda()
        {

            var query = (from p in db.PRODUTOS
                         where p.PRO_VENDA == true 
                         select p).ToList();

            return ToDTO(query);
        }

        /// <summary>
        /// Traz uma lista baseado no nome do produto informado.
        /// Usado geralmente para autocompletes.
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public IList<AutoCompleteDTO<int>> ListarPorNomeAutocomplete(string nome)
        {           
            var query = (from x in db.PRODUTOS
                        where x.PRO_NOME.Contains(nome)
                        select new AutoCompleteDTO<int>()
                        {
                            label = x.PRO_NOME,
                            value = x.PRO_ID
                        });           

            return query.ToList();
        }

    }
}

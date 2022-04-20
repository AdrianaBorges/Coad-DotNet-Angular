using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Model.Dto.Custons;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PRO_ID")]
    public class ProdutosSRV : ServiceAdapter<PRODUTOS, ProdutosDTO>
    {
        public ProdutosDAO _dao { get; set; }

        public ProdutosSRV()
        {
            _dao = new ProdutosDAO();
            SetDao(_dao);
        }

        public ProdutosSRV(ProdutosDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public IList<ProdutosDTO> ObterProdutosInformativoSemanal(bool MDP, int? tipoEnvio = null)
        {
            return _dao.ObterProdutosInformativoSemanal(MDP, tipoEnvio);
        }
        ///
        public ProdutosDTO VerficarIncluir(ProdutosDTO _pro, int _tiponf)
        {
            var _produto = new ProdutosDTO();

            if (_pro.PRO_ID > 0)
                _produto = this.FindById(_pro.PRO_ID);
            else
                _produto = this.BuscarPorNCMDescricao(_pro.NCM_ID, _pro.PRO_NOME);

            if (_produto == null)
            {
                if (_tiponf == 1)
                    this.InsertSemIdentity(_pro.PRO_ID, _pro.PRO_SIGLA, _pro.PRO_NOME, _pro.NCM_ID);
                else
                    this.Save(_pro);
            }
            else
            {
                _pro.PRO_ID = _produto.PRO_ID;
                _pro.PRO_SIGLA = _produto.PRO_SIGLA;
                _pro.PRO_NOME = _produto.PRO_NOME;
                _pro.PRO_ID_DERVADO = _produto.PRO_ID_DERVADO;
                _pro.PRO_MOD_CARTA_URA = _produto.PRO_MOD_CARTA_URA;
                _pro.PRO_TIPO_REMESSA = _produto.PRO_TIPO_REMESSA;
                _pro.PRO_RECEBE_MALA = _produto.PRO_RECEBE_MALA;
                _pro.PRO_RECEBE_PASTA_SN = _produto.PRO_RECEBE_PASTA_SN;
                _pro.PRO_PRODUTO_ACABADO = _produto.PRO_PRODUTO_ACABADO;
                _pro.PRO_STATUS = _produto.PRO_STATUS;
                _pro.PRO_EMITE_NF = _produto.PRO_EMITE_NF;
                _pro.NCM_ID = _produto.NCM_ID;
                _pro.GRUPO_ID = _produto.GRUPO_ID;
                _pro.DATA_CADASTRO = _produto.DATA_CADASTRO;
                _pro.DATA_ALTERA = _produto.DATA_ALTERA;
                _pro.PRO_UN_COMPRA = _produto.PRO_UN_COMPRA;
                _pro.PRO_UN_VEND = _produto.PRO_UN_VEND;
                _pro.PRO_PRECO_COMPRA = _produto.PRO_PRECO_COMPRA;
                _pro.PRO_PRECO_CUSTO = _produto.PRO_PRECO_CUSTO;
                _pro.AREA_ID = _produto.AREA_ID;
                _pro.TIPO_PRO = _produto.TIPO_PRO;
            }

            return _pro;
        }
        public List<Produto> BuscarPorNCM(string _ncm_id)
        {
            return new ProdutosDAO().BuscarPorNCM(_ncm_id);
        }
        public List<Produto> BuscarPorDescricao(string _pro_nome)
        {
            return new ProdutosDAO().BuscarPorDescricao(_pro_nome);
        }
        public Produto BuscarPorID(int _pro_id)
        {
            return new ProdutosDAO().BuscarPorID(_pro_id);
        }

        /// <summary>
        /// Busca todos os produtos exceto os que possuiem o ID iqual a um dos passado pela lista de lstParams
        /// </summary>
        /// <param name="lstId"></param>
        /// <returns></returns>
        public IList<ProdutosDTO> FindAllExcept(ICollection<int> lstId)
        {
            return _dao.FindAllExcept(lstId);
        }
        public ProdutosDTO BuscarPorNCMDescricao(string _ncm_id, string _descricao)
        {
            return _dao.BuscarPorNCMDescricao(_ncm_id, _descricao);
        }

            /// <summary>
            /// Retorna todos os produtos com data exclusão diferente de null, descrição diferente de null e respeitando o tipo de produto informado.
            /// </summary>
            /// <returns></returns>
            public IList<ProdutosDTO> BuscarPorTipoProduto(int _tipo_pro)
        {
            return _dao.BuscarPorTipoProduto(_tipo_pro);
        }

        public IList<ProdutosDTO> FindAllExcept(int? id)
        {
            return _dao.FindAllExcept(id);
        }

        /// <summary>
        /// Inclui um novo produto se ele não contiver um ID,
        /// Se ele possuir um ID ele será alterado
        /// </summary>
        public void SalvarProduto(ProdutosDTO produto) 
        {
            try
            {

                if (produto.PRO_ID != null)
                {
                    produto.DATA_ALTERA = DateTime.Now;
                    Merge(produto);
                }
                else
                {
                    produto.DATA_CADASTRO = DateTime.Now;
                    Save(produto);
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

        public Pagina<ProdutosDTO> Produtos(Boolean? ativo, int? grupoId, int? tipoProdutoId, int? areaId, string sigla = null, string nome = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Produtos(ativo, grupoId, tipoProdutoId, areaId, sigla, nome, pagina, itensPorPagina);
           return resp;
        }


        public void DeletarProduto(int produtoId)
        {
            var produto = this.FindById(produtoId);
            produto.DATA_EXCLUSAO = DateTime.Now;
            Merge(produto);
        }

        /// <summary>
        /// Pega todos os produtos não marcados como excluido
        /// </summary>
        /// <returns></returns>
        public IList<ProdutosDTO> FindAllValid(
            bool? prodVenda = null,
            bool? prodUra = null,
            bool? prodPortal = null,
            bool? prodPortalST = null)
        {
            return _dao.FindAllValid(prodVenda, prodUra, prodPortal, prodPortalST);
        }
        public virtual void InsertSemIdentity(int? _pro_id, string _pro_sigla, string _pro_nome, string _ncm_id)
        {
            _dao.InsertSemIdentity(_pro_id, _pro_sigla, _pro_nome, _ncm_id);
        }

        /// <summary>
        /// Pega todos os produtos de venda
        /// </summary>
        /// <param name="ativos"></param>
        /// <param name="grupoId"></param>
        /// <param name="tipoProdutoId"></param>
        /// <param name="areaId"></param>
        /// <param name="sigla"></param>
        /// <param name="nome"></param>
        /// <param name="pagina"></param>
        /// <param name="itensPorPagina"></param>
        /// <returns></returns>
        public Pagina<ProdutosDTO> ProdutosDeVenda(Boolean? ativos,
           int? grupoId,
           int? tipoProdutoId,
           int? areaId,
           string sigla = null,
           string nome = null,
           int pagina = 1,
           int itensPorPagina = 10)
        {
            return  _dao.Produtos(ativos, grupoId, tipoProdutoId, areaId, sigla, nome, pagina, itensPorPagina, true);
        }

        /// <summary>
        /// Pega todos os produtos da ura
        /// </summary>
        /// <param name="ativos"></param>
        /// <param name="grupoId"></param>
        /// <param name="tipoProdutoId"></param>
        /// <param name="areaId"></param>
        /// <param name="sigla"></param>
        /// <param name="nome"></param>
        /// <param name="pagina"></param>
        /// <param name="itensPorPagina"></param>
        /// <returns></returns>
        public Pagina<ProdutosDTO> ProdutosDaUra(Boolean? ativos,
            int? grupoId,
            int? tipoProdutoId,
            int? areaId,
            string sigla = null,
            string nome = null,
            int pagina = 1,
            int itensPorPagina = 10)
        {
            return _dao.Produtos(ativos, grupoId, tipoProdutoId, areaId, sigla, nome, pagina, itensPorPagina, null, true);
        }

        /// <summary>
        /// Pega todos os produtos do portal
        /// </summary>
        /// <param name="ativos"></param>
        /// <param name="grupoId"></param>
        /// <param name="tipoProdutoId"></param>
        /// <param name="areaId"></param>
        /// <param name="sigla"></param>
        /// <param name="nome"></param>
        /// <param name="pagina"></param>
        /// <param name="itensPorPagina"></param>
        /// <returns></returns>
        public Pagina<ProdutosDTO> ProdutosDoPortal(Boolean? ativos,
            int? grupoId,
            int? tipoProdutoId,
            int? areaId,
            string sigla = null,
            string nome = null,
            int pagina = 1,
            int itensPorPagina = 10)
        {
            return _dao.Produtos(ativos, grupoId, tipoProdutoId, areaId, sigla, nome, pagina, itensPorPagina, null, null, true);
        }

        /// <summary>
        /// Pega todos os produtos do portal ST
        /// </summary>
        /// <param name="ativos"></param>
        /// <param name="grupoId"></param>
        /// <param name="tipoProdutoId"></param>
        /// <param name="areaId"></param>
        /// <param name="sigla"></param>
        /// <param name="nome"></param>
        /// <param name="pagina"></param>
        /// <param name="itensPorPagina"></param>
        /// <returns></returns>
        public Pagina<ProdutosDTO> ProdutosDoPortalST(Boolean? ativos,
            int? grupoId,
            int? tipoProdutoId,
            int? areaId,
            string sigla = null,
            string nome = null,
            int pagina = 1,
            int itensPorPagina = 10)
        {
            return _dao.Produtos(ativos, grupoId, tipoProdutoId, areaId, sigla, nome, pagina, itensPorPagina, null, null, null, true);
        }

        /// <summary>
        /// Traz uma lista baseado no nome do produto informado.
        /// Usado geralmente para autocompletes.
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public IList<ProdutosDTO> ListarPorNome(string nome)
        {
            return _dao.ListarPorNome(nome);
        }

        
        /// <summary>
        /// Traz uma lista produtos anexos tipo (livros, apostilas, pastas) baseado no nome do produto informado.
        /// Usado geralmente para autocompletes.
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public IList<ProdutosDTO> ListarProdutosAnexosPorNome(string nome)
        {
            return _dao.ListarProdutosAnexosPorNome(nome);
        }

        /// <summary>
        /// Checa se o produto é um curso
        /// </summary>
        /// <param name="PRO_ID"></param>
        /// <returns></returns>
        public bool ChecaProdutoEhCurso(int? PRO_ID)
        {
            return _dao.ChecaProdutoEhCurso(PRO_ID);
        }

        public bool ChecarProdutoPodeGeraSenha(int? PRO_ID)
        {
            var ehCurso = ChecaProdutoEhCurso(PRO_ID);
            var geraAssinatura = ChecarGeraSenha(PRO_ID);
            return (!ehCurso && geraAssinatura);
        }

        public bool ChecarGeraSenha(int? PRO_ID)
        {
            return _dao.ChecarGeraSenha(PRO_ID);
        }

        public ProdutoPerfilDTO ListarPerfil(int pro_id)
        {
            return _dao.ListarPerfil(pro_id);
        }
        public IList<ProdutoPerfilDTO> ListarPerfis(int pro_id)
        {

            return _dao.ListarPerfis(pro_id);
        }
        public IList<ProdutoPerfilDTO> ListarPerfis()
        {
            return _dao.ListarPerfis();
        }

        public IList<ProdutosDTO> ListarProdutosVenda()
        {
            return _dao.ListarProdutosVenda();
        }

        public IList<AutoCompleteDTO<int>> ListarPorNomeAutocomplete(string nome)
        {
            return _dao.ListarPorNomeAutocomplete(nome);
        }

    }
}

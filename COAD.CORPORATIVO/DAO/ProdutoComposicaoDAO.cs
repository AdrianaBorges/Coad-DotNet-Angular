using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.Reflection;
using COAD.CORPORATIVO.Model.Dto.Custons;

namespace COAD.CORPORATIVO.DAO
{
    public class ProdutoComposicaoDAO : AbstractGenericDao<PRODUTO_COMPOSICAO, ProdutoComposicaoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ProdutoComposicaoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }
        public IList<ProdutoTabelaPrecoDTO> FindByProdOrigem(int _cmp_origem,int _ttp_id = 1, int _tpg_id = 9)
        {
            // _ttp_id => 1 Mensal Recorrente
            // _ttp_id => 6 Anual

            var query = (from tp in db.TABELA_PRECO
                         join rt in db.REGIAO_TABELA_PRECO on tp.TP_ID equals rt.TP_ID
                         join tt in db.TABELA_PRECO_TIPO_PAGAMENTO on tp.TP_ID equals tt.TP_ID
                         join c in db.PRODUTO_COMPOSICAO on tp.CMP_ID equals c.CMP_ID
                         where rt.RG_ID == 11 && c.CMP_ID_ORIGEM == _cmp_origem && tt.TPG_ID == _tpg_id && tp.TTP_ID == _ttp_id 
                         select new ProdutoTabelaPrecoDTO
                         {
                             CMP_ID = tp.CMP_ID,
                             CMP_ID_ORIGEM = c.CMP_ID_ORIGEM,
                             CMP_DESCRICAO = c.CMP_DESCRICAO,
                             RTP_PRECO_VENDA = rt.RTP_PRECO_VENDA,
                             RG_ID = rt.RG_ID,
                             TP_ID = tp.TP_ID,
                             TPG_ID = tt.TPG_ID,

                         }).ToList();


            return query;
            
        }

        public IList<ProdutoComposicaoDTO> BuscarProdutoComposicaoAtivoPorProdutoId(int produtoId)
        {
            var query = (from cmp in db.PRODUTO_COMPOSICAO
                         join pci in db.PRODUTO_COMPOSICAO_ITEM on cmp.CMP_ID equals pci.CMP_ID
                         where pci.PRO_ID == produtoId
                         && cmp.DATA_EXCLUSAO == null
                         select cmp
                         ).ToList();
            return ToDTO(query);
        }

        public Pagina<ProdutoComposicaoDTO> ProdutosComposicoes(string nome = null, 
            string nomeEstrangeiro = null, string nomeProduto = null, 
            int pagina = 1, int itensPorPaginas = 7, bool? produtoInteresse = null, int? proId = null)
        {
            IQueryable<PRODUTO_COMPOSICAO> query = GetDbSet().AsQueryable();

            if (!String.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.CMP_DESCRICAO.Contains(nome));
            }

            if (!String.IsNullOrEmpty(nomeEstrangeiro))
            {
                query = query.Where(x => x.CMP_NOME_ESTRANGEIRO.Contains(nomeEstrangeiro));
            }

            //if (!String.IsNullOrEmpty(nomeProduto))
            //{
            //    query = query.Where(x => x.PRODUTOS.PRO_NOME.Contains(nomeProduto) || x.PRODUTOS.PRO_SIGLA.Contains(nomeProduto));
            //}

            if (produtoInteresse != null && (bool) produtoInteresse)
            {
                query = query.Where(x => x.CMP_PRO_INTERESSE == (bool) produtoInteresse);
            
            }

            //if (proId != null)
            //{
            //    query = query.Where(x => x.PRO_ID == proId);
            //}

            query = query.Where(x => x.DATA_EXCLUSAO == null);

            return ToDTOPage(query, pagina, itensPorPaginas);
        }

        public override ProdutoComposicaoDTO FindByIdConverted(params object[] id)
        {
            var obj = GetDbSet().Find(id);
            ProdutoComposicaoDTO dto = ToDTO(obj);
            dto.PRODUTO_COMPOSICAO_ITEM = Convert<IEnumerable<PRODUTO_COMPOSICAO_ITEM>, List<ProdutoComposicaoItemDTO>>(obj.PRODUTO_COMPOSICAO_ITEM);
            return dto;
        }

        public IList<ProdutoComposicaoDTO> FindAllValid()
        {
            var query = GetDbSet().Where(p => p.DATA_EXCLUSAO == null && p.PRODUTO_COMPOSICAO_ITEM.Count > 0);
            return ToDTO(query);
        }

        public IList<ProdutoComposicaoDTO> ProdutosDeInteresse(string descricao)
        {
            var query = GetDbSet().Where(x => x.DATA_EXCLUSAO == null && x.CMP_PRO_INTERESSE && x.PRODUTO_COMPOSICAO_ITEM.Count() > 0);

            if (!string.IsNullOrWhiteSpace(descricao))
            {
                query = query.Where(x => x.CMP_DESCRICAO.Contains(descricao));
            }
                
            query = query.OrderBy(or => or.CMP_DESCRICAO);

            return ToDTO(query);
        }


        public Pagina<ProdutoComposicaoDTO> ListarCursos(string descricao, 
            bool? produtoInteresse = null, 
            int pagina = 1, 
            int registrosPorPagina = 7, 
            int? empId = null)
        {
            var query = (from 
                            cmp in db.PRODUTO_COMPOSICAO join
                            cmpItem in db.PRODUTO_COMPOSICAO_ITEM on cmp.CMP_ID equals cmpItem.CMP_ID
                        where 
                            cmp.DATA_EXCLUSAO == null && 
                            (produtoInteresse == null || produtoInteresse == false|| cmp.CMP_PRO_INTERESSE == produtoInteresse) && 
                            cmpItem.PRODUTOS.GRUPO_ID == 2 &&
                            cmpItem.PRODUTOS.DATA_EXCLUSAO == null &&
                            (empId == null || cmp.EMP_ID == empId)
                        select cmp);
            
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                query = query.Where(x => x.CMP_DESCRICAO.Contains(descricao));
            }

            query = query.OrderBy(or => or.CMP_DESCRICAO);

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public Pagina<ProdutoComposicaoDTO> ListarProdutosExcetoCursos(string descricao, 
            bool? produtoInteresse = null, 
            int pagina = 1, 
            int registrosPorPagina = 7, 
            int? empId = null)
        {
            var query = (from
                            cmp in db.PRODUTO_COMPOSICAO
                         where
                            (cmp.DATA_EXCLUSAO == null) &&
                            (produtoInteresse == null || produtoInteresse == false || cmp.CMP_PRO_INTERESSE == produtoInteresse) &&
                            !(from cmpItem in db.PRODUTO_COMPOSICAO_ITEM 
                            where
                                cmpItem.PRODUTOS.GRUPO_ID == 2 &&
                                cmpItem.PRODUTOS.DATA_EXCLUSAO == null 
                            select cmpItem.CMP_ID)
                            .Contains(cmp.CMP_ID)
                            && (from subItem in db.PRODUTO_COMPOSICAO_ITEM
                                where subItem.CMI_GERA_ASSINATURA_LEGADO
                                select subItem.CMP_ID)
                            .Contains(cmp.CMP_ID) &&
                            (empId == null || cmp.EMP_ID == empId)
                        select cmp);

            if (!string.IsNullOrWhiteSpace(descricao))
            {
                query = query.Where(x => x.CMP_DESCRICAO.Contains(descricao));
            }

            query = query.OrderBy(or => or.CMP_DESCRICAO);

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public bool ChecaProdutoComposicaoEhCurso(int cmdId)
        {
            var query = (from cmpItem in db.PRODUTO_COMPOSICAO_ITEM
                         where
                             cmpItem.PRODUTOS.GRUPO_ID == 2 &&
                             cmpItem.PRODUTOS.DATA_EXCLUSAO == null &&
                             cmpItem.CMP_ID == cmdId
                         select cmpItem.CMP_ID);
            var count = query.Count();

            return (count > 0);
        }

        public int? RetornaQuantidadeDeConsultasDoProdutoComposto(int? cmpId)
        {
            var query = (from item in db.PRODUTO_COMPOSICAO_ITEM
                         where item.CMP_ID == cmpId &&
                             item.PRODUTOS.TPC_ID == 1
                         select item.CMI_QTDE_PERIODO);

            int? consultas = query.FirstOrDefault();
            return consultas;
        }

        public ProdutoComposicaoDTO ObterProdutoDeInteressePorNome(string descricao)
        {
            var query = (from pro in 
                             db.PRODUTO_COMPOSICAO 
                         where pro.DATA_EXCLUSAO == null && 
                         pro.CMP_PRO_INTERESSE && 
                         pro.CMP_DESCRICAO.Trim().ToLower() == descricao.Trim().ToLower()
                             select pro).FirstOrDefault();

            return ToDTO(query);
        }

        public ProdutoComposicaoDTO ObterProdutoPorNome(string descricao)
        {
            var query = (from pro in
                             db.PRODUTO_COMPOSICAO
                         where pro.DATA_EXCLUSAO == null &&
                         pro.CMP_DESCRICAO.Trim().ToLower() == descricao.Trim().ToLower()
                         select pro).FirstOrDefault();

            return ToDTO(query);
            
        }


        public ProdutoComposicaoDTO ObterProdutoRenovacao(int? pro_id)
        {
            var query = (from i in db.PRODUTO_COMPOSICAO_ITEM
                         join p in db.PRODUTO_COMPOSICAO on i.CMP_ID equals p.CMP_ID 
                        where p.DATA_EXCLUSAO == null 
                           && i.PRO_ID == pro_id
                           && p.CMP_RENOVACAO == true
                         select p).FirstOrDefault();

            return ToDTO(query);

        }


        public RequisicaoProdutoRenovacaoDTO RetornarProdutoCompostoSimilar(int? proId, string codAssinatura, int? empId)
        {
            var query = (from ass in db.ASSINATURA
                         where ass.ASN_NUM_ASSINATURA == codAssinatura
                         let assi = ass
                         from
                            cmi in db.PRODUTO_COMPOSICAO_ITEM join
                            cmp in db.PRODUTO_COMPOSICAO on cmi.CMP_ID equals cmp.CMP_ID
                         where
                            cmp.DATA_EXCLUSAO == null && // Produto composto não pode estar marcado como excluído.
                            cmp.CMP_RENOVACAO == true &&
                            (from cmi0 in db.PRODUTO_COMPOSICAO_ITEM // o produto item deve ter um co-item que pertence ao produto informado
                             where cmi0.PRO_ID == proId &&
                             cmi.CMI_GERA_ASSINATURA_LEGADO // deve gerar assinatura
                             select cmi0.CMP_ID).Contains(cmi.CMP_ID) &&
                             (empId == null || cmp.EMP_ID == empId)
                             
                            // && 
                            //(
                            //    // Se existe algum item que corresponde a quantidade de consultas anterior a assinatura. Se não tiver retorna qualquer 1
                            //    (from cmi2 in db.PRODUTO_COMPOSICAO_ITEM 
                            //    where cmi2.CMP_ID == cmi.CMP_ID &&
                            //    cmi2.PRODUTOS.TPC_ID == 1 && 
                            //    cmi2.CMI_QTDE_PERIODO == ass.ASN_QTDE_CONS_CONTRATO
                            //    select cmi2.CMP_ID).Count() <= 0 ||
                            //    cmi.PRODUTOS.TPC_ID == 1 && 
                            //    cmi.CMI_QTDE_PERIODO == ass.ASN_QTDE_CONS_CONTRATO
                            //)
                         select new RequisicaoProdutoRenovacaoDTO()
                         {
                             CmpId = cmi.CMP_ID,
                             ProId = cmi.PRO_ID,
                             QtdConsultas = cmi.CMI_QTDE_PERIODO,
                             NumeroAssinatura = codAssinatura,
                             ValorDaVenda = cmi.CMI_PRECO_UNIT
                         });

            return query.FirstOrDefault();
                         //();
        }

        public Pagina<ProdutoComposicaoDTO> ListarProdutosVitrine(string nomeProduto = null, int pagina = 1, int registrosPorPagina = 12)
        {
            if (string.IsNullOrWhiteSpace(nomeProduto))
            {
                nomeProduto = null;

            }
            var query = (from 
                            cmp in db.PRODUTO_COMPOSICAO
                         where 
                            cmp.DATA_EXCLUSAO == null &&
                            (nomeProduto == null || cmp.CMP_DESCRICAO.Contains(nomeProduto)) &&
                            cmp.CMP_EXIBIR_VITRINE == true
                         select cmp);

            return ToDTOPage(query, pagina, registrosPorPagina);
        }
		
        public ProdutoComposicaoDTO BuscarProduto ( int id = 0 )
        {

            var query = (from
                            cmp in db.PRODUTO_COMPOSICAO
                         where
                            cmp.DATA_EXCLUSAO == null &&
                            ( cmp.CMP_ID == id  ) &&
                            cmp.CMP_EXIBIR_VITRINE == true
                         select cmp);

            return ToDTO(query).First();
        }
    }
}

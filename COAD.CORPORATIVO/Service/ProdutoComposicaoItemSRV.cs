using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CMP_ID", "PRO_ID")]
    public class ProdutoComposicaoItemSRV : GenericService<PRODUTO_COMPOSICAO_ITEM, ProdutoComposicaoItemDTO, int>
    {
        public ProdutoComposicaoItemDAO _dao { get; set; }


        public ProdutoComposicaoItemSRV()
        {
            this._dao = new ProdutoComposicaoItemDAO();
            Dao = this._dao;
        }

        public ProdutoComposicaoItemSRV(ProdutoComposicaoItemDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }

        /// <summary>
        /// Verifica se já existe uma composição enterior com o mesmo produto selecionado
        /// </summary>
        /// <param name="composicaoItemDTO"></param>
        public void ChecaDuplicidade(ProdutoComposicaoItemDTO composicaoItemDTO)
        {
            if (!_dao.ChecaDuplicidade(composicaoItemDTO))
            {
                throw new ValidationException("Já existe uma composição com o produto selecionado. ");
            }
        }

        public void DeletarComposicaoItens(ProdutoComposicaoDTO composicao)
        {
            if (composicao != null && composicao.CMP_ID != null)
            {
                var cmpId = composicao.CMP_ID;
                var composicaoDoBanco = new ProdutoComposicaoSRV().FindByIdFullLoad((int)cmpId);

                ExcluirList(composicao, composicaoDoBanco, "PRODUTO_COMPOSICAO_ITEM");
            }
        }

        public void SalvarItensComposicao(ProdutoComposicaoDTO composicao, IEnumerable<ProdutoComposicaoItemDTO> composicaoItem, bool ehCurso = false)
        {

            if (composicao != null && composicao.CMP_ID != null)
            {
                var composicaoId = composicao.CMP_ID;
                foreach (var item in composicaoItem)
                {
                    if (item.CMP_ID == null || item.CMP_ID != composicaoId)
                        item.CMP_ID = composicaoId;

                    if (ehCurso)
                    {
                        item.TTP_ID = null;
                        item.TIPO_PERIODO = null;
                    }
                }

                SaveOrUpdateNonIdentityKeyEntity(composicaoItem);
            }
        }


        public bool ProdutoComposicaoPossuiComposicaoItemDeCurso(int? CMP_ID)
        {
            return _dao.ProdutoComposicaoPossuiComposicaoItemDeCurso(CMP_ID);
        }

        /// <summary>
        /// Verifica se o produto composto possui um item de curso. 
        /// Se a composição item não for encontrada é adicionada ao produto composto.
        /// Esse método deve ser usado para composições que serão usadas como curso.
        /// </summary>
        /// <param name="produtoCompo"></param>
        public void AdicionarProdutoCurso(ProdutoComposicaoDTO produtoCompo)
        {
            if (produtoCompo != null && produtoCompo.CMP_ID != null)
            {
               
                var lstCurso = produtoCompo.PRODUTO_COMPOSICAO_ITEM.Where(x => x.PRODUTOS != null && x.PRODUTOS.GRUPO_ID == 2);

                if (lstCurso.Count() > 0)
                {

                    var item = lstCurso.FirstOrDefault();

                    item.CMI_PRECO_TOTAL = "0";
                    item.CMI_QTDE_PERIODO = 0;
                        
                    if (item.CMP_ID == null)
                    {
                        item.CMP_ID = produtoCompo.CMP_ID;
                    }

                    if (item.CMI_QTDE == null)
                    {
                        item.CMP_ID = produtoCompo.CMP_ID;
                    }

                    if (item.CMI_PRECO_UNIT == null)
                    {
                        item.CMI_PRECO_UNIT = produtoCompo.CMP_VLR_VENDA;
                    }



                }
                else
                {
                    var produtoCompItem = new ProdutoComposicaoItemDTO()
                    {
                        CMP_ID = produtoCompo.CMP_ID,
                        PRO_ID = 40,
                        CMI_QTDE = 1,
                        CMI_PRECO_UNIT = produtoCompo.CMP_VLR_VENDA,
                        CMI_PRECO_TOTAL = "0",
                        CMI_QTDE_PERIODO = 0
                    };

                    produtoCompo.PRODUTO_COMPOSICAO_ITEM.Add(produtoCompItem);
                }
            }
        }

        public ProdutoComposicaoItemDTO ObterProdutoComposicaoItemQueGeraAssinatura(int? cmpId)
        {
            return _dao.ObterProdutoComposicaoItemQueGeraAssinatura(cmpId);
        }

        public int? ObterProIdParaGerarAssinatura(int? cmpId)
        {
            var produtoComposicaoItem = ObterProdutoComposicaoItemQueGeraAssinatura(cmpId);
            if (produtoComposicaoItem == null)
            {
                throw new FaturamentoException("Erro. O produto escolhido não possui itens faturáveis. Ou não possui nenhum item.");
            }
            return produtoComposicaoItem.PRO_ID;            
        }

        public ProdutosDTO ObterProdutoParaGerarAssinatura(int? cmpId)
        {
            var produtoComposicaoItem = ObterProdutoComposicaoItemQueGeraAssinatura(cmpId);
            if (produtoComposicaoItem == null)
            {
                throw new FaturamentoException("Erro. O produto escolhido não possui itens faturáveis. Ou não possui nenhum item.");
            }
            return produtoComposicaoItem.PRODUTOS;
        }

        public IList<ProdutoComposicaoItemDTO> BuscaProdutoComposicaoItemPorComposicaoId(int? cmpId)
        {
            var dadosProdutosItem = _dao.BuscaProdCmpItemPorComposicaoId(cmpId);

            return dadosProdutosItem;
        }
    }
}

using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CMP_ID", "TTP_ID")] 
    public class ProdutoComposicaoTipoPeriodoSRV : ServiceAdapter<PRODUTO_COMPOSICAO_TIPO_PERIODO, ProdutoComposicaoTipoPeriodoDTO, int>
    {
        private ProdutoComposicaoTipoPeriodoDAO _dao;

        public ProdutoComposicaoTipoPeriodoSRV()
        {
            _dao = new ProdutoComposicaoTipoPeriodoDAO();
            SetDao(_dao);
        }

        public ProdutoComposicaoTipoPeriodoSRV(ProdutoComposicaoTipoPeriodoDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public void ExcluirTipoPeriodo(ProdutoComposicaoDTO produtoComposicao)
        {
            var CMP_ID = (int)produtoComposicao.CMP_ID;
            var produtoCompostoDoBanco = new ProdutoComposicaoSRV().FindByIdFullLoad(CMP_ID, false, true);

            ExcluirList(produtoComposicao, produtoCompostoDoBanco, "PRODUTO_COMPOSICAO_TIPO_PERIODO");
        }

        public void SalvarEExcluirProdutoComposicaoTipoPeriodo(ProdutoComposicaoDTO produtoComposicao)
        {
            ExcluirTipoPeriodo(produtoComposicao);
            //new CursoProxySRV().ExcluirPropertyList<AreaConsultoriaCursoProxyDTO>(curso, "areaConCursoProxy");

            var lstProdutoComposicaoTipoPerido = produtoComposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO;

            if (lstProdutoComposicaoTipoPerido != null)
            {
                SalvarProdutoComposicaoTipoPeriodo(produtoComposicao, lstProdutoComposicaoTipoPerido.AsQueryable());
            }
        }

        public void SalvarProdutoComposicaoTipoPeriodo(ProdutoComposicaoDTO produtoComposicao, IQueryable<ProdutoComposicaoTipoPeriodoDTO> lstProdutoComposicaoTipoPeriodo)
        {
            if (lstProdutoComposicaoTipoPeriodo != null)
            {
                CheckAndAssignKeyFromParentToChildsList(produtoComposicao, lstProdutoComposicaoTipoPeriodo, "CMP_ID");

                foreach (var itemProdutoTipoPeriodo in lstProdutoComposicaoTipoPeriodo)
                {
                    if (itemProdutoTipoPeriodo != null && itemProdutoTipoPeriodo.TTP_ID == null && itemProdutoTipoPeriodo.TIPO_PERIODO != null)
                    {
                        itemProdutoTipoPeriodo.TTP_ID = itemProdutoTipoPeriodo.TIPO_PERIODO.TTP_ID;
                    }
                }

                SaveOrUpdateNonIdentityKeyEntity(lstProdutoComposicaoTipoPeriodo);
            }
        }
    }
}

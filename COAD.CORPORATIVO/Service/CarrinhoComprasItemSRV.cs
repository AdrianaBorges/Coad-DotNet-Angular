

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("CRC_ID", "CMP_ID")]
	public class CarrinhoComprasItemSRV : GenericService<CARRINHO_COMPRAS_ITEM, CarrinhoComprasItemDTO, Int32>
	{
        public ProdutoComposicaoSRV _produtoComposicao { get; set; }
        public CarrinhoComprasItemDAO _dao { get; set; }

        public CarrinhoComprasItemSRV(CarrinhoComprasItemDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

        private void PreencherProdutoComposicao(ICollection<CarrinhoComprasItemDTO> lstItens)
        {
            if(lstItens != null)
            {
                foreach(var itm in lstItens)
                {
                    itm.PRODUTO_COMPOSICAO = _produtoComposicao.FindById(itm.CMP_ID);
                }
            }

        }

        public ICollection<CarrinhoComprasItemDTO> ListarCarrinhoComprasItensDoCarrinho(int? crcId)
        {
            var lstItens = _dao.ListarCarrinhoComprasItensDoCarrinho(crcId);
            PreencherProdutoComposicao(lstItens);
            return lstItens;
        }

        public void PreencherItensNoCarrinho(CarrinhoComprasDTO carrinhoCompras)
        {
            if(carrinhoCompras != null && carrinhoCompras.CRC_ID != null)
            {
                carrinhoCompras.CARRINHO_COMPRAS_ITEM = ListarCarrinhoComprasItensDoCarrinho(carrinhoCompras.CRC_ID);
            }
        }

        public void SalvarEExcluirCarrinhoItens(CarrinhoComprasDTO carrinhoItem)
        {
            var itens = carrinhoItem.CARRINHO_COMPRAS_ITEM;
            if (itens != null)
            {
                ExcluirConfigImposto(carrinhoItem);
                SalvarConfigImposto(itens, carrinhoItem);
            }

        }

        public void SalvarConfigImposto(IEnumerable<CarrinhoComprasItemDTO> itens, CarrinhoComprasDTO carrinhoCompras)
        {
            CheckAndAssignKeyFromParentToChildsList(carrinhoCompras, itens, "CRC_ID");
            SaveOrUpdateNonIdentityKeyEntity(itens);
        }
        

        public void ExcluirConfigImposto(CarrinhoComprasDTO carrinhoCompras)
        {
            if (carrinhoCompras.DATA_CANCELAMENTO == null)
            {
                var crcId = carrinhoCompras.CRC_ID;
                var nfConfigBanco = ServiceFactory.RetornarServico<CarrinhoComprasSRV>().FindByIdFullLoaded(crcId, true);
                var lstConfigImposto = GetMissinList(carrinhoCompras, nfConfigBanco, "CARRINHO_COMPRAS_ITEM");
                DeletarItens(lstConfigImposto);
            }
        }

        public void DeletarItens(IEnumerable<CarrinhoComprasItemDTO> itens)
        {
            if (itens != null)
            {
                foreach (var regTab in itens)
                {
                    regTab.DATA_CANCELAMENTO = DateTime.Now;
                }

                SaveOrUpdateNonIdentityKeyEntity(itens);
            }
        }
    }
}

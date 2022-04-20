using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes.Maps;
using System.Transactions;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Service;
using COAD.CORPORATIVO.Service.Custons;
namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PIC_ID")]
    public class PropostaItemComprovanteSRV : GenericService<PROPOSTA_ITEM_COMPROVANTE, PropostaItemComprovanteDTO, int>
    {
        public PropostaItemComprovanteDAO _dao; 
        
        public PropostaItemComprovanteSRV()
        {
            this._dao = new PropostaItemComprovanteDAO();
        }

        public PropostaItemComprovanteSRV(PropostaItemComprovanteDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }

        public IList<PropostaItemComprovanteDTO> ListarPropostaItemComprovante(int? ppiId = null, int? ipeId = null)
        {
            return _dao.ListarPropostaItemComprovante(ppiId, ipeId);
        }

        public void PreencherPropostaItemComprovante(PropostaItemDTO propostaItem)
        {
            if (propostaItem != null && propostaItem.PPI_ID != null)
            {
                propostaItem.PROPOSTA_ITEM_COMPROVANTE = ListarPropostaItemComprovante(propostaItem.PPI_ID);

                foreach (var proItmCom in propostaItem.PROPOSTA_ITEM_COMPROVANTE)
                {
                    proItmCom.PROPOSTA_ITEM = null;
                    proItmCom.ITEM_PEDIDO = null;
                }
            }
        }

        public void PreencherPropostaItemComprovanteNoPedido(ICollection<ItemPedidoDTO> lstItemPedido)
        {
            if(lstItemPedido != null)
            {
                foreach (var item in lstItemPedido)
                {
                    PreencherPropostaItemComprovanteNoPedido(item);
                }
            }
        }


        public void PreencherPropostaItemComprovanteNoPedido(ItemPedidoDTO itemPedido)
        {
            if (itemPedido != null && itemPedido.IPE_ID != null)
            {
                itemPedido.PROPOSTA_ITEM_COMPROVANTE = ListarPropostaItemComprovante(null, itemPedido.IPE_ID);

                foreach (var proItmCom in itemPedido.PROPOSTA_ITEM_COMPROVANTE)
                {
                    proItmCom.PROPOSTA_ITEM = null;
                    proItmCom.ITEM_PEDIDO = null;
                }
            }
        }

        public void ChecarExcluirPropostaItemComprovanteAusentes(IPedidoItem pedidoItem, string usuario, int? REP_ID)
        {
            var propostaItemSRV = ServiceFactory.RetornarServico<PropostaItemSRV>();

            if (pedidoItem != null)
            { 
                IPedidoItem objetoDoBanco = null;
                ICollection<PropostaItemComprovanteDTO> lstComprovantes = null;

                if(pedidoItem is PropostaItemDTO)
                {
                    objetoDoBanco = propostaItemSRV.FindByIdFullLoaded(((PropostaItemDTO)pedidoItem).PPI_ID, true);
                    lstComprovantes = objetoDoBanco.Comprovantes;
                }
                else if(pedidoItem is ItemPedidoDTO)
                {
                    lstComprovantes = ListarPropostaItemComprovante(null, ((ItemPedidoDTO)pedidoItem).IPE_ID);
                }           

                var lstParaExcluir = GetMissinList(lstComprovantes, pedidoItem.Comprovantes);

                MarcarPropostaItemComprovanteComoExcluido(lstParaExcluir, usuario, REP_ID, pedidoItem.PstId);
            }
        }

        public void SalvarPropostaItemComprovante(IPedidoItem pedidoItem, string usuario, int? REP_ID)
        {
            if (pedidoItem != null)
            {
                var lstPropostaItemComprovante = pedidoItem.Comprovantes;

                ChecarExcluirPropostaItemComprovanteAusentes(pedidoItem, usuario, REP_ID);
                if (pedidoItem is PropostaItemDTO)
                {
                    CheckAndAssignKeyFromParentToChildsList(pedidoItem, lstPropostaItemComprovante, "PPI_ID");

                }
                else if(pedidoItem is ItemPedidoDTO)
                {
                    CheckAndAssignKeyFromParentToChildsList(pedidoItem, lstPropostaItemComprovante, "IPE_ID");
                }                
                var lstPropostaItemComprovanteSalvo = SaveOrUpdateAll(lstPropostaItemComprovante).ToList();

            }
        }
        
        public void MarcarPropostaItemComprovanteComoExcluido(IEnumerable<PropostaItemComprovanteDTO> lstPropostaItemComprovante, string usuario, int? REP_ID, int? pstId)
        {
            if (lstPropostaItemComprovante != null)
            {
                foreach (var proItemCom in lstPropostaItemComprovante)
                {
                    proItemCom.DATA_EXCLUSAO = DateTime.Now;
                    var toString = proItemCom.ToString();

                    ServiceFactory.RetornarServico<HistoricoPedidoSRV>().RegistrarHistoricoComprovanteExcluido(usuario, REP_ID, proItemCom.PPI_ID, proItemCom.IPE_ID, pstId, toString);
                }

                MergeAll(lstPropostaItemComprovante);
            }
        }

        public void SalvarPropostaItemComprovanteComTransacao(IPedidoItem pedidoProposta, string usuario, int? REP_ID)
        {
            if (pedidoProposta != null)
            {
                using (var scope = new TransactionScope())
                {
                    SalvarPropostaItemComprovante(pedidoProposta, usuario, REP_ID);
                    scope.Complete();
                }
            }
        }

        public void AssociarComprovantesDaPropostaNoItemPedido(int? ppiId, int? ipeId)
        {
            if(ppiId != null && ipeId != null)
            {
                var lstComprovantes = ListarPropostaItemComprovante(ppiId);
                foreach(var comp in lstComprovantes)
                {
                    comp.IPE_ID = ipeId;
                }

                SaveOrUpdateAll(lstComprovantes);
            }
        }        
    }
}
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Util;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("IPE_ID", "PGT_ID")]
    public class ItemPedidoPedidoPagamentoSRV : ServiceAdapter<ITEM_PEDIDO_PEDIDO_PAGAMENTO, ItemPedidoPedidoPagamentoDTO, int>
    {
        private ItemPedidoPedidoPagamentoDAO _dao = new ItemPedidoPedidoPagamentoDAO();

        [ServiceProperty("PGT_ID", Name = "pedidoPagamento", PropertyName = "PEDIDO_PAGAMENTO", FindById = true)]
        protected PedidoPagamentoSRV _pedidoPagamentoSRV = new PedidoPagamentoSRV();

        protected TipoPagamentoSRV _tipoPagamentoSRV = new TipoPagamentoSRV();

        public ItemPedidoPedidoPagamentoSRV()
        {
            SetDao(_dao);
        }

        public void SalvarItemPedidoPagamento(ItemPedidoDTO itemPedido)
        {
            if (itemPedido != null)
            {
                var lstPedidoPedidoPagamento = itemPedido.ITEM_PEDIDO_PEDIDO_PAGAMENTO;

                if (lstPedidoPedidoPagamento != null)
                {
                    CheckAndAssignKeyFromParentToChildsList(itemPedido, lstPedidoPedidoPagamento, "IPE_ID");

                    foreach (var pedidoPedidoPagamento in lstPedidoPedidoPagamento)
                    {
                        _processarSalvamentoItemPedidoPagamento(pedidoPedidoPagamento);
                    }
                }
            }
        }

        private void _processarSalvamentoItemPedidoPagamento(ItemPedidoPedidoPagamentoDTO pedidoPedidoPagamento)
        {
            if (pedidoPedidoPagamento.PEDIDO_PAGAMENTO != null)
            {
                var pedidoPagamento = pedidoPedidoPagamento.PEDIDO_PAGAMENTO;
                pedidoPedidoPagamento.PEDIDO_PAGAMENTO = null;

                var pedidoPagamentoSalvo = _pedidoPagamentoSRV.SalvarPedidoPagamento(pedidoPagamento);
                pedidoPedidoPagamento.PGT_ID = pedidoPagamentoSalvo.PGT_ID;

                SaveOrUpdateNonIdentityKeyEntity(pedidoPedidoPagamento);
                pedidoPedidoPagamento.PEDIDO_PAGAMENTO = pedidoPagamento;

            }
        }

        public ICollection<ItemPedidoPedidoPagamentoDTO> ConverterParaPedidoPagamento(ItemPedidoDTO itemPedido, TipoPagamentoDTO tipoPagamento)
        {
            ICollection<ItemPedidoPedidoPagamentoDTO> lstPedido = new List<ItemPedidoPedidoPagamentoDTO>();

            if (itemPedido != null)
            {
                if (tipoPagamento != null)
                {
                    if (tipoPagamento.TPG_TIPO == 1)
                    {
                        _tipoPagamentoSRV.PreencherTiposDePagamentosNoTipoPagamentoComposto(tipoPagamento);


                        if (tipoPagamento.ListaTipoPagamento != null)
                        {
                            int index = 0;

                            foreach (var subTipoPagamento in tipoPagamento.ListaTipoPagamento)
                            {
                                int? parcela = null;

                                if (index == 0)
                                {
                                    parcela = 1;
                                    var pedPag = GerarPedidoPagamento(itemPedido, subTipoPagamento, parcela, true, itemPedido.DataVencimento);
                                    lstPedido.Add(pedPag);
                                }
                                else
                                {
                                    parcela = itemPedido.IPE_PARCELA;

                                    if (parcela > 0)
                                    {
                                        DateTime? dataVenc = null;
                                        if (itemPedido.DataVencimentoSegParcela != null)
                                            dataVenc = itemPedido.DataVencimentoSegParcela; 
                                        else
                                            dataVenc = DateUtil.AdicionaMes(itemPedido.DataVencimento, 1, itemPedido.DataVencimento.Value.Day);
                                        var pedPag = GerarPedidoPagamento(itemPedido, subTipoPagamento, parcela, false, dataVenc);
                                        lstPedido.Add(pedPag);
                                    }
                                }

                                index++;
                            }
                        }
                    }
                    else
                    {
                        var parcelas = itemPedido.IPE_PARCELA;
                        var pedPag = GerarPedidoPagamento(itemPedido, tipoPagamento, parcelas, true, itemPedido.DataVencimento);
                        lstPedido.Add(pedPag);
                    }
                }
            }

            return lstPedido;
        }

        private ItemPedidoPedidoPagamentoDTO GerarPedidoPagamento(ItemPedidoDTO itemPedido, TipoPagamentoDTO tipoPagamento, int? parcelas, bool entrada, DateTime? dataVencimento = null)
        {
            ItemPedidoPedidoPagamentoDTO pedidoPedidoPagamento = new ItemPedidoPedidoPagamentoDTO()
            {
                DATA_ASSOCIACAO = DateTime.Now,
                ITEM_PEDIDO = itemPedido
            };

            decimal? vlrParcela = null; 
            decimal? vlrTotal = null;

            if(itemPedido.IPE_POSSUI_ENTRADA != null && (bool)itemPedido.IPE_POSSUI_ENTRADA && entrada)
            {
                vlrParcela = itemPedido.IPE_VALOR_ENTRADA;
                vlrTotal = itemPedido.IPE_VALOR_ENTRADA;
            }
            else{
                vlrParcela = itemPedido.IPE_VALOR_PARCELA;
                vlrTotal = (itemPedido.TPG_ID != 9) ? itemPedido.IPE_PARCELA * itemPedido.IPE_VALOR_PARCELA : vlrParcela;

            }

            var pedidoPagamento = new PedidoPagamentoDTO()
            {
                TPG_ID = tipoPagamento.TPG_ID,
                TIPO_PAGAMENTO = tipoPagamento,
                PGT_QTDE_PARCELAS = parcelas,
                PGT_VLR_PARCELA = vlrParcela,
                PGT_VLR_TOTAL = vlrTotal,
                PGT_ENTRADA = entrada,
                PGT_DATA_VENCIMENTO = dataVencimento
            };

            pedidoPedidoPagamento.PEDIDO_PAGAMENTO = pedidoPagamento;

            return pedidoPedidoPagamento;
        }

        
    }
}

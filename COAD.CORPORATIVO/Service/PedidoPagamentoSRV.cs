using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;
using GenericCrud.Metadatas;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PGT_ID")]
    public class PedidoPagamentoSRV : ServiceAdapter<PEDIDO_PAGAMENTO, PedidoPagamentoDTO>
    {
        private PedidoPagamentoDAO _dao;

        public PedidoPagamentoSRV()
        {
            _dao = new PedidoPagamentoDAO();  
            SetDao(_dao);
        }

        public PedidoPagamentoSRV(PedidoPagamentoDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public PedidoPagamentoDTO SalvarPedidoPagamento(PedidoPagamentoDTO pedidoPagamento)
        {
            PedidoPagamentoDTO pedido = null;
            if (pedidoPagamento != null)
            {
                if (pedidoPagamento.TIPO_PAGAMENTO != null)
                {
                    pedidoPagamento.TPG_ID = pedidoPagamento.TIPO_PAGAMENTO.TPG_ID;
                    pedidoPagamento.TIPO_PAGAMENTO = null;
                }
                pedido = Save(pedidoPagamento);
            }
            return pedido;
        }

        public PedidoPagamentoDTO RetornarTipoPagamentoDeEntrada(int? IPE_ID)
        {
            return _dao.RetornarTipoPagamentoDeEntrada(IPE_ID);
        }

        public void MarcarPedidoPagamentoDaParcelaComoPago(ParcelasDTO parcela, RequisicaoPagamentoDTO requisicao)
        {
            var pedidoPagamento = FindById(parcela.PGT_ID);
            MarcarPedidoPagamentoPago(pedidoPagamento, requisicao);
        }

        public void MarcarPedidoPagamentoPago(PedidoPagamentoDTO pedidoPagamento, RequisicaoPagamentoDTO requisicao)
        {
            if (pedidoPagamento != null && pedidoPagamento.PGT_PAGO != true)
            {
                pedidoPagamento.PGT_PAGO = true;
                pedidoPagamento.PGT_DATA_PAGAMENTO = (requisicao != null && requisicao.DataLiquidacao != null) ? requisicao.DataLiquidacao : DateTime.Now;

                if (requisicao != null)
                {
                    AdicionarDadosDeGatewayNoPedidoPagamento(requisicao, pedidoPagamento);
                }
                SaveOrUpdate(pedidoPagamento);
            }
        }
        
        private void ChecaStatusGateWay(PedidoPagamentoDTO pedidoPagamento)
        {
            if (pedidoPagamento != null)
            {
                if (pedidoPagamento.PGT_ID == 7)
                {
                    if (pedidoPagamento.PGT_STATUS_TRANSACAO != "Paid" && pedidoPagamento.PGT_STATUS_TRANSACAO != "Overpaid" && pedidoPagamento.PGT_STATUS_TRANSACAO != "Underpaid")
                    {
                        var msg = "O status do pedido não pode ser alterado. O status de retorno do gateway foi {0}.";
                        msg = string.Format(msg, pedidoPagamento.PGT_STATUS_TRANSACAO);
                        throw new PedidoException(msg);
                    }
                }
            }
        }

        private void AdicionarDadosDeGatewayNoPedidoPagamento(RequisicaoPagamentoDTO requisicao, PedidoPagamentoDTO pedidoPagamento)
        {
            pedidoPagamento.PGT_CODIGO_DE_BARRAS = requisicao.CodigoBarras;
            pedidoPagamento.PGT_URL_BOLETO = requisicao.UrlBoleto;
            pedidoPagamento.ORDER_KEY = requisicao.OrderKey;
            pedidoPagamento.ORDER_KEY_REF = requisicao.OrderReference;

            if (pedidoPagamento.TPG_ID == 7 && requisicao.ChaveTransacaoBoleto != null)
            {
                pedidoPagamento.PGT_CHAVE_TRANSACAO = requisicao.ChaveTransacaoBoleto.ToString();
                pedidoPagamento.PGT_STATUS_TRANSACAO = requisicao.StatusTransacaoBoleto.ToString();

            }
            else if (pedidoPagamento.TPG_ID == 9 && requisicao.ChaveTransacaoCartao != null)
            {
                pedidoPagamento.PGT_CHAVE_TRANSACAO = requisicao.ChaveTransacaoCartao.ToString();
                pedidoPagamento.PGT_STATUS_TRANSACAO = requisicao.StatusTransacaoCC.ToString();
            }
            else
            {
                var chave = (requisicao.ChaveTransacaoCartao != null) ? requisicao.ChaveTransacaoCartao : requisicao.ChaveTransacaoBoleto;

                if (chave != null)
                {
                    pedidoPagamento.PGT_CHAVE_TRANSACAO = chave.ToString();
                }
            }

        }

        public decimal? RetornarPagamentoDeEntradaRealizado(ItemPedidoDTO itemPedido)
        {
            decimal? entrada = null;

            if (itemPedido != null && itemPedido.IPE_ID != null)
            {
                var ipeId = itemPedido.IPE_ID;
                var pedidoPagamento = RetornarTipoPagamentoDeEntrada(ipeId);

                if (pedidoPagamento != null)
                {
                    if (pedidoPagamento.TPG_ID == 9)
                    {
                        entrada = pedidoPagamento.PGT_VLR_TOTAL;
                    }
                    else
                    {
                        entrada = pedidoPagamento.PGT_VLR_PARCELA;
                    }

                }
            }
            return entrada;
        }


        /// <summary>
        /// Retorna o pagamento que não seja a entrada. Se for pagamento único retorna null.
        /// </summary>
        /// <param name="IPE_ID"></param>
        /// <returns></returns>
        public PedidoPagamentoDTO RetornarPagamentoTirandoAEntrada(int? IPE_ID)
        {
            return _dao.RetornarTipoPagamentoTirandoAEntrada(IPE_ID);
        }


        /// <summary>
        /// Retorna o valor da parcela restante (não levando em conta a entrada ou seja uma forma de pagamento única).
        /// </summary>
        /// <returns></returns>
        public decimal? RetornarParcelasRestantesASeremPagas(ItemPedidoDTO itemPedido)
        {
            decimal? vlrParcelas = null;

            if (itemPedido != null && itemPedido.IPE_ID != null)
            {
                var ipeId = itemPedido.IPE_ID;
                var pedidoPagamento = RetornarPagamentoTirandoAEntrada(ipeId);

                if (pedidoPagamento != null)
                {
                    vlrParcelas = pedidoPagamento.PGT_VLR_PARCELA;
                }
            }
            return vlrParcelas;

        }

        public IList<PedidoPagamentoDTO> ListarPedidoPagamentoPorItem(int? IPE_ID)
        {
            return _dao.ListarPedidoPagamentoPorItem(IPE_ID);
        }

        /// <summary>
        /// Compara as duas formas de pagamento e retorna a forma de pagamento que é válida. (R1) <para></para>
        /// (R1) - <para></para>
        ///     1) Se a entrada é não nula e o restante é nullo. Retorna a entrada. <para></para>
        ///     2) Se a entrada é valida e não esta paga. Retorna a entrada. <para></para>
        ///     3) Se a entrada e o restante é o mesmo objeto. Retorna a entrada. <para></para>
        ///     4) Se a entrada é valida e está paga e existe pagamento restante. Retorna a pagamento restante. <para></para>
        /// </summary>
        /// <param name="pedidoPagamentoEntrada"></param>
        /// <param name="pagamentoRestante"></param>
        /// <returns></returns>
        [MetodoAuxiliar]
        public static PedidoPagamentoDTO CompararEntradaComPagamentoRestante(PedidoPagamentoDTO pedidoPagamentoEntrada, PedidoPagamentoDTO pagamentoRestante)
        {
            PedidoPagamentoDTO pagamentoUsado = null;

            if(pedidoPagamentoEntrada == null && pagamentoRestante == null)
            {
                return null;
            }

            if (pedidoPagamentoEntrada != null && pagamentoRestante == null)
            {
                pagamentoUsado = pedidoPagamentoEntrada;
            }
            else
                if (pedidoPagamentoEntrada.PGT_ID == pagamentoRestante.PGT_ID)
                {
                    pagamentoUsado = pedidoPagamentoEntrada;
                }
                else if (pedidoPagamentoEntrada.PGT_PAGO != true)
                {
                    pagamentoUsado = pedidoPagamentoEntrada;
                }
                else if (pagamentoRestante != null)
                {
                    pagamentoUsado = pagamentoRestante;
                }
                else
                {
                    pagamentoUsado = pedidoPagamentoEntrada;
                }

            return pagamentoUsado;

        }

        public int? RetornarCodigoTipoPagamentoDeEntrada(int? ipe)
        {
            return _dao.RetornarCodigoTipoPagamentoDeEntrada(ipe);
        }

        public int? RetornarCodigoTipoPagamentoTirandoEntrada(int? ipe)
        {
            return _dao.RetornarCodigoTipoPagamentoTirandoEntrada(ipe);
        }

        public int? RetornarCodigoPedidoPagamentoDeEntrada(int? ipe)
        {
            return _dao.RetornarCodigoPedidoPagamentoDeEntrada(ipe);
        }

        public int? RetornarCodigoPedidoPagamentoTirandoEntrada(int? ipe)
        {
            return _dao.RetornarCodigoPedidoPagamentoTirandoEntrada(ipe);
        }

        /// <summary>
        /// Verifica se o pagamento de entrada é do tipo informado no argumento (tipoPagamento)
        /// </summary>
        /// <param name="ipe">Código do Item de Pedido</param>
        /// <param name="tipoPagamento">Tipo de pagamento para comparar</param>
        /// <returns></returns>
        public bool PagamentoDeEntradaEhDoTipo(int? ipe, TipoPagamentoCoorporativoEnum tipoPagamento)
        {
            var codigotipoPagamento = RetornarCodigoTipoPagamentoDeEntrada(ipe);
            return (codigotipoPagamento == (int)tipoPagamento);
        }

        /// <summary>
        /// Verifica se o pagamento de entrada é do tipo informado no argumento (tipoPagamento)
        /// </summary>
        /// <param name="ipe">Código do Item de Pedido</param>
        /// <param name="tipoPagamento">Tipo de pagamento para comparar</param>
        /// <returns></returns>
        public bool PagamentoRestanteEhDoTipo(int? ipe, TipoPagamentoCoorporativoEnum tipoPagamento)
        {
            var codigotipoPagamento = RetornarCodigoTipoPagamentoTirandoEntrada(ipe);
            return (codigotipoPagamento == (int)tipoPagamento);
        }
    }
}

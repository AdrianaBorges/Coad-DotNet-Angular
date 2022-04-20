using GatewayApiClient;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public enum TipoPagamentoGateway 
    {
        PEDIDO = 1,

        PARCELA = 2,

        /// <summary>
        /// Nesse caso a parcela que terá a baixa realizada será a informada no campo lstParcelasAPagar
        /// </summary>
        PARCELA_INFORMADA = 3
    }
    
    public class RequisicaoPagamentoDTO
    {
        public RequisicaoPagamentoDTO()
        {
            marcarPedidoPagamentoPago = true;
            darBaixar = true;

        }

        public int? CodigoItemPedido { get; set; }
        public string CodigoDaParcela { get; set; }
        public decimal? ValorPagamento { get; set; }
        public decimal? ValorRestante { get; set; }
        public int? qtdParcelas { get; set; }
        public int? TPG_ID { get; set; }
        public bool Recorrente { get; set; }
        public int? CLI_ID { get; set; }
        public int? REP_ID { get; set; }
        public int? REP_ID_EXECUTOU_A_ACAO { get; set; }
        public bool darBaixar { get; set; }
        public bool? marcarPedidoPagamentoPago { get; set; }
        public DateTime? DataLiquidacao { get; set; }

        public string USU_LOGIN { get; set; }
        public ItemPedidoDTO ITEM_PEDIDO { get; set; }

        public ClienteDto CLIENTE { get; set; }
        public ClienteEnderecoDto CLIENTE_ENDERECO { get; set; }
        public PedidoPagamentoDTO ENTRADA { get; set; }

        public IList<ParcelasDTO> lstParcelasAPagar { get; set; }
        public List<ParcelaLiquidacaoDTO> lstParcelasLiquidacao {get; set;}
        public List<ParcelaAlocadaUpdateDTO> lstParcelasAlocadas { get; set; }
        
        public PedidoPagamentoDTO PAGAMENTO_RESTANTE { get; set; }
        public TipoPagamentoGateway tipoPagamentoGateway { get; set; }

        public Guid? ChaveTransacaoBoleto { get; set; }
        public Guid? ChaveTransacaoCartao { get; set; }
        public string UrlBoleto { get; set; }
        public string CodigoBarras { get; set; }
        public CreditCardTransactionStatusEnum? StatusTransacaoCC { get; set; }

        public BoletoTransactionStatusEnum? StatusTransacaoBoleto { get; set; }
        public string StatusTransacao { get; set; }
        public string OrderKey { get; set; }
        public string OrderReference { get; set; }
        public string AuthorizationCode { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class DadosDeParcelaDTO
    {
        public ItemPedidoDTO itemPedido {get; set;}
        public ContratoDTO contrato {get; set;}
        public PedidoPagamentoDTO pedidoPagamento {get; set;}
        public PropostaItemDTO propostaItem { get; set; }
        
        public int numeroDaParcela {get; set;}
        
        public bool parcelaZerada { get; set; }
        public bool paga { get; set; }
        public bool? entrada { get; set; }
        public bool alocAutomatica { get; set; }
        public int? tipoPagamento { get; set; }
        public int? empId { get; set; }
        public int? PGT_ID { get; set; }
        public int? iffId { get; set; }

        public DateTime? dataVencimento { get; set; }
        public DateTime? dataPagamento { get; set; }
        
        public decimal? valorParcela { get; set; }
        public decimal? valorPago { get; set; }

        public string CHAVE_TRANSACAO {get; set;}
        public string CODIGO_DE_BARRAS { get; set; }
        public string URL_BOLETO { get; set; }
        public string STATUS_TRANSACAO { get; set; }

        public bool PodeAlocar { get; set; }

    }
}

using COAD.FISCAL.Model.DTO.Enumerados;
using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class PedidoDTO
    {
        public PedidoDTO()
        {
            Items = new HashSet<PedidoItemDTO>();
            TipoOperacaoNota = TipoOperacaoEnum.VENDA;
            ChaveNotasReferenciadas = new List<string>();
        }

        public TipoOperacaoEnum TipoOperacaoNota { get; set; }
        public int? CodPedido { get; set; }
        public string NaturezaDaOperacao {
            get {
                switch (TipoOperacaoNota)
                {
                    case TipoOperacaoEnum.VENDA: return "venda";
                    case TipoOperacaoEnum.DEVOLUCAO: return "devolução";
                    case TipoOperacaoEnum.CONSIGNACAO: return "consignação";
                    case TipoOperacaoEnum.IMPORTACAO: return "importação";
                    case TipoOperacaoEnum.REMESSA: return "remessa";
                    case TipoOperacaoEnum.TRANSFERENCIA: return "transferência";
                    default: return "venda";
                }
            }
        }
        public DateTime? DataFaturamento { get; set; }
        public EmpresaDTO Empresa { get; set; }
        public ClienteDTO Cliente { get; set; }
        public TipoPagamentoEnum TipoPagamento { get; set; }
        //public IndPagEnum IndicacaoDePagamento { get; set; }
        public ICollection<string> ChaveNotasReferenciadas { get; set; }
        public ICollection<PedidoItemDTO> Items { get; set; }
        public string ObservacoesNotaFiscal { get; set; }

        public string Contrato { get; set; }
    }
}

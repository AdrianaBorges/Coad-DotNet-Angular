using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class PesquisaPedidoDTO
    {
        public PesquisaPedidoDTO()
        {
            exibirExcluidos = true;
        }

        public string assinatura { get; set; }
        public int? REP_ID { get; set; }
        public int? CLI_ID { get; set; }
        public int? CMP_ID { get; set; }
        public string nomeCliente { get; set; }
        public string cpfCnpjCliente { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
        public int? UEN_ID { get; set; }
        public int? RG_ID { get; set; }
        public bool exibirExcluidos { get; set; }
        public bool? aprovacaoPendente { get; set; }
        public int? PRT_ID { get; set; }
        public int? PPI_ID { get; set; }
        public int? PED_CRM_ID { get; set; }
        public int? IPE_ID { get; set; }
        public int? PST_ID { get; set; }
        public int? numeroNotaInicial { get; set; }
        public int? numeroNotaFinal { get; set; }
        public DateTime? dataFaturamentoInicial { get; set; }
        public DateTime? dataFaturamentoFinal { get; set; }
        public bool? semNotaFiscal { get; set; }
        public DateTime? dataPagamento { get; set; }
        public DateTime? grupoDataPedido { get; set; }
        public DateTime? grupoDataPedidoFaturamento { get; set; }
        public int? TNE_ID { get; set; }

    }
}

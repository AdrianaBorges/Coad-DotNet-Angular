using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ResumoPropostaDTO
    {
        public int? EmpId { get; set; }
        public string NomeCliente { get; set; }
        public int? CodigoProposta { get; set; }
        public int? CodigoItemProposta { get; set; }
        public string NomeDoProduto { get; set; }
        public int? Quantidade { get; set; }
        public decimal? ValorEntrada { get; set; }
        public decimal? ValorParcela { get; set; }
        public int? QuantidadeParcela { get; set; }
       
        public string TipoPagamentoDesc { get; set; }
        public TipoPagamentoDTO TipoPagamento { get; set; }
        public TipoPagamentoDTO TipoPagamentoEntrada { get; set; }
        public TipoPagamentoDTO TipoPagamentoParcela { get; set; }
        public int? TpgId { get; set; }
        public DateTime? DataDeVencimentoEntrada { get; set; }
        public DateTime? DataDeVencimentoParcela { get; set; }
        public string NomeRepresentante { get; set; }
        public int? RepId { get; set; }
        public string EmailRepresentante { get; set; }

        public decimal? Total { get; set; }

    }
}

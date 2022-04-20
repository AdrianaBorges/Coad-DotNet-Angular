using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ResumoParcelamentoDTO
    {
        public RegiaoTabelaPrecoDTO REGIAO_TABELA_PRECO { get; set; }
        public TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }
        public TipoPeriodoDTO TIPO_PERIODO { get; set; }
        public bool? PermitirParcelaCortesia { get; set; }
        public int? Parcela { get; set; }
        public decimal? ValorParcela { get; set; }
        public decimal? Total { get; set; }
        public decimal? PrecoUnitario { get; set; }
    }
}

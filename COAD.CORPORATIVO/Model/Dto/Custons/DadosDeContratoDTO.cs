using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class DadosDeContratoDTO
    {
        public decimal? ValorContrato { get; set; }
        public decimal? ValorParcela { get; set; }
        public decimal? ValorDeEntrada { get; set; }
        public decimal? ValorRestante { get; set; }
        public decimal? ValorBruto { get; set; }
        public decimal? ValorServico { get; set; }
        public decimal? ValorProduto { get; set; }

        public DateTime? DataFaturamento { get; set; }
        public int? NumeroParcelas { get; set; }
        public bool GerarNotaFiscal { get; set; }
        public short? DiaVencimentoVendaRecorrente { get; set; }
        public bool? Cortesia { get; set; }
        public string CarId { get; set; }
        public int? PeriodoMesBonus { get; set; }
    }
}

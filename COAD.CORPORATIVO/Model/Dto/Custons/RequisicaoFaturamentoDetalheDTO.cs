using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class RequisicaoFaturamentoDetalheDTO
    {
        public int? IpeId { get; set; }
        public bool GerarNotaFiscal { get; set; }
        public int? EmpId { get; set; }
        public DateTime? DataFaturamento { get; set; }
        public string NomeProduto { get; set; }
        public decimal? ValorPedido { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public bool BloqueiaGeracaoNota { get; set; }
        
    }
}

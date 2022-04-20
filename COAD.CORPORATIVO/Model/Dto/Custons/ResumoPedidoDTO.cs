using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ResumoPedidoDTO
    {
        public string NomeCliente { get; set; }
        public int? CodigoPedido { get; set; }
        public string NomeDoProduto { get; set; }
        public int? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? PorcentagemDesconto { get; set; }
        public decimal? ValorDeDesconto { get; set; }
        public InfoFaturaDTO InformacoesImposto { get; set; }
        public decimal? TotalBruto { get; set; }

        public decimal? Total { get; set; }

    }
}

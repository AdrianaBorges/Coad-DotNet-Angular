using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class CalculoRequestImpostoDTO
    {
        public CalculoRequestImpostoDTO()
        {
            qtd = 1;
            qtdParcelas = 1;
        }

        public int? CmpID { get; set; }
        public bool? EhServico { get; set; }
        public int? tipoCliId { get; set; }
        public IPedidoItem PedidoItem { get; set; }
        public int? rgId { get; set; }
        public decimal? valorUnitario { get; set; }
        public int? qtd { get; set; }
        public int? qtdParcelas { get; set; }

        public bool? empresaDoSimples { get; set; }
        public bool cemPorCentoFaturado { get; set; }
        public bool? sobreTotal { get; set; }
        public bool? arredondarParaBaixo { get; set; }
        public InfoFaturaDTO ImpostosParaSomar { get; set; }
        public InfoFaturaItemDTO ImpostosItmParaSomar { get; set; }
    }
}

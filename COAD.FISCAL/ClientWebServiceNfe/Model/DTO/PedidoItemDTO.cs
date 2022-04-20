using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class PedidoItemDTO
    {
        public PedidoItemDTO()
        {
            Parcelas = new HashSet<ParcelaDTO>();
        }

        public ProdutoDTO Produto { get; set; }
        public Servico Servico { get; set; }

        public decimal? QtdComercial { get; set; }
        public decimal? QtdTributavel { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? ValorTotal { get; set; }
        public ICollection<ParcelaDTO> Parcelas { get; set; }
    }
}

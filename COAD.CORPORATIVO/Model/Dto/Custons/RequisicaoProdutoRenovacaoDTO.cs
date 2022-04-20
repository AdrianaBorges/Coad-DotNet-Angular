using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class RequisicaoProdutoRenovacaoDTO
    {
        public int? ProId { get; set; }
        public string NumeroAssinatura { get; set; }
        public int? QtdConsultas { get; set; }
        public int? CmpId { get; set; }
        public decimal? ValorDaVenda { get; set; }
        public ProdutoComposicaoDTO ProdutoComposicao { get; set; }
    }
}

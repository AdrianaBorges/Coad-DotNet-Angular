using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce
{
    public class ProdutoEcommerceDTO
    {
        /// <summary>
        /// Valor Bruto calculado com as configurações de nota do pedido
        /// </summary>
        public decimal? ValorVenda { get; set; }
        public ProdutoComposicaoDTO ProdutoOriginal { get; set; }
        public InfoFaturaDTO infoFatura { get; set; }
    }
}

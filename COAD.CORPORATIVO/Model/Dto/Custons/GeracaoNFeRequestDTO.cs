using COAD.FISCAL.Model;
using COAD.SEGURANCA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class GeracaoNFeRequestDTO
    {
        public int? IpeId { get; set; }
        public ClienteDto cliente { get; set; }
        public ClienteEnderecoDto endereco { get; set; }
        public EmpresaModel empresa { get; set; }
        public ProdutoComposicaoDTO produtoComposicao { get; set; }
        public ProdutosDTO produto { get; set; }
        public ItemPedidoDTO itemPedido {get; set;}
        public int? cfop { get; set; }
        public string path { get; set; }
        public int? NFX_ID { get; set; }
        public DateTime? DataFaturamento { get; set; }
        public string CodContrato { get; set; }
        public TNFeInfNFeCobr cobranca { get; set; }
    }
}

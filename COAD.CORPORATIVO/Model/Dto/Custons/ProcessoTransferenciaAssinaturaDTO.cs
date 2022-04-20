using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ProcessoTransferenciaAssinaturaDTO
    {
        public string CodAssinaturaOrigem { get; set; }
        public string NovoCodAssinatura { get; set; }
        public string CodContrato { get; set; }
        public int? CodProduto { get; set; }
        public ProdutoComposicaoDTO ProdutoComposicao { get; set; }
        public int? AcrescimoNoMes { get; set; }
        public int? Mes { get; set; }
        public int? RepId { get; set; }
        public string Login { get; set; }
        public ItemPedidoDTO ItemPedido { get; set; }
        public string Observacoes { get; set; }
    }
}

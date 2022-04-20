using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class PedidosRetroativosSemNotaDTO
    {
        public int? CodigoItemPedido { get; set; }
        public int? CodigoPedido { get; set; }
        public string NomeProduto { get; set; }
        public int? CodigoCliente { get; set; }
        public string NomeCliente { get; set; }
        public string CNPJ_CPF_Cliente { get; set; }
        public string NomeRepresentante { get; set; }
        public string CarId { get; set; }
        public DateTime? DataEmissaoPedido { get; set; }
        public DateTime? DataFaturamento { get; set; }
        public decimal? TotalDoPedido { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public interface IPedido
    {
        int? CliId { get; }
        bool? EmpresaDoSimples { get; }
        bool? FaturadoCemPorCento { get; }
        ICollection<IPedidoItem> PedidoItens { get; }
     }
}

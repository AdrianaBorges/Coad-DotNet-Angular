using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class TiposNegocioPedidoDAO : RepositorioCorp<TIPO_PEDIDO>
    {
        public IQueryable<TIPO_PEDIDO> BuscarPedidosAtivos()
        {            
            var listaPedidosAtivo = (from x in db.TIPO_PEDIDO where x.TIPO_PED_ATIVO == 1 select x);
            return listaPedidosAtivo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;

namespace COAD.CORPORATIVO.Service
{
    public class TiposNegocioPedidoSRV : ServiceCorp<TIPO_PEDIDO>
    {
        public IQueryable<TIPO_PEDIDO> BuscarPedidosAtivos()
        {            
            return new TiposNegocioPedidoDAO().BuscarPedidosAtivos();
        }
    }
}

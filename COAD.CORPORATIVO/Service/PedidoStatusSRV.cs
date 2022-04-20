using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PST_ID")]
    public class PedidoStatusSRV : ServiceAdapter<PEDIDO_STATUS, PedidoStatusDTO, int>
    {
        private PedidoStatusDAO _dao = new PedidoStatusDAO();

        public PedidoStatusSRV()
        {
            SetDao(_dao);
        }

        public PedidoStatusSRV(PedidoStatusDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }
    
    }
}

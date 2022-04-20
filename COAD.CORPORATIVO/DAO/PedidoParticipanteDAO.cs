using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class PedidoParticipanteDAO : DAOAdapter<PEDIDO_PARTICIPANTE, PedidoParticipanteDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public PedidoParticipanteDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<PedidoParticipanteDTO> ListPedidoParticipanteByItemPedido(int? IPE_ID)
        {
            var query = GetDbSet().Where(x => x.IPE_ID == IPE_ID);
            return ToDTO(query);
        }

        public IList<PedidoParticipanteDTO> ListPedidoParticipanteByPropostaItem(int? PPI_ID)
        {
            var query = GetDbSet().Where(x => x.PPI_ID == PPI_ID);
            return ToDTO(query);
        }

    }
}

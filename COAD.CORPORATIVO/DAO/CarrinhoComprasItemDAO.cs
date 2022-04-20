

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.DAO
{
    public class CarrinhoComprasItemDAO : AbstractGenericDao<CARRINHO_COMPRAS_ITEM, CarrinhoComprasItemDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public CarrinhoComprasItemDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public ICollection<CarrinhoComprasItemDTO> ListarCarrinhoComprasItensDoCarrinho(int? crcId)
        {
            var query = (from carItm in db.CARRINHO_COMPRAS_ITEM
                         where
                            carItm.CRC_ID == crcId /*&&
                            carItm.DATA_CANCELAMENTO == null*/
                         orderby carItm.DATA_CRIACAO ascending
                         select carItm);

            return ToDTO(query);
        }

        
    }
}

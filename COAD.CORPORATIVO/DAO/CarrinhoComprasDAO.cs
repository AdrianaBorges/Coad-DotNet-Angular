

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
    public class CarrinhoComprasDAO : AbstractGenericDao<CARRINHO_COMPRAS, CarrinhoComprasDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public CarrinhoComprasDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public CarrinhoComprasDTO RetornarCarrinhoDoCliente(int? cliId)
        {
            var query = (from
                            car in db.CARRINHO_COMPRAS
                         where
                             car.CLI_ID == cliId /*&&
                             car.DATA_CANCELAMENTO == null &&
                             car.DATA_CONFIRMACAO == null*/
                         orderby car.DATA_CRIACAO descending
                         select car);
            
            return ToDTO(query.FirstOrDefault());
        }

        
    }
}

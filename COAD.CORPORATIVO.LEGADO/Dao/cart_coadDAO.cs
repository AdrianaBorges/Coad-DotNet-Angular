using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class cart_coadDAO : AbstractGenericDao<cart_coad, cart_coadDTO, object>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }
        public cart_coadDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }

        public cart_coadDTO BuscarPrimeiroCartCoadPorCnpjCpf(string cpfCnpj)
        {
            
            var cartCoad =
                (from cart_co in db.cart_coad
                 join cli in db.CLIENTES on cart_co.CODIGO equals cli.CODIGO
                 where cli.CGC == cpfCnpj
                 select cart_co);

            return ToDTO(cartCoad.FirstOrDefault());
        }

        public cart_coadDTO BuscarPrimeiroCartCoadPorCliId(int? CLI_ID)
        {
            var cartCoad =
                (from cart_co in db.cart_coad
                 where cart_co.CLI_ID == CLI_ID
                 select cart_co);

            return ToDTO(cartCoad.FirstOrDefault());
        }
    }
}

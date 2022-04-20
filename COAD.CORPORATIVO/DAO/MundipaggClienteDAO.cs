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
    public class MundipaggClienteDAO : DAOAdapter<MUNDIPAGG_CLIENTE, MundipaggClienteDTO, int>
    {

        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public MundipaggClienteDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public MundipaggClienteDTO BuscarMundipaggClientePorCoadCliId(int? coadCliId)
        {
            var query = (from mpg in db.MUNDIPAGG_CLIENTE
                                       where mpg.CLI_ID == coadCliId
                                       select mpg);

            var mundipaggClienteDTO = ToDTO(query.FirstOrDefault());

            return mundipaggClienteDTO;
        }

        public MundipaggClienteDTO BuscarMundipaggClientePorCoadCusId(string cusId)
        {

            var query = (from mpg in db.MUNDIPAGG_CLIENTE where mpg.MPG_CLI_CUS_ID == cusId select mpg);

            var mundipaggClienteDTO = ToDTO(query.FirstOrDefault());

            return mundipaggClienteDTO;
        }
    }
}

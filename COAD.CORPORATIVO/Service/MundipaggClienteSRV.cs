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
    [ServiceConfig("MPG_CLI_ID")]
    public class MundipaggClienteSRV : GenericService<MUNDIPAGG_CLIENTE, MundipaggClienteDTO, int>
    {
        public MundipaggClienteDAO _dao { get; set; }
        public MundipaggClienteSRV(MundipaggClienteDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public MundipaggClienteDTO BuscarMundipaggClientePorCliId(int? coadCliId)
        {
            try
            {
                var clienteMundipagg = _dao.BuscarMundipaggClientePorCoadCliId(coadCliId);
                return clienteMundipagg;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MundipaggClienteDTO BuscarMundipaggClientePorCusId(string cusId)
        {
            try
            {
                var clienteMundipagg = _dao.BuscarMundipaggClientePorCoadCusId(cusId);
                return clienteMundipagg;
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}

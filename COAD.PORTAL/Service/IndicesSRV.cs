using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO;
using COAD.PORTAL.Model.DTO;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.Service
{
    public class IndicesSRV: GenericService<INDICES_PORTAL_PROC_Result, IndicesDTO, string>
    {
        private IndicesDAO _dao = new IndicesDAO();

        public IndicesSRV()
        {
            Dao = _dao;           
        }


        public IList<IndicesDTO> Indices()
        {
            return _dao.Indices();
        }
    }
}

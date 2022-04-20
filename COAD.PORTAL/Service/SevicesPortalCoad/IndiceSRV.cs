using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.DAOPortalCoad;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.Service.SevicesPortalCoad
{
    public class IndiceSRV : GenericService<idc_agregado, IndiceDTO, string>
    {
        public IndiceDAO _dao = new IndiceDAO();

        public IndiceSRV()
        {
            Dao = _dao;
            //SetKeys("id");
            //this.Dao = _dao;
        }

    }
}

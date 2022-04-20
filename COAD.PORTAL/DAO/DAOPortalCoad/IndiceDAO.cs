using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.DAOPortalCoad
{
    public class IndiceDAO : AbstractGenericDao<idc_agregado, IndiceDTO, string>
    {
        private coadEntities db { get; set; }

        public IndiceDAO()
        {
            SetProfileName("portalCoad");
            //db = GetDb<COADEntities>(false);
        }
    }
}

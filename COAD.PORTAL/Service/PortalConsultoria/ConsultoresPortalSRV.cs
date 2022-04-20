using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.PortalConsultoria;
using COAD.PORTAL.Model.DTO.PortalConsultoria;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Service.PortalConsultoria
{
    public class ConsultoresPortalSRV : GenericService<consultores, ConsultoresPortalDTO, string>
    {
        public ConsultoresDAO _dao = new ConsultoresDAO();

        public ConsultoresPortalSRV()
        {
            //Dao = _dao;
            SetKeys("id");
            this.Dao = _dao;
        }

        public ConsultoresPortalDTO BuscarConsultorPorLogin(string login)
        {
            return _dao.BuscarConsultorPorLogin(login);
        }

        public ConsultoresPortalDTO BuscarConsultorPorLoginEColecionador(string login, string colec)
        {
            return _dao.BuscarConsultorPorLoginEColecionador(login, colec);
        }
    }
}

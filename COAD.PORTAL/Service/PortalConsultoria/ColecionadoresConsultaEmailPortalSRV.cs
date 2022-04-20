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
    public class ColecionadoresConsultaEmailPortalSRV : GenericService<colecionadores, ColecionadoresConsultaEmailPortalDTO, string>
    {
        public ColecionadoresConsultaEmailPortalDAO _dao = new ColecionadoresConsultaEmailPortalDAO();

        public ColecionadoresConsultaEmailPortalSRV()
        {
            //Dao = _dao;
            //SetKeys("id");
            this.Dao = _dao;
        }

        public ColecionadoresConsultaEmailPortalDTO BuscarColecionadorPorPerfil(string perfil)
        {
            return _dao.BuscarColecionadorPorPerfil(perfil);
        }
    }
}

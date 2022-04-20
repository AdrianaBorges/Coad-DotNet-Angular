using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Model.DTO.PortalConsultoria;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.DAO.PortalConsultoria
{
    public class ColecionadoresConsultaEmailPortalDAO : AbstractGenericDao<colecionadores, ColecionadoresConsultaEmailPortalDTO, string>
    {
        private consultoriaEntities db { get; set; }

        public ColecionadoresConsultaEmailPortalDAO()
        {
            SetProfileName("portalConsultoria");
            //db = GetDb<COADEntities>(false);
        }

        public ColecionadoresConsultaEmailPortalDTO BuscarColecionadorPorPerfil(string perfil)
        {
            colecionadores query = query = GetDbSet().Where(x => x.menu_coadcorp.Equals(perfil)).FirstOrDefault();
            return ToDTO(query);
        }
    }
}

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
    public class ConsultoresDAO : AbstractGenericDao<consultores, ConsultoresPortalDTO, string>
    {
        private consultoriaEntities db { get; set; }

        public ConsultoresDAO()
        {
            SetProfileName("portalConsultoria");
            db = GetDb<consultoriaEntities>(false);
        }

        public ConsultoresPortalDTO BuscarConsultorPorLogin(string login)
        {
            var _query = (from consultor in db.consultores where consultor.usuario.Equals(login) select consultor).FirstOrDefault();
            return ToDTO(_query);
        }

        public ConsultoresPortalDTO BuscarConsultorPorLoginEColecionador(string login, string colec)
        {
            var _query = (from consultor in db.consultores where consultor.usuario.Equals(login) && consultor.colec.Equals(colec) select consultor).FirstOrDefault();
            return ToDTO(_query);
        }
    }
}

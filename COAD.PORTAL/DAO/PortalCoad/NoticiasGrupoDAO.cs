using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.PortalCoad
{
    public class NoticiasGrupoDAO : AbstractGenericDao<noticias_grupos, NoticiasGrupoDTO, int>
    {
        private coadEntities db { get; set; }

        public NoticiasGrupoDAO()
        {
            SetProfileName("portalCoad");
            //db = GetDb<COADEntities>(false);
        }

        public NoticiasGrupoDTO BuscarPorTipoEID(int id, int idTipo)
        {
            noticias_grupos query = GetDbSet().Where(x => x.id == id && x.id_tipo == idTipo).FirstOrDefault();
            return ToDTO(query);
        }
    }
}

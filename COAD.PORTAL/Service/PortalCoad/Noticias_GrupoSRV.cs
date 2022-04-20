using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.PortalCoad;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.Service.PortalCoad
{
    public class Noticias_GrupoSRV : GenericService<noticias_grupos, NoticiasGrupoDTO, int>
    {
        private NoticiasGrupoDAO _dao = new NoticiasGrupoDAO();

        public Noticias_GrupoSRV()
        {
            Dao = _dao;
        }

        public NoticiasGrupoDTO BuscarPorTipoEID(int id, int idTipo)
        {
            return _dao.BuscarPorTipoEID(id, idTipo);
        }
    }
}

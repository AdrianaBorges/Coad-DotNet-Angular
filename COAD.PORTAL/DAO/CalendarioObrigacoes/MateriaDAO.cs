using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.CalendarioObrigacoes
{
    public class MateriaDAO : AbstractGenericDao<BUSCAR_MATERIA_POR_ID_PORTAL_PROC_Result, MateriaDTO, string>
    {
        private static COADIARIOEntities db { get; set; }

        public MateriaDAO()
        {
            SetProfileName("portal");
            db = GetDb<COADIARIOEntities>(false);
        }

        public MateriaDTO Materia(string id)
        {
            BUSCAR_MATERIA_POR_ID_PORTAL_PROC_Result materia = db.BUSCAR_MATERIA_POR_ID_PORTAL_PROC(id).FirstOrDefault();
            return ToDTO(materia);
        }
    }
}

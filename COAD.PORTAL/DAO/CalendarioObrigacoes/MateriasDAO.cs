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
    public class MateriasDAO : AbstractGenericDao<BUSCAR_MATERIA_PORTAL_PROC_Result, MateriasDTO, string>
    {
        private static COADIARIOEntities db { get; set; }

        public MateriasDAO()
        {
            SetProfileName("portal");
            db = GetDb<COADIARIOEntities>(false);
        }

        public Pagina<MateriasDTO> Materias(string label, string tipo, string num_ato, string ano, int pagina = 1, int nLinha = 7)
        {
            IList<BUSCAR_MATERIA_PORTAL_PROC_Result> materias = db.BUSCAR_MATERIA_PORTAL_PROC(label, tipo, num_ato, ano).ToList();
            return ToDTOPage(materias, pagina, nLinha);
        }
    }
}

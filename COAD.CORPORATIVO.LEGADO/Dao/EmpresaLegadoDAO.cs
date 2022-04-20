using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class EmpresaLegadoDAO : AbstractGenericDao<empresas, EmpresaLegadoDTO, int>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }
        
        public EmpresaLegadoDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }

        public EmpresaLegadoDTO FindById(int? id)
        {
            var query = (from em in db.empresas where em.id == id select em).FirstOrDefault();
            return ToDTO(query);
        }
    
    }
}

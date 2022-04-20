using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COAD.SEGURANCA.DAO
{
    public class FuncionalidadeSistemaDAO : DAOAdapter<FUNCIONALIDADE_SISTEMA, FuncionalidadeSistemaDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public FuncionalidadeSistemaDAO()
        {
            SetProfileName("coadsys");
            db = GetDb<COADSYSEntities>();
        }
        
    }
}

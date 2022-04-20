using Coad.GenericCrud.Dao.Base;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.SEGURANCA.DAO
{
   public class ParametroGrupoDAO : AbstractGenericDao<PARAMETRO_GRUPO, ParametroGrupoDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public ParametroGrupoDAO()
        {
            SetProfileName("coadsys");
            db = GetDb<COADSYSEntities>();
        }
    }
}
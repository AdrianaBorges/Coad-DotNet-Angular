

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.RM.Repositorios.Base;
using COAD.RM.Model.Dto;
using COAD.RM.Repositorios.Contexto;

namespace COAD.RM.DAO
{
    public class PfuncDAO : AbstractGenericDao<PFUNC, PfuncDTO, object>
    {
        public CorporeRMEntities db { get { return GetDb<CorporeRMEntities>(); } set { } }

        public PfuncDAO()
        {
            this.SetProfileName("rm");
            db = GetDb<CorporeRMEntities>(false);
        }
        
    }
}

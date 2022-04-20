using Coad.GenericCrud.Dao.Base;
using COAD.PROSPECTADOS.Model.Dto;
using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Dao
{
    public class ProspectsDAO : AbstractGenericDao<prospects, ProspectsDTO, string>
    {
        public prospectadosEntities db { get { return GetDb<prospectadosEntities>(); } set { } }

        public ProspectsDAO()
        {
            SetProfileName("prospectados");
            db = GetDb<prospectadosEntities>(false);
        }

    }
}

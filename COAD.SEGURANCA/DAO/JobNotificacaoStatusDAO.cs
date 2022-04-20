

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.SEGURANCA.DAO
{
    public class JobNotificacaoStatusDAO : AbstractGenericDao<JOB_NOTIFICACAO_STATUS, JobNotificacaoStatusDTO, Int32>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public JobNotificacaoStatusDAO()
        {
            SetProfileName("coadsys");
            db = GetDb<COADSYSEntities>(false);
        }

        
    }
}

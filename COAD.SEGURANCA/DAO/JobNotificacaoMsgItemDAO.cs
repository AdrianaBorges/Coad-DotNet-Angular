

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
    public class JobNotificacaoMsgItemDAO : AbstractGenericDao<JOB_NOTIFICACAO_MSG_ITEM, JobNotificacaoMsgItemDTO, Int32>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public JobNotificacaoMsgItemDAO()
        {
            SetProfileName("coadsys");
            db = GetDb<COADSYSEntities>(false);
        }

        public IList<JobNotificacaoMsgItemDTO> ListarJobNotificacaoMsgItem(int? jnf)
        {
            var query = (from
                            jNtMsg in db.JOB_NOTIFICACAO_MSG_ITEM
                         where
                             jNtMsg.JNF_ID == jnf
                         select jNtMsg);
            return ToDTO(query);
        } 
    }
}

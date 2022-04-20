

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System.Data.Objects;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COAD.SEGURANCA.DAO
{
    public class JobNotificacaoDAO : AbstractGenericDao<JOB_NOTIFICACAO, JobNotificacaoDTO, Int32>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public JobNotificacaoDAO()
        {
            SetProfileName("coadsys");
            db = GetDb<COADSYSEntities>(false);
        }

        public IList<JobNotificacaoDTO> ListarNotificacaoPorJob(int? jobId)
        {
            var query = (from jobNf
                            in db.JOB_NOTIFICACAO
                         where jobNf.JOB_ID == jobId
                         select jobNf);
            return ToDTO(query);
        }

        public IList<JobNotificacaoDTO> ListarNotificacaoPendentePorJob(int? jobId)
        {
            var query = (from jobNf
                            in db.JOB_NOTIFICACAO
                         where 
                            jobNf.JOB_ID == jobId &&
                            jobNf.JNT_ID == 1
                         select jobNf);
            return ToDTO(query);
        }


        public Pagina<JobNotificacaoDTO> ListarNotificacaoAtivasPorJob(string usuario, int pagina = 1, int registrosPorPagina = 5)
        {
            var query = (from jobNf
                            in db.JOB_NOTIFICACAO
                         where                             
                            (jobNf.USU_LOGIN == usuario && jobNf.JNF_DATA_CANCELAMENTO == null) &&
                            (jobNf.JNF_DATA_CONCLUSAO == null || 
                                DateTime.Now <= EntityFunctions.AddMinutes(jobNf.JNF_DATA_CONCLUSAO, 5))
                         select jobNf);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        
    }
}

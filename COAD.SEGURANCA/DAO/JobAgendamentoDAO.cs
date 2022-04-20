
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
using COAD.CORPORATIVO.Model;
using COAD.SEGURANCA.Model.Dto;

namespace COAD.SEGURANCA.DAO
{
    public class JobAgendamentoDAO : DAOAdapter<JOB_AGENDAMENTO, JobAgendamentoDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public JobAgendamentoDAO()
        {
            SetProfileName("coadsys");
        }

        public Pagina<JobAgendamentoDTO> PesquisarJobAgendamento(int pagina = 1, int registrosPorPagina = 15)
        {
            var query = (from job in db.JOB_AGENDAMENTO
                         where 1 == 1
                         select job);

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public IList<JobAgendamentoDTO> PesquisarJobsParaExecucaoManual()
        {
            var query = (from job in db.JOB_AGENDAMENTO
                         where
                            job.JOB_ATIVADO == true &&
                            job.JOB_EXECUTAR_AGORA == true &&
                            (job.JOB_EXECUTANDO == null ||
                            job.JOB_EXECUTANDO == false)
                         select job);

            return ToDTO(query);
        }

        public IList<JobAgendamentoDTO> PesquisarJobsDesativados()
        {
            var query = (from job in db.JOB_AGENDAMENTO
                         where
                            job.JOB_ATIVADO == false || job.JOB_ATIVADO == null
                         select job);

            return ToDTO(query);
        }

        public IList<JobAgendamentoDTO> PesquisarJobsPendentesDeAtivacao()
        {
            var query = (from job in db.JOB_AGENDAMENTO
                         where
                            job.JOB_ATIVAR == true                            
                         select job);

            return ToDTO(query);
        }

        public Pagina<JobAgendamentoDTO> PesquisarJobsAtivos(int pagina = 1, int registrosPorPagina = 3)
        {
            var query = (from job in db.JOB_AGENDAMENTO
                         where 
                            job.JOB_ATIVADO == true &&
                            job.JOB_EXECUTANDO == true
                         select job);

            return ToDTOPage(query, pagina, registrosPorPagina);
        }
    }
}



using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Jobs.DataSource;
using GenericCrud.Models.Enumerados;

namespace COAD.SEGURANCA.Service
{ 
	[ServiceConfig("JNF_ID")]
	public class JobNotificacaoSRV : GenericService<JOB_NOTIFICACAO, JobNotificacaoDTO, Int32>
	{

        public JobNotificacaoDAO _dao { get; set; }

        public JobNotificacaoSRV(JobNotificacaoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public void RegistrarJobNotificacao(JobNotificacaoRequestDTO jobNotify)
        {
            var jobNtf = new JobNotificacaoDTO()
            {
                JOB_ID = jobNotify.jobID,
                JNF_COD_REF = jobNotify.codRef,
                JNF_COD_STR_REF = jobNotify.codRefStr,
                JNF_DESCRICAO = jobNotify.descricao,
                USU_LOGIN = jobNotify.usuario,
                REP_ID = jobNotify.repId,
                JNF_DATA = DateTime.Now,
                JNT_ID = 1
            };

            Save(jobNtf);
        }

        public void ConcluirJobNotificacao(int? jnfId)
        {
            if(jnfId != null)
            {
                var jobNotif = FindById(jnfId);

                if(jobNotif != null)
                {
                    jobNotif.JNF_DATA_CONCLUSAO = DateTime.Now;
                    Merge(jobNotif);
                }
            }
        }

        public void CancelarJobNotificacao(int? jnfId)
        {
            if (jnfId != null)
            {
                var jobNotif = FindById(jnfId);

                if (jobNotif != null)
                {
                    jobNotif.JNF_DATA_CANCELAMENTO = DateTime.Now;
                    Merge(jobNotif);
                }
            }
        }

        public IList<JobNotificacaoDTO> ListarNotificacaoPorJob(int? jobId)
        {
            return _dao.ListarNotificacaoPorJob(jobId);
        }

        public IList<JobNotificacaoDTO> ListarNotificacaoPendentePorJob(int? jobId)
        {
            return _dao.ListarNotificacaoPendentePorJob(jobId);
        }

        public Pagina<JobNotificacaoDTO> ListarNotificacaoAtivasPorJob(string usuario, int pagina = 1, int registrosPorPagina = 5)
        {
            return _dao.ListarNotificacaoAtivasPorJob(usuario, pagina, registrosPorPagina);
        }

        public void ProcessarItensNotificacaoJob(int? jobId)
        {
            if (JobsRegister.Jobs != null && JobsRegister.Jobs.Count > 0)
            {
                var job = JobsRegister.Jobs.Where(x => x.Id == jobId).FirstOrDefault();

                if(job != null && job.NotifyHandler != null)
                {
                    var lstNotifyJob = ListarNotificacaoPendentePorJob(jobId);
                    if(lstNotifyJob != null)
                    {
                        foreach(var ntF in lstNotifyJob)
                        {
                            if(ntF.JNF_COD_REF != null)
                            {
                                var result = job.NotifyHandler.GetJobStatus(ntF.JNF_COD_REF);
                                
                                if(result.JobStatus == JobStatusEnum.PENDENTE)
                                {
                                    ntF.JNT_ID = 1;
                                }
                                else
                                if(result.JobStatus == JobStatusEnum.CONCLUIDO)
                                {
                                    ntF.JNT_ID = 2;
                                    ntF.JNF_DATA_CONCLUSAO = DateTime.Now;
                                }
                                else
                                if(result.JobStatus == JobStatusEnum.ERRO)
                                {
                                    ntF.JNT_ID = 3;
                                    ntF.JNF_ULTIMO_ERRO = result.ErroMsg;
                                }
                            }
                        }

                        SaveOrUpdateAll(lstNotifyJob);
                    }
                }
            };
        }
    }
}

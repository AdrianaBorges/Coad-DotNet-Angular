using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.SEGURANCA.DAO;
using COAD.CORPORATIVO.Model;
using COAD.SEGURANCA.Model.Dto;
using GenericCrud.Util;
using System.Transactions;
using System.ServiceProcess;
using Quartz.Impl;
using Quartz;
using GenericCrud.Models;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("JOB_ID")]
    public class JobAgendamentoSRV : GenericService<JOB_AGENDAMENTO, JobAgendamentoDTO, int>
    {
        private JobAgendamentoDAO _dao { get; set; }

        public JobAgendamentoSRV(JobAgendamentoDAO _dao) 
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public JobAgendamentoSRV()
        {
            this._dao = new JobAgendamentoDAO();
            this.Dao = _dao;
        }


        public bool ChecarJobAtivo(int? jobId)
        {
            var ativo = false;
                var job = FindById(jobId);

                if (job != null)
                    ativo = (job.JOB_ATIVADO == true);
            return ativo;
        }

        public bool ChecarJobExecutando(int? jobId)
        {
            var executando = false;
            var job = FindById(jobId);

                if (job != null)
                    executando = (job.JOB_EXECUTANDO == true);
            return executando;
        }

        public void MarcarInicioExecucao(int? jobId)
        {
            var job = FindById(jobId);

            if (job != null && job.JOB_ATIVADO == true)
            {
                job.JOB_EXECUTANDO = true;
                job.JOB_QTD_FALHA = 0;
                job.JOB_QTD_SUCESSO = 0;
                job.JOB_INICIO_EXECUCAO = DateTime.Now;
                Merge(job);
            }
            
        }

        public void MarcarFimExecucao(int? jobId)
        {
            var job = FindById(jobId);

            if (job != null && job.JOB_ATIVADO == true)
            {
                job.JOB_EXECUTANDO = false;
                job.JOB_DATA_ULTIMA_EXECUCAO = DateTime.Now;
                job.JOB_EXECUTAR_AGORA = false;
                Merge(job);
            }
        }

        public string EmailJobAgendamento(int? jobId)
        {
            var job = FindById(jobId);

            if (job != null)
            {
                var email = job.JOB_EMAIL_ENVIO;
                if (!string.IsNullOrWhiteSpace(email))
                {
                    return email;
                }
            }
            return SysUtils.RetornaEmailDeTeste();
        }

        public void TerminarExecucao()
        {
            using (var scope = new TransactionScope())
            {
                var lstJobs = FindAll();

                foreach (var job in lstJobs)
                {
                    job.JOB_EXECUTANDO = false;
                    job.JOB_COD_EXE_REF = null;
                    job.JOB_COD_STR_EXE_REF = null;
                }

                MergeAll(lstJobs);
                scope.Complete();
            }
        }

        public void LigarDesligarJob(int? jobId)
        {
            var job = FindById(jobId);
            if(job != null)
            {
                if (job.JOB_ATIVADO == null)
                    job.JOB_ATIVADO = false;
                job.JOB_ATIVADO = !job.JOB_ATIVADO;

                if (job.JOB_ATIVADO == true)
                    job.JOB_ATIVAR = true;
                Merge(job);
            }
        }

        public Pagina<JobAgendamentoDTO> PesquisarJobAgendamento(int pagina = 1, int registrosPorPagina = 15)
        {
            return _dao.PesquisarJobAgendamento(pagina, registrosPorPagina);
        }

        public IList<JobAgendamentoDTO> PesquisarJobsParaExecucaoManual()
        {
            var lstJobs = _dao.PesquisarJobsParaExecucaoManual();
            return lstJobs;
        }

        public IList<JobAgendamentoDTO> PesquisarJobsDesativados()
        {
            return _dao.PesquisarJobsDesativados();
        }

        public void ExecutarJobManualmente(IScheduler scheduler)
        {     
            var lstJobs = PesquisarJobsParaExecucaoManual();

            if (scheduler != null && lstJobs != null)
            {
                foreach (var job in lstJobs)
                {
                    var jobDetail = scheduler.GetJobDetail(new JobKey(job.JOB_ID.ToString()));

                    if (jobDetail != null)
                    {
                        var tempJob = jobDetail
                            .GetJobBuilder()
                            .WithDescription(jobDetail.Description)
                            .StoreDurably(false)
                            .WithIdentity("temp_$[" + jobDetail.Key.Name + "]")
                            .Build();

                        scheduler.ScheduleJob(tempJob, TriggerBuilder.Create()
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(2)).Build());
                    }

                }
            }

        }

        public void PausarJob(IScheduler scheduler)
        {
            var lstJobs = PesquisarJobsDesativados();

            if (scheduler != null && lstJobs != null)
            {
                foreach (var job in lstJobs)
                {
                    if (job != null)
                    {
                        scheduler.PauseJob(new JobKey(job.JOB_ID.ToString()));
                    }
                }
            }

        }

        public void ReativarJob(IScheduler scheduler)
        {
            var lstJobs = PesquisarJobsPendentesDeAtivacao();

            if (scheduler != null && lstJobs != null)
            {
                foreach (var job in lstJobs)
                {
                    if (job != null)
                    {
                        scheduler.ResumeJob(new JobKey(job.JOB_ID.ToString()));
                        job.JOB_ATIVAR = false;
                        Merge(job);
                    }
                }
            }

        }

        public IList<JobAgendamentoDTO> PesquisarJobsPendentesDeAtivacao()
        {
            return _dao.PesquisarJobsPendentesDeAtivacao();
        }

        public void ExecutarManualmenteJob(int? jobId, int? codRef = null, string codRefStr = null)
        {
            if(jobId != null)
            {
                using (var scope = new TransactionScope())
                {
                    var job = FindById(jobId);
                    job.JOB_EXECUTAR_AGORA = true;
                    job.JOB_COD_EXE_REF = codRef;
                    job.JOB_COD_EXE_REF_DESC = codRefStr;
                    Merge(job);

                    scope.Complete();
                }
            }
        }

        public void RegistrarPassoBatch(int? jobId, BatchStatus batchStatus)
        {
            if (jobId != null && batchStatus != null)
            {
                using (var scope = new TransactionScope())
                {
                    var job = FindById(jobId);
                    job.JOB_EXECUTAR_AGORA = true;
                    job.JOB_BATCH_NOME = batchStatus.BatchStepName;
                    job.JOB_BATCH_PROCESSED_ITENS = batchStatus.ProcessedItens;
                    job.JOB_BATCH_TOTAL_ITENS = batchStatus.TotalItens;
                    job.JOB_BATCH_PROGRESS = batchStatus.Progress;

                    Merge(job);

                    scope.Complete();
                }
            }
        }

        public void AdicionarContagemSucesso(int? jobId)
        {
            var jobAgen = FindById(jobId);
            if (jobAgen != null)
            {
                if (jobAgen.JOB_QTD_SUCESSO == null)
                    jobAgen.JOB_QTD_SUCESSO = 0;
                jobAgen.JOB_QTD_SUCESSO++;
                Merge(jobAgen);
            }
        }

        public void AdicionarContagemFalha(int? jobId)
        {
            var jobAgen = FindById(jobId);
            if (jobAgen != null)
            {
                if (jobAgen.JOB_QTD_FALHA == null)
                    jobAgen.JOB_QTD_FALHA = 0;
                jobAgen.JOB_QTD_FALHA++;
                Merge(jobAgen);
            }
        }

        public int? RetornarCodReferencia(int? jobId)
        {
            var jobAgen = FindById(jobId);
            if (jobAgen != null)
                return jobAgen.JOB_COD_EXE_REF;
            return null;
        }

        public string RetornarCodReferenciaStr(int? jobId)
        {
            var jobAgen = FindById(jobId);
            if (jobAgen != null)
                return jobAgen.JOB_COD_STR_EXE_REF;
            return null;
        }

        public Pagina<JobAgendamentoDTO> PesquisarJobsAtivos(int pagina = 1, int registrosPorPagina = 3)
        {
            return _dao.PesquisarJobsAtivos(pagina, registrosPorPagina);
        }
    }
}

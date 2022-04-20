using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Custons.Context;
using GenericCrud.Config.IOCContainer;
using GenericCrud.Models;
using GenericCrud.Service;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;

namespace COAD.SEGURANCA.Jobs
{
    public abstract class JobBase : IJob
    {
        
        public void Execute(IJobExecutionContext context)
        {
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            using (containerConfig.Container.BeginScope())
            {
                HistoricoExecucaoSRV _historicoExecucao = ServiceFactory.RetornarServico<HistoricoExecucaoSRV>();
                JobAgendamentoSRV jobAgendamentoSRV = ServiceFactory.RetornarServico<JobAgendamentoSRV>();
                JobNotificacaoSRV jobNotificacao = ServiceFactory.RetornarServico<JobNotificacaoSRV>();

                var type = GetType();
                var nomeClasse = type.FullName;
                var nomeAssembly = type.AssemblyQualifiedName;
                var nomeDoJob = context.JobDetail.Description;
                var idStr = context.JobDetail.Key.Name;
                idStr = new Regex(@"(temp_\$\[(\d*)\])").Replace(idStr, "$2");
                int id = 0;
                int.TryParse(idStr, out id);

                var ativo = jobAgendamentoSRV.ChecarJobAtivo(id);
                var executando = jobAgendamentoSRV.ChecarJobExecutando(id);

                var nomeServico = (!string.IsNullOrWhiteSpace(nomeDoJob)) ? nomeDoJob : nomeClasse;

                try
                {

                    if (ativo && !executando)
                    {
                        BatchContext contextoImportacao = new BatchContext(id, jobAgendamentoSRV);
                        contextoImportacao.IniciarPassoBatch("Iniciando Execução", true);
                        jobAgendamentoSRV.MarcarInicioExecucao(id);
                        Executar(context, contextoImportacao);

                    }
                }
                catch (Exception e)
                {
                    _historicoExecucao.Incluir("Job Scheduler", "Ocorreu uma falha na execução!", DateTime.Now, nomeServico, nomeAssembly, e);
                    throw new Exception("Ocorreu uma falha na execução do job", e);

                }
                finally
                {
                    jobAgendamentoSRV.MarcarFimExecucao(id);
                    jobNotificacao.ProcessarItensNotificacaoJob(id);
                }
                // Lógica Final
            }
        }

        public abstract void Executar(IJobExecutionContext context, BatchContext batchContext);
    }
}

using COAD.CORPORATIVO.Jobs;
using COAD.CORPORATIVO.Jobs.NotifyHandler;
using COAD.SEGURANCA.Jobs.Controles;
using COAD.SEGURANCA.Jobs.DataSource;
using COAD.SEGURANCA.Jobs.NotifyHandler;
using GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHEDULER.Jobs
{
    public static class SchedulerJobsRegister
    {
        public static void RegistrarJobs()
        {

            // Registro de Jobs
            JobsRegister.Jobs = new JobSchedulerDTO[]{

                    new JobSchedulerDTO(){
                        Id = 1,
                        segundos = 40,
                        IJobType = typeof(EmailJob),
                        Nome = "Envio de email.",
                        NotifyHandler = new EmailNotifyHandler()
                     },
                    new JobSchedulerDTO()
                    {
                        Id = 2,
                        minutos = 1,
                        IJobType = typeof(BaixaPropostaPedidoJob),
                        Nome = "Baixa de Proposta/Pedido"
                    },
                    new JobSchedulerDTO()
                    {
                        Id = 6,
                        cronExpression = @"0 0 8,12,17 ? * MON,TUE,WED,THU,FRI *", // as 8h, 12h e 17 nos dias de segunda à sexta
                        //minutos = 5,
                        IJobType = typeof(RelatorioPropostasAtrasadasJob),
                        Nome = "Relatório de Propostas Atrasadas"
                    },
                    new JobSchedulerDTO()
                    {
                        Id = 7,
                        segundos = 5,
                        IJobType = typeof(JobSchedulerExecucaoManual),
                        Nome = "Controlador de JOBS"
                    },
                    new JobSchedulerDTO()
                    {
                        Id = 8,
                        segundos = 10,
                        IJobType = typeof(ImportacaoSuspectJob),
                        Nome = "Importação de Suspects"
                    },
                    new JobSchedulerDTO()
                    {
                        Id = 9,
                        cronExpression = @"0 0 8,12,17 ? * MON,TUE,WED,THU,FRI *", // as 8h, 12h e 17 nos dias de segunda à sexta
                        //minutos = 5,
                        IJobType = typeof(NotificacaoPropostasAtrasadasRepreJob),
                        Nome = "Notificação Propostas Atrasadas Repre"
                    },
                    new JobSchedulerDTO()
                    {
                        Id = 10,
                        segundos = 40,
                        IJobType = typeof(EnvioLoteDeNotasJob),
                        Nome = "Envio de Lote",
                        NotifyHandler = new EnvioNfeNotifyHandler()
                    },
                    new JobSchedulerDTO()
                    {
                        Id = 11,
                        segundos = 20,
                        IJobType = typeof(ProcessamentoRetornoLoteJob),
                        Nome = "Processamento do Lote",
                        NotifyHandler = new ProcessRetornoNfeNotifyHandler()
                    },
                    new JobSchedulerDTO()
                    {
                        Id = 13,
                        segundos = 20,
                        IJobType = typeof(ProcessamentoRetornoLoteNfseJob),
                        Nome = "Processamento do Lote NFse",
                        NotifyHandler = new ProcessRetornoNfeNotifyHandler()
                    },
                    new JobSchedulerDTO()
                    {
                        Id = 14,
                        minutos = 60,
                        IJobType = typeof(EnvioBoletosJob),
                        Nome = "Envio Automático de Boletos"
                    },
                };
        }
    }
}
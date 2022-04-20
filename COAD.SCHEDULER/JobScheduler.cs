using COAD.CORPORATIVO.Jobs;
using GenericCrud.Models;
using GenericCrud.Service;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using GenericCrud.Exceptions;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Jobs;
using COAD.SEGURANCA.Jobs.Controles;
using COAD.SEGURANCA.Jobs.DataSource;

namespace COAD.SCHEDULER
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public long dwServiceType;
        public ServiceState dwCurrentState;
        public long dwControlsAccepted;
        public long dwWin32ExitCode;
        public long dwServiceEspecificExitCode;
        public long dwCheckPoint;
        public long dwWaitHint;
    }

    public partial class JobScheduler : ServiceBase
    {
        public SchedulerSRV schedulerSRV { get; set; }
        
        public JobScheduler()
        {
            InitializeComponent();
            eventLog1 = new EventLog();

            if (!EventLog.SourceExists("BaseSchedulerSource"))
            {
                EventLog.CreateEventSource("BaseSchedulerSource", "Log2Scheduler");
            }

            eventLog1.Source = "BaseSchedulerSource";
            eventLog1.Log = "Log2Scheduler";
        }

        protected override void OnStart(string[] args)
        {
            Debugger.Launch();
            eventLog1.WriteEntry("Executando OnStart"); // executando log

            try
            {
                // lógica para mudança de status do serviço

                ServiceStatus serviceStatus = new ServiceStatus();
                serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING; // Alterando status para Inicialização pendente
                serviceStatus.dwWaitHint = 99000000000;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                eventLog1.WriteEntry("Configurando o Quartz...");
                
                    JobsRegister.Jobs = new JobSchedulerDTO[]{ 
                    
                    new JobSchedulerDTO(){
                        Id = 1,
                        segundos = 40,
                        IJobType = typeof(EmailJob),
                        Nome = "Envio de email."
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
                    //new JobSchedulerDTO()
                    //{
                    //    Id = 10,
                    //    segundos = 40,
                    //    IJobType = typeof(EnvioLoteDeNotasJob),
                    //    Nome = "Envio de Lote"
                    //},
                    //new JobSchedulerDTO()
                    //{
                    //    Id = 11,
                    //    segundos = 20,
                    //    IJobType = typeof(ProcessamentoRetornoLoteJob),
                    //    Nome = "Processamento do Lote"
                    //},
                };

                eventLog1.WriteEntry("Anexando o eventoLog para o serviço interno...");
                schedulerSRV.EventLog = eventLog1;

                eventLog1.WriteEntry("Startando o Quartz...");
                schedulerSRV.Start(JobsRegister.Jobs);

                serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING; // Alterando status para Rodando
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                eventLog1.WriteEntry("Serviço Iniciado com êxito...");
                
            }
            catch (Exception e)
            {
                var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                eventLog1.WriteEntry("Ocorreu um erro executar o inicio do serviço" + e, EventLogEntryType.Error);
                throw;
            }

        }

        protected override void OnStop()
        {
            try
            {
                eventLog1.WriteEntry("Executando OnStop");
                schedulerSRV.Stop();

                ServiceFactory.RetornarServico<JobAgendamentoSRV>().TerminarExecucao();
            }
            catch (Exception e)
            {
                var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                eventLog1.WriteEntry("Ocorreu um erro ao tentar parar o serviço: " + e, EventLogEntryType.Error);
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }
}

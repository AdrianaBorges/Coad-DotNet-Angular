using COAD.AGENDADOR.Config;
using GenericCrud.Exceptions;
using GenericCrud.Service;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SCHEDULER
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Debugger.Launch(); 
            EventLog
            eventLog = new EventLog();

            if (!EventLog.SourceExists("MainSchedulerSource"))
            {
                EventLog.CreateEventSource("MainSchedulerSource", "LogScheduler");
            }

            eventLog.Source = "MainSchedulerSource";
            eventLog.Log = "LogScheduler";

            try
            {
                AgendadorConfig.Configurar();
                JobScheduler job = ServiceFactory.RetornarServico<JobScheduler>();

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    job
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception e)
            {
                var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                eventLog.WriteEntry(string.Format("Ocorreu um erro ao tentar iniciar o serviço {0}", mensagem));

                throw new Exception("Ocorreu um erro ao tentar iniciar o serviço", e);
            }
        }
    }
}

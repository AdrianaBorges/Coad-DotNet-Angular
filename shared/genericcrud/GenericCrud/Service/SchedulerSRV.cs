using GenericCrud.Config.IOCContainer;
using GenericCrud.Exceptions;
using GenericCrud.Models;
using GenericCrud.Util;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service
{
    public class SchedulerSRV
    {
        public SchedulerSRV()
        {
            switch(SysUtils.RetornarAmbienteEnum())
            {
                case AmbienteEnum.Dev:
                    {
                        ServiceName = "JobSchedulerDEV";
                        HostName = "RJ-TI02a-DSK";
                        break;
                    }
                case AmbienteEnum.Homol:
                    {
                        ServiceName = "JobSchedulerHOMOL";
                        HostName = "10.228.5.10";
                        break;
                    }
                case AmbienteEnum.Prod:
                    {
                        ServiceName = "JobSchedulerPROD";
                        HostName = "10.228.5.10";
                        break;
                    }
            }
            
        }

        public EventLog EventLog { get; set; }
        public string ServiceName { get; set; }
        public string HostName { get; set; }

        public void RegistrarLog(string mensagem)
        {
            if (EventLog != null)
            {
                EventLog.WriteEntry(mensagem);
            }
        }

        public void RegistrarLogErro(string mensagem)
        {
            if (EventLog != null)
            {
                EventLog.WriteEntry(mensagem, EventLogEntryType.Error);
            }
        }

        public void Start(ICollection<JobSchedulerDTO> jobs)
        {
            try
            {
                RegistrarLog("Obtendo a instância do Quartz");
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();
                                
                if (jobs != null && jobs.Count > 0)
                {
                    foreach (var tipo in jobs)
                    {
                        IJobDetail job = tipo.GetJob();
                        var concurrent = job.ConcurrentExecutionDisallowed;
                        ITrigger trigger = tipo.CriarTrigger();
                        job.JobDataMap.Add("lstJobsDTO", jobs);
                        scheduler.ScheduleJob(job, trigger);
                    }
                }
            }
            catch (Exception e)
            {
                string mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                RegistrarLogErro(mensagem);
            }

        }

        public void Stop()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            IOCContainerProxy.StaticDispose();
            scheduler.Shutdown();
        }

        public void StartService()
        {
            ServiceController serviceController = new ServiceController(ServiceName, HostName);
            serviceController.Start();
            serviceController.Close();
        }

        public void StopService()
        {
            ServiceController serviceController = new ServiceController(ServiceName, HostName);
            serviceController.Stop();
            serviceController.Close();
        }
    }
}
    
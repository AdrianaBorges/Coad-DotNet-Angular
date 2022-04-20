using GenericCrud.Models.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    public class JobSchedulerDTO
    {
        public int? segundos { get; set; }
        public int? minutos { get; set; }
        public int? horas { get; set; }

        public int? dias { get; set; }
        public int? meses { get; set; }

        public string Nome { get; set; }
        public int? Id { get; set; }
        public string cronExpression { get; set; }
        public Type IJobType { get; set; }

        public IJobNotifyHandler NotifyHandler { get; set; }

        public ITrigger CriarTrigger()
        {
            if (segundos != null && minutos == null && horas == null && dias == null && meses == null)
            {
                ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(x => 
                    x.WithIntervalInSeconds((int)segundos).RepeatForever())
                    .Build();

                return trigger;
            }

            if (segundos == null && minutos != null && horas == null && dias == null && meses == null)
            {
                ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(x =>
                    x.WithIntervalInMinutes((int)minutos).RepeatForever())
                    .Build();

                return trigger;
            }

            if (segundos == null && minutos == null && horas != null && dias == null && meses == null)
            {
                ITrigger trigger = TriggerBuilder.Create().WithSimpleSchedule(x =>
                    x.WithIntervalInHours((int)horas).RepeatForever())
                    .Build();

                return trigger;
            }

            if (!string.IsNullOrWhiteSpace(cronExpression))
            {
                ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(cronExpression)
                    .StartNow()
                    .Build();
                return trigger;
            }            

            return null;
        }

        public IJobDetail GetJob()
        {
            if (!string.IsNullOrWhiteSpace(Nome))
            {
                var jobDetail = JobBuilder.Create(IJobType)
                    .WithDescription(Nome);

                if(Id != null)
                {
                    jobDetail.WithIdentity(Id.ToString());
                }
                return jobDetail.Build();
            }
            return JobBuilder.Create(IJobType).Build();
        }
    }
}

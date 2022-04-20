using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Custons.Context;
using GenericCrud.Service;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Jobs.Controles
{
    [DisallowConcurrentExecution]
    public class JobSchedulerPararJob : JobBase
    {
        public override void Executar(IJobExecutionContext context, BatchContext batchContext)
        {
            var agendamentoSRV = ServiceFactory.RetornarServico<JobAgendamentoSRV>();

            if (agendamentoSRV != null)
            {
                agendamentoSRV.PausarJob(context.Scheduler);
            }
        }
    }
}

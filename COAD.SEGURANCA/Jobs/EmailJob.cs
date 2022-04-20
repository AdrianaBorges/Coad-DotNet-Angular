using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Jobs;
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

namespace COAD.CORPORATIVO.Jobs
{
    [DisallowConcurrentExecution]
    public class EmailJob : JobBase
    {
        public override void Executar(IJobExecutionContext context, BatchContext batchContext)
        {
            var filaEmailSRV = ServiceFactory.RetornarServico<FilaEmailSRV>();

            if (filaEmailSRV != null)
            {
                filaEmailSRV.ProcessarFilaDeEnvio(batchContext);
            }
        }
    }
}

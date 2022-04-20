using COAD.SEGURANCA.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using GenericCrud.Service;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.FISCAL.Service.Integracoes;
using GenericCrud.Util;

namespace COAD.CORPORATIVO.Jobs
{

    [DisallowConcurrentExecution]
    public class ProcessamentoRetornoLoteNfseJob : JobBase
    {
        public override void Executar(IJobExecutionContext context, BatchContext batchContext)
        {
            var inegrNFeSRV = ServiceFactory.RetornarServico<IntegrNfeSRV>();

            if (inegrNFeSRV != null)
            {
                inegrNFeSRV.defaultPath = SysUtils.DefaultPath;

                if (inegrNFeSRV.LoteSRV != null)
                    inegrNFeSRV.LoteSRV.DefaultPath = SysUtils.DefaultPath;
                inegrNFeSRV.ExecutarTarefaProcessarRetornoLoteNfse(batchContext);
            }
        }
    }
}

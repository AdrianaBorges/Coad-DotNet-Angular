using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Jobs;
using COAD.SEGURANCA.Service.Custons.Context;
using GenericCrud.Service;
using Quartz;

namespace COAD.CORPORATIVO.Jobs
{
    // BORGES - envio de boleto de cobrança

    [DisallowConcurrentExecution]
    public class EnvioBoletosJob : JobBase
    {
        public override void Executar(IJobExecutionContext context, BatchContext batchContext)
        {
            var parcelasSRV = ServiceFactory.RetornarServico<ParcelasSRV>();

            if (parcelasSRV != null)
            {
                parcelasSRV.EnviarBoletosEmAberto(batchContext);
            }
        }
    }
}

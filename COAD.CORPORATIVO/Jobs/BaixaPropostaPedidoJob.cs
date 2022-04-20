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

namespace COAD.CORPORATIVO.Jobs
{
    public class BaixaPropostaPedidoJob : JobBase
    {
        public override void Executar(IJobExecutionContext context, BatchContext batchContext)
        {
            var parcelasSRV = ServiceFactory.RetornarServico<ParcelasSRV>();

            if (parcelasSRV != null)
            {
                parcelasSRV.ProcessarBaixaPropostaPedido(false, batchContext);
            }
        }
    }
}

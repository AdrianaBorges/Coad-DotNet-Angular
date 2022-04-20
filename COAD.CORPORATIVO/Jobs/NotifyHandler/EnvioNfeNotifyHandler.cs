using GenericCrud.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCrud.Models;
using GenericCrud.Models.Enumerados;
using GenericCrud.Service;
using COAD.FISCAL.Service.Integracoes;
using COAD.FISCAL.Model.Integracoes.Enumerados;

namespace COAD.CORPORATIVO.Jobs.NotifyHandler
{
    public class EnvioNfeNotifyHandler : IJobNotifyHandler
    {
        public NotifyHandleResult GetJobStatus(int? codRef)
        {

            var result = new NotifyHandleResult();
            result.JobStatus = JobStatusEnum.PENDENTE;
            if (codRef != null)
            {
                var lote = ServiceFactory.RetornarServico<IntegrNfeSRV>().LoteSRV.RetornarLote(codRef);
                if (lote != null)
                {
                    if (lote.Status == StatusLoteEnum.LOTE_ENVIADO_NAO_PROCESSADO)
                    {
                        result.JobStatus = JobStatusEnum.CONCLUIDO;
                        return result;
                    }
                    else                    
                    if (lote.Status == StatusLoteEnum.ERRO_AO_PROCESSAR)
                    {
                        result.JobStatus = JobStatusEnum.ERRO;
                        result.ErroMsg = string.Format("Erro do sistema {0}. Mensagem de retorno: Código: {1}. Motivo {2}", lote.MsgErroSistema, lote.CodRetorno, lote.MensagemRetorno);
                        return result;
                    }
                    
                }
            }
            return result;
        }

        public NotifyHandleResult GetJobStatus(string codRefStr)
        {
            throw new NotImplementedException();
        }
    }
}

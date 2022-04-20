using GenericCrud.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCrud.Models.Enumerados;
using GenericCrud.Service;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Service;
using GenericCrud.Models;

namespace COAD.SEGURANCA.Jobs.NotifyHandler
{
    public class EmailNotifyHandler : IJobNotifyHandler
    {
        public NotifyHandleResult GetJobStatus(int? codRef)
        {
            var result = new NotifyHandleResult();
            result.JobStatus = JobStatusEnum.PENDENTE;
            if(codRef != null)
            {
                var filaEmail = ServiceFactory.RetornarServico<FilaEmailSRV>().FindById(codRef);
                var notificacaoSistema = ServiceFactory.RetornarServico<NotificacaoSistemaSRV>();
                if (filaEmail != null)
                {
                    if(filaEmail.FLE_DATA_ENVIO != null)
                    {
                        result.JobStatus = JobStatusEnum.CONCLUIDO;
                        return result;
                    }
                    else
                    {
                        var nof = notificacaoSistema.RetornarNotificacoesSistema(3, codRef);
                        if (nof != null && nof.Count > 0)
                        {
                            result.JobStatus = JobStatusEnum.ERRO;
                            result.ErroMsg = nof.FirstOrDefault().NTS_ERRO_DESCRICAO;

                            return result;
                        }
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

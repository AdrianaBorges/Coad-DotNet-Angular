using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Model.Dto.Custons;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCAO.Util;
using GenericCrud.Exceptions;
using GenericCrud.Models;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Service.Custons
{
    public class BatchCustomSRV
    {
        public HistoricoExecucaoSRV _serviceHist { get; set; }
        public NotificacaoSistemaSRV _notificacaoSistema { get; set; }

        public void AdicionarStatusDeBatchImportacaoSuspect(string sessionId, BatchStatus batchStatus)
        {
            BatchUtil.AdicionarBatchStatus(batchStatus, sessionId, "importacaoSuspect");
        }

        public void RemoverStatusDeBatchImportacaoSuspect(string sessionId)
        {
            BatchUtil.LimparBatchStatus(sessionId, "importacaoSuspect");
        }

        public BatchStatus RetornarStatusDeBatchImportacaoSuspect(string sessionId)
        {
            var batchStatus = BatchUtil.RetornarBatchStatus(sessionId, "importacaoSuspect");

            if (batchStatus != null && batchStatus.IsRunning)
            {
                return batchStatus;
            }

            return null;
        }

        public void RegistrarErroBatch(RegistroErroBatchDTO data)
        {
            var historicoExecucaoDTO = new RegistroNotificacaoSistemaDTO()
            {
                codTipoJob = data.tipoJob,
                descricao = (!string.IsNullOrWhiteSpace(data.descricao)) ? data.descricao : "Ocorreu um falha na execução!",
                exception = data.e,
                nomeDaExecucao = data.nomeDaExecucao,
                nomeProjeto = data.servico,
                nomeServico = data.projeto,
                descricaoCodigoReferencia = data.descricaoCodigoReferencia,
                codReferenciaStr = data.codTipoJobStr,
                codReferencia = data.codReferencia,
                data = DateTime.Now
            };

            ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(historicoExecucaoDTO);

            if(data.batchEx != null)
            {
                if(data.contabilizarFalha)
                    ++data.batchEx.TotalFalha;

                Message message = Message.Fail("Erros de Validação");
                var exMensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(data.e, message);
                var lstValidacoes = new Dictionary<string, List<string>>();

                if(message != null)
                {
                    foreach(var men in message.subMessages)
                    {
                        if(men.ValidationErrors != null && men.ValidationErrors.Count() > 0)
                        {
                            lstValidacoes = lstValidacoes.Concat(men.ValidationErrors).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                }

                data.batchEx.ListErros.Add(new ErroReportItemDTO()
                {
                    Contexto = data.context,
                    Mensagem = exMensagem,
                    ValidationErrors = lstValidacoes
                });
            }
        }

        public void RegistrarNotificacaoErroBatch(RegistroErroBatchDTO data)
        {
            var historicoExecucaoDTO = new RegistroNotificacaoSistemaDTO()
            {
                codTipoJob = data.tipoJob,
                descricao = (!string.IsNullOrWhiteSpace(data.descricao)) ? data.descricao : "Ocorreu um falha na execução!",
                exception = data.e,
                nomeDaExecucao = data.nomeDaExecucao,
                nomeProjeto = data.servico,
                nomeServico = data.projeto,
                descricaoCodigoReferencia = data.descricaoCodigoReferencia,
                codReferenciaStr = data.codTipoJobStr,
                codReferencia = data.codReferencia,
                qtdOcorrenciaEnvioEmail = data.qtdOcorrenciaEnvioEmail,
                codTipoNotificacaoSistema = data.codTipoNotificacaoSistema,
                codNotificacaoSistemaOrigem = data.codNotificacaoOrigem,
                reporteErroEmail = data.reporteErroEmail,
                data = DateTime.Now,
                cc = data.ccEmail,
                cco = data.ccoEmail,
                enviarEmailErro = data.enviarEmailErro
            };

            _notificacaoSistema.Incluir(historicoExecucaoDTO);

            if (data.batchEx != null)
            {
                if (data.contabilizarFalha)
                    ++data.batchEx.TotalFalha;

                Message message = Message.Fail("Erros de Validação");
                var exMensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(data.e, message);
                var lstValidacoes = new Dictionary<string, List<string>>();

                if (message != null)
                {
                    foreach (var men in message.subMessages)
                    {
                        if (men.ValidationErrors != null && men.ValidationErrors.Count() > 0)
                        {
                            lstValidacoes = lstValidacoes.Concat(men.ValidationErrors).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                }

                data.batchEx.ListErros.Add(new ErroReportItemDTO()
                {
                    Contexto = data.context,
                    Mensagem = exMensagem,
                    ValidationErrors = lstValidacoes
                });
            }
        }
    }
}

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
using COAD.FISCAL.Model.Integracoes.Interfaces;

namespace COAD.CORPORATIVO.Jobs.NotifyHandler
{
    public class ProcessRetornoNfeNotifyHandler : IJobNotifyHandler
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
                    if (lote.Status == StatusLoteEnum.PROCESSADA_COM_EXITO)
                    {
                        return result;
                    }
                    else                    
                    if (lote.Status == StatusLoteEnum.ERRO_AO_PROCESSAR)
                    {
                        result.JobStatus = JobStatusEnum.ERRO;
                        result.ErroMsg = string.Format("Erro do sistema {0}. Mensagem de retorno: Código: {1}. Motivo {2}", lote.MsgErroSistema, lote.CodRetorno, lote.MensagemRetornoProcessamento);
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

        public void ChecarItensDoLote(NotifyHandleResult result, INFeLote lote)
        {
            if(lote.Itens != null)
            {
                foreach(var loteItm in lote.Itens)
                {
                    if(loteItm.Status == StatusLoteItemEnum.REJEITADA || 
                        loteItm.Status == StatusLoteItemEnum.REJEITADA_E_INUTILIZADA)
                    {
                        if(result.JobStatus != JobStatusEnum.CONCLUIDO_COM_ERROS)
                            result.JobStatus = JobStatusEnum.CONCLUIDO_COM_ERROS;
                        
                        var mensagem = "A Nota Fiscal do item de Pedido {0} não foi autorizada: Código de erro {1}. Mensagem Retorno '{2}'";
                        mensagem = string.Format(mensagem, loteItm.CodPedido, loteItm.CodRetorno, loteItm.MensagemRetorno);

                        result.MsgItens.Add(new NotifyHandlerResultItem()
                        {
                            CodRef = loteItm.CodPedido,
                            Data = DateTime.Now,
                            Mensagem = mensagem
                        });
                    }
                    else 
                    if(loteItm.Status == StatusLoteItemEnum.AUTORIZADA)
                    {

                        if (!string.IsNullOrWhiteSpace(loteItm.MsgErroSistema))
                        {
                            result.JobStatus = JobStatusEnum.CONCLUIDO_COM_ERROS;
                            var mensagem = "A Nota Fiscal foi Autorizada. Mas o correu algum erro ao executar as tarefas pós autorização (Cadastrar a Nota, Enviar pro cliente, ETC). Erro: {0}";
                            mensagem = string.Format(mensagem, loteItm.MsgErroSistema);
                            result.MsgItens.Add(new NotifyHandlerResultItem()
                            {
                                CodRef = loteItm.CodPedido,
                                Data = DateTime.Now,
                                Mensagem = mensagem
                            });
                        }
                        else
                        {
                            result.JobStatus = JobStatusEnum.CONCLUINDO_OPERACAO;
                        }
                    }
                    else 
                    if(loteItm.Status == StatusLoteItemEnum.AUTORIZADA_E_ENVIADA)
                    {
                        result.JobStatus = JobStatusEnum.CONCLUIDO;
                    }
                }
            }

        }
    }
}

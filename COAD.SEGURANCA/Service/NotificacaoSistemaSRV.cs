using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.SEGURANCA.Model.Custons;
using Coad.GenericCrud.ActionResultTools;
using GenericCrud.Exceptions;
using COAD.SEGURANCA.Service;
using GenericCrud.Service;
using COAD.SEGURANCA.Model.Custons.FonteDadosTemplate;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service.Interfaces;
using GenericCrud.Util;
using System.Transactions;
using COAD.CRYPT;
using System.Security.Policy;
using System.Web;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("NTS_ID")]
    public class NotificacaoSistemaSRV : GenericService<NOTIFICACAO_SISTEMA, NotificacaoSistemaDTO, int>
    {
        public NotificacaoSistemaDAO _dao { get; set; }
        public HistoricoExecucaoSRV _historicoExecucaoSRV { get; set; }
        public CryptService cryptService { get; set; }

        public NotificacaoSistemaSRV()
        {
            this._dao = new NotificacaoSistemaDAO();
            this.Dao = _dao;
            this.cryptService = new CryptService();
        }

        public NotificacaoSistemaSRV(NotificacaoSistemaDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
            this.cryptService = new CryptService();
        }

        public NotificacaoSistemaDTO RetornarNotificacaoSistema(int? tnsId, int? codRefInt = null, string codRefStr = null, string descricaoEx = null)
        {
            return _dao.RetornarNotificacaoSistema(tnsId, codRefInt, codRefStr, descricaoEx);
        }

        public IList<NotificacaoSistemaDTO> RetornarNotificacoesSistema(int? tns_id, int? codRefInt = null, string codRefStr = null)
        {
            return _dao.RetornarNotificacoesSistema(tns_id, codRefInt, codRefStr);
        }

        public void Incluir(RegistroNotificacaoSistemaDTO regNot)
        {
            string nomeExcecao = null;
            string descricaoEx = null;
            string stackTrace = null;
            bool? erro = regNot.erro;
            int? codTipoNotificacao = regNot.codTipoNotificacaoSistema;
            int? codReferencia = regNot.codReferencia;
            string codReferenciaStr = regNot.codReferenciaStr;
            var e = regNot.exception;

            if(regNot.reporteErroEmail == true)
            {
                codTipoNotificacao = 4;
                codReferencia = null;
                codReferenciaStr = null;
            }   
            
            if (e != null)
            {
                nomeExcecao = e.GetType().FullName;
                //descricaoEx = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                stackTrace = e.StackTrace.ToString();
                regNot.erro = true;

                var message = Message.Fail("Ocorreu um erro de validação");
                descricaoEx = ExceptionFormatter.RecursiveFindExceptionsMessage(e, message);

                if (message != null)
                {
                    StringBuilder sb = new StringBuilder(" Erros de validação: ->");
                    foreach (var men in message.subMessages)
                    {
                        if (men.ValidationErrors != null && men.ValidationErrors.Count() > 0)
                        {
                            foreach (var key in men.ValidationErrors.Keys)
                            {
                                sb.Append("(");
                                sb.Append(key);
                                sb.Append(") ====> [ ");

                                int index = 0;
                                var list = men.ValidationErrors[key];
                                foreach (var validation in list)
                                {
                                    sb.Append(validation);
                                    if (index > 0)
                                        sb.Append(", ");
                                    index++;
                                }
                                sb.Append(" ] ");

                            }
                            descricaoEx += sb.ToString();
                            //lstValidacoes = lstValidacoes.Concat(men.ValidationErrors).ToDictionary(x => x.Key, x => x.Value);
                        }
                    }
                }
            }


            NotificacaoSistemaDTO notificacaoSistema = null;
            notificacaoSistema = RetornarNotificacaoSistema(codTipoNotificacao, codReferencia, codReferenciaStr /*, descricaoEx */);

            if (notificacaoSistema == null)
                notificacaoSistema = new NotificacaoSistemaDTO()
                {
                    NTS_DATA = (regNot.data != null) ? regNot.data : DateTime.Now,
                    NTS_NUMERO_CORRENCIA = 1,
                    NTS_SERVICO = regNot.nomeServico,
                    NTS_PROJETO = regNot.nomeProjeto,
                    NTS_COD_REF_INT = regNot.codReferencia,
                    TNS_ID = (regNot.reporteErroEmail == true) ? 4 : regNot.codTipoNotificacaoSistema,
                    NTS_DESCRICAO_COD_REF = regNot.descricaoCodigoReferencia,
                    NTS_COD_REF_STR = regNot.codReferenciaStr
                };
            else
            {
                if (notificacaoSistema.NTS_NUMERO_CORRENCIA == null)
                    notificacaoSistema.NTS_NUMERO_CORRENCIA = 0;
                notificacaoSistema.NTS_NUMERO_CORRENCIA++;

               
            }

            notificacaoSistema.NTS_DESCRICAO = regNot.descricao;
            notificacaoSistema.NTS_ERRO_NOME = nomeExcecao;
            notificacaoSistema.NTS_ERRO_DESCRICAO = descricaoEx;
            notificacaoSistema.NTS_STACK_TRACE = stackTrace;            

           var retorno = SaveOrUpdate(notificacaoSistema);
            if (notificacaoSistema.NTS_ID == null)
                notificacaoSistema.NTS_ID = retorno.NTS_ID;

            regNot.codNotificacaoSistema = notificacaoSistema.NTS_ID;
            _historicoExecucaoSRV.Incluir(regNot);

            if((regNot.reporteErroEmail == null || regNot.reporteErroEmail == false) &&
                (notificacaoSistema.NTS_ERRO_PAUSADO_ATE == null || 
                (notificacaoSistema.NTS_ERRO_PAUSADO_ATE != null && DateTime.Now.Date > notificacaoSistema.NTS_ERRO_PAUSADO_ATE.Value.Date)))
            {
                EnviarEmailDeNotificacaoFalhaNoJob(notificacaoSistema, regNot);
            }

        }

        /// <summary>
        /// Ao passar o tipo de notificação e informar uma lista de códigos de referencia, pesquisa e retorna os códigos que não possua nenhuma notificação sistema.
        /// </summary>
        /// <param name="tnsId"></param>
        /// <param name="lstCodRefId"></param>
        /// <returns></returns>
        public IList<int> RetornarCodRefenciasIntSemNotificacoesEmAberto(IList<int> lstCodRefId, int? tnsId)
        {
            return _dao.RetornarCodRefenciasIntSemNotificacoesEmAberto(lstCodRefId, tnsId);
        }

        /// <summary>
        /// Ao passar o tipo de notificação e informar uma lista de códigos de referencia, pesquisa e retorna os códigos que não possua nenhuma notificação sistema.
        /// </summary>
        /// <param name="tnsId"></param>
        /// <param name="lstCodRefStr"></param>
        /// <returns></returns>
        public IList<string> RetornarCodRefenciasStrSemNotificacoesEmAberto(IList<string> lstCodRefStr, int? tnsId)
        {
            return _dao.RetornarCodRefenciasStrSemNotificacoesEmAberto(lstCodRefStr, tnsId);
        }

        public void EnviarEmailDeNotificacaoFalhaNoJob(NotificacaoSistemaDTO notificacao, RegistroNotificacaoSistemaDTO registroNotificacao)
        {
            if (registroNotificacao.erro == true &&
                registroNotificacao.enviarEmailErro == true &&
                registroNotificacao.qtdOcorrenciaEnvioEmail != null &&
                   registroNotificacao.qtdOcorrenciaEnvioEmail > 0 && (notificacao.NTS_NUMERO_CORRENCIA == 1 ||
                   notificacao.NTS_NUMERO_CORRENCIA % registroNotificacao.qtdOcorrenciaEnvioEmail  == 0))
            {
                var detalhesEmailFalha = GerarDetalhesFonteDadosFalhaJob(registroNotificacao);

                var nomeJob = detalhesEmailFalha.NomeDoJob;
                var descricaoCodigo = detalhesEmailFalha.DescricaoCodigoReferencia;
                var cod = detalhesEmailFalha.CodReferencia;

                var assunto = string.Format("Ocorreu um erro ao tentar executar o job {0} de Código {1} [{2}]", nomeJob, cod, detalhesEmailFalha.Ambiente);
                var corpo = ServiceFactory.RetornarServico<TemplateHTMLSRV>().ProcessarTemplate(5, detalhesEmailFalha);

                ServiceFactory.RetornarServico<IEmailSRV>().EnviarEmail(new EmailRequestDTO() {
                    Assunto = assunto,
                    EmailDestino = @"desenvolvimentoti@coad.com.br",
                    CorpoEmail = corpo,
                    codNotificacaoSistema = registroNotificacao.codNotificacaoSistema,
                    reporteDeErro = true,
                    CC_ERRO = registroNotificacao.cc,
                    CCO_ERRO = registroNotificacao.cco
                });
              
            }
        }

        public DetalhesEmailFalhaJobDTO GerarDetalhesFonteDadosFalhaJob(RegistroNotificacaoSistemaDTO hist)
        {
            string descricaoEx = null;
            string stackTrace = null;
            
            if (hist.exception != null)
            {
                descricaoEx = ExceptionFormatter.RecursiveFindExceptionsMessage(hist.exception);
                stackTrace = hist.exception.StackTrace;
            }
            var codReferencia = (hist.codReferencia != null) ? hist.codReferencia.ToString() : hist.codReferenciaStr;
            var detalhesEmail = new DetalhesEmailFalhaJobDTO()
            {
                CodReferencia = codReferencia,
                NomeDoJob = hist.nomeDaExecucao,
                DataErro = hist.data,
                ErroDescricao = descricaoEx,
                StackTrace = stackTrace,
                DescricaoCodigoReferencia = hist.descricaoCodigoReferencia,
                Ambiente = SysUtils.RetornarAmbienteNameTotal(),
                HostName = SysUtils.RetornarHostName(),
                CodReferenciaHash =  !string.IsNullOrWhiteSpace(codReferencia) ? cryptService.CriptografarTripleDES(codReferencia) : null
            };

            if(HttpContext.Current != null)
            {
                detalhesEmail.CodReferenciaHash = HttpContext.Current.Server.UrlEncode(detalhesEmail.CodReferenciaHash);
            }

            return detalhesEmail;
        }

        public void MarcarNotificacaoResolvida(int? tnsId, int? codRefInt = null, string codRefStr = null)
        {
            var lstNotificacoes = RetornarNotificacoesSistema(tnsId, codRefInt, codRefStr);
            if(lstNotificacoes != null)
            {
                foreach(var not in lstNotificacoes)
                {
                    not.NTS_DATA_RESOLUCAO = DateTime.Now;
                }

                MergeAll(lstNotificacoes);
            }

        }

        public void CancelarEnvioDeNotificacoes(string hash, string contexto = null)
        {
            using(var scope = new TransactionScope())
            {
                int fleIdInt = 0;
                int.TryParse(cryptService.DescriptografarTripleDES(hash), out fleIdInt);

                var filaEmail = ServiceFactory.RetornarServico<FilaEmailSRV>().FindById(fleIdInt);

                if (filaEmail != null && filaEmail.FLE_DATA_CANCELAMENTO == null)
                {
                    filaEmail.FLE_DATA_CANCELAMENTO = DateTime.Now;
                    ServiceFactory.RetornarServico<FilaEmailSRV>().Merge(filaEmail);

                    ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(new RegistroNotificacaoSistemaDTO()
                    {

                        descricao = string.Format("A fila de E-Mail de código foi cancelada via {0}.", contexto),
                        data = DateTime.Now,
                        descricaoCodigoReferencia = "Código da fila de E-Mail",
                        nomeDaExecucao = "E-Mail Cancelado",
                        codReferencia = filaEmail.FLE_ID,
                        codNotificacaoSistema = filaEmail.NTS_ID,
                        nomeProjeto = "COAD.SEGURANCA",
                        nomeServico = "NotificacaoSistemaSRV",
                    });

                    var notificacao = RetornarNotificacaoSistema(3, filaEmail.FLE_ID);
                    if (notificacao != null && notificacao.NTF_DATA_CANCELAMENTO == null && notificacao.NTS_DATA_RESOLUCAO == null)
                    {
                        notificacao.NTF_DATA_CANCELAMENTO = DateTime.Now;
                        Merge(notificacao);

                        ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(new RegistroNotificacaoSistemaDTO()
                        {

                            descricao = string.Format("A notificação de código foi cancelada via {0}.", contexto),
                            data = DateTime.Now,
                            descricaoCodigoReferencia = "Código da Notificação do Sistema",
                            nomeDaExecucao = "Notificação Cancelada",
                            codReferencia = notificacao.NTS_ID,
                            codNotificacaoSistema = notificacao.NTS_ID,
                            nomeProjeto = "COAD.SEGURANCA",
                            nomeServico = "NotificacaoSistemaSRV",
                        });                        

                    }
                    
                }
                scope.Complete();
            }

        }

        public void MarcarNotificacaoCancelada(int? tnsId, int? codRefInt = null, string codRefStr = null, string contexto = null)
        {
            var lstNotificacoes = RetornarNotificacoesSistema(tnsId, codRefInt, codRefStr);
            if (lstNotificacoes != null)
            {
                foreach (var not in lstNotificacoes)
                {
                    if(not.NTF_DATA_CANCELAMENTO == null && not.NTS_DATA_RESOLUCAO == null)
                    {
                        not.NTF_DATA_CANCELAMENTO = DateTime.Now;
                        ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(new RegistroNotificacaoSistemaDTO()
                        {

                            descricao = string.Format("A notificação de código foi cancelada via {0}.", contexto),
                            data = DateTime.Now,
                            descricaoCodigoReferencia = "Código da Notificação do Sistema",
                            nomeDaExecucao = "Notificação Cancelada",
                            codReferencia = not.NTS_ID,
                            codNotificacaoSistema = not.NTS_ID,
                            nomeProjeto = "COAD.SEGURANCA",
                            nomeServico = "NotificacaoSistemaSRV",
                        });
                    }
                }

                MergeAll(lstNotificacoes);
            }

        }


        public void PausarErroNotificacao(string hash, int? qtdDias, string contexto = null)
        {
            using (var scope = new TransactionScope())
            {

                int fleIdInt = 0;
                int.TryParse(cryptService.DescriptografarTripleDES(hash), out fleIdInt);
                var filaEmail = ServiceFactory.RetornarServico<FilaEmailSRV>().FindById(fleIdInt);

                if (filaEmail != null)
                {
                    var dataPausa = DateTime.Now.AddDays((int)qtdDias);

                    var notificacao = RetornarNotificacaoSistema(3, filaEmail.FLE_ID);

                    if(notificacao != null)
                    {
                        notificacao.NTS_ERRO_PAUSADO_ATE = dataPausa;

                        Merge(notificacao);

                        ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(new RegistroNotificacaoSistemaDTO()
                        {
                            descricao = string.Format("A notificação de E-Email foi pausado por {0} dias via {1}.", qtdDias, contexto),
                            data = DateTime.Now,
                            descricaoCodigoReferencia = "Código da Fila de E-Mail",
                            nomeDaExecucao = "Notificação Pausada",
                            codReferencia = filaEmail.NTS_ID,
                            nomeProjeto = "COAD.SEGURANCA",
                            nomeServico = "FilaEmailSRV",
                        });
                    }
                    scope.Complete();
                }
            }
        }

    }
}

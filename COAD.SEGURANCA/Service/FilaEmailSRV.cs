using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Config.Email;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Model.Custons.Pesquisas;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.SEGURANCA.Service.Interfaces;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("FLE_ID")]
    public class FilaEmailSRV : ServiceAdapter<FILA_EMAIL, FilaEmailDTO, int>
    {
        private static bool _podeEnviar = true;
        public static bool PodeEnviar { 
            get 
            { 
                return _podeEnviar; 
            } 
            set 
            { 
                _podeEnviar = value; 
            }
        }

        private FilaEmailDAO _dao { get; set; }
        public IEmailSRV _emailSRV { get; set; }
        public HistoricoExecucaoSRV _historicoExecucao { get; set; }
        public JobAgendamentoSRV jobAgendamento { get; set; }
        public JobNotificacaoSRV jobNotificacao { get; set; }
        
        public FilaEmailSRV()
        {
            this._dao = new FilaEmailDAO();
            this.SetDao(_dao);
        }

        public FilaEmailSRV(FilaEmailDAO _dao)
        {
            this._dao = _dao;
            this.SetDao(_dao);
        }

        /// <summary>
        /// Retorna os emails que foram adionados em uma fila de envio.
        /// </summary>
        /// <returns></returns>
        public IList<FilaEmailDTO> ListarEmailsPendentesDeEnvio()
        {
            return _dao.ListarEmailsPendentesDeEnvio();
        }

        public void InserirEmailNaFila(
            EmailRequestDTO request)
        {

            string usuario = "COADSYS";
            int? repID = 1;

            if (SessionContext.PossuiSessao())
            {
                usuario = SessionContext.login;
                repID = SessionContext.rep_id;
            }

            var result = ValidatorProxy.RecursiveValidate(request);

            if (result.IsValid)
            {
                FilaEmailDTO filaEmail = new FilaEmailDTO()
                {
                    FLE_EMAIL = request.EmailDestino,
                    FLE_ASSUNTO = request.Assunto,
                    FLE_CORPO = request.CorpoEmail,
                    CLI_ID = null,
                    REP_ID = repID,
                    FLE_DATA_CRIACAO = DateTime.Now,
                    FLM_METODO_ORIGEM = request.MetodoOrigem,
                    FLM_SERVICO_ORIGEM = request.ServicoOrigem,
                    FLE_ACTION_NAME = request.ActionName,
                    FLE_ARGUMENTO_ACAO = request.ActionArg,
                    NTS_ID = request.codNotificacaoSistema,
                    FLE_REPORTACAO_ERRO = request.reporteDeErro,
                    USU_LOGIN = usuario,
                    FLE_CC = request.CC,
                    FLE_CCO = request.CCO,
                    FLE_CC_ERRO = request.CC_ERRO,
                    FLE_CCO_ERRO = request.CCO_ERRO,
                    FLE_COD_SMTP = request.codSMTP,
                    FLE_PATH_ANEXO = request.pathAnexo,
                    FLE_CALLBACK_CONTEXT_KEY = request.CallbackContextKey,
                    FLE_CALLBACK_CONTEXT_KEY_STR = request.CallbackContextKeyStr,
                    FLE_SUCCESS_CALLBACK = request.SuccessCallback,
                    FLE_FAIL_CALLBACK = request.FailCallback
                };

                var retorno = Save(filaEmail);

                if(retorno != null && retorno.FLE_ID != null)
                {
                    jobNotificacao.RegistrarJobNotificacao(new JobNotificacaoRequestDTO() {

                        codRef = retorno.FLE_ID,
                        usuario = usuario,
                        repId = repID,
                        jobID = 1,
                        descricao = string.Format("Envio de E-Mail para {0}", request.EmailDestino)
                    });
                }

            }
            else
            {
                throw new ValidacaoException("Não é agendar o envio de E-Mail. Verifique se o E-Mail de envio está correto.", result);
            }
        }

        /// <summary>
        /// Indica ao sistema que esse email já foi enviado e não precisa ser enviado novamente.
        /// </summary>
        /// <param name="email"></param>
        public void MarcarEmailEnviado(FilaEmailDTO email)
        {
            if (email != null)
            {
                email.FLE_DATA_ENVIO = DateTime.Now;
                SaveOrUpdate(email);

                int? tnsId = null;
                if(email.FLE_REPORTACAO_ERRO == true)
                {
                    tnsId = 4;
                }
                else
                {
                    tnsId = 3;
                }

                ServiceFactory.RetornarServico<NotificacaoSistemaSRV>()
                    .MarcarNotificacaoResolvida(tnsId, email.FLE_ID);

                if (!string.IsNullOrWhiteSpace(email.FLE_SUCCESS_CALLBACK))
                {
                    EmailActionContainer.ExecutarSuccessCallback(email.FLE_SUCCESS_CALLBACK, email);
                }
            }
        }

        /// <summary>
        /// Pega todos os emails pendentes de envio realiza o envio.
        /// </summary>
        public void ProcessarFilaDeEnvio(BatchContext batchContext = null)
        {

            if (batchContext == null)
                batchContext = new BatchContext();

            PodeEnviar = false;
            DateTime? dataPausa = null;
            int? codigoNotificacao = null;
            bool? reporteErroEmail = null;
            string ccErro = null;
            string ccoErro = null;
            
            int? codFila = null;
            try
            {
                var lstEmails = ListarEmailsPendentesDeEnvio();

                if (lstEmails != null && lstEmails.Count() > 0)
                {
                    batchContext.IniciarPassoBatch("Enviando os E-Mails", true, lstEmails.Count);

                    foreach (var filaEmail in lstEmails)
                    {
                        try
                        {
                            codFila = filaEmail.FLE_ID;
                            var emailRequest = new EmailRequestDTO()
                            {
                                EmailDestino = filaEmail.FLE_EMAIL,
                                Assunto = filaEmail.FLE_ASSUNTO,
                                CorpoEmail = filaEmail.FLE_CORPO,
                                EnviarAgendado = false,
                                reporteDeErro = filaEmail.FLE_REPORTACAO_ERRO,
                                CC = filaEmail.FLE_CC,
                                CCO = filaEmail.FLE_CCO,
                                CC_ERRO = filaEmail.FLE_CC_ERRO,
                                CCO_ERRO = filaEmail.FLE_CCO_ERRO,
                                codSMTP = filaEmail.FLE_COD_SMTP
                                    
                            };

                            var email = filaEmail.FLE_EMAIL;
                            var assunto = filaEmail.FLE_ASSUNTO;
                            var corpo = filaEmail.FLE_CORPO;
                            var actionName = filaEmail.FLE_ACTION_NAME;
                            var actionArg = filaEmail.FLE_ARGUMENTO_ACAO;
                            dataPausa = filaEmail.FLE_ERRO_PAUSADO_ATE;
                            codigoNotificacao = filaEmail.NTS_ID;
                            reporteErroEmail = filaEmail.FLE_REPORTACAO_ERRO;
                            ccErro = filaEmail.FLE_CC_ERRO;
                            ccoErro = filaEmail.FLE_CCO_ERRO;
                            var pathAnexo = filaEmail.FLE_PATH_ANEXO;
                            if (!string.IsNullOrWhiteSpace(actionName))
                            {
                                var bytesPDF = EmailActionContainer.ExecutarAcaoEmail(actionName, actionArg);

                                //emailRequest.LstAnexos.Add(bytesPDF);
                                emailRequest.Anexos.Add(new EmailRequestAnexoDTO() {

                                    NomeArquivo = "boleto.pdf",
                                    Extensao = "pdf",
                                    Bytes = bytesPDF
                                });
                            }

                            if (!string.IsNullOrWhiteSpace(pathAnexo))
                            {
                                if (File.Exists(pathAnexo))
                                {
                                    try
                                    {
                                        var fileName = Path.GetFileName(pathAnexo);
                                        var extensao = Path.GetExtension(pathAnexo);

                                        byte[] bytes = File.ReadAllBytes(pathAnexo);
                                        emailRequest.Anexos.Add(new EmailRequestAnexoDTO()
                                        {
                                            NomeArquivo = fileName,
                                            Extensao = extensao,
                                            Bytes = bytes
                                        });                                  
                                    }
                                    catch(FileNotFoundException e)
                                    {
                                        throw new FileNotFoundException(
                                            string.Format(
                                                "Não é possível anexar o arquivo {0} na fila {1}. O arquivo não foi encontrado.",
                                                    pathAnexo, codFila)
                                            , e);
                                    }
                                }
                            }


                            _historicoExecucao.Incluir("Envio de Email", string.Format("Enviando o email para o endereço de Email: '{0}' Código da Fila {1}", email, codFila), DateTime.Now, "FilaEmailSRV", "Segurança");
                            _emailSRV.EnviarEmail(emailRequest);
                            _historicoExecucao.Incluir("Envio de Email", string.Format("Enviado o email para o endereço de Email: '{0}' Código da Fila {1}", email, codFila), DateTime.Now, "FilaEmailSRV", "Segurança");

                            MarcarEmailEnviado(filaEmail);

							batchContext.AdicionarContagemSucesso();

                        }
                        catch(Exception e)
                        {
                            string chaveErro = string.Format("Código da Fila de E-Mail Id {0}.", codFila);

                                ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarNotificacaoErroBatch(new RegistroErroBatchDTO()
                                {
                                    batchEx = batchContext,
                                    context = chaveErro,
                                    e = e,
                                    nomeDaExecucao = "Fila de E-Mail",
                                    projeto = "SEGURANCA",
                                    servico = "FilaEmailSRV",
                                    tipoJob = 1,
                                    descricaoCodigoReferencia = "Código da Fila de E-Mail",
                                    codReferencia = codFila,
                                    contabilizarFalha = false,
                                    qtdOcorrenciaEnvioEmail = 60,
                                    codTipoNotificacaoSistema = 3,
                                    codNotificacaoOrigem = codigoNotificacao,
                                    reporteErroEmail = reporteErroEmail,
                                    ccEmail = ccErro,
                                    ccoEmail = ccoErro,
                                    enviarEmailErro = false
                                });

                            if (!string.IsNullOrWhiteSpace(filaEmail.FLE_FAIL_CALLBACK))
                            {
                                EmailActionContainer.ExecutarFailCallback(filaEmail.FLE_FAIL_CALLBACK, filaEmail);
                            }
							batchContext.AdicionarContagemFalha();
                        }
                        finally
                        {

                            batchContext.IncrementarPassoBatch();
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao tentar procesar a fila de emails", e);
            }
         
        }

        public void CancelarEmailNaFila(int? fleId, string contexto = null)
        {
            using(var scope = new TransactionScope())
            {
                var filaEmail = FindById(fleId);
                filaEmail.FLE_DATA_CANCELAMENTO = DateTime.Now;

                Merge(filaEmail);

                ServiceFactory.RetornarServico<NotificacaoSistemaSRV>().MarcarNotificacaoCancelada(3, filaEmail.FLE_ID, null, contexto);
                scope.Complete();
            }
        }

        public void PausarErroEmailNaFila(int? fleId, int? qtdDias, string contexto = null)
        {
            using (var scope = new TransactionScope())
            {

                var dataPausa = DateTime.Now.AddDays((int)qtdDias);

                var filaEmail = FindById(fleId);
                filaEmail.FLE_ERRO_PAUSADO_ATE = dataPausa;

                Merge(filaEmail);

                ServiceFactory.RetornarServico<HistoricoExecucaoSRV>().Incluir(new RegistroNotificacaoSistemaDTO()
                {
                    descricao = string.Format("A notificação de E-Email foi pausado por {0} dias via {1}.", qtdDias, contexto),
                    data = DateTime.Now,
                    descricaoCodigoReferencia = "Código da Fila de E-Mail",
                    nomeDaExecucao = "Notificação Pausada",
                    codReferencia = fleId,
                    nomeProjeto = "COAD.SEGURANCA",
                    nomeServico = "FilaEmailSRV",
                });
                scope.Complete();
            }
        }

        public void TestarCriacaoDeEmail()
        {
            _emailSRV.EnviarEmail(new EmailRequestDTO()
            {

                Assunto = "Finalize a compra do seu produto COAD",
                EmailDestino = "dx_diego@hotmail.com",
                CorpoEmail = "<h3>Testando 133445566</h3>",
                codSMTP = 1
            });
        }

        public Pagina<ListaFilaEmailDTO> PesquisarFilaEmail(PesquisarFilaEmailDTO pesquisaDTO)
        {
            return _dao.PesquisarFilaEmail(pesquisaDTO);
        }
        
        public void AlterarEnderecoEmail(ListaFilaEmailDTO filaEmail)
        {
            if (filaEmail == null)
                throw new Exception("Nenhum objeto foi informado.");

            using (var scope = new TransactionScope())
            {
                var objfilaEmail = FindById(filaEmail.Id);
                objfilaEmail.FLE_EMAIL = filaEmail.Email;

                Merge(objfilaEmail);
                scope.Complete();
            }
        }

    }
}

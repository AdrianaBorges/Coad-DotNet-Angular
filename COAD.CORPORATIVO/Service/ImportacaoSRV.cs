using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;
using System.Web;
using System.IO;
using GenericCrud.Util;
using GenericCrud.Excel;
using COAD.CORPORATIVO.Util;
using Coad.GenericCrud.Extensions;
using GenericCrud.Metadatas;
using COAD.CORPORATIVO.Service.Custons;
using GenericCrud.Service.Formatting;
using GenericCrud.Models.HistoryRegister;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Comparators;
using GenericCrud.Models.MessageFormatter;
using GenericCrud.Models.Comparators;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCAO.Util;
using COAD.SEGURANCA.Exceptions;
using GenericCrud.Service;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.SEGURANCA.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Batch;
using GenericCrud.Validations;
using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ImportacaoSuspect;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("IMP_ID")]
    public class ImportacaoSRV : GenericService<IMPORTACAO, ImportacaoDTO, int>
    {

        public ImportacaoDAO _dao { get; set; }
        public ClienteSRV _clienteSRV { get; set; }
        public MunicipioSRV _municipioService { get; set; }
        public RegiaoSRV _regiaoSRV { get; set; }
        public AssinaturaEmailSRV _assinaturaEmalSRV { get; set; }
        public AssinaturaTelefoneSRV _assinaturaTelefoneSRV { get; set; }
        public OrigemCadastroSRV _origemCadastroSRV { get; set; }
        public ProdutoComposicaoSRV _produtoComposicaoSRV { get; set; }
        public InfoMarketingSRV _infoMkt { get; set; }
        public AreasSRV _areasSRV { get; set; }
        public BatchCustomSRV _batchSRV { get; set; }
        public ImportacaoStatusSRV _importacaoStatusSRV { get; set; }
        public MessageFormatterService formatterService { get; set; }
        public JobAgendamentoSRV _jobAgendamento { get; set; }
        public ImportacaoHistoricoSRV _importacaoHistoricoSRV { get; set; }
        public RepresentanteSRV _representanteSRV { get; set; }

        public ImportacaoSRV()
        {
            this._dao = new ImportacaoDAO();
            this.Dao = _dao;
            this._clienteSRV = new ClienteSRV();
            this._municipioService = new MunicipioSRV();
            this._regiaoSRV = new RegiaoSRV();
            this._assinaturaEmalSRV = new AssinaturaEmailSRV();
            this._assinaturaTelefoneSRV = new AssinaturaTelefoneSRV();
            this._origemCadastroSRV = new OrigemCadastroSRV();
            this._produtoComposicaoSRV = new ProdutoComposicaoSRV();
            this._infoMkt = new InfoMarketingSRV();
            this._areasSRV = new AreasSRV();
            this._batchSRV = new BatchCustomSRV();
            this._representanteSRV = new RepresentanteSRV();
        }

        public ImportacaoSRV(ImportacaoDAO _dao)
        {
            this.Dao = _dao;
        }

        public ImportacaoDTO AgendarImportacao(string sessionId, int? repId, string usuario)
        {
            try
            {
                var file = BatchUtil.RetornarObjeto<HttpPostedFileBase>(sessionId);
                if (file != null)
                {
                                        
                    string path = SysUtils.FormatarPathComNomeAmbiente(@"\\rj-app-srv\share\DEV\import_temp\{{ambiente}}\");
                    
                    var unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var fileName = string.Format(@"{0}import-({1})-{2}", path, unixTimestamp, file.FileName);

                    FileUtil.CriarDiretorioPermisaoServicoWin(fileName);
                    if (FileUtil.IsDirectoryWritable(fileName))
                    {

                        file.SaveAs(fileName);
                        var importacao = AgendarImportacao(repId, usuario, fileName);
                        return importacao;
                    }
                    else
                    {
                        throw new Exception(string.Format("O arquivo '{0}' não pode ser salvo. Talves não exista permissão na pasta de destino, ou ela não exista.", fileName));
                    }

                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível agendar a importação", e);
            }

        }
        public ImportacaoDTO AgendarImportacao(int? repId, string usuLogin, string path)
        {
            var data = DateTime.Now;
            ImportacaoDTO importacao = new ImportacaoDTO()
            {
                IMP_DATA = data,
                IMS_ID = 6,
                REP_ID = repId,
                USU_LOGIN = usuLogin,
                IMS_PATH_ARQUIVO = path
            };

            var imp = Save(importacao);

            var mensagem = @"O representante {0} - usuario ({1}) agendou a execução desta importação.";
            var nomeRepresentante = _representanteSRV.RetornarNomeRepresentante(repId);
            mensagem = string.Format(mensagem, nomeRepresentante, usuLogin);

            _importacaoHistoricoSRV.IncluirHistoricoImportacao(mensagem, imp, null, repId, usuLogin);
            imp.IMPORTACAO_STATUS = _importacaoStatusSRV.FindById(imp.IMS_ID);
            return imp;
        }

        public ImportacaoDTO RetornarProximaImportacaoEmAberto()
        {
            return _dao.RetornarProximaImportacaoEmAberto();
        }

        public BatchContext ExecutarAtualizacaoSuspectsIncorretos(int? impID, string path, int? repID, string usuLogin = null)
        {
            HttpPostedFileBase arquivo = UploadUtil.RetornarArquivoDeUpload();
            if(arquivo != null)
            {
                return AtualizarSuspectsIncorretos(impID, arquivo, path, repID, usuLogin);
            }
            return null;
        }

        public BatchContext AtualizarSuspectsIncorretos(int? impID, HttpPostedFileBase file, string path, int? repID, string usuLogin = null)
        {
            if(file != null)
            {
                BatchContext context = new BatchContext();

                var fileName = string.Format(@"{0}temp\{1}", path, file.FileName);

                file.SaveAs(fileName);
                IList<ImportacaoSuspectDTO> lstSuspects = new List<ImportacaoSuspectDTO>();
                using(var excel = new ExcelLoad(fileName))
                {
                    lstSuspects = excel.ToDTO<ImportacaoSuspectDTO>();
                }

                File.Delete(fileName);
                var importacaoSuspectSRV = ServiceFactory.RetornarServico<ImportacaoSuspectSRV>();
                if(lstSuspects != null && lstSuspects.Count > 0)
                {
                    foreach(var sus in lstSuspects)
                    {
                        using(var scope = new TransactionScope())
                        {
                            var importacaoSuspectDoBanco = importacaoSuspectSRV.FindById(sus.IPS_ID);
                            if(importacaoSuspectDoBanco.IMP_ID != null && importacaoSuspectDoBanco.IMP_ID == impID)
                            {
                                importacaoSuspectDoBanco.AlterarDados(sus);
                                importacaoSuspectSRV.Merge(importacaoSuspectDoBanco);
                                context.TotalExito++;
                            }
                            else
                            {
                                context.TotalFalha++;
                                string mensagem = null;
                                if(importacaoSuspectDoBanco == null)
                                    mensagem = string.Format(@"O prospect não pode ser atualizado. O código {0} não foi encontrado.", sus.IPS_ID);
                                else
                                    mensagem = string.Format(@"O prospect de Código {0} não pertence a importação selecionada.", importacaoSuspectDoBanco.IPS_ID);

                                var lstValidacao = new List<string>() { mensagem };

                                var reportErro = new ErroReportItemDTO()
                                {
                                    Contexto = string.Format("Código de Importação {0}", importacaoSuspectDoBanco.IPS_ID),
                                    Mensagem = "Erro na planilha de Suspect"
                                };

                                reportErro.ValidationErrors.Add("importacaoSuspect.IPS_ID", lstValidacao);
                                context.ListErros.Add(reportErro);
                            }
                            scope.Complete();
                        }
                    }

                    var importacao = FindById(impID);
                    string mensagemDeErro = null;

                    if (context.TotalExito > 0)
                    {
                        importacao.IMS_ID = 3;
                        Merge(importacao);

                    }

                    if (context.TotalFalha > 0)
                    {
                        mensagemDeErro = string.Format(@"Além disso houveram {0} erros.", context.TotalFalha);
                    }

                    var mensagemHis = @"O representante {0} - usuario ({1}) atualizou dados de importação de {2} suspects com sucesso. {3}. No total de {4} suspects na planilha.";
                    var nomeRepresentante = _representanteSRV.RetornarNomeRepresentante(repID);
                    mensagemHis = string.Format(mensagemHis, nomeRepresentante, usuLogin, context.TotalExito, mensagemDeErro, lstSuspects.Count);

                    _importacaoHistoricoSRV.IncluirHistoricoImportacao(mensagemHis, importacao, null, repID, usuLogin);
                }
                return context;
            }
            return null;
        }

        public void ProcessarImportacao(BatchContext batchContext = null)
        {
            ImportacaoDTO importacao = null;
            try
            {
                if (batchContext == null)
                {
                    batchContext = new BatchContext();
                    batchContext.JobID = 8;
                    _jobAgendamento.MarcarInicioExecucao(8);
                }
                ContextoImportacaoDTO contexto = new ContextoImportacaoDTO();
                contexto.BatchContext = batchContext;
                batchContext.IniciarPassoBatch("Iniciando importação...", false);
                int? codRef = contexto.BatchContext.RetornarCodReferencia();

                if (codRef != null)
                    importacao = FindById(codRef);
                else
                    importacao = RetornarProximaImportacaoEmAberto();

                if(importacao != null)
                {
                    importacao.IMS_ID = 7;
                    Merge(importacao);
                    contexto.Importacao = importacao;
                    if(importacao.IMP_PLANILHA_INSERIDA == null)
                    {
                        var path = importacao.IMS_PATH_ARQUIVO;
                        var lstSuspectsDTO = ConverterPlanilha(path);
                        InserirDadosNaImportacao(lstSuspectsDTO, importacao, contexto);
                        importacao.IMP_PLANILHA_INSERIDA = true;
                        Merge(importacao);
                       
                    }

                    if(importacao.IMP_PLANILHA_INSERIDA == true)
                    { 
                        batchContext.IniciarPassoBatch("Importando os clientes...", false);
                        ProcessarSuspectsDeImportacao(importacao.IMP_ID, contexto, importacao);

                        if (contexto.BatchContext.TotalFalha == null || contexto.BatchContext.TotalFalha == 0)
                        {
                            importacao.IMS_ID = 4;
                            Merge(importacao);
                        }
                    }

                    if(importacao.IMS_ID != 4)
                    {
                        importacao.IMS_ID = 3;
                        Merge(importacao);
                    }
                    importacao.IMP_DATA_ULTIMA_EXECUCAO = DateTime.Now;
                    Merge(importacao);
                    _importacaoHistoricoSRV.IncluirHistoricoImportacao("A processo de importação terminou de executar.", importacao, batchContext);
                }
            }
            catch(Exception e)
            {
                if(importacao != null)
                {
                    importacao.IMS_ID = 3;
                    Merge(importacao);
                    _importacaoHistoricoSRV.IncluirHistoricoErroImportacao(importacao, e);
                }
                else
                {
                    string chaveErro = string.Format("Erro ao importar os suspects.");

                    ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarNotificacaoErroBatch(new RegistroErroBatchDTO()
                    {
                        batchEx = batchContext,
                        context = chaveErro,
                        e = e,
                        nomeDaExecucao = "Importação de Suspect",
                        projeto = "CORPORATIVO",
                        servico = "ImportacaoSRV",
                        tipoJob = 7,
                        descricaoCodigoReferencia = "Não existe",
                        codReferencia = 0,
                        contabilizarFalha = false,
                        qtdOcorrenciaEnvioEmail = 60,

                    });
                }
            }
            finally
            {
                _jobAgendamento.MarcarFimExecucao(8);
            }
        }

        private ICollection<ImportacaoSuspectDTO> ConverterPlanilha(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
            {                
                var fileName = filePath.Split('\\').Last();

                string path = SysUtils.FormatarPathComNomeAmbiente(@"C:\planilha_temp\{{ambiente}}\{0}");
                var newPath = string.Format(path, fileName);

                FileUtil.CriarDiretorioPermisaoServicoWin(newPath);
                if (FileUtil.IsDirectoryWritable(newPath))
                {
                    File.Copy(filePath, newPath, true);
                    ICollection<ImportacaoSuspectDTO> lstSuspectsDTO = null;
                    using (var excelLoad = new ExcelLoad(newPath))
                    {
                        lstSuspectsDTO = excelLoad.ToDTO<ImportacaoSuspectDTO>();
                    }
                    return lstSuspectsDTO;
                }
                else
                {
                    throw new Exception(string.Format("O arquivo '{0}' não foi encontrado.", newPath));
                }
            }
            else
            {
                throw new Exception(string.Format("O arquivo '{0}' não foi encontrado.", filePath));
            }
        }
        

        public void InserirDadosNaImportacao(ICollection<ImportacaoSuspectDTO> lstSuspects, ImportacaoDTO importacao, ContextoImportacaoDTO contextoImportacao)
        {
            if (lstSuspects != null && lstSuspects.Count > 0 && importacao != null)
            {
                contextoImportacao.BatchContext.IniciarPassoBatch("Salvando Planilha", true, lstSuspects.Count);

                int index = 0;
                foreach (var sus in lstSuspects)
                {
                    try
                    {
                        using (var scope = new TransactionScope())
                        {
                            sus.IMP_ID = importacao.IMP_ID;
                            sus.IMS_ID = 1;
                            ServiceFactory.RetornarServico<ImportacaoSuspectSRV>().Save(sus);
                            scope.Complete();
                        }
                        contextoImportacao.BatchContext.AdicionarContagemSucesso();
                    }
                    catch(Exception ex)
                    {
                        var mensagem = "Não foi possível importar o prospect na linha {0} da planilha.";
                        mensagem = string.Format(mensagem, index + 2);

                        contextoImportacao.BatchContext.AdicionarContagemFalha();
                        _importacaoHistoricoSRV.IncluirHistoricoErroImportacao(importacao, ex, mensagem);
                    }
                    finally{
                        contextoImportacao.BatchContext.IncrementarPassoBatch();
                    }

                    index++;
                }

            }
        }

        public void ProcessarSuspectsDeImportacao(int? impID, ContextoImportacaoDTO contextoSuspect, ImportacaoDTO importacao)
        {
            var _servicoImportacao = ServiceFactory.RetornarServico<ImportacaoSuspectSRV>();
            var lstSuspects = _servicoImportacao.ListarSuspectsNaoImportados(impID);

            if (lstSuspects != null && lstSuspects.Count > 0)
            {
                _servicoImportacao.ImportarClientes(importacao, lstSuspects, contextoSuspect);
            }
        }

        public Pagina<ImportacaoDTO> PesquisarImportacoes(
            PesquisaImportacaoDTO pesquisaImportacaoDTO)
        {
            return _dao.PesquisarImportacoes(pesquisaImportacaoDTO);
        }


        public BatchProgressDTO RetornarProgressoDaImportacao(int? impID)
        {
            var importacao = FindById(impID);
            var dadosDoJob = _jobAgendamento.FindById(8);
            var ultimosErros = _importacaoHistoricoSRV.BuscarUltimosHistoricosDeErroDaImportacao(impID);
            
            if(importacao != null && dadosDoJob != null)
            {
                var batchProgress = new BatchProgressDTO()
                {
                    Executando = (importacao.IMS_ID == 7),
                    CodStatus = importacao.IMS_ID,
                    LstErros = ultimosErros,
                    ProcessedItens = dadosDoJob.JOB_BATCH_PROCESSED_ITENS,
                    TotalItens = dadosDoJob.JOB_BATCH_TOTAL_ITENS,
                    NomePassoBatch = dadosDoJob.JOB_BATCH_NOME,
                    Progress = (dadosDoJob.JOB_BATCH_PROGRESS != null) ? (int) dadosDoJob.JOB_BATCH_PROGRESS : 0,
                    QuantidadeFalha = (dadosDoJob.JOB_QTD_FALHA != null) ? (int)dadosDoJob.JOB_QTD_FALHA : 0,
                    QuantidadeSucesso = (dadosDoJob.JOB_QTD_SUCESSO != null) ? (int)dadosDoJob.JOB_QTD_SUCESSO : 0
                };

                return batchProgress;
            }

            return null;
        }

        public void ReexecutarImportacao(int? impID, int? repID = null, string usuLogin = null)
        {
            using(var scope = new TransactionScope())
            {
                var importacao = FindById(impID);
                if(importacao != null)
                {
                    importacao.IMS_ID = 6;                
                    Merge(importacao);
                }

                var mensagem = @"O representante {0} - usuario ({1}) agendou a execução desta importação.";
                var nomeRepresentante = _representanteSRV.RetornarNomeRepresentante(repID);
                mensagem = string.Format(mensagem, nomeRepresentante, usuLogin);

                _importacaoHistoricoSRV.IncluirHistoricoImportacao(mensagem, importacao, null, repID, usuLogin);
                scope.Complete();
            }
        }

        public void CancelarImportacao(int? impID, int? repID, string usuLogin)
        {
            using(var scope = new TransactionScope())
            {
                var importacao = FindById(impID);
                importacao.IMS_ID = 5;
                importacao.IMP_DATA_CANCELAMENTO = DateTime.Now;
                Merge(importacao);
                var mensagem = @"O representante {0} - usuario ({1}) Cancelou a importação.";
                var nomeRepresentante = _representanteSRV.RetornarNomeRepresentante(repID);
                mensagem = string.Format(mensagem, nomeRepresentante, usuLogin);

                _importacaoHistoricoSRV.IncluirHistoricoImportacao(mensagem, importacao, null, repID, usuLogin);
                scope.Complete();
            }
        }
        public ImportacaoDTO RetornarImportacaoWebServiceDoDia(DateTime? date)
        {
            return _dao.RetornarImportacaoWebServiceDoDia(date);
        }

        public ImportacaoDTO GetImportacaoDiariaWebService()
        {
            var data = DateTime.Now;
            var importacao = RetornarImportacaoWebServiceDoDia(data);

            if(importacao == null)
            {
                importacao = new ImportacaoDTO()
                {
                    IMP_DATA = data,
                    IMP_WEB_SERVICE = true,
                    REP_ID = 1,
                    USU_LOGIN = "COADCORP"
                };
                importacao = Save(importacao);
            }

            return importacao;
        }

        public void AgendarImportacaoDiariaWebService(ClienteImportacaoWebServiceDTO cliente)
        {
            var validation = ValidatorProxy.RecursiveValidate<ClienteImportacaoWebServiceDTO>(cliente);
            if (!validation.IsValid)
            {
                throw new ValidacaoException("Não é possível salvar os dados do cliente.", validation);
            }

            using (var scope = new TransactionScope())
            {
                var importacao = GetImportacaoDiariaWebService();

                if(importacao != null)
                {
                    var importacaoSuspect = cliente.ConverterParaImportacaoSuspect();
                    importacaoSuspect.IMP_ID = importacao.IMP_ID;
                    importacaoSuspect.IMS_ID = 1;

                    ServiceFactory.RetornarServico<ImportacaoSuspectSRV>().Save(importacaoSuspect);
                    importacao.IMS_ID = 6;
                    importacao.IMP_PLANILHA_INSERIDA = true;

                    Merge(importacao);
                }

                scope.Complete();
            }
                    
        }

    }
}

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
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using GenericCrud.Excel.Impl;
using COAD.SEGURANCA.Model.Custons;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("IPS_ID")]
    public class ImportacaoSuspectSRV : GenericService<IMPORTACAO_SUSPECT, ImportacaoSuspectDTO, int>
    {
        public ImportacaoSuspectDAO _dao { get; set; }
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
        public ImportacaoSRV _importacaoSRV { get; set; }
        public ImportacaoStatusSRV _importacaoStatusSRV { get; set; }        
        public MessageFormatterService formatterService { get; set; }
        public ImportacaoHistoricoSRV _importacaoHistoricoSRV { get; set; }
        
        public ImportacaoSuspectSRV()
        {
            this._dao = new ImportacaoSuspectDAO();
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

            this.formatterService = FormatterServiceLocalFactory.CriarMessageFormatterServiceCoorporativo();
            this.Comparator = new GenericComparator<ImportacaoSuspectDTO>("IPS_NOME", 
                "IPS_CPF_CNPJ", 
                "IPS_TELEFONE", 
                "IPS_FAX", 
                "IPS_CELULAR", 
                "IPS_EMAIL");

            this.Dao = _dao;
        }

        public ImportacaoSuspectSRV(ImportacaoSuspectDAO _dao)
        {
            this.Dao = _dao;
            this._dao = _dao;
            this.formatterService = FormatterServiceLocalFactory.CriarMessageFormatterServiceCoorporativo();
            this.Comparator = new GenericComparator<ImportacaoSuspectDTO>("IPS_NOME",
                "IPS_CPF_CNPJ",
                "IPS_TELEFONE",
                "IPS_FAX",
                "IPS_CELULAR",
                "IPS_EMAIL");

            this.Dao = _dao;
        }

        public void ReceberUploadPlanilhaSuspects(HttpPostedFileBase file, string serverPath, string path, string sessionId)
        {
            try
            {

                if (file != null)
                {
                    var ambiente = SysUtils.RetornarAmbienteNameTotal();

                    if (!string.IsNullOrWhiteSpace(ambiente))
                        ambiente = StringUtil.LimparAcentuacao(ambiente.ToLower());
                    var fileName = string.Format(@"planilha-de-carga-{1:yyyy-MM-dd hh-mm-ss}.xlsx", ambiente, DateTime.Now);

                    var uploadPath = new FileFluent(file)
                        .CheckExtensions("xls", "xlsx", "calc")
                        .SetLocations(serverPath, path)
                        .SetFileName(fileName)
                        .CheckValidations()
                        .TrySave();
                    
                    if (File.Exists(uploadPath))
                    {
                        using (var excelLoad = new ExcelLoad(uploadPath))
                        {
                            var lstSuspectsDTO = excelLoad.ToDTO<ImportacaoSuspectDTO>();
                            BatchUtil.ArmazenarObjeto(sessionId, lstSuspectsDTO);

                            // deletar o arquivo do servidor
                            File.Delete(uploadPath);
                        }
                    }
                }
                else
                {
                    throw new ArgumentNullException("O argumento file não pode ser null");
                }
            }
            catch (Exception e)
            {
                throw new UploadException("Ocorreu um erro ao tentar realizar o upload da planilha.", e);
            }
        }

        public void ReceberUploadPlanilhaSuspectsEAgendarImportacao(HttpPostedFileBase file, string serverPath, string path, string sessionId)
        {
            try
            {

                if (file != null)
                {
                    var ambiente = SysUtils.RetornarAmbienteNameTotal();

                    if (!string.IsNullOrWhiteSpace(ambiente))
                        ambiente = StringUtil.LimparAcentuacao(ambiente.ToLower());
                    var fileName = string.Format(@"planilha-de-carga-{0:yyyy-MM-dd hh-mm-ss}.{1}", DateTime.Now, file.FileName.Split('.').Last());
                    BatchUtil.ArmazenarObjeto(sessionId, file);

                    //var uploadPath = new FileFluent(file)
                    //    .CheckExtensions("xls", "xlsx", "calc")
                    //    .SetLocations(serverPath, path)
                    //    .SetFileName(fileName)
                    //    .CheckValidations()
                    //    .TrySave();

                    //if (File.Exists(uploadPath))
                    //{
                    //    BatchUtil.ArmazenarObjeto(sessionId, uploadPath);

                    //    using (var excelLoad = new ExcelLoad(uploadPath))
                    //    {
                    //        var lstSuspectsDTO = excelLoad.ToDTO<ImportacaoSuspectDTO>(limitTo: 10);
                    //        BatchUtil.ArmazenarObjeto(sessionId + "_previa", lstSuspectsDTO);

                    //        // deletar o arquivo do servidor
                    //        File.Delete(uploadPath + "_previa");
                    //    }
                    //}


                }
                else
                {
                    throw new ArgumentNullException("O argumento file não pode ser null");
                }
            }
            catch (Exception e)
            {
                throw new UploadException("Ocorreu um erro ao tentar realizar o upload da planilha.", e);
            }
        }

        public PreviaImportacaoSuspectDTO GerarPreviaDeImportacao(string sessionId)
        { 
            var lstImportacaoSuspect = BatchUtil.RetornarObjeto<IEnumerable<ImportacaoSuspectDTO>>(sessionId + "_previa");
            var lstImportacaoCorreta = lstImportacaoSuspect.Distinct(GetComparator());

            var quantidadeNaPlanilha = lstImportacaoSuspect.Count();
            var quantidadeReal = lstImportacaoCorreta.Count();
            var quantidadeDuplicada = quantidadeNaPlanilha - quantidadeReal;

            var pagina = lstImportacaoSuspect.Paginar(1, 50);


            PreviaImportacaoSuspectDTO previaSuspectResult = new PreviaImportacaoSuspectDTO()
            {
                QuantidadeRegistros = quantidadeNaPlanilha,
                QuantidadeReal = quantidadeReal,
                QuantidadeDuplicada = quantidadeDuplicada
            };
            

            if (pagina != null)
            {
                previaSuspectResult.ListaResumo = pagina.lista.ToList();
            }
            BatchUtil.LimparObjeto(sessionId + "_previa");
            return previaSuspectResult;
        }


        /// <summary>
        /// Busca no banco os clientes que já existem (por cnpj, telefone, email) e separa os clientes já existentes e os que não existem.
        /// </summary>
        /// <param name="lstClientes"></param>
        public ClienteDto BuscarClientesJaExistentes(IEnumerable<ImportacaoSuspectDTO> lstSuspect)
        {
            if (lstSuspect != null)
            {
                IList<string> lstCnpf = lstSuspect.Where(x => x.IPS_CPF_CNPJ != null)
                    .Select(sel => sel.IPS_CPF_CNPJ)
                    .ToList();

                var susTel = lstSuspect.Where(x => x.IPS_TELEFONE != null || x.IPS_CELULAR != null || x.IPS_FAX != null);

                IList<string> lstTelefone = new List<string>()
                    .Concat(susTel.Where(x => x.IPS_FAX != null).Select(sel => sel.IPS_FAX))
                    .Concat(susTel.Where(x => x.IPS_CELULAR != null).Select(sel => sel.IPS_CELULAR))
                    .Concat(susTel.Where(x => x.IPS_TELEFONE != null).Select(sel => sel.IPS_TELEFONE))
                    .ToList();

                IList<string> lstEmail = lstSuspect
                    .Where(x => x.IPS_EMAIL != null)
                    .Select(sel => sel.IPS_EMAIL)
                    .ToList();

                var cliente = _clienteSRV.BuscarClientesJaExistentes(lstCnpf, lstTelefone, lstEmail);
                
                if(cliente != null)
                {
                    _assinaturaEmalSRV.PreencherEmailAssinaturaNoCliente(cliente);
                    _assinaturaTelefoneSRV.PreencherTelefoneAssinaturaNoCliente(cliente);
                }
                
                return cliente;
            }

            return null;
        }

        //public void ValidarEPrepararImportacaoClientes()
        //{
        //    var importacaoSuspectResult = UploadUtil.RetornarObjetoDeUpload<ImportacaoSuspectResultDTO>();

        //    if (importacaoSuspectResult != null && !importacaoSuspectResult.Validado)
        //    {
        //        var lstClientes = BuscarClientesJaExistentes(importacaoSuspectResult.ListaSuspectsNaoConvertidos);
        //        importacaoSuspectResult.ListaClienteAtualizar = lstClientes;
        //    }
        //}

        

        [MetodoTopLevel]
        public ContextoImportacaoDTO ImportarClientes(string sessionId, int? REP_ID_DEMANDANTE)
        {
            ContextoImportacaoDTO contextoImportacao = new ContextoImportacaoDTO();
            contextoImportacao.IniciarTempoGeral();
            contextoImportacao.IniciarPassoBatch("Iniciando importação...", false);
            
            try
            {
                //_batchSRV.AdicionarStatusDeBatchImportacaoSuspect(sessionId, contextoImportacao.BatchContext);

                TransactionOptions txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                // ValidarEPrepararImportacaoClientes();
                var lstImportacaoSuspect = BatchUtil.RetornarObjeto<ICollection<ImportacaoSuspectDTO>>(sessionId);
                if (lstImportacaoSuspect != null)
                {
                    BatchUtil.LimparObjeto(sessionId);

                    // var lstClientesEncontrados = importacaoSuspectResult.ListaClienteAtualizar;
                    //var lstCliente = ConverterParaCliente((ICollection<ImportacaoSuspectDTO>)lstImportacaoSuspect, contextoImportacao);

                    // até esse ponto ainda não há qualquer espécie de alteração de dados a partir daqui há
                    using (var scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                    {
                        contextoImportacao.IniciarPassoBatch("Processando dados do cliente..", true);
                        if (lstImportacaoSuspect != null)
                        {
                            //// Compara a lista de suspects e clientes e devolve os suspetcs que não estão presentes na outra lista, 
                            //var lstSuspectsNovos = SepararSuspectsQueJaSaoClientes(lstImportacaoSuspect, lstClientesEncontrados);

                            //_clienteSRV.InsersaoEmMassaDeClientes(lstCliente, contextoImportacao, REP_ID_DEMANDANTE);

                            //if (lstClientesEncontrados != null && lstClientesEncontrados.Count() > 0)
                            //{
                            //    _clienteSRV.EncarteirarESalvarVariosClientes(lstClientesEncontrados, contextoImportacao, REP_ID_DEMANDANTE);
                            //}
                        }

                        contextoImportacao.IniciarPassoBatch("Completado com sucesso!!", false);
                        scope.Complete();
                    }
                }
                else
                {
                    throw new UploadException("As informações da planilha não estão mais disponível. A sessão pode ter expirado. Recarregue a tela e tente novamente.");
                }

            }
            catch (Exception e)
            {
                throw new Exception("Ocorre um erro", e);
            }
            finally
            {
                contextoImportacao.PararTempoGeral();
                _batchSRV.RemoverStatusDeBatchImportacaoSuspect(sessionId);
            }

            return contextoImportacao;
        }
        public ClienteDto ConverterParaCliente(ImportacaoSuspectDTO susp, int index)
        {
            ClienteDto cliente = new ClienteDto()
            {
                CLI_NOME = susp.IPS_NOME,
                CLI_CPF_CNPJ = susp.IPS_CPF_CNPJ,
                CLI_A_C = susp.IPS_CONTATO,
                INFO_MARKETING = new InfoMarketingDTO()
                {
                    O_CAD_ID = 13
                },
                IPS_ID = susp.IPS_ID
            };

            if (!string.IsNullOrEmpty(susp.IPS_CLASSIFICACAO))
            {
                if (susp.IPS_CLASSIFICACAO.ToLower() == "p")
                {
                    cliente.CLA_CLI_ID = 2;
                }
                else
                {
                    cliente.CLA_CLI_ID = 1;
                }
            }

            if (!string.IsNullOrEmpty(susp.IPS_TIPO_CLIENTE))
            {
                if (susp.IPS_TIPO_CLIENTE.ToLower() == "f")
                {
                    cliente.TIPO_CLI_ID = 2;
                    cliente.CLI_TP_PESSOA = "F";
                }
                else if (susp.IPS_TIPO_CLIENTE.ToLower() == "j")
                {
                    cliente.TIPO_CLI_ID = 3;
                    cliente.CLI_TP_PESSOA = "J";
                }
                else if (susp.IPS_TIPO_CLIENTE.ToLower() == "o")
                {
                    cliente.TIPO_CLI_ID = 1;
                    cliente.CLI_TP_PESSOA = "O";
                }
                else
                {
                    cliente.TIPO_CLI_ID = 2;
                    cliente.CLI_TP_PESSOA = "F";
                }
            }
            else
            {
                cliente.TIPO_CLI_ID = 2;
                cliente.CLI_TP_PESSOA = "F";
            }

            ICollection<ClienteEnderecoDto> lstEndereco = ConverterEndereco(susp);
            cliente.CLIENTES_ENDERECO = lstEndereco;

            ICollection<AssinaturaTelefoneDTO> lstTelefone = ConverterTelefone(susp);
            cliente.ASSINATURA_TELEFONE = lstTelefone;

            ICollection<AssinaturaEmailDTO> lstEmail = ConverterEmail(susp);
            cliente.ASSINATURA_EMAIL = lstEmail;

            cliente.INFO_MARKETING = ConverterInformacaoDeMarketing(susp.IPS_ORIGEM_CADASTRO, susp.IPS_PRODUTO_INTERESSE, susp.IPS_AREA_INTERESSE);
            AtribuirRegiao(susp, cliente);
            
            return cliente;
        }

        private void AtribuirRegiao(ImportacaoSuspectDTO susp, ClienteDto cliente)
        {
            RegiaoDTO regiao = _regiaoSRV.EncontrarRegiaoPorNome(susp.IPS_REGIAO);

            if (regiao != null)
            {
                cliente.RegiaoIdParaRodizio = regiao.RG_ID;
            }
            else
            {
                regiao = _regiaoSRV.PesquisarRegiaoPorCidadeEEstado(susp.IPS_UF);
                if (regiao != null)
                {
                    cliente.RegiaoIdParaRodizio = regiao.RG_ID;
                }
                else
                {
                    string message = null;
                    if (!string.IsNullOrWhiteSpace(susp.IPS_REGIAO))
                    {
                        message = @"Não é possível importar os Suspects de Importação {0}. O mesmo não possui a região de destíno válida [{1}]. Verifigue o nome informado. Podem haver outros clientes com a região incorreta também.";
                        message = string.Format(message, susp.IPS_NOME, susp.IPS_ID, susp.IPS_REGIAO);
                    }
                    else
                    {
                        message = @"Não é possível importar os Suspects de Importação {0}. Nenhuma região foi informada. Ou não foi possível localizar uma região compatível com a UF e cidade do Suspect. Por isso, não é possível fazer o rodizio.";
                        message = string.Format(message, susp.IPS_NOME);
                    }

                    throw new Exception(message);
                }
            }

        }

        [MetodoAuxiliar]
        public ICollection<ClienteEnderecoDto> ConverterEndereco(ImportacaoSuspectDTO susp)
        {
            if (!string.IsNullOrWhiteSpace(susp.IPS_BAIRRO) ||
                !string.IsNullOrWhiteSpace(susp.IPS_CIDADE) ||
                !string.IsNullOrWhiteSpace(susp.IPS_UF))
            {
                ICollection<ClienteEnderecoDto> lstClientesEnderecos = new HashSet<ClienteEnderecoDto>();

                var munId = _municipioService.RetornarMunIdPorDescricao(susp.IPS_CIDADE, susp.IPS_UF);
                ClienteEnderecoDto cliEnd = new ClienteEnderecoDto() 
                { 
                    END_UF = susp.IPS_UF,
                    END_MUNICIPIO = susp.IPS_CIDADE,
                    END_BAIRRO = susp.IPS_BAIRRO,
                    END_TIPO = 2,
                    MUN_ID = munId                    
                };

                lstClientesEnderecos.Add(cliEnd);

                return lstClientesEnderecos;
            }

            return null;
        }

        [MetodoAuxiliar]
        public ICollection<AssinaturaTelefoneDTO> ConverterTelefone(ImportacaoSuspectDTO susp)
        {
            ICollection<AssinaturaTelefoneDTO> lstTelefones = new HashSet<AssinaturaTelefoneDTO>();

            if (!string.IsNullOrWhiteSpace(susp.IPS_TELEFONE))
            {
                var telefoneCheio = susp.IPS_TELEFONE;
                string ddd = null;
                string telefone = null;

                StringUtil.SepararTelefoneDoDDD(telefoneCheio, out ddd, out telefone);

                AssinaturaTelefoneDTO assTel = new AssinaturaTelefoneDTO()
                {
                    ATE_TELEFONE = telefone,
                    ATE_DDD = ddd,
                    TIPO_TEL_ID = 4
                };

                lstTelefones.Add(assTel);
            }

            if (!string.IsNullOrWhiteSpace(susp.IPS_FAX))
            {

                var faxCheio = susp.IPS_FAX;
                string ddd = null;
                string fax = null;

                StringUtil.SepararTelefoneDoDDD(faxCheio, out ddd, out fax);
                AssinaturaTelefoneDTO assTelFax = new AssinaturaTelefoneDTO()
                {
                    ATE_TELEFONE = fax,
                    ATE_DDD = ddd,
                    TIPO_TEL_ID = 2
                };

                lstTelefones.Add(assTelFax);
            }


            if (!string.IsNullOrWhiteSpace(susp.IPS_CELULAR))
            {

                var celularCheio = susp.IPS_CELULAR;
                string ddd = null;
                string celular = null;

                StringUtil.SepararTelefoneDoDDD(celularCheio, out ddd, out celular);
                AssinaturaTelefoneDTO assTelCel = new AssinaturaTelefoneDTO()
                {
                    ATE_TELEFONE = celular,
                    ATE_DDD = ddd,
                    TIPO_TEL_ID = 1
                };

                lstTelefones.Add(assTelCel);
            }

            return lstTelefones;

        }
        
        [MetodoAuxiliar]
        public ICollection<AssinaturaEmailDTO> ConverterEmail(ImportacaoSuspectDTO susp)
        {
            ICollection<AssinaturaEmailDTO> lstEmail = new HashSet<AssinaturaEmailDTO>();

            if (!string.IsNullOrWhiteSpace(susp.IPS_EMAIL))
            {
                var email = new AssinaturaEmailDTO()
                {
                    AEM_EMAIL = susp.IPS_EMAIL                    
                };

                lstEmail.Add(email);
            }

            return lstEmail;
        }

        public InfoMarketingDTO ConverterInformacaoDeMarketing(string origemCadastroDesc, string produtoInteresseDesc, string areaDeInteresse)
        {
            InfoMarketingDTO infoMkt = new InfoMarketingDTO();

            if (!string.IsNullOrWhiteSpace(origemCadastroDesc))
            {

                var origemCadastro = _origemCadastroSRV.ObterOrigemCadastroPorNome(origemCadastroDesc);
                if (origemCadastro != null)
                {
                    infoMkt.ORIGEM_CADASTRO = origemCadastro;
                    infoMkt.O_CAD_ID = origemCadastro.O_CAD_ID;
                }
                else
                {
                    throw new Exception(string.Format("Não é possível encontrar a origem de cadastro {0}", origemCadastroDesc));
                }
            }

            if (!string.IsNullOrWhiteSpace(produtoInteresseDesc))
            {
                var produtoDeInteresse = _produtoComposicaoSRV.ObterProdutoDeInteressePorNome(produtoInteresseDesc);

                if (produtoDeInteresse == null)
                {
                    throw new Exception(string.Format("Não é possível encontrar o produto de origem {0}", produtoInteresseDesc));                
                }

                var infoMktProdutoInteresse = new ProdutoComposicaoInfoMarketingDTO()
                {
                    CMP_ID = produtoDeInteresse.CMP_ID,
                    DATA_ASSOCIACAO = DateTime.Now,
                    PRODUTO_COMPOSICAO = produtoDeInteresse
                };

                infoMkt.PRODUTO_COMPOSICAO_INFO_MARKETING.Add(infoMktProdutoInteresse);
            }

            if (!string.IsNullOrWhiteSpace(areaDeInteresse))
            {
                var area = _areasSRV.ObterAreasPorNome(areaDeInteresse);

                if (area == null)
                {
                    throw new Exception(string.Format("Não é possível encontrar a área de interesse {0}", areaDeInteresse));
                }

                var areaInteresseInfoMkt = new AreaInfoMarketingDTO()
                {
                    AREA_ID = area.AREA_ID,
                    AREAS = area,
                    DATA_ASSOCIACAO = DateTime.Now,                    
                };

                infoMkt.AREA_INFO_MARKETING.Add(areaInteresseInfoMkt);
            }

            return infoMkt;
        }

        public IList<ImportacaoSuspectDTO> SepararSuspectsQueJaSaoClientes(IEnumerable<ImportacaoSuspectDTO> lstSuspect, IEnumerable<ClienteDto> clientes)
        {
            IList<ImportacaoSuspectDTO> lstSuspectsResposta = new List<ImportacaoSuspectDTO>();
            var comparator = new IClienteComparator();

            foreach (var sus in lstSuspect)
            {
                bool existe = false;
                foreach (var cli in clientes)
                {
                    var exi = comparator.Equals(sus, cli);
                    existe = existe || exi;
                    
                    if (existe)
                        break;
                }

                if (!existe)
                {
                    lstSuspectsResposta.Add(sus);
                }
            }

            return lstSuspectsResposta;
        }

        /// <summary>
        /// Busca no banco os clientes que já existem (por cnpj, telefone, email) e separa os clientes já existentes e os que não existem.
        /// </summary>
        /// <param name="lstClientes"></param>
        public ClienteDto VerificarSeEhClienteERetornar(ImportacaoSuspectDTO susp)
        {
            if (susp != null)
            {
                IList<string> lstCnpf = new List<string>();

                if (!string.IsNullOrWhiteSpace(susp.IPS_CPF_CNPJ))
                {
                    lstCnpf.Add(susp.IPS_CPF_CNPJ);
                }

                IList<string> lstTel = new List<string>();

                if(!string.IsNullOrWhiteSpace(susp.IPS_TELEFONE))
                {
                    lstTel.Add(susp.IPS_TELEFONE);    
                }

                if (!string.IsNullOrWhiteSpace(susp.IPS_FAX))
                {
                    lstTel.Add(susp.IPS_FAX);
                }

                if (!string.IsNullOrWhiteSpace(susp.IPS_CELULAR))
                {
                    lstTel.Add(susp.IPS_CELULAR);
                }

                IList<string> lstEmail = new List<string>();

                if (!string.IsNullOrWhiteSpace(susp.IPS_EMAIL))
                {
                    lstEmail.Add(susp.IPS_EMAIL);
                }

                var cli = _clienteSRV.BuscarClientesJaExistentes(lstCnpf, lstTel, lstEmail);

                if (cli != null)
                {
                    _assinaturaEmalSRV.PreencherEmailAssinaturaNoCliente(cli);
                    _assinaturaTelefoneSRV.PreencherTelefoneAssinaturaNoCliente(cli);
                    _infoMkt.PreencherInformacoesDeMarketing(cli);
                    ProcessarAtributosDoCliente(cli, susp);
                    AtribuirRegiao(susp, cli);
                }

                return cli;
            }

            return null;
        }

        /// <summary>
        /// Verifica quais atributos não existem no cliente mas existem nas no suspects
        /// </summary>
        /// <param name="lstClientes"></param>
        /// <param name="susp"></param>
        public void ProcessarAtributosDoCliente(ClienteDto cli, ImportacaoSuspectDTO susp)
        {
            if (cli != null && susp != null)
            {

                if (string.IsNullOrWhiteSpace(cli.CLI_CPF_CNPJ) && !string.IsNullOrWhiteSpace(susp.CNPJ_CPF))
                {
                    cli.CLI_CPF_CNPJ = susp.CNPJ_CPF;
                }

                var lstTelefone = ConverterTelefone(susp);
                var lstEmail = ConverterEmail(susp);
                var infoMkt = ConverterInformacaoDeMarketing(susp.IPS_ORIGEM_CADASTRO, susp.IPS_PRODUTO_INTERESSE, susp.IPS_AREA_INTERESSE);

                var lstTelefoneCli = cli.ASSINATURA_TELEFONE;
                var lstEmailCLi = cli.ASSINATURA_EMAIL;
                var infMktCli = cli.INFO_MARKETING;

                var lstTelefonesNovos = lstTelefone.Where(x => !lstTelefoneCli.
                    Select(sel => sel.ATE_DDD + sel.ATE_TELEFONE).
                    Contains(x.ATE_DDD + x.ATE_TELEFONE));

                var lstEmailsNovos = lstEmail.Where(x => !lstEmailCLi.
                    Select(sel => sel.AEM_EMAIL).
                    Contains(x.AEM_EMAIL));

                if (infMktCli == null)
                {
                    infMktCli = new InfoMarketingDTO();
                    cli.INFO_MARKETING = infMktCli;
                }

                var lstProdutoCompostoProdutoComposto = infoMkt.PRODUTO_COMPOSICAO_INFO_MARKETING;
                var lstProdutoInteresse = lstProdutoCompostoProdutoComposto.Where(x => infMktCli != null && infMktCli.PRODUTO_COMPOSICAO_INFO_MARKETING != null &&
                    !infMktCli.PRODUTO_COMPOSICAO_INFO_MARKETING.Select(sel => sel.PRODUTO_COMPOSICAO.CMP_ID).Contains(x.PRODUTO_COMPOSICAO.CMP_ID));

                lstTelefoneCli = lstTelefoneCli.Concat(lstTelefonesNovos).ToList();
                lstEmailCLi = lstEmailCLi.Concat(lstEmailsNovos).ToList();

                if (infMktCli.O_CAD_ID == null &&
                    infoMkt != null &&
                    infoMkt.O_CAD_ID != null)
                {
                    infMktCli.O_CAD_ID = infoMkt.O_CAD_ID;
                    infMktCli.ORIGEM_CADASTRO = infoMkt.ORIGEM_CADASTRO;
                }

                infMktCli.PRODUTO_COMPOSICAO_INFO_MARKETING = lstProdutoCompostoProdutoComposto.Concat(lstProdutoInteresse).ToList();

                cli.ASSINATURA_TELEFONE = lstTelefoneCli;
                cli.ASSINATURA_EMAIL = lstEmailCLi;
            }
        }
        //------------------------------------------------------------
        public IList<ImportacaoSuspectDTO> ListarSuspectsNaoImportados(int? impID)
        {
            return _dao.ListarSuspectsNaoImportados(impID);
        }

        public IList<ImportacaoSuspectDTO> ListarSuspectsNaoImportadosComHistorico(int? impID)
        {
            var lstSuspects = ListarSuspectsNaoImportados(impID);

            if(lstSuspects != null)
            {
                foreach(var sus in lstSuspects)
                {
                    var hist = _importacaoHistoricoSRV.BuscarUltimoHistoricoDeErro(sus.IPS_ID);
                    if (hist != null)
                    {
                        sus.UltimoHistoricoRegistrado = string.Format(@"{0:dd/MM/yyyy - HH:mm:ss} - {1}", hist.IMH_DATA, hist.IMH_DESCRICAO)
                            .Replace("<br />", "")
                            .Trim();
                        sus.Erros = "Veja o erro no Comentário.";
                    }
                }
            }

            return lstSuspects;
        }

        public void ImportarClientes(ImportacaoDTO importacao, IEnumerable<ImportacaoSuspectDTO> lstImportacaoSuspect, ContextoImportacaoDTO contextoImportacao)
        {
            if (lstImportacaoSuspect != null && lstImportacaoSuspect.Count() > 0)
            {
                var qtdTotal = lstImportacaoSuspect.Count();

                lstImportacaoSuspect = lstImportacaoSuspect.Distinct(GetComparator());

                importacao.IMP_QTD_SUS_TOTAL = qtdTotal;
                importacao.IMP_QTD_REAL_SUS = lstImportacaoSuspect.Count();
                importacao.IMP_QTD_SUS_DUPLICADA = qtdTotal - lstImportacaoSuspect.Count();
                _importacaoSRV.Merge(importacao);

                contextoImportacao.BatchContext.IniciarPassoBatch("Importando cliente", true, lstImportacaoSuspect.Count());

                foreach (var sus in lstImportacaoSuspect)
                {
                    ImportarCliente(importacao, sus, contextoImportacao);
                }
            }

        }
        public void ImportarCliente(ImportacaoDTO importacao, 
            ImportacaoSuspectDTO importacaoSus, 
            ContextoImportacaoDTO contextoImportacao)
        {
            if(importacaoSus != null)
            {
                try
                {
                    contextoImportacao.ImportacaoSuspect = importacaoSus;
                    
                    ClienteDto clienteEncontrado = null;
                    using (var scope = new TransactionScope())
                    {
                        clienteEncontrado = VerificarSeEhClienteERetornar(importacaoSus);
                        if (clienteEncontrado != null)
                        {
                            clienteEncontrado.IPS_ID = importacaoSus.IPS_ID;
                        }
                        else
                        {
                            clienteEncontrado = ConverterParaCliente(importacaoSus, 0);                            
                        }

                        _clienteSRV.SalvarClientesImportacao(clienteEncontrado , contextoImportacao, importacao.REP_ID);

                        if (importacao.IMP_QTD_PROC_SUCCESSO == null)
                            importacao.IMP_QTD_PROC_SUCCESSO = 0;
                        
                        importacao.IMP_QTD_PROC_SUCCESSO++;
                        contextoImportacao.BatchContext.AdicionarContagemSucesso();
                        _importacaoSRV.Merge(importacao);

                        importacaoSus.IMP_DATA_ULTIMA_EXECUCAO = DateTime.Now;
                        importacaoSus.IMS_ID = 4;
                        Merge(importacaoSus);

                        string mensagem = string.Format("O Cliente foi Importado com Sucesso! O cliente recebeu o código {0}", clienteEncontrado.CLI_ID);
                        _importacaoHistoricoSRV.IncluirHistoricoImportacaoSuspect(mensagem, importacaoSus);
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    importacaoSus.IMS_ID = 2;
                    importacaoSus.IMP_DATA_ULTIMA_EXECUCAO = DateTime.Now;
                    Merge(importacaoSus);

                    if (importacao.IMP_QTD_PROC_FALHA == null)
                        importacao.IMP_QTD_PROC_FALHA = 0;
                    importacao.IMP_QTD_PROC_FALHA++;
                    _importacaoSRV.Merge(importacao);
                    contextoImportacao.BatchContext.AdicionarContagemFalha();

                    _importacaoHistoricoSRV.IncluirHistoricoErroImportacaoSuspect(importacaoSus, ex);
                }
                finally
                {
                    contextoImportacao.BatchContext.IncrementarPassoBatch();
                }
            }
        }

        public Pagina<ImportacaoSuspectDTO> PesquisarImportacaoSuspects(PesquisaImportacaoSuspectDTO pesquisaImportacao)
        {
            return _dao.PesquisarImportacaoSuspects(pesquisaImportacao);
        }

        public FileInfoDTO RetornarPlanilhaImportacaoNaoProcessada(int? impID, string path)
        {
            try
            {
                var lstSuspects = ListarSuspectsNaoImportadosComHistorico(impID);

                if(lstSuspects != null && lstSuspects.Count > 0)
                {
                    var fileName = string.Format(@"{0}download\suspects-com-erro-importacao-{1:yyyy-MM-dd hh-mm-ss}.xlsx", path, DateTime.Now);

                    using (ExcelProxyOpenXML excelLoad = new ExcelProxyOpenXML())
                    {
                        excelLoad.ToSheet(fileName, lstSuspects);
                    };
                    
                    var bytes =  File.ReadAllBytes(fileName);
                    var downloadInfo = new FileInfoDTO()
                    {
                        Path = fileName,
                        Bytes = bytes
                    };

                    File.Delete(fileName);
                    return downloadInfo;
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception("Não é possível gerar a planilha", e);
            }
        }
                        
    }
}

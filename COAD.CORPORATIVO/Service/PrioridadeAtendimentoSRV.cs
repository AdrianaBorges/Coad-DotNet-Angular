using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Transactions;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Service.Formatting;
using COAD.CORPORATIVO.Service.Custons;
using GenericCrud.Models.MessageFormatter;
using COAD.CORPORATIVO.Model.Dto.Formatters;
using COAD.SEGURANCA.Service.Custons.Context;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PRI_ID")]
    public class PrioridadeAtendimentoSRV : GenericService<PRIORIDADE_ATENDIMENTO, PrioridadeAtendimentoDTO, int>
    {
        private PrioridadeAtendimentoDAO _dao;
        public RepresentanteSRV _representanteSRV { get; set; }
        public RegiaoSRV _regiaoSRV { get; set; }
        public MessageFormatterService formatter { get; set; }
        public InfoMarketingSRV _infoMkt { get; set; }

        public PrioridadeAtendimentoSRV(PrioridadeAtendimentoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
            this.formatter = FormatterServiceLocalFactory.CriarMessageFormatterServiceCoorporativo();
        }

        public PrioridadeAtendimentoSRV()
        {
            this._dao = new PrioridadeAtendimentoDAO();
            this._representanteSRV = new RepresentanteSRV();
            this._regiaoSRV = new RegiaoSRV();
            this.formatter = new MessageFormatterService();
            this._infoMkt = new InfoMarketingSRV();
            this.formatter = FormatterServiceLocalFactory.CriarMessageFormatterServiceCoorporativo();
        
            this.Dao = _dao;
        }


        public void RegistrarPrioridade(int REP_ID, int CLI_ID, int TP_PRI_ID, string nota = null, int? REP_ID_DEMANDANTE = null)
        {
            var RG_ID = _regiaoSRV.ObterRgIdDoRepresentante(REP_ID);

            PrioridadeAtendimentoDTO prioridadeAtendimento = new PrioridadeAtendimentoDTO()
            {
                CLI_ID = CLI_ID,
                PRI_DATA = DateTime.Now,
                REP_ID = REP_ID,
                TP_PRI_ID = TP_PRI_ID,
                PRI_NOTA = nota,
                REP_ID_DEMANDANTE = REP_ID_DEMANDANTE,
                RG_ID = RG_ID
            };

            Save(prioridadeAtendimento);
        }

        /// <summary>
        /// Confirma o atendimento de prioridade
        /// Geralmente usado quando precisa dar baixa na prioridade de um cliente
        /// ao registrar o atendimento
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <param name="CLI_ID"></param>
        public void ConfirmarAtendimentoDePrioridade(int REP_ID, int CLI_ID)
        {
            var RG_ID = _regiaoSRV.ObterRgIdDoRepresentante(REP_ID);
 
            var lstPrioridades = GetPrioridadesByRepresentanteEOperadoras(REP_ID, CLI_ID, RG_ID);

            if (lstPrioridades != null)
            {
                foreach (var prioridade in lstPrioridades)
                {
                    prioridade.PRI_DATA_CONFIRMACAO = DateTime.Now;
                }

                MergeAll(lstPrioridades);
            }
        }

        /// <summary>
        /// Confirma todas as prioridades em uma região do cliente passado.
        /// Geralmente usado para retirar todas as prioridades de um cliente antes de um
        /// reencarteiramento.
        /// </summary>
        /// <param name="RG_ID">Região dos representantes que terão as prioridade confirmadas</param>
        /// <param name="CLI_ID">Id do cliente vinculado as prioridades da operadora.</param>
        public void ConfirmarAtendimentoDePrioridadeDaRegiaoEDoCliente(int? RG_ID, int CLI_ID)
        {
            var lstPrioridades = GetPrioridadesByRegiaoDoRepresentanteECliente(RG_ID, CLI_ID);

            if (lstPrioridades != null)
            {
                foreach (var prioridade in lstPrioridades)
                {
                    prioridade.PRI_DATA_CONFIRMACAO = DateTime.Now;
                }

                MergeAll(lstPrioridades);
            }
        }


        /// <summary>
        /// Confirma todas as prioridades do cliente passado.
        /// Geralmente usado para retirar todas as prioridades de um cliente antes de um
        /// reencarteiramento.
        /// </summary>
        /// <param name="RG_ID">Região dos representantes que terão as prioridade confirmadas</param>
        /// <param name="CLI_ID">Id do cliente vinculado as prioridades da operadora.</param>
        public void ConfirmarAtendimentoDePrioridadeDoClientePorRegiaoRepresentante(int? REP_ID, int CLI_ID)
        {
            var representante = _representanteSRV.FindById(REP_ID);

            if (representante != null && representante.RG_ID != null)
            {
                var RG_ID = representante.RG_ID;
                ConfirmarAtendimentoDePrioridadeDaRegiaoEDoCliente(RG_ID, CLI_ID);
            }
            
        }


        /// <summary>
        /// Devolve a prioridade de atendimento do representante.
        /// Além disso, verifica se o representante requerente está na mesma região do representante que ele quer visualizar
        /// </summary>
        /// <param name="REP_REQUERENTE_ID">Id do representante que precisa visualizar essa informação</param>
        /// <param name="REP_ID"></param>
        /// <param name="pagina"></param>
        /// <param name="registrosPorPagina"></param>
        /// <returns></returns>
        public Pagina<PrioridadeAtendimentoDTO> GetPrioridadesByRepresentante(int? REP_REQUERENTE_ID, int? REP_ID, int pagina = 1, int registrosPorPagina = 5, int? RG_ID = null)
        {
            if (_representanteSRV.RepresentantesExistemNaMesmaRegiao(REP_REQUERENTE_ID, REP_ID))
            {
                return _dao.GetPrioridadesByRepresentante(REP_ID, pagina, registrosPorPagina, RG_ID);
            }

            throw new AcessoADadosNaoPermitidoException("Você está tentado acessar dados de um representante que não pertence a sua região");

            
        }

        public Pagina<PrioridadeAtendimentoDTO> GetPrioridadesByRepresentante(int? REP_ID, int pagina = 1, int registrosPorPagina = 5, int? RG_ID = null)
        {
            return _dao.GetPrioridadesByRepresentante(REP_ID, pagina, registrosPorPagina, RG_ID);
        }

        public IList<PrioridadeAtendimentoDTO> GetPrioridadesByRepresentanteEOperadoras(int? REP_ID, int? CLI_ID, int? RG_ID = null)
        {
            return _dao.GetPrioridadesByRepresentanteEOperadoras(REP_ID, CLI_ID, RG_ID);
        }

        /// <summary>
        /// Retorna todas as prioridade de um cliente nas carteiras da região passada
        /// </summary>
        /// <param name="RG_ID">Id da região onde as prioridades serão pesquisadas</param>
        /// <param name="CLI_ID">Id do cliente onde as prioridades serão pesquisadas</param>
        /// <returns></returns>
        public IList<PrioridadeAtendimentoDTO> GetPrioridadesByRegiaoDoRepresentanteECliente(int? RG_ID, int? CLI_ID)
        {
            return _dao.GetPrioridadesDoClienteByRegiaoDoRepresentante(RG_ID, CLI_ID);
        }

        /// <summary>
        /// Retorna todas as prioridade de um cliente
        /// </summary>
        /// <param name="CLI_ID">Id do cliente onde as prioridades serão pesquisadas</param>
        /// <returns></returns>
        public IList<PrioridadeAtendimentoDTO> GetPrioridadesDoCliente(int? RG_ID)
        {
            return _dao.GetPrioridadesDoCliente(RG_ID);
        }


        /// <summary>
        /// Retorna todos os clientes encaminhados
        /// </summary>
        /// <param name="data"></param>
        /// <param name="UEN_ID"></param>
        /// <param name="RG_ID"></param>
        /// <param name="pagina"></param>
        /// <param name="registrosPorPagina"></param>
        /// <returns></returns>
        public Pagina<PrioridadeAtendimentoDTO> ClientesComPrioridadeEncaminhados(DateTime data, int? UEN_ID = null, int? RG_ID = null, int pagina = 1, int registrosPorPagina = 100)
        {
            return _dao.ClientesComPrioridadeEncaminhados(data, UEN_ID, RG_ID, pagina, registrosPorPagina);
        }

        public void CriarPrioridadesAtendimento(
            IEnumerable<ClienteDto> lstClientes,
            int? rgID,
            ContextoImportacaoDTO context,
            int? REP_ID_DEMANDANTE = null)
        {
            if (lstClientes != null)
            {
                //contexto.IniciarPassoBatch("Gerando prioridades...", true);

                int? tipoPrioridade = null;

                var _carteiramento = new CarteiramentoSRV();
                var lstPrioridadeAtendimento = new List<PrioridadeAtendimentoDTO>();

                var lstIds = lstClientes
                    .Where(x => x.CLI_ID != null)
                    .Select(sel => sel.CLI_ID).ToList();

                var lstCarteiraCliente = _carteiramento.ListarCarteiramentoDoClientesPorRegiao(lstIds, rgID);
                var repIdFormatter = new RepIdFormatter();
                var repreEncaminhamento = repIdFormatter.Format(REP_ID_DEMANDANTE);

                //contexto.BatchStatus.TotalItens = lstCarteiraCliente.Count();

                var data = DateTime.Now;
                foreach (var carCli in lstCarteiraCliente)
                {
                    _infoMkt.PreencherInformacoesDeMarketing(lstClientes);
                    var cliente = lstClientes.Where(x => x.CLI_ID == carCli.CLI_ID).FirstOrDefault();

                    string produtoInteresseStr = "Não informado";
                    string areaInteresse = "Não informado";
                    string areaOrigemStr = "Não informado";
                    string telefonesStr = "Não informado";
                    string emailsStr = "Não informado";
                    string comentarioCliente = "Não informado";
                    var importacao = context.Importacao;
                    var importacaoSuspect = context.ImportacaoSuspect;

                    if (cliente != null)
                    {
                        if (importacaoSuspect != null)
                        {
                            if (!string.IsNullOrWhiteSpace(importacaoSuspect.IPS_COMENTARIO_CLIENTE))
                            {
                                comentarioCliente = importacaoSuspect.IPS_COMENTARIO_CLIENTE;
                            }

                            var tipoImportacao = importacaoSuspect.IPS_TIPO_IMPORTACAO;

                            if (string.IsNullOrWhiteSpace(tipoImportacao))
                                tipoPrioridade = 5;
                            else if (tipoImportacao.Trim().ToUpper() == "LEAD")
                                tipoPrioridade = 5;
                            else if (tipoImportacao.Trim().ToUpper() == "ATIVO")
                                tipoPrioridade = 6;
                            else
                            {
                                string erro = "O tipo de Importação '{0}' é inválido. Informe 'LEAD' ou não preencha esse campo para importação normal e 'ATIVO' para marcar importação como ativo";
                                erro = string.Format(erro, tipoImportacao);
                                 throw new Exception(erro);
                            }
                        }

                        var infoMkt = cliente.INFO_MARKETING;
                        var telefones = cliente.ASSINATURA_TELEFONE;
                        var emails = cliente.ASSINATURA_EMAIL;
                        
                        if (infoMkt != null)
                        {
                            if (infoMkt.ORIGEM_CADASTRO != null)
                            {
                                areaOrigemStr = infoMkt.ORIGEM_CADASTRO.O_CAD_DESCRICAO;
                            }

                            if (infoMkt.PRODUTO_COMPOSICAO_INFO_MARKETING != null)
                            {
                                produtoInteresseStr = null;
                                foreach (var pro in infoMkt.PRODUTO_COMPOSICAO_INFO_MARKETING)
                                {
                                    if (pro.PRODUTO_COMPOSICAO != null)
                                    {
                                        produtoInteresseStr += pro.PRODUTO_COMPOSICAO.CMP_DESCRICAO + ", ";
                                    }
                                }
                            }

                            if (infoMkt.AREA_INFO_MARKETING != null)
                            {
                                areaInteresse = null;
                                foreach (var are in infoMkt.AREA_INFO_MARKETING)
                                {

                                    if (are.AREAS != null)
                                    {
                                        areaInteresse += are.AREAS.AREA_NOME + ", ";
                                    }
                                }
                            }
                        }

                        if (telefones != null)
                        {
                            telefonesStr = null;
                            foreach (var tel in telefones)
                            {
                                telefonesStr += tel.ATE_DDD + tel.ATE_TELEFONE + ", ";
                            }
                        }

                        if (emails != null)
                        {
                            emailsStr = null;
                            foreach (var email in emails)
                            {
                                emailsStr += email.AEM_EMAIL + ", ";
                            }
                        }
                        
                    }
                    
                    var mensagem = @"O cliente {0} acaba ser adicionado a sua prioridade por importação gerada por administrador do sistema {1} com os seguintes campos.                        
                        emails: ({2});
                        telefones: ({3});
                        origem de cadastro: ({4});
                        produto de interesse: ({5});
                        área de intesse ({6});
                        Comentário do Cliente '{7}'
                    ";

                    mensagem = string.Format(mensagem, carCli.NomeCliente, repreEncaminhamento, emailsStr, telefonesStr, areaOrigemStr, produtoInteresseStr, areaInteresse, comentarioCliente);
                    
                    var prioridadeAtendimento = new PrioridadeAtendimentoDTO()
                    {
                        CLI_ID = carCli.CLI_ID,
                        REP_ID = carCli.REP_ID,
                        RG_ID = carCli.RG_ID,
                        PRI_NOTA = mensagem,
                        PRI_DATA = data,
                        REP_ID_DEMANDANTE = REP_ID_DEMANDANTE,
                        TP_PRI_ID = tipoPrioridade
                    };

                    lstPrioridadeAtendimento.Add(prioridadeAtendimento);

                    if (importacao != null)
                    {
                        string usuario = "sistema";
                        if (!string.IsNullOrWhiteSpace(importacao.USU_LOGIN))
                            usuario = importacao.USU_LOGIN;


                        ServiceFactory.RetornarServico<HistAtendSRV>().RegistrarHistorico(DateTime.Now, mensagem, usuario, carCli.REP_ID, carCli.CLI_ID, 12);
                        ServiceFactory.RetornarServico<ImportacaoResultadoRodizioSRV>().AdicionarResultado(importacao.IMP_ID, carCli.REP_ID, carCli.RG_ID, null, 1);
                    }

                }

                if (lstPrioridadeAtendimento != null)
                {
                    BulkInsert(lstPrioridadeAtendimento);
                }

                
            }
        }
    }
}


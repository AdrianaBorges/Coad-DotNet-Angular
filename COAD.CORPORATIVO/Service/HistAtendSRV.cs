using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.UTIL.Grafico;
using COAD.UTIL.Grafico.Base;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("HAT_ID")]
    public class HistAtendSRV : ServiceAdapter<HISTORICO_ATENDIMENTO, HistoricoAtendimentoDTO>
    {
        public HistAtendDAO _dao { get; set; }
        public RepresentanteSRV _representanteService { get; set; }
        public RegiaoSRV _regiaoSRV { get; set; }
        public UENSRV _uenSRV { get; set; }
        
        public HistAtendSRV()
        {
            _dao = new HistAtendDAO();
            this.Dao = _dao;

            this._representanteService = new RepresentanteSRV();
            this._regiaoSRV = new RegiaoSRV();
            this._uenSRV = new UENSRV();
        }

        public HistAtendSRV(HistAtendDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public IList<BUSCAR_HSITORICO_ATEND_SAC_CABEC> BuscarAtendimentoPorTipo(DateTime? _dtini = null, DateTime? _dtfim = null, int _uen_id = 0)
        {
            return _dao.BuscarAtendimentoPorTipo(_dtini, _dtfim, _uen_id);
        }
        public IList<HistoricoAtendimentoDTO> BuscarPorAssinatura(string _assinatura = null)
        {
            return _dao.BuscarPorAssinatura(_assinatura);
        }
        public IList<HistoricoAtendimentoDTO> BuscarPorCliente(int _cli_id)
        {
            return _dao.BuscarPorCliente(_cli_id);
        }
        public IList<HistoricoAtendimentoDTO> BuscarEtiquetas()
        {
            return _dao.BuscarEtiquetas();
        }
        public IList<HistoricoAtendimentoDTO> BuscarEtiquetas(DateTime _dtini, DateTime _dtfim)
        {
            return _dao.BuscarEtiquetas(_dtini, _dtfim);
        }
        public Pagina<HistoricoAtendimentoDTO> BuscarPorCliente(int _cli_id, int pagina = 1, int registroPorPagina = 10)
        {
            return _dao.BuscarPorCliente(_cli_id, pagina, registroPorPagina);
        }
        public IList<HistoricoAtendimentoDTO> BuscarPorPeriodo(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, bool _etiqueta = false)
        {
            return _dao.BuscarPorPeriodo(_asn_id, _dtini, _dtfim, _etiqueta);
        }
        public Pagina<HistoricoAtendimentoDTO> BuscarPorPeriodo(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, bool _etiqueta = false, int pagina = 1, int registroPorPagina = 7)
        {
            return _dao.BuscarPorPeriodo(_asn_id, _dtini, _dtfim, _etiqueta, pagina, registroPorPagina);
        }
        public Pagina<HistoricoAtendimentoDTO> FindHistoricoByCliId(int CLI_ID, DateTime? dataInicial = null, DateTime? dataFinal = null, int pagina = 1, int registroPorPagina = 10, int? UEN_ID = 1)
        {
            return _dao.FindHistoricoByCliId(CLI_ID, dataInicial, dataFinal, pagina, registroPorPagina, UEN_ID);
        }

        public IList<HistoricoAtendimentoDTO> FindHistoricoByCliIdSemPaginacao(int CLI_ID, DateTime? dataInicial = null, DateTime? dataFinal = null, int? UEN_ID = 1)
        {
            return _dao.FindHistoricoByCliIdSemPaginacao(CLI_ID, dataInicial, dataFinal, UEN_ID);
        }

        public IList<HistoricoAtendimentoDTO> FindHistoricosByAgendamento(int AGE_ID)
        {
            return _dao.FindHistoricosByAgendamento(AGE_ID);
        }

        public void MarcarEtiquetasEmitidas()
        {
            try
            {
                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    var query = this.BuscarEtiquetas();

                    foreach (var item in query)
                    {
                        item.HAT_DATA_RESOLUCAO = DateTime.Now;
                    }

                    this.MergeAll(query);

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao enviar o email " + ex.Message);
            }

        }


        public void PreencherHistorico(AgendamentoDTO agendamento)
        {
            if (agendamento != null && agendamento.AGE_ID != null)
            {
                var AGE_ID = agendamento.AGE_ID;
                var historico = FindHistoricosByAgendamento((int) AGE_ID);
                agendamento.HISTORICO_ATENDIMENTO = historico;
            }
        }

        public void SalvarHistoricoAtendimentoDoAgendamento(AgendamentoDTO agenda, string usuario = null, int ACA_ID = 6)
        {
            var data = DateTime.Now;
            if (agenda != null && agenda.HISTORICO_ATENDIMENTO != null)
            {
                foreach (var hist in agenda.HISTORICO_ATENDIMENTO)
                {
                    int? UEN_ID = _uenSRV.ObterUenIdDoRepresentante(agenda.REP_ID);
                    if (hist.AGE_ID == null)
                    {
                        hist.AGE_ID = agenda.AGE_ID;
                        hist.ACA_ID = ACA_ID;
                        hist.HAT_DATA_HIST = data;
                        hist.HAT_DATA_RESOLUCAO = data;
                        hist.HAT_IMP_ETIQUETA = false;
                        hist.TIP_ATEND_ID = 6;
                        hist.UEN_ID = UEN_ID;
                        hist.REP_ID = agenda.REP_ID;

                        if(!string.IsNullOrEmpty(usuario)){

                            hist.USU_LOGIN = usuario;
                        }                        

                    }
                }

                if (agenda.CLI_ID != null)
                {
                    new ClienteSRV().AtualizarDataAlteracao(agenda.CLI_ID, data);
                }

                InserirHistoricoAtendimento(agenda.HISTORICO_ATENDIMENTO);
            }
        }

        /// <summary>
        /// Insere um histórico de atendimento para contato realizado
        /// </summary>
        /// <param name="date">Data do atendimento</param>
        /// <param name="observacao">Descrição do histórico</param>
        /// <param name="usuario">O usuário que gerou o histórico</param>
        /// <param name="REP_ID">Id do representante que gerou o histórico</param>
        /// <param name="CLI_ID">Id do cliente ao qual o histórico será anexado.</param>
        /// <param name="ACA_ID">Tipo de histórico de atendimento</param>
        public void InserirHistoricoAtendimento(DateTime? date, string observacao, string usuario, int? REP_ID, int? CLI_ID, int ACA_ID = 6,int? PED_CRM_ID = null)
        {
            if (CLI_ID != null)
            {
                string obs = "Contato Realizado: '{0}'";
                string observacaoFinal = string.Format(obs, observacao);

                //IList<AssinaturaDTO> listAss = new AssinaturaSRV().FindAssinaturaFranquiaPorCliente((int) CLI_ID);

                //if (listAss != null && listAss.Count() >= 1)
                //{
                //    var clientes = listAss.FirstOrDefault();

                //    HistoricoAtendimentoDTO hist = new HistoricoAtendimentoDTO();
                //    hist.ACA_ID = ACA_ID;
                //    hist.HAT_DATA_HIST = date;
                //    hist.HAT_DATA_RESOLUCAO = date;
                //    hist.HAT_IMP_ETIQUETA = false;
                //    hist.TIP_ATEND_ID = 6;
                //    hist.HAT_DESCRICAO = observacaoFinal;
                //    hist.ASN_NUM_ASSINATURA = clientes.ASN_NUM_ASSINATURA;
                //    hist.UEN_ID = 1;
                //    hist.REP_ID = REP_ID;

                //    if (!string.IsNullOrEmpty(usuario))
                //    {
                //        hist.USU_LOGIN = usuario;
                //    }

                //    InserirHistoricoAtendimento(new List<HistoricoAtendimentoDTO>() { hist });
                //}

                RegistrarHistorico(date, observacaoFinal, usuario, REP_ID, CLI_ID, ACA_ID, PED_CRM_ID);
            }
                       
        }

        /// <summary>
        /// Registra um histórico de atendimento.
        /// Serve para qualquer tipo de histórico.
        /// Esse método é um método geral que pode ser usado como base 
        /// para métodos mais especializados.
        /// </summary>
        /// <param name="date">Data do atendimento</param>
        /// <param name="observacao">Descrição do histórico</param>
        /// <param name="usuario">O usuário que gerou o histórico</param>
        /// <param name="REP_ID">Id do representante que gerou o histórico</param>
        /// <param name="CLI_ID">Id do cliente ao qual o histórico será anexado.</param>
        /// <param name="ACA_ID">Tipo de histórico de atendimento</param>
        public void RegistrarHistorico(
            DateTime? date, 
            string observacao, 
            string usuario, 
            int? REP_ID, 
            int? CLI_ID, 
            int? ACA_ID = 6, 
            int? PED_CRM_ID = null, 
            int? IPE_ID = null,
            int? TIP_ATEND_ID = 6,
            int? ppiId = null)
        {
            if (CLI_ID != null)
            {
                //AssinaturaDTO clientes = new AssinaturaSRV().FindPrimeiraAssinaturaFranquiaPorCliente((int)CLI_ID);

                 var UEN_ID = _uenSRV.ObterUenIdDoRepresentante(REP_ID);

                 if (UEN_ID == null)
                 {
                     UEN_ID = 1;
                 }
                 HistoricoAtendimentoDTO hist = new HistoricoAtendimentoDTO();
                    hist.ACA_ID = ACA_ID;
                    hist.HAT_DATA_HIST = date;
                    hist.HAT_DATA_RESOLUCAO = date;
                    hist.HAT_IMP_ETIQUETA = false;
                    hist.TIP_ATEND_ID = TIP_ATEND_ID;
                    hist.HAT_DESCRICAO = observacao;
                    hist.CLI_ID = CLI_ID;
                    hist.UEN_ID = UEN_ID;
                    hist.REP_ID = REP_ID;
                    hist.PED_CRM_ID = PED_CRM_ID;
                    hist.IPE_ID = IPE_ID;
                    hist.PPI_ID = ppiId;

                    if (!string.IsNullOrEmpty(usuario))
                    {
                        hist.USU_LOGIN = usuario;
                    }

                    ServiceFactory.RetornarServico<ClienteSRV>().AtualizarDataAlteracao(CLI_ID, date);
                    InserirHistoricoAtendimento(new List<HistoricoAtendimentoDTO>() { hist });
                
            }

        }

        public void InserirHistoricoAtendimento(IEnumerable<HistoricoAtendimentoDTO> lstHistoricoAtendimento)
        {
            if (lstHistoricoAtendimento != null)
            {
                SaveAll(lstHistoricoAtendimento);
            }
        }

        /// <summary>
        /// Método especializado para registrar o histórico na ação de reencarteirar um cliente.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando o reencarteiramento</param>
        /// <param name="REP_ID_RECEBEU_CLIENTE">Id do representante que recebeu o novo cliente. Seja ele por rodizo, ou recebimento direto.</param>
        /// <param name="CLI_ID">Id do cliente reencarteirado onde este histórico será anexado.</param>
        public void RegistraHistoricoDeReencarteiramento(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? REP_ID_RECEBEU_CLIENTE, int? CLI_ID)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);
            var representanteQueRecebeuOCliente = _representanteService.FindById(REP_ID_RECEBEU_CLIENTE);
            

            // Lógica de composição da string de descrição do histórico
            string nomeRepresentanteQueExecutouAAcao = 
                                (representanteQueExecutouAAcao != null  && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME)) 
                                    ?  representanteQueExecutouAAcao.REP_NOME :"(Nome indisponível)";


            string nomeRepresentanteQueRecebeuOCliente =
                                (representanteQueRecebeuOCliente != null && !string.IsNullOrWhiteSpace(representanteQueRecebeuOCliente.REP_NOME))
                                    ? representanteQueRecebeuOCliente.REP_NOME : "(Nome indisponível)";

            string descricao = @"Este cliente foi reencarteirado pelo(a) representante '{0}'. O cliente foi adicionado a carteira do(a) representante '{1}'";

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, nomeRepresentanteQueRecebeuOCliente);

            //Registrando o histórico
            RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 9);
        }


        /// <summary>
        /// Método especializado para registrar o histórico na ação de encaminhar um cliente.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando o encaminhamento</param>
        /// <param name="REP_ID_RECEBEU_CLIENTE">Id do representante que recebeu o novo cliente.</param>
        /// <param name="CLI_ID">Id do cliente reencarteirado onde este histórico será anexado.</param>
        public void RegistraHistoricoEncaminhamento(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? REP_ID_RECEBEU_CLIENTE, int? CLI_ID, string observacao = "")
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);
            var representanteQueRecebeuOCliente = _representanteService.FindById(REP_ID_RECEBEU_CLIENTE);


            // Lógica de composição da string de descrição do histórico
            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string nomeRepresentanteQueRecebeuOCliente =
                                (representanteQueRecebeuOCliente != null && !string.IsNullOrWhiteSpace(representanteQueRecebeuOCliente.REP_NOME))
                                    ? representanteQueRecebeuOCliente.REP_NOME : "(Nome indisponível)";

            string descricao = @"Este cliente foi encaminhado pelo(a) representante '{0}' para o representante {1} contendo as seguintes observações: {2}";

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, nomeRepresentanteQueRecebeuOCliente, observacao);

            //Registrando o histórico
            RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 11);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na ação de adicionar uma nova região ao cliente.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="REP_ID_RECEBEU_CLIENTE">Id do representante que recebeu o novo cliente.</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistraHistoricoDeAdicionarRegiao(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? REP_ID_RECEBEU_CLIENTE, int? CLI_ID, int? RG_ID)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);
            var representanteQueRecebeuOCliente = _representanteService.FindById(REP_ID_RECEBEU_CLIENTE);
            var regiaoAdicionado = _regiaoSRV.FindById(RG_ID);


            // Lógica de composição da string de descrição do histórico
            string nomeDaRegiao =
                                (regiaoAdicionado != null && !string.IsNullOrWhiteSpace(regiaoAdicionado.RG_DESCRICAO))
                                    ? regiaoAdicionado.RG_DESCRICAO : "(Nome indisponível)";

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string nomeRepresentanteQueRecebeuOCliente =
                                (representanteQueRecebeuOCliente != null && !string.IsNullOrWhiteSpace(representanteQueRecebeuOCliente.REP_NOME))
                                    ? representanteQueRecebeuOCliente.REP_NOME : "(Nome indisponível)";


            string descricao = @"Este cliente foi adicionado a região {0}, pelo(a) representante '{1}'. O cliente foi adicionado a carteira do(a) representante '{2}'";

            string mensagemFinal = string.Format(descricao, nomeDaRegiao, nomeRepresentanteQueExecutouAAcao, nomeRepresentanteQueRecebeuOCliente);

            //Registrando o histórico
            RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 10);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na ação de remover uma nova região ao cliente.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="REP_ID_RECEBEU_CLIENTE">Id do representante que recebeu o novo cliente.</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistraHistoricoDeRemocaoDaRegiao(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? CLI_ID, int? RG_ID)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);
            var regiaoAdicionado = _regiaoSRV.FindById(RG_ID);


            // Lógica de composição da string de descrição do histórico
            string nomeDaRegiao =
                                (regiaoAdicionado != null && !string.IsNullOrWhiteSpace(regiaoAdicionado.RG_DESCRICAO))
                                    ? regiaoAdicionado.RG_DESCRICAO : "(Nome indisponível)";

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";



            string descricao = @"Este cliente foi removido da região {0}, pelo(a) representante '{1}'.";

            string mensagemFinal = string.Format(descricao, nomeDaRegiao, nomeRepresentanteQueExecutouAAcao);

            //Registrando o histórico
            RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 14);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na ação de informar uma venda.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoVendaEfetuada(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? CLI_ID, int? PED_CRM_ID, string observacao = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);


            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string descricao = @"O representante {0} indicou que efetuou uma venda. Notas do representante. {1}'";

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, observacao);

            //Registrando o histórico
            RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 13, PED_CRM_ID);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na ação de criar o login único.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoCriacaoLoginUnico(int? CLI_ID, string login, IList<string> lstCodAssinatura, ICollection<RastreamentoAlteracaoLoginUnicoDTO> Rastreamentos)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações do cliente para registrar seus nomes no histórico
            var clienteAcao = ServiceFactory.RetornarServico<ClienteSRV>().FindById(CLI_ID);
            
            string clienteQueExecutouAAcao =
                                (clienteAcao != null && !string.IsNullOrWhiteSpace(clienteAcao.CLI_NOME))
                                    ? clienteAcao.CLI_NOME : "(Nome indisponível)";

            string cpfCliente = (clienteAcao != null && !string.IsNullOrWhiteSpace(clienteAcao.CLI_CPF_CNPJ))
                                    ? clienteAcao.CLI_CPF_CNPJ : "(CPF/CNPJ indisponível)";

            string descricao = @"O cliente {0} de CPF/CNPJ {1}. Criou o login único {2} e associou as seguintes assinaturas ({3})";
            string strAssinatura = null;
            string strRastreamento = null;

            if (lstCodAssinatura != null)
            {
                StringBuilder sb = new StringBuilder();

                int index = 0;
                int tamanho = lstCodAssinatura.Count();

                foreach (var ass in lstCodAssinatura)
                {
                    if (index > 0 && index < (tamanho))
                    {
                        sb.Append(", ");
                    }
                    sb.Append(ass);

                    index++;
                }

                strAssinatura = sb.ToString();
            }

            if (Rastreamentos != null)
            {
                StringBuilder sb = new StringBuilder();

                int index = 0;
                int tamanho = Rastreamentos.Count();

                foreach (var track in Rastreamentos)
                {
                    if (index > 0 && index < (tamanho))
                    {
                        sb.Append(", \n");
                    }
                    sb.Append("A assinatura: ");
                    sb.Append(track.CodAssinatura);
                    sb.Append(" Foi transferido do Cliente de Código: (");
                    sb.Append(track.CodClienteAnterior);
                    sb.Append(") \n Para o Cliente de Código: (");
                    sb.Append(track.CodClienteRecebido);
                    sb.Append("). ");

                    index++;
                }

                strRastreamento = sb.ToString();
            }
            
            string mensagemFinal = string.Format(descricao, clienteQueExecutouAAcao, cpfCliente, login, strAssinatura);

            //Registrando o histórico
            RegistrarHistorico(dataDeHj, mensagemFinal, null, null, CLI_ID, 21, null, null, 110);


            if (Rastreamentos != null && Rastreamentos.Count() > 0)
            {
                mensagemFinal = mensagemFinal + "Rastreamento da Assinatura: \n {0} .";
                mensagemFinal = string.Format(mensagemFinal, strRastreamento);
            }
            // Registrando o Log
            SysException.RegistrarLog(mensagemFinal, "", null);
        }
        

        /// <summary>
        /// Verifica no banco de dados se existe alguma ocorrência de histórico (por agendamento, ou pela clientes)
        /// para o cliente de Id passado.
        /// </summary>
        /// <param name="CLI_ID">Id do cliente testado</param>
        /// <returns></returns>
        public bool VerificarClientePossuiHistorico(int? CLI_ID)
        {
            return _dao.VerificarClientePossuiHistorico(CLI_ID);
        }

        public GraficoDataSource ListarAtendimentosRealizadosNoMes(DateTime? data, int? UEN_ID = 1)
        {
            if (data == null)
            {
                data = DateTime.Now;
            }

            var ano = ((DateTime)data).Year;
            var mes = ((DateTime)data).Month;

            var atendimentos = _dao.ListarAtendimentosRealizadosNoMes(ano, mes, UEN_ID);

            DefaultGraficoDataSource grafico = new DefaultGraficoDataSource();
            grafico.SetTitle("Atendimentos Realizados Mês");
            grafico.SetSubTitle("Atendimentos realizados por operadora no mês");
            grafico.data = atendimentos.ToList();

            return grafico;

        }

        public GraficoDataSource ListarAtendimentosRealizadosNoMesPorRegiao(DateTime? data, int? UEN_ID = 1)
        {
            if (data == null)
            {
                data = DateTime.Now;
            }

            var ano = ((DateTime)data).Year;
            var mes = ((DateTime)data).Month;

            var atendimentos = _dao.ListarAtendimentosRealizadosNoMesPorRegiao(ano, mes, UEN_ID);

            DefaultGraficoDataSource grafico = new DefaultGraficoDataSource();
            grafico.SetTitle("Atendimentos Realizados Mês");
            grafico.SetSubTitle("Atendimentos realizados por Região no mês");
            grafico.data = atendimentos.ToList();

            return grafico;

        }

        public IList<RelatorioAtendimentosXVendasEfetuadasDTO> ListarRelatorioAtendimentoXVendasPorRegiao(DateTime? data, int? UEN_ID = 1)
        {
            if (data == null)
            {
                data = DateTime.Now;
            }

            var ano = ((DateTime)data).Year;
            var mes = ((DateTime)data).Month;

            return _dao.ListarRelatorioAtendimentoXVendasPorRegiao(ano, mes, UEN_ID);
        }

        public MultiSerieGraficoDataSource GerarRelatorioAtendimentoXVendasPorRegiao(DateTime? data, int? UEN_ID = 1)
        {

            var listaRelatorios = ListarRelatorioAtendimentoXVendasPorRegiao(data, UEN_ID);

            if (listaRelatorios != null && listaRelatorios.Count() > 0)
            {
                var labels = listaRelatorios.Select(sel => new JsonGrafico() { label = sel.RG_DESCRICAO }).ToList();
                var atendimentos = listaRelatorios.Select(sel => new JsonGrafico() { intData = sel.QTD_ATENDIMENTOS }).ToList();
                var vendasRealizadas = listaRelatorios.Select(sel => new JsonGrafico() { intData = sel.QTD_VENDAS_REALIZADAS }).ToList();

                MultiSerieGraficoDataSource dataSource = new MultiSerieGraficoDataSource();
                dataSource.SetTitle("Atendimentos X Vendas");
                dataSource.SetSubTitle("Relatórios de Atendimentos X Vendas seguimentado por região");

                dataSource.InsertCategories().AddCategoryList(labels);

                var dataset1 = dataSource.InsertDataSet();
                dataset1.seriesName = "Atendimentos Realizados";
                dataset1.data = atendimentos;

                var dataset2 = dataSource.InsertDataSet();
                dataset2.seriesName = "Vendas Realizadas";
                dataset2.data = vendasRealizadas;
                dataset2.renderAs = "line";

                return dataSource;

            }

            return null;
        }

        /// <summary>
        /// Método especializado para registrar o histórico de um novo cliente importação.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_RECEBEU_CLIENTE">Id do representante que recebeu o novo cliente. Seja ele por rodizo, ou recebimento direto.</param>
        /// <param name="CLI_ID">Id do cliente reencarteirado onde este histórico será anexado.</param>
        public void RegistraHistoricoNovoClienteImportacao(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? CLI_ID)
        {
            DateTime? dataDeHj = DateTime.Now;


            string descricao = @"Este cliente foi cadastrado através de uma importação.";

            string mensagemFinal = string.Format(descricao);

            //Registrando o histórico
            RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 12);
        }

        /// <summary>
        /// Método especializado para registrar o histórico para encarteiramento por importação.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="REP_ID_RECEBEU_CLIENTE">Id do representante que recebeu o novo cliente.</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistraHistoricoEncarteiramentoPorImportacao(string usuario, int? REP_ID_RECEBEU_CLIENTE, int? CLI_ID, int? RG_ID)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueRecebeuOCliente = _representanteService.FindById(REP_ID_RECEBEU_CLIENTE);
            var regiaoAdicionado = _regiaoSRV.FindById(RG_ID);


            // Lógica de composição da string de descrição do histórico
            string nomeDaRegiao =
                                (regiaoAdicionado != null && !string.IsNullOrWhiteSpace(regiaoAdicionado.RG_DESCRICAO))
                                    ? regiaoAdicionado.RG_DESCRICAO : "(Nome indisponível)";


            string nomeRepresentanteQueRecebeuOCliente =
                                (representanteQueRecebeuOCliente != null && !string.IsNullOrWhiteSpace(representanteQueRecebeuOCliente.REP_NOME))
                                    ? representanteQueRecebeuOCliente.REP_NOME : "(Nome indisponível)";


            string descricao = @"Este cliente foi adicionado a região '{0}' por meio de importação. O cliente foi adicionado a carteira do(a) representante '{1}'";

            string mensagemFinal = string.Format(descricao, nomeDaRegiao, nomeRepresentanteQueRecebeuOCliente);

            //Registrando o histórico
            RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_RECEBEU_CLIENTE, CLI_ID, 10);
        }

        /// <summary>
        /// Método especializado para registrar o histórico para de boleto automático enviado.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="codParcela"></param>
        /// <param name="CLI_ID"></param>
        public void RegistrarHistoricoEnvioAutomaricoBoleto(string usuario, string codParcela, int? CLI_ID, string email)
        {
            DateTime? dataDeHj = DateTime.Now;

            var _parcelaSRV = ServiceFactory.RetornarServico<ParcelasSRV>();

            var parcela = _parcelaSRV.FindById(codParcela);

            decimal? vlrParcela = 0.00m;
            DateTime? dataVencimento = null;

            if(parcela != null)
            {
                vlrParcela = (parcela.PAR_VLR_BOLETO != null) ? parcela.PAR_VLR_BOLETO : parcela.PAR_VLR_PARCELA;
                dataVencimento = parcela.PAR_DATA_VENCTO;
            }

            string descricao = @"Um boleto no valor de R$ {0:N} referente a parcela '{1}' com vencimento para {2:dd/MM/yyyy} foi enviado para o cliente de forma automática. '{3}'";

            string mensagemFinal = string.Format(descricao, vlrParcela, codParcela, dataVencimento, email);

            RegistrarHistorico(dataDeHj, mensagemFinal, usuario, null, CLI_ID, 27);
        }



    }
}

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
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("AGE_ID")]
    public class AgendamentoSRV : GenericService<AGENDAMENTO, AgendamentoDTO, int>
    {
        private AgendamentoDAO _dao;
        public CarteiramentoSRV _carteiramentoSRV { get; set; }
        public HistAtendSRV _histAtentimentoSRV { get; set; }
        public PrioridadeAtendimentoSRV _prioridadeAtendimento { get; set; }
        public RepresentanteSRV _representanteSRV { get; set; }

        public AgendamentoSRV()
        {
            this._dao = new AgendamentoDAO();
            this._carteiramentoSRV = new CarteiramentoSRV();
            this._histAtentimentoSRV = new HistAtendSRV();
            this._prioridadeAtendimento = new PrioridadeAtendimentoSRV();
            this._representanteSRV = new RepresentanteSRV();
            this.Dao = _dao;
        }

        public AgendamentoSRV(AgendamentoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public void SalvarAgendamento(AgendamentoDTO agenda, string usuario = null)
        {
            if (agenda != null)
            {
                using (var scope = new TransactionScope())
                {
                    var CLI_ID = agenda.CLI_ID;
                    var REP_ID = agenda.REP_ID;

                    var carteira = _carteiramentoSRV.ListarCarteiraDoRepreECliente((int)CLI_ID, (int)REP_ID);

                    if (carteira != null)
                    {
                        agenda.CAR_ID = carteira.CAR_ID;
                    }

                    _processaSalvamento(agenda);
                    _histAtentimentoSRV.SalvarHistoricoAtendimentoDoAgendamento(agenda, usuario);

                    if (REP_ID != null && CLI_ID != null)
                    {
                        _prioridadeAtendimento.ConfirmarAtendimentoDePrioridade((int)REP_ID, (int)CLI_ID);
                    }


                    scope.Complete();
                }

            }
            
        }


        private void _processaSalvamento(AgendamentoDTO dto)
        {
            if (dto != null && dto.REP_ID != null)
            {
                if (dto.AGE_ID != null)
                {
                    Merge(dto, "AGE_ID");
                }
                else
                {
                    var obj = Save(dto);
                    if (obj != null && obj.AGE_ID != null)
                    {
                        dto.AGE_ID = obj.AGE_ID;
                    }
                }
            }
        }        

        public void ConfirmarAgendamento(AgendamentoDTO agendamento, string USU_LOGIN = null)
        {
            using (var scope = new TransactionScope())
            {
                agendamento.AGE_DATA_CONFIRMACAO = DateTime.Now;
                SalvarComHistorico(agendamento, USU_LOGIN);

                int? REP_ID = agendamento.REP_ID;
                int? CLI_ID = agendamento.CLI_ID;

                if(REP_ID != null && CLI_ID != null)
                {
                    _prioridadeAtendimento.ConfirmarAtendimentoDePrioridade((int) REP_ID, (int) CLI_ID);
                }

                scope.Complete();
            }
        }

        public void Reagendar(AgendamentoDTO agendamento, string USU_LOGIN = null)
        {
            using (var scope = new TransactionScope())
            {
                var hoje = DateTime.Now;
                    agendamento.AGE_DATA_REAGENDAMENTO = hoje;

                    var historico = agendamento.HISTORICO_ATENDIMENTO.FirstOrDefault();
                    
                    if (historico != null)
                    {
                        string observacao = historico.HAT_DESCRICAO;
                        string observacaoNova = "O agendamento do cliente foi reagendado no dia {0}. Descrição: '{1}'";
                        string observacaoFinal = string.Format(observacaoNova, hoje, observacao);

                        historico.HAT_DESCRICAO = observacaoFinal;
                    }

                    SalvarComHistorico(agendamento, USU_LOGIN, 8);
                    scope.Complete();

            }
        }

        public void SalvarComHistorico(AgendamentoDTO agendamento, string USU_LOGIN = null, int ACA_ID = 7)
        {
            var agen = SaveOrUpdate(agendamento);
            _histAtentimentoSRV.SalvarHistoricoAtendimentoDoAgendamento(agendamento, USU_LOGIN, ACA_ID);
        }

        public AgendamentoDTO FindByIdFullLoaded(int AGE_ID)
        {
            var dto = FindById(AGE_ID);

            if (dto != null)
            {
                dto.HISTORICO_ATENDIMENTO = new HistAtendSRV().FindHistoricosByAgendamento(AGE_ID);
            }

            return dto;
        }
        
        public Pagina<AgendamentoDTO> ListarAgendamentosDoDia(DateTime? data, int? REP_ID, int? pagina = 1, int? registrosPorPagina = 7, int? RG_ID = null)
        {
            return _dao.ListarAgendamentosDoDia(data, REP_ID, pagina, registrosPorPagina, RG_ID);
        }

        public Pagina<AgendamentoDTO> ListarAgendamentosAtrasados(DateTime? data, int? REP_ID, int? pagina = 1, int? registrosPorPagina = 7, int? RG_ID = null)
        {
            return _dao.ListarAgendamentosAtrasados(data, REP_ID, pagina, registrosPorPagina, RG_ID);
        }

        public Pagina<AgendamentoDTO> ListarAgendamentosVindouros(DateTime? data, int? REP_ID, int? pagina = 1, int? registrosPorPagina = 7, int? RG_ID = null)
        {
            return _dao.ListarAgendamentosVindouros(data, REP_ID, pagina, registrosPorPagina, RG_ID);
        }

        public Pagina<AgendamentoDTO> ListarAgendamentosDoDiaGerente(DateTime? data, int? REP_GERENTE_ID, int? REP_ID, int? pagina = 1, int? registrosPorPagina = 7, int? RG_ID = null)
        {

            if (_representanteSRV.RepresentantesExistemNaMesmaRegiao(REP_GERENTE_ID, REP_ID))
            {
                return _dao.ListarAgendamentosDoDia(data, REP_ID, pagina, registrosPorPagina, RG_ID);
            }

            throw new AcessoADadosNaoPermitidoException("Você está tentado acessar dados de um representante que não pertence a sua região");

        }

        public Pagina<AgendamentoDTO> ListarAgendamentosAtrasadosGerente(DateTime? data, int? REP_GERENTE_ID, int? REP_ID, int? pagina = 1, int? registrosPorPagina = 7, int? RG_ID = null)
        {
            if (_representanteSRV.RepresentantesExistemNaMesmaRegiao(REP_GERENTE_ID, REP_ID))
            {
                return _dao.ListarAgendamentosAtrasados(data, REP_ID, pagina, registrosPorPagina, RG_ID);
            }

            throw new AcessoADadosNaoPermitidoException("Você está tentado acessar dados de um representante que não pertence a sua região");

        }

        public Pagina<AgendamentoDTO> ListarAgendamentosVindourosGerente(DateTime? data, int? REP_GERENTE_ID, int? REP_ID, int? pagina = 1, int? registrosPorPagina = 7, int? RG_ID = null)
        {
            if (_representanteSRV.RepresentantesExistemNaMesmaRegiao(REP_GERENTE_ID, REP_ID))
            {
                return _dao.ListarAgendamentosVindouros(data, REP_ID, pagina, registrosPorPagina, RG_ID);
            }

            throw new AcessoADadosNaoPermitidoException("Você está tentado acessar dados de um representante que não pertence a sua região");

        }

        public Pagina<AgendamentoDTO> Agendamentos(DateTime? dataInicial, DateTime? dataFinal, int? REP_ID, int? CLI_ID, int pagina = 1, int registrosPorPagina = 7, int? RG_ID = null)
        {
            return _dao.Agendamentos(dataInicial, dataFinal, REP_ID, CLI_ID, pagina, registrosPorPagina, RG_ID);
        }

        public IList<AgendamentoDTO> GetAgendamentosAteData(DateTime? data, int? REP_ID, int? RG_ID = null)
        {
            return _dao.GetAgendamentosAteData(data, REP_ID, RG_ID);
        }

        public void ConfirmarAgendamentosAteData(DateTime? data, int? REP_ID, int? RG_ID = null)
        {
            var agendamentos = GetAgendamentosAteData(data, REP_ID, RG_ID);

            foreach (var agendamento in agendamentos)
            {
                agendamento.AGE_DATA_CONFIRMACAO = DateTime.Now;
            }

            MergeAll(agendamentos);
        }
    }
}

using System;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.DAO.Reflection;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("AGC_ID")]
    public class AgendaCobrancaSRV : ServiceAdapter<AGENDA_COBRANCA, AgendaCobrancaDTO, int>
    {
        public AgendaCobrancaSRV _serviceAgenda { get; set; }
        private AgendaCobrancaDAO _dao = new AgendaCobrancaDAO();

        public AgendaCobrancaSRV()
        {
            SetDao(_dao);
        }
        public Pagina<AgendaCobrancaCustomDto> BuscarAgendamento(string assinatura
                                                               , string cliente
                                                               , string atendente
                                                               , string cnpj
                                                               , DateTime? dataini = null
                                                               , DateTime? datafim = null
                                                               , bool pendente = true
                                                               , int pagina = 1
                                                               , int registroPorPagina = 20)
        {
            return _dao.BuscarAgendamento(assinatura, cliente, atendente, cnpj, dataini, datafim, pendente, pagina, registroPorPagina);

        }
        public void GravarAgendamento(AgendaCobrancaDTO _agenda)
        {
            var _agendaAnt = this.FindById(_agenda.AGC_REAGENDAMENTO);

            if (_agendaAnt != null) {
                _agendaAnt.AGC_DATA_ATENDIMENTO = DateTime.Now;
                _agendaAnt.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
            }


            var txOpt = new TransactionOptions();
            txOpt.IsolationLevel = IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;

            using (TransactionScope scope = new TransactionScope())
            {
                var _histAtend = new HistoricoAtendimentoDTO();

                _histAtend.CLI_ID = _agenda.CLI_ID;
                _histAtend.HAT_DESCRICAO = _agenda.AGC_ASSUNTO;
                _histAtend.HAT_DATA_HIST = DateTime.Now;
                _histAtend.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                _histAtend.HAT_SOLICITANTE = SessionContext.autenticado.USU_LOGIN;
                _histAtend.UEN_ID = 3;
                _histAtend.ACA_ID = 26;
                _histAtend.TIP_ATEND_ID = _agenda.TIP_ATEND_ID;
                
                new HistAtendSRV().Save(_histAtend);

                if (_agendaAnt != null)
                    this.Merge(_agendaAnt);

                if (_agenda.AGC_GERAR_AGENDAMENTO == true)
                {
                    var _horaAgenda = _agenda.AGC_HORA_AGENDA.Split(':');
                    var _dataagenda = new DateTime(_agenda.AGC_DATA_AGENDA.Value.Year
                                                 , _agenda.AGC_DATA_AGENDA.Value.Month
                                                 , _agenda.AGC_DATA_AGENDA.Value.Day
                                                 , int.Parse(_horaAgenda[0])
                                                 , int.Parse(_horaAgenda[1]), 0);
                    _agenda.AGC_RESUMO = _agenda.AGC_ASSUNTO;
                    _agenda.AGC_DATA_AGENDA = _dataagenda;
                    _agenda.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    _agenda.AGC_DATA_REGISTRO = DateTime.Now;
                    _agenda.STATUS = false;

                    this.Save(_agenda);

                }

                scope.Complete();
            }

        }

        public void BaixaManualDeAgendamento(string _assinatura)
        {
            var txOpt = new TransactionOptions();
            txOpt.IsolationLevel = IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
            {
                var ag = _dao.BuscarAgendamentoPorParcela(_assinatura);

                if (ag != null)
                {
                    ag.STATUS = true;
                }

                SaveOrUpdate(ag);

                scope.Complete();
            }
        }

        public void ExecutarUpdateEmAgendamentos()
        {
            using (var context = new COADCORPEntities())
            {
                context.ExecutarProcedure("AtualizaStatusAgendaCobranca");

            }
        }
    }
}

using System;
using System.Linq;
using System.Linq.Dynamic;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Extensions;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.DAO
{
    public class AgendaCobrancaDAO : DAOAdapter<AGENDA_COBRANCA, AgendaCobrancaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AgendaCobrancaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public Pagina<AgendaCobrancaCustomDto> BuscarAgendamento(string assinatura
                                                               , string cliente = null
                                                               , string atendente = null
                                                               , string cnpj = null
                                                               , DateTime? dataini = null
                                                               , DateTime? datafim = null
                                                               , bool pendente = true
                                                               , int pagina = 1
                                                               , int registroPorPagina = 20)
        {
            DateTime? _dtfim = null;

            if (datafim != null)
            {
                _dtfim = new DateTime(datafim.Value.Year, datafim.Value.Month, datafim.Value.Day);
                _dtfim = _dtfim.Value.AddDays(1);
            }

            var query = (from g in db.AGENDA_COBRANCA
                         join a in db.ASSINATURA on g.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID 
                         where (g.AGC_DATA_AGENDA >= dataini && g.AGC_DATA_AGENDA < _dtfim)
                            && (g.STATUS == false)
                            //&& SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now) >= 7
                            //&& SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now) <= 90
                            && ((assinatura == null || assinatura == "") || (assinatura == g.ASN_NUM_ASSINATURA))
                            && ((cliente == null || cliente == "") || (cliente == l.CLI_NOME))
                            && ((cnpj == null || cnpj == "") || (cnpj == l.CLI_CPF_CNPJ))
                            && ((atendente == null || atendente == "") || (atendente == g.USU_LOGIN))
                            && ((pendente == false) || (pendente && g.AGC_DATA_ATENDIMENTO == null))
                         select new AgendaCobrancaCustomDto
                         {
                             ASN_NUM_ASSINATURA = g.ASN_NUM_ASSINATURA,
                             CLI_ID = l.CLI_ID,
                             CLI_NOME = l.CLI_NOME,
                             AGC_ID = g.AGC_ID,
                             AGC_DATA_ATENDIMENTO = g.AGC_DATA_ATENDIMENTO,
                             AGC_DATA_AGENDA = g.AGC_DATA_AGENDA,
                             AGC_HORA_AGENDA = g.AGC_HORA_AGENDA,
                             AGC_ASSUNTO = g.AGC_ASSUNTO,
                             AGC_REAGENDAMENTO = g.AGC_REAGENDAMENTO,
                             USU_LOGIN = g.USU_LOGIN
                         }).OrderByDescending(x => x.AGC_DATA_AGENDA);

            //var _lista = new List<ParcelasAtrasoCustomDTO>();


            return query.Paginar<AgendaCobrancaCustomDto>(pagina, registroPorPagina);

        }

        public AgendaCobrancaDTO BuscarAgendamentoPorParcela(string numeroAssinatura)
        {
            var query = (from age in db.AGENDA_COBRANCA
                         where age.ASN_NUM_ASSINATURA == numeroAssinatura && age.STATUS == false
                         orderby age.AGC_ID descending
                         select age);

            var listaAgendaDTO = ToDTO(query.FirstOrDefault());
            return listaAgendaDTO;
        }

    }
}

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.UTIL.Grafico;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public partial class BUSCAR_HSITORICO_ATEND_SAC_CABEC
    {
        public BUSCAR_HSITORICO_ATEND_SAC_CABEC()
        {
            this.HSITORICO_ATEND_Result = new HashSet<BUSCAR_HSITORICO_ATEND_SAC_Result>();
        }

        public string CLA_ATEND_DESCRICAO { get; set; }
        public string USU_LOGIN { get; set; }
        public virtual ICollection<BUSCAR_HSITORICO_ATEND_SAC_Result> HSITORICO_ATEND_Result { get; set; }
        
    }

    public class HistAtendDAO : DAOAdapter<HISTORICO_ATENDIMENTO, HistoricoAtendimentoDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public HistAtendDAO()
        {
           db = GetDb<COADCORPEntities>(false);
        }
        public IList<BUSCAR_HSITORICO_ATEND_SAC_CABEC> BuscarAtendimentoPorTipo(DateTime? _dtini = null, DateTime? _dtfim = null, int _uen_id = 0, string _usu_login = null)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            if (_dtini == null)
            {
                _dtinicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                _dtfinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, _dtini.Value.Day);
                _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, _dtfim.Value.Day);
            }
          
            _dtfinal = _dtfinal.AddDays(1);

            var query = db.BUSCAR_HSITORICO_ATEND_SAC(_dtinicial, _dtfinal, _usu_login);

            var _cabecHist = new List<BUSCAR_HSITORICO_ATEND_SAC_CABEC>();
            var _litem = new BUSCAR_HSITORICO_ATEND_SAC_CABEC();

            var _usuLogin = "";
            var _CLA_ATEND_DESCRICAO = "";


            foreach (var item in query)
            {
                if (_usuLogin != item.USU_LOGIN || _CLA_ATEND_DESCRICAO != item.CLA_ATEND_DESCRICAO)
                {
                    if (_usuLogin != "" && _CLA_ATEND_DESCRICAO != "")
                        _cabecHist.Add(_litem);

                    _litem = new BUSCAR_HSITORICO_ATEND_SAC_CABEC();
                    _litem.USU_LOGIN = item.USU_LOGIN;
                    _litem.CLA_ATEND_DESCRICAO = item.CLA_ATEND_DESCRICAO;
                    _usuLogin = item.USU_LOGIN;
                    _CLA_ATEND_DESCRICAO = item.CLA_ATEND_DESCRICAO;

                }

                _litem.HSITORICO_ATEND_Result.Add(item);

            }

            return _cabecHist;

        }


        public IQueryable<HISTORICO_ATENDIMENTO> TemplateHistoricoDoCliente(int CLI_ID, DateTime? dataInicial = null, DateTime? dataFinal = null, int? UEN_ID = 1)
        {
            var query = GetDbSet().Where(x =>
                (x.CLI_ID == CLI_ID || (from age in db.AGENDAMENTO where age.CLI_ID == CLI_ID select age.AGE_ID).Contains((int)x.AGE_ID)) &&
                x.UEN_ID == UEN_ID).Distinct();


            if (dataInicial != null)
            {
                DateTime dataIniComparacao = ((DateTime) dataInicial).Date;
                query = query.Where(x => EntityFunctions.TruncateTime(x.HAT_DATA_HIST) >= dataIniComparacao );
            }

            if (dataFinal != null)
            {
                DateTime dataFimComparacao = ((DateTime)dataFinal).Date;
                query = query.Where(x =>  EntityFunctions.TruncateTime(x.HAT_DATA_HIST) <= dataFimComparacao );
            }

            query = query.OrderByDescending(x => x.HAT_DATA_HIST);
            return query;

        }

        public IList<HistoricoAtendimentoDTO> BuscarPorAssinatura(string _assinatura = null, int pagina = 1, int registroPorPagina = 10)
        {
            IQueryable<HISTORICO_ATENDIMENTO> query =  db.HISTORICO_ATENDIMENTO.Where(x => x.ASN_NUM_ASSINATURA  == _assinatura).OrderByDescending(x => x.HAT_DATA_HIST);
            return ToDTO(query);
        }
        public IList<HistoricoAtendimentoDTO> BuscarPorCliente(int _cli_id)
        {
            IQueryable<HISTORICO_ATENDIMENTO> query = db.HISTORICO_ATENDIMENTO.Where(x => x.CLI_ID == _cli_id).OrderByDescending(x => x.HAT_DATA_HIST);
            return ToDTO(query);
        }
        public Pagina<HistoricoAtendimentoDTO> BuscarPorCliente(int _cli_id, int pagina = 1, int registroPorPagina = 10)
        {
            IQueryable<HISTORICO_ATENDIMENTO> query = db.HISTORICO_ATENDIMENTO.Where(x => x.CLI_ID == _cli_id).OrderByDescending(x => x.HAT_DATA_HIST);

            return ToDTOPage(query, pagina, registroPorPagina);
        }

        public IList<HistoricoAtendimentoDTO> BuscarEtiquetas(DateTime _dtini, DateTime _dtfim)
        {
 
            _dtfim = _dtfim.AddDays(1);

            var query = (from h in db.HISTORICO_ATENDIMENTO
                         where (h.HAT_DATA_HIST >= _dtini && h.HAT_DATA_HIST < _dtfim) &&
                               (h.HAT_IMP_ETIQUETA == true) &&
                               (h.HAT_DATA_RESOLUCAO == null)
                         orderby h.HAT_DATA_HIST descending
                         select h).ToList();
            
            return ToDTO(query);
        }

        public IList<HistoricoAtendimentoDTO> BuscarEtiquetas()
        {
       
            var query = (from h in db.HISTORICO_ATENDIMENTO
                         where (h.HAT_DATA_RESOLUCAO ==  null) &&
                               (h.HAT_IMP_ETIQUETA == true)
                         orderby h.HAT_DATA_HIST descending
                         select h).ToList();

            return ToDTO(query);
        }


        public IList<HistoricoAtendimentoDTO> BuscarPorPeriodo(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, bool _etiqueta = false)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            if (_dtini == null)
            {
                _dtinicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                _dtfinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                _dtinicial = (DateTime)_dtini;
                _dtfinal = (DateTime)_dtfim;
            }

            _dtfinal = _dtfinal.AddDays(1);

            var query = (from h in db.HISTORICO_ATENDIMENTO
                         where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                               (h.HAT_DATA_HIST >= _dtinicial && h.HAT_DATA_HIST <= _dtfinal) &&
                               (_etiqueta == false || (_etiqueta != false && h.HAT_IMP_ETIQUETA == true)) &&
                               (h.ASN_NUM_ASSINATURA != null)
                         orderby h.HAT_DATA_HIST descending
                         select h).ToList();


            return ToDTO(query);
        }
        public Pagina<HistoricoAtendimentoDTO> BuscarPorPeriodo(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, bool _etiqueta = false, int pagina = 1, int registroPorPagina = 7)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            if (_dtini == null)
            {
                _dtinicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                _dtfinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else
            {
                _dtinicial = (DateTime)_dtini;
                _dtfinal = (DateTime)_dtfim;
            }

            _dtfinal.AddDays(1);

            var query = (from h in db.HISTORICO_ATENDIMENTO
                         where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                               (h.HAT_DATA_HIST >= _dtinicial && h.HAT_DATA_HIST <= _dtfinal) &&
                               (_etiqueta == false || (_etiqueta != false && h.HAT_IMP_ETIQUETA == true)) &&
                               (h.ASN_NUM_ASSINATURA != null)
                         orderby h.HAT_DATA_HIST descending
                         select h).ToList();
            

            return ToDTOPage(query,pagina,registroPorPagina);
        }

        public Pagina<HistoricoAtendimentoDTO> FindHistoricoByCliId(int CLI_ID, DateTime? dataInicial = null, DateTime? dataFinal = null, int pagina = 1, int registroPorPagina = 10, int? UEN_ID = 1)
        {
            var query = TemplateHistoricoDoCliente(CLI_ID, dataInicial, dataFinal, UEN_ID);
           
            return ToDTOPage(query, pagina, registroPorPagina );
        }

        public IList<HistoricoAtendimentoDTO> FindHistoricoByCliIdSemPaginacao(int CLI_ID, DateTime? dataInicial = null, DateTime? dataFinal = null, int? UEN_ID = 1)
        {
            var query = TemplateHistoricoDoCliente(CLI_ID, dataInicial, dataFinal, UEN_ID);
            return ToDTO(query);
        }

        public IList<HistoricoAtendimentoDTO> FindHistoricosByAgendamento(int AGE_ID)
        {
            var query = GetDbSet().Where(x => x.AGENDAMENTO.AGE_ID == AGE_ID);

            return ToDTO(query);
        }
        
        /// <summary>
        /// Verifica no banco de dados se existe alguma ocorrência de histórico (por agendamento, ou pela clientes)
        /// para o cliente de Id passado.
        /// </summary>
        /// <param name="CLI_ID">Id do cliente testado</param>
        /// <returns></returns>
        public bool VerificarClientePossuiHistorico(int? CLI_ID, int? UEN_ID = 1)
        {
            var query = TemplateHistoricoDoCliente((int) CLI_ID, UEN_ID: UEN_ID);

            int count = query.Count();
            return (count > 0);
        }

        public IQueryable<JsonGrafico> ListarAtendimentosRealizadosNoMes(int? ano = null, int? mes = null, int? UEN_ID = 1)
        {
            if(ano == null || mes == null)
            {
                var date = DateTime.Now;
                
                if(ano == null)
                {
                    ano = date.Year;
                }

                if(mes == null)
                {
                    mes = date.Month;
                }

            }

            var query = (from hist in db.HISTORICO_ATENDIMENTO
                         where hist.UEN_ID == 1 &&
                             hist.ACA_ID == 7 &&
                             hist.HAT_DATA_HIST.Year == ano &&
                             hist.HAT_DATA_HIST.Month == mes
                         group hist by hist.REPRESENTANTE.REP_NOME
                        into grupo
                        select new JsonGrafico() { 
                        label = grupo.Key,
                        intData = grupo.Count()
                        });

            return query;
        }

        public IQueryable<JsonGrafico> ListarAtendimentosRealizadosNoMesPorRegiao(int? ano = null, int? mes = null, int? UEN_ID = 1)
        {
            if (ano == null || mes == null)
            {
                var date = DateTime.Now;

                if (ano == null)
                {
                    ano = date.Year;
                }

                if (mes == null)
                {
                    mes = date.Month;
                }

            }

            var query = (from hist in db.HISTORICO_ATENDIMENTO
                         where hist.UEN_ID == 1 &&
                             hist.ACA_ID == 7 &&
                             hist.HAT_DATA_HIST.Year == ano &&
                             hist.HAT_DATA_HIST.Month == mes
                         group hist by hist.REPRESENTANTE.REGIAO.RG_DESCRICAO
                             into grupo
                             select new JsonGrafico()
                             {
                                 label = grupo.Key,
                                 intData = grupo.Count()
                             });

            return query;
        }

        public IList<RelatorioAtendimentosXVendasEfetuadasDTO> ListarRelatorioAtendimentoXVendasPorRegiao(int? ano = null, int? mes = null, int? UEN_ID = 1)
        {
            var templateHistoricos = (from hs in db.HISTORICO_ATENDIMENTO
                                      where hs.UEN_ID == UEN_ID &&
                                      hs.REPRESENTANTE.UEN_ID == UEN_ID &&
                                      hs.HAT_DATA_HIST.Year == ano &&
                                      hs.HAT_DATA_HIST.Month == mes
                                      select hs);

            var query = (from rg in db.REGIAO
                                     orderby rg.RG_DESCRICAO
                                     select new RelatorioAtendimentosXVendasEfetuadasDTO()
                                     {
                                         RG_DESCRICAO = rg.RG_DESCRICAO,
                                         QTD_ATENDIMENTOS = 
                                                (from hs in templateHistoricos 
                                                  where hs.REPRESENTANTE.REGIAO.RG_ID == rg.RG_ID &&
                                                  hs.ACA_ID == 7 select hs)
                                                  .Count(),

                                         QTD_VENDAS_REALIZADAS = 
                                            (from hs in templateHistoricos
                                             where hs.REPRESENTANTE.REGIAO.RG_ID == rg.RG_ID &&
                                                      hs.ACA_ID == 13
                                                      select hs)
                                                      .Count() 
                                     });


            return query.ToList();
        }
        
    }
}

using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.UTIL.Grafico;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class QtdeConsumoDTO
    {
        public string periodo { get; set; }
        public string uraid { get; set; }
        public string assinatura { get; set; }
        public string nome { get; set; }
        public int contratadas { get; set; }
        public int qtde { get; set; }
        public decimal ano { get; set; }
        public decimal mes { get; set; }
     
    }

    public class HistAtendUraDAO : DAOAdapter<HIST_ATEND_URA, HistAtendUraDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public HistAtendUraDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public IList<HistAtendUraDTO> BuscarPorAssinatura(string asn_id)
        {
            var query = db.HIST_ATEND_URA.Where(x => x.ASN_NUM_ASSINATURA == asn_id).AsQueryable();

            return ToDTO(query);
        }
        public IList<HistAtendUraDTO> BuscarPorPeriodo(string _asn_id, string _ura_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
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

            var query = (from h in db.HIST_ATEND_URA
                         where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                               (_ura_id == null || (_ura_id != null && _ura_id == h.URA_ID)) &&
                               (h.HAU_DATA_CADASTRO >= _dtinicial && h.HAU_DATA_CADASTRO <= _dtfinal) &&
                               (h.HAU_ATENDIDO == "S")
                         orderby h.HAU_DATA_CADASTRO descending
                         select h).ToList();


            return ToDTO(query);
        }
        public IList<HistAtendUraDTO> BuscarPorPeriodo(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
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

            var query = (from h in db.HIST_ATEND_URA
                         where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                               (h.HAU_DATA_CADASTRO >= _dtinicial && h.HAU_DATA_CADASTRO <= _dtfinal) &&
                               (h.HAU_ATENDIDO == "S")
                         orderby h.HAU_DATA_CADASTRO descending
                         select h).ToList();


            return ToDTO(query);
        }
        public IList<QtdeConsumoDTO> BuscarQtdePorAssinatura(string _asn_id, string _contrato)
        {
   
            var query = (from h in db.HIST_ATEND_URA
                         join c in db.CONTRATOS on h.ASSINATURA.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                         where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                               (
                                 ((c.CTR_DATA_CANC == null && c.CTR_PRORROGADO == 1) && (h.HAU_DATA_CADASTRO >= c.CTR_DATA_INI_VIGENCIA && h.HAU_DATA_CADASTRO <= DateTime.Now)) ||
                                 ((c.CTR_DATA_CANC == null) && (h.HAU_DATA_CADASTRO >= c.CTR_DATA_INI_VIGENCIA && h.HAU_DATA_CADASTRO <= c.CTR_DATA_FIM_VIGENCIA)) ||
                                 ((c.CTR_DATA_CANC != null) && (h.HAU_DATA_CADASTRO >= c.CTR_DATA_INI_VIGENCIA && h.HAU_DATA_CADASTRO <= c.CTR_DATA_CANC))
                                ) &&
                               (h.HAU_ATENDIDO == "S") && (c.CTR_NUM_CONTRATO == _contrato)
                         group h by new { h.URA_ID, h.HAU_DATA_CADASTRO.Year, h.HAU_DATA_CADASTRO.Month, h.ASN_NUM_ASSINATURA, h.ASSINATURA.CLIENTES.CLI_NOME, h.ASSINATURA.ASN_QTDE_CONS_CONTRATO  } into f
                         orderby f.Key.URA_ID, f.Key.ASN_NUM_ASSINATURA, f.Key.CLI_NOME
                         select new QtdeConsumoDTO
                         {
                             periodo = SqlFunctions.StringConvert((decimal)f.Key.Month) + "/" + SqlFunctions.StringConvert((decimal)f.Key.Year),
                             uraid = f.Key.URA_ID,
                             assinatura = f.Key.ASN_NUM_ASSINATURA,
                             nome = f.Key.CLI_NOME,
                             contratadas = f.Key.ASN_QTDE_CONS_CONTRATO,
                             qtde = f.Count(),
                             ano = (decimal)f.Key.Year,
                             mes = (decimal)f.Key.Month

                         }).AsQueryable();


            return query.ToList();
        }
        public IList<QtdeConsumoDTO> BuscarQtdePorAssinatura(string _asn_id, Nullable<DateTime> _dtini=null,  Nullable<DateTime> _dtfim = null)
        {            
            DateTime _dtinicial; 
            DateTime _dtfinal; 

            if (_dtini == null)
            {
                _dtinicial = new DateTime( DateTime.Now.Year, DateTime.Now.Month, 1);
                _dtfinal = new DateTime( DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day);
            }
            else
            {   
                _dtinicial = (DateTime)_dtini;
                _dtfinal = (DateTime)_dtfim;
            }

            string _periodo = _dtinicial.Day.ToString() + "/" + _dtinicial.Month.ToString() + "/" + _dtinicial.Year.ToString() + " a " + _dtfinal.Day.ToString() + "/" + _dtfinal.Month.ToString() + "/" + _dtfinal.Year.ToString();

            _dtfinal = _dtfinal.AddDays(1);

            var query = (from h in db.HIST_ATEND_URA
                        where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                              (h.HAU_DATA_CADASTRO >= _dtinicial && h.HAU_DATA_CADASTRO <= _dtfinal) &&
                              (h.HAU_ATENDIDO == "S")
                        group h by new { h.URA_ID, h.ASN_NUM_ASSINATURA, h.ASSINATURA.CLIENTES.CLI_NOME, h.ASSINATURA.ASN_QTDE_CONS_CONTRATO } into f
                         orderby f.Key.URA_ID, f.Key.ASN_NUM_ASSINATURA, f.Key.CLI_NOME
                         select new QtdeConsumoDTO
                            {
                                periodo = _periodo,
                                uraid = f.Key.URA_ID,
                                assinatura = f.Key.ASN_NUM_ASSINATURA,
                                nome = f.Key.CLI_NOME,
                                contratadas = f.Key.ASN_QTDE_CONS_CONTRATO,
                                qtde = f.Count()
                            }).AsQueryable();


            return query.ToList();
        }
        public IList<QtdeConsumoDTO> BuscarTotalPorAssinatura(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
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

            string _periodo = _dtinicial.Day.ToString() + "/" + _dtinicial.Month.ToString() + "/" + _dtinicial.Year.ToString() + " a " + _dtfinal.Day.ToString() + "/" + _dtfinal.Month.ToString() + "/" + _dtfinal.Year.ToString();

            _dtfinal = _dtfinal.AddDays(1);

            var query = (from h in db.HIST_ATEND_URA
                         where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                               (h.HAU_DATA_CADASTRO >= _dtinicial && h.HAU_DATA_CADASTRO <= _dtfinal) &&
                               (h.HAU_ATENDIDO == "S")
                         group h by new {h.ASN_NUM_ASSINATURA, h.ASSINATURA.CLIENTES.CLI_NOME, h.ASSINATURA.ASN_QTDE_CONS_CONTRATO } into f
                         orderby  f.Key.ASN_NUM_ASSINATURA, f.Key.CLI_NOME
                         select new QtdeConsumoDTO
                         {
                             periodo = _periodo,
                             assinatura = f.Key.ASN_NUM_ASSINATURA,
                             nome = f.Key.CLI_NOME,
                             contratadas = f.Key.ASN_QTDE_CONS_CONTRATO,
                             qtde = f.Count()
                         }).AsQueryable();


            return query.ToList();
        }
        public IList<JsonGrafico> BuscarTotalPorRamal(string _ura_id,  Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            int lastday = DateTime.DaysInMonth(_dtfim.Value.Year, _dtfim.Value.Month);

            _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, 1);
            _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, lastday);

            _dtfinal = _dtfinal.AddDays(1);

            var query = (from h in db.HIST_ATEND_URA
                         where (h.HAU_DATA_CADASTRO >= _dtinicial && h.HAU_DATA_CADASTRO <= _dtfinal) &&
                               (h.HAU_ATENDIDO == "S") && ( h.URA_ID ==_ura_id)
                         group h by new {h.HAU_RAMAL } into f
                         orderby f.Key.HAU_RAMAL
                         select new JsonGrafico
                         {
                             label = SqlFunctions.StringConvert((double)f.Key.HAU_RAMAL),
                             data = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }
        public IList<JsonGrafico> BuscarTotalPorUF(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;
            
            int lastday = DateTime.DaysInMonth(_dtfim.Value.Year, _dtfim.Value.Month);

            _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, 1);
            _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, lastday);

            _dtfinal = _dtfinal.AddDays(1);

            var query = (from h in db.HIST_ATEND_URA
                         where (h.HAU_DATA_CADASTRO >= _dtinicial && h.HAU_DATA_CADASTRO <= _dtfinal) &&
                               (h.HAU_ATENDIDO == "S")
                         group h by new { h.URA_ID } into f
                         orderby f.Key.URA_ID
                         select new JsonGrafico
                         {
                             label = f.Key.URA_ID,
                             data = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }
        public IList<JsonGrafico> BuscarTotalPorProduto(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            int lastday = DateTime.DaysInMonth(_dtfim.Value.Year, _dtfim.Value.Month);

            _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, 1);
            _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, lastday);

            _dtfinal = _dtfinal.AddDays(1);
            
            var query = (from h in db.HIST_ATEND_URA
                         join a in db.ASSINATURA  on h.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                         where (h.HAU_DATA_CADASTRO >= _dtinicial && h.HAU_DATA_CADASTRO <= _dtfinal) &&
                               (h.HAU_ATENDIDO == "S")
                         group h by new { a.PRO_ID, p.PRO_SIGLA } into f
                         orderby f.Count(), f.Key.PRO_SIGLA ascending 
                         select new JsonGrafico
                         {
                             label = SqlFunctions.StringConvert((double)f.Key.PRO_ID) + " - " + f.Key.PRO_SIGLA,
                             data = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }
    
    }
}

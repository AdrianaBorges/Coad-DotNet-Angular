using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class HistAtendEmailDAO : DAOAdapter<HIST_ATEND_EMAIL, HistAtendEmailDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public HistAtendEmailDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public IList<HistAtendEmailDTO> BuscarPorAssinatura(string asn_id)
        {
            var query = db.HIST_ATEND_EMAIL.Where(x => x.ASN_NUM_ASSINATURA == asn_id).AsQueryable();

            return ToDTO(query);
        }
        public IList<HistAtendEmailDTO> BuscarPorPeriodo(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
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

            var query = (from h in db.HIST_ATEND_EMAIL
                         where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                               (h.HAE_DTCADASTRO >= _dtinicial && h.HAE_DTCADASTRO <= _dtfinal)
                         orderby h.HAE_DTCADASTRO descending
                         select h).ToList();


            return ToDTO(query);
        }
        public IList<QtdeConsumoDTO> BuscarQtdePorAssinatura(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
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

            var query = (from h in db.HIST_ATEND_EMAIL
                         where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                               (h.HAE_DTCADASTRO >= _dtinicial && h.HAE_DTCADASTRO <= _dtfinal)
                         group h by new { h.ASN_NUM_ASSINATURA, h.ASSINATURA.CLIENTES.CLI_NOME  } into f
                         orderby f.Key.ASN_NUM_ASSINATURA, f.Key.CLI_NOME
                         select new QtdeConsumoDTO
                         {
                             periodo = _periodo,
                             uraid = "CONSULTORIA",
                             assinatura = f.Key.ASN_NUM_ASSINATURA,
                             nome = f.Key.CLI_NOME,
                             contratadas = 0,
                             qtde = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }
        public IList<QtdeConsumoDTO> BuscarQtdePorAssinatura(string _asn_id, string _contrato)
        {

            var query = (from h in db.HIST_ATEND_EMAIL
                         join c in db.CONTRATOS on h.ASSINATURA.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA  
                         where (_asn_id == null || (_asn_id != null && _asn_id == h.ASN_NUM_ASSINATURA)) &&
                               (c.CTR_NUM_CONTRATO == _contrato) &&      
                               (
                                 ((c.CTR_DATA_CANC == null && c.CTR_PRORROGADO == 1) && (h.HAE_DTCADASTRO >= c.CTR_DATA_INI_VIGENCIA && h.HAE_DTCADASTRO <= DateTime.Now)) ||
                                 ((c.CTR_DATA_CANC == null) && (h.HAE_DTCADASTRO >= c.CTR_DATA_INI_VIGENCIA && h.HAE_DTCADASTRO <= c.CTR_DATA_FIM_VIGENCIA)) ||
                                 ((c.CTR_DATA_CANC != null) && (h.HAE_DTCADASTRO >= c.CTR_DATA_INI_VIGENCIA && h.HAE_DTCADASTRO <= c.CTR_DATA_CANC)) 
                                )
                         group h by new { h.HAE_DTCADASTRO.Year, h.HAE_DTCADASTRO.Month, h.ASN_NUM_ASSINATURA, h.ASSINATURA.CLIENTES.CLI_NOME } into f
                         orderby f.Key.ASN_NUM_ASSINATURA, f.Key.CLI_NOME
                         select new QtdeConsumoDTO
                         {
                             periodo = SqlFunctions.StringConvert((decimal)f.Key.Month) + "/" + SqlFunctions.StringConvert((decimal)f.Key.Year),
                             uraid = "CONSULTORIA",
                             assinatura = f.Key.ASN_NUM_ASSINATURA,
                             nome = f.Key.CLI_NOME,
                             contratadas = 0,
                             qtde = f.Count(),
                             ano = (decimal)f.Key.Year,
                             mes = (decimal)f.Key.Month

                         }).AsQueryable();


            return query.ToList();
        }
     

    }
}

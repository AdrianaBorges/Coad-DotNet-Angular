using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.UTIL.Grafico;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{

    public class LogSimuladorDAO : AbstractGenericDao<LOG_SIMULADOR, LogSimuladorDTO, int>
    {
        public COADCORPEntities dbcorp { get; set; }

                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 

        public LogSimuladorDAO()
            : base()
        {
            SetProfileName("GED");

            dbcorp = new COADCORPEntities();
            db = GetDb<COADGEDEntities>(false);
        }
        public IList<JsonGrafico> BuscarTotalPorHora(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _tipoacesso = null, string _tdc_id = null)
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

            string _periodo = _dtinicial.Day.ToString() + "/" + _dtinicial.Month.ToString() + "/" + _dtinicial.Year.ToString() + " a " + _dtfinal.Day.ToString() + "/" + _dtfinal.Month.ToString() + "/" + _dtfinal.Year.ToString();

            _dtfinal = _dtfinal.AddDays(1);

            var query = (from l in db.LOG_SIMULADOR
                         where (l.LSI_DATA_ACESSO >= _dtinicial && l.LSI_DATA_ACESSO <= _dtfinal) &&
                               (l.LSI_TIPO_ACESSO == _tipoacesso) &&
                               (l.ASN_NUM_ASSINATURA != "99A00001") &&
                               (l.TDC_ID == _tdc_id)
                         group l by new { l.LSI_DATA_ACESSO.Hour } into f
                         orderby f.Key.Hour
                         select new JsonGrafico
                         {
                             label = SqlFunctions.StringConvert((double)f.Key.Hour),
                             data = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }
        public IList<JsonGrafico> BuscarTotalPorDia(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _tipoacesso = null, string _tdc_id = null)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            int lastday = DateTime.DaysInMonth(_dtfim.Value.Year, _dtfim.Value.Month);

           _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, 1);
           _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, lastday);

            var query = (from l in db.LOG_SIMULADOR
                         where (l.LSI_DATA_ACESSO >= _dtinicial && l.LSI_DATA_ACESSO <= _dtfinal) &&
                               (l.LSI_TIPO_ACESSO == _tipoacesso) &&
                               (l.ASN_NUM_ASSINATURA != "99A00001") &&
                               (l.TDC_ID == _tdc_id)
                         group l by new { l.LSI_DATA_ACESSO.Month, l.LSI_DATA_ACESSO.Day } into f
                         orderby f.Key.Month, f.Key.Day
                         select new JsonGrafico
                         {
                             label = SqlFunctions.StringConvert((double)f.Key.Day) + "/" + SqlFunctions.StringConvert((double)f.Key.Month),
                             data = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }
        public IList<JsonGrafico> BuscarTotalPorUFCalc(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _tipoacesso = null, string _tdc_id = null)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            _dtinicial = new DateTime(DateTime.Now.Year,1, 1);
            _dtfinal = new DateTime(DateTime.Now.Year, 12, 31);
   
            _dtfinal = _dtfinal.AddDays(1);

            var query01 = (from l in db.LOG_SIMULADOR
                                         where (l.LSI_DATA_ACESSO >= _dtinicial && l.LSI_DATA_ACESSO <= _dtfinal) &&
                                               (l.LSI_TIPO_ACESSO == _tipoacesso) &&
                                               (l.ASN_NUM_ASSINATURA != "99A00001") &&
                                               (l.TDC_ID == _tdc_id)
                                         orderby l.ASN_NUM_ASSINATURA
                                         select l).ToList();

            var assinaturas = query01.Select(x => x.ASN_NUM_ASSINATURA).Distinct();

            var query02 = (from e in dbcorp.CLIENTES_ENDERECO
                           join a in dbcorp.ASSINATURA on e.CLI_ID equals a.CLI_ID
                           where (e.END_TIPO == 1) && (assinaturas.Contains(a.ASN_NUM_ASSINATURA))
                           select new {a, e});

            query02 = query02.ToList().AsQueryable(); 

             var resultado =    (from l in query01
                                 join q in query02 on l.ASN_NUM_ASSINATURA equals q.a.ASN_NUM_ASSINATURA
                                 //join a in dbcorp.ASSINATURA on l.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                                 //join e in dbcorp.CLIENTES_ENDERECO on a.CLI_ID equals e.CLI_ID
                                group q.e by new { q.e.END_UF } into f
                                 orderby f.Count(), f.Key.END_UF descending
                                 select new JsonGrafico
                                 {
                                     label = f.Key.END_UF,
                                     data = f.Count()

                                 }).AsQueryable();


             return resultado.ToList();
        }

        public IList<JsonGrafico> BuscarTabelasPorPeriodo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, int _grupo = 0)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            int lastday = DateTime.DaysInMonth(_dtfim.Value.Year, _dtfim.Value.Month);

            _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, 1);
            _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, lastday);

            var query = (from l in db.LOG_SIMULADOR
                         join t in db.TAB_DINAMICA_CONFIG on l.TDC_ID equals t.TDC_ID
                         where (l.LSI_DATA_ACESSO >= _dtinicial && l.LSI_DATA_ACESSO <= _dtfinal) &&
                               (l.LSI_TIPO_ACESSO == "L") &&
                               (t.TDC_TIPO == 1) &&
                               (t.TGR_ID == _grupo) && 
                               (l.ASN_NUM_ASSINATURA != "99A00001") 
                         group l by new {t.TDC_NOME_TABELA, l.TDC_ID} into f
                         orderby f.Count(), f.Key.TDC_NOME_TABELA ascending 
                         select new JsonGrafico
                         {
                             label = f.Key.TDC_NOME_TABELA,
                             data = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }
        public IList<JsonGrafico> BuscarTabelasPorGrupo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            int lastday = DateTime.DaysInMonth(_dtfim.Value.Year, _dtfim.Value.Month);

            _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, 1);
            _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, lastday);

            var query = (from l in db.LOG_SIMULADOR
                         join t in db.TAB_DINAMICA_CONFIG on l.TDC_ID equals t.TDC_ID
                         where (l.LSI_DATA_ACESSO >= _dtinicial && l.LSI_DATA_ACESSO <= _dtfinal) &&
                               (l.LSI_TIPO_ACESSO == "L") &&
                               (t.TDC_TIPO == 1) &&
                               (l.ASN_NUM_ASSINATURA != "99A00001")
                         group l by new { t.TGR_ID, t.TAB_DINAMICA_GRUPO.TGR_DESCRICAO } into f
                         orderby f.Count(), f.Key.TGR_DESCRICAO ascending
                         select new JsonGrafico
                         {
                             label = f.Key.TGR_DESCRICAO,
                             data = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }
        public IList<JsonGrafico> BuscarTabelasPorGrupo(int _ano)
        {
            var query = (from l in db.LOG_SIMULADOR
                         join t in db.TAB_DINAMICA_CONFIG on l.TDC_ID equals t.TDC_ID
                         where (l.LSI_DATA_ACESSO.Year == _ano) &&
                               (l.LSI_TIPO_ACESSO == "L") &&
                               (t.TDC_TIPO == 1) &&
                               (l.ASN_NUM_ASSINATURA != "99A00001")
                         group l by new { t.TGR_ID, t.TAB_DINAMICA_GRUPO.TGR_DESCRICAO } into f
                         orderby f.Count(), f.Key.TGR_DESCRICAO ascending
                         select new JsonGrafico
                         {
                             label = f.Key.TGR_DESCRICAO,
                             data = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }

        public IList<TabelasGrupoAcesso> BuscarTabelasMaisAcessadas(int _qteregistros, int _tdc_tipo, int? _tgr_tipo)
        {
            var query = (from l in db.LOG_SIMULADOR
                         join t in db.TAB_DINAMICA_CONFIG on l.TDC_ID equals t.TDC_ID
                         where t.TDC_TIPO == _tdc_tipo 
                            && t.TDC_DATA_PUBLICACAO != null
                            && (_tgr_tipo == null || t.TAB_DINAMICA_GRUPO.TGR_TIPO == _tgr_tipo)
                         group l by new { t.TDC_ID, t.TDC_NOME_TABELA} into f
                         orderby f.Count() descending
                         select new TabelasGrupoAcesso
                         {
                             grupo = f.Key.TDC_ID,
                             nome = f.Key.TDC_NOME_TABELA,
                             dados = f.Count()

                         });

            if (_qteregistros > 0)
                query = query.Take(_qteregistros);

            return query.ToList();
        }

        public IList<TabelasGrupoAcesso> BuscarTabelasAcessadas(int _qteregistros, int _tdc_tipo, int? _tgr_tipo)
        {
            var query = (from l in db.LOG_SIMULADOR
                         join t in db.TAB_DINAMICA_CONFIG on l.TDC_ID equals t.TDC_ID
                         where t.TDC_TIPO == _tdc_tipo 
                            && t.TDC_DATA_PUBLICACAO != null
                            && (_tgr_tipo == null || t.TAB_DINAMICA_GRUPO.TGR_TIPO == _tgr_tipo)
                         group l by new { t.TDC_ID, t.TDC_NOME_TABELA } into f
                         orderby f.Max(x => x.LSI_DATA_ACESSO) descending
                         select new TabelasGrupoAcesso
                         {
                             grupo = f.Key.TDC_ID,
                             nome = f.Key.TDC_NOME_TABELA,
                             dados = 1

                         });

            if (_qteregistros > 0)
                query = query.Take(_qteregistros);

            return query.ToList();
        }
        
        public IList<TabelasGrupoAcesso> BuscarTabelasPorGrupo(int _mes, int _ano, int _grupo = 0, string _tdc_id = null, int _qteregistros = 0)
        {
            var query = (from l in db.LOG_SIMULADOR
                         join t in db.TAB_DINAMICA_CONFIG on l.TDC_ID equals t.TDC_ID
                         join g in db.TAB_DINAMICA_GRUPO on t.TGR_ID equals g.TGR_ID
                         where (l.LSI_DATA_ACESSO.Month == _mes) &&
                               (l.LSI_DATA_ACESSO.Year == _ano) &&
                               (l.LSI_TIPO_ACESSO == "L") &&
                               (l.ASN_NUM_ASSINATURA != "99A00001") &&
                               ((_grupo == 0) || (_grupo != 0 && _grupo == (int)t.TGR_ID)) &&
                               ((_tdc_id == null) || (_tdc_id != null && _tdc_id == t.TDC_ID))
                         group l by new { g.TGR_DESCRICAO, l.LSI_DATA_ACESSO.Year, l.LSI_DATA_ACESSO.Month, t.TDC_ID, t.TDC_NOME_TABELA } into f
                         orderby f.Key.TGR_DESCRICAO, f.Key.Year, f.Key.Month, f.Count() descending
                         select new TabelasGrupoAcesso
                         {
                             grupo = f.Key.TGR_DESCRICAO,
                             nome = f.Key.TDC_NOME_TABELA,
                             ano = f.Key.Year,
                             mes = f.Key.Month,
                             dados = f.Count()

                         });

            if (_qteregistros > 0)
                query = query.Take(_qteregistros);

            return query.ToList();
        }
        public IList<TabelasGrupoAcesso> BuscarTabelasPorGrupo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, int _grupo = 0, string _tdc_id = null, int _qteregistros = 0)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            int lastday = DateTime.DaysInMonth(_dtfim.Value.Year, _dtfim.Value.Month);

            _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, 1);
            _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, lastday);

            var query = (from l in db.LOG_SIMULADOR
                         join t in db.TAB_DINAMICA_CONFIG on l.TDC_ID equals t.TDC_ID
                         join g in db.TAB_DINAMICA_GRUPO on t.TGR_ID equals g.TGR_ID
                         where (l.LSI_DATA_ACESSO >= _dtinicial && l.LSI_DATA_ACESSO <= _dtfinal) &&
                               (l.LSI_TIPO_ACESSO == "L") &&
                               (t.TDC_TIPO == 1) &&
                               (l.ASN_NUM_ASSINATURA != "99A00001") &&
                               ((_grupo == 0) || (_grupo != 0 && _grupo == (int)t.TGR_ID)) &&
                               ((_tdc_id == null) || (_tdc_id != null && _tdc_id == t.TDC_ID))
                         group l by new { g.TGR_DESCRICAO, l.LSI_DATA_ACESSO.Year, l.LSI_DATA_ACESSO.Month, t.TDC_ID, t.TDC_NOME_TABELA } into f
                         orderby f.Key.TGR_DESCRICAO, f.Key.Year, f.Key.Month, f.Count() descending
                         select new TabelasGrupoAcesso
                          {
                              grupo = f.Key.TGR_DESCRICAO,
                              nome = f.Key.TDC_NOME_TABELA,
                              ano = f.Key.Year,
                              mes = f.Key.Month,
                              dados = f.Count()

                          });


            if (_qteregistros > 0)
                query = query.Take(_qteregistros);

            return query.ToList();
        }
        public IList<TabelasGrupoAcesso> BuscarSimuladorPorGrupo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, int _grupo = 0, string _tdc_id = null, int _qteregistros = 0)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            int lastday = DateTime.DaysInMonth(_dtfim.Value.Year, _dtfim.Value.Month);

            _dtinicial = new DateTime(_dtini.Value.Year, _dtini.Value.Month, 1);
            _dtfinal = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, lastday);

            var query = (from l in db.LOG_SIMULADOR
                         join t in db.TAB_DINAMICA_CONFIG on l.TDC_ID equals t.TDC_ID
                         join g in db.TAB_DINAMICA_GRUPO on t.TGR_ID equals g.TGR_ID
                         where (l.LSI_DATA_ACESSO >= _dtinicial && l.LSI_DATA_ACESSO <= _dtfinal) &&
                               (l.LSI_TIPO_ACESSO == "C") &&
                               (t.TDC_TIPO == 2) &&
                               (l.ASN_NUM_ASSINATURA != "99A00001") &&
                               ((_grupo == 0) || (_grupo != 0 && _grupo == (int)t.TGR_ID)) &&
                               ((_tdc_id == null) || (_tdc_id != null && _tdc_id == t.TDC_ID))
                         group l by new { g.TGR_DESCRICAO, l.LSI_DATA_ACESSO.Year, l.LSI_DATA_ACESSO.Month, t.TDC_ID, t.TDC_NOME_TABELA } into f
                         orderby f.Key.TGR_DESCRICAO, f.Key.Year, f.Key.Month, f.Count() descending
                         select new TabelasGrupoAcesso
                         {
                             grupo = f.Key.TGR_DESCRICAO,
                             nome = f.Key.TDC_NOME_TABELA,
                             ano = f.Key.Year,
                             mes = f.Key.Month,
                             dados = f.Count()

                         });


            if (_qteregistros > 0)
                query = query.Take(_qteregistros);

            return query.ToList();
        }
      
        
        public IList<QTDE_ACESSOS_POR_CLIENTE_VW> BuscarAcessoClientesPorPeriodo(int _mes, int _ano, string _tdc_id)
        {
            var query = (from l in dbcorp.QTDE_ACESSOS_POR_CLIENTE_VW
                         where (l.MES_DATA_ACESSO == _mes) &&
                               (l.ANO_DATA_ACESSO == _ano) &&
                               (l.TIPO_ACESSO == "L" ) &&
                               (l.IDTABELA  == _tdc_id) 
                         select l).AsQueryable();

            return query.ToList();
        }
        public IList<QTDE_ACESSOS_POR_CLIENTE_VW> BuscarAcessoClientesPorPeriodo(int _mes, int _ano, string _tipo_acesso, string _assinatura)
        {
            
            var query = (from l in db.LOG_SIMULADOR
                         join t in db.TAB_DINAMICA_CONFIG on l.TDC_ID equals t.TDC_ID
                         join g in db.TAB_DINAMICA_GRUPO on t.TGR_ID equals g.TGR_ID
                         //join a in dbcorp.ASSINATURA on l.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         //join c in dbcorp.CLIENTES on a.CLI_ID equals c.CLI_ID
                        where (l.LSI_DATA_ACESSO.Month == _mes) &&
                              (l.LSI_DATA_ACESSO.Year == _ano) &&
                              (l.ASN_NUM_ASSINATURA != "99A00001") &&
                              (l.LSI_TIPO_ACESSO == _tipo_acesso) &&
                              (l.ASN_NUM_ASSINATURA == _assinatura)
                         group l by new { l.LSI_DATA_ACESSO.Year, l.LSI_DATA_ACESSO.Month, t.TDC_ID, t.TDC_NOME_TABELA, l.ASN_NUM_ASSINATURA, l.LSI_TIPO_ACESSO } into f
                       orderby f.Key.Year, f.Key.Month, f.Count() descending
                        select new QTDE_ACESSOS_POR_CLIENTE_VW
                         {
                             IDTABELA = f.Key.TDC_ID,
                             NOME_TABELA = f.Key.TDC_NOME_TABELA,
                             ASSINATURA = f.Key.ASN_NUM_ASSINATURA,
                             //NOME_CLIENTE = f.Key.CLI_NOME,
                             TIPO_ACESSO = f.Key.LSI_TIPO_ACESSO,
                             MES_DATA_ACESSO = f.Key.Month,
                             ANO_DATA_ACESSO = f.Key.Year,
                             QTDE = f.Count()

                             //grupo = f.Key.TGR_DESCRICAO,
                             //nome = f.Key.TDC_NOME_TABELA,
                             //ano = f.Key.Year,
                             //mes = f.Key.Month,
                             //dados = f.Count()

                         }).AsQueryable();


            return query.ToList();
        }
        public IList<LISTA_ACESSOS_POR_CLIENTE_VW> BuscarListaClientesPorPeriodo(int _mes, int _ano, string _tdc_id)
        {
            var query = (from l in dbcorp.LISTA_ACESSOS_POR_CLIENTE_VW
                         where (l.DATA_ACESSO.Month == _mes) &&
                               (l.DATA_ACESSO.Year == _ano) &&
                               (l.TIPO_ACESSO == "L") &&
                               (l.IDTABELA == _tdc_id)
                         select l).AsQueryable();

            return query.ToList();
        }
 

    }
}

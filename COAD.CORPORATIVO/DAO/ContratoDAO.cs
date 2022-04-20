using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using System.Data.Objects;
using Coad.GenericCrud.Extensions;
using Coad.GenericCrud.Repositorios.Base;
using COAD.UTIL.Grafico;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using COAD.SEGURANCA.DAO;
using System.Data.Objects.SqlClient;

namespace COAD.CORPORATIVO.DAO
{

    public class CONFERENCIA_FINANCEIRA_HEADER
    {
        public CONFERENCIA_FINANCEIRA_HEADER()
        {
            this.RELATORIO = new HashSet<CONFERENCIA_FINANCEIRA_TMP>();
            this.TOTAL_ENTRADA = new HashSet<CONFERENCIA_TOTAL_ENTRADA>();
            this.TOTAL_BAIXADO = new HashSet<CONFERENCIA_TOTAL_ENTRADA>();
            this.TOTAL_FATURAMENTO = new HashSet<CONFERENCIA_TOTAL_ENTRADA>();

        }

        public virtual ICollection<CONFERENCIA_FINANCEIRA_TMP> RELATORIO { get; set; }
        public virtual ICollection<CONFERENCIA_TOTAL_ENTRADA> TOTAL_ENTRADA { get; set; }
        public virtual ICollection<CONFERENCIA_TOTAL_ENTRADA> TOTAL_BAIXADO { get; set; }
        public virtual ICollection<CONFERENCIA_TOTAL_ENTRADA> TOTAL_FATURAMENTO { get; set; }
    }

    public class CONFERENCIA_TOTAL_ENTRADA
    {
        public string TIPO_PAGAMENTO { get; set; }
        public Nullable<decimal> VALOR_PARCELAS { get; set; }
        public Nullable<decimal> VALOR_JUROS { get; set; }
        public Nullable<decimal> VALOR_DESCONTO { get; set; }
        public Nullable<decimal> VALOR_PAGO { get; set; }

    }

    public class ContratoDAO : DAOAdapter<CONTRATOS, ContratoDTO, string>
    {

        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ContratoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public JsonGraficoResponse ContratosPorRepresentante(int _ano, int _mes, string _regiao_uf)
        {
            try
            {
                JsonGraficoResponse _resultado = new JsonGraficoResponse();

                _resultado.Titulo = "Contratos Representante (P/ data de Faturamento)";
                _resultado.Descricao = "Faturmento Mes: " + _mes.ToString() + "/" + _ano.ToString() + " Região " + _regiao_uf;

                List<ContratosPorRepresentanteDTO> _lista = new List<ContratosPorRepresentanteDTO>();

                var _retorno = this.BuscarFaturamentoRepresentanteSint(_mes, _ano, 2, null, null);

                foreach (var _item in _retorno)
                {
                    ContratosPorRepresentanteDTO c = new ContratosPorRepresentanteDTO();
                    c.MES = _mes;
                    c.REP_ID = _item.REP_ID;
                    c.REP_NOME = _item.REP_NOME;
                    c.CAR_ID = _item.CAR_ID;
                    c.QTDE = 1;

                    _lista.Add(c);

                }

                foreach (var _item in _lista)
                {
                    JsonGrafico g = new JsonGrafico();
                    g.label = _item.REP_NOME + " (" + _item.CAR_ID + ")";
                    g.data = (int)_item.QTDE;
                    _resultado.Dados.Add(g);
                }

                return _resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
        public IList<RPT_CONTRATOS_TIPO_PGTO_Result> ListarContratoTipoPgto(DateTime _dtini
                                                                          , DateTime _dtfim
                                                                          , int _emp_id
                                                                          , bool _tipodata
                                                                          , int _qtdeParcelas = 0
                                                                          , int _grupo_id = 0)
        {

            var _dtfinal = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _dtfinal = _dtfinal.AddDays(1);

            IList<RPT_CONTRATOS_TIPO_PGTO_Result> _query = db.RPT_CONTRATOS_TIPO_PGTO(_dtini, _dtfinal, _emp_id, _tipodata, _qtdeParcelas, _grupo_id).ToList();

            return _query;

        }
        public JsonGraficoDataSource BuscarContratoTipoPgto(int _mes, int _ano, int _emp_id, int _qtdeParcelas = 0, int? _grupo_id = 0)
        {

            var _datasource = new JsonGraficoDataSource();
            var _listacategoria = new List<tp_label>();
            var _series = new List<tp_data>();
            var _categories = new tp_categories();
            decimal _totaldataset = 0;


            var  _query = db.GRAFICO_CONTRATOS_TIPO_PGTO(_ano, _mes, _emp_id, _qtdeParcelas, _grupo_id).ToList();


            if (_qtdeParcelas == 0)
            {
                for (var ind = 1; ind <= 12; ind++)
                {
                    var _categoria = new tp_label();
                    _categoria.label = ind.ToString();
                    _listacategoria.Add(_categoria);

                }
            }
            else
            {
                var _categoria = new tp_label();
                _categoria.label = _qtdeParcelas.ToString();
                _listacategoria.Add(_categoria);
            }

            string _tipo_pagamento = null;

            foreach (var a in _query)
            {

                if (a.TIPO_PAGAMENTO != _tipo_pagamento || _tipo_pagamento == null)
                {
                    _tipo_pagamento = a.TIPO_PAGAMENTO;

                    var _serie = new tp_data();
                    _serie.seriesname = a.TIPO_PAGAMENTO;
                    _series.Add(_serie);
                }

            }

            foreach (var a in _series)
            {

                if (_qtdeParcelas == 0)
                {

                    for (var ind = 1; ind <= 12; ind++)
                    {
                        var _item = _query.Where(x => x.QTDE_PARCELAS == ind && x.TIPO_PAGAMENTO == a.seriesname).FirstOrDefault();

                        if (_item != null)
                        {
                            _totaldataset += (decimal)_item.VALOR_PARCELAS;

                            var _value = new tp_value();
                            _value.value = _item.VALOR_PARCELAS.ToString();
                            a.data.Add(_value);
                        }
                        else
                        {
                            var _value = new tp_value();
                            _value.value = "0";
                            a.data.Add(_value);
                        }

                    }
                }
                else
                {
                    var _item = _query.Where(x => x.QTDE_PARCELAS == _qtdeParcelas && x.TIPO_PAGAMENTO == a.seriesname).FirstOrDefault();

                    if (_item != null)
                    {
                        _totaldataset += (decimal)_item.VALOR_PARCELAS;

                        var _value = new tp_value();
                        _value.value = _item.VALOR_PARCELAS.ToString();
                        a.data.Add(_value);
                    }
                    else
                    {
                        var _value = new tp_value();
                        _value.value = "0";
                        a.data.Add(_value);
                    }
                }
            }


            _datasource.dataset = _series;

            _categories.category = _listacategoria;

            _datasource.totaldataset = _totaldataset; 

            _datasource.categories.Add(_categories);

            _datasource.totaldataset = 10;

            return _datasource;
        }
        public JsonGraficoDataSource BuscarQtdeValor(DateTime _dtini, DateTime _dtfim, int _emp_id, int? _qtdeParcelas = 0, int? _grupo_id = 0)
        {

            var _datasource = new JsonGraficoDataSource();
            var _listadados = new List<tp_dataPieColumn3d>();
            var _listacategoria = new List<tp_label>();
            decimal _totaldataset = 0;

            if (_qtdeParcelas == 0)
            {
                for (var ind = 1; ind <= 12; ind++)
                {
                    var _categoria = new tp_label();
                    _categoria.label = ind.ToString();
                    _listacategoria.Add(_categoria);

                }
            }
            else
            {
                var _categoria = new tp_label();
                _categoria.label = _qtdeParcelas.ToString();
                _listacategoria.Add(_categoria);
            }

            var _dtfinal = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _dtfinal = _dtfinal.AddDays(1);

            IList <RPT_CONTRATOS_TIPO_PGTO_Result> _query = db.RPT_CONTRATOS_TIPO_PGTO(_dtini, _dtfinal, _emp_id, true, _qtdeParcelas, _grupo_id).ToList();


            foreach (var b in _listacategoria)
            {

                int _qtdeparcelas = int.Parse(b.label);

                var _item = _query.Where(x => x.QTDE_PARCELAS == _qtdeparcelas).Sum(x => x.VALOR_PARCELAS);

                _totaldataset +=  System.Convert.ToDecimal(_item);

                if (_item != null)
                {
                    var _itemdb = new tp_dataPieColumn3d();
                    _itemdb.label = b.label;
                    _itemdb.value = _item.ToString();
                    _listadados.Add(_itemdb);
                }
                else
                {
                    var _itemdb = new tp_dataPieColumn3d();
                    _itemdb.label = b.label;
                    _itemdb.value = "0";
                    _listadados.Add(_itemdb);
                }

            }

            _datasource.totaldataset = _totaldataset;

            _datasource.data = _listadados;

            return _datasource;
        }
        public JsonGraficoDataSource BuscarProdutosTipoPgto(DateTime _dtini, DateTime _dtfim, int _emp_id, int _qtdeParcelas = 0, int? _grupo_id = 0)
        {

            var _datasource = new JsonGraficoDataSource();
            var _listadados = new List<tp_dataPieColumn3d>();

            var _lista = (from i in db.ITEM_PEDIDO
                          join a in db.ASSINATURA on i.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          group p by new { p.PRO_ID, p.PRO_NOME } into grp
                          select new ProdutosDTO
                          {
                              PRO_ID = grp.Key.PRO_ID,
                              PRO_NOME = grp.Key.PRO_NOME

                          }).ToList();


            IList<RPT_CONTRATOS_TIPO_PGTO_Result> _query = db.RPT_CONTRATOS_TIPO_PGTO(_dtini, _dtfim, _emp_id, true, _qtdeParcelas, _grupo_id).ToList();

            foreach (var b in _lista)
            {
                var _item = _query.Where(x => x.PRO_NOME == b.PRO_NOME).Sum(x => x.QTDE_PEDIDOS);

                if (_item != null)
                {
                    var _itemdb = new tp_dataPieColumn3d();
                    _itemdb.label = b.PRO_NOME;
                    _itemdb.value = _item.ToString();
                    _listadados.Add(_itemdb);
                }
                else
                {
                    var _itemdb = new tp_dataPieColumn3d();
                    _itemdb.label = b.PRO_NOME;
                    _itemdb.value = "0";
                    _listadados.Add(_itemdb);
                }

            }


            _datasource.data = _listadados;

            return _datasource;
        }
        public JsonGraficoDataSource BuscarVendasRepresentante(int _mes, int _ano, int? _emp_id, int? _grupo_id )
        {

            var _datasource = new JsonGraficoDataSource();
            
            var _lista = (from c in db.CONTRATOS
                          join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          join g in db.GRUPO on p.GRUPO_ID equals g.GRUPO_ID
                          join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID
                          where (c.CTR_DATA_FAT.Value.Year == _ano) &&
                                (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                     (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                     c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                     )
                                 )) &&
                                 ((c.CTR_DATA_FAT.Value.Year == _ano) &&
                                 (c.CTR_DATA_FAT.Value.Month == _mes) &&
                                 ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                 ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)))
                          group c by new { c.REP_ID, r.REP_NOME } into grp
                          select new tp_dataPieColumn3d()
                          {
                              label = grp.Key.REP_NOME
                             ,
                              value = SqlFunctions.StringConvert(grp.Sum(x => x.CTR_VLR_BRUTO))
                             ,
                              vlrdouble = grp.Sum(x => x.CTR_VLR_BRUTO)

                          }).OrderBy(x => x.value);

            _datasource.totaldataset = _lista.Sum(x => x.vlrdouble);

            _datasource.data = _lista.ToList();
            
            return _datasource;
        }
        public JsonGraficoDataSource BuscarVendasGrupo(int _mes, int _ano, int? _emp_id)
        {

            var _datasource = new JsonGraficoDataSource();
            var _listadados = new List<tp_dataPieColumn3d>();

            var _lista = (from c in db.CONTRATOS
                          join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          join g in db.GRUPO on p.GRUPO_ID equals g.GRUPO_ID
                          join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID
                          where (c.CTR_DATA_FAT.Value.Year == _ano) &&
                                (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                    c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                                )) &&
                                ((c.CTR_DATA_FAT.Value.Year  == _ano) &&
                                 (c.CTR_DATA_FAT.Value.Month == _mes)) &&
                                ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id))
                          group c by new { g.GRUPO_ID, g.GRU_DESCRICAO } into grp
                          select new tp_dataPieColumn3d() {
                              label = grp.Key.GRU_DESCRICAO
                             ,
                              value = SqlFunctions.StringConvert(grp.Sum(x => x.CTR_VLR_BRUTO))
                             ,
                              vlrdouble = grp.Sum(x => x.CTR_VLR_BRUTO)

                          }).OrderBy(x => x.value);

            _datasource.totaldataset = _lista.Sum(x => x.vlrdouble);

            _datasource.data = _lista.ToList();

            return _datasource;
        }
        public JsonGraficoDataSource BuscarContratosEvolucaoAnual(int _mes, int _anoini, int _ano, int? _emp_id, int? _grupo_id)
        {

            var _datasource = new JsonGraficoDataSource();
            var _listadados = new List<tp_dataPieColumn3d>();

            var _lista = (from c in db.CONTRATOS
                          join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          join g in db.GRUPO on p.GRUPO_ID equals g.GRUPO_ID
                          where (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                     c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                                )) &&
                                (c.CTR_DATA_FAT.Value.Year >= _anoini && c.CTR_DATA_FAT.Value.Year <= _ano) &&
                                ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id))
                          group c by new { c.CTR_DATA_FAT.Value.Year } into grp
                          select new tp_dataPieColumn3d()
                          {
                              label = SqlFunctions.StringConvert((double)grp.Key.Year)
                             ,
                              value = SqlFunctions.StringConvert(grp.Sum(x => (decimal)x.CTR_VLR_BRUTO))
                             ,
                              vlrdouble = grp.Sum(x => x.CTR_VLR_BRUTO)

                          }).OrderBy(x => x.label);

            if (_lista != null)
            {
                _datasource.totaldataset = _lista.Sum(x => x.vlrdouble);

                _datasource.data = _lista.ToList();

                decimal _vlrant = 0;
                bool _primeiro = true;
                foreach (var i in _datasource.data)
                {
                    if (!_primeiro)
                    {
                        i.perc = ((i.vlrdouble - _vlrant) / _vlrant) * 100;
                        i.perc = (decimal)Math.Round((double)i.perc, 2);
                    }

                    _primeiro = false;

                    _vlrant = (decimal)i.vlrdouble;
                }

            }

            return _datasource;
        }
        public JsonGraficoDataSource BuscarVendasEvolucao(int _mes, int _anoini, int _ano, int? _emp_id, int? _grupo_id)
        {

            var _datasource = new JsonGraficoDataSource();
            var _listadados = new List<tp_dataPieColumn3d>();

            var _lista = (from c in db.CONTRATOS
                          join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          join g in db.GRUPO on p.GRUPO_ID equals g.GRUPO_ID
                          where (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                     c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                                )) &&
                                ((c.CTR_DATA_FAT.Value.Year >= _anoini && c.CTR_DATA_FAT.Value.Year <= _ano) &&
                                 (c.CTR_DATA_FAT.Value.Month == _mes)) &&
                                ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id))
                          group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year } into grp
                          select new tp_dataPieColumn3d()
                          {
                              label = SqlFunctions.StringConvert((double)grp.Key.Month) + "/" + SqlFunctions.StringConvert((double)grp.Key.Year)
                             ,
                              value = SqlFunctions.StringConvert(grp.Sum(x => (decimal)x.CTR_VLR_BRUTO))
                             ,
                              vlrdouble = grp.Sum(x => x.CTR_VLR_BRUTO)

                          }).OrderBy(x => x.label);

            if (_lista!=null)
            {
                _datasource.totaldataset = _lista.Sum(x => x.vlrdouble);

                _datasource.data = _lista.ToList();

                decimal _vlrant = 0;
                bool _primeiro = true;
                foreach (var i in _datasource.data)
                {
                    if (!_primeiro)
                    {
                        i.perc = ((i.vlrdouble - _vlrant) / _vlrant) * 100;
                        i.perc = (decimal)Math.Round((double)i.perc, 2);
                    }

                    _primeiro = false;

                    _vlrant = (decimal)i.vlrdouble;
                }

            }

            return _datasource;
        }
        public JsonGraficoDataSource BuscarFaturamentoAnualSint(int? _ano, int? _emp_id, int? _rep_id, int? _grupo_id)
        {
            var _datasource = new JsonGraficoDataSource();

            var _query = (from a in db.ASSINATURA
                          join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                          join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                          join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                          from j2 in j1.DefaultIfEmpty()
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          where (c.CTR_DATA_FAT.Value.Year == _ano) &&
                                (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                     (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                     c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                     )
                                 )) &&
                                ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                ((_rep_id == null) || (_rep_id != null && c.REP_ID == _rep_id)) &&
                                ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id))
                          group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year } into grp
                          select new tp_dataPieColumn3d
                          {
                              label = SqlFunctions.StringConvert((double)grp.Key.Month) + "/" + SqlFunctions.StringConvert((double)grp.Key.Year)
                             ,value = SqlFunctions.StringConvert(grp.Sum(x => (decimal)x.CTR_VLR_BRUTO))
                             ,vlrdouble = grp.Sum(x => x.CTR_VLR_BRUTO)
                          }).ToList();

            _datasource.totaldataset = _query.Sum(x => x.vlrdouble);

            _datasource.data = _query;

            return _datasource;
        }
        public JsonGraficoDataSource RelatorioFaturamentoFranquia(int? ano, int? mes, int? UEN_ID = 1)
        {
            var _datasource = new JsonGraficoDataSource();

            var _query = (
                    from car in db.CARTEIRA_CLIENTE
                    join cli in db.CLIENTES on car.CLI_ID equals cli.CLI_ID
                    join ass in db.ASSINATURA on cli.CLI_ID equals ass.CLI_ID
                    join con in db.CONTRATOS on ass.ASN_NUM_ASSINATURA equals con.ASN_NUM_ASSINATURA
                    where
                       ass.PRO_ID == 40 &&
                       car.CARTEIRA.UEN_ID == UEN_ID &&
                       con.CTR_DATA_FAT != null &&
                       ((DateTime)con.CTR_DATA_FAT).Year == ano &&
                       ((DateTime)con.CTR_DATA_FAT).Month == mes
                    group new { car, con } by
                         car.CARTEIRA.REGIAO.RG_DESCRICAO
                        into grp
                    select new tp_dataPieColumn3d
                    {
                        label = grp.Key
                       ,value = SqlFunctions.StringConvert(grp.Sum(x => (decimal)x.con.CTR_VLR_BRUTO))
                       ,vlrdouble = grp.Sum(x => (decimal)x.con.CTR_VLR_BRUTO)

                    }).OrderBy(or => or.label).ToList();

            _datasource.totaldataset = _query.Sum(x => x.vlrdouble);

            _datasource.data = _query;

            return _datasource;

        }
        public IList<ContratoDTO> BuscarPorAssinatura(string _asn_num_assinatura, int numpagina = 1, int linhas = 10)
        {
            var query = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == _asn_num_assinatura).OrderByDescending(x => x.CTR_DATA_INI_VIGENCIA).ToList();

            var listaContratoDTO = ToDTO(query);

            foreach (var item in listaContratoDTO)
            {
                if (DateTime.Now >= item.CTR_DATA_INI_VIGENCIA && DateTime.Now <= item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "VIGENTE";
                else if (DateTime.Now > item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "ENCERRADO";
                else if (DateTime.Now < item.CTR_DATA_INI_VIGENCIA)
                    item.SITUACAO = "FUTURO";

                if (item.CTR_PRORROGADO == 1 && DateTime.Now > item.CTR_DATA_FIM_VIGENCIA && item.CTR_DATA_CANC == null)
                    item.SITUACAO = "PRORROGADO";

                if (item.CTR_DATA_CANC != null)
                    item.SITUACAO = "CANCELADO";

                if (item.CTR_DATA_CANC < item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "CANC.DESISTÊNCIA";

            }


            return listaContratoDTO;
        }
        public ContratoDTO BuscarUltimoContratoValido(string numeroAssinatura)
        {
            var query = (from con in db.CONTRATOS
                         where con.ASN_NUM_ASSINATURA == numeroAssinatura &&
                         (DateTime.Now <= con.CTR_DATA_FIM_VIGENCIA)
                         && (con.CTR_DATA_CANC == null) 
                         orderby con.CTR_DATA_INI_VIGENCIA descending
                         select con);

            var listaContratoDTO = ToDTO(query.FirstOrDefault());
            return listaContratoDTO;
        }
        public string BuscarUltimoContrato(string _asn)
        {
            var query = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == _asn).Max(x => x.CTR_NUM_CONTRATO);

            return query;
        }
        public ContratoDTO BuscarUltimoContrato(string _asn, bool _ativos)
        {
            var query = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == _asn && x.CTR_DATA_CANC == null).Max(x => x.CTR_NUM_CONTRATO);

            var _ret = db.CONTRATOS.Where(x => x.CTR_NUM_CONTRATO == query).FirstOrDefault();

            return ToDTO(_ret);
        }
        public ContratoDTO BuscarContratoNF(int _nf_numero, int _cli_id)
        {
            var _query = (from c in db.CLIENTES
                          join a in db.ASSINATURA on c.CLI_ID equals a.CLI_ID
                          join t in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals t.ASN_NUM_ASSINATURA
                          where (t.CTR_NUMERO_NOTA == _nf_numero && a.CLI_ID == _cli_id)
                          select t).FirstOrDefault();

            return ToDTO(_query);
        }
        public ContratoDTO BuscarUltimoObjetoContrato(string objAssinatura)
        {
            var query = (from con in db.CONTRATOS
                         where con.ASN_NUM_ASSINATURA == objAssinatura
                         orderby con.CTR_DATA_INI_VIGENCIA descending
                         select con);
            return ToDTO(query.FirstOrDefault());
        }
        public Pagina<ContratoDTO> ListarContratos(string numeroAssinatura, int pagina = 1, int registrosPorPagina = 7)
        {
            var query = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == numeroAssinatura).
                OrderByDescending(x => x.CTR_DATA_INI_VIGENCIA).
                ThenByDescending(x => x.CTR_NUM_CONTRATO);

            var listaContratoDTO = ToDTOPage(query, pagina, registrosPorPagina);

            return listaContratoDTO;
        }
        public Pagina<ContratoDTO> ListarContratos(string numeroAssinatura, int? IPE_ID = null, int? IPE_ID_EXCLUIR = null, int pagina = 1, int registrosPorPagina = 7)
        {
            var query = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == numeroAssinatura &&
                (IPE_ID == null || x.IPE_ID == IPE_ID) &&
                (IPE_ID_EXCLUIR == null || x.IPE_ID != IPE_ID_EXCLUIR)
                ).
                OrderByDescending(x => x.CTR_DATA_INI_VIGENCIA).
                ThenByDescending(x => x.CTR_NUM_CONTRATO);

            var listaContratoDTO = ToDTOPage(query, pagina, registrosPorPagina);

            return listaContratoDTO;
        }
        public IList<ContratoDTO> ListarContratosDoItemPedido(int? ipeId, string codAssinatura = null)
        {
            if (string.IsNullOrWhiteSpace(codAssinatura))
            {
                codAssinatura = null;
            }
            var query = (from con in db.CONTRATOS
                         where
                            con.IPE_ID == ipeId &&
                            (
                                codAssinatura == null ||
                                con.ASN_NUM_ASSINATURA == codAssinatura
                            )
                         orderby con.CTR_NUM_CONTRATO descending
                         select con);

            return ToDTO(query);
        }
        public IList<RelContratosCanceladosDTO> BuscarContratosCancelados(int _mes, int _ano, int? _emp_id, int? _grupo_id, int _tipo, int? _rep_id)
        {
            var listaContratoDTO = (from a in db.ASSINATURA
                                    join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                                    join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                                    join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                                    join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                                    from j2 in j1.DefaultIfEmpty()
                                    where ((c.CTR_DATA_CANC.Value.Year  == _ano) &&
                                           (c.CTR_DATA_CANC.Value.Month == _mes) &&
                                           (c.CTR_DATA_CANC < c.CTR_DATA_FIM_VIGENCIA))  &&
                                          ((_emp_id   == null) || (_emp_id   != null && c.EMP_ID   == _emp_id))   &&
                                          ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                                          ((_rep_id   == null) || (_rep_id   != null && c.REP_ID   == _rep_id))
                                    select new RelContratosCanceladosDTO()
                                    {
                                        ASN_NUM_ASSINATURA = a.ASN_NUM_ASSINATURA,
                                        CTR_NUM_CONTRATO = c.CTR_NUM_CONTRATO,
                                        CLI_ID = a.CLI_ID,
                                        CLI_NOME = a.CLIENTES.CLI_NOME,
                                        ASN_A_C = a.ASN_A_C,
                                        CTR_DATA_FAT = c.CTR_DATA_FAT,
                                        CTR_DATA_CANC = c.CTR_DATA_CANC,
                                        CTR_DATA_INI_VIGENCIA = c.CTR_DATA_INI_VIGENCIA,
                                        CTR_DATA_FIM_VIGENCIA = c.CTR_DATA_FIM_VIGENCIA,
                                        CTR_VLR_CONTRATO = c.CTR_VLR_BRUTO,
                                        REP_NOME = j2.REP_NOME,
                                        CAR_ID = db.CARTEIRA_REPRESENTANTE.Where(x => x.EMP_ID == c.EMP_ID && x.REP_ID == c.REP_ID).FirstOrDefault().CAR_ID,
                                        CTR_VLR_BRUTO = c.CTR_VLR_BRUTO

                                    }
                       ).OrderBy(x => x.CTR_DATA_CANC).ToList();

            return listaContratoDTO;
        }
        public IList<RelFaturamentoProdutoDTO> BuscarFaturamentoProduto(DateTime _dtini
                                                                      , DateTime _dtfim
                                                                      , int? _emp_id
                                                                      , int? _grupo_id
                                                                      , bool _tipodata)
        {
            var _dtfinal = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _dtfinal = _dtfinal.AddDays(1);

            var query = (from a in db.ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                         join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                         where (((_tipodata)  && ((c.CTR_DATA_FAT.Value >= _dtini) && (c.CTR_DATA_FAT.Value < _dtfinal))) ||
                                ((_tipodata==false) && ((c.CTR_DATA_FATURAMENTO_EFETIVO.Value >= _dtini) && (c.CTR_DATA_FATURAMENTO_EFETIVO.Value < _dtfinal))) )&&
                                 (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                    c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                               )) &&
                               ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                               ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id))
                               && (c.CTR_VLR_BRUTO > 0)
                         group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year, a.PRO_ID, p.PRO_SIGLA } into grp
                         select new RelFaturamentoProdutoDTO()
                         {
                             MES_FAT = grp.Key.Month,
                             ANO_FAT = grp.Key.Year,
                             PRO_ID = grp.Key.PRO_ID,
                             PRO_NOME = grp.Key.PRO_SIGLA,
                             QTDE_CONTRATOS = grp.Count(),
                             VENDA_NOVA = 0,
                             RENOVACAO = 0,
                             VALOR_TOTAL = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),
                             PR_MEDIO_VENDA = 0,
                             QTDE_VENDANOVA = 0,
                             QTDE_RENOVACAO = 0,
                             PR_MEDIO_RENOVACAO = 0,
                         }).OrderByDescending(x => x.VALOR_TOTAL).ThenByDescending(x => x.QTDE_CONTRATOS).ToList();

            var qvenda = (from a in db.ASSINATURA
                          join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                          join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          where (((_tipodata) && ((c.CTR_DATA_FAT.Value >= _dtini) && (c.CTR_DATA_FAT.Value < _dtfinal))) ||
                                 ((_tipodata == false) && ((c.CTR_DATA_FATURAMENTO_EFETIVO.Value >= _dtini) && (c.CTR_DATA_FATURAMENTO_EFETIVO.Value < _dtfinal)))) &&
                                  (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                     (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                     c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                     )
                                )) &&
                                ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                                (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA) == 1)
                                && (c.CTR_VLR_BRUTO > 0)
                          group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year, a.PRO_ID, p.PRO_SIGLA } into grp
                          select new RelFaturamentoProdutoDTO()
                          {
                              MES_FAT = grp.Key.Month,
                              ANO_FAT = grp.Key.Year,
                              PRO_ID = grp.Key.PRO_ID,
                              PRO_NOME = grp.Key.PRO_SIGLA,
                              QTDE_CONTRATOS = grp.Count(),
                              VENDA_NOVA = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),
                              RENOVACAO = 0,
                              VALOR_TOTAL = 0,
                              PR_MEDIO_VENDA = 0,
                              QTDE_VENDANOVA = grp.Count(),
                              QTDE_RENOVACAO = 0,
                              PR_MEDIO_RENOVACAO = 0,
                           }).OrderByDescending(x => x.VALOR_TOTAL).ThenByDescending(x => x.QTDE_CONTRATOS).ToList();


            var qrenovacao = (from a in db.ASSINATURA
                              join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                              join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                              join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                              where (((_tipodata) && ((c.CTR_DATA_FAT.Value >= _dtini) && (c.CTR_DATA_FAT.Value < _dtfinal))) ||
                                     ((_tipodata == false) && ((c.CTR_DATA_FATURAMENTO_EFETIVO.Value >= _dtini) && (c.CTR_DATA_FATURAMENTO_EFETIVO.Value < _dtfinal)))) &&
                                     (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                         (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                         c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                         )
                                    )) &&
                                    ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                    ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                                     (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA) > 1)
                                     && (c.CTR_VLR_BRUTO > 0)
                              group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year, a.PRO_ID, p.PRO_SIGLA } into grp
                              select new RelFaturamentoProdutoDTO()
                              {
                                  MES_FAT = grp.Key.Month,
                                  ANO_FAT = grp.Key.Year,
                                  PRO_ID = grp.Key.PRO_ID,
                                  PRO_NOME = grp.Key.PRO_SIGLA,
                                  QTDE_CONTRATOS = grp.Count(),
                                  VENDA_NOVA = 0,
                                  RENOVACAO = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),
                                  VALOR_TOTAL = 0,
                                  PR_MEDIO_VENDA = 0,
                                  QTDE_VENDANOVA = 0,
                                  QTDE_RENOVACAO = grp.Count(),
                                  PR_MEDIO_RENOVACAO = 0,
                              }).OrderByDescending(x => x.VALOR_TOTAL).ThenByDescending(x => x.QTDE_CONTRATOS).ToList();


            foreach (var item in query)
            {
                var venda = qvenda.Find(x => x.MES_FAT == item.MES_FAT &&
                                             x.ANO_FAT == item.ANO_FAT &&
                                             x.PRO_ID == item.PRO_ID);

                var renovacao = qrenovacao.Find(x => x.MES_FAT == item.MES_FAT &&
                                                     x.ANO_FAT == item.ANO_FAT &&
                                                     x.PRO_ID == item.PRO_ID);

                if (venda != null){

                    item.VENDA_NOVA = venda.VENDA_NOVA;
                    item.QTDE_VENDANOVA = venda.QTDE_VENDANOVA;
                    if (item.QTDE_VENDANOVA > 0)
                        item.PR_MEDIO_VENDA = Math.Round((decimal)(item.VENDA_NOVA / item.QTDE_VENDANOVA), 2);
                }

                if (renovacao != null){
                    item.RENOVACAO = renovacao.RENOVACAO;
                    item.QTDE_RENOVACAO = renovacao.QTDE_RENOVACAO;
                    if (item.QTDE_RENOVACAO > 0)
                        item.PR_MEDIO_RENOVACAO = Math.Round((decimal)(item.RENOVACAO / item.QTDE_RENOVACAO), 2);
                }
        
            }


            return query;
        }
        public IList<RelFaturamentoRepresentanteDTO> BuscarFaturamentoRepresentante(DateTime _dtini, DateTime _dtfim, int? _emp_id, int? _rep_id, int? _grupo_id, int _tipo)
        {

            var _dataini = new DateTime(_dtini.Year, _dtini.Month, _dtini.Day);
            var _datafim = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);

            _datafim = _datafim.AddDays(1);

            var query = (from a in db.ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                         join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                         from j2 in j1.DefaultIfEmpty()
                         join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                         where ((c.CTR_DATA_FAT >= _dataini) &&
                               (c.CTR_DATA_FAT  < _datafim)) &&
                               (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                    c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                                )) &&
                                ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                ((_rep_id == null) || (_rep_id != null && c.REP_ID == _rep_id)) &&
                                ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                                ((_tipo == 0) ||
                                 ((_tipo == 1) && (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT <= c.CTR_DATA_FAT) > 1)) ||
                                 ((_tipo == 2) && (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT <= c.CTR_DATA_FAT) == 1))
                                )

                         select new RelFaturamentoRepresentanteDTO()
                         {
                             UF = db.CLIENTES_ENDERECO.Where(x => x.CLI_ID == a.CLI_ID).Max(x => x.END_UF),
                             CTR_DATA_FAT = c.CTR_DATA_FAT,
                             CTR_DATA_FAT_ANT = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT < c.CTR_DATA_FAT).Max(x => x.CTR_DATA_FAT),
                             CTR_NUM_CONTRATO_ANT = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT < c.CTR_DATA_FAT).Max(x => x.CTR_NUM_CONTRATO),
                             VALOR_TOTAL_ANT = (from c1 in db.CONTRATOS
                                                where c1.CTR_NUM_CONTRATO == db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT < c.CTR_DATA_FAT).Max(x => x.CTR_NUM_CONTRATO)
                                                select c1).FirstOrDefault().CTR_VLR_BRUTO,
                             MES_FAT = c.CTR_DATA_FAT.Value.Month,
                             ANO_FAT = c.CTR_DATA_FAT.Value.Year,
                             REP_ID = j2.REP_ID,
                             REP_NOME = j2.REP_NOME,
                             CAR_ID = db.CARTEIRA_REPRESENTANTE.Where(x => x.EMP_ID == c.EMP_ID && x.REP_ID == c.REP_ID).FirstOrDefault().CAR_ID,
                             ASN_NUM_ASSINATURA = a.ASN_NUM_ASSINATURA,
                             CTR_NUM_CONTRATO = c.CTR_NUM_CONTRATO,
                             CLI_ID = a.CLI_ID,
                             CLI_NOME = l.CLI_NOME,
                             VALOR_TOTAL = c.CTR_VLR_BRUTO,
                             ASN_QTDE_CONS_CONTRATO = a.ASN_QTDE_CONS_CONTRATO,
                         }).OrderByDescending(x => x.CTR_DATA_FAT).ToList();


            return query;
        }
        public IList<RelFaturamentoRepresentanteDTO> BuscarFaturamentoRepresentante(int? _mes, int? _ano, int? _emp_id, int? _rep_id, int? _grupo_id, int _tipo = 0)
        {

            var query = (from a in db.ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                         join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                         from j2 in j1.DefaultIfEmpty()
                         join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                         where ((c.CTR_DATA_FAT.Value.Year == _ano) &&
                               (c.CTR_DATA_FAT.Value.Month == _mes)) &&
                               (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                    c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                                )) &&
                                ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                ((_rep_id == null) || (_rep_id != null && c.REP_ID == _rep_id)) &&
                                ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                                ((_tipo == 0) ||
                                 ((_tipo == 1) && (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT <= c.CTR_DATA_FAT) > 1)) ||
                                 ((_tipo == 2) && (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT <= c.CTR_DATA_FAT) == 1))
                                )

                         select new RelFaturamentoRepresentanteDTO()
                         {
                             UF = db.CLIENTES_ENDERECO.Where(x => x.CLI_ID == a.CLI_ID).Max(x => x.END_UF),
                             CTR_DATA_FAT = c.CTR_DATA_FAT,
                             CTR_DATA_FAT_ANT = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT < c.CTR_DATA_FAT).Max(x => x.CTR_DATA_FAT),
                             CTR_NUM_CONTRATO_ANT = db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT < c.CTR_DATA_FAT).Max(x => x.CTR_NUM_CONTRATO),
                             VALOR_TOTAL_ANT = (from c1 in db.CONTRATOS
                                                where c1.CTR_NUM_CONTRATO == db.CONTRATOS.Where(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT < c.CTR_DATA_FAT).Max(x => x.CTR_NUM_CONTRATO)
                                                select c1).FirstOrDefault().CTR_VLR_BRUTO,
                             MES_FAT = c.CTR_DATA_FAT.Value.Month,
                             ANO_FAT = c.CTR_DATA_FAT.Value.Year,
                             REP_ID = j2.REP_ID,
                             REP_NOME = j2.REP_NOME,
                             CAR_ID = db.CARTEIRA_REPRESENTANTE.Where(x => x.EMP_ID == c.EMP_ID && x.REP_ID == c.REP_ID).FirstOrDefault().CAR_ID,
                             ASN_NUM_ASSINATURA = a.ASN_NUM_ASSINATURA,
                             CTR_NUM_CONTRATO = c.CTR_NUM_CONTRATO,
                             CLI_ID = a.CLI_ID,
                             CLI_NOME = l.CLI_NOME,
                             VALOR_TOTAL = c.CTR_VLR_BRUTO,
                             ASN_QTDE_CONS_CONTRATO = a.ASN_QTDE_CONS_CONTRATO,
                         }).OrderBy(x => x.CAR_ID).ThenByDescending(x => x.CTR_DATA_FAT).ToList();


            return query;
        }
        public IList<RelFaturamentoRepresentanteSintDTO> BuscarFaturamentoRepresentanteSint(DateTime _dtini, DateTime _dtfim, int? _emp_id, int? _rep_id, int? _grupo_id)
        {
            var _dataini = new DateTime(_dtini.Year, _dtini.Month, _dtini.Day);
            var _datafim = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);

            _datafim = _datafim.AddDays(1);

            var query = (from a in db.ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                         join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                         from j2 in j1.DefaultIfEmpty()
                         join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                         where ((c.CTR_DATA_FAT >= _dataini) &&
                               (c.CTR_DATA_FAT < _datafim)) &&
                               (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                    c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                                )) &&
                               ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                               ((_rep_id == null) || (_rep_id != null && c.REP_ID == _rep_id)) &&
                               ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id))
                         group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year, c.EMP_ID, c.REP_ID, j2.REP_NOME } into grp
                         select new RelFaturamentoRepresentanteSintDTO()
                         {
                             EMP_ID = grp.Key.EMP_ID,
                             MES_FAT = grp.Key.Month,
                             ANO_FAT = grp.Key.Year,
                             REP_ID = grp.Key.REP_ID,
                             CAR_ID = db.CARTEIRA_REPRESENTANTE.Where(x => x.EMP_ID == grp.Key.EMP_ID && x.REP_ID == grp.Key.REP_ID).FirstOrDefault().CAR_ID,
                             REP_NOME = grp.Key.REP_NOME,
                             QTDE_CONTRATOS = grp.Count(),
                             VALOR_RENOVACAO = 0,
                             VALOR_VENDA = 0,
                             VALOR_TOTAL = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),
                             VALOR_RECEBIDO = db.PARCELAS.Where(x => x.CONTRATOS.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                     x.CONTRATOS.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                     x.CONTRATOS.EMP_ID == grp.Key.EMP_ID &&
                                                                     x.CONTRATOS.REP_ID == grp.Key.REP_ID &&
                                                                     x.PAR_DATA_PAGTO != null).Sum(x => x.PAR_VLR_PAGO),
                             QTDE_PRODUTOS = db.CONTRATOS.Where(x => x.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                     x.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                     x.EMP_ID == grp.Key.EMP_ID &&
                                                                     x.REP_ID == grp.Key.REP_ID &&
                                                                     (x.ASSINATURA.PRODUTOS.GRUPO_ID == 3 ||
                                                                       x.ASSINATURA.PRODUTOS.GRUPO_ID == 4 ||
                                                                       x.ASSINATURA.PRODUTOS.GRUPO_ID == 5)).Count(),
                             VALOR_PRODUTO = db.CONTRATOS.Where(x => x.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                      x.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                      x.EMP_ID == grp.Key.EMP_ID &&
                                                                      x.REP_ID == grp.Key.REP_ID &&
                                                                      (x.ASSINATURA.PRODUTOS.GRUPO_ID == 3 ||
                                                                       x.ASSINATURA.PRODUTOS.GRUPO_ID == 4 ||
                                                                       x.ASSINATURA.PRODUTOS.GRUPO_ID == 5)).Sum(X => X.CTR_VLR_BRUTO),
                             QTDE_CURSO = db.CONTRATOS.Where(x => x.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                  x.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                  x.EMP_ID == grp.Key.EMP_ID &&
                                                                  x.REP_ID == grp.Key.REP_ID &&
                                                                  (x.ASSINATURA.PRODUTOS.GRUPO_ID == 2)).Count(),
                             VALOR_CURSO = db.CONTRATOS.Where(x => x.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                      x.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                      x.EMP_ID == grp.Key.EMP_ID &&
                                                                      x.REP_ID == grp.Key.REP_ID &&
                                                                      (x.ASSINATURA.PRODUTOS.GRUPO_ID == 2)).Sum(X => X.CTR_VLR_BRUTO),
                         }).OrderByDescending(x => x.VALOR_TOTAL).ToList();


            var query2 = (from a in db.ASSINATURA
                          join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                          join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                          join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                          from j2 in j1.DefaultIfEmpty()
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          where ((c.CTR_DATA_FAT >= _dataini) &&
                                 (c.CTR_DATA_FAT < _datafim)) &&
                                (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                    c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                                )) &&
                               ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                               ((_rep_id == null) || (_rep_id != null && c.REP_ID == _rep_id)) &&
                               ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                               (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT <= c.CTR_DATA_FAT) == 1)
                          group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year, c.EMP_ID, c.REP_ID, j2.REP_NOME } into grp
                          select new RelFaturamentoRepresentanteSintDTO()
                          {
                              MES_FAT = grp.Key.Month,
                              ANO_FAT = grp.Key.Year,
                              REP_ID = grp.Key.REP_ID,
                              QTDE_CONTRATOS = grp.Count(),
                              VALOR_RENOVACAO = 0,
                              VALOR_VENDA = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),
                              VALOR_TOTAL = 0,
                          }).ToList();


            var query3 = (from a in db.ASSINATURA
                          join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                          join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                          join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                          from j2 in j1.DefaultIfEmpty()
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          where ((c.CTR_DATA_FAT >= _dataini) &&
                                   (c.CTR_DATA_FAT < _datafim)) &&
                                   (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                        (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                        c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                        )
                                    )) &&
                                   ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                   ((_rep_id == null) || (_rep_id != null && c.REP_ID == _rep_id)) &&
                                   ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                                (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT <= c.CTR_DATA_FAT) > 1)
                          group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year, c.EMP_ID, c.REP_ID, j2.REP_NOME } into grp
                          select new RelFaturamentoRepresentanteSintDTO()
                          {
                              MES_FAT = grp.Key.Month,
                              ANO_FAT = grp.Key.Year,
                              REP_ID = grp.Key.REP_ID,
                              QTDE_CONTRATOS = grp.Count(),
                              VALOR_RENOVACAO = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),
                              VALOR_VENDA = 0,
                              VALOR_TOTAL = 0,
                          }).ToList();


            foreach (var item in query)
            {
                var venda = query2.Find(x => x.REP_ID == item.REP_ID);
                var renovacao = query3.Find(x => x.REP_ID == item.REP_ID);


                if (venda != null)
                {
                    item.VALOR_VENDA = venda.VALOR_VENDA;
                    item.QTDE_VENDA = venda.QTDE_CONTRATOS;
                }

                if (renovacao != null)
                {
                    item.VALOR_RENOVACAO = renovacao.VALOR_RENOVACAO;
                    item.QTDE_RENOVACAO = renovacao.QTDE_CONTRATOS;

                }

                if (item.VALOR_RECEBIDO > 0)
                {
                    item.PERC_RECEBIDO = (item.VALOR_RECEBIDO * 100) / item.VALOR_TOTAL;
                    item.PERC_RECEBIDO = Math.Round((decimal)item.PERC_RECEBIDO, 2);
                }

                item.QTDE_CURSO = (item.QTDE_CURSO > 0) ? item.QTDE_CURSO : 0;
                item.QTDE_PRODUTOS = (item.QTDE_PRODUTOS > 0) ? item.QTDE_PRODUTOS : 0;

                item.VALOR_CURSO = (item.VALOR_CURSO > 0) ? item.VALOR_CURSO : 0;
                item.VALOR_PRODUTO = (item.VALOR_PRODUTO > 0) ? item.VALOR_PRODUTO : 0;

                item.QTDE_ASSINATURA = item.QTDE_CONTRATOS - (item.QTDE_CURSO + item.QTDE_PRODUTOS);
                item.VALOR_ASSINATURA = item.VALOR_TOTAL - (item.VALOR_CURSO + item.VALOR_PRODUTO);

            }

            return query;
        }
        public IList<RelFaturamentoRepresentanteSintDTO> BuscarFaturamentoRepresentanteSint(int? _mes, int? _ano, int? _emp_id, int? _rep_id, int? _grupo_id)
        {


            var query = (from a in db.ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                         join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                         from j2 in j1.DefaultIfEmpty()
                         join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                         where ((c.CTR_DATA_FAT.Value.Year == _ano) &&
                               (c.CTR_DATA_FAT.Value.Month == _mes)) &&
                               (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                    c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                                )) &&
                               ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                               ((_rep_id == null) || (_rep_id != null && c.REP_ID == _rep_id)) &&
                               ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id))
                         group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year, c.EMP_ID, c.REP_ID, j2.REP_NOME } into grp
                         select new RelFaturamentoRepresentanteSintDTO()
                         {
                             EMP_ID = grp.Key.EMP_ID,
                             MES_FAT = grp.Key.Month,
                             ANO_FAT = grp.Key.Year,
                             REP_ID = grp.Key.REP_ID,
                             CAR_ID = db.CARTEIRA_REPRESENTANTE.Where(x => x.EMP_ID == grp.Key.EMP_ID && x.REP_ID == grp.Key.REP_ID).FirstOrDefault().CAR_ID,
                             REP_NOME = grp.Key.REP_NOME,
                             QTDE_CONTRATOS = grp.Count(),
                             VALOR_RENOVACAO = 0,
                             VALOR_VENDA = 0,
                             VALOR_TOTAL = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),
                             VALOR_RECEBIDO = db.PARCELAS.Where(x => x.CONTRATOS.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                     x.CONTRATOS.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                     x.CONTRATOS.EMP_ID == grp.Key.EMP_ID &&
                                                                     x.CONTRATOS.REP_ID == grp.Key.REP_ID &&
                                                                     x.PAR_DATA_PAGTO != null).Sum(x => x.PAR_VLR_PAGO),
                             QTDE_PRODUTOS = db.CONTRATOS.Where(x => x.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                     x.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                     x.EMP_ID == grp.Key.EMP_ID &&
                                                                     x.REP_ID == grp.Key.REP_ID &&
                                                                     (x.ASSINATURA.PRODUTOS.GRUPO_ID == 3 ||
                                                                       x.ASSINATURA.PRODUTOS.GRUPO_ID == 4 ||
                                                                       x.ASSINATURA.PRODUTOS.GRUPO_ID == 5)).Count(),
                             VALOR_PRODUTO = db.CONTRATOS.Where(x => x.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                      x.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                      x.EMP_ID == grp.Key.EMP_ID &&
                                                                      x.REP_ID == grp.Key.REP_ID &&
                                                                      (x.ASSINATURA.PRODUTOS.GRUPO_ID == 3 ||
                                                                       x.ASSINATURA.PRODUTOS.GRUPO_ID == 4 ||
                                                                       x.ASSINATURA.PRODUTOS.GRUPO_ID == 5) ).Sum(X => X.CTR_VLR_BRUTO),
                             QTDE_CURSO = db.CONTRATOS.Where(x => x.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                  x.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                  x.EMP_ID == grp.Key.EMP_ID &&
                                                                  x.REP_ID == grp.Key.REP_ID &&
                                                                  (x.ASSINATURA.PRODUTOS.GRUPO_ID == 2)).Count(),
                             VALOR_CURSO = db.CONTRATOS.Where(x => x.CTR_DATA_FAT.Value.Month == grp.Key.Month &&
                                                                      x.CTR_DATA_FAT.Value.Year == grp.Key.Year &&
                                                                      x.EMP_ID == grp.Key.EMP_ID &&
                                                                      x.REP_ID == grp.Key.REP_ID &&
                                                                      (x.ASSINATURA.PRODUTOS.GRUPO_ID == 2)).Sum(X => X.CTR_VLR_BRUTO),
                         }).OrderByDescending(x => x.VALOR_TOTAL).ToList();


            var query2 = (from a in db.ASSINATURA
                          join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                          join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                          join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                          from j2 in j1.DefaultIfEmpty()
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          where ((c.CTR_DATA_FAT.Value.Year == _ano) &&
                                (c.CTR_DATA_FAT.Value.Month == _mes)) &&
                                (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                    (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                    c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                    )
                                )) &&
                               ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                               ((_rep_id == null) || (_rep_id != null && c.REP_ID == _rep_id)) &&
                               ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                               (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT <= c.CTR_DATA_FAT) == 1)
                          group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year, c.EMP_ID, c.REP_ID, j2.REP_NOME } into grp
                          select new RelFaturamentoRepresentanteSintDTO()
                          {
                              MES_FAT = grp.Key.Month,
                              ANO_FAT = grp.Key.Year,
                              REP_ID = grp.Key.REP_ID,
                              QTDE_CONTRATOS = grp.Count(),
                              VALOR_RENOVACAO = 0,
                              VALOR_VENDA = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),
                              VALOR_TOTAL = 0,
                          }).ToList();


            var query3 = (from a in db.ASSINATURA
                          join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                          join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                          join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                          from j2 in j1.DefaultIfEmpty()
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          where ((c.CTR_DATA_FAT.Value.Year == _ano) &&
                                   (c.CTR_DATA_FAT.Value.Month == _mes)) &&
                                   (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                        (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                        c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                        )
                                    )) &&
                                   ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id)) &&
                                   ((_rep_id == null) || (_rep_id != null && c.REP_ID == _rep_id)) &&
                                   ((_grupo_id == null) || (_grupo_id != null && p.GRUPO_ID == _grupo_id)) &&
                                (db.CONTRATOS.Count(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA && x.CTR_DATA_FAT <= c.CTR_DATA_FAT) > 1)
                          group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year, c.EMP_ID, c.REP_ID, j2.REP_NOME } into grp
                          select new RelFaturamentoRepresentanteSintDTO()
                          {
                              MES_FAT = grp.Key.Month,
                              ANO_FAT = grp.Key.Year,
                              REP_ID = grp.Key.REP_ID,
                              QTDE_CONTRATOS = grp.Count(),
                              VALOR_RENOVACAO = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),
                              VALOR_VENDA = 0,
                              VALOR_TOTAL = 0,
                          }).ToList();


            foreach (var item in query)
            {
                var venda = query2.Find(x => x.REP_ID == item.REP_ID);
                var renovacao = query3.Find(x => x.REP_ID == item.REP_ID);


                if (venda != null)
                {
                    item.VALOR_VENDA = venda.VALOR_VENDA;
                    item.QTDE_VENDA = venda.QTDE_CONTRATOS;
                }

                if (renovacao != null)
                {
                    item.VALOR_RENOVACAO = renovacao.VALOR_RENOVACAO;
                    item.QTDE_RENOVACAO = renovacao.QTDE_CONTRATOS;

                }

                if (item.VALOR_RECEBIDO > 0)
                {
                    item.PERC_RECEBIDO = (item.VALOR_RECEBIDO * 100) / item.VALOR_TOTAL;
                    item.PERC_RECEBIDO = Math.Round((decimal)item.PERC_RECEBIDO, 2);
                }

                item.QTDE_CURSO = (item.QTDE_CURSO > 0) ? item.QTDE_CURSO : 0;
                item.QTDE_PRODUTOS = (item.QTDE_PRODUTOS > 0) ? item.QTDE_PRODUTOS : 0;

                item.VALOR_CURSO = (item.VALOR_CURSO > 0) ? item.VALOR_CURSO : 0;
                item.VALOR_PRODUTO = (item.VALOR_PRODUTO > 0) ? item.VALOR_PRODUTO : 0;

                item.QTDE_ASSINATURA = item.QTDE_CONTRATOS - (item.QTDE_CURSO + item.QTDE_PRODUTOS);
                item.VALOR_ASSINATURA = item.VALOR_TOTAL - (item.VALOR_CURSO + item.VALOR_PRODUTO);

            }

            return query;
        }
        public IList<RelFaturamentoAreaDTO> BuscarFaturamentoProdutoUF( DateTime _dtini
                                                                      , DateTime _dtfim
                                                                      , int _emp_id
                                                                      , int _grupo_id
                                                                      , bool _tipodata)
        {
            var _dtfinal = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _dtfinal = _dtfinal.AddDays(1);

            var _query = db.RPT_CONTRATOS_PRODUTO_REGIAO(_dtini, _dtfinal, _emp_id, _grupo_id, _tipodata).OrderBy(x => x.AREA_NOME).ThenBy(x => x.PRO_NOME).ThenBy(x => x.REGIAO_UF).ToList();

            var _listaArea = new List<RelFaturamentoAreaDTO>();
             

            var _total = _query.Sum(x => x.VALOR);

            var _area = new RelFaturamentoAreaDTO();
            var _produto = new RelFaturamentoAreaProdutoDTO();

            int? _AREA_ID = null;
            int? _PRO_ID = null;

            foreach (var _linha in _query)
            {

                if (_linha.PRO_ID != _PRO_ID)
                {
                    if (_PRO_ID != null)
                    {
                        _produto.QTDE = _produto.LISTAUF.Sum(x => x.QTDE);
                        _produto.VALOR = _produto.LISTAUF.Sum(x => x.VALOR);
                        _produto.CANCELADOS = _produto.LISTAUF.Sum(x => x.CANCELADOS);
                        _area.LISTAPROD.Add(_produto);
                    }

                    _PRO_ID = _linha.PRO_ID;
                    _produto = new RelFaturamentoAreaProdutoDTO();
                    _produto.PRO_ID = _linha.PRO_ID;
                    _produto.PRO_NOME = _linha.PRO_NOME;

                }

                if (_linha.AREA_ID != _AREA_ID)
                {
                    if (_AREA_ID != null)
                    {
                        _area.QTDE = _area.LISTAPROD.Sum(x => x.QTDE);
                        _area.VALOR = _area.LISTAPROD.Sum(x => x.VALOR);
                        _area.CANCELADOS = _area.LISTAPROD.Sum(x => x.CANCELADOS);
                        _listaArea.Add(_area);
                    }
                    _area = new RelFaturamentoAreaDTO();
                    _area.AREA_ID = _linha.AREA_ID;
                    _area.AREA_NOME = _linha.AREA_NOME;

                    _AREA_ID = _linha.AREA_ID;
                    _PRO_ID = _linha.PRO_ID;

                }

                _produto.LISTAUF.Add(_linha);

            }

            _produto.QTDE = _produto.LISTAUF.Sum(x => x.QTDE);
            _produto.VALOR = _produto.LISTAUF.Sum(x => x.VALOR);
            _produto.CANCELADOS = _produto.LISTAUF.Sum(x => x.CANCELADOS);
            _area.LISTAPROD.Add(_produto);
            _area.QTDE = _area.LISTAPROD.Sum(x => x.QTDE);
            _area.VALOR = _area.LISTAPROD.Sum(x => x.VALOR);
            _area.CANCELADOS = _area.LISTAPROD.Sum(x => x.CANCELADOS);
            _listaArea.Add(_area);

            return _listaArea;

        }
        public IList<RPT_CONTRATOS_POR_REGIAO_Result>  BuscarFaturamentoUF(DateTime _dtini
                                                                         , DateTime _dtfim
                                                                         , int _emp_id
                                                                         , int _grupo_id
                                                                         , bool _tipodata)

        {
            var _dtfinal = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _dtfinal = _dtfinal.AddDays(1);

            var query = db.RPT_CONTRATOS_POR_REGIAO(_dtini, _dtfinal, _emp_id, _grupo_id, _tipodata).ToList();

            return query;
        }
        public Pagina<RelFaturamentoContratoDTO> BuscarFaturamentoContrato(string _ctr, string _asn, int numpagina = 1, int linhas = 10)

        {
            var query = (from c in db.CONTRATOS
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join k in db.CLIENTES on a.CLI_ID equals k.CLI_ID
                         where ((_ctr == null) || (_ctr != null && c.CTR_NUM_CONTRATO == _ctr)) &&
                               ((_asn == null) || (_asn != null && c.ASN_NUM_ASSINATURA == _asn))
                         select new RelFaturamentoContratoDTO()
                         {
                             CTR_NUM_CONTRATO = c.CTR_NUM_CONTRATO,
                             ASN_NUM_ASSINATURA = c.ASN_NUM_ASSINATURA,
                             PED_NUM_PEDIDO = c.PED_NUM_PEDIDO,
                             CTR_DATA_FAT = c.CTR_DATA_FAT,
                             CTR_DATA_INI_VIGENCIA = c.CTR_DATA_INI_VIGENCIA,
                             CTR_DATA_FIM_VIGENCIA = c.CTR_DATA_FIM_VIGENCIA,
                             CLI_ID = k.CLI_ID,
                             CLI_NOME = k.CLI_NOME,
                             CTR_VLR_CONTRATO = c.CTR_VLR_BRUTO,
                             CTR_DATA_CANC = c.CTR_DATA_CANC,
                             CTR_NUMERO_NOTA = c.CTR_NUMERO_NOTA,
                             IPE_ID = c.IPE_ID,
                         }).OrderBy(x => x.CTR_DATA_FAT);


            var _resultado = query.Paginar(numpagina, linhas);
            _resultado.lista = _resultado.lista.ToList();

            foreach (var item in query)
            {
                if (DateTime.Now >= item.CTR_DATA_INI_VIGENCIA && DateTime.Now <= item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "Vigente";
                else if (DateTime.Now > item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "Encerrado";
                else if (DateTime.Now < item.CTR_DATA_INI_VIGENCIA)
                    item.SITUACAO = "Futuro";

                if (item.CTR_DATA_CANC <= item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "Cancelado";
            }

            return _resultado;

        }

        public Pagina<RelFaturamentoContratoDTO> BuscarFaturamentoContratoSint(DateTime? _dtini, DateTime? _dtfim, int? _grupo_id, bool _tipodata, int numpagina = 1, int linhas = 10)
        {

            DateTime _datafim = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, _dtfim.Value.Day);
            _datafim = _datafim.AddDays(1);

            var _query = (from c in db.CONTRATOS
                          join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          join k in db.CLIENTES on a.CLI_ID equals k.CLI_ID
                          where (_grupo_id == null || (_grupo_id != null && _grupo_id == p.GRUPO_ID))
                             && (((_tipodata)  && ((c.CTR_DATA_FAT >= _dtini)                 && (c.CTR_DATA_FAT < _datafim))) ||
                                 ((!_tipodata) && ((c.CTR_DATA_FATURAMENTO_EFETIVO >= _dtini) && (c.CTR_DATA_FATURAMENTO_EFETIVO < _datafim))))
                             && (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                                              (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                                               c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                                              )
                                                            )
                                )
                          group c by new { c.EMP_ID } into grp
                          select new RelFaturamentoContratoDTO()
                          {
                              EMP_ID = grp.Key.EMP_ID,
                              CTR_VLR_CONTRATO = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO),

                          }).OrderBy(x => x.EMP_ID).ToList();

            var _listaEmp   = new EmpresaDAO().FindAll();
            var _listaGeral = new List<RelFaturamentoContratoDTO>();

            foreach (var _item in _listaEmp)
            {
                var _linha = new RelFaturamentoContratoDTO();
                var _ite = _query.Where(x => x.EMP_ID == _item.EMP_ID).FirstOrDefault();

                _linha.EMP_ID = _item.EMP_ID;
                _linha.EMP_RAZAO_SOCIAL = _item.EMP_RAZAO_SOCIAL;

                if (_ite != null)
                {
                    _linha.CTR_VLR_CONTRATO = _ite.CTR_VLR_CONTRATO;
                }

                _listaGeral.Add(_linha);

            }


            return _listaGeral.Paginar(numpagina, linhas);
        }
        public Pagina<RelFaturamentoContratoDTO> BuscarFaturamentoContrato(DateTime? _dtini
                                                                         , DateTime? _dtfim
                                                                         , int? _emp_id
                                                                         , int? _grupo_id
                                                                         , bool _tipodata
                                                                         , int numpagina = 1
                                                                         , int linhas = 10)


        {

            var query = this.BuscarFaturamentoContrato(_dtini, _dtfim, _emp_id, _grupo_id, _tipodata);

            var _resultado = query.Paginar(numpagina, linhas);
            _resultado.lista = _resultado.lista.ToList();


            foreach (var item in _resultado.lista)
            {
                if (DateTime.Now >= item.CTR_DATA_INI_VIGENCIA && DateTime.Now <= item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "Vigente";
                else if (DateTime.Now > item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "Encerrado";
                else if (DateTime.Now < item.CTR_DATA_INI_VIGENCIA)
                    item.SITUACAO = "Futuro";

                if (item.CTR_DATA_CANC <= item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "Cancelado";
            }


            return _resultado;
        }

        public List<RelFaturamentoContratoDTO> BuscarFaturamentoContrato(DateTime? _dtini, DateTime? _dtfim, int? _emp_id, int? _grupo_id, bool _tipodata)
        {
            DateTime _datafim = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, _dtfim.Value.Day);
            _datafim = _datafim.AddDays(1);

            var _query = (from c in db.CONTRATOS
                          join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join p in db.PRODUTOS on a.PRO_ID equals p.PRO_ID
                          join k in db.CLIENTES on a.CLI_ID equals k.CLI_ID
                          where (_grupo_id == null || (_grupo_id != null && _grupo_id == p.GRUPO_ID))
                             && (((_tipodata)  && ((c.CTR_DATA_FAT >= _dtini) && (c.CTR_DATA_FAT < _datafim))) ||
                                 ((!_tipodata) && ((c.CTR_DATA_FATURAMENTO_EFETIVO >= _dtini) && (c.CTR_DATA_FATURAMENTO_EFETIVO < _datafim))))
                             && (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                                              (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                                                c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                                               )
                                                            ))
                             && ((_emp_id == null) || (_emp_id != null && c.EMP_ID == _emp_id))
                          select new RelFaturamentoContratoDTO()
                          {
                              CTR_NUM_CONTRATO = c.CTR_NUM_CONTRATO,
                              ASN_NUM_ASSINATURA = c.ASN_NUM_ASSINATURA,
                              PED_NUM_PEDIDO = c.PED_NUM_PEDIDO,
                              CTR_DATA_FAT = c.CTR_DATA_FAT,
                              CTR_DATA_INI_VIGENCIA = c.CTR_DATA_INI_VIGENCIA,
                              CTR_DATA_FIM_VIGENCIA = c.CTR_DATA_FIM_VIGENCIA,
                              CLI_ID = k.CLI_ID,
                              CLI_NOME = k.CLI_NOME,
                              CTR_VLR_CONTRATO = c.CTR_VLR_BRUTO,
                              CTR_DATA_CANC = c.CTR_DATA_CANC,
                              CTR_NUMERO_NOTA = c.CTR_NUMERO_NOTA,
                              IPE_ID = c.IPE_ID,
                              AEM_EMAIL = db.ASSINATURA_EMAIL.FirstOrDefault(x => x.ASN_NUM_ASSINATURA == c.ASN_NUM_ASSINATURA).AEM_EMAIL
                          }).OrderBy(x => x.CTR_DATA_FAT).ThenBy(x => x.PED_NUM_PEDIDO).ThenBy(x => x.CTR_NUM_CONTRATO).ToList();

            return _query;

        }
        public List<RelAReceberDTO> BuscarTitulosAReceberLista(  DateTime _dtini
                                                          , DateTime _dtfim
                                                          , int _emp_id
                                                          , int _tipodata = 0
                                                          , int _tiporel = 0
                                                          , int _tipobanco = 0
                                                          , string _banid = null
                                                          , int _grupoid = 0
                                                          , int _tipobaixa = 0)
        {
            if (_banid == "")
                _banid = null;


            DateTime _datafim = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _datafim = _datafim.AddDays(1);

            var _query = (from p in db.PARCELAS
                          join t in db.TIPO_PAGAMENTO on p.TPG_ID equals t.TPG_ID into t1
                          from t2 in t1.DefaultIfEmpty()
                          join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO into c1
                          from c2 in c1.DefaultIfEmpty()

                          join a in db.ASSINATURA on c2.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA into a1
                          from a2 in a1.DefaultIfEmpty()


                          join o in db.PRODUTOS on a2.PRO_ID equals o.PRO_ID into o1
                          from o2 in o1.DefaultIfEmpty()
                          where ((_tipodata == 0 && (p.PAR_DATA_PAGTO >= _dtini && p.PAR_DATA_PAGTO < _datafim)) ||
                                 (_tipodata == 1 && (p.PAR_DATA_VENCTO >= _dtini && p.PAR_DATA_VENCTO < _datafim)) ||
                                 (_tipodata == 2 && (c2.CTR_DATA_FAT >= _dtini && c2.CTR_DATA_FAT < _datafim))
                                 )
                             && ((_tiporel == 2) ||
                                 (_tiporel == 0 && p.PAR_DATA_PAGTO == null) ||
                                 (_tiporel == 1 && p.PAR_DATA_PAGTO != null))
                             && ((_tipobanco == 0) ||
                                 (_tipobanco == 1 && (p.REM_ID != null ||
                                                      p.PAR_DATA_ALOC != null)
                                 ) ||
                                 (_tipobanco == 2 && (p.REM_ID == null &&
                                                      p.PAR_DATA_ALOC == null &&
                                                      (p.PAR_ALOC_AUTOMATICA == false || p.PAR_ALOC_AUTOMATICA == null)
                                                      )
                                  ) ||
                                  (_tipobanco == 3 && (p.REM_ID == null &&
                                                      p.PAR_DATA_PAGTO == null &&
                                                      p.PAR_DATA_ALOC == null &&
                                                      p.PAR_PODE_ALOCAR == true &&
                                                      (p.PAR_ALOC_AUTOMATICA == false || p.PAR_ALOC_AUTOMATICA == null)
                                                      )
                                  )

                                  )
                             && ((_banid == null) ||
                                 (_banid != null && p.BAN_ID == _banid))
                             && ((_grupoid == 0) ||
                                 (_grupoid != 0 && o2.GRUPO_ID == _grupoid))
                             && (p.EMP_ID == _emp_id)
                             && (p.DATA_EXCLUSAO == null)
                             && (_tipobaixa == 0 || (_tipobaixa == 1  && p.PAR_BAIXA_MANUAL == false) ||
                                                    (_tipobaixa == 2  && p.PAR_BAIXA_MANUAL == true))
                          select new RelAReceberDTO()
                          {
                              TIPO_VENDA = o2.GRUPO.GRU_DESCRICAO,
                              CLI_ID = a2.CLI_ID,
                              CLI_NOME = a2.CLIENTES.CLI_NOME,
                              BAN_ID = p.BAN_ID,
                              ALOCACAO = p.PAR_DATA_ALOC,
                              REM_ID = p.REM_ID,
                              BAN_NOME = p.BANCOS.BAN_NOME,
                              FATURAMENTO = c2.CTR_DATA_FAT,
                              VENCIMENTO = p.PAR_DATA_VENCTO,
                              PAR_DATA_PAGTO = p.PAR_DATA_PAGTO,
                              ASN_NUM_ASSINATURA = a2.ASN_NUM_ASSINATURA,
                              CTR_NUM_CONTRATO = c2.CTR_NUM_CONTRATO,
                              PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                              VALOR_FATURADO = p.PAR_VLR_PARCELA,
                              VALOR_PAGO = p.PAR_VLR_PAGO,
                              TPG_ID = t2.TPG_ID,
                              TPG_DESCRICAO = t2.TPG_DESCRICAO

                          }).ToList();



            var _valorFaturado = _query.Sum(x => x.VALOR_FATURADO);
            var _valorPago = _query.Sum(x => x.VALOR_PAGO);

            var _total = new RelAReceberDTO();

            _total.TIPO_VENDA = "T O T A L";
            _total.VALOR_FATURADO = _valorFaturado;
            _total.VALOR_PAGO = _valorPago;

            _query.Add(_total);

           
            return _query.OrderBy(x => x.TIPO_VENDA).ToList();


        }

        public RelAReceberTotalisDTO BuscarTitulosAReceber(DateTime _dtini
                                                          ,DateTime _dtfim
                                                          ,int _emp_id
                                                          ,int _tipodata = 0
                                                          ,int _tiporel = 0
                                                          ,int _tipobanco = 0
                                                          ,string _banid = null
                                                          ,int _grupoid = 0
                                                          ,int _tipobaixa = 0
                                                          ,int _rem_id = 0 
                                                          ,int pagina = 1
                                                          ,int numpaginas = 12)
        {
            if (_banid == "")
                _banid = null;


            DateTime _datafim = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _datafim = _datafim.AddDays(1);

            var _query = (from p in db.PARCELAS 
                          join t in db.TIPO_PAGAMENTO on p.TPG_ID equals t.TPG_ID into t1
                          from t2 in t1.DefaultIfEmpty()
                          join c in db.CONTRATOS  on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO into c1
                          from c2 in c1.DefaultIfEmpty()

                          join a in db.ASSINATURA on c2.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA into a1
                          from a2 in a1.DefaultIfEmpty()


                          join o in db.PRODUTOS   on a2.PRO_ID equals o.PRO_ID into o1
                          from o2 in o1.DefaultIfEmpty()

                          where ((_tipodata == 0 && (p.PAR_DATA_PAGTO >= _dtini && p.PAR_DATA_PAGTO < _datafim)) ||
                                 (_tipodata == 1 && (p.PAR_DATA_VENCTO >= _dtini && p.PAR_DATA_VENCTO < _datafim)) ||
                                 (_tipodata == 2 && (c2.CTR_DATA_FAT >= _dtini && c2.CTR_DATA_FAT < _datafim))
                                 )
                             && ((_tiporel == 2) ||
                                 (_tiporel == 0 && p.PAR_DATA_PAGTO == null) ||
                                 (_tiporel == 1 && p.PAR_DATA_PAGTO != null))
                             && ((_tipobanco == 0) ||
                                 (_tipobanco == 1 && (p.REM_ID != null ||
                                                      p.PAR_DATA_ALOC != null)
                                 ) ||
                                 (_tipobanco == 2 && (p.REM_ID == null &&
                                                      p.PAR_DATA_ALOC == null &&
                                                      (p.PAR_ALOC_AUTOMATICA == false || p.PAR_ALOC_AUTOMATICA == null)
                                                      )
                                  ) ||
                                  (_tipobanco == 3 && (p.REM_ID == null &&
                                                      p.PAR_DATA_PAGTO == null &&
                                                      p.PAR_DATA_ALOC == null &&
                                                      p.PAR_PODE_ALOCAR == true &&
                                                      (p.PAR_ALOC_AUTOMATICA == false || p.PAR_ALOC_AUTOMATICA == null)
                                                      )
                                  )

                                  )
                             && ((_banid == null) ||
                                 (_banid != null && p.BAN_ID == _banid))
                             && ((_grupoid == 0) ||
                                 (_grupoid != 0 && o2.GRUPO_ID == _grupoid))
                             && (p.EMP_ID == _emp_id)
                             && (p.DATA_EXCLUSAO == null)
                             && (_tipobaixa == 0 || (_tipobaixa == 1 && p.PAR_BAIXA_MANUAL == false) ||
                                                    (_tipobaixa == 2 && p.PAR_BAIXA_MANUAL == true))


                          select new RelAReceberDTO()
                          {
                              TIPO_VENDA = o2.GRUPO.GRU_DESCRICAO,
                              CLI_ID = a2.CLI_ID,
                              CLI_NOME = a2.CLIENTES.CLI_NOME,
                              BAN_ID = p.BAN_ID,
                              ALOCACAO = p.PAR_DATA_ALOC,
                              REM_ID = p.REM_ID,
                              BAN_NOME = p.BANCOS.BAN_NOME,
                              FATURAMENTO = c2.CTR_DATA_FAT,
                              VENCIMENTO = p.PAR_DATA_VENCTO,
                              PAR_DATA_PAGTO = p.PAR_DATA_PAGTO,
                              ASN_NUM_ASSINATURA = a2.ASN_NUM_ASSINATURA,
                              CTR_NUM_CONTRATO = c2.CTR_NUM_CONTRATO,
                              PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                              VALOR_FATURADO = p.PAR_VLR_PARCELA,
                              VALOR_PAGO = p.PAR_VLR_PAGO,
                              TPG_ID = t2.TPG_ID,
                              TPG_DESCRICAO = t2.TPG_DESCRICAO

                          }).ToList();

            

            if (_rem_id > 0)
            {
                var _query02 = (from q in _query
                                where (from a in db.PARCELA_ALOCADA
                                       where a.REM_ID == _rem_id && a.PAR_NUM_PARCELA == q.PAR_NUM_PARCELA
                                       select a).Count() == 0
                                select q).ToList();

                //var _query03 = (from b in db.PARCELA_ALOCADA
                //                join p in db.PARCELAS on b.PAR_NUM_PARCELA equals p.PAR_NUM_PARCELA
                //                join t in db.TIPO_PAGAMENTO on p.TPG_ID equals t.TPG_ID into t1
                //                from t2 in t1.DefaultIfEmpty()
                //                join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO into c1
                //                from c2 in c1.DefaultIfEmpty()
                //                join a in db.ASSINATURA on c2.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA into a1
                //                from a2 in a1.DefaultIfEmpty()
                //                join o in db.PRODUTOS on a2.PRO_ID equals o.PRO_ID into o1
                //                from o2 in o1.DefaultIfEmpty()
                //                where (_query.Count(q =>  q.REM_ID == _rem_id && q.PAR_NUM_PARCELA == b.PAR_NUM_PARCELA)) == 0

                //                select new RelAReceberDTO()
                //                {
                //                    TIPO_VENDA = o2.GRUPO.GRU_DESCRICAO,
                //                    CLI_ID = a2.CLI_ID,
                //                    CLI_NOME = a2.CLIENTES.CLI_NOME,
                //                    BAN_ID = p.BAN_ID,
                //                    ALOCACAO = p.PAR_DATA_ALOC,
                //                    REM_ID = p.REM_ID,
                //                    BAN_NOME = p.BANCOS.BAN_NOME,
                //                    FATURAMENTO = c2.CTR_DATA_FAT,
                //                    VENCIMENTO = p.PAR_DATA_VENCTO,
                //                    PAR_DATA_PAGTO = p.PAR_DATA_PAGTO,
                //                    ASN_NUM_ASSINATURA = a2.ASN_NUM_ASSINATURA,
                //                    CTR_NUM_CONTRATO = c2.CTR_NUM_CONTRATO,
                //                    PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                //                    VALOR_FATURADO = p.PAR_VLR_PARCELA,
                //                    VALOR_PAGO = p.PAR_VLR_PAGO,
                //                    TPG_ID = t2.TPG_ID,
                //                    TPG_DESCRICAO = t2.TPG_DESCRICAO

                //                }).ToList();

                _query = _query02.ToList();
            }
            
            var _total = new RelAReceberTotalisDTO();

            _total.VALOR_FATURADO = _query.Sum(x => x.VALOR_FATURADO);
            _total.VALOR_PREVISTO = _query.Sum(x => x.VALOR_FATURADO);
            _total.VALOR_PAGO = _query.Sum(x => x.VALOR_PAGO);

            var _paginado = _query.Paginar<RelAReceberDTO>(pagina, numpaginas);

        
            _total.Lista = _paginado;

            return _total;


        }

        public IList<RelResumoCReceberDTO> BuscarResumoCReceber( DateTime _dtini
                                                               , DateTime _dtfim
                                                               , int _emp_id
                                                               , bool _tipodata)
        {
            var resp = new RelResumoCReceberDTO();
            var lista = new List<RelResumoCReceberDTO>();
            var _mes = _dtfim.Month;
            var _ano = _dtfim.Year;

            DateTime? _dataref = new DateTime(_ano, _mes, DateTime.DaysInMonth(_ano, _mes));

            var query = (from c in db.CONTRATOS
                         where ((_tipodata)  || ((c.CTR_DATA_FAT.Value >= _dtini) && (c.CTR_DATA_FAT.Value <= _dtfim))) 
                            && ((!_tipodata) || ((c.CTR_DATA_FATURAMENTO_EFETIVO.Value == _dtini) && (c.CTR_DATA_FATURAMENTO_EFETIVO.Value == _dtfim)))
                            && (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                                        (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                                        c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                                        )
                                                    ))
                            && ((_emp_id == 0) || (_emp_id != 0 && c.EMP_ID == _emp_id))
                         group c by new { c.CTR_DATA_FAT.Value.Month, c.CTR_DATA_FAT.Value.Year } into grp
                         select new RelResumoCReceberDTO()
                         {
                             MES_FAT = grp.Key.Month,
                             ANO_FAT = grp.Key.Year,
                             VALOR_FATURADO = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO)
                         }).FirstOrDefault();


            var query1 = (from c in db.CONTRATOS
                          join p in db.PARCELAS on c.CTR_NUM_CONTRATO equals p.CTR_NUM_CONTRATO
                          where ((_tipodata) || ((p.PAR_DATA_PAGTO.Value >= _dtini) && (p.PAR_DATA_PAGTO.Value <= _dtfim)))
                             && ((_emp_id == 0) || (_emp_id != 0 && c.EMP_ID == _emp_id))
                          group p by new { p.PAR_DATA_PAGTO.Value.Month, p.PAR_DATA_PAGTO.Value.Year } into grp
                          select new RelResumoCReceberDTO()
                          {
                              MES_FAT = grp.Key.Month,
                              ANO_FAT = grp.Key.Year,
                              VALOR_PAGO = (decimal)grp.Sum(x => x.PAR_VLR_PARCELA)
                          }).FirstOrDefault();

            var query2 = (from c in db.CONTRATOS
                          join p in db.PARCELAS on c.CTR_NUM_CONTRATO equals p.CTR_NUM_CONTRATO
                          where ((_tipodata) || ((c.CTR_DATA_FIM_VIGENCIA.Value >= _dtini) && (c.CTR_DATA_FIM_VIGENCIA.Value <= _dtfim)))
                             && ((p.PAR_DATA_PAGTO == null) || (p.PAR_DATA_PAGTO > _dataref))
                             && (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                        (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                        c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                        )
                                  ))
                             && ((_emp_id == 0) || (_emp_id != 0 && c.EMP_ID == _emp_id))
                          group c by new { c.EMP_ID } into grp
                          select new RelResumoCReceberDTO()
                          {
                              MES_FAT = _mes,
                              ANO_FAT = _ano,
                              VALOR_CANCELADO = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO)
                          }).FirstOrDefault();


            var query3 = (from c in db.CONTRATOS
                          join p in db.PARCELAS on c.CTR_NUM_CONTRATO equals p.CTR_NUM_CONTRATO
                          join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join k in db.CLIENTES on a.CLI_ID equals k.CLI_ID
                          where ((p.PAR_DATA_PAGTO == null) || (p.PAR_DATA_PAGTO > _dataref))
                             && (c.CTR_DATA_FIM_VIGENCIA > _dataref)
                             && (c.CTR_DATA_FAT <= _dataref)
                             && (c.CTR_DATA_CANC == null || (c.CTR_DATA_CANC != null &&
                                        (c.CTR_DATA_CANC.Value.Year > c.CTR_DATA_FAT.Value.Year ||
                                        c.CTR_DATA_CANC.Value.Month > c.CTR_DATA_FAT.Value.Month
                                        )
                                    ))
                             && ((_emp_id == 0) || (_emp_id != 0 && c.EMP_ID == _emp_id))
                          group c by new { c.EMP_ID } into grp
                          select new RelResumoCReceberDTO()
                          {
                              MES_FAT = _mes,
                              ANO_FAT = _ano,
                              VALOR_RECEBER = (decimal)grp.Sum(x => x.CTR_VLR_BRUTO)
                          }).FirstOrDefault();

            if (query != null)
                resp.VALOR_FATURADO = query.VALOR_FATURADO;

            if (query1 != null)
                resp.VALOR_PAGO = query1.VALOR_PAGO;

            if (query2 != null)
                resp.VALOR_CANCELADO = query2.VALOR_CANCELADO;

            if (query3 != null)
                resp.VALOR_RECEBER = query3.VALOR_RECEBER;

            lista.Add(resp);

            return lista;
        }
        public DateTime? RetornarDataDoUltimoFaturamentoPorEmpresa(int? empId)
        {
            var query = (from
                            con in db.CONTRATOS
                         join itm in db.ITEM_PEDIDO on con.IPE_ID equals itm.IPE_ID
                         where
                            con.EMP_ID == empId &&
                            con.CTR_DATA_CANC == null &&
                            con.CTR_NUMERO_NOTA != null &&
                            con.CTR_GERA_NOTA_FISCAL == true
                         orderby con.CTR_DATA_FAT descending
                         select con.CTR_DATA_FAT
                         ).FirstOrDefault();

            return query;
        }
        public IEnumerable<ContratoDTO> ListarContratosValidosPorAssinatura(string assinatura)
        {
            var query = (from con
                            in db.CONTRATOS
                         where
                             con.ASN_NUM_ASSINATURA == assinatura &&
                             con.CTR_DATA_CANC == null
                         select con);

            return ToDTO(query);
        }
        public RelApuracaoRecebimentoTotalCustomDTO BuscarApuraRecebimento (Nullable<DateTime> _dtini
                                                                          , Nullable<DateTime> _dtfim
                                                                          , int _emp_id = 0
                                                                          , string _ban_id = null
                                                                          , int _gru_id = 0
                                                                          , int _tipobaixa = 0)
        {

            var _Date00 = new DateTime(_dtfim.Value.Year, _dtfim.Value.Month, _dtfim.Value.Day);
            var _Date02 = new DateTime?(_Date00);
            _dtfim = _Date02.Value.AddDays(1);

            DateTime? _dataant = null;
            var _registro = new RelApuracaoRecebimentoCustomDTO();
            var _lista = new List<RelApuracaoRecebimentoCustomDTO>();
            var _total = new RelApuracaoRecebimentoTotalCustomDTO();

            _total.PAR_VLR_PREVISTO = 0;
            _total.PAR_VLR_PREV_PRIMEIRA = 0;
            _total.PAR_VLR_PAGO = 0;
            _total.PAR_VLR_PAGO_VENCIMENTO = 0;
            _total.PAR_VLR_PAGO_PRIMEIRA = 0;
            _total.PAR_VLR_BAIXA_MANUAL = 0;
            _total.PAR_VLR_BAIXA = 0;
            _total.PAR_VLR_DIFERENCA = 0;
            

            var _query = db.APURAR_RECEBIMENTO(_dtini, _dtfim, _emp_id, _ban_id, _gru_id, _tipobaixa).ToList();

            foreach (var _item in _query)
            {

                if (_dataant == null)
                {
                    _dataant = _item.PAR_DATA_PAGTO;
                    _registro = new RelApuracaoRecebimentoCustomDTO();
                    _registro.PAR_DATA_PAGTO  = _item.PAR_DATA_PAGTO;
                    _registro.PAR_VLR_PREVISTO = 0;
                    _registro.PAR_VLR_PAGO = 0;
                    _registro.PAR_VLR_PAGO_PRIMEIRA = 0;
                    _registro.PAR_VLR_PAGO_VENCIMENTO = 0;
                    _registro.PAR_VLR_BAIXA_MANUAL = 0;
                    _registro.PAR_VLR_BAIXA = 0;
                    _registro.PAR_VLR_DIFERENCA = 0;
                    _registro.PAR_VLR_PREV_PRIMEIRA = 0;


                }

                if (_dataant != _item.PAR_DATA_PAGTO)
                {
                    _lista.Add(_registro);
                    _dataant = _item.PAR_DATA_PAGTO;
                    _registro = new RelApuracaoRecebimentoCustomDTO();
                    _registro.PAR_DATA_PAGTO = _item.PAR_DATA_PAGTO;
                    _registro.PAR_VLR_PREVISTO = 0;
                    _registro.PAR_VLR_PAGO = 0;
                    _registro.PAR_VLR_PAGO_PRIMEIRA = 0;
                    _registro.PAR_VLR_PAGO_VENCIMENTO = 0;
                    _registro.PAR_VLR_BAIXA_MANUAL = 0;
                    _registro.PAR_VLR_BAIXA = 0;
                    _registro.PAR_VLR_DIFERENCA = 0;
                    _registro.PAR_VLR_PREV_PRIMEIRA = 0;

                }

                _registro.PAR_VLR_PREVISTO += (_item.PAR_VLR_PREVISTO) != null? ((decimal)_item.PAR_VLR_PREVISTO) : 0;
                _registro.PAR_VLR_PREV_PRIMEIRA += (_item.PAR_VLR_PREV_PRIMEIRA) != null ? ((decimal)_item.PAR_VLR_PREV_PRIMEIRA) : 0;
                _registro.PAR_VLR_PAGO += (_item.PAR_VLR_PAGO) != null ? ((decimal)_item.PAR_VLR_PAGO) : 0;
                _registro.PAR_VLR_PAGO_VENCIMENTO += (_item.PAR_VLR_PAGO_VENCIMENTO) != null ? ((decimal)_item.PAR_VLR_PAGO_VENCIMENTO) : 0;
                _registro.PAR_VLR_PAGO_PRIMEIRA += (_item.PAR_VLR_PAGO_PRIMEIRA) != null ? ((decimal)_item.PAR_VLR_PAGO_PRIMEIRA) : 0;
                _registro.PAR_VLR_BAIXA_MANUAL += (_item.PAR_VLR_BAIXA_MANUAL) != null ? ((decimal)_item.PAR_VLR_BAIXA_MANUAL) : 0;
                _registro.PAR_VLR_BAIXA += (_item.PAR_VLR_BAIXA) != null ? ((decimal)_item.PAR_VLR_BAIXA) : 0;
                _registro.PAR_VLR_DIFERENCA += (_item.PAR_VLR_DIFERENCA) != null ? ((decimal)_item.PAR_VLR_DIFERENCA) : 0;
                _registro.APURARCAO.Add(_item);

                //---------------------
                _total.PAR_VLR_PREVISTO += (_item.PAR_VLR_PREVISTO) != null ? ((decimal)_item.PAR_VLR_PREVISTO) : 0;
                _total.PAR_VLR_PREV_PRIMEIRA += (_item.PAR_VLR_PREV_PRIMEIRA) != null ? ((decimal)_item.PAR_VLR_PREV_PRIMEIRA) : 0;
                _total.PAR_VLR_PAGO += (_item.PAR_VLR_PAGO) != null ? ((decimal)_item.PAR_VLR_PAGO) : 0;
                _total.PAR_VLR_PAGO_VENCIMENTO += (_item.PAR_VLR_PAGO_VENCIMENTO) != null ? ((decimal)_item.PAR_VLR_PAGO_VENCIMENTO) : 0;
                _total.PAR_VLR_PAGO_PRIMEIRA += (_item.PAR_VLR_PAGO_PRIMEIRA) != null ? ((decimal)_item.PAR_VLR_PAGO_PRIMEIRA) : 0;
                _total.PAR_VLR_BAIXA_MANUAL += (_item.PAR_VLR_BAIXA_MANUAL) != null ? ((decimal)_item.PAR_VLR_BAIXA_MANUAL) : 0;
                _total.PAR_VLR_BAIXA += (_item.PAR_VLR_BAIXA) != null ? ((decimal)_item.PAR_VLR_BAIXA) : 0;
                _total.PAR_VLR_DIFERENCA += (_item.PAR_VLR_DIFERENCA) != null ? ((decimal)_item.PAR_VLR_DIFERENCA) : 0;

                //---------------------

            }

            _lista.Add(_registro);

            _total.LISTA = _lista.OrderBy(x => x.PAR_DATA_PAGTO).ToList();

            return _total;

        }
        public CONFERENCIA_FINANCEIRA_HEADER BuscarConferenciaFinanceira(DateTime _Date01, DateTime _Date02, int _emp_id, int? _grupo_id, int _tipodata = 0)
        {


            _Date02 = new DateTime(_Date02.Year, _Date02.Month, _Date02.Day);
            _Date02 = _Date02.AddDays(1);

            db.RPT_CONFERENCIA_FINANCEIRA(_Date01, _Date02, _emp_id, 0);

            var _query = (from c in db.CONFERENCIA_FINANCEIRA_TMP
                         where (
                                 (_tipodata == 2 && c.CTR_DATA_FATURAMENTO_EFETIVO >= _Date01 && c.CTR_DATA_FATURAMENTO_EFETIVO < _Date02) ||
                                 (_tipodata == 1 && c.CTR_DATA_FAT >= _Date01 && c.CTR_DATA_FAT < _Date02) ||
                                 (_tipodata == 0 && c.PLI_DATA_PAGAMENTO >= _Date01 && c.PLI_DATA_PAGAMENTO < _Date02) 
                               )
                            && (c.EMP_ID == _emp_id)
                            && (_grupo_id == 0 || (_grupo_id > 0 && c.GRUPO_ID == _grupo_id))
                          select c);


            var _totalentrada = new List<CONFERENCIA_TOTAL_ENTRADA>();
            var _totalbaixado = new List<CONFERENCIA_TOTAL_ENTRADA>();


            decimal _vlrtotalfat = 0;
            decimal _vlrtotalajustado = 0;


            foreach (var item in _query)
            {
                var _entrada = _totalentrada.FirstOrDefault(x => x.TIPO_PAGAMENTO == item.FORMA_PAGAMENTOA);
                
                decimal _vlrentrada   = (item.CTR_VLR_ENTRADA != null) ? (decimal)item.CTR_VLR_ENTRADA : 0;
                decimal _vlrentradapg = (item.VLR_ENTRADA_PAGO != null) ? (decimal)item.VLR_ENTRADA_PAGO : 0;
                decimal _vlrjuros = (_vlrentradapg - _vlrentrada) > 0 ? (_vlrentradapg - _vlrentrada) : 0;
                decimal _vlrdesconto = (_vlrentrada - _vlrentradapg) > 0 ? (_vlrentrada - _vlrentradapg) : 0;

                if (_entrada == null)
                {
                    _entrada = new CONFERENCIA_TOTAL_ENTRADA();
                    _entrada.TIPO_PAGAMENTO = item.FORMA_PAGAMENTOA;
                    _entrada.VALOR_PARCELAS = _vlrentrada;
                    _entrada.VALOR_PAGO = _vlrentradapg;
                    _entrada.VALOR_JUROS = _vlrjuros;
                    _entrada.VALOR_DESCONTO = _vlrdesconto;
                    _totalentrada.Add(_entrada);
                }
                else
                {
                    _entrada.VALOR_PARCELAS += _vlrentrada;
                    _entrada.VALOR_PAGO += _vlrentradapg;
                    _entrada.VALOR_JUROS += _vlrjuros;
                    _entrada.VALOR_DESCONTO += _vlrdesconto;
                }


                if (item.CTR_VALOR_BAIXADO > 0)
                {
                    string _formapagto = "";

                    if (item.FORMA_PAGAMENTOA != "Boleto")
                        _formapagto = item.FORMA_PAGAMENTOA;

                    if (item.FORMA_PAGAMENTOB != "Boleto" )
                        _formapagto = item.FORMA_PAGAMENTOB;
                     
                    var _baixado = _totalbaixado.FirstOrDefault(x => x.TIPO_PAGAMENTO == _formapagto);

                    decimal _vlrbaixado = (item.CTR_VALOR_BAIXADO != null) ? (decimal)item.CTR_VALOR_BAIXADO : 0;


                    if (_baixado == null)
                    {
                        _baixado = new CONFERENCIA_TOTAL_ENTRADA();
                        _baixado.TIPO_PAGAMENTO = _formapagto;
                        _baixado.VALOR_PARCELAS = _vlrbaixado;
                        _baixado.VALOR_PAGO = _vlrbaixado;
                        _baixado.VALOR_JUROS = 0;
                        _baixado.VALOR_DESCONTO = 0;
                        _totalbaixado.Add(_baixado);
                    }
                    else
                    {
                        _baixado.VALOR_PARCELAS += _vlrbaixado;
                        _baixado.VALOR_PAGO += _vlrbaixado;
                        _baixado.VALOR_JUROS += 0;
                        _baixado.VALOR_DESCONTO += 0;
                    }
                }


                _vlrtotalfat += (decimal)item.CTR_SUBTOT_CONTRATO;
                _vlrtotalajustado += (decimal)item.CTR_VLR_CONTRATO;


            }

            var _totalfaturamento = new List<CONFERENCIA_TOTAL_ENTRADA>();

            var _subtotal = new CONFERENCIA_TOTAL_ENTRADA();
            _subtotal.TIPO_PAGAMENTO = "S U B .T O T A L";
            _subtotal.VALOR_PARCELAS = _vlrtotalfat;
            _subtotal.VALOR_PAGO = _vlrtotalfat;
            _totalfaturamento.Add(_subtotal);

            var _ajuste = new CONFERENCIA_TOTAL_ENTRADA();
            _ajuste.TIPO_PAGAMENTO = "A J U S T E";
            _ajuste.VALOR_PARCELAS = (_vlrtotalajustado - _vlrtotalfat);
            _ajuste.VALOR_PAGO = (_vlrtotalajustado - _vlrtotalfat);
            _totalfaturamento.Add(_ajuste);

            var _totalajustado = new CONFERENCIA_TOTAL_ENTRADA();
            _totalajustado.TIPO_PAGAMENTO = "T O T A L ";
            _totalajustado.VALOR_PARCELAS = _vlrtotalajustado;
            _totalajustado.VALOR_PAGO = _vlrtotalajustado;
            _totalfaturamento.Add(_totalajustado);


            //--------------------------

            var _totbaixa = new CONFERENCIA_TOTAL_ENTRADA();
            _totbaixa.TIPO_PAGAMENTO = "T O T A L ";
            _totbaixa.VALOR_PARCELAS = 0;
            _totbaixa.VALOR_PAGO = 0;
            _totbaixa.VALOR_JUROS = 0;
            _totbaixa.VALOR_DESCONTO = 0;

            var _totentr = new CONFERENCIA_TOTAL_ENTRADA();
            _totentr.TIPO_PAGAMENTO = "T O T A L ";
            _totentr.VALOR_PARCELAS = 0;
            _totentr.VALOR_PAGO = 0;
            _totentr.VALOR_JUROS = 0;
            _totentr.VALOR_DESCONTO = 0;

            foreach (var item in _totalbaixado)
            {
                _totbaixa.VALOR_PARCELAS += item.VALOR_PARCELAS;
                _totbaixa.VALOR_PAGO += item.VALOR_PAGO;
                _totbaixa.VALOR_JUROS += item.VALOR_JUROS;
                _totbaixa.VALOR_DESCONTO += item.VALOR_DESCONTO;

            }

            foreach (var item in _totalentrada)
            {
                _totentr.VALOR_PARCELAS += item.VALOR_PARCELAS;
                _totentr.VALOR_PAGO += item.VALOR_PAGO;
                _totentr.VALOR_JUROS += item.VALOR_JUROS;
                _totentr.VALOR_DESCONTO += item.VALOR_DESCONTO;
            }

            _totalbaixado.Add(_totbaixa);
            _totalentrada.Add(_totentr);

            //------------------

            var _retorno = new CONFERENCIA_FINANCEIRA_HEADER();
            _retorno.RELATORIO = _query.ToList();
            _retorno.TOTAL_BAIXADO = _totalbaixado.OrderBy(x => x.TIPO_PAGAMENTO).ToList();
            _retorno.TOTAL_ENTRADA = _totalentrada.OrderBy(x => x.TIPO_PAGAMENTO).ToList();
            _retorno.TOTAL_FATURAMENTO = _totalfaturamento;

            return _retorno;

        }
        public List<RepresentanteDTO> ListarApuracaoVendas(int _mes, int _ano, int? _repid)
        {
            var query = (from a in db.REPRESENTANTE
                         select new RepresentanteDTO()
                         {
                             REP_ID = a.REP_ID,
                             REP_NOME = a.REP_NOME
                         }).ToList();

            return query;

        }
        public IList<RelPrevisaoReceitaProdutoDTO> BuscarPrevisaoReceita(int? _ano, int? _emp_id, int? _grupo_id)
        {
            var query = (from p in db.PARCELAS
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO into c1
                         from c2 in c1.DefaultIfEmpty()
                         join a in db.ASSINATURA on c2.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA into a1
                         from a2 in a1.DefaultIfEmpty()
                         join o in db.PRODUTOS on a2.PRO_ID equals o.PRO_ID into o1
                         from o2 in o1.DefaultIfEmpty()
                         where (p.DATA_EXCLUSAO == null) &&
                               ((_emp_id == null) || (_emp_id != null && p.EMP_ID == _emp_id)) &&
                               ((_grupo_id == null) || (_grupo_id != null && o2.GRUPO_ID == _grupo_id)) &&
                               (p.PAR_DATA_VENCTO.Year == _ano)
                         group p by new {  p.PAR_DATA_VENCTO.Month, p.PAR_DATA_VENCTO.Year } into grp
                         select new RelPrevisaoReceitaProdutoDTO()
                         {
                             MES_FAT = grp.Key.Month,
                             ANO_FAT = grp.Key.Year,
                             TOT_PREVISTO = grp.Sum(x => x.PAR_VLR_PARCELA),
                             TOT_REC_MES = db.PARCELAS.Where(x => x.PAR_DATA_PAGTO != null &&
                                                                  x.PAR_DATA_VENCTO.Month == grp.Key.Month &&
                                                                  x.PAR_DATA_VENCTO.Year  == grp.Key.Year &&
                                                                  x.PAR_DATA_PAGTO.Value.Month == grp.Key.Month &&
                                                                  x.PAR_DATA_PAGTO.Value.Year  == grp.Key.Year  &&
                                                                (_emp_id == null || x.CONTRATOS.EMP_ID == _emp_id) &&
                                                                (_grupo_id == null || x.CONTRATOS.ASSINATURA.PRODUTOS.GRUPO_ID == _grupo_id)                                             
                                                                 ).Sum(x => x.PAR_VLR_PAGO),
                             TOT_REC_OUTROS = db.PARCELAS.Where(x => x.PAR_DATA_PAGTO != null &&
                                                                    (x.PAR_DATA_VENCTO.Month != grp.Key.Month ||
                                                                     x.PAR_DATA_VENCTO.Year  != grp.Key.Year)   &&
                                                                  x.PAR_DATA_PAGTO.Value.Month == grp.Key.Month &&
                                                                  x.PAR_DATA_PAGTO.Value.Year == grp.Key.Year   &&
                                                                (_emp_id == null || x.CONTRATOS.EMP_ID == _emp_id) &&
                                                                (_grupo_id == null || x.CONTRATOS.ASSINATURA.PRODUTOS.GRUPO_ID == _grupo_id)
                                                                 ).Sum(x => x.PAR_VLR_PAGO),
                             TOT_REC = 0,
                         }).OrderByDescending(x => x.ANO_FAT).ThenByDescending(x => x.MES_FAT).ToList();

            

            var _totais = new RelPrevisaoReceitaProdutoDTO();
            _totais.MES_FAT = 0;
            _totais.ANO_FAT = 0;
            _totais.TOT_REC = 0;
            _totais.TOT_REC_MES = 0;
            _totais.TOT_REC_OUTROS = 0;
            _totais.TOT_PREVISTO = 0;

            foreach (var _item in query )
            {
                _item.TOT_REC = _item.TOT_REC_OUTROS + _item.TOT_REC_MES;
                _item.PERC_REALIZADO = ((_item.TOT_REC * 100) / _item.TOT_PREVISTO) != null ? ((_item.TOT_REC * 100) / _item.TOT_PREVISTO) : 0;
                _item.PERC_REALIZADO = Math.Round((decimal)_item.PERC_REALIZADO, 2);
                _totais.TOT_REC_MES += _item.TOT_REC_MES;
                _totais.TOT_REC_OUTROS += _item.TOT_REC_OUTROS;
                _totais.TOT_REC += (_item.TOT_REC != null) ? (decimal)_item.TOT_REC : 0;
                _totais.TOT_PREVISTO += (_item.TOT_PREVISTO != null) ? (decimal)_item.TOT_PREVISTO : 0;
            }

            query.Add(_totais);

            return query;
        }

        /// <summary>
        /// ///
        /// </summary>
        /// <param name="lstPedCrmId"></param>
        /// <returns></returns>
        public IList<ContratoDTO> ListarContratosQGeraNota(IList<int?> lstPedCrmId, int? nctId)
        {
            var query = (from
                                ped in db.PEDIDO_CRM join 
                                itm in db.ITEM_PEDIDO on ped.PED_CRM_ID equals itm.PED_CRM_ID join
                                con in db.CONTRATOS on itm.IPE_ID equals con.IPE_ID
                            where
                               lstPedCrmId.Contains(itm.PED_CRM_ID) &&
                               (
                                itm.PST_ID == 3 || 
                                itm.PST_ID == 11 || 
                                itm.PST_ID == 12 ||
                                itm.PST_ID == 13 ||
                                itm.PST_ID == 14 
                               ) &&
                               (con.CTR_GERA_NOTA_FISCAL == true ||
                               con.CTR_GERA_NOTA_FISCAL == null) &&
                               (con.CTR_CORTESIA == null || con.CTR_CORTESIA == 0) &&
                               (from nfc in db.NOTA_FISCAL_CONFIG
                                where 
                                    nfc.NCT_ID == nctId &&
                                    nfc.DATA_EXCLUSAO == null
                                && 
                                    (
                                        ped.PED_CEM_POR_CENTO_FATURADO == null ||
                                        ped.PED_CEM_POR_CENTO_FATURADO == false ||
                                        (
                                            ped.PED_CEM_POR_CENTO_FATURADO == true && 
                                            nfc.NFC_APLICAR_100_POR_CENTO_FAT == true
                                        )
                                    )
                                select nfc.CMP_ID).Contains(con.CMP_ID)
                            select 
                                con
                         );

            return ToDTO(query);
        }
        
    }
}

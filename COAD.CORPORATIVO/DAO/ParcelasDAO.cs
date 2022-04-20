using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Extensions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Util;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Util;

namespace COAD.CORPORATIVO.DAO
{
    public class ParcelasDAO : AbstractGenericDao<PARCELAS, ParcelasDTO, string>
    {

        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ParcelasDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public IList<ParcelasDTO> buscarPorNossoNumero(string nn)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.PAR_NOSSO_NUMERO == nn);
            return ToDTO(query);
        }


        public IList<ParcelasDTO> buscarParcelaPorParte(string titulo)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.PAR_NUM_PARCELA.Contains(titulo));
            return ToDTO(query);
        }
        public void EfetuarTransmissao(int _rem_id, int _cta_id)
        {
            db.REGISTRAR_TRANSMISSAO(_rem_id, _cta_id);
        }
        public IList<ParcelasDTO> CarregarItensAvulsos(string _ASN_NUM_ASSINATURA, string _CLI_NOME)
        {
            var _query =  (from p in db.PARCELAS
                           join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                           join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                           join d in db.CLIENTES on a.CLI_ID equals d.CLI_ID
                          where (_CLI_NOME == null            || (_CLI_NOME != null           && d.CLI_NOME.Contains(_CLI_NOME)))
                             && (_ASN_NUM_ASSINATURA == null  || (_ASN_NUM_ASSINATURA != null && a.ASN_NUM_ASSINATURA == _ASN_NUM_ASSINATURA))
                             && p.DATA_EXCLUSAO == null
                             && p.PAR_DATA_PAGTO == null
                          select new ParcelasDTO
                          {
                              ASN_NUM_ASSINATURA = a.ASN_NUM_ASSINATURA,
                              CLI_NOME = d.CLI_NOME,
                              PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                              PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                              PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                              PAR_DATA_ALOC = p.PAR_DATA_ALOC
                          }).OrderByDescending(x => x.PAR_DATA_VENCTO).ToList();

             var _que01 = (from p in db.PARCELAS
                           join ppi in db.PROPOSTA_ITEM on p.PPI_ID equals ppi.PPI_ID
                           join pp in db.PROPOSTA on ppi.PRT_ID equals pp.PRT_ID
                           join d in db.CLIENTES on pp.CLI_ID equals d.CLI_ID
                          where d.CLI_NOME.Contains(_CLI_NOME)
                             && p.DATA_EXCLUSAO == null
                             && p.PAR_DATA_PAGTO == null
                          select new ParcelasDTO
                          {
                              ASN_NUM_ASSINATURA = null,
                              CLI_NOME = d.CLI_NOME,
                              PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                              PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                              PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                              PAR_DATA_ALOC = p.PAR_DATA_ALOC
                          }).OrderByDescending(x => x.PAR_DATA_VENCTO).ToList();

            var _que03 = (from p in db.PARCELAS
                          join i in db.ITEM_PEDIDO on p.IPE_ID equals i.IPE_ID
                          join d in db.PEDIDO_CRM  on i.PED_CRM_ID equals d.PED_CRM_ID
                          join c in db.CLIENTES on d.CLI_ID equals c.CLI_ID
                         where c.CLI_NOME.Contains(_CLI_NOME)
                            && p.DATA_EXCLUSAO == null
                            && p.PAR_DATA_PAGTO == null
                          select new ParcelasDTO
                          {
                              ASN_NUM_ASSINATURA = null,
                              CLI_NOME = c.CLI_NOME,
                              PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                              PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                              PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                              PAR_DATA_ALOC = p.PAR_DATA_ALOC
                          }).OrderByDescending(x => x.PAR_DATA_VENCTO).ToList();

            var _queryRet = _query.Union(_que01).Union(_que03).ToList();
            
            return _queryRet;
        }

        public IList<ParcelasDTO> listarParcelasAlocacaoAvulsa(ContaDTO _conta)
        {
            var query = (from p in db.PARCELAS
                         where p.PAR_ALOC_AUTOMATICA == true
                            && p.DATA_EXCLUSAO == null
                            && p.PAR_DATA_ALOC == null
                            && p.CTA_ID != null
                            && p.EMP_ID == _conta.CTA_ALOCAR_TITULO_DA_EMP_ID
                         orderby p.EMP_ID
                               , p.CTA_ID
                         select p);

            return ToDTO(query);
        }

        public IList<ParcelasDTO> listarParcelasAvulsas(int _REM_ID)
        {
            var query = (from p in db.PARCELAS
                         where p.PAR_ALOC_AUTOMATICA == true
                            && p.DATA_EXCLUSAO == null
                            && p.REM_ID == _REM_ID
                            && p.CTA_ID != null
                         orderby p.EMP_ID, p.CTA_ID
                         select p);

            return ToDTO(query);
        }

        public IList<ParcelasAtrasoCustomDTO> ListarDebitoDetalhadamente(string _assinatura, int _cliente)
        {
            var _query = (from par in db.PARCELAS
                          join con in db.CONTRATOS on par.CTR_NUM_CONTRATO equals con.CTR_NUM_CONTRATO
                          join ass in db.ASSINATURA on con.ASN_NUM_ASSINATURA equals ass.ASN_NUM_ASSINATURA
                          join cli in db.CLIENTES on ass.CLI_ID equals cli.CLI_ID
                          where cli.CLI_ID == _cliente
                              && ass.ASN_NUM_ASSINATURA == _assinatura
                              && par.PAR_DATA_PAGTO == null
                              && SqlFunctions.DateDiff("day", par.PAR_DATA_VENCTO, DateTime.Now) >= 7
                          select new ParcelasAtrasoCustomDTO
                          {
                              PAR_NUM_PARCELA = par.PAR_NUM_PARCELA,
                              PAR_DATA_VENCTO = par.PAR_DATA_VENCTO,
                              DIAS_ATRASO = GetDiasEmAtraso(par.PAR_DATA_VENCTO),
                              PAR_VLR_PARCELA = par.PAR_VLR_PARCELA,
                              CLI_NOME = cli.CLI_NOME
                          }).OrderByDescending(x => x.PAR_DATA_VENCTO).ToList();

            return _query;

        }

        private int GetDiasEmAtraso(DateTime dtVencimento)
        {
            TimeSpan diffResult = DateTime.Now.ToUniversalTime().Subtract(dtVencimento.ToUniversalTime());

            return diffResult.Days - 7;

        }

        public List<ParcelasAtrasoCustomDTO> ListarTitulosVencidos(int _cli_id)
        {
            // tratar parcela prorrogana na query
            var sitProrrogado = "PRO";

            var _query = (from p in db.PARCELAS
                          join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                          join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join d in db.CLIENTES on a.CLI_ID equals d.CLI_ID
                          where (d.CLI_ID == _cli_id)
                             //&& ((c.CTR_DATA_INI_VIGENCIA <= DateTime.Now
                             //&& ((c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                             //   (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1))) || c.CTR_CORTESIA == 1)
                             && c.CTR_DATA_CANC == null
                             && p.DATA_EXCLUSAO == null
                             && p.PAR_DATA_PAGTO == null
                             && SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now) >= 7
                             && SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now) <= 90

                          select p).OrderByDescending(x => x.PAR_DATA_VENCTO).ToList();

            var _lista = new List<ParcelasAtrasoCustomDTO>();
            string _ctr_ant = null;

            var _listaParcelas = ToDTO(_query);
            var _item = new ParcelasAtrasoCustomDTO();

            foreach (var i in _listaParcelas)
            {
                if (_ctr_ant != i.CTR_NUM_CONTRATO)
                {
                    if (_ctr_ant != null)
                    {
                        _lista.Add(_item);
                    }

                    _item = new ParcelasAtrasoCustomDTO();

                    var _ctr = db.CONTRATOS.Where(x => x.CTR_NUM_CONTRATO == i.CTR_NUM_CONTRATO).FirstOrDefault();
                    if (_ctr != null)
                        _item.ASN_NUM_ASSINATURA = _ctr.ASN_NUM_ASSINATURA;

                    _item.CTR_NUM_CONTRATO = i.CTR_NUM_CONTRATO;
                    _item.VLR_TOTAL_DEBITO = _query.Where(x => x.CTR_NUM_CONTRATO == i.CTR_NUM_CONTRATO).Sum(x => x.PAR_VLR_PARCELA);

                    _ctr_ant = i.CTR_NUM_CONTRATO;
                }


                i.PAR_DIAS_ATRASO = (DateTime.Now.Subtract(i.PAR_DATA_VENCTO)).Days;

                _item.PARCELAS.Add(i);

            }

            _lista.Add(_item);

            return _lista;

        }

        public IList<ParcelasAtrasoCustomDTO> BuscarDebitoDetalhadamente(string assinatura, int _cli_id)
        {
            var _query = (from p in db.PARCELAS
                          join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                          join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          join d in db.CLIENTES on a.CLI_ID equals d.CLI_ID
                          where (d.CLI_ID == _cli_id)
                             && ((c.CTR_DATA_INI_VIGENCIA <= DateTime.Now
                             && ((c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1))) || c.CTR_CORTESIA == 1)
                             && c.CTR_DATA_CANC == null
                             && p.DATA_EXCLUSAO == null
                             && p.PAR_DATA_PAGTO == null
                             && SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now) >= 7
                          select p).OrderByDescending(x => x.PAR_DATA_VENCTO).ToList();

            var _lista = new List<ParcelasAtrasoCustomDTO>();
            string _ctr_ant = null;

            var _listaParcelas = ToDTO(_query);
            var _item = new ParcelasAtrasoCustomDTO();

            foreach (var i in _listaParcelas)
            {
                if (_ctr_ant != i.CTR_NUM_CONTRATO)
                {
                    if (_ctr_ant != null)
                    {
                        _lista.Add(_item);
                    }

                    _item = new ParcelasAtrasoCustomDTO();

                    var _ctr = db.CONTRATOS.Where(x => x.CTR_NUM_CONTRATO == i.CTR_NUM_CONTRATO).FirstOrDefault();
                    if (_ctr != null)
                        _item.ASN_NUM_ASSINATURA = _ctr.ASN_NUM_ASSINATURA;

                    _item.CTR_NUM_CONTRATO = i.CTR_NUM_CONTRATO;
                    _item.VLR_TOTAL_DEBITO = _query.Where(x => x.CTR_NUM_CONTRATO == i.CTR_NUM_CONTRATO).Sum(x => x.PAR_VLR_PARCELA);

                    _ctr_ant = i.CTR_NUM_CONTRATO;
                }


                i.PAR_DIAS_ATRASO = (DateTime.Now.Subtract(i.PAR_DATA_VENCTO)).Days;

                _item.PARCELAS.Add(i);

            }

            _lista.Add(_item);

            return _lista;

        }

        public IList<ParcelasNegociacaoCustomDTO> ListarNegociacaoAtraso(Nullable<decimal> _valor, int _qtdeParcelas)
        {

            var _lista = new List<ParcelasNegociacaoCustomDTO>();

            for (var i = 1; i <= _qtdeParcelas; i++)
            {
                var _item = new ParcelasNegociacaoCustomDTO();
                _item.QTDE_PARCELAS = i;
                _item.VALOR_TOTAL = _valor;
                _item.VALOR_PARCELAS = Math.Round((decimal)(_valor / i), 2);

                _lista.Add(_item);
            }

            return _lista;

        }
        public List<ParcelasDTO> ListarTitulosParaAlocacaoDet(int _emp_id)
        {

            var query = (from p in db.PARCELAS
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join t in db.CLIENTES on a.CLI_ID equals t.CLI_ID
                         join r in db.PRODUTOS on a.PRO_ID equals r.PRO_ID
                         where p.PAR_DATA_PAGTO == null
                             && p.PAR_DATA_ALOC == null
                             && p.DATA_EXCLUSAO == null
                             && (
                                  ((c.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                  ((c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                   (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1))) || c.CTR_CORTESIA == 1) ||
                                   (c.CTR_DATA_INI_VIGENCIA > DateTime.Now)
                                )
                             && (EntityFunctions.DiffDays(DateTime.Now, p.PAR_DATA_VENCTO) >= 15)
                             && (p.PAR_VLR_PARCELA >= 15)
                             && (p.EMP_ID == _emp_id)
                             && (p.PAR_PODE_ALOCAR)
                             && (p.PAR_ALOC_AUTOMATICA == null || p.PAR_ALOC_AUTOMATICA != true)
                             && (p.REM_ID == null)
                             && (t.CLI_CPF_CNPJ != "" && t.CLI_CPF_CNPJ != null)
                         select new ParcelasDTO
                         {
                             EMP_ID = p.EMP_ID,
                             ASN_NUM_ASSINATURA = a.ASN_NUM_ASSINATURA,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             CLI_NOME = t.CLI_NOME,
                             PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                             PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                             PAR_DIAS_VENCIMENTO = EntityFunctions.DiffDays(DateTime.Now, p.PAR_DATA_VENCTO)

                         }).OrderByDescending(x => x.EMP_ID).ThenBy(x => x.PAR_DATA_VENCTO).ToList();


            return query;

        }
        public Pagina<ParcelasDTO> ListarTitulosParaAlocacaoDet(int _emp_id, int pagina = 1, int numpaginas = 12)
        {

            var query = (from p in db.PARCELAS
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join t in db.CLIENTES on a.CLI_ID equals t.CLI_ID
                         join r in db.PRODUTOS on a.PRO_ID equals r.PRO_ID
                        where p.PAR_DATA_PAGTO == null
                            && p.PAR_DATA_ALOC == null
                            && p.DATA_EXCLUSAO == null
                            && (
                                 ((c.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                 ((c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                  (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1))) || c.CTR_CORTESIA == 1) ||
                                  (c.CTR_DATA_INI_VIGENCIA > DateTime.Now)
                               )
                            && (EntityFunctions.DiffDays(DateTime.Now, p.PAR_DATA_VENCTO) >= 15)
                            && (p.PAR_VLR_PARCELA >= 15)
                            && (p.EMP_ID == _emp_id)
                            && (p.PAR_PODE_ALOCAR)
                            && (p.PAR_ALOC_AUTOMATICA == null || p.PAR_ALOC_AUTOMATICA != true)
                            && (p.REM_ID == null)
                            && (t.CLI_CPF_CNPJ != "" && t.CLI_CPF_CNPJ != null)
                         select new ParcelasDTO
                         {
                             EMP_ID = p.EMP_ID,
                             ASN_NUM_ASSINATURA = a.ASN_NUM_ASSINATURA,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             CLI_NOME = t.CLI_NOME,
                             PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                             PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                             PAR_DIAS_VENCIMENTO = EntityFunctions.DiffDays(DateTime.Now, p.PAR_DATA_VENCTO)

                         }).OrderByDescending(x => x.EMP_ID).ThenBy(x => x.PAR_DATA_VENCTO).ToList();


            return query.Paginar<ParcelasDTO>(pagina, numpaginas);
            
        }
        public IList<BoletosAlocadosDTO> ListarTitulosParaAlocacao(int _emp_id)
        {

            if ( _emp_id > -1 )
            {

                var query = (from p in db.PARCELAS
                             join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                             join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                             join t in db.CLIENTES on a.CLI_ID equals t.CLI_ID
                             join r in db.PRODUTOS on a.PRO_ID equals r.PRO_ID
                             where p.PAR_DATA_PAGTO == null
                                 && p.PAR_DATA_ALOC == null
                                 && p.DATA_EXCLUSAO == null
                                 && (
                                      ((c.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                      ((c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                       (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1))) || c.CTR_CORTESIA == 1) ||
                                       (c.CTR_DATA_INI_VIGENCIA > DateTime.Now)
                                    )
                                 && (EntityFunctions.DiffDays(DateTime.Now, p.PAR_DATA_VENCTO) >= 15)
                                 && (p.PAR_VLR_PARCELA >= 15)
                                 && (p.EMP_ID == _emp_id)
                                 && (p.PAR_PODE_ALOCAR)
                                 && (p.PAR_ALOC_AUTOMATICA == null || p.PAR_ALOC_AUTOMATICA != true)
                                 && (p.REM_ID == null)
                                 && (t.CLI_CPF_CNPJ != "" && t.CLI_CPF_CNPJ != null)
                             group p by new
                             {
                                 p.EMP_ID

                             }
                             into f
                             orderby f.Key.EMP_ID
                             select new BoletosAlocadosDTO
                             {
                                 EMP_ID = f.Key.EMP_ID,
                                 QTDE_TITULOS = f.Count(),
                                 TOTAL_DISPONIVEL = (decimal)f.Sum(p => p.PAR_VLR_PARCELA),
                                 TOTAL_NECESSARIO = (decimal)f.Where(p => EntityFunctions.DiffDays(DateTime.Now, p.PAR_DATA_VENCTO) <= 20).Sum(p => p.PAR_VLR_PARCELA)
                             }).OrderByDescending(x => x.EMP_ID).ToList();

                return query;

            }
            else
            {

                var query = (from p in db.PARCELAS
                             join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                             join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                             join t in db.CLIENTES on a.CLI_ID equals t.CLI_ID
                             join r in db.PRODUTOS on a.PRO_ID equals r.PRO_ID
                             where p.PAR_DATA_PAGTO == null
                                 && p.PAR_DATA_ALOC == null
                                 && p.DATA_EXCLUSAO == null
                                 && (
                                      ((c.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                      ((c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                       (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1))) || c.CTR_CORTESIA == 1) ||
                                       (c.CTR_DATA_INI_VIGENCIA > DateTime.Now)
                                    )
                                 && (EntityFunctions.DiffDays(DateTime.Now, p.PAR_DATA_VENCTO) >= 15)
                                 && (p.PAR_VLR_PARCELA >= 15)
                                 && (p.PAR_PODE_ALOCAR)
                                 && (p.PAR_ALOC_AUTOMATICA == null || p.PAR_ALOC_AUTOMATICA != true)
                                 && (p.REM_ID == null)
                                 && (t.CLI_CPF_CNPJ != "" && t.CLI_CPF_CNPJ != null)
                             group p by new
                             {
                                 p.EMP_ID

                             }
                             into f
                             orderby f.Key.EMP_ID
                             select new BoletosAlocadosDTO
                             {
                                 EMP_ID = f.Key.EMP_ID,
                                 QTDE_TITULOS = f.Count(),
                                 TOTAL_DISPONIVEL = (decimal)f.Sum(p => p.PAR_VLR_PARCELA),
                                 TOTAL_NECESSARIO = (decimal)f.Where(p => EntityFunctions.DiffDays(DateTime.Now, p.PAR_DATA_VENCTO) <= 20).Sum(p => p.PAR_VLR_PARCELA)
                             }).OrderByDescending(x => x.EMP_ID).ToList();

                return query;

            }

        }

        public IList<BoletosAlocadosDTO> ListarTitulosAlocados(DateTime _dtini, DateTime _dtfim, string _banid = null)
        {
            if (_banid == "")
                _banid = null;

            var query = (from p in db.PARCELAS
                         join d in db.PARCELA_ALOCADA on p.PAR_NUM_PARCELA equals d.PAR_NUM_PARCELA
                         where (EntityFunctions.TruncateTime(p.PAR_DATA_ALOC) >= EntityFunctions.TruncateTime(_dtini)
                            && EntityFunctions.TruncateTime(p.PAR_DATA_ALOC) <= EntityFunctions.TruncateTime(_dtfim))
                            //&& d.ALO_DATA_DESALOCACAO == null
                            && (_banid == null || (_banid  != null && _banid == p.BAN_ID))

                         group p by new
                         {
                             p.BAN_ID,
                             p.BANCOS.BAN_NOME

                         } into f
                         orderby f.Key.BAN_ID
                         select new BoletosAlocadosDTO
                         {
                             BAN_ID = f.Key.BAN_ID,
                             BAN_DESCRICAO = f.Key.BAN_NOME,
                             QTDE_TITULOS = f.Count(),
                             TOTAL_ALOCADO = (decimal)f.Sum(p => p.PAR_VLR_PARCELA),
                             TOTAL_VENCIDO = (decimal)f.Where(p => p.PAR_DATA_PAGTO == null && p.PAR_DATA_VENCTO < DateTime.Now).Sum(p => p.PAR_VLR_PARCELA),
                             TOTAL_ORIGINAL = (decimal)f.Where(p => p.PAR_DATA_PAGTO != null).Sum(p => p.PAR_VLR_PARCELA),
                             TOTAL_JUROS = (decimal)f.Where(p => p.PAR_DATA_PAGTO != null).Sum(p => p.PAR_VLR_PAGO - p.PAR_VLR_PARCELA),
                             TOTAL_PAGO = (decimal)f.Where(p => p.PAR_DATA_PAGTO != null).Sum(p =>  p.PAR_VLR_PAGO),
                             TOTAL_EMABERTO = (decimal)f.Where(p => p.PAR_DATA_PAGTO == null && p.PAR_DATA_VENCTO >= DateTime.Now).Sum(p => p.PAR_VLR_PARCELA),

                         }).OrderByDescending(x => x.TOTAL_ALOCADO).ToList();

            var _total = new BoletosAlocadosDTO();

            _total.BAN_DESCRICAO  = "T O T A L";
            _total.QTDE_TITULOS   = 0;
            _total.TOTAL_ALOCADO  = 0;
            _total.TOTAL_VENCIDO  = 0;
            _total.TOTAL_ORIGINAL = 0;
            _total.TOTAL_JUROS    = 0;
            _total.TOTAL_PAGO     = 0;
            _total.TOTAL_EMABERTO = 0;

            foreach (var _item in query)
            {
                _total.QTDE_TITULOS   += _item.QTDE_TITULOS;
                _total.TOTAL_ALOCADO  += (_item.TOTAL_ALOCADO  != null) ? _item.TOTAL_ALOCADO  : 0;
                _total.TOTAL_VENCIDO  += (_item.TOTAL_VENCIDO  != null) ? _item.TOTAL_VENCIDO  : 0;
                _total.TOTAL_ORIGINAL += (_item.TOTAL_ORIGINAL != null) ? _item.TOTAL_ORIGINAL : 0;
                _total.TOTAL_JUROS    += (_item.TOTAL_JUROS    != null) ? _item.TOTAL_JUROS    : 0;
                _total.TOTAL_PAGO     += (_item.TOTAL_PAGO     != null) ? _item.TOTAL_PAGO     : 0;
                _total.TOTAL_EMABERTO += (_item.TOTAL_EMABERTO != null) ? _item.TOTAL_EMABERTO : 0;
            }

            query.Add(_total);

            return query;

        }

        public Pagina<ParcelasRetornoCustomDTO> BuscarparcelasRetorno(int? _cnq_id, string _parNumParcela, int pagina = 1, int numpaginas = 12)
        {

            var query = (from n in db.CNAB_ARQUIVOS
                         join i in db.CNAB_ARQUIVOS_ITEM on n.CNQ_ID equals i.CNQ_ID
                         join p in db.PARCELAS on i.PAR_NUM_PARCELA equals p.PAR_NUM_PARCELA
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         where (_cnq_id == null || _cnq_id == n.CNQ_ID) &&
                               (_parNumParcela.Trim() == null || _parNumParcela == p.PAR_NUM_PARCELA)
                         select new ParcelasRetornoCustomDTO
                         {
                             BAN_ID = p.BAN_ID,
                             CTA_ID = p.CTA_ID,
                             CLI_NOME = l.CLI_NOME,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             CTR_NUM_CONTRATO = p.CTR_NUM_CONTRATO,
                             PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                             PAR_VLR_PAGO = p.PAR_VLR_PAGO,
                             PAR_DATA_PAGTO = p.PAR_DATA_PAGTO,
                             PAR_NOSSO_NUMERO = p.PAR_NOSSO_NUMERO,
                             CNQ_ID = p.CNQ_ID,
                             REM_ID = p.REM_ID,
                             PAR_BAIXA_MANUAL = p.PAR_BAIXA_MANUAL,
                             CNI_DATA_PAGTO = i.CNI_DATA_PAGTO,
                             OCT_CODIGO = i.OCT_CODIGO,
                             CNI_ACAO = i.CNI_ACAO,
                             CNI_VLR_JUROS = i.CNI_VLR_JUROS,
                             CNI_VLR_PAGO = i.CNI_VLR_PAGO,
                         });





            return query.Paginar<ParcelasRetornoCustomDTO>(pagina, numpaginas);
        }
        public Pagina<ParcelasRetornoCustomDTO> BuscarparcelasRetorno(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome, int pagina = 1, int numpaginas = 12)
        {

            var query = (from n in db.CNAB_ARQUIVOS
                         join i in db.CNAB_ARQUIVOS_ITEM on n.CNQ_ID equals i.CNQ_ID
                         join p in db.PARCELAS on i.PAR_NUM_PARCELA equals p.PAR_NUM_PARCELA
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         where (EntityFunctions.TruncateTime(n.CNQ_DATA_ARQUIVO) >= EntityFunctions.TruncateTime(_data_ini) &&
                                 EntityFunctions.TruncateTime(n.CNQ_DATA_ARQUIVO) <= EntityFunctions.TruncateTime(_data_fim)) &&
                                (_ban_id == null || (_ban_id != null && _ban_id == p.BAN_ID)) &&
                                (_nome == null || (_nome != null && _nome == n.CNQ_NOME))
                         select new ParcelasRetornoCustomDTO
                         {
                             BAN_ID = p.BAN_ID,
                             CTA_ID = p.CTA_ID,
                             CLI_NOME = l.CLI_NOME,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             CTR_NUM_CONTRATO = p.CTR_NUM_CONTRATO,
                             PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                             PAR_VLR_PAGO = p.PAR_VLR_PAGO,
                             PAR_DATA_PAGTO = p.PAR_DATA_PAGTO,
                             PAR_NOSSO_NUMERO = p.PAR_NOSSO_NUMERO,
                             CNQ_ID = p.CNQ_ID,
                             REM_ID = p.REM_ID,
                             PAR_BAIXA_MANUAL = p.PAR_BAIXA_MANUAL,
                             CNI_DATA_PAGTO = i.CNI_DATA_PAGTO,
                             OCT_CODIGO = i.OCT_CODIGO,
                             CNI_ACAO = i.CNI_ACAO,
                             CNI_VLR_JUROS = i.CNI_VLR_JUROS,
                             CNI_VLR_PAGO = i.CNI_VLR_PAGO,
                         });



            return query.Paginar<ParcelasRetornoCustomDTO>(pagina, numpaginas);
        }


        // registros para remessa bancária...
        public Pagina<AReceberRelatorioDTO> BuscarAreceber(AReceberPesquisaDTO _param, int pagina = 1, int numpaginas = 20)
        {
            var query = (from p in db.PARCELAS
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         join q in db.PARCELA_LIQUIDACAO on p.PAR_NUM_PARCELA equals q.PAR_NUM_PARCELA into j1
                         from j2 in j1.DefaultIfEmpty()
                         where p.PAR_PODE_ALOCAR == true &&
                               p.EMP_ID == _param.emp_id &&
                               (_param.tipo == 2 || (_param.tipo == 0 && p.PAR_DATA_PAGTO == null) ||
                                                     (_param.tipo == 1 && p.PAR_DATA_PAGTO != null)) &&
                               (_param.banco == 0 || (_param.banco == 1 && p.PAR_DATA_ALOC == null) ||
                                                     (_param.banco == 2 && p.PAR_DATA_ALOC != null)) &&
                               (((c.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                 ((c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                  (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1))) ||
                                   c.CTR_CORTESIA == 1) || (c.CTR_DATA_INI_VIGENCIA > DateTime.Now)) && 
                                   c.CTR_DATA_CANC == null &&
                                   p.DATA_EXCLUSAO == null

                         select new AReceberRelatorioDTO {
                             GRUPO = "",
                             EMP_ID = c.EMP_ID,
                             EMP_RAZAO_SOCIAL = "",
                             BAN_NOME = j2.BAN_ID,
                             FORMA_PGTO = j2.PLI_TIPO_DOC,
                             CTR_DATA_FAT = c.CTR_DATA_FAT,
                             CLI_CPF_CNPJ = l.CLI_CPF_CNPJ,
                             CTR_NUM_CONTRATO = c.CTR_NUM_CONTRATO,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                             PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                             PAR_DATA_PAGTO = p.PAR_DATA_PAGTO,
                             PAR_VLR_PAGO = p.PAR_VLR_PAGO
                         });


            return query.Paginar<AReceberRelatorioDTO>(pagina, numpaginas);
        }

        // ALT: 07/08/2017 - titulos da assinatura
        public IList<ParcelasBoletoDTO> obterTitulosDaAssinatura(string idAssinatura)
        {
            var _contas = new ContaSRV().FindAll();

            var query = (from p in db.PARCELAS
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         where (
                                (c.ASN_NUM_ASSINATURA == idAssinatura &&
                                (c.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                (c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now ||
                                (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1) || c.CTR_CORTESIA == 1 || c.CTR_DATA_INI_VIGENCIA > DateTime.Now) && c.CTR_DATA_CANC == null && p.DATA_EXCLUSAO == null))

                                || (c.ASN_NUM_ASSINATURA == idAssinatura && c.CTR_DATA_FIM_VIGENCIA > DateTime.Now && c.CTR_DATA_CANC == null)
                               )
                            && (EntityFunctions.DiffDays(DateTime.Now, p.PAR_DATA_VENCTO) <= 20)
                         orderby c.CTR_ANO_VIGENCIA descending, p.PAR_DATA_VENCTO
                         select new ParcelasBoletoDTO
                         {
                             PAR_NOSSO_NUMERO = p.PAR_NOSSO_NUMERO,
                             ASN_NUM_ASSINATURA = c.ASN_NUM_ASSINATURA,
                             CTR_NUM_CONTRATO = c.CTR_NUM_CONTRATO,
                             CTR_DATA_FAT = c.CTR_DATA_FAT,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             CLI_NOME = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_NOME,
                             PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                             PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                             PAR_DATA_PAGTO = p.PAR_DATA_PAGTO,
                             DIAS_ATRASO = SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now),
                             ASN_A_C = c.ASSINATURA.ASN_A_C,
                             BAN_ID = p.BAN_ID,
                             CLI_ID = c.ASSINATURA.CLI_ID,
                             CTA_ID = p.CTA_ID,
                             CTA_ENVIA_BOLETO = null
                         }).OrderByDescending(x => x.PAR_DATA_VENCTO).ToList();

            foreach(var ite in query)
            {
                var xconta = _contas.Where(x => x.CTA_ID == ite.CTA_ID).FirstOrDefault();
                if (xconta != null)
                    ite.CTA_ENVIA_BOLETO = ((bool)(xconta.CTA_ENVIA_BOLETO)&&(!String.IsNullOrWhiteSpace(ite.PAR_NOSSO_NUMERO)));
            }

            return query;
        }

        public IList<ParcelasDTO> SelecionarProcedureRegistrosDeAlocacao(string titulo, int empresa = 2, DateTime? vencI = null, DateTime? vencF = null, Decimal vlrI = 0, Decimal vlrF = 0, string ban_id = null)
        {
            var query = db.ALOCAR_REGISTROS(titulo, empresa, vencI, vencF, vlrI, vlrF, ban_id);
            var lista = (from a in query
                         select new ParcelasDTO
                         {
                             CLI_NOME = a.CLI_NOME,
                             ASN_NUM_ASSINATURA = a.ASN_NUM_ASSINATURA,
                             PAR_NUM_PARCELA = a.PAR_NUM_PARCELA,
                             CTR_NUM_CONTRATO = a.CTR_NUM_CONTRATO,
                             EMP_ID = a.EMP_ID,
                             PAR_DATA_VENCTO = a.PAR_DATA_VENCTO,
                             PAR_VLR_BOLETO = a.PAR_VLR_BOLETO,
                             PAR_VENC_BOLETO = a.PAR_VENC_BOLETO,
                             CNQ_ID = a.CNQ_ID,
                             PAR_VLR_PARCELA = a.PAR_VLR_PARCELA,
                             PAR_MORA_MES = a.PAR_MORA_MES,
                             PAR_CART_ALOC = a.PAR_CART_ALOC,
                             PAR_DATA_ALOC = a.PAR_DATA_ALOC,
                             PAR_DATA_REIMPRESSAO = a.PAR_DATA_REIMPRESSAO,
                             PAR_VLR_PAGO = a.PAR_VLR_PAGO,
                             PAR_DATA_PAGTO = a.PAR_DATA_PAGTO,
                             PAR_NOSSO_NUMERO = a.PAR_NOSSO_NUMERO,
                             PAR_CEDENTE = a.PAR_CEDENTE,
                             PAR_SITUACAO = a.PAR_SITUACAO,
                             BAN_ID = a.BAN_ID,
                             DATA_ALTERA = a.DATA_ALTERA,
                             USU_LOGIN = a.USU_LOGIN,
                             USU_LOGIN_PRORROGACAO = a.USU_LOGIN_PRORROGACAO,
                             DATA_PRORROGACAO = a.DATA_PRORROGACAO,
                             PAR_CHAVE_TRANSACAO = a.PAR_CHAVE_TRANSACAO,
                             PAR_URL_BOLETO = a.PAR_URL_BOLETO,
                             PAR_CODIGO_DE_BARRAS = a.PAR_CODIGO_DE_BARRAS,
                             PAR_STATUS_TRANSACAO = a.PAR_STATUS_TRANSACAO,
                             PAR_SEQ_PARCELA = a.PAR_SEQ_PARCELA,
                             TPG_ID = a.TPG_ID,
                             CTA_ID = a.CTA_ID,
                             PAR_REMESSA = a.PAR_REMESSA,
                             PGT_ID = a.PGT_ID,
                             PAR_TRANSMITIDO = a.PAR_TRANSMITIDO,
                             IPE_ID = a.IPE_ID,
                             PAR_PARCELA_DO_PEDIDO = a.PAR_PARCELA_DO_PEDIDO,
                             PAR_COD_LEGADO = a.PAR_COD_LEGADO,
                             ORDER_KEY = a.ORDER_KEY,
                             ORDER_KEY_REF = a.ORDER_KEY_REF,
                             AUTHORIZATION_CODE = a.AUTHORIZATION_CODE,
                             PAR_PODE_ALOCAR = a.PAR_PODE_ALOCAR,
                             REM_ID = a.REM_ID,
                             PAR_TIPO_ALOC = a.PAR_TIPO_ALOC,
                             PPI_ID = a.PPI_ID,
                             PAR_ALOC_AUTOMATICA = a.PAR_ALOC_AUTOMATICA,
                             DATA_EXCLUSAO = a.DATA_EXCLUSAO,
                             PAR_BAIXA_MANUAL = a.PAR_BAIXA_MANUAL
                         }).ToList();

            return lista;
        }
        public IList<ParcelasDTO> Selecionar(int empresa = 2, DateTime? vencI = null, DateTime? vencF = null, Decimal vlrI = 0, Decimal vlrF = 0)
        {
            IQueryable<PARCELAS> query = GetDbSet();

            DateTime? dataini = null;
            DateTime? datafim = null;

            if (vencI != null)
            {
                dataini = new DateTime(vencI.Value.Year, vencI.Value.Month, vencI.Value.Day);
                datafim = new DateTime(vencF.Value.Year, vencF.Value.Month, vencF.Value.Day);
            }

            // apenas contrato ativo e parcelas a mais de 15 dias do vencimento \\
            query = (from p in db.PARCELAS
                     join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                     where p.PAR_PODE_ALOCAR == true &&
                           p.EMP_ID == empresa &&
                           //p.PAR_TRANSMITIDO == null &&
                           //p.PAR_REMESSA == null &&
                           p.PAR_DATA_PAGTO == null &&
                           p.PAR_DATA_ALOC == null &&
                           (
                           ((c.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                             ((c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                              (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1))) ||
                                c.CTR_CORTESIA == 1) || (c.CTR_DATA_INI_VIGENCIA > DateTime.Now)
                               ) && c.CTR_DATA_CANC == null &&

                           (SqlFunctions.DateDiff("day", DateTime.Now, p.PAR_DATA_VENCTO) >= 15) &&
                           ((vencI != null && vencF != null) && (p.PAR_DATA_VENCTO >= dataini && p.PAR_DATA_VENCTO <= datafim)) &&
                           ((vlrI > 0 && vlrF > 0) && (p.PAR_VLR_PARCELA >= vlrI && p.PAR_VLR_PARCELA <= vlrF))

                     select p);

            return ToDTO(query);
        }


        public IList<ParcelasDTO> LerParcelaAlocada(int _rem_id)
        {
            var query = (from p in db.PARCELAS
                             //join r in db.PARCELAS_REMESSA on p.REM_ID equals r.REM_ID
                         where (p.REM_ID == _rem_id) && (p.PAR_DATA_ALOC != null)
                         select p);

            return ToDTO(query);
        }

        public IQueryable<ParcelasConciliacaoRemDTO> BuscarParcelasRemessa(int _rem_id)
        {
            IQueryable<ParcelasConciliacaoRemDTO> query = (from p in db.PARCELAS
                                                           join a in db.PARCELA_ALOCADA on p.PAR_NUM_PARCELA equals a.PAR_NUM_PARCELA
                                                           join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                                                           join l in db.PARCELA_LIQUIDACAO on p.PAR_NUM_PARCELA equals l.PAR_NUM_PARCELA into j1
                                                           from j2 in j1.DefaultIfEmpty()
                                                           where (a.REM_ID == _rem_id)
                                                           select new ParcelasConciliacaoRemDTO
                                                           {
                                                               REM_ID = p.REM_ID,
                                                               ASN_NUM_ASSINATURA = p.CONTRATOS.ASN_NUM_ASSINATURA,
                                                               CLI_NOME = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_NOME,
                                                               PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                                                               PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                                                               PAR_VLR_PARCELA = (decimal)p.PAR_VLR_PARCELA,
                                                               BAN_ID = p.BAN_ID,
                                                               AUTHORIZATION_CODE = p.AUTHORIZATION_CODE
                                                           });

            return query;
        }
        public Pagina<ParcelasConciliacaoRemDTO> BuscarParcelasRemessa(int _rem_id, int pagina, int numpaginas)
        {
            IQueryable<ParcelasConciliacaoRemDTO> query = (from p in db.PARCELAS
                                                           join a in db.PARCELA_ALOCADA on p.PAR_NUM_PARCELA equals a.PAR_NUM_PARCELA
                                                           join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO into leftC
                                                           from c in leftC.DefaultIfEmpty()
                                                           where (a.REM_ID == _rem_id)
                                                           select new ParcelasConciliacaoRemDTO
                                                           {
                                                               REM_ID = p.REM_ID,
                                                               ASN_NUM_ASSINATURA = p.CONTRATOS.ASN_NUM_ASSINATURA,
                                                               CLI_NOME = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_NOME,
                                                               PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                                                               PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                                                               PAR_DATA_PAGTO = p.PAR_DATA_PAGTO,
                                                               PAR_VLR_PARCELA = (decimal)p.PAR_VLR_PARCELA,
                                                               PAR_VLR_PAGO = (decimal)p.PAR_VLR_PAGO,
                                                               BAN_ID = p.BAN_ID,
                                                               AUTHORIZATION_CODE = p.AUTHORIZATION_CODE
                                                           });

            //.OrderBy(x => x.PLI_DATA).ThenBy(x => x.CLI_NOME)

            return query.Paginar<ParcelasConciliacaoRemDTO>(pagina, numpaginas);
        }

        // registros rejeitados para restauração...
        public IList<ParcelasDTO> RestaurarRejeitados(int remessa)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.PAR_TRANSMITIDO == "R" && x.REM_ID == remessa);
            return ToDTO(query);
        }

        // registros de títulos disponíveis...
        public IList<ParcelasDTO> LerTitulosDisponiveis(int empresa = 2)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.EMP_ID == empresa && x.PAR_TRANSMITIDO == null && x.PAR_REMESSA == null && (x.PAR_DATA_PAGTO == null) &&
                (SqlFunctions.DateDiff("day", DateTime.Now, x.PAR_DATA_VENCTO) >= 15));
            return ToDTO(query);
        }

        // registros de títulos transmitidos ou remetido ao banco...
        public IList<ParcelasDTO> LerTitulosTransmitidos(int remessa)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.PAR_TRANSMITIDO == "S" && x.REM_ID == remessa);
            return ToDTO(query);
        }

        // registros de títulos rejeitados pelo banco...
        public IList<ParcelasDTO> LerTitulosRejeitados(int remessa)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.PAR_TRANSMITIDO == "R" && x.REM_ID == remessa);
            return ToDTO(query);
        }

        // registros de títulos alocados no banco...
        public IList<ParcelasDTO> LerTitulosAlocados(int remessa)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.PAR_TRANSMITIDO == "A" && x.REM_ID == remessa);
            return ToDTO(query);
        }

        // registros de títulos vencidos a receber...
        public IList<ParcelasDTO> LerTitulosVencidosReceber(int empresa = 2, DateTime? de = null, DateTime? ate = null)
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.EMP_ID == empresa && x.PAR_TRANSMITIDO == "A" && x.PAR_DATA_PAGTO == null && x.PAR_DATA_VENCTO < dt && (x.PAR_DATA_VENCTO >= de && x.PAR_DATA_VENCTO <= ate));
            return ToDTO(query);
        }

        // registros de títulos vincendos a receber...
        public IList<ParcelasDTO> LerTitulosVincendosReceber(int empresa = 2, DateTime? de = null, DateTime? ate = null)
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.EMP_ID == empresa && x.PAR_TRANSMITIDO == "A" && x.PAR_DATA_PAGTO == null && x.PAR_DATA_VENCTO > dt && (x.PAR_DATA_VENCTO >= de && x.PAR_DATA_VENCTO <= ate));
            return ToDTO(query);
        }

        // registros de títulos recebidos...
        public IList<ParcelasDTO> LerTitulosRecebidos(int empresa = 2, DateTime? de = null, DateTime? ate = null)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.EMP_ID == empresa && x.PAR_TRANSMITIDO == "A" && x.PAR_DATA_PAGTO != null && (x.PAR_DATA_PAGTO >= de && x.PAR_DATA_PAGTO <= ate));
            return ToDTO(query);
        }

        // registros de títulos expirando...
        public IList<ParcelasDTO> LerTitulosExpirando(int empresa = 2, DateTime? dataBase = null)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            dataBase = dataBase == null ? DateTime.Now : dataBase;
            query = query.Where(x => x.EMP_ID == empresa && x.PAR_REMESSA == null && (x.PAR_DATA_PAGTO == null) &&
                (SqlFunctions.DateDiff("day", x.PAR_DATA_VENCTO, (DateTime)dataBase) >= 15 &&
                (SqlFunctions.DateDiff("day", x.PAR_DATA_VENCTO, (DateTime)dataBase) <= 30)));
            return ToDTO(query);
        }

        // registros de títulos expirados...
        public IList<ParcelasDTO> LerTitulosExpirados(int empresa = 2, DateTime? dataBase = null)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            dataBase = dataBase == null ? DateTime.Now : dataBase;
            query = query.Where(x => x.EMP_ID == empresa && x.PAR_REMESSA == null && (x.PAR_DATA_PAGTO == null) &&
                (SqlFunctions.DateDiff("day", x.PAR_DATA_VENCTO, (DateTime)dataBase) < 15));
            return ToDTO(query);
        }

        // registros de títulos para gerar boletos...
        public List<ParcelasDTO> LerTitulosGerarBoletos(int remessa)
        {
            IQueryable<PARCELAS> query = GetDbSet();
            query = query.Where(x => x.REM_ID == remessa && (x.PAR_TRANSMITIDO.Contains("S") || x.PAR_TRANSMITIDO.Contains("A")));
            return ToDTO(query).ToList();
        }

        // filtro comum...
        public IList<ParcelasDTO> LerTitulos(int? empresa = 2, string banco = null, int? rem_id = null, string transmitido = null)
        {
            IQueryable<PARCELAS> query = GetDbSet();

            if (empresa != null)
            {
                query = query.Where(x => x.EMP_ID == empresa);
            }
            if (!String.IsNullOrWhiteSpace(banco))
            {
                query = query.Where(x => x.BAN_ID == banco);
            }
            if (rem_id != null)
            {
                query = query.Where(x => x.REM_ID == rem_id);
            }
            if (!String.IsNullOrWhiteSpace(transmitido))
            {
                query = query.Where(x => x.PAR_TRANSMITIDO == transmitido);
            }

            query = query.OrderBy(x => x.EMP_ID).ThenBy(y => y.BAN_ID);

            return ToDTO(query);
        }

        public IList<ParcelasDTO> BuscarPorContrato(string _numcontrato)
        {
            var listaparcelas = (from x in db.PARCELAS
                                 where x.CTR_NUM_CONTRATO == _numcontrato select x).OrderByDescending(x => x.PAR_DATA_VENCTO);

            var _empsrv = new EmpresaSRV();

            var _contas = new ContaSRV().FindAll();


            //----------------------------

            var listapartcelasDTO = ToDTO(listaparcelas);

            foreach (var item in listapartcelasDTO)
            {
                var _empresa = _empsrv.FindById(item.EMP_ID);

                if (item.PAR_DATA_PAGTO == null)
                {
                    item.PAR_DIAS_ATRASO = 0;
                    item.PAR_VLR_JUROS = 0;
                    item.PAR_MORA_DIA = 0;
                    item.PAR_VLR_BOLETO = 0;

                    var _dias = (DateTime.Now - item.PAR_DATA_VENCTO).TotalDays;

                    if (_dias > 0)
                    {

                        item.PAR_MORA_DIA = (item.PAR_MORA_MES / 30 / 100);
                        item.PAR_VLR_JUROS = (item.PAR_VLR_PARCELA * (item.PAR_MORA_DIA * (decimal)_dias));
                        item.PAR_VLR_JUROS = Math.Round((decimal)item.PAR_VLR_JUROS, 2);
                        item.PAR_DIAS_ATRASO = (int)_dias;
                        item.PAR_VLR_DESP_ADM = (_empresa.EMP_DESP_ADM_BOLETO == null) ? 0 : _empresa.EMP_DESP_ADM_BOLETO;
                        item.PAR_VLR_BOLETO = item.PAR_VLR_PARCELA + item.PAR_VLR_JUROS + item.PAR_VLR_DESP_ADM;
                        item.PAR_VLR_BOLETO = Math.Round((decimal)item.PAR_VLR_BOLETO, 2);

                    }

                }

                var xconta = _contas.Where(x => x.CTA_ID == item.CTA_ID).FirstOrDefault();

                if (xconta != null)
                    item.CTA_ENVIA_BOLETO = (xconta.CTA_ENVIA_BOLETO == true) && (!String.IsNullOrWhiteSpace(item.PAR_NOSSO_NUMERO));
            }

            return listapartcelasDTO;

        }
        public ParcelasDTO BuscarParcela(string _par_num_parcela, bool _baixamanual = false )
        {
            var _parcela = (from x in db.PARCELAS
                            where x.PAR_NUM_PARCELA == _par_num_parcela
                           select x).FirstOrDefault();

            if (_parcela == null)
                throw new Exception("Parcela não encontrada.");

            var _remessa = (from r in db.PARCELAS_REMESSA
                            where r.REM_ID == _parcela.REM_ID
                            select r).FirstOrDefault();

            /*
            if (!_baixamanual)
                if (_remessa != null && _remessa.TRE_ID == 3)
                    throw new Exception("Para esta parcela não é permitida a emissão do boleto avulso. (LIRA)");
            */

            var _item = new ParcelasDTO();
            var _empsrv = new EmpresaSRV();

            if (_parcela != null)
            {
                _item = ToDTO(_parcela);

                var _empresa = _empsrv.FindById(_item.EMP_ID);

                if (_item.PAR_DATA_PAGTO == null)
                {
                    _item.PAR_DIAS_ATRASO = 0;
                    _item.PAR_VLR_JUROS = 0;
                    _item.PAR_MORA_DIA = 0;
                    _item.PAR_VLR_BOLETO = 0;

                    var _dias = (DateTime.Now - _item.PAR_DATA_VENCTO).TotalDays;

                    if (_dias > 0)
                    {

                        _item.PAR_MORA_DIA = (_item.PAR_MORA_MES / 30 / 100);
                        _item.PAR_VLR_JUROS = (_item.PAR_VLR_PARCELA * (_item.PAR_MORA_DIA * (decimal)_dias));
                        _item.PAR_VLR_JUROS = Math.Round((decimal)_item.PAR_VLR_JUROS, 2);
                        _item.PAR_DIAS_ATRASO = (int)_dias;
                        _item.PAR_VLR_DESP_ADM = _empresa.EMP_DESP_ADM_BOLETO;
                        _item.PAR_VLR_BOLETO = _item.PAR_VLR_PARCELA + _item.PAR_VLR_JUROS + _item.PAR_VLR_DESP_ADM;
                        _item.PAR_VLR_BOLETO = Math.Round((decimal)_item.PAR_VLR_BOLETO, 2);

                    }
                    else
                    {
                        _item.PAR_MORA_DIA = 0;
                        _item.PAR_DIAS_ATRASO = 0;
                        _item.PAR_VLR_DESP_ADM = 0;
                        _item.PAR_VLR_BOLETO = _item.PAR_VLR_PARCELA;

                    }

                }
            }


            var _dados = this.BuscarTitulosAtrasoCobranca(_parcela.PAR_NUM_PARCELA);

            if (_dados != null)
            {
                _item.CLI_NOME = _dados.CLI_NOME;
                _item.ASN_NUM_ASSINATURA = _dados.ASN_NUM_ASSINATURA;
            }

            return _item;

        }
        public ParcelasDTO BuscarParcelaNossoNumero(string _par_nosso_numero)
        {
            var _parcela = (from x in db.PARCELAS
                            where x.PAR_NOSSO_NUMERO == _par_nosso_numero
                            select x).FirstOrDefault();

            if (_parcela == null)
                throw new Exception("Parcela não encontrada.");

            var _remessa = (from r in db.PARCELAS_REMESSA
                            where r.REM_ID == _parcela.REM_ID
                            select r).FirstOrDefault();

            /*
            if (_remessa != null && _remessa.TRE_ID == 3)
                throw new Exception("Para esta parcela não é permitida a emissão do boleto avulso. (LIRA)");
            */

            var _empsrv = new EmpresaSRV();

            var _item = ToDTO(_parcela);

            var _empresa = _empsrv.FindById(_item.EMP_ID);

            if (_item.PAR_DATA_PAGTO == null)
            {
                _item.PAR_DIAS_ATRASO = 0;
                _item.PAR_VLR_JUROS = 0;
                _item.PAR_MORA_DIA = 0;
                _item.PAR_VLR_BOLETO = 0;

                var _dias = (DateTime.Now - _item.PAR_DATA_VENCTO).TotalDays;

                if (_dias > 0)
                {

                    _item.PAR_MORA_DIA = (_item.PAR_MORA_MES / 30 / 100);
                    _item.PAR_VLR_JUROS = (_item.PAR_VLR_PARCELA * (_item.PAR_MORA_DIA * (decimal)_dias));
                    _item.PAR_VLR_JUROS = Math.Round((decimal)_item.PAR_VLR_JUROS, 2);
                    _item.PAR_DIAS_ATRASO = (int)_dias;
                    _item.PAR_VLR_DESP_ADM = _empresa.EMP_DESP_ADM_BOLETO;
                    _item.PAR_VLR_BOLETO = _item.PAR_VLR_PARCELA + _item.PAR_VLR_JUROS + _item.PAR_VLR_DESP_ADM;
                    _item.PAR_VLR_BOLETO = Math.Round((decimal)_item.PAR_VLR_BOLETO, 2);

                }
                else
                {
                    _item.PAR_MORA_DIA = 0;
                    _item.PAR_DIAS_ATRASO = 0;
                    _item.PAR_VLR_DESP_ADM = 0;
                    _item.PAR_VLR_BOLETO = _item.PAR_VLR_PARCELA;

                }

            }

            var _dados = this.BuscarTitulosAtrasoCobranca(_parcela.PAR_NUM_PARCELA);

            if (_dados != null)
            {
                _item.CLI_NOME = _dados.CLI_NOME;
                _item.ASN_NUM_ASSINATURA = _dados.ASN_NUM_ASSINATURA;
            }

            return _item;

        }
        public void ProcessarRetorno(int _cnq_id)
        {
            db.BAIXA_CNAB(_cnq_id, SessionContext.autenticado.USU_LOGIN);
        }
        public ParcelasConciliacaoRemTotalDTO BuscarAuditoriaRetorno(   int? _cnq_id
                                                                      , int? _rem_id
                                                                      , string _parNumParcela
                                                                      , string _parNossoNumero
                                                                      , string _ban_id
                                                                      , string _oct_codigo
                                                                      , int _ipe_id = 0
                                                                      , int _ppi_id = 0
                                                                      , string _cnqnome = null 
                                                                      , int _pagina = 1
                                                                      , int _registroPorPagina = 10)
        {
            
            var query = (from p in db.PARCELAS 
                         join r in db.PARCELA_ALOCADA on p.PAR_NUM_PARCELA equals r.PAR_NUM_PARCELA into leftr
                         from r in leftr.DefaultIfEmpty()
                         join b in db.BANCOS on r.BAN_ID equals b.BAN_ID 
                         join i in db.CNAB_ARQUIVOS_ITEM on new { r.PAR_NUM_PARCELA, r.BAN_ID } equals new { i.PAR_NUM_PARCELA, i.BAN_ID } into lefti
                         from i in lefti.DefaultIfEmpty()
                         join c in db.CNAB_ARQUIVOS on i.CNQ_ID equals c.CNQ_ID into leftc
                         from c in leftc.DefaultIfEmpty()
                         join o in db.OCORRENCIA_RETORNO on new { i.OCT_CODIGO, i.BAN_ID } equals new { o.OCT_CODIGO, o.BAN_ID } into lefto
                         from o in lefto.DefaultIfEmpty()
                         where ((_rem_id == null) || (_rem_id == p.REM_ID)) &&
                               ((_ban_id == null || _ban_id == "") || (_ban_id == p.BAN_ID)) &&
                               ((_oct_codigo == null || _oct_codigo == "") || (_oct_codigo == i.OCT_CODIGO)) &&
                               ((_parNumParcela == null) || (_parNumParcela == p.PAR_NUM_PARCELA)) &&
                               ((_parNossoNumero == null) || (_parNossoNumero == p.PAR_NOSSO_NUMERO)) &&
                               ((_ipe_id == 0) || (_ipe_id == p.IPE_ID)) &&
                               ((_ppi_id == 0) || (_ppi_id == p.PPI_ID)) 
                         select new ParcelasConciliacaoRemDTO
                         {
                             CNQ_ID = c.CNQ_ID,
                             REM_ID = p.REM_ID,
                             IPE_ID = p.IPE_ID,
                             PPI_ID = p.PPI_ID,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             PAR_NOSSO_NUMERO = p.PAR_NOSSO_NUMERO,
                             CNQ_DATA_LIDO = c.CNQ_DATA_LIDO,
                             CNQ_DATA_PROCESSADO = c.CNQ_DATA_PROCESSADO,
                             PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                             PAR_VLR_PAGO = p.PAR_VLR_PAGO,
                             PAR_DATA_PAGTO = p.PAR_DATA_PAGTO,
                             PAR_DATA_ALOC = r.ALO_DATA_ALOCACAO,
                             OCT_CODIGO = i.OCT_CODIGO,
                             OCT_DESCRICAO = o.OCT_DESCRICAO,
                             OCT_BAIXAR_TITULO = o.OCT_BAIXAR_TITULO,
                             OCT_DESALOCAR_TITULO = o.OCT_DESALOCAR_TITULO,
                             BAN_ID = r.BAN_ID,
                             BAN_NOME = b.BAN_NOME,
                             DATA_EXCLUSAO = p.DATA_EXCLUSAO
                         });


            if (_cnq_id != 0 && _cnq_id != null)
                query = query.Where(x => x.CNQ_ID == _cnq_id);

            var queryRetorno = query;

            if ((_oct_codigo     == null || _oct_codigo     == "")  &&
                (_rem_id         == null || _rem_id         ==  0)  &&
                (_parNossoNumero == null || _parNossoNumero == "")  &&
                (_cnqnome        == null || _cnqnome        == "")  &&
                (_ipe_id == 0)  && 
                (_ppi_id == 0)
               ) 
            {
                var query1 = (from i in db.CNAB_ARQUIVOS_ITEM_ERRO
                              join c in db.CNAB_ARQUIVOS on i.CNQ_ID equals c.CNQ_ID into leftc
                              from c in leftc.DefaultIfEmpty()
                              where ((_ban_id == null || _ban_id == "") || (_ban_id == c.BAN_ID)) &&
                                    ((_parNumParcela == null || _parNumParcela == "") || (_parNumParcela == i.CNE_NUM_PARCELA))
                              select new ParcelasConciliacaoRemDTO
                              {
                                  CNQ_ID = c.CNQ_ID,
                                  REM_ID = null,
                                  IPE_ID = null,
                                  PPI_ID = null,
                                  PAR_NUM_PARCELA = i.CNE_NUM_PARCELA,
                                  PAR_NOSSO_NUMERO = "",
                                  CNQ_DATA_LIDO = c.CNQ_DATA_LIDO,
                                  CNQ_DATA_PROCESSADO = c.CNQ_DATA_PROCESSADO,
                                  PAR_VLR_PARCELA = null,
                                  PAR_VLR_PAGO = null,
                                  PAR_DATA_PAGTO = null,
                                  PAR_DATA_ALOC = null,
                                  OCT_CODIGO = "",
                                  OCT_DESCRICAO = i.CNE_ERRO,
                                  OCT_BAIXAR_TITULO = false,
                                  OCT_DESALOCAR_TITULO = false,
                                  BAN_ID = c.BAN_ID,
                                  BAN_NOME = null,
                                  DATA_EXCLUSAO = null
                              });

                if (_cnq_id != 0 && _cnq_id != null)
                    query1 = query1.Where(x => x.CNQ_ID == _cnq_id);

                if (query1 != null)
                    queryRetorno = query1;

                if (query != null && query1 != null)
                    queryRetorno = query.Union(query1);
            }
            
            var _conciliacao = new ParcelasConciliacaoRemTotalDTO();


            _conciliacao.PAR_VLR_TOTAL = queryRetorno.Sum(x => x.PAR_VLR_PARCELA);
            _conciliacao.PAR_VLR_PAGO = queryRetorno.Sum(x => x.PAR_VLR_PAGO);

            queryRetorno = queryRetorno.OrderBy(x => x.PAR_DATA_ALOC);

            _conciliacao.PAGINA_CONCILIACAO = queryRetorno.Paginar<ParcelasConciliacaoRemDTO>(_pagina, _registroPorPagina);

            return _conciliacao;

        }
        public ParcelasAtrasoCobrancaDTO BuscarTitulosAtrasoCobranca(string _par_num_parcela)
        {

            var query = (from c in db.CONTRATOS
                         join p in db.PARCELAS on c.CTR_NUM_CONTRATO equals p.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join s in db.ASSINATURA_SENHA on a.ASN_NUM_ASSINATURA equals s.ASN_NUM_ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         where (p.PAR_NUM_PARCELA == _par_num_parcela)
                         select new ParcelasAtrasoCobrancaDTO
                         {
                             ASN_NUM_ASSINATURA = c.ASN_NUM_ASSINATURA,
                             CTR_NUM_CONTRATO = c.CTR_NUM_CONTRATO,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             CLI_NOME = l.CLI_NOME,
                             PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                             PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                             DIAS_ATRASO = SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now),
                             ASN_A_C = a.ASN_A_C,
                             BAN_ID = p.BAN_ID,
                             CLI_ID = a.CLI_ID

                         }).FirstOrDefault();


            return query;

        }
        public Pagina<ParcelasAtrasoCobrancaDTO> BuscarTitulosAtrasoCobranca(string assinatura
                                                                           , string cnpj
                                                                           , DateTime? dataini = null
                                                                           , DateTime? datafim = null
                                                                           , int atrasoini = 7
                                                                           , int atrasofim = 90
                                                                           , bool todos = false
                                                                           , int pagina = 1, int registroPorPagina = 20)
        {
            DateTime? _dtfim = null;

            if (datafim !=null)
            {
                _dtfim = new DateTime(datafim.Value.Year, datafim.Value.Month, datafim.Value.Day);
                _dtfim = _dtfim.Value.AddDays(1);
            }

            var query = (from c in db.CONTRATOS
                         join p in db.PARCELAS on c.CTR_NUM_CONTRATO equals p.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         //join s in db.ASSINATURA_SENHA on a.ASN_NUM_ASSINATURA equals s.ASN_NUM_ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         where ((c.CTR_DATA_INI_VIGENCIA <= DateTime.Now
                            && ((c.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                (c.CTR_DATA_FIM_VIGENCIA < DateTime.Now && c.CTR_PRORROGADO == 1))) || c.CTR_CORTESIA == 1)
                            && c.CTR_DATA_CANC == null
                            && p.DATA_EXCLUSAO == null
                            && p.PAR_DATA_PAGTO == null
                            && SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now) >= atrasoini
                            && SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now) < atrasofim
                            && (todos || (todos == false && db.AGENDA_COBRANCA.Where(x => x.ASN_NUM_ASSINATURA == a.ASN_NUM_ASSINATURA && x.AGC_DATA_ATENDIMENTO == null && x.STATUS == false).Count() == 0))
                            && (p.PAR_DATA_VENCTO >= dataini && p.PAR_DATA_VENCTO < _dtfim)
                            && ((assinatura == null || assinatura == "") || (assinatura == a.ASN_NUM_ASSINATURA))
                            && ((cnpj == null || cnpj == "") || (cnpj == l.CLI_CPF_CNPJ))
                            && (db.PARCELAS.Where(x => x.CTR_NUM_CONTRATO == c.CTR_NUM_CONTRATO
                                                  && x.DATA_EXCLUSAO == null
                                                  && x.PAR_DATA_PAGTO == null
                                                  && SqlFunctions.DateDiff("day", x.PAR_DATA_VENCTO, DateTime.Now) >= atrasoini).Count() == 1)
                         select new ParcelasAtrasoCobrancaDTO
                         {
                             ASN_NUM_ASSINATURA = c.ASN_NUM_ASSINATURA,
                             CTR_NUM_CONTRATO = c.CTR_NUM_CONTRATO,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             CLI_NOME = l.CLI_NOME,
                             PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                             PAR_VLR_PARCELA = p.PAR_VLR_PARCELA,
                             DIAS_ATRASO = SqlFunctions.DateDiff("day", p.PAR_DATA_VENCTO, DateTime.Now),
                             ASN_A_C = a.ASN_A_C,
                             BAN_ID = p.BAN_ID,
                             CLI_ID = a.CLI_ID

                         }).OrderBy(x => x.DIAS_ATRASO).ThenBy(x => x.CLI_NOME);


            return query.Paginar<ParcelasAtrasoCobrancaDTO>(pagina, registroPorPagina);

        }
        public IList<ParcelasConciliacaoDTO> BuscarConciliacaoTitulos(DateTime _dtini, DateTime _dtfim, int _emp_id, string _ban_id = null, string _parcela = null)
        {
            if (_ban_id == "")
                _ban_id = null;

            if (_parcela == "")
                _parcela = null;

            _dtfim = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _dtfim = _dtfim.AddDays(1);


            var query = (from p in db.PARCELAS
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         join l in db.CLIENTES on a.CLI_ID equals l.CLI_ID
                         where ( p.PAR_NUM_PARCELA == _parcela) || 
                               (
                                   (p.PAR_DATA_PAGTO >= _dtini && p.PAR_DATA_PAGTO < _dtfim) && 
                                   (c.EMP_ID == _emp_id) && 
                                   (_ban_id == null || (_ban_id != null && p.BAN_ID == _ban_id))
                               )

                         select new ParcelasConciliacaoDTO
                         {
                             ASN_NUM_ASSINATURA = a.ASN_NUM_ASSINATURA,
                             CLI_NOME = l.CLI_NOME,
                             PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                             PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                             PAR_VLR_PARCELA = (decimal)p.PAR_VLR_PARCELA,
                             PLI_DATA = (DateTime)p.PAR_DATA_PAGTO,
                             PLI_VALOR = (decimal)p.PAR_VLR_PAGO,
                             BAN_ID = p.BAN_ID,
                             AUTHORIZATION_CODE = p.AUTHORIZATION_CODE,
                             PLI_TIPO_DOC = null
                         }).OrderBy(x => x.PLI_DATA).ThenBy(x => x.CLI_NOME);




            return query.ToList();

        }
        public Pagina<ParcelasConciliacaoDTO> BuscarConciliacaoTitulos(DateTime _dtini, DateTime _dtfim, int _emp_id, string _ban_id = null, string _parcela = null, int _tipodata = 0, int? _tipoBaixa = null,  int pagina = 1, int registroPorPagina = 20)
        {
            if (_ban_id == "")
                _ban_id = null;

            if (_parcela == "")
                _parcela = null;

            _dtfim = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _dtfim = _dtfim.AddDays(1);

            var query3 = new List<ParcelasConciliacaoDTO>();

            if (_parcela != null)
            {
                var query = (from p in db.PARCELAS
                             join l in db.PARCELA_LIQUIDACAO on p.PAR_NUM_PARCELA equals l.PAR_NUM_PARCELA
                             join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                             join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                             join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                             from j2 in j1.DefaultIfEmpty()
                             where (p.PAR_NUM_PARCELA == _parcela)
                             select new ParcelasConciliacaoDTO
                             {
                                 CLI_ID = a.CLI_ID,
                                 ASN_NUM_ASSINATURA = p.CONTRATOS.ASN_NUM_ASSINATURA,
                                 CLI_NOME = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_NOME,
                                 CLI_CPF_CNPJ = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_CPF_CNPJ,
                                 PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                                 PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                                 PAR_VLR_PARCELA = (decimal)p.PAR_VLR_PARCELA,
                                 PLI_DATA = l.PLI_DATA,
                                 PLI_DATA_BAIXA = l.PLI_DATA_BAIXA,
                                 PLI_VALOR = (decimal)p.PAR_VLR_PAGO,
                                 BAN_ID = p.BAN_ID,
                                 AUTHORIZATION_CODE = p.AUTHORIZATION_CODE,
                                 ORDER_KEY = p.ORDER_KEY,
                                 PLI_TIPO_DOC = l.PLI_TIPO_DOC,
                                 REP_ID = c.REP_ID,
                                 REP_NOME = j2.REP_NOME,
                                 REGIAO_UF = c.REGIAO_UF
                             });

                query3 = query.ToList();
            }
            else
            {

                var query2 = (from p in db.PARCELAS
                              join l in db.PARCELA_LIQUIDACAO on p.PAR_NUM_PARCELA equals l.PAR_NUM_PARCELA
                              join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                              join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                              join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                              from j2 in j1.DefaultIfEmpty()
                              where (
                                        (
                                          (_tipodata == 0 && (l.PLI_DATA >= _dtini && l.PLI_DATA < _dtfim)) ||
                                          (_tipodata == 1 && (l.PLI_DATA_BAIXA >= _dtini && l.PLI_DATA_BAIXA < _dtfim))
                                         )

                                     && (c.EMP_ID == _emp_id)
                                     && (_ban_id == null || (_ban_id != null && l.BAN_ID == _ban_id))
                                     && (_tipoBaixa == null || (
                                        (_tipoBaixa == 0 && p.PAR_BAIXA_MANUAL == true) ||
                                        (_tipoBaixa == 1 && (p.PAR_BAIXA_MANUAL == false || p.PAR_BAIXA_MANUAL == null))))
                                     )

                              select new ParcelasConciliacaoDTO
                              {
                                  CLI_ID = a.CLI_ID,
                                  ASN_NUM_ASSINATURA = p.CONTRATOS.ASN_NUM_ASSINATURA,
                                  CLI_NOME = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_NOME,
                                  CLI_CPF_CNPJ = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_CPF_CNPJ,
                                  PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                                  PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                                  PAR_VLR_PARCELA = (decimal)p.PAR_VLR_PARCELA,
                                  PLI_DATA = l.PLI_DATA,
                                  PLI_DATA_BAIXA = l.PLI_DATA_BAIXA,
                                  PLI_VALOR = (decimal)p.PAR_VLR_PAGO,
                                  BAN_ID = p.BAN_ID,
                                  AUTHORIZATION_CODE = p.AUTHORIZATION_CODE,
                                  ORDER_KEY = p.ORDER_KEY,
                                  PLI_TIPO_DOC = l.PLI_TIPO_DOC,
                                  REP_ID = c.REP_ID,
                                  REP_NOME = j2.REP_NOME,
                                  REGIAO_UF = c.REGIAO_UF
                              });

                query3 = query2.OrderByDescending(x => x.PLI_DATA).ThenBy(x => x.PLI_DATA_BAIXA).ToList();

            }

            var _soma = new ParcelasConciliacaoDTO();
            _soma.ASN_NUM_ASSINATURA = "T O T A L";
            _soma.PAR_VLR_PARCELA = 0;
            _soma.PLI_VALOR = 0;

            foreach (var item in query3) {

                if (item.PAR_VLR_PARCELA != null)
                    _soma.PAR_VLR_PARCELA += (decimal)item.PAR_VLR_PARCELA;

                if (item.PLI_VALOR != null)
                    _soma.PLI_VALOR += (decimal)item.PLI_VALOR;

            }

            query3.Add(_soma);


            return query3.Paginar(pagina, registroPorPagina);

        }
        public IList<ParcelasConciliacaoDTO> BuscarConciliacao(DateTime _dtini, DateTime _dtfim, int _emp_id, string _ban_id = null, string _parcela = null, int _tipodata = 0, int? _tipoBaixa = null)
        {
            if (_ban_id == "")
                _ban_id = null;

            if (_parcela == "")
                _parcela = null;

            _dtfim = new DateTime(_dtfim.Year, _dtfim.Month, _dtfim.Day);
            _dtfim = _dtfim.AddDays(1);

            var query3 = new List<ParcelasConciliacaoDTO>();

            if (_parcela != null)
            {
                var query = (from p in db.PARCELAS
                             join l in db.PARCELA_LIQUIDACAO on p.PAR_NUM_PARCELA equals l.PAR_NUM_PARCELA
                             join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                             join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                             join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                             from j2 in j1.DefaultIfEmpty()
                             where (p.PAR_NUM_PARCELA == _parcela)
                             select new ParcelasConciliacaoDTO
                             {
                                 CLI_ID = a.CLI_ID,
                                 ASN_NUM_ASSINATURA = p.CONTRATOS.ASN_NUM_ASSINATURA,
                                 CLI_NOME = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_NOME,
                                 CLI_CPF_CNPJ = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_CPF_CNPJ,
                                 PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                                 PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                                 PAR_VLR_PARCELA = (decimal)p.PAR_VLR_PARCELA,
                                 PLI_DATA = l.PLI_DATA,
                                 PLI_DATA_BAIXA = l.PLI_DATA_BAIXA,
                                 PLI_VALOR = (decimal)p.PAR_VLR_PAGO,
                                 BAN_ID = p.BAN_ID,
                                 AUTHORIZATION_CODE = p.AUTHORIZATION_CODE,
                                 ORDER_KEY = p.ORDER_KEY,
                                 PLI_TIPO_DOC = l.PLI_TIPO_DOC,
                                 REP_ID = c.REP_ID,
                                 REP_NOME = j2.REP_NOME,
                                 REGIAO_UF = c.REGIAO_UF
                             });

                query3 = query.ToList();
            }
            else
            {

                var query2 = (from p in db.PARCELAS
                              join l in db.PARCELA_LIQUIDACAO on p.PAR_NUM_PARCELA equals l.PAR_NUM_PARCELA
                              join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                              join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                              join r in db.REPRESENTANTE on c.REP_ID equals r.REP_ID into j1
                              from j2 in j1.DefaultIfEmpty()
                              where (
                                        (
                                          (_tipodata == 0 && l.PLI_DATA >= _dtini && l.PLI_DATA < _dtfim) ||
                                          (_tipodata == 1 && l.PLI_DATA_BAIXA >= _dtini && l.PLI_DATA_BAIXA < _dtfim)
                                         )

                                     && (c.EMP_ID == _emp_id)
                                     && (_ban_id == null || (_ban_id != null && l.BAN_ID == _ban_id))
                                     && (_tipoBaixa == null || (
                                        (_tipoBaixa == 0 && p.PAR_BAIXA_MANUAL == true) ||
                                        (_tipoBaixa == 1 && (p.PAR_BAIXA_MANUAL == false || p.PAR_BAIXA_MANUAL == null))))
                                     )

                              select new ParcelasConciliacaoDTO
                              {
                                  CLI_ID = a.CLI_ID,
                                  ASN_NUM_ASSINATURA = p.CONTRATOS.ASN_NUM_ASSINATURA,
                                  CLI_NOME = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_NOME,
                                  CLI_CPF_CNPJ = p.CONTRATOS.ASSINATURA.CLIENTES.CLI_CPF_CNPJ,
                                  PAR_NUM_PARCELA = p.PAR_NUM_PARCELA,
                                  PAR_DATA_VENCTO = p.PAR_DATA_VENCTO,
                                  PAR_VLR_PARCELA = (decimal)p.PAR_VLR_PARCELA,
                                  PLI_DATA = l.PLI_DATA,
                                  PLI_DATA_BAIXA = l.PLI_DATA_BAIXA,
                                  PLI_VALOR = (decimal)p.PAR_VLR_PAGO,
                                  BAN_ID = p.BAN_ID,
                                  AUTHORIZATION_CODE = p.AUTHORIZATION_CODE,
                                  ORDER_KEY = p.ORDER_KEY,
                                  PLI_TIPO_DOC = l.PLI_TIPO_DOC,
                                  REP_ID = c.REP_ID,
                                  REP_NOME = j2.REP_NOME,
                                  REGIAO_UF = c.REGIAO_UF
                              }).ToList();

                query3 = query2.OrderByDescending(x => x.PLI_DATA).ThenBy(x => x.PLI_DATA_BAIXA).ToList();

            }

            var _soma = new ParcelasConciliacaoDTO();
            _soma.ASN_NUM_ASSINATURA = "T O T A L";
            _soma.PAR_VLR_PARCELA = 0;
            _soma.PLI_VALOR = 0;

            foreach (var item in query3)
            {
                if (item.PAR_VLR_PARCELA != null)
                    _soma.PAR_VLR_PARCELA += (decimal)item.PAR_VLR_PARCELA;

                if (item.PLI_VALOR != null)
                    _soma.PLI_VALOR += (decimal)item.PLI_VALOR;
                
            }

            query3.Add(_soma);

            return query3;

        }

 
        public IList<BoletosAlocadosDTO> BuscarTitulosAlocados(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
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

            var query = (from p in db.PARCELAS
                         where (p.PAR_DATA_ALOC >= _dtinicial && p.PAR_DATA_ALOC <= _dtfinal)
                         group p by new
                         {
                             p.BAN_ID,
                             p.BANCOS.BAN_NOME,
                             p.PAR_VLR_PARCELA

                         } into f
                         orderby f.Key.BAN_ID
                         select new BoletosAlocadosDTO
                         {
                             BAN_ID = f.Key.BAN_ID,
                             BAN_DESCRICAO = f.Key.BAN_NOME,
                             QTDE_TITULOS = f.Count(),
                             VALOR_TITULOS = (decimal)f.Sum(p => p.PAR_VLR_PARCELA)
                         });

            return query.ToList();

        }
        public Pagina<ParcelasDTO> BuscarTitulosProrrogados(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _situacao = null, int pagina = 1, int registroPorPagina = 20)
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

            var query = (from p in db.PARCELAS
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         where (p.DATA_PRORROGACAO >= _dtinicial && p.DATA_PRORROGACAO <= _dtfinal) &&
                               (_situacao == null || (_situacao != null && p.PAR_SITUACAO == _situacao))
                         select p);

            return ToDTOPage(query, pagina, registroPorPagina);

        }
        public IList<ParcelasDTO> BuscarTitulosProrrogados(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _situacao = null)
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

            var query = (from p in db.PARCELAS
                         join c in db.CONTRATOS on p.CTR_NUM_CONTRATO equals c.CTR_NUM_CONTRATO
                         join a in db.ASSINATURA on c.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                         where (p.DATA_PRORROGACAO >= _dtinicial && p.DATA_PRORROGACAO <= _dtfinal) &&
                               (_situacao == null || (_situacao != null && p.PAR_SITUACAO == _situacao))
                         select p);

            return ToDTO(query);

        }
        public ParcelasDTO BuscarUltimaParcela(string numeroDoContrato)
        {
            var query = GetDbSet()
                .Where(x => x.CTR_NUM_CONTRATO == numeroDoContrato)
                .OrderByDescending(ord => ord.CTR_NUM_CONTRATO)
                .FirstOrDefault();

            return ToDTO(query);
        }
        public Pagina<ParcelasDTO> ListarPorContratos(string numeroContrato, int pagina = 1, int registrosPorPagina = 7)
        {
            var listaparcelas = (from x in db.PARCELAS 
                                 where x.CTR_NUM_CONTRATO == numeroContrato && 
                                 x.DATA_EXCLUSAO == null 
                                 select x).
                                 OrderByDescending(x => x.PAR_DATA_VENCTO);

            var listapartcelasDTO = ToDTOPage(listaparcelas, pagina, registrosPorPagina);
            return listapartcelasDTO;

        }
        public IList<ParcelasDTO> ObterParcelasDoPedidoPagamento(int? PGT_ID, int? IPE_ID = null, bool? paga = null)
        {
            var listaparcelas = (from x in db.PARCELAS where
                                     x.PGT_ID == PGT_ID && 
                                     (IPE_ID == null || x.IPE_ID == IPE_ID) &&
                                     ((paga == null) || 
                                        (paga == false && x.PAR_VLR_PAGO == null && x.PAR_DATA_PAGTO == null) ||
                                        (paga == true && x.PAR_VLR_PAGO != null && x.PAR_DATA_PAGTO != null)
                                     && x.DATA_EXCLUSAO == null
                                    )
                                     
                                 select x).OrderBy(x => x.PAR_DATA_VENCTO);
            
            var listapartcelasDTO = ToDTO(listaparcelas);

            return listapartcelasDTO;

        }
        public ParcelasDTO ObterParcelaDoPedidoPagamento(int? TPG_ID)
        {
            var listaparcelas = (from x in db.PARCELAS 
                                 where x.TPG_ID == TPG_ID &&
                                 x.DATA_EXCLUSAO == null
                                 select x)
                                 .OrderBy(x => x.PAR_DATA_VENCTO);

            var listapartcelasDTO = ToDTO(listaparcelas.FirstOrDefault());

            return listapartcelasDTO;
        }
        public ParcelasDTO ObterProximaParcelaDoPedidoEmAberto(int? IPE_ID)
        {
            var query = (from par in db.PARCELAS 
                         where 
                         ((par.ITEM_PEDIDO.PST_ID == 3 && par.CONTRATOS.IPE_ID == IPE_ID ) || // pego a parcela pelo contrato se o pedido já foi faturado 
                         (par.IPE_ID == IPE_ID)) // senão pego direto pelo pedido
                         && (par.PAR_VLR_PAGO == null)
                         && (par.DATA_EXCLUSAO == null)
                         orderby par.PAR_DATA_VENCTO ascending 
                         select par);

            return ToDTO(query.FirstOrDefault());
        }
        public bool ChecarPedidoPendentePossuiParcelas(int? IPE_ID)
        {
            var query = (from par in db.PARCELAS
                         where
                         par.ITEM_PEDIDO.PST_ID == 1 && 
                         par.IPE_ID == IPE_ID &&
                         par.DATA_EXCLUSAO == null
                         select par.PAR_NUM_PARCELA);

            var conta = query.Count();
            return (conta > 0);
        }
        public IList<ParcelasDTO> ObterParcelasDoPedidoEPedidoPagamento(int? IPE_ID, int? PGT_ID)
        {
            var query = (from par in db.PARCELAS
                         where par.IPE_ID == IPE_ID && 
                         par.PGT_ID == PGT_ID &&
                         par.PAR_PARCELA_DO_PEDIDO == true &&
                         par.DATA_EXCLUSAO == null
                         orderby par.PAR_DATA_VENCTO ascending
                         select par);

            return ToDTO(query);

        }
        public bool PossuiParcelasDoPedidoEPedidoPagamento(int? IPE_ID, int? PGT_ID)
        {
            var query = (from par in db.PARCELAS
                         where par.IPE_ID == IPE_ID && 
                         par.PGT_ID == PGT_ID && 
                         par.PAR_PARCELA_DO_PEDIDO == true
                         select par);

            var count = query.Count();

            return (query.Count() > 0);

        }
        public ParcelasDTO ObterUltimaParcelaDoPedido(int? IPE_ID, int? PGT_ID = null)
        {
            var query = (from par in db.PARCELAS
                         where par.IPE_ID == IPE_ID &&
                         (PGT_ID == null || par.PGT_ID == PGT_ID) &&
                         par.DATA_EXCLUSAO == null
                         select par).
                         OrderByDescending(x => x.PAR_DATA_VENCTO);

            var parcela = query.FirstOrDefault();

            return ToDTO(parcela);

        }
        public bool ChecarParcelaJaAlocada(string codParcela)
        {
            var query = (from par in db.PARCELAS
                         where
                             par.PAR_NUM_PARCELA == codParcela &&
                             par.PAR_DATA_ALOC != null
                         select true).FirstOrDefault();

            return (query);
        }
        public IList<ParcelasDTO> FindByIdList(IEnumerable<string> lstNumeroParcelas)
        {
            lstNumeroParcelas = lstNumeroParcelas.Select(x => x.Trim());

            var query = (from par in db.PARCELAS
                         where lstNumeroParcelas.Contains(par.PAR_NUM_PARCELA)
                         select par);

            return ToDTO(query);

        }

        public IList<ParcelasDTO> ListarParcelaPorProposta(int? ppiId)
        {
            var query = (from par in db.PARCELAS 
                         where par.PPI_ID == ppiId &&
                         par.DATA_EXCLUSAO == null
                             select par);

            return ToDTO(query);
        }

        public bool HasParcelaNaProposta(int? ppiId)
        {
            var count = (from par in db.PARCELAS
                         where par.PPI_ID == ppiId &&
                         par.DATA_EXCLUSAO == null
                         select par)
                         .Count();

            return (count > 0);
        }
        public ParcelasDTO ObterProximaParcelaDaPropostaEmAberto(int? ppiId)
        {
            var query = (from par in db.PARCELAS
                         where
                         par.PROPOSTA_ITEM.DATA_EXCLUSAO == null &&
                         par.PPI_ID == ppiId &&
                         par.PAR_VLR_PAGO == null &&
                         par.DATA_EXCLUSAO == null
                         orderby par.PAR_DATA_VENCTO ascending
                         select par);

            return ToDTO(query.FirstOrDefault());
        }

        public ParametroDTO RetornarDadosDoBoleto(string numParcela)
        {
            var query = (from par in db.PARCELAS
                         where par.PAR_NUM_PARCELA == numParcela
                         select new ParametroDTO() { 
                            idConta = par.CTA_ID,
                            idEmpresa = par.EMP_ID,
                            idTitulo = par.PAR_NUM_PARCELA,
                            preAlocado = true,
                            idRemessa = "01",
                            CodContrato = par.CTR_NUM_CONTRATO,
                            segVia = par.PAR_SEG_VIA,
                            idClienteNoContrato = par.CONTRATOS.ASSINATURA.CLI_ID,
                            idCliente =  
                                (par.PROPOSTA_ITEM.PROPOSTA.CLI_ID != null) ? 
                                 par.PROPOSTA_ITEM.PROPOSTA.CLI_ID : 
                                 par.ITEM_PEDIDO.PEDIDO_CRM.CLI_ID
                         
                         })
                         .FirstOrDefault();
            return query;
        }

        public IList<ParcelasDTO> ListarParcelasALiberarAcesso()
        {
            var query = (from parLiq in db.PARCELA_LIQUIDACAO join 
                         par in db.PARCELAS on parLiq.PAR_NUM_PARCELA equals par.PAR_NUM_PARCELA
                         where (par.PROPOSTA_ITEM.PST_ID == 1 ||
                         par.PROPOSTA_ITEM.PST_ID == 2) &&
                         par.PAR_VLR_PAGO != null &&
                         par.PAR_DATA_PAGTO != null
                         orderby parLiq.PLI_DATA_BAIXA ascending
                         select par).Take(100);

            return ToDTO(query);
        }

        public bool ExisteParcelasPagas(int? ipeId = null, int? ppiId = null)
        {
            if (ipeId == null && ppiId == null)
                throw new ArgumentNullException("Não é possível verificar se há parcelas pagas. O código do pedido ou o código da proposta deve ser informado. Nenhum dos dois foram informados.");

            var query = (from par in db.PARCELAS where 
                         (ipeId == null || par.IPE_ID == ipeId) &&
                         (ppiId == null || par.PPI_ID == ppiId) &&
                            par.PAR_VLR_PAGO != null &&
                            par.PAR_DATA_PAGTO != null &&
                            par.DATA_EXCLUSAO == null
                            select par);

            return (query.Count() > 0);
        }

        public ICollection<ParcelasDTO> ListarParcelasPreFaturamentoDoPedido(int? ipeId)
        {
            var query = (from par in
                             db.PARCELAS
                         where
                             par.IPE_ID == ipeId &&
                             par.DATA_EXCLUSAO == null &&
                             par.CTR_NUM_CONTRATO == null
                         select par);

            return ToDTO(query);
        }

        public ICollection<ParcelasDTO> ListarParcelasPagasDoContrato(string codContrato)
        {
            var query = (from par in
                             db.PARCELAS
                         where
                            par.CTR_NUM_CONTRATO == codContrato &&
                            par.PAR_VLR_PAGO != null &&
                            par.PAR_DATA_PAGTO != null
                         select par);
            return ToDTO(query);
        }

        public ICollection<ParcelasDTO> ListarParcelasContratoPorSequenciaParcela(string codContrato, IEnumerable<int?> lstSeqParcela)
        {
            if(lstSeqParcela != null && !string.IsNullOrWhiteSpace(codContrato))
            {

                var query = (from par in
                                 db.PARCELAS
                             where
                                par.CTR_NUM_CONTRATO == codContrato &&
                                lstSeqParcela.Contains(par.PAR_SEQ_PARCELA)
                             select par);
                return ToDTO(query);
            }

            return new HashSet<ParcelasDTO>();
        }
        public IList<ParcelasDTO> ListarParcelasContrato(string codContrato)
        {
            var query = (from par in db.PARCELAS
                        where par.CTR_NUM_CONTRATO == codContrato 
                           && par.PAR_DATA_PAGTO ==  null
                           && par.DATA_EXCLUSAO == null
                       select par);

            return ToDTO(query);
         
        }
        public Pagina<ParcelasDTO> ListarParcelasPagas(string contrato = null, 
            int? ppiId = null, 
            int? ipeId = null, 
            int pagina = 1, 
            int registrosPorPagina = 7)
        {
            if (string.IsNullOrWhiteSpace(contrato) && 
                ppiId == null && 
                ipeId == null)
            {
                return new Pagina<ParcelasDTO>();
            }

            if (string.IsNullOrWhiteSpace(contrato))
                contrato = null;

            var query = (from par in
                             db.PARCELAS
                         where
                            (par.PAR_VLR_PAGO != null || par.PAR_DATA_PAGTO != null) &&
                            (contrato == null || par.CTR_NUM_CONTRATO == contrato) &&
                            (ppiId == null || par.PPI_ID == ppiId) &&
                            (ipeId == null || par.IPE_ID == ipeId)
                         select par);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public IList<ParcelasDTO> ListarParcelasFaturadasEmAberto(int cliId, DateTime data, int? prtIdExcluir = null)
        {
            var dataVencimentoMin = DateTime.Now.AddYears(-5);
            
            var queryParcelaAssinatura = (from 
                                            ass in db.ASSINATURA join 
                                            con in db.CONTRATOS on ass.ASN_NUM_ASSINATURA equals con.ASN_NUM_ASSINATURA join
                                            par in db.PARCELAS on con.CTR_NUM_CONTRATO equals par.CTR_NUM_CONTRATO
                                          where
                                             (  ass.CLI_ID == cliId &&
                                                par.DATA_EXCLUSAO == null &&
                                                EntityFunctions.TruncateTime(par.PAR_DATA_VENCTO) <= EntityFunctions.TruncateTime(data) &&
                                                EntityFunctions.TruncateTime(par.PAR_DATA_VENCTO) > EntityFunctions.TruncateTime(dataVencimentoMin)
                                             ) &&
                                             (
                                                 par.PAR_DATA_PAGTO == null ||
                                                 par.PAR_VLR_PAGO == null
                                             )
                                          select par);

            //var queryParcelaProposta = (from 
            //                                prt in db.PROPOSTA join 
            //                                ppi in db.PROPOSTA_ITEM on prt.PRT_ID equals ppi.PRT_ID join
            //                                par in db.PARCELAS on ppi.PPI_ID equals par.PPI_ID
            //                              where
            //                                (prtIdExcluir == null || ppi.PRT_ID != prtIdExcluir) &&
            //                                 (  prt.CLI_ID == cliId &&
            //                                    par.DATA_EXCLUSAO == null &&
            //                                    EntityFunctions.TruncateTime(par.PAR_DATA_VENCTO) <= EntityFunctions.TruncateTime(data)
            //                                 ) &&
            //                                 (
            //                                     par.PAR_DATA_PAGTO == null ||
            //                                     par.PAR_VLR_PAGO == null
            //                                 )
            //                              select par);

            //var queryParcelaPedido = (from 
            //                                pedCrm in db.PEDIDO_CRM join 
            //                                ipe in db.ITEM_PEDIDO on pedCrm.PED_CRM_ID equals ipe.PED_CRM_ID join
            //                                par in db.PARCELAS on ipe.IPE_ID equals par.IPE_ID
            //                              where
            //                                 (  pedCrm.CLI_ID == cliId &&
            //                                    par.DATA_EXCLUSAO == null &&
            //                                    EntityFunctions.TruncateTime(par.PAR_DATA_VENCTO) <= EntityFunctions.TruncateTime(data)
            //                                 ) &&
            //                                 (
            //                                     par.PAR_DATA_PAGTO == null ||
            //                                     par.PAR_VLR_PAGO == null
            //                                 )
            //                              select par);

            //var query = queryParcelaAssinatura.Union(queryParcelaProposta).Union(queryParcelaPedido);
            //return ToDTO(query.Distinct());

            return ToDTO(queryParcelaAssinatura);
        }

        public IList<ParcelasDTO> ListarParcelasEmAbertoDaAssinatura(string assinatura, DateTime data)
        {
            var dataVencimentoMin = DateTime.Now.AddYears(-5);
            var query = (from 
                            con in db.CONTRATOS join
                            par in db.PARCELAS on con.CTR_NUM_CONTRATO equals par.CTR_NUM_CONTRATO
                        where
                            (  con.ASN_NUM_ASSINATURA == assinatura &&
                                par.DATA_EXCLUSAO == null &&
                                EntityFunctions.TruncateTime(par.PAR_DATA_VENCTO) <= EntityFunctions.TruncateTime(data) &&
                                EntityFunctions.TruncateTime(par.PAR_DATA_VENCTO) > EntityFunctions.TruncateTime(dataVencimentoMin)
                            ) &&
                            (
                                par.PAR_DATA_PAGTO == null ||
                                par.PAR_VLR_PAGO == null
                            ) &&
                            (((con.CTR_DATA_INI_VIGENCIA <= DateTime.Now &&
                                 ((con.CTR_DATA_FIM_VIGENCIA >= DateTime.Now) ||
                                  (con.CTR_DATA_FIM_VIGENCIA < DateTime.Now && con.CTR_PRORROGADO == 1))) ||
                                   con.CTR_CORTESIA == 1) || (con.CTR_DATA_INI_VIGENCIA > DateTime.Now)) && con.CTR_DATA_CANC == null
                         select par);
            

            return ToDTO(query.Distinct());
        }

        public IList<ParcelasDTO> ListarParcelaPorPedido(int? ipeID)
        {
            var query = (from par in db.PARCELAS
                         where par.IPE_ID == ipeID &&
                         par.DATA_EXCLUSAO == null
                         select par);

            return ToDTO(query);
        }

        public IList<ParcelasDTO> ListarParcelasParaEnvioBoletoDasContas(IList<int> lstCTAId)
        {
            var date = DateTime.Now;
            var diasVencimento = (SysUtils.InHomologation()) ? 15 : 10;
            var ateData = DateUtil.AdicionaDia(date, diasVencimento);

            if (lstCTAId == null)
                lstCTAId = new List<int>();
            var query = (from
                            par in db.PARCELAS
                         where
                             (
                                 par.PAR_DATA_PAGTO == null ||
                                 par.PAR_VLR_PAGO == null
                             ) 
                             &&
                             (
                                par.CTR_NUM_CONTRATO != null ||
                                par.PPI_ID != null || 
                                par.IPE_ID != null
                             ) 
                             &&
                             par.DATA_EXCLUSAO == null &&
                             EntityFunctions.TruncateTime(par.PAR_DATA_VENCTO) >= EntityFunctions.TruncateTime(date) &&
                             EntityFunctions.TruncateTime(par.PAR_DATA_VENCTO) <= EntityFunctions.TruncateTime(ateData) &&
                             (
                                par.PAR_DATA_AGEN_ENVIO == null ||
                                EntityFunctions.TruncateTime(par.PAR_DATA_AGEN_ENVIO) < EntityFunctions.TruncateTime(date)
                             ) &&
                             par.REM_ID != null &&
                             par.CTA_ID != null &&
                             (
                                 par.PAR_NOSSO_NUMERO != null
                             )
                             &&
                             par.PAR_DATA_ENVIO == null 
                             &&
                             (par.CTA_ID != null && lstCTAId.Contains((int) par.CTA_ID))
                         select par
                         );
            return ToDTO(query);
        }

        public DetalhesEnvioBoletoDTO RetornarDetalhesDoEnvioDeBoleto(string codParcela)
        {
            var query = (from 
                            par in db.PARCELAS join 
                            con in db.CONTRATOS on par.CTR_NUM_CONTRATO equals con.CTR_NUM_CONTRATO join
                            ipe in db.ITEM_PEDIDO on con.IPE_ID equals ipe.IPE_ID join
                            pro in db.PRODUTO_COMPOSICAO on ipe.CMP_ID equals pro.CMP_ID join
                            ped in db.PEDIDO_CRM on ipe.PED_CRM_ID equals ped.PED_CRM_ID join
                            cli in db.CLIENTES on ped.CLI_ID equals cli.CLI_ID
                        where 
                             par.PAR_NUM_PARCELA == codParcela
                        select 
                            new DetalhesEnvioBoletoDTO()
                            {
                                CliId = cli.CLI_ID,
                                CodigoItem = con.IPE_ID,
                                DataVenc = par.PAR_DATA_VENCTO,
                                NomeCliente = cli.CLI_NOME,
                                NomeProduto = pro.CMP_DESCRICAO,
                                ValorBoletoDecimal = (par.PAR_VLR_BOLETO != null) ? par.PAR_VLR_BOLETO : par.PAR_VLR_PARCELA,
                                Assinatura = con.ASN_NUM_ASSINATURA
                            }
                        );
            return query.FirstOrDefault();
        }

        public IQueryable<string> ListarArquivosRemessaParaOZip(int? REM_ID)
        {

            var listaparcelas = (from nf in db.NOTA_FISCAL
                                 join
                                    pa in db.PARCELAS on nf.IPE_ID equals pa.IPE_ID
                                 where pa.REM_ID == REM_ID
                                 select nf.NF_PATH_ARQUIVO + "|" + nf.CTR_NUM_CONTRATO ).Distinct();

            return listaparcelas;

        }


        public Pagina<CLIENTE_PASSIVEL_COBRANCA> ListarTitulosEmAtrasoPrimeiraParcela(string assinatura, string cliente, string atendente, string cnpj, DateTime? dataini, DateTime? datafim, int pagina, bool primeiraParcela)
        {
            string q = null;

            if (!string.IsNullOrWhiteSpace(assinatura))
            {
                q = "Select * " +
                    "From CLIENTE_PASSIVEL_COBRANCA as par " +
                    "inner join PARCELAS pa on par.PAR_NUM_PARCELA = pa.PAR_NUM_PARCELA and pa.PAR_DATA_PAGTO is null " +
                    "Where (not exists (select * from AGENDA_COBRANCA as age WHERE age.STATUS = 0 AND age.CLI_ID = par.CLI_ID)) " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' and par.PAR_PRIMEIRA_PARCELA = '" + primeiraParcela + "' " +
                    "And par.ASN_NUM_ASSINATURA = '" + assinatura + "' " +
                    "Order by par.DIAS_ATRASO, par.CLI_NOME";

                IQueryable<CLIENTE_PASSIVEL_COBRANCA> qa = db.Database.SqlQuery<CLIENTE_PASSIVEL_COBRANCA>(q).AsQueryable();
                return qa.Paginar(pagina, 20);
            }

            if (!string.IsNullOrWhiteSpace(cliente))
            {
                q = "Select * " +
                    "From CLIENTE_PASSIVEL_COBRANCA as par " +
                    "inner join PARCELAS pa on par.PAR_NUM_PARCELA = pa.PAR_NUM_PARCELA and pa.PAR_DATA_PAGTO is null " +
                    "Where (not exists (select * from AGENDA_COBRANCA as age WHERE age.STATUS = 0 AND age.CLI_ID = par.CLI_ID)) " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' and par.PAR_PRIMEIRA_PARCELA = '" + primeiraParcela + "' " +
                    "And par.CLI_NOME like '%" + cliente + "%' " +
                    "Order by par.DIAS_ATRASO, par.CLI_NOME";

                IQueryable<CLIENTE_PASSIVEL_COBRANCA> qa = db.Database.SqlQuery<CLIENTE_PASSIVEL_COBRANCA>(q).AsQueryable();
                return qa.Paginar(pagina, 20);
            }

            if (!string.IsNullOrWhiteSpace(atendente))
            {
                q = "Select * " +
                    "From CLIENTE_PASSIVEL_COBRANCA as par " +
                    "inner join PARCELAS pa on par.PAR_NUM_PARCELA = pa.PAR_NUM_PARCELA and pa.PAR_DATA_PAGTO is null " +
                    "Where (not exists (select * from AGENDA_COBRANCA as age WHERE age.STATUS = 0 AND age.CLI_ID = par.CLI_ID)) " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' and par.PAR_PRIMEIRA_PARCELA = '" + primeiraParcela + "' " +
                    "And par.USU_LOGIN like '%" + atendente + "%' " +
                    "Order by par.DIAS_ATRASO, par.CLI_NOME";

                IQueryable<CLIENTE_PASSIVEL_COBRANCA> qa = db.Database.SqlQuery<CLIENTE_PASSIVEL_COBRANCA>(q).AsQueryable();
                return qa.Paginar(pagina, 20);

            }

            q = "Select * " +
                "From CLIENTE_PASSIVEL_COBRANCA as par " +
                "inner join PARCELAS pa on par.PAR_NUM_PARCELA = pa.PAR_NUM_PARCELA and pa.PAR_DATA_PAGTO is null " +
                "Where (not exists (select * from AGENDA_COBRANCA as age WHERE age.STATUS = 0 AND age.CLI_ID = par.CLI_ID)) " +
                "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' and par.PAR_PRIMEIRA_PARCELA = '" + primeiraParcela + "' " +
                "Order by par.DIAS_ATRASO, par.CLI_NOME";

            IQueryable<CLIENTE_PASSIVEL_COBRANCA> qy = db.Database.SqlQuery<CLIENTE_PASSIVEL_COBRANCA>(q).AsQueryable();

            return qy.Paginar(pagina, 20);

        }

        //Agenda de cobrança
        public Pagina<PARCELA_PENDENTE> ListarTitulosEmAtraso(string assinatura, string cliente, string atendente, string cnpj, DateTime? dataini, DateTime? datafim, int pagina, bool primeiraParcela)
        {
            string q = null;

            if (!string.IsNullOrWhiteSpace(assinatura))
            {
                q = "Select * " +
                    "From PARCELA_PENDENTE as par " +
                    "inner join PARCELAS pa on par.PAR_NUM_PARCELA = pa.PAR_NUM_PARCELA and pa.PAR_DATA_PAGTO is null " +
                    "Where (not exists (select * from AGENDA_COBRANCA as age WHERE age.STATUS = 0 AND age.CLI_ID = par.CLI_ID)) " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' and par.ASN_NUM_ASSINATURA = '" + assinatura + "' " +
                    "Order by par.DIAS_ATRASO, par.CLI_NOME";

                IQueryable<PARCELA_PENDENTE> qa = db.Database.SqlQuery<PARCELA_PENDENTE>(q).AsQueryable();
                return qa.Paginar(pagina, 20);
            }

            if (!string.IsNullOrWhiteSpace(cliente))
            {
                q = "Select * " +
                    "From PARCELA_PENDENTE as par " +
                    "inner join PARCELAS pa on par.PAR_NUM_PARCELA = pa.PAR_NUM_PARCELA and pa.PAR_DATA_PAGTO is null " +
                    "Where (not exists (select * from AGENDA_COBRANCA as age WHERE age.STATUS = 0 AND age.CLI_ID = par.CLI_ID)) " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' " +
                    "And par.CLI_NOME like '%" + cliente + "%' " +
                    "Order by par.DIAS_ATRASO, par.CLI_NOME";

                IQueryable<PARCELA_PENDENTE> qa = db.Database.SqlQuery<PARCELA_PENDENTE>(q).AsQueryable();
                return qa.Paginar(pagina, 20);
            }

            if (!string.IsNullOrWhiteSpace(atendente))
            {
                q = "Select * " +
                    "From PARCELA_PENDENTE as par " +
                    "inner join PARCELAS pa on par.PAR_NUM_PARCELA = pa.PAR_NUM_PARCELA and pa.PAR_DATA_PAGTO is null " +
                    "Where (not exists (select * from AGENDA_COBRANCA as age WHERE age.STATUS = 0 AND age.CLI_ID = par.CLI_ID)) " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' " +
                    "And par.USU_LOGIN like '%" + atendente + "%' " +
                    "Order by par.DIAS_ATRASO, par.CLI_NOME";

                IQueryable<PARCELA_PENDENTE> qa = db.Database.SqlQuery<PARCELA_PENDENTE>(q).AsQueryable();
                return qa.Paginar(pagina, 20);
            }

            if (!string.IsNullOrWhiteSpace(cnpj))
            {
                q = "Select * " +
                    "From PARCELA_PENDENTE as par " +
                    "inner join PARCELAS pa on par.PAR_NUM_PARCELA = pa.PAR_NUM_PARCELA and pa.PAR_DATA_PAGTO is null " +
                    "inner join CLIENTES cli on cli.CLI_ID = par.CLI_ID " +
                    "Where (not exists (select * from AGENDA_COBRANCA as age WHERE age.STATUS = 0 AND age.CLI_ID = par.CLI_ID)) " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' " +
                    "And cli.CLI_CPF_CNPJ like '%" + cnpj + "%' " +
                    "Order by par.DIAS_ATRASO, par.CLI_NOME";

                IQueryable<PARCELA_PENDENTE> qa = db.Database.SqlQuery<PARCELA_PENDENTE>(q).AsQueryable();
                return qa.Paginar(pagina, 20);
            }

            q = "Select * " +
                "From PARCELA_PENDENTE as par " +
                "inner join PARCELAS pa on par.PAR_NUM_PARCELA = pa.PAR_NUM_PARCELA and pa.PAR_DATA_PAGTO is null " +
                "Where (not exists (select * from AGENDA_COBRANCA as age WHERE age.STATUS = 0 AND age.CLI_ID = par.CLI_ID)) " +
                "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' " +
                "Order by par.DIAS_ATRASO, par.CLI_NOME";

            IQueryable<PARCELA_PENDENTE> qy = db.Database.SqlQuery<PARCELA_PENDENTE>(q).AsQueryable();

            return qy.Paginar(pagina, 20);

        }

        public Pagina<ParcelasDTO> ListarTitulosComParcelaLiberada(string assinatura, string cliente, string atendente, string cnpj, DateTime? dataini, DateTime? datafim, int pagina)
        {
            string q = null;

            if (!string.IsNullOrWhiteSpace(assinatura))
            {
                q = "Select par.PAR_NUM_PARCELA, ass.ASN_NUM_ASSINATURA, par.PAR_DATA_VENCTO,par.PAR_VLR_PARCELA, cli.CLI_NOME, par.DATA_ALTERA, par.USU_LOGIN, par.PAR_SITUACAO " +
                    "From PARCELAS as par " +
                    "inner join CONTRATOS con on par.CTR_NUM_CONTRATO = con.CTR_NUM_CONTRATO and par.PAR_DATA_PAGTO is null " +
                    "inner join ASSINATURA ass on con.ASN_NUM_ASSINATURA = ass.ASN_NUM_ASSINATURA " +
                    "inner join CLIENTES cli on cli.CLI_ID = ass.CLI_ID " + 
                    "Where par.PAR_SITUACAO = 'LIB' " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' " +
                    "And ass.ASN_NUM_ASSINATURA = '" + assinatura + "' " +
                    "Order by par.PAR_DATA_VENCTO desc, cli.CLI_NOME";

                IQueryable<ParcelasDTO> qy = db.Database.SqlQuery<ParcelasDTO>(q).AsQueryable();
                return qy.Paginar(pagina, 20);
            }

            if (!string.IsNullOrWhiteSpace(cliente))
            {
                q = "Select par.PAR_NUM_PARCELA, ass.ASN_NUM_ASSINATURA, par.PAR_DATA_VENCTO,par.PAR_VLR_PARCELA, cli.CLI_NOME, par.DATA_ALTERA, par.USU_LOGIN, par.PAR_SITUACAO " +
                    "From PARCELAS as par " +
                    "inner join CONTRATOS con on par.CTR_NUM_CONTRATO = con.CTR_NUM_CONTRATO and par.PAR_DATA_PAGTO is null " +
                    "inner join ASSINATURA ass on con.ASN_NUM_ASSINATURA = ass.ASN_NUM_ASSINATURA " +
                    "inner join CLIENTES cli on cli.CLI_ID = ass.CLI_ID " +
                    "Where par.PAR_SITUACAO = 'LIB' " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' " +
                    "And cli.CLI_NOME like '%" + cliente + "%' " +
                    "Order by par.PAR_DATA_VENCTO desc, cli.CLI_NOME";

                IQueryable<ParcelasDTO> qy = db.Database.SqlQuery<ParcelasDTO>(q).AsQueryable();
                return qy.Paginar(pagina, 20);
            }

            if (!string.IsNullOrWhiteSpace(cnpj))
            {
                q = "Select par.PAR_NUM_PARCELA, ass.ASN_NUM_ASSINATURA, par.PAR_DATA_VENCTO,par.PAR_VLR_PARCELA, cli.CLI_NOME, par.DATA_ALTERA, par.USU_LOGIN, par.PAR_SITUACAO " +
                    "From PARCELAS as par " +
                    "inner join CONTRATOS con on par.CTR_NUM_CONTRATO = con.CTR_NUM_CONTRATO and par.PAR_DATA_PAGTO is null " +
                    "inner join ASSINATURA ass on con.ASN_NUM_ASSINATURA = ass.ASN_NUM_ASSINATURA " +
                    "inner join CLIENTES cli on cli.CLI_ID = ass.CLI_ID " +
                    "Where par.PAR_SITUACAO = 'LIB' " +
                    "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' " +
                    "And cli.CLI_CPF_CNPJ like '%" + cnpj + "%' " +
                    "Order by par.PAR_DATA_VENCTO desc, cli.CLI_NOME";

                IQueryable<ParcelasDTO> qy = db.Database.SqlQuery<ParcelasDTO>(q).AsQueryable();
                return qy.Paginar(pagina, 20);
            }

            q = "Select par.PAR_NUM_PARCELA, ass.ASN_NUM_ASSINATURA, par.PAR_DATA_VENCTO,par.PAR_VLR_PARCELA, cli.CLI_NOME, par.DATA_ALTERA, par.USU_LOGIN, par.PAR_SITUACAO " +
                   "From PARCELAS as par " +
                   "inner join CONTRATOS con on par.CTR_NUM_CONTRATO = con.CTR_NUM_CONTRATO and par.PAR_DATA_PAGTO is null " +
                   "inner join ASSINATURA ass on con.ASN_NUM_ASSINATURA = ass.ASN_NUM_ASSINATURA " +
                   "inner join CLIENTES cli on cli.CLI_ID = ass.CLI_ID " +
                   "Where par.PAR_SITUACAO = 'LIB' " +
                   "And par.PAR_DATA_VENCTO between '" + dataini + "' and '" + datafim + "' " +
                   "Order by par.PAR_DATA_VENCTO desc, cli.CLI_NOME";

            IQueryable<ParcelasDTO> qa = db.Database.SqlQuery<ParcelasDTO>(q).AsQueryable();
            return qa.Paginar(pagina, 20);

        }

        public Pagina<PARCELA_PENDENTE> ListarTitulosComParcelaLiberada(int pagina)
        {
            IList<PARCELA_PENDENTE> query;

                 query = db.PARCELA_PENDENTE
                      .Where(x => x.PAR_SITUACAO == "LIB")
                      .OrderBy(x => x.PAR_DATA_VENCTO).ThenBy(x => x.CLI_NOME).ToList();

            return query.Paginar(pagina, 20);

        }

        //public void ExecutarClientePassivelDeCobranca()
        //{
        //    ObjectResult<PASSIVEL_DE_COBRANCA_Result> query = db.PASSIVEL_DE_COBRANCA();

        //}

    }

}

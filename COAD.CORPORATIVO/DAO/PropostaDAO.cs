using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.IOCContainer.Proxies;
using System.Data.Objects;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Data.Objects.SqlClient;
using Coad.GenericCrud.Dao.Base;
using COAD.UTIL.Grafico;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using GenericCrud.Models.Filtros;
using Coad.GenericCrud.Extensions;
using COAD.CORPORATIVO.Model.Dto.Custons.Listagens;

namespace COAD.CORPORATIVO.DAO
{
    public class PropostaDAO : AbstractGenericDao<PROPOSTA, PropostaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public PropostaDAO()
        {
            //db = GetDb<COADCORPEntities>();
        }

        public Pagina<PropostaDTO> ListarPropostas(PesquisaPropostaDTO pesquisaPropostaDTO)
        {
            string nomeCliente = pesquisaPropostaDTO.nomeCliente;
            int? cliId = pesquisaPropostaDTO.CLI_ID;
            int? pstId = pesquisaPropostaDTO.PST_ID;
            string assinatura = pesquisaPropostaDTO.assinatura;
            string cpfCnpj = pesquisaPropostaDTO.cpfCnpj;
            DateTime? dataInicial = pesquisaPropostaDTO.dataInicial;
            DateTime? dataFinal = pesquisaPropostaDTO.dataInicial;
            DateTime? dataPagamento = pesquisaPropostaDTO.dataPagamento;
            DateTime? dataVencimentoInicial = pesquisaPropostaDTO.dataVencimentoInicial;
            DateTime? dataVencimentoFinal = pesquisaPropostaDTO.dataVencimentoFinal;
            int? repId = pesquisaPropostaDTO.REP_ID;
            int? rgId = pesquisaPropostaDTO.RG_ID;
            int? uenId = pesquisaPropostaDTO.UEN_ID;
            int pagina = pesquisaPropostaDTO.pagina;
            int registrosPorPagina = pesquisaPropostaDTO.registrosPorPagina;
            int? tppId = pesquisaPropostaDTO.TPP_ID;
            int? tpgId = pesquisaPropostaDTO.TPG_ID;
            int? proId = pesquisaPropostaDTO.PRO_ID;
            int? prtId = pesquisaPropostaDTO.PRT_ID;
            int? ppiId = pesquisaPropostaDTO.PPI_ID;
            int? tneId = pesquisaPropostaDTO.TNE_ID;

            if (string.IsNullOrWhiteSpace(cpfCnpj)){
                cpfCnpj = null;
            }

            if (string.IsNullOrWhiteSpace(nomeCliente))
            {
                nomeCliente = null;
            }

            IQueryable<PROPOSTA> query = (from pro in db.PROPOSTA
                                          where
                                             (pro.DATA_EXCLUSAO == null) &&
                                             (dataInicial == null || EntityFunctions.TruncateTime(pro.DATA_CADASTRO) >= EntityFunctions.TruncateTime(dataInicial)) &&
                                             (dataFinal == null || EntityFunctions.TruncateTime(pro.DATA_CADASTRO) <= EntityFunctions.TruncateTime(dataFinal)) &&
                                             (cliId == null || pro.CLI_ID == cliId) &&
                                             (pstId == null || pro.PST_ID == pstId) &&
                                             (cpfCnpj == null || pro.CLIENTES.CLI_CPF_CNPJ.Contains(cpfCnpj)) &&
                                             (nomeCliente == null || pro.CLIENTES.CLI_NOME.Contains(nomeCliente)) &&
                                             (repId == null || pro.REP_ID == repId) &&
                                             (rgId == null || pro.RG_ID == rgId) &&
                                             (uenId == null || pro.UEN_ID == uenId) &&
                                             (tppId == null || pro.TPP_ID == tppId) &&
                                             (prtId == null || pro.PRT_ID == prtId) &&
                                             (tneId == null || pro.TNE_ID == tneId)
                                          select pro);

            if (ppiId != null)
            {
                query = (from ppi in db.PROPOSTA_ITEM
                         join pro in query on ppi.PRT_ID equals pro.PRT_ID
                         where ppi.PPI_ID == ppiId
                         select pro);
            }

            if (tpgId != null || 
                proId != null || 
                dataVencimentoInicial != null || 
                dataVencimentoFinal != null)
            {
                query = (from pro in query
                         where
                           (from proItm in db.PROPOSTA_ITEM
                            where
                                (proId == null || (proItm.CMP_ID != null &&
                                        (from cmi in db.PRODUTO_COMPOSICAO_ITEM
                                            where cmi.PRO_ID == proId
                                            select cmi.CMP_ID).Contains((int)proItm.CMP_ID))) &&
                                (tpgId == null || proItm.TPG_ID == tpgId) &&
                                (dataVencimentoInicial == null || EntityFunctions.TruncateTime(proItm.PPI_DATA_VENCIMENTO) >= EntityFunctions.TruncateTime(dataVencimentoInicial)) &&
                                (dataVencimentoFinal == null || EntityFunctions.TruncateTime(proItm.PPI_DATA_VENCIMENTO) <= EntityFunctions.TruncateTime(dataVencimentoFinal))
                            select proItm.PRT_ID)
                           .Contains(pro.PRT_ID)
                                     select pro);
            }

            if (!string.IsNullOrWhiteSpace(assinatura))
            {
                query = (from
                            pro in query
                         join
                            cli in db.CLIENTES on pro.CLI_ID equals cli.CLI_ID
                         join
                            ass in db.ASSINATURA on cli.CLI_ID equals ass.CLI_ID
                         where ass.ASN_NUM_ASSINATURA == assinatura
                         select pro);
            }

            if(dataPagamento != null)
            {
                var lstStatus = new List<int?>() { 7, 8 };
                query = (from
                            hist in db.HISTORICO_PEDIDO
                         join
                            itm in db.PROPOSTA_ITEM on hist.PPI_ID equals itm.PPI_ID
                         join
                            pro in query on itm.PRT_ID equals pro.PRT_ID
                        where 
                            EntityFunctions .TruncateTime(hist.HIP_DATA) == EntityFunctions.TruncateTime(dataPagamento) &&
                            lstStatus.Contains(hist.PST_ID)
                         select pro);
            }

            query = query.OrderByDescending(or => or.DATA_CADASTRO);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public int? RetornarCliIdDaPropostaPorPropostaItem(int? ppiId)
        {
            var query = (from pItm in db.PROPOSTA_ITEM
                         where pItm.PPI_ID == ppiId
                         select pItm.PROPOSTA
                         .CLI_ID).FirstOrDefault();
            return query;

        }

        public JsonGraficoResumo ListarPropostaPeriodo(int _mes, int _ano, int? _emp_id, int? _grupo_id)
        {
            var _retorno = new JsonGraficoResumo();

            var query = (from p in db.PROPOSTA
                         join i in db.PROPOSTA_ITEM on p.PRT_ID equals i.PRT_ID
                         join c in db.PRODUTO_COMPOSICAO_ITEM on i.CMP_ID equals c.CMP_ID 
                         join r in db.PRODUTOS on c.PRO_ID equals r.PRO_ID
                         join g in db.GRUPO on r.GRUPO_ID equals g.GRUPO_ID
                         where (p.DATA_CADASTRO.Value.Year == _ano) &&
                               (p.DATA_CADASTRO.Value.Month == _mes) &&
                               ((_emp_id == null) || (_emp_id != null && p.EMP_ID == _emp_id)) &&
                               ((_grupo_id == null) || (_grupo_id != null && g.GRUPO_ID == _grupo_id)) &&
                               (c.CMI_GERA_ASSINATURA_LEGADO == true)
                         group i by new { p.DATA_CADASTRO.Value.Month, p.DATA_CADASTRO.Value.Day } into f
                         orderby f.Key.Month, f.Key.Day
                         select new tp_dataPieColumn3d
                         {
                             label = SqlFunctions.StringConvert((double)f.Key.Day) + "/" + SqlFunctions.StringConvert((double)f.Key.Month),
                             value = SqlFunctions.StringConvert(f.Sum(x => (decimal)x.PPI_TOTAL))
                         
                         }).AsQueryable();

            var _pagos = (from p in db.PROPOSTA
                          join i in db.PROPOSTA_ITEM on p.PRT_ID equals i.PRT_ID
                          join c in db.PRODUTO_COMPOSICAO_ITEM on i.CMP_ID equals c.CMP_ID
                          join r in db.PRODUTOS on c.PRO_ID equals r.PRO_ID
                          join g in db.GRUPO on r.GRUPO_ID equals g.GRUPO_ID
                          where (p.DATA_CADASTRO.Value.Year == _ano) &&
                                (p.DATA_CADASTRO.Value.Month == _mes) &&
                                (p.PST_ID == 2 || p.PST_ID == 7 || p.PST_ID == 8) &&
                               ((_emp_id == null) || (_emp_id != null && p.EMP_ID == _emp_id)) &&
                               ((_grupo_id == null) || (_grupo_id != null && g.GRUPO_ID == _grupo_id)) &&
                               (c.CMI_GERA_ASSINATURA_LEGADO == true)
                          group i by new { p.DATA_CADASTRO.Value.Year, p.DATA_CADASTRO.Value.Month } into f
                          orderby f.Key.Month
                          select new JsonGrafico
                          {
                              label = "CONFIRMADOS",
                              decimalData = f.Sum(x => (decimal)x.PPI_TOTAL)

                          }).FirstOrDefault();

            var _canc = (from p in db.PROPOSTA
                         join i in db.PROPOSTA_ITEM on p.PRT_ID equals i.PRT_ID
                         join c in db.PRODUTO_COMPOSICAO_ITEM on i.CMP_ID equals c.CMP_ID
                         join r in db.PRODUTOS on c.PRO_ID equals r.PRO_ID
                         join g in db.GRUPO on r.GRUPO_ID equals g.GRUPO_ID
                         where (p.DATA_CADASTRO.Value.Year == _ano) &&
                               (p.DATA_CADASTRO.Value.Month == _mes) &&
                               (p.PST_ID == 5) &&
                               ((_emp_id == null) || (_emp_id != null && p.EMP_ID == _emp_id)) &&
                               ((_grupo_id == null) || (_grupo_id != null && g.GRUPO_ID == _grupo_id)) &&
                               (c.CMI_GERA_ASSINATURA_LEGADO == true)
                         group i by new { p.DATA_CADASTRO.Value.Year, p.DATA_CADASTRO.Value.Month } into f
                         orderby f.Key.Month
                         select new JsonGrafico
                         {
                             label = "CANCELADA",
                             decimalData = f.Sum(x => (decimal)x.PPI_TOTAL)

                         }).FirstOrDefault();

            var _todos = (from p in db.PROPOSTA
                          join i in db.PROPOSTA_ITEM on p.PRT_ID equals i.PRT_ID
                          join c in db.PRODUTO_COMPOSICAO_ITEM on i.CMP_ID equals c.CMP_ID
                          join r in db.PRODUTOS on c.PRO_ID equals r.PRO_ID
                          join g in db.GRUPO on r.GRUPO_ID equals g.GRUPO_ID
                          where (p.DATA_CADASTRO.Value.Year == _ano) &&
                                (p.DATA_CADASTRO.Value.Month == _mes) &&
                                ((_emp_id == null) || (_emp_id != null && p.EMP_ID == _emp_id)) &&
                                ((_grupo_id == null) || (_grupo_id != null && g.GRUPO_ID == _grupo_id)) &&
                                (c.CMI_GERA_ASSINATURA_LEGADO == true)
                          group i by new { p.DATA_CADASTRO.Value.Year, p.DATA_CADASTRO.Value.Month } into f
                          orderby f.Key.Month
                          select new JsonGrafico
                          {
                              label = "TOTAL",
                              decimalData = f.Sum(x => (decimal)x.PPI_TOTAL)

                          }).FirstOrDefault();

            if (_todos != null)
                _retorno.total = _todos.decimalData;

            if (_pagos != null)
                _retorno.pagos = _pagos.decimalData;

            if (_canc != null)
                _retorno.cancelada = _canc.decimalData;

            _retorno.grafico.data = query.ToList(); 

            return _retorno;

        }

        public IList<AutoCompleteDTO> ListarAssinaturaDaPropostaAutoComplete(string assinatura)
        {
            if (!string.IsNullOrEmpty(assinatura) && assinatura.Count() > 4)
            {
                var query = (
                            from
                                itm in db.PROPOSTA_ITEM
                            where itm.ASN_NUM_ASSINATURA.Contains(assinatura)
                            select new AutoCompleteDTO()
                            {

                                label = itm.ASN_NUM_ASSINATURA,
                                value = itm.ASN_NUM_ASSINATURA
                            });

                return query.ToList();

            }
            return new List<AutoCompleteDTO>();
        }

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorStatus()
        {
            var query = (from pro in db.PROPOSTA
                         where pro.PST_ID != null
                         group pro by new
                         {
                             pro.PEDIDO_STATUS.PST_STATUS,
                             pro.PEDIDO_STATUS.PST_ID
                         } into grp
                         select new
                         {

                             Chave = "PST_ID",
                             Label = grp.Key.PST_STATUS,
                             Count = grp.Count(),
                             Valor = grp.Key.PST_ID
                         }).ToList();

            var retorno = (from st in query
                           select new GrupoDeFiltroDTO()
                           {

                               chave = "PST_ID",
                               label = st.Label,
                               Count = st.Count,
                               valor = st.Valor
                           }).AsQueryable();
            return retorno;
        }

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorTipoProposta()
        {
            var query = (from pro in db.PROPOSTA
                         where pro.TPP_ID != null
                         group pro by new
                         {
                             pro.TIPO_PROPOSTA.TPP_DESCRICAO,
                             pro.TIPO_PROPOSTA.TPP_ID
                         } into grp
                         select new
                         {

                             Chave = "TPP_ID",
                             Label = grp.Key.TPP_DESCRICAO,
                             Count = grp.Count(),
                             Valor = grp.Key.TPP_ID
                         }).ToList();

            var retorno = (from st in query
                           select new GrupoDeFiltroDTO()
                           {

                               chave = "TPP_ID",
                               label = st.Label,
                               Count = st.Count,
                               valor = st.Valor
                           }).AsQueryable();
            return retorno;
        }

        public int? RetornarCodPedidoDaProposta(int? prtId)
        {
            var query = (from pedCrm in db.PEDIDO_CRM
                         where pedCrm.PRT_ID == prtId
                         select pedCrm.PED_CRM_ID)
                         .FirstOrDefault();
            return query;
        }

        public Pagina<ListagemPropostaDTO> PesquisarPropostaPendConf(string queryStr, DateTime? dataInicial = null, DateTime? dataFinal = null, RequisicaoPaginacao requisicao = null)
        {
            if (string.IsNullOrWhiteSpace(queryStr))
            {
                queryStr = null;
            }

            var query = (from
                            pro in db.PROPOSTA join
                            cli in db.CLIENTES on pro.CLI_ID equals cli.CLI_ID
                         where
                            pro.DATA_EXCLUSAO == null &&
                            pro.PST_ID == 2 &&
                            (
                                queryStr == null ||
                                cli.CLI_NOME.Contains(queryStr) ||
                                cli.CLI_CPF_CNPJ.Contains(queryStr) ||
                                SqlFunctions.StringConvert((double) pro.PRT_ID).Contains(queryStr)
                            )
                         select pro);

            if(dataInicial != null ||
                dataFinal != null)
            {
                var lstStatus = new List<int?>() {2, 7, 8 };
                query = (from
                            hist in db.HISTORICO_PEDIDO
                         join
                            itm in db.PROPOSTA_ITEM on hist.PPI_ID equals itm.PPI_ID
                         join
                            pro in query on itm.PRT_ID equals pro.PRT_ID
                         where
                             (dataInicial == null || EntityFunctions.TruncateTime(hist.HIP_DATA) >= EntityFunctions.TruncateTime(dataInicial)) &&
                                (dataFinal == null || EntityFunctions.TruncateTime(hist.HIP_DATA) <= EntityFunctions.TruncateTime(dataFinal)) &&
                             lstStatus.Contains(hist.PST_ID)
                         select pro);
            }

            var queryFinal = query.Select(x => new ListagemPropostaDTO()
            {
                PropostaID = x.PRT_ID,
                DataCadastro = x.DATA_CADASTRO,
                UenID = x.UEN_ID,
                CpfCnpjCliente = x.CLIENTES.CLI_CPF_CNPJ,
                NomeCliente = x.CLIENTES.CLI_NOME
            }).Distinct();

            return queryFinal.Paginar(requisicao);
        }

    }
}
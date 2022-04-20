using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Extensions;
using COAD.CORPORATIVO.Model.Dto.Custons;
using System.Data.Objects.SqlClient;
using COAD.SEGURANCA.Model;
using System.Data.Objects;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Util;

namespace COAD.CORPORATIVO.DAO
{
    public class PedidoCRMDAO : DAOAdapter<PEDIDO_CRM, PedidoCRMDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public PedidoCRMDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public IQueryable<PEDIDO_CRM> TemplatePedidoCRM(
            PesquisaPedidoDTO pesquisaDTO)
        {
            var assinatura = pesquisaDTO.assinatura;
            var nomeCliente = pesquisaDTO.nomeCliente;
            var cpfCnpjCliente = pesquisaDTO.cpfCnpjCliente;
            var PED_CRM_ID = pesquisaDTO.PED_CRM_ID;
            var IPE_ID = pesquisaDTO.IPE_ID;
            var PRT_ID = pesquisaDTO.PRT_ID;
            var PPI_ID = pesquisaDTO.PPI_ID;
            var REP_ID = pesquisaDTO.REP_ID;
            var CLI_ID = pesquisaDTO.CLI_ID;
            var CMP_ID = pesquisaDTO.CMP_ID;
            var dataInicial = pesquisaDTO.dataInicial;
            var dataFinal = pesquisaDTO.dataFinal;
            var UEN_ID = pesquisaDTO.UEN_ID;
            var RG_ID = pesquisaDTO.RG_ID;
            var exibirExcluidos = true;
            var aprovacaoPendente = pesquisaDTO.aprovacaoPendente;
            var PST_ID = pesquisaDTO.PST_ID;
            var numeroNotaInicial = pesquisaDTO.numeroNotaInicial;
            var numeroNotaFinal = pesquisaDTO.numeroNotaFinal;
            var dataFaturamentoInicial = pesquisaDTO.dataFaturamentoInicial;
            var dataFaturamentoFinal = pesquisaDTO.dataFaturamentoFinal;
            var semNotaFiscal = pesquisaDTO.semNotaFiscal;
            var dataPagamento = pesquisaDTO.dataPagamento;
            var grupoDataPedido = pesquisaDTO.grupoDataPedido;
            var grupoDataPedidoFaturamento = pesquisaDTO.grupoDataPedidoFaturamento;
            var TNE_ID = pesquisaDTO.TNE_ID;

            if(grupoDataPedido != null)
            {
                dataInicial = grupoDataPedido;
                dataFinal = grupoDataPedido;
            }

            if (grupoDataPedidoFaturamento != null)
            {
                dataFaturamentoInicial = grupoDataPedidoFaturamento;
                dataFaturamentoFinal = grupoDataPedidoFaturamento;
            }

            IQueryable<PEDIDO_CRM> query = (from
                            x in db.PEDIDO_CRM select x);

            if (numeroNotaInicial != null || numeroNotaFinal != null)
            {
                //query = (from nfx in db.NFE_XML
                //         join
                //            ipe in db.ITEM_PEDIDO on nfx.IPE_ID equals ipe.IPE_ID
                //         join
                //            ped in query on ipe.PED_CRM_ID equals ped.PED_CRM_ID
                //         where
                //            (numeroNotaInicial == null || nfx.NFX_NUMERO_NOTA >= numeroNotaInicial) &&
                //            (numeroNotaFinal == null || nfx.NFX_NUMERO_NOTA <= numeroNotaFinal) &&
                //             (nfx.NFX_DATA_EXCLUSAO == null
                //             || nfx.NFX_NUM_EXTORNADO == null
                //             || nfx.NFX_NUM_EXTORNADO == false)

                //         select ped);

                query = (from nfe in db.NOTA_FISCAL
                         join
                            ipe in db.ITEM_PEDIDO on nfe.IPE_ID equals ipe.IPE_ID
                         join
                            ped in query on ipe.PED_CRM_ID equals ped.PED_CRM_ID
                         where
                            (numeroNotaInicial == null || nfe.NF_NUMERO >= numeroNotaInicial) &&
                            (numeroNotaFinal == null || nfe.NF_NUMERO <= numeroNotaFinal)
                         select ped);
            }

            if (dataFaturamentoFinal != null || dataFaturamentoInicial != null)
            {
                query = (from
                            ipe in db.ITEM_PEDIDO 
                         join
                            ped in query on ipe.PED_CRM_ID equals ped.PED_CRM_ID
                         where
                            (dataFaturamentoInicial == null || EntityFunctions.TruncateTime(ipe.IPE_DATA_FATURAMENTO) >= EntityFunctions.TruncateTime(dataFaturamentoInicial)) &&
                            (dataFaturamentoFinal == null || EntityFunctions.TruncateTime(ipe.IPE_DATA_FATURAMENTO) <= EntityFunctions.TruncateTime(dataFaturamentoFinal))

                         select ped);                
            }
            
            if(semNotaFiscal == true)
            {
                query = (from
                            ipe in db.ITEM_PEDIDO
                         join
                            ped in query on ipe.PED_CRM_ID equals ped.PED_CRM_ID
                         join 
                            con in db.CONTRATOS on ipe.IPE_ID equals con.IPE_ID
                         where
                            con.CTR_DATA_CANC == null &&
                            ped.PST_ID != 12 && 
                            ped.PST_ID != 1 &&
                            ped.PST_ID != 5 &&
                            !(from 
                                    nf in db.NOTA_FISCAL
                                where
                                    nf.NF_TIPO == 1 && (nf.NF_STATUS == "PEN" || nf.NF_STATUS == "ENV")
                                select nf.IPE_ID)
                             .Contains(ipe.IPE_ID) &&
                             !(from
                                    nfx in db.NFE_XML                              
                               select nfx.IPE_ID)
                             .Contains(ipe.IPE_ID) &&
                            (con.CTR_GERA_NOTA_FISCAL == true) &&
                            (con.CTR_CORTESIA == null || con.CTR_CORTESIA == 0)
                         select ped);
            }

            if (!string.IsNullOrWhiteSpace(nomeCliente))
            {
                query = query.Where(x => x.CLIENTES.CLI_NOME.Contains(nomeCliente));
            }

            if (!string.IsNullOrWhiteSpace(cpfCnpjCliente))
            {
                query = query.Where(x => x.CLIENTES.CLI_CPF_CNPJ.Contains(cpfCnpjCliente));
            }

            if (!string.IsNullOrWhiteSpace(assinatura))
            {
                query = (from
                            ped in query
                         join
                            cli in db.CLIENTES on ped.CLI_ID equals cli.CLI_ID
                         join
                            ass in db.ASSINATURA on cli.CLI_ID equals ass.CLI_ID
                         where ass.ASN_NUM_ASSINATURA == assinatura
                         select ped);
            }

            if(IPE_ID != null || PPI_ID != null)
            {
                query = (from 
                            ped in query join 
                            itm in db.ITEM_PEDIDO on ped.PED_CRM_ID equals itm.PED_CRM_ID
                         where 
                            (IPE_ID == null || itm.IPE_ID == IPE_ID) &&
                            (PPI_ID == null || itm.PPI_ID == PPI_ID)
                         select ped);
            }

            query = (from x in query
                     where
                        (PED_CRM_ID == null || x.PED_CRM_ID == PED_CRM_ID) &&
                        (REP_ID == null || x.REP_ID == REP_ID) &&
                        (CLI_ID == null || x.CLI_ID == CLI_ID) &&
                        (CMP_ID == null || x.CMP_ID == CMP_ID) &&
                        (dataInicial == null || EntityFunctions.TruncateTime(x.PED_CRM_DATA) >= EntityFunctions.TruncateTime(dataInicial)) &&
                        (dataFinal == null || EntityFunctions.TruncateTime(x.PED_CRM_DATA) <= EntityFunctions.TruncateTime(dataFinal)) &&
                        (UEN_ID == null || x.UEN_ID == UEN_ID) &&
                        (RG_ID == null || x.RG_ID == RG_ID) &&
                        (exibirExcluidos == true || (x.PST_ID == null || x.PST_ID != 5)) &&
                        ((aprovacaoPendente == null || aprovacaoPendente != true) || x.PST_ID == 6) &&
                        (PRT_ID == null || x.PRT_ID == PRT_ID) &&
                        (PST_ID == null || x.PST_ID == PST_ID) &&
                        (TNE_ID == null || x.TNE_ID == TNE_ID)
                     select x);

            return query;
        }

        public Pagina<PedidoCRMDTO> ListPedidoCRM(int? REP_ID, 
            int? CLI_ID, 
            int? CMP_ID,
            int pagina = 1, 
            int registrosPorPagina = 7)
        {
            var pesquisaDTO = new PesquisaPedidoDTO()
            {
                REP_ID = REP_ID,
                CLI_ID = CLI_ID,
                CMP_ID = CMP_ID,
            };
            var query = TemplatePedidoCRM(pesquisaDTO).OrderByDescending(x => x.PED_CRM_DATA).Where(s => s.PED_CRM_VENDA_INFORMADA == true);
            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public Pagina<PedidoCRMDTO> ListarPedidos(
            PesquisaPedidoDTO pesquisaDTO,
            int pagina = 1, 
            int registrosPorPagina = 7)
        {
            var query = TemplatePedidoCRM(pesquisaDTO)
                                            .Where(s => s.PED_CRM_VENDA_INFORMADA == false)
                                            .OrderByDescending(x => x.PED_CRM_DATA);
            
            return ToDTOPage(query, pagina, registrosPorPagina);           
        }

        public int? RetornarCliIdDoPedidoPorItemPedido(int? ipeId)
        {
            var query = (from itm in db.ITEM_PEDIDO where itm.IPE_ID == ipeId
                             select itm.PEDIDO_CRM.CLI_ID)
                             .FirstOrDefault();
            return query;
        }

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorStatus()
        {
            var query = (from ped in db.PEDIDO_CRM
                         where ped.PST_ID != null 
                         group ped by new
                         {
                             ped.PEDIDO_STATUS.PST_STATUS,
                             ped.PEDIDO_STATUS.PST_ID
                         } into grp
                         select new {

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

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorData()
        {
            DateTime dataAtual = DateTime.Now;
            DateTime? dataFinal = DateUtil.AdicionaMes(dataAtual, 1);

            var dataInicial = DateUtil.RetornaDiaPrimeiro(dataAtual);
            dataFinal = DateUtil.RetornaDateComUltimoDiaDoMes(dataFinal);


            var query = (from ped in db.PEDIDO_CRM
                         where
                         EntityFunctions.TruncateTime(ped.PED_CRM_DATA) >= EntityFunctions.TruncateTime(dataInicial) &&
                         EntityFunctions.TruncateTime(ped.PED_CRM_DATA) <= EntityFunctions.TruncateTime(dataFinal)
                         group ped by new
                         {
                             Data = EntityFunctions.TruncateTime(ped.PED_CRM_DATA.Value)
                         } into grp
                         select new 
                         {

                             Chave = "grupoDataPedido",
                             Label = grp.Key.Data,
                             Data = grp.Key.Data
                         }).ToList();


                        var resultado = (from grp in query
                             select new GrupoDeFiltroDTO()
                             {
                                 chave = grp.Chave,
                                 label = (grp.Label.HasValue) ? grp.Label.Value.ToString("dd/MM/yyyy") : null,
                                 valor = grp.Data
                             }).AsQueryable();
            return resultado;

        }

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorDataFaturamento()
        {
            DateTime dataAtual = DateTime.Now;
            DateTime? dataFinal = DateUtil.AdicionaMes(dataAtual, 4);

            var dataInicial = DateUtil.RetornaDiaPrimeiro(dataAtual);
            dataFinal = DateUtil.RetornaDateComUltimoDiaDoMes(dataFinal);


            var query = (from itm in db.ITEM_PEDIDO
                         where
                         EntityFunctions.TruncateTime(itm.IPE_DATA_FATURAMENTO) >= EntityFunctions.TruncateTime(dataInicial) &&
                         EntityFunctions.TruncateTime(itm.IPE_DATA_FATURAMENTO) <= EntityFunctions.TruncateTime(dataFinal)
                         group itm by new
                         {
                             Data = EntityFunctions.TruncateTime(itm.IPE_DATA_FATURAMENTO)

                         } into grp
                         select new 
                         {

                             Chave = "grupoDataPedidoFaturamento",
                             Label = grp.Key.Data,
                             Count = grp.Count(),
                             Data = grp.Key.Data
                         }).ToList();

            var resultado = (from grp in query
                             select new GrupoDeFiltroDTO()
                             {
                                 chave = grp.Chave,
                                 label = (grp.Label.HasValue) ? grp.Label.Value.ToString("dd/MM/yyyy") : null,
                                 Count = grp.Count,
                                 valor = grp.Data
                             }).AsQueryable();
            return resultado;
        }

        public IList<AutoCompleteDTO> ListarAssinaturaDoPedidoAutoComplete(string assinatura)
        {
            if (!string.IsNullOrEmpty(assinatura)&& assinatura.Count() > 4)
            {
                var query = (
                            from
                                itm in db.ITEM_PEDIDO
                            where itm.ASN_NUM_ASSINATURA.Contains(assinatura)
                            select new AutoCompleteDTO() {

                                label = itm.ASN_NUM_ASSINATURA,
                                value = itm.ASN_NUM_ASSINATURA
                            });

                return query.ToList();

            }
            return new List<AutoCompleteDTO>();
        }

        public bool PropostaPossuiPedido(int? prtID)
        {
            var query = (from ped in db.PEDIDO_CRM
                         where ped.PRT_ID == prtID
                         select ped);
            return (query.Count() > 0);
        }
    }
}

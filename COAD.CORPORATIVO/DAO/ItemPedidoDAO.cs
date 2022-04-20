using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Extensions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COAD.CORPORATIVO.DAO
{
    public class ItemPedidoDAO : DAOAdapter<ITEM_PEDIDO, ItemPedidoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ItemPedidoDAO()
        {            
            db = GetDb<COADCORPEntities>();
        }

        public ItemPedidoDTO BuscarPedidoPorTransacao(string _orderkey)
        {
            var query = GetDbSet().Where(x => x.ORDER_KEY == _orderkey).FirstOrDefault();
            return ToDTO(query);
        }

        public IList<ItemPedidoDTO> ListarItemPedidoDoPedido(int? pedCrmId)
        {
            var query = GetDbSet().Where(x => x.PED_CRM_ID == pedCrmId);
            return ToDTO(query);
        }

        public int? ChecaStatusItensIguais(int? PED_CRM_ID)
        {
            var status = (from subItem in db.ITEM_PEDIDO
                                      where subItem.PED_CRM_ID == PED_CRM_ID
                                      group subItem.PST_ID by subItem.PST_ID
                                          into grp
                                          select new CountDTO() { 
                                              Label = SqlFunctions.StringConvert((double) grp.Key.Value).Trim(),
                                              Value = grp.Count()
                                          }
                             );

            var count = status.Count();

            if(count == 1)
            {
                return int.Parse(status.Select(x => x.Label).FirstOrDefault());
            }

            return null;
            
        }

        public bool ChecaSeExisteStatus(int? pedidoCRMId, int? statusId)
        {
            var count= (from subItem in db.ITEM_PEDIDO
                          where subItem.PED_CRM_ID == pedidoCRMId &&
                                subItem.PST_ID == statusId
                                 select subItem).Count();

            return (count > 0);
        }

        public string RetormaPathDaNotaFiscalDoItemPedido(int? ipeId)
        {
            var query = (from itm in db.ITEM_PEDIDO 
                         where itm.IPE_ID == ipeId 
                         select itm.IPE_PATH_NFE).FirstOrDefault();

            return query;
        }

        public RequisicaoPagamentoDTO RetornarDadosDePagamento(int? ipeId)
        {
            var query = (from itm in db.ITEM_PEDIDO
                         where itm.IPE_ID == ipeId
                         select new {
                             itemPedido = itm,
                             cliId = itm.PEDIDO_CRM.CLIENTES.CLI_ID,
                             cliente = itm.PEDIDO_CRM.CLIENTES,
                             endereco = itm.PEDIDO_CRM.CLIENTES.CLIENTES_ENDERECO.OrderByDescending(or => or.END_TIPO).FirstOrDefault(),
                             Recorrente = itm.TIPO_PERIODO.TTP_RECORRENTE,
                             pedidoPagamentoEntrada = itm.
                                ITEM_PEDIDO_PEDIDO_PAGAMENTO.Where(x => 
                                    (from itemPedPedPag in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                                            where itemPedPedPag.IPE_ID == ipeId
                                            select itemPedPedPag.PEDIDO_PAGAMENTO).Count() == 1

                                    || x.PEDIDO_PAGAMENTO.PGT_ENTRADA == true).FirstOrDefault().PEDIDO_PAGAMENTO,
                                pedidoPagamentoRestante = itm.ITEM_PEDIDO_PEDIDO_PAGAMENTO.Where(x => 
                            
                                  x.PEDIDO_PAGAMENTO.PGT_ENTRADA == null ||
                                  x.PEDIDO_PAGAMENTO.PGT_ENTRADA == false).FirstOrDefault().PEDIDO_PAGAMENTO
                         }).FirstOrDefault();

            var requisicaoPagamento = new RequisicaoPagamentoDTO(){
                
                ITEM_PEDIDO = ToDTO(query.itemPedido),
                CLIENTE = Convert<CLIENTES, ClienteDto>(query.cliente),
                CLIENTE_ENDERECO = Convert<CLIENTES_ENDERECO, ClienteEnderecoDto>(query.endereco),
                ENTRADA = Convert<PEDIDO_PAGAMENTO, PedidoPagamentoDTO>(query.pedidoPagamentoEntrada),
                PAGAMENTO_RESTANTE = Convert<PEDIDO_PAGAMENTO, PedidoPagamentoDTO>(query.pedidoPagamentoRestante),
                CLI_ID = query.cliId,
                Recorrente = query.Recorrente
            };

            return requisicaoPagamento;
        }

        public IList<ItemPedidoDTO> ListarItemPedidoDosPedidosFaturado(DateTime? dataFaturamentoAgendado)
        {
            var query = (from con in db.CONTRATOS
                         where 
                         con.IPE_ID >= 433 && 
                         con.CTR_DATA_CANC == null &&
                         (con.ITEM_PEDIDO.IPE_CORTESIA == null || con.ITEM_PEDIDO.IPE_CORTESIA == false) &&
                            con.ITEM_PEDIDO.PST_ID == 3 &&
                            !(from nfe in db.NFE_XML where
                                (nfe.NFX_DATA_EXCLUSAO == null
                                    || nfe.NFX_NUM_EXTORNADO == null
                                    || nfe.NFX_NUM_EXTORNADO == false)
                              select nfe.IPE_ID)
                            .Contains(con.IPE_ID) &&
                            (con.CTR_CORTESIA == null || con.CTR_CORTESIA == 0)
                            orderby con.IPE_ID ascending, con.CTR_DATA_FAT ascending
                         select con.ITEM_PEDIDO);

            return ToDTO(query);
        }

        private IQueryable<PedidosRetroativosSemNotaDTO> TemplatePedidosFaturadosSemNotaFiscalGerada(DateTime? dataFaturamento, int? empId, int? ipeIdParExcluir = null)
        {
            var query = (from 
                            ped in db.PEDIDO_CRM join cli in db.CLIENTES on ped.CLI_ID equals cli.CLI_ID join
                            itm in db.ITEM_PEDIDO on ped.PED_CRM_ID equals itm.PED_CRM_ID join
                            con in db.CONTRATOS on itm.IPE_ID equals con.IPE_ID
                         where
                            itm.PST_ID == 3 &&
                            con.EMP_ID == empId &&
                            (ipeIdParExcluir == null || itm.IPE_ID != ipeIdParExcluir) &&
                            (con.CTR_GERA_NOTA_FISCAL == null || con.CTR_GERA_NOTA_FISCAL == true) &&
                            (con.CTR_CORTESIA == null || con.CTR_CORTESIA == 0) &&
                            con.CTR_NUMERO_NOTA == null &&
                            EntityFunctions.TruncateTime(con.CTR_DATA_FAT) < EntityFunctions.TruncateTime(dataFaturamento)
                         select
                         new PedidosRetroativosSemNotaDTO()
                         {
                             CodigoItemPedido = itm.IPE_ID,
                             CodigoPedido = ped.PED_CRM_ID,
                             NomeProduto = itm.PRODUTO_COMPOSICAO.CMP_DESCRICAO,
                             CodigoCliente = cli.CLI_ID,
                             NomeCliente = cli.CLI_NOME,
                             CNPJ_CPF_Cliente = cli.CLI_CPF_CNPJ,
                             NomeRepresentante = ped.REPRESENTANTE.REP_NOME,
                             CarId = ped.CAR_ID,
                             DataEmissaoPedido = ped.PED_CRM_DATA,
                             DataFaturamento = con.CTR_DATA_FAT,
                             TotalDoPedido = itm.IPE_TOTAL
                         });

            return query;
        }

        /// <summary>
        /// Retorna a quantidade de pedidos que foram faturadas, deveriam ter notas geradas, 
        /// mas inda não possuem nota até no máximo a data informada.
        /// Esse método é útil para checar se a numeração da nota não irá ser gerada ignorando contratos anteriores sem nota gerada.
        /// </summary>
        /// <param name="ateDataFaturamento">Pesquisa pedidos faturadas até essa data.</param>
        /// <returns></returns>
        public int? RetornarQtdPedidosNotaNaoGeradaPorData(DateTime? ateDataFaturamento, int? empId, int? ipeIdParaExcluir = null)
        {
            var query = TemplatePedidosFaturadosSemNotaFiscalGerada(ateDataFaturamento, empId, ipeIdParaExcluir).Count();
            return query;
        }

        /// <summary>
        /// Retorna os pedidos que foram faturadas, deveriam ter notas geradas, 
        /// mas inda não possuem nota até no máximo a data informada.
        /// Esse método é útil para checar quais pedidos deveria ter suas notas geradas antes de gerar com a data informada.
        /// </summary>
        /// <param name="ateDataFaturamento">Pesquisa pedidos faturadas até essa data.</param>
        /// <returns></returns>
        public Pagina<PedidosRetroativosSemNotaDTO> RetornarPedidosNotaNaoGeradaPorData(DateTime? ateDataFaturamento, int? empId, int? ipeIdParaExcluir = null, int pagina = 1, int registrosPorPagina = 15)
        {
            var query = TemplatePedidosFaturadosSemNotaFiscalGerada(ateDataFaturamento, empId, ipeIdParaExcluir);
            return query.Paginar(pagina, registrosPorPagina);
        }

        public ItemPedidoDTO ListarItemPedidoDaAssinatura(string assinatura)
        {
            var query = (from itm 
                            in db.ITEM_PEDIDO
                         where itm.ASN_NUM_ASSINATURA == assinatura
                         select itm).FirstOrDefault();

            return ToDTO(query);
        }
  
    }
}

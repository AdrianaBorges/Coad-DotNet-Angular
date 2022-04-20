using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;

namespace COAD.CORPORATIVO.DAO
{
    public class PedidoPagamentoDAO : DAOAdapter<PEDIDO_PAGAMENTO, PedidoPagamentoDTO>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public PedidoPagamentoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public PedidoPagamentoDTO RetornarTipoPagamentoDeEntrada(int? IPE_ID)
        {
            var query = (from x in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                         where x.IPE_ID == IPE_ID
                                &&
                                (
                                    (from itemPedPedPag in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                                        where itemPedPedPag.IPE_ID == IPE_ID
                                        select itemPedPedPag.PEDIDO_PAGAMENTO).Count() == 1
                                
                                    || x.PEDIDO_PAGAMENTO.PGT_ENTRADA == true
                                )
                         select x.PEDIDO_PAGAMENTO).FirstOrDefault();
            return ToDTO(query);

        }

        public PedidoPagamentoDTO RetornarTipoPagamentoTirandoAEntrada(int? IPE_ID)
        {
            var query = (from x in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                         where x.IPE_ID == IPE_ID &&
                         (
                             x.PEDIDO_PAGAMENTO.PGT_ENTRADA == null || 
                             x.PEDIDO_PAGAMENTO.PGT_ENTRADA == false
                          )   
                         select x.PEDIDO_PAGAMENTO).FirstOrDefault();
            return ToDTO(query);

        }

        public IList<PedidoPagamentoDTO> ListarPedidoPagamentoPorItem(int? IPE_ID)
        {
            var query = (from x in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                         where x.IPE_ID == IPE_ID                                
                         select x.PEDIDO_PAGAMENTO);
            return ToDTO(query);

        }

        public int? RetornarCodigoTipoPagamentoDeEntrada(int? ipe)
        {
            var query = (from x in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                             where x.IPE_ID == ipe &&
                                (
                                    (from itemPedPedPag in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                                        where itemPedPedPag.IPE_ID == ipe
                                        select itemPedPedPag.PEDIDO_PAGAMENTO).Count() == 1
                                
                                    || x.PEDIDO_PAGAMENTO.PGT_ENTRADA == true
                                )
                             select x.PEDIDO_PAGAMENTO.TPG_ID)
                             .FirstOrDefault();
            return query;
        }

        public int? RetornarCodigoTipoPagamentoTirandoEntrada(int? ipe)
        {
            var query = (from x in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                         where x.IPE_ID == ipe &&
                         (
                             x.PEDIDO_PAGAMENTO.PGT_ENTRADA == null ||
                             x.PEDIDO_PAGAMENTO.PGT_ENTRADA == false
                          )
                         select x.PEDIDO_PAGAMENTO.TPG_ID)
                             .FirstOrDefault();
            return query;
        }

        public int? RetornarCodigoPedidoPagamentoDeEntrada(int? ipe)
        {
            var query = (from x in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                         where x.IPE_ID == ipe &&
                            (
                                (from itemPedPedPag in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                                 where itemPedPedPag.IPE_ID == ipe
                                 select itemPedPedPag.PEDIDO_PAGAMENTO).Count() == 1

                                || x.PEDIDO_PAGAMENTO.PGT_ENTRADA == true
                            )
                         select x.PEDIDO_PAGAMENTO.PGT_ID)
                             .FirstOrDefault();
            return query;
        }

        public int? RetornarCodigoPedidoPagamentoTirandoEntrada(int? ipe)
        {
            var query = (from x in db.ITEM_PEDIDO_PEDIDO_PAGAMENTO
                         where x.IPE_ID == ipe &&
                         (
                             x.PEDIDO_PAGAMENTO.PGT_ENTRADA == null ||
                             x.PEDIDO_PAGAMENTO.PGT_ENTRADA == false
                          )
                         select x.PEDIDO_PAGAMENTO.PGT_ID)
                             .FirstOrDefault();
            return query;
        }
    }
}

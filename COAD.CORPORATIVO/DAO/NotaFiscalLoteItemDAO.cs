

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Models.Filtros;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COAD.CORPORATIVO.DAO
{
    public class NotaFiscalLoteItemDAO : AbstractGenericDao<NOTA_FISCAL_LOTE_ITEM, NotaFiscalLoteItemDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public NotaFiscalLoteItemDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLote(int? nflID)
        {
            var query = (from ntItm in db.NOTA_FISCAL_LOTE_ITEM
                         where ntItm.NFL_ID == nflID
                         select ntItm);

            return ToDTO(query);
        }

        public NotaFiscalLoteItemDTO ListarItemLoteAutorizado(int? nflID)
        {
            var query = (from ntItm in db.NOTA_FISCAL_LOTE_ITEM
                         where 
                            ntItm.NF_ID == nflID &&
                            (ntItm.NLS_ID == 6 || ntItm.NLS_ID == 11)
                         select ntItm);

            return ToDTO(query.FirstOrDefault());
        }


        public IList<NotaFiscalLoteItemDTO> ListarItensDoLotePorPedidoItem(int? ipeID)
        {
            var query = (from
                            ntItm in db.NOTA_FISCAL_LOTE_ITEM join
                            nfl in db.NOTA_FISCAL_LOTE on ntItm.NFL_ID equals nfl.NFL_ID
                         where 
                            ntItm.IPE_ID == ipeID &&
                            (nfl.NLT_ID == 1 || nfl.NLT_ID == 2)
                         orderby ntItm.NLI_ID descending
                         select ntItm);

            return ToDTO(query);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteServicoPorPedidoItem(int? ipeID)
        {
            var query = (from
                            ntItm in db.NOTA_FISCAL_LOTE_ITEM join
                            nfl in db.NOTA_FISCAL_LOTE on ntItm.NFL_ID equals nfl.NFL_ID
                         where 
                            ntItm.IPE_ID == ipeID &&
                            (nfl.NLT_ID == 3)
                         orderby ntItm.NLI_ID descending
                         select ntItm);

            return ToDTO(query);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLotePorPropostaItem(int? ppiID)
        {
            var query = (from
                            ntItm in db.NOTA_FISCAL_LOTE_ITEM join
                            nfl in db.NOTA_FISCAL_LOTE on ntItm.NFL_ID equals nfl.NFL_ID
                         where
                            ntItm.PPI_ID == ppiID &&
                            (nfl.NLT_ID == 1 || nfl.NLT_ID == 2)
                         orderby ntItm.NLI_ID descending
                         select ntItm);

            return ToDTO(query);
        }

        
        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteServicoPorPropostaItem(int? ppiID)
        {
            var query = (from
                            ntItm in db.NOTA_FISCAL_LOTE_ITEM join
                            nfl in db.NOTA_FISCAL_LOTE on ntItm.NFL_ID equals nfl.NFL_ID
                         where
                            ntItm.PPI_ID == ppiID &&
                            (nfl.NLT_ID == 3)
                         orderby ntItm.NLI_ID descending
                         select ntItm);

            return ToDTO(query);
        }

        public NotaFiscalLoteItemDTO ListarItensPorChave(string chaveNota)
        {
            var query = (from ntItm in db.NOTA_FISCAL_LOTE_ITEM
                         where ntItm.NLI_CHAVE_NOTA == chaveNota &&
                         ntItm.NOTA_FISCAL_LOTE.NLT_ID == 1
                         orderby ntItm.NLI_ID descending
                         select ntItm).FirstOrDefault();

            return ToDTO(query);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteDaNota(int? nfID)
        {
            var query = (from ntItm in db.NOTA_FISCAL_LOTE_ITEM join
                            nfl in db.NOTA_FISCAL_LOTE on ntItm.NFL_ID equals nfl.NFL_ID
                         where 
                            ntItm.NF_ID == nfID &&
                            (nfl.NLT_ID == 1 || nfl.NLT_ID == 2)
                         orderby ntItm.NLI_ID descending
                         select ntItm);

            return ToDTO(query);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteDaNotaService(int? nfID)
        {
            var query = (from ntItm in db.NOTA_FISCAL_LOTE_ITEM join
                            nfl in db.NOTA_FISCAL_LOTE on ntItm.NFL_ID equals nfl.NFL_ID
                         where 
                            ntItm.NF_ID == nfID &&
                            nfl.NLT_ID == 3
                         orderby ntItm.NLI_ID descending
                         select ntItm);

            return ToDTO(query);
        }

        /// <summary>
        /// Lista itens de lote que ainda não foram processados.
        /// </summary>
        /// <param name="ppiID"></param>
        /// <returns></returns>
        public IList<NotaFiscalLoteItemDTO> ListarItensDoLotePendentePorPropostaItem(int? ppiID)
        {
            var query = (from 
                            ltItm in db.NOTA_FISCAL_LOTE_ITEM join
                            lote in db.NOTA_FISCAL_LOTE on ltItm.NFL_ID equals lote.NFL_ID
                         where 
                            ltItm.PPI_ID == ppiID &&
                            (   ltItm.NLS_ID == 1 || 
                                ltItm.NLS_ID == 4 ||
                                ltItm.NLS_ID == 9
                            ) 
                            &&
                            (
                                lote.NLS_ID == 1 ||
                                lote.NLS_ID == 8 ||
                                lote.NLS_ID == 9
                            )                            
                         orderby ltItm.NLI_ID descending
                         select ltItm);

            return ToDTO(query);
        }

        /// <summary>
        /// Lista itens de lote de nota fiscais que ainda não foram processados.
        /// </summary>
        /// <param name="ipeID"></param>
        /// <returns></returns>
        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteNFePendentePorItemPedido(int? ipeID, int? nfcId)
        {
            var query = (from 
                            ltItm in db.NOTA_FISCAL_LOTE_ITEM join
                            lote in db.NOTA_FISCAL_LOTE on ltItm.NFL_ID equals lote.NFL_ID
                         where 
                            (lote.NLT_ID == 1 || lote.NLT_ID == 2) &&
                            ltItm.IPE_ID == ipeID &&
                            (   ltItm.NLS_ID == 1 || 
                                ltItm.NLS_ID == 4 ||
                                ltItm.NLS_ID == 9
                            ) 
                            &&
                            (
                                lote.NLS_ID == 1 ||
                                lote.NLS_ID == 8 ||
                                lote.NLS_ID == 9
                            )
                            && ltItm.NFC_ID == nfcId
                         orderby ltItm.NLI_ID descending
                         select ltItm);

            return ToDTO(query);
        }

        /// <summary>
        /// Lista itens de lote de nota fiscais de serviço que ainda não foram processados.
        /// </summary>
        /// <param name="ipeID"></param>
        /// <returns></returns>
        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteNFsePendentePorItemPedido(int? ipeID, int? nfcId)
        {
            var query = (from 
                            ltItm in db.NOTA_FISCAL_LOTE_ITEM join
                            lote in db.NOTA_FISCAL_LOTE on ltItm.NFL_ID equals lote.NFL_ID
                         where 
                            (lote.NLT_ID == 3) &&
                            ltItm.IPE_ID == ipeID &&
                            (   ltItm.NLS_ID == 1 || 
                                ltItm.NLS_ID == 4 ||
                                ltItm.NLS_ID == 9
                            ) 
                            &&
                            (
                                lote.NLS_ID == 1 ||
                                lote.NLS_ID == 8 ||
                                lote.NLS_ID == 9
                            ) 
                            && ltItm.NFC_ID == nfcId
                         orderby ltItm.NLI_ID descending
                         select ltItm);

            return ToDTO(query);
        }

        /// <summary>
        /// Lista itens de lote de notas fiscais que ainda não foram processados ou com erro.
        /// </summary>
        /// <param name="ipeID"></param>
        /// <returns></returns>
        public Pagina<NotaFiscalLoteItemDTO> ListarNfeLoteItmComErroOuPendente(RequisicaoPaginacao requisicao)
        {
            var query = (from
                            ltItm in db.NOTA_FISCAL_LOTE_ITEM join
                            lote in db.NOTA_FISCAL_LOTE on ltItm.NFL_ID equals lote.NFL_ID
                         where
                            (ltItm.NLS_ID != 3 &&
                                ltItm.NLS_ID != 6 &&
                                ltItm.NLS_ID != 7 &&
                                ltItm.NLS_ID != 11 &&
                                ltItm.NLS_ID != 12
                            )
                            &&
                            (
                                lote.NLS_ID != 3 &&
                                lote.NLS_ID != 6 &&
                                lote.NLS_ID != 7 &&
                                lote.NLS_ID != 11 &&
                                lote.NLS_ID != 12
                            )
                            &&
                            ltItm.IPE_ID != null &&
                            (
                                from itm in db.NOTA_FISCAL_LOTE_ITEM
                                where itm.IPE_ID == ltItm.IPE_ID
                                group itm by itm.IPE_ID into g
                                select new 
                                {
                                    IPE_ID = g.Key,
                                    MAX_LOTE_ID = g.Max(x => x.NLI_ID)  
                                }
                            ).FirstOrDefault().MAX_LOTE_ID == ltItm.NLI_ID                            

                         orderby ltItm.NLI_ID descending
                         select ltItm);

            return ToDTOPage(query, requisicao);
        }

        public ICollection<NotaFiscalLoteItemDTO> ListarItens(ICollection<int> LstCodLoteitem)
        {
            if(LstCodLoteitem != null)
            {
                var query = (from
                                ltItm in db.NOTA_FISCAL_LOTE_ITEM
                             where
                                LstCodLoteitem.Contains(ltItm.NLI_ID)
                             select ltItm);
                return ToDTO(query);
            }
            return new List<NotaFiscalLoteItemDTO>();
        }
    }
}

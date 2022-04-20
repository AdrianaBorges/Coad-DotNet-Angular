

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Model.Custons;
using GenericCrud.Models.Filtros;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("NLI_ID")]
	public class NotaFiscalLoteItemSRV : GenericService<NOTA_FISCAL_LOTE_ITEM, NotaFiscalLoteItemDTO, Int32>
	{

        public NotaFiscalLoteItemDAO _dao { get; set; }

        public NotaFiscalLoteItemSRV(NotaFiscalLoteItemDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLote(int? nflID)
        {
            return _dao.ListarItensDoLote(nflID);
        }

        public NotaFiscalLoteItemDTO ListarItemLoteAutorizado(int? nflID)
        {
            return _dao.ListarItemLoteAutorizado(nflID);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLotePorPedidoItem(int? ipeID)
        {
            return _dao.ListarItensDoLotePorPedidoItem(ipeID);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteServicoPorPedidoItem(int? ipeID)
        {
            return _dao.ListarItensDoLoteServicoPorPedidoItem(ipeID);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLotePorPropostaItem(int? ipeID)
        {
            return _dao.ListarItensDoLotePorPropostaItem(ipeID);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteServicoPorPropostaItem(int? ipeID)
        {
            return _dao.ListarItensDoLoteServicoPorPropostaItem(ipeID);
        }

        public NotaFiscalLoteItemDTO ListarItensPorChave(string chaveNota)
        {
            return _dao.ListarItensPorChave(chaveNota);
        }

        public void AssociarLoteProPedido(int? ppiId, int? ipeId)
        {
            if (ppiId != null && ipeId != null)
            {
                var lstLotesItens = ListarItensDoLotePorPropostaItem(ppiId);
                foreach (var comp in lstLotesItens)
                {
                    comp.IPE_ID = ipeId;
                }

                SaveOrUpdateAll(lstLotesItens);
            }
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteDaNota(int? nfID)
        {
            return _dao.ListarItensDoLoteDaNota(nfID);
        }

        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteDaNotaService(int? nfID)
        {
            return _dao.ListarItensDoLoteDaNotaService(nfID);
        }

        /// <summary>
        /// Lista itens de lote que ainda não foram processados.
        /// </summary>
        /// <param name="ppiID"></param>
        /// <returns></returns>
        public IList<NotaFiscalLoteItemDTO> ListarItensDoLotePendentePorPropostaItem(int? ppiID)
        {
            return _dao.ListarItensDoLotePendentePorPropostaItem(ppiID);
        }

        /// <summary>
        /// Lista itens de lote de nota fiscais que ainda não foram processados.
        /// </summary>
        /// <param name="ipeID"></param>
        /// <returns></returns>
        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteNFePendentePorItemPedido(int? ipeID, int? nfcId)
        {
            return _dao.ListarItensDoLoteNFePendentePorItemPedido(ipeID, nfcId);
        }

        public FileInfoDTO RetornarArquivoDaNota(int? ntLoteItemID)
        {
            var loteItem = FindById(ntLoteItemID);

            if (loteItem != null && loteItem.BinarioNFeXml != null && !string.IsNullOrWhiteSpace(loteItem.PathArquivoNFeXml))
            {
                FileInfoDTO info = new FileInfoDTO();
                info.Bytes = loteItem.BinarioNFeXml;
                info.Path = loteItem.PathArquivoNFeXml;

                return info;
            }

            return null;
        }

        /// <summary>
        /// Lista itens de lote de nota fiscais de serviço que ainda não foram processados.
        /// </summary>
        /// <param name="ipeID"></param>
        /// <returns></returns>
        public IList<NotaFiscalLoteItemDTO> ListarItensDoLoteNFsePendentePorItemPedido(int? ipeID, int? nfcId)
        {
            return _dao.ListarItensDoLoteNFsePendentePorItemPedido(ipeID, nfcId);
        }

        public Pagina<NotaFiscalLoteItemDTO> ListarNfeLoteItmComErroOuPendente(RequisicaoPaginacao requisicao)
        {
            return _dao.ListarNfeLoteItmComErroOuPendente(requisicao);
        }

        public void CancelarProcessamentoItem(ICollection<int> lstNliId)
        {

            if (lstNliId != null)
            {
                var lstItens = new List<NotaFiscalLoteItemDTO>();
                using (var scope = new TransactionScope())
                {

                    foreach (var nliId in lstNliId)
                    {
                        var item = FindById(nliId);

                        if (item != null)
                        {
                            item.NLS_ID = 12;
                            item.NOTA_FISCAL_LOTE.NLS_ID = 12;
                            lstItens.Add(item);
                        }
                    }
                    MergeAll(lstItens);

                    ServiceFactory.RetornarServico<NotaFiscalLoteSRV>()
                        .SalvarLoteDosItens(lstItens);

                    scope.Complete();
                }
            }
        }

        public ICollection<NotaFiscalLoteItemDTO> ListarItens(ICollection<int> LstCodLoteitem)
        {
            return _dao.ListarItens(LstCodLoteitem);
        }
        public void PreencherItensNoLote(NotaFiscalLoteDTO lote)
        {
            if(lote != null)
            {
                lote.NOTA_FISCAL_LOTE_ITEM = ListarItensDoLote(lote.NFL_ID);
            }
        }

    }
}

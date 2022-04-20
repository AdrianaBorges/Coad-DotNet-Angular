using COAD.CORPORATIVO.Model.Dto;
using COAD.FISCAL.Service.Integracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service.Custons
{
    public class LoteItemNFeSRVImpl : IntegrLoteItemNFeSRV<NotaFiscalLoteItemDTO>
    {
        public NotaFiscalLoteItemSRV NotaFiscalLoteItem { get; set; }

        public override IList<INFeLoteItem> RetornarItensDoLote(int? LoteID)
        {
            var result = NotaFiscalLoteItem.ListarItensDoLote(LoteID);
            if (result != null)
                return result.Cast<INFeLoteItem>().ToList();
            return new List<INFeLoteItem>();
        }
        

        public override INFeLoteItem RetornarLoteItem(int? ItemLoteID)
        {
            return NotaFiscalLoteItem.FindById(ItemLoteID);
        }

        public override INFeLoteItem RetornarLoteItemPorChave(string ChaveNota)
        {
            return NotaFiscalLoteItem.ListarItensPorChave(ChaveNota);
        }

        public override INFeLoteItem SalvarOuAtualizarLoteItem(INFeLoteItem Item)
        {
            return NotaFiscalLoteItem.SaveOrUpdate((NotaFiscalLoteItemDTO) Item);
        }

        public override ICollection<INFeLoteItem> SalvarOuAtualizarLoteItens(ICollection<INFeLoteItem> Itens)
        {
           var lstLoteItem = NotaFiscalLoteItem.SaveOrUpdateAll(Itens.Cast<NotaFiscalLoteItemDTO>());

            if(lstLoteItem != null)
            {
                var service = ServiceFactory.RetornarServico<NotaFiscalLoteItemMsgSRV>();
                foreach(var item in lstLoteItem)
                {
                    service.SalvarNotaFiscalLoteItemMsg(item);
                }
            }

            if (lstLoteItem != null)
                return lstLoteItem.Cast<INFeLoteItem>().ToList();

            return new List<INFeLoteItem>();
        }

        public override INotaFiscalReferenciada CriarNotaNotaReferenciada()
        {
            return new NotaFiscalReferenciadaDTO();
        }

        public override ICollection<INotaFiscalReferenciada> SalvarNotaFiscalReferenciadas(ICollection<INotaFiscalReferenciada> notasFiscaisReferenciadas)
        {
            if(notasFiscaisReferenciadas != null)
            {
                var notasFiscaisRef = notasFiscaisReferenciadas.Cast<NotaFiscalReferenciadaDTO>();
                var retorno = ServiceFactory.RetornarServico<NotaFiscalReferenciadaSRV>().SaveOrUpdateAll(notasFiscaisRef);
                return retorno.Cast<INotaFiscalReferenciada>().ToList();
            }
            return new List<INotaFiscalReferenciada>();
        }

        public override INFeLoteItem RetornarLoteItemDaNotaAutorizada(int? CodNotaFiscal)
        {
            return NotaFiscalLoteItem.ListarItemLoteAutorizado(CodNotaFiscal);
        }
    }
}

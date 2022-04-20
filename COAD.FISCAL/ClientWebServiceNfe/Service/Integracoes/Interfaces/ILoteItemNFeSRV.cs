using COAD.FISCAL.Model;
using COAD.FISCAL.Model.DTO;
using COAD.FISCAL.Model.Integracoes;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.NFSe;
using COAD.FISCAL.Model.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Service.Integracoes.Interfaces
{
    public interface ILoteItemNFeSRV
    {

        /// <summary>
        /// Extrai do DTO de nota Fiscal a Chave e o número da nota fiscal.
        /// Esse método não Salva no Banco.
        /// </summary>
        /// <param name="loteItem"></param>
        /// <param name="NFe"></param>
        void InserirChaveENumeroDaNota(INFeLoteItem loteItem, NotaFiscal NFe);

        void InserirChaveENumeroDaNotaFiscalServico(INFeLoteItem loteItem, Rps rps);

        ICollection<INFeLoteItem> SalvarOuAtualizarLoteItens(ICollection<INFeLoteItem> Itens);
        INFeLoteItem SalvarOuAtualizarLoteItem(INFeLoteItem Item);
        INFeLoteItem RetornarLoteItem(int? ItemLoteID);
        ICollection<INotaFiscalReferenciada> SalvarNotaFiscalReferenciadas(ICollection<INotaFiscalReferenciada> notasFiscaisReferenciadas);
        INFeLoteItem RetornarLoteItemDaNotaAutorizada(int? CodNotaFiscal);
    }
}

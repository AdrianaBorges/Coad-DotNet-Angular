using COAD.FISCAL.Model.Integracoes.Enumerados;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes.Interfaces
{
    public interface INFeLoteItem
    {
        int? LoteID { get; set; }
        int? ItemLoteID { get; set; }
        StatusLoteItemEnum Status { get; set; }
        TipoLoteItemEnum Tipo { get; set; }
        INFeLote Lote { get; set; }

        int? CodPedido { get; set; }
        int? NfConfigID { get; set; }
        int? CodProposta { get; set; }
        string CodContrato { get; set; }
        Nullable<int> ClienteID { get; set; }
        Nullable<int> EmpresaID { get; set; }
        Nullable<int> FornecedorID { get; set; }
        DateTime? DataFaturamento { get; set; }
        DateTime? DataEmissao { get; set; }
        int? NumeroNota { get; set; }
        string ChaveNota { get; set; }
        Nullable<int> CodRetorno { get; set; }
        Nullable<int> CodNotaFiscal { get; set; }
        string MensagemRetorno { get; set; }
        string PathArquivoNFeXml { get; set; }
        byte[] BinarioNFeXml { get; set; }
        Nullable<int> NumeroRps { get; set; }

        string MsgErroSistema { get; set; }
        string NumeroProtocolo { get; set; }
        string CartaCorrecao { get; set; }
        Nullable<System.DateTime> DataAutorizacaoRejeicao { get; set; }
        ICollection<INotaFiscalReferenciada> NotaFiscalReferenciados { get; set; }
        ICollection<INotaFiscalItemMSG> NotaFiscalLoteItemMSG { get; set; }
        Nullable<bool> NotaAntecipada { get; set; }
        string Serie { get; set; }


    }
}

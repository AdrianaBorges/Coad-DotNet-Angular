using COAD.FISCAL.Model.Integracoes.Enumerados;
using System;

namespace COAD.FISCAL.Model.Integracoes.Interfaces
{
    public interface INotaFiscalEventoDTO
    {
        int? EventoID { get; set; }
        int? NotaFiscalID { get; set; }
        TipoLoteItemEnum Tipo { get; set; }
        string CNPJ { get; set; }
        string CondicaoUso { get; set; }
        DateTime? Data { get; set; }
        string DescCartaCorrecao { get; set; }
        string EventoXMLID { get; set; }
        string DescJustificativa { get; set; }
        string NumeroProtocolo { get; set; }
        int? CodRetorno { get; set; }
        string DescRetorno { get; set; }
        string ArquivoNome { get; set; }
        byte[] Arquivo { get; set; }

    }
}
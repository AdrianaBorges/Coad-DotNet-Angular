using COAD.FISCAL.Model.Integracoes.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes.Interfaces
{
    public interface INotaFiscal
    {
        int CodNotaFiscal { get; set; }
        int NumeroNota { get; set; }
        int? CodPedido { get; set; }
        int? CodProposta { get; set; }
        string CodContrato { get; set; }
        string ChaveNota { get; set; }
        string ProtocoloAutorizacao { get; set; }
        StatusNotaFiscalEnum Status { get; set; }
        int? CodEmpresa { get; set; }
        int? CodCliente { get; set; }
        string NomeArquivo { get; set; }
        byte[] Arquivo { get; set; }
        string NomeArquivoEvento { get; set; }
        byte[] ArquivoEvento { get; set; }
        bool? EventoAnexado { get; set; }
        Nullable<bool> NotaAntecipada { get; set; }
        TipoNFEnum Tipo { get; set; }
        string CodVerificacao { get; set; }

    }
}

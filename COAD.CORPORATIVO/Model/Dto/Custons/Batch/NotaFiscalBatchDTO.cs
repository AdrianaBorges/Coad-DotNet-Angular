using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Batch
{
    public class NotaFiscalBatchDTO
    {
        public NotaFiscalBatchDTO()
        {
            this.ListCodPedidos = new List<NotaFiscaBatchItemDTO>();
        }

        public string Path { get; set; }
        public int? CodigoInicial { get; set; }
        public int? CodigoFinal { get; set; }
        public bool? enviarEmail { get; set; }
        public int? numeroTentativas { get; set; }
        public IList<NotaFiscaBatchItemDTO> ListCodPedidos { get; set; }
    }
}
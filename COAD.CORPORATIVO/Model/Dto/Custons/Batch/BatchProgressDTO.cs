using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Batch
{
    public class BatchProgressDTO
    {
        public BatchProgressDTO()
        {
            LstErros = new HashSet<ImportacaoHistoricoDTO>();
        }

        public bool Executando { get; set; }
        public int QuantidadeSucesso { get; set; }
        public int? CodStatus { get; set; }
        public DateTime? UltimaExecucao { get; set; }
        
        public int QuantidadeFalha { get; set; }
        public string NomePassoBatch { get; set; }
        public int? TotalItens { get; set; }
        public int? ProcessedItens { get; set; }
        public int ItensRestantes {
            get {
                var totalItens = (TotalItens != null) ? (int) TotalItens : 0;
                var itensProcess = (ProcessedItens != null) ? (int) ProcessedItens : 0;
                return (totalItens - itensProcess);
            }
            private set { }
        }
        public int Progress { get; set; }

        public ICollection<ImportacaoHistoricoDTO> LstErros { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Historicos
{
    public class HistoricoTransAssinaturaDTO
    {
        public string usuario{ get; set; }
        public int? REP_ID_EXECUTOU_A_ACAO { get; set; }
        public int? CLI_ID { get; set; }
        public int? PeriodoBonus { get; set; }
        public string assinaturaAnterior { get; set; }
        public string assinaturaNova { get; set; }
        public string ContratoOrigem { get; set; }
        public string ContratoDestino { get; set; }
        public int? ipeId { get; set; }
        public int? pedCrmId { get; set; }
        public int? ppiId { get; set; }
        public int? prtId { get; set; }
        public int? pstId { get; set; }
        public string observacoes { get; set; }
    }
}

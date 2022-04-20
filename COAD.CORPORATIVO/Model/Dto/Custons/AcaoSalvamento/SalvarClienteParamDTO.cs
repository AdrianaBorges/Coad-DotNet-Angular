using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.AcaoSalvamento
{
    public class SalvarClienteParamDTO
    {

        public int? RepIdDemandante { get; set; }
        public int? RegiaoId { get; set; }
        public int? RepIdEncarteirar { get; set; }
        public bool PularValidacao { get; set; }
        public bool IgnorarRodizio { get; set; }
        public bool AtualizarEncarteiramentos { get; set; }
        public bool ValidacaoEmailTelCPFNaoRestritiva { get; set; }
        public bool ForcarSalvamento { get; set; }
        //public bool EhIsentoDeInscricaoEstadual { get; set; }

    
    }
}

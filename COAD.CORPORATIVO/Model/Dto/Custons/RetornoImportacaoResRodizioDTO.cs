using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class RetornoImportacaoResRodizioDTO
    {
        public ICollection<ImportacaoResultadoRodizioDTO> Items { get; set; }
        public int? QtdRodizio { get; set; }
        public int? QtdPrioridades { get; set; }
        public int? Total { get; set; }

    }
}

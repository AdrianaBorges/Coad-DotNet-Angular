using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ResultadoCalculoImpostoDTO
    {
        public InfoFaturaDTO ResultadoEntrada { get; set; }
        public InfoFaturaDTO ResultadoParcela { get; set; }
    }
}

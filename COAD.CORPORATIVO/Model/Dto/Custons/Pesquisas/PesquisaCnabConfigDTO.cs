using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class PesquisaCnabConfigDTO
    {
        public PesquisaCnabConfigDTO()
        {
            this.pagina = 1;
            this.registrosPorPagina = 6;
        }
        public int? empId { get; set; }
        public string banId { get; set; }
        public int? tipoRegistro { get; set; }
        public int pagina { get; set; }
        public int registrosPorPagina { get; set; }
    }
}

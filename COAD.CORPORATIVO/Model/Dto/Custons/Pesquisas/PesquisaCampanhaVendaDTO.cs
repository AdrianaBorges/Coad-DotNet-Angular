using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class PesquisaCampanhaVendaDTO
    {
        public PesquisaCampanhaVendaDTO()
        {
            this.pagina = 1;
            this.registrosPorPagina = 7;
        }

        public int? TppId { get; set; }
        public DateTime? DataInicioDe { get; set; }
        public DateTime? DataInicioAte { get; set; }

        public DateTime? DataFimDe { get; set; }
        public DateTime? DataFimAte { get; set; }

        public int pagina { get; set; }
        public int registrosPorPagina { get; set; }
    }
}

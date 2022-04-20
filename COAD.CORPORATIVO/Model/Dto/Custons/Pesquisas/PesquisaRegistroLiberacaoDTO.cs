using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class PesquisaRegistroLiberacaoDTO
    {
        public PesquisaRegistroLiberacaoDTO()
        {
            pagina = 1;
            registrosPorPagina = 10;
        }

        public int? rliId { get; set; }
        public string descricao { get; set; }
        public int? ppiId { get; set; }
        public int? prtId { get; set; }
        public string produto { get; set; }
        public int pagina { get; set; }
        public int registrosPorPagina { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class PesquisaImportacaoDTO
    {
        public PesquisaImportacaoDTO()
        {
            this.pagina = 1;
            this.registrosPorPagina = 6;
        }

        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
        public int? imsID { get; set; }
        public int? repID { get; set; }
        public int pagina { get; set; }
        public bool importacaoDiaria { get; set; }
        public int registrosPorPagina { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Dto.Custons.Pesquisas
{
    public class PesquisaTemplatesDTO
    {
        public PesquisaTemplatesDTO()
        {
            pagina = 1;
            registrosPorPagina = 10;
        }

        public int? tplId { get; set; }

        public string descricao { get; set; }
        public Nullable<bool> layout { get; set; }
        public Nullable<int> tgrId { get; set; }
        public int pagina { get; set; }
        public int registrosPorPagina { get; set; }

    }
}

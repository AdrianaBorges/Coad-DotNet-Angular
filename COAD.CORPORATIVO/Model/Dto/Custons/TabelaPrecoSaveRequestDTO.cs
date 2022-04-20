using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class TabelaPrecoSaveRequestDTO
    {
        public ICollection<TabelaPrecoDTO> TABELA_PRECO_ATUALIZAR { get; set; }
        public ICollection<TabelaPrecoDTO> TABELA_PRECO_EXCLUSAO { get; set; }
    }
}

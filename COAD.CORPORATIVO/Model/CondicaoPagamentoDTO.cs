using COAD.CORPORATIVO.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model
{
    public class CondicaoPagamentoDTO
    {
        public CondicaoPagamentoDTO()
        {
            this.TABELA_PRECO = new HashSet<TabelaPrecoDTO>();
        }
    
        public int CO_PG_ID { get; set; }
        public string CO_PG_DESCRICAO { get; set; }
    
        public virtual ICollection<TabelaPrecoDTO> TABELA_PRECO { get; set; }
    }
}

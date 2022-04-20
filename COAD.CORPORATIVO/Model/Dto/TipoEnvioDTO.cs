using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoEnvioDTO
    {
        public TipoEnvioDTO()
        {
            this.PRODUTO_COMPOSICAO = new HashSet<ProdutoComposicaoDTO>();
        }
    
        public int TIPO_ENVIO_ID { get; set; }
        public string TIPO_ENVIO_DESCRICAO { get; set; }

        public virtual ICollection<ProdutoComposicaoDTO> PRODUTO_COMPOSICAO { get; set; }
    }
}

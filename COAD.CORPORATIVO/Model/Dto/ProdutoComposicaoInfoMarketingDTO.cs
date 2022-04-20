using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ProdutoComposicaoInfoMarketingDTO
    {
        public int? CMP_ID { get; set; }
        public int? MKT_CLI_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }
        
        public virtual InfoMarketingDTO INFO_MARKETING { get; set; }
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }
    }
}

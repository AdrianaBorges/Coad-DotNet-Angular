using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ProdutoIntegracaoDTO
    {
        public int prodId { get; set; }

        public IList<ProdutoComposicaoIntegracaoDTO> prodCompIntegrList { get; set; }

    }
}

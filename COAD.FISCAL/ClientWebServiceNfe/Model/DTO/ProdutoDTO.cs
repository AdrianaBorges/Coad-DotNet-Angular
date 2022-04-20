using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class ProdutoDTO
    {
        public string CodProduto { get; set; }
        public string Nome { get; set; }
        public string NCM { get; set; }
        public string UnidadeComercial { get; set; }
        public string UnidadeTributavel { get; set; }
        public string CFOP { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class UraProdutoDTO
    {
        public UraProdutoDTO()
        {
            this.URA_CONFIG = new HashSet<UraConfigDTO>();
            this.URA_PRODUTO_AREA = new HashSet<UraProdutoAreaDTO>();
        }
    
        public string URA_ID { get; set; }
        public int PRO_ID { get; set; }
        public Nullable<bool> URA_VIP { get; set; }
        public Nullable<bool> URA_ATIVA { get; set; }
    
        public virtual ProdutosDTO PRODUTOS { get; set; }
        public virtual UraDTO URA { get; set; }
        public virtual ICollection<UraConfigDTO> URA_CONFIG { get; set; }
        public virtual ICollection<UraProdutoAreaDTO> URA_PRODUTO_AREA { get; set; }

    }
}

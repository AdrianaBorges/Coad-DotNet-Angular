using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class UraConfigDTO
    {
        public UraConfigDTO()
        {
            this.URA_PRODUTO_AREA = new HashSet<UraProdutoAreaDTO>();
        }
        
        public string URA_ID { get; set; }
        public int PRO_ID { get; set; }
        public string UF_SIGLA_ACESSO { get; set; }
        public Nullable<int> URA_ACESSO { get; set; }

        public virtual UraDTO URA { get; set; }
        public virtual UraProdutoDTO URA_PRODUTO { get; set; }
        public virtual ICollection<UraProdutoAreaDTO> URA_PRODUTO_AREA { get; set; }
    }
}

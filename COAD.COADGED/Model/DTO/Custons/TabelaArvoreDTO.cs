using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO.Custons
{

    public partial class TabelaArvoreDTO
    {
        public TabelaArvoreDTO()
        {
            this.ITEM = new HashSet<TabelaArvoreDTO>();
        }
        public Nullable<decimal> ID { get; set; }
        public string CODIGO { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<decimal> ID_NODE { get; set; }
        public string CODAUX { get; set; }
        public virtual ICollection<TabelaArvoreDTO> ITEM { get; set; }
    }

}

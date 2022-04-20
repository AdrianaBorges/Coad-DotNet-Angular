using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class TipoDocFiscalDTO
    {
        public TipoDocFiscalDTO()
        {
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
        }

        public string TDF_ID { get; set; }
        public string TDF_DESCRICAO { get; set; }

        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }
    }
}

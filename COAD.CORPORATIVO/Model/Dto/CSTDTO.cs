using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class CSTDTO
    {
        public CSTDTO()
        {
            this.NOTA_FISCAL_ITEM = new HashSet<NotaFiscalItemDTO>();
        }
    
        public string CST_ID { get; set; }
        public string CST_DESCRICAO { get; set; }
        public Nullable<int> CST_TIPO_ID { get; set; }
    
        public virtual CSTTipoDTO CST_TIPO { get; set; }
        public virtual ICollection<NotaFiscalItemDTO> NOTA_FISCAL_ITEM { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class LinhaProdutoInformativoDTO
    {
        public int INF_ID { get; set; }
        public string INF_DECRICAO { get; set; }
        public string INF_PERIODICIDADE { get; set; }
        public string INF_TIPO { get; set; }
        public Nullable<int> LIN_PRO_ID { get; set; }

        public virtual LinhaProdutoDTO LINHA_PRODUTO { get; set; }
    }
}

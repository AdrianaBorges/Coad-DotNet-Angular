using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class OrigemAcessoSubMenuDTO
    {
        public int OAC_ID { get; set; }
        public string OAM_MENU { get; set; }
        public int OAS_ID { get; set; }
        public string OAS_DESCRICAO { get; set; }
        public string TDC_ID { get; set; }
        public string OAS_PATH_ARQUIVO { get; set; }
        public Nullable<int> OAS_ORDEM { get; set; }

        public virtual OrigemAcessoMenuDTO ORIGEM_ACESSO_MENU { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class OrigemAcessoMenuDTO
    {
        public OrigemAcessoMenuDTO()
        {
            this.ORIGEM_ACESSO_SUBMENU = new HashSet<OrigemAcessoSubMenuDTO>();
        }
    
        public int OAC_ID { get; set; }
        public string OAM_MENU { get; set; }
        public Nullable<int> OAM_ORDEM { get; set; }

        public virtual OrigemAcessoRefDTO ORIGEM_ACESSO_REF { get; set; }
        public virtual ICollection<OrigemAcessoSubMenuDTO> ORIGEM_ACESSO_SUBMENU { get; set; }
    }
}

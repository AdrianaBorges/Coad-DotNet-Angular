using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class TabDinamicaGrupoDTO
    {
        public TabDinamicaGrupoDTO()
        {
            this.TAB_DINAMICA_CONFIG = new HashSet<TabDinamicaConfigDTO>();
        }

        public int TGR_ID { get; set; }
        public string TGR_DESCRICAO { get; set; }
        public Nullable<int> TGR_TIPO { get; set; }

        public virtual ICollection<TabDinamicaConfigDTO> TAB_DINAMICA_CONFIG { get; set; }
    }
}

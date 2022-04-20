using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class FilaCadastroDTO
    {
        public int? FLC_ID { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public int FLC_ORDEM { get; set; }
        public Nullable<System.DateTime> FLC_DATA { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public Nullable<bool> FLC_IMPORTACAO { get; set; }

        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
        public virtual RegiaoDTO REGIAO { get; set; }
    }
}

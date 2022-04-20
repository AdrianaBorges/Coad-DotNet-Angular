using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model
{
    public partial class clienteLegDTO
    {
        public string CODIGO { get; set; }
        public string CGC { get; set; }
        public string INSCRICAO { get; set; }
        public int AUTOID { get; set; }
        public string DH_SUBIR { get; set; }
        public string DH_SUBIU { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<System.DateTime> DATA_INSERT { get; set; }
    }
}

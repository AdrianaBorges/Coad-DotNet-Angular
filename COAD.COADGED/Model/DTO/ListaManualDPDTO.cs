using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class ListaManualDPDTO
    {

        public string MOD_DESCRICAO { get; set; }
        public string MAN_ASSUNTO { get; set; }
        public string MAI_TITULO { get; set; }
        public string USU_LOGIN { get; set; }
        public string USU_LOGIN_ALT { get; set; }
        public Nullable<DateTime> DATA_INSERT { get; set; }
        public Nullable<DateTime> DATA_ALTERA { get; set; }
        public Nullable<DateTime> MAI_DATA_PUBLICACAO { get; set; }
        public Nullable<int> MOD_ID { get; set; }
        public Nullable<int> MAI_ID { get; set; }
        public Nullable<int> MAN_ID { get; set; }
        public string MAI_DESCRICAO { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    public partial class EmailsDTO
    {

        public string ASSINATURA { get; set; }
        public string E_MAIL { get; set; }
        public string PRINCIPAL_SN { get; set; }
        public string DT_LISTAGEM_CONF { get; set; }
        public int? AUTOID { get; set; }
        public string DH_SUBIR { get; set; }
        public string DH_SUBIU { get; set; }
        public string SETOR { get; set; }
        public string DECISOR { get; set; }
        public Nullable<System.DateTime> DATA_INSERT { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }

        /// <summary>
        /// Esse campo não existe no banco
        /// </summary>
        public int? IdEmailCoadCorp { get; set; }
    }
}

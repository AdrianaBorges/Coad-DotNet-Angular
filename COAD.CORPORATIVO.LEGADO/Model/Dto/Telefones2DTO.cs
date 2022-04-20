using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    public partial class Telefones2DTO
    {
        public string ASSINATURA { get; set; }
        public string DDD_TEL { get; set; }
        public string TIPO { get; set; }
        public string DH_SUBIR { get; set; }
        public string DH_SUBIU { get; set; }
        public int? id { get; set; }
        public string SETOR { get; set; }
        public string TELEFONE { get; set; }
        public Nullable<System.DateTime> DATA_INSERT { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }

        /// <summary>
        /// Esse id não existe no banco
        /// </summary>
        public int? IdTelCoadCorp { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class ContatoDTO
    {
        public int CON_ID { get; set; }
        public string CON_CARGO { get; set; }
        public string CON_PROFISAO { get; set; }
        public string CON_EMAIL { get; set; }
        public string CON_TELEFONE { get; set; }
        public Nullable<int> CLI_ID { get; set; }

        public virtual ClienteDto CLIENTES { get; set; }
    }
}

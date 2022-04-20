using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class AuditoriaDTO
    {
        public int AUD_ID { get; set; }
        public Nullable<System.DateTime> AUD_DATA_ALTERA { get; set; }
        public Nullable<int> AUD_CAMPO_ALTERA { get; set; }
        public string AUD_VALOR_ANT_ALTERA { get; set; }
        public string AUD_TABELA_ALTERA { get; set; }
        public Nullable<int> FOR_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }

        public virtual ClienteDto CLIENTES { get; set; }
        public virtual FornecedorDTO FORNECEDOR { get; set; }
    }
}

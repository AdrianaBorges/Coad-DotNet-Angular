using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class CarteiramentoClienteRepresentanteDTO
    {
        public int? REP_ID { get; set; }
        public int? CLI_ID { get; set; }
        public int? RG_ID { get; set; }
        public string DescricaoRegiao { get; set; }
        public string NomeRepresentante { get; set; }
        public string NomeCliente { get; set; }

    }
}

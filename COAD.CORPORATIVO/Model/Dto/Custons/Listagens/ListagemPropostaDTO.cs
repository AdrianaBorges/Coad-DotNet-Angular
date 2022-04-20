using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Listagens
{
    public class ListagemPropostaDTO
    {
        public int? PropostaID { get; set; }
        public DateTime? DataCadastro { get; set; }
        public int? UenID { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCnpjCliente { get; set; }

    }
}

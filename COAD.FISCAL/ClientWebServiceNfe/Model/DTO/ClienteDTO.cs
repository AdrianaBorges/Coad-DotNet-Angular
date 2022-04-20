using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class ClienteDTO
    {
        public int? CodCliente { get; set; }
        public string Nome { get; set; }

        public TipoClienteEnum TipoCliente { get; set; }

        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string IE { get; set; }
        public EnderecoDTO Endereco { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Buscas
{
    public class BuscarClientePorIdDTO
    {
        public bool trazInfoMarketing { get; set; }
        public bool trazClienteTelefone { get; set; }
        public bool trazAssinaturaEmail { get; set; }
        public bool validaEnderecoBasica { get; set; }
        public bool trazCarteiraCliente { get; set; }
        public bool trazInfoImposto { get; set; }
    }
}

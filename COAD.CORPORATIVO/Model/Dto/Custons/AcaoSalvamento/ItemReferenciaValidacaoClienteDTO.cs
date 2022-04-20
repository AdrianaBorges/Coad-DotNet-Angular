using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.AcaoSalvamento
{
    public class ItemReferenciaValidacaoClienteDTO
    {
        public ClienteDto Cliente { get; set; }
        public bool NomeDuplicado { get; set; }
        public bool CPFDuplicado { get; set; }
        public bool TelefoneDuplicado { get; set; }
        public bool EmailDuplicado { get; set; }
    }
}

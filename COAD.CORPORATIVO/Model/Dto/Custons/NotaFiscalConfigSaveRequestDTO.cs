using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class NotaFiscalConfigSaveRequestDTO
    {
        public ICollection<NotaFiscalConfigDTO> NotaFiscalConfigAtualizar { get; set; }
        public ICollection<NotaFiscalConfigDTO> NotaFiscalConfigExcluir { get; set; }
    }
}

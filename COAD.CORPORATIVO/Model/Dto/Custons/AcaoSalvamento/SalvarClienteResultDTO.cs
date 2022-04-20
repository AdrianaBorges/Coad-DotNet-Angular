using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.AcaoSalvamento
{
    public class SalvarClienteResultDTO
    {
        public SalvarClienteResultDTO()
        {
            this.ItensReferencia = new HashSet<ItemReferenciaValidacaoClienteDTO>();
        }
        public ResultClienteDuplicadoDTO ResultadoValidacaoDuplicidade { get; set; }
        public ClienteDto Cliente { get; set; }
        public ICollection<ItemReferenciaValidacaoClienteDTO> ItensReferencia { get; set; }

    }
}

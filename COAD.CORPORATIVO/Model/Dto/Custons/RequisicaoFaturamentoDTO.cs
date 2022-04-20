using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class RequisicaoFaturamentoDTO
    {
        public RequisicaoFaturamentoDTO()
        {
            LstRequisicaoFaturamento = new HashSet<RequisicaoFaturamentoDetalheDTO>();
        }

        public int? PrtId { get; set; }
        public DateTime? DataFaturamento { get; set; }
        public ICollection<RequisicaoFaturamentoDetalheDTO> LstRequisicaoFaturamento { get; set; }
    }
}

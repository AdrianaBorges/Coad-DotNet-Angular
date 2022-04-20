using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes.Interfaces
{
    public interface INotaFiscalReferenciada
    {
        int? LoteItemID { get; set; }
        int? CodNotaFiscal { get; set; }
        string ChaveNota { get; set; }
    }
}

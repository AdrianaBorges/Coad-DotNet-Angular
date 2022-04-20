using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes.Interfaces
{
    public interface INotaFiscalItemMSG
    {
        int? LoteItemID { get; set; }
        string CodMsg { get; set; }
        string Msg { get; set; }
        string Correcao { get; set; }
    }
}

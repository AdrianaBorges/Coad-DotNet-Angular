using COAD.FISCAL.Model.Integracoes.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes.Interfaces
{
    public interface INFeLote
    {
        TipoLoteEnum TipoLote { get; set; }
        int? LoteID { get; set; }
        int? EmpresaID { get; set; }
        DateTime? DataCadastro { get; set; }
        string CodRecibo { get; set; }
        StatusLoteEnum Status { get; set; }
        ICollection<INFeLoteItem> Itens { get; set; }
        Nullable<int> CodRetorno { get; set; }
        string MensagemRetorno { get; set; }        
        Nullable<int> CodRetornoProcessamento { get; set; }
        string MensagemRetornoProcessamento { get; set; }
        bool? EnvioImediato { get; set; }
        string MsgErroSistema { get; set; }

    }
}

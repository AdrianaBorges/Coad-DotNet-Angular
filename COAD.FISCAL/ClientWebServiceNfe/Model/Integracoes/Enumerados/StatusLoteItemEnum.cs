using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes.Enumerados
{
    public enum StatusLoteItemEnum
    {
        ENVIO_PENDENTE = 1,
        AGUARDANDO_RETORNO = 2,
        REJEITADA = 3,
        AUTORIZADA = 4,
        CANCELADA = 5,
        LOTE_EM_PROCESSAMENTO = 9,
        REJEITADA_E_INUTILIZADA = 10,
        AUTORIZADA_E_ENVIADA = 11
    }
}

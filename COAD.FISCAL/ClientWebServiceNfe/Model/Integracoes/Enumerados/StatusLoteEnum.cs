using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes.Enumerados
{
    public enum StatusLoteEnum
    {
        ENVIO_PENDENTE = 1,
        ERRO_AO_PROCESSAR = 2,
        PROCESSADA_COM_EXITO = 3,
        LOTE_ENVIADO_NAO_PROCESSADO = 8,
        LOTE_EM_PROCESSAMENTO = 9
    }
}

using COAD.SEGURANCA.Service.Custons.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Dto.Custons.Batch
{
    public class RegistroErroBatchDTO
    {
        public RegistroErroBatchDTO()
        {
            contabilizarFalha = true;
        }

        public string nomeDaExecucao { get; set; }
        public Exception e { get; set; }
        public BatchContext batchEx { get; set; }
        public string servico { get; set; }
        public int? tipoJob { get; set; }
        public string projeto { get; set; }
        public string context { get; set; }
        public int? codReferencia { get; set; }
        public string descricaoCodigoReferencia { get; set; }
        public string codTipoJobStr { get; set; }
        public bool contabilizarFalha { get; set; }
        public int? qtdOcorrenciaEnvioEmail { get; set; }
        public int? codNotificacaoSistema { get; set; }
        public string descricao { get; set; }
        public int? codTipoNotificacaoSistema { get; set; }
        public int? codNotificacaoOrigem { get; set; }
        public bool? reporteErroEmail { get; set; }
        public bool enviarEmailErro { get; set; } = true;
        public int? codRepresentante { get; set; }

        public string ccEmail { get; set; }
        public string ccoEmail { get; set; }

    }
}

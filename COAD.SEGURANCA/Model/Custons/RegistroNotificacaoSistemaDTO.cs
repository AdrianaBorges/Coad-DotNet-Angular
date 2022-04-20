using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Custons
{
    public class RegistroNotificacaoSistemaDTO
    {
        public bool? erro { get; set; }
        public string nomeDaExecucao { get; set; }
        public string descricao { get; set; }
        public DateTime? data { get; set; }
        public string nomeServico { get; set; }
        public string nomeProjeto { get; set; }
        public Exception exception { get; set; }
        public int? codTipoJob { get; set; }
        public int? codTipoNotificacaoSistema { get; set; }
        public string codReferenciaStr { get; set; }
        public int? codReferencia { get; set; }
        public string descricaoCodigoReferencia { get; set; }

        public int? codNotificacaoSistema { get; set; }
        public int? codNotificacaoSistemaOrigem { get; set; }
        public int? qtdOcorrenciaEnvioEmail { get; set; }
        public bool? reporteErroEmail { get; set; }
        public bool enviarEmailErro { get; set; } = true;

        public string cc { get; set; }
        public string cco { get; set; }
        

    }
}

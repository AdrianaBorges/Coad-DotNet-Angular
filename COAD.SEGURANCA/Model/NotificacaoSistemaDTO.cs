using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(NOTIFICACAO_SISTEMA))]
    public class NotificacaoSistemaDTO
    {
        public NotificacaoSistemaDTO()
        {
            this.FILA_EMAIL = new HashSet<FilaEmailDTO>();
            this.HISTORICO_EXECUCAO = new HashSet<HistoricoExecucaoDTO>();
        }

        public int? NTS_ID { get; set; }
        public string NTS_DESCRICAO { get; set; }
        public Nullable<int> NTS_COD_REF_INT { get; set; }
        public string NTS_COD_REF_STR { get; set; }
        public string NTS_DESCRICAO_COD_REF { get; set; }
        public Nullable<System.DateTime> NTS_DATA { get; set; }
        public Nullable<int> TNS_ID { get; set; }
        public Nullable<int> NTS_NUMERO_CORRENCIA { get; set; }
        public string NTS_SERVICO { get; set; }
        public string NTS_PROJETO { get; set; }
        public Nullable<System.DateTime> NTS_DATA_RESOLUCAO { get; set; }
        public string NTS_ERRO_NOME { get; set; }
        public string NTS_ERRO_DESCRICAO { get; set; }
        public string NTS_STACK_TRACE { get; set; }
        public Nullable<System.DateTime> NTF_DATA_CANCELAMENTO { get; set; }
        public Nullable<System.DateTime> NTS_ERRO_PAUSADO_ATE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoNotificacaoSistemaDTO TIPO_NOTIFICACAO_SISTEMA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<FilaEmailDTO> FILA_EMAIL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<HistoricoExecucaoDTO> HISTORICO_EXECUCAO { get; set; }
    }
}

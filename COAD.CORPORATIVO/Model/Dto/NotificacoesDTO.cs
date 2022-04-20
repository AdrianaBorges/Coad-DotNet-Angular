using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(NOTIFICACOES))]
    public class NotificacoesDTO
    {

        public NotificacoesDTO()
        {
            NTF_EXIBIDO = false;
        }

        public int? NTF_ID { get; set; }
        public Nullable<int> TP_NTF_ID { get; set; }
        public string NTF_DESCRICAO { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public Nullable<int> AGE_ID { get; set; }
        public string URG_NTF_ID { get; set; }
        public bool NTF_VISUALIZADO { get; set; }
        public Nullable<System.DateTime> NTF_DATA { get; set; }
        public Nullable<int> CLI_ID { get; set; }   
        public Nullable<int> REP_QUE_ENCAMINHOU { get; set; }
        public Nullable<System.DateTime> DATA_AGENDAMENTO { get; set; }
        public Nullable<bool> NTF_EXIBIDO { get; set; }
        public Nullable<int> NTF_COD_REF_INT { get; set; }
        public string NTF_COD_REF_STR { get; set; }

        public virtual AgendamentoDTO AGENDAMENTO { get; set; }
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
        public virtual TipoNotificacaoDTO TIPO_NOTIFICACAO { get; set; }
        public virtual UrgenciaNotificacaoDTO URGENCIA_NOTIFICACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ClienteDto CLIENTES { get; set; }
    }
}

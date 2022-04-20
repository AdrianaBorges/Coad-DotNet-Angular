using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(JOB_AGENDAMENTO))]
    public class JobAgendamentoDTO
    {
        public JobAgendamentoDTO()
        {
            this.JOB_NOTIFICACAO = new HashSet<JobNotificacaoDTO>();
        }

        public int? JOB_ID { get; set; }
        public string JOB_NOME { get; set; }
        public string JOB_DESCRICAO { get; set; }
        public Nullable<bool> JOB_ATIVADO { get; set; }
        public Nullable<System.DateTime> JOB_DATA_ULTIMA_EXECUCAO { get; set; }
        public Nullable<int> JOB_MINUTOS_MAX_OCIOSO { get; set; }
        public Nullable<bool> JOB_EXECUTANDO { get; set; }
        public string JOB_EMAIL_ENVIO { get; set; }

        public Nullable<bool> JOB_EXECUTAR_AGORA { get; set; }
        public Nullable<System.DateTime> JOB_INICIO_EXECUCAO { get; set; }
        public Nullable<bool> JOB_ATIVAR { get; set; }
        public string JOB_BATCH_NOME { get; set; }
        public Nullable<int> JOB_BATCH_PROCESSED_ITENS { get; set; }
        public Nullable<int> JOB_BATCH_TOTAL_ITENS { get; set; }
        public Nullable<int> JOB_BATCH_PROGRESS { get; set; }
        public string JOB_PENDING_ITENS_DESC { get; set; }
        public Nullable<int> JOB_COD_EXE_REF { get; set; }
        public string JOB_COD_STR_EXE_REF { get; set; }
        public string JOB_COD_EXE_REF_DESC { get; set; }
        public Nullable<int> JOB_QTD_FALHA { get; set; }
        public Nullable<int> JOB_QTD_SUCESSO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<JobNotificacaoDTO> JOB_NOTIFICACAO { get; set; }

        public int ItensRestantes {
            get
            {
                if (JOB_BATCH_TOTAL_ITENS != null && JOB_BATCH_PROCESSED_ITENS != null)
                    return JOB_BATCH_TOTAL_ITENS.Value - JOB_BATCH_PROCESSED_ITENS.Value;
                return 0;
            }
        }
        public string JobPendingItensDescFormatted
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(JOB_PENDING_ITENS_DESC))
                {
                    return string.Format(JOB_PENDING_ITENS_DESC, ItensRestantes);
                }
                return null;
            }
        }
    }
}

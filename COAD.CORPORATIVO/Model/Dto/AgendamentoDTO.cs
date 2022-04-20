using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(AGENDAMENTO))]
    public class AgendamentoDTO
    {

        public AgendamentoDTO()
        {
            this.HISTORICO_ATENDIMENTO = new HashSet<HistoricoAtendimentoDTO>();
            this.NOTIFICACOES = new HashSet<NotificacoesDTO>();
        }

        public int? AGE_ID { get; set; }

        //[PresentDate(ErrorMessage = "A data de agendamento não pode ser antes de hoje")]
        [Required(ErrorMessage = "Defina a data e hora do agendamento")]       
        public Nullable<System.DateTime> AGE_DATA_AGENDAMENTO { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public string CAR_ID { get; set; }
        public Nullable<int> REP_ID { get; set; }

        [Required(ErrorMessage = "Digite a descrição do agendamento")]
        [MinLength(4, ErrorMessage = "A descrição deve possuir no mínimo 4 dígitos")]
        public string AGE_DESCRICAO { get; set; }

        public Nullable<System.DateTime> AGE_DATA_CONFIRMACAO { get; set; }

        public Nullable<System.DateTime> AGE_DATA_REAGENDAMENTO { get; set; }
        public Nullable<int> RG_ID { get; set; }

        public virtual CarteiraDTO CARTEIRA { get; set; }
        public virtual ClienteDto CLIENTES { get; set; }
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }

        [Required]
        public virtual ICollection<HistoricoAtendimentoDTO> HISTORICO_ATENDIMENTO { get; set; }
        public virtual ICollection<NotificacoesDTO> NOTIFICACOES { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }
    }
}

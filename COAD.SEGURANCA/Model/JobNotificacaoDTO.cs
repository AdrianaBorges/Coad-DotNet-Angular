

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.SEGURANCA.Model
{    
	[Mapping(typeof(JOB_NOTIFICACAO))]
	public class JobNotificacaoDTO
	{
		// Normal Properties
		public Int32 JNF_ID { get; set; }
		public Nullable<Int32> JOB_ID { get; set; }
		public String JNF_DESCRICAO { get; set; }
		public Nullable<DateTime> JNF_DATA { get; set; }
		public Nullable<DateTime> JNF_DATA_CONCLUSAO { get; set; }
		public Nullable<DateTime> JNF_DATA_CANCELAMENTO { get; set; }
		public Nullable<Int32> REP_ID { get; set; }
		public String USU_LOGIN { get; set; }
		public Nullable<Int32> JNF_COD_REF { get; set; }
		public String JNF_COD_STR_REF { get; set; }
        public Nullable<int> JNT_ID { get; set; }
        public string JNF_ULTIMO_ERRO { get; set; }

        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual JobAgendamentoDTO JOB_AGENDAMENTO { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual UsuarioModel USUARIO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual JobNotificacaoStatusDTO JOB_NOTIFICACAO_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<JobNotificacaoMsgItemDTO> JOB_NOTIFICACAO_MSG_ITEM { get; set; }

    }
}

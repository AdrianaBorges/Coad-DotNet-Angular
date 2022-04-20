

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
	[Mapping(typeof(JOB_NOTIFICACAO_STATUS))]
	public class JobNotificacaoStatusDTO
	{
		public JobNotificacaoStatusDTO(){

			this.JOB_NOTIFICACAO = new HashSet<JobNotificacaoDTO>();
		}

		// Normal Properties
		public Int32 JNT_ID { get; set; }
		public String JNT_DESCRICAO { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<JobNotificacaoDTO> JOB_NOTIFICACAO { get; set; }

	}
}

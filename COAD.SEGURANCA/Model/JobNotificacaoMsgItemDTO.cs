

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
	[Mapping(typeof(JOB_NOTIFICACAO_MSG_ITEM))]
	public class JobNotificacaoMsgItemDTO
	{
		// Normal Properties
		public Int32 NTM_ID { get; set; }
		public Nullable<Int32> JNF_ID { get; set; }
		public String NTM_MENSAGEM { get; set; }
		public Nullable<Int32> NTM_COD_REF { get; set; }
		public String NTM_COD_REF_STR { get; set; }
		public Nullable<DateTime> NTM_DATA { get; set; }
		
		// Object Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual JobNotificacaoDTO JOB_NOTIFICACAO { get; set; }
		
		// Collections Properties
		
	}
}

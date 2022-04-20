using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.Model.Dto
{    
	[Mapping(typeof(SEQUENCIAL_NOSSO_NUMERO))]
	public class SequencialNossoNumeroDTO
	{
		// Normal Properties
		public string BAN_ID { get; set; }
		public int? EMP_ID { get; set; }
		public int? SQN_SEQUENCIAL { get; set; }
        public Nullable<int> SQN_LIMITE_FAIXA { get; set; }

        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual BancosDTO BANCOS { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual EmpresaRefDTO EMPRESA_REF { get; set; }
		
		// Collections Properties
		
	}
}

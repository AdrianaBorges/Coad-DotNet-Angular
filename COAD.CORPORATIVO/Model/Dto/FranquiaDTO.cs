

using System;
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
	[Mapping(typeof(FRANQUIA))]
	public class FranquiaDTO
	{
		// Normal Properties
		public Int32 FRA_ID { get; set; }
		public String FRA_NOME { get; set; }
		public Nullable<Int32> RG_ID { get; set; }
		
		// Object Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual RegiaoDTO REGIAO { get; set; }
		
		// Collections Properties
		
	}
}

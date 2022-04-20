

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
	[Mapping(typeof(IMPOSTO_INFO_FATURA_ITEM))]
	public class ImpostoInfoFaturaItemDTO
	{
		// Normal Properties
		public Int32? IMP_ID { get; set; }
		public Int32? IFI_ID { get; set; }
		public Nullable<Decimal> IFI_PERCENTUAL_DESCONTO { get; set; }
		public Nullable<Decimal> IFI_VALOR_DESCONTADO { get; set; }
		
		// Object Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual ImpostoDTO IMPOSTO { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual InfoFaturaItemDTO INFO_FATURA_ITEM { get; set; }
		
		// Collections Properties
		
	}
}

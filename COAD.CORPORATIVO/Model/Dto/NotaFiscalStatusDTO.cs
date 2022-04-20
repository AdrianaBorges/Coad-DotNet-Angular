

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
	[Mapping(typeof(NOTA_FISCAL_STATUS))]
	public class NotaFiscalStatusDTO
	{
		public NotaFiscalStatusDTO(){

			this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
		}

		// Normal Properties
		public Int32 NST_ID { get; set; }
		public String NST_DESCRICAO { get; set; }
		public String NTS_SIGLA { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }

	}
}

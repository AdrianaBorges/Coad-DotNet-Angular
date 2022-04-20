

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
	[Mapping(typeof(NOTA_FISCAL_TIPO))]
	public class NotaFiscalTipoDTO
	{
		public NotaFiscalTipoDTO(){

			this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
		}

		// Normal Properties
		public Int32 NTP_ID { get; set; }
		public String NTP_DESCRICAO { get; set; }
		
		// Object Properties
		
		// Collections Properties 
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }

	}
}

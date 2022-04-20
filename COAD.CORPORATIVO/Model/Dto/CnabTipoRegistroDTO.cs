

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
	[Mapping(typeof(CNAB_TIPO_REGISTRO))]
	public class CnabTipoRegistroDTO
	{
		public CnabTipoRegistroDTO(){

			this.CNAB = new HashSet<CnabDTO>();
		}

		// Normal Properties
		public string CTR_ID { get; set; }
		public string CTR_DESCRICAO { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<CnabDTO> CNAB { get; set; }

	}
}

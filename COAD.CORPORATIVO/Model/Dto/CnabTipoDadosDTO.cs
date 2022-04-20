

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
	[Mapping(typeof(CNAB_TIPO_DADOS))]
	public class CnabTipoDadosDTO
	{
		public CnabTipoDadosDTO(){

			this.CNAB = new HashSet<CnabDTO>();
		}

		// Normal Properties
		public String CTD_ID { get; set; }
		public String CTD_DESCRICAO { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<CnabDTO> CNAB { get; set; }

	}
}

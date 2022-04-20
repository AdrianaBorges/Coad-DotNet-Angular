

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
	[Mapping(typeof(NOTA_FISCAL_LOTE_TIPO))]
	public class NotaFiscalLoteTipoDTO
	{
		public NotaFiscalLoteTipoDTO(){

			this.NOTA_FISCAL_LOTE = new HashSet<NotaFiscalLoteDTO>();
		}

		// Normal Properties
		public Int32 NLT_ID { get; set; }
		public String NLT_DESCRICAO { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<NotaFiscalLoteDTO> NOTA_FISCAL_LOTE { get; set; }

	}
}

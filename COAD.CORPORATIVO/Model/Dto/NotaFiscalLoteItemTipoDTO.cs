

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
	[Mapping(typeof(NOTA_FISCAL_LOTE_ITEM_TIPO))]
	public class NotaFiscalLoteItemTipoDTO
	{
		public NotaFiscalLoteItemTipoDTO(){

			this.NOTA_FISCAL_LOTE_ITEM = new HashSet<NotaFiscalLoteItemDTO>();
            this.NOTA_FISCAL_EVENTO = new HashSet<NotaFiscalEventoDTO>();
        }

		// Normal Properties
		public Int32 NIT_ID { get; set; }
		public String NIT_DESCRICAO { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<NotaFiscalLoteItemDTO> NOTA_FISCAL_LOTE_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalEventoDTO> NOTA_FISCAL_EVENTO { get; set; }

    }
}

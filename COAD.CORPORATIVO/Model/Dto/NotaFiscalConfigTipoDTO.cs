

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
	[Mapping(typeof(NOTA_FISCAL_CONFIG_TIPO))]
	public class NotaFiscalConfigTipoDTO
	{
		public NotaFiscalConfigTipoDTO(){

			this.NOTA_FISCAL_CONFIG = new HashSet<NotaFiscalConfigDTO>();
            this.INFO_FATURA_ITEM = new HashSet<InfoFaturaItemDTO>();
        }

		// Normal Properties
		public Int32 NCT_ID { get; set; }
		public String NCT_DESCRICAO { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<NotaFiscalConfigDTO> NOTA_FISCAL_CONFIG { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<InfoFaturaItemDTO> INFO_FATURA_ITEM { get; set; }

    }
}

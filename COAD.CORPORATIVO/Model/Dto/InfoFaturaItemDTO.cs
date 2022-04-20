

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
	[Mapping(typeof(INFO_FATURA_ITEM))]
	public class InfoFaturaItemDTO
	{
		public InfoFaturaItemDTO(){

			this.IMPOSTO_INFO_FATURA_ITEM = new HashSet<ImpostoInfoFaturaItemDTO>();
		}

		// Normal Properties
		public Nullable<Int32> IFI_ID { get; set; }
		public Nullable<Decimal> IFI_VALOR_BRUTO { get; set; }
		public Nullable<Decimal> IFI_TOTAL_LIQUIDO { get; set; }
		public Nullable<Decimal> IFI_TOTAL_DESCONTADO { get; set; }
		public Nullable<Decimal> IFI_PERCENTUAL_TOTAL_DESCONTADO { get; set; }
		public Nullable<Decimal> IFI_PERCENTUAL_REFERENCIA { get; set; }
		public Nullable<Int32> IFF_ID { get; set; }
        public Nullable<int> NFC_ID { get; set; }
        public Nullable<decimal> IFI_BASE_CALCULO { get; set; }
        public Nullable<int> NCT_ID { get; set; }
        public Nullable<bool> IFF_N_RETEVE_POR_REGRA { get; set; }

        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual InfoFaturaDTO INFO_FATURA { get; set; }
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<ImpostoInfoFaturaItemDTO> IMPOSTO_INFO_FATURA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalConfigDTO NOTA_FISCAL_CONFIG { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalConfigTipoDTO NOTA_FISCAL_CONFIG_TIPO { get; set; }

    }
}

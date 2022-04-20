

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
	[Mapping(typeof(CAMPANHA_VENDAS_PRODUTO_COMPOSICAO))]
	public class CampanhaVendasProdutoComposicaoDTO
	{
		// Normal Properties
		public Int32? CVE_ID { get; set; }
		public Int32? CMP_ID { get; set; }
		public Nullable<DateTime> DATA_ASSOCIACAO { get; set; }
		
		// Object Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual CampanhaVendaDTO CAMPANHA_VENDA { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }
		
		// Collections Properties
		
	}
}

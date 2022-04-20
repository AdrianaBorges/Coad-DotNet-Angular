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
	[Mapping(typeof(CARRINHO_COMPRAS))]
	public class CarrinhoComprasDTO
	{
		public CarrinhoComprasDTO(){

			this.CARRINHO_COMPRAS_ITEM = new HashSet<CarrinhoComprasItemDTO>();
		}

		// Normal Properties
		public Int32? CRC_ID { get; set; }
		public Nullable<Int32> CLI_ID { get; set; }
		public Nullable<DateTime> DATA_CRIACAO { get; set; }
		public Nullable<Decimal> CRC_VALOR_BRUTO { get; set; }
		public Nullable<Decimal> CRC_VALOR_FRETE { get; set; }
		public Nullable<Decimal> CRC_VALOR_DESCONTO { get; set; }
		public Nullable<Decimal> CRC_VALOR_LIQUIDO { get; set; }
        public Nullable<System.DateTime> DATA_CANCELAMENTO { get; set; }
        public Nullable<System.DateTime> DATA_CONFIRMACAO { get; set; }

        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual ClienteDto CLIENTES { get; set; }
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<CarrinhoComprasItemDTO> CARRINHO_COMPRAS_ITEM { get; set; }

	}
}

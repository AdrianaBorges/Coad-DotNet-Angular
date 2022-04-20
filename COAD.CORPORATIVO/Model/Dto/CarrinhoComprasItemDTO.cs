

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
	[Mapping(typeof(CARRINHO_COMPRAS_ITEM))]
	public class CarrinhoComprasItemDTO
	{
		// Normal Properties
		public Int32? CRC_ID { get; set; }
		public Int32? CMP_ID { get; set; }
		public Nullable<Int32> CCI_QTD { get; set; }
		public Nullable<Decimal> CCI_VALOR_UNITARIO { get; set; }
		public Nullable<Decimal> CCI_VALOR_TOTAL { get; set; }
		public Nullable<DateTime> DATA_CRIACAO { get; set; }
        public Nullable<System.DateTime> DATA_CANCELAMENTO { get; set; }
        public Nullable<System.DateTime> DATA_CONFIRMACAO { get; set; }
        public Nullable<int> IFF_ID { get; set; }
        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual CarrinhoComprasDTO CARRINHO_COMPRAS { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }
        public virtual InfoFaturaDTO INFO_FATURA { get; set; }

        // Collections Properties

    }
}

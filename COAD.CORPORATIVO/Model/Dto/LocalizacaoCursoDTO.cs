

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
	[Mapping(typeof(LOCALIZACAO_CURSO))]
	public class LocalizacaoCursoDTO
	{
		public LocalizacaoCursoDTO(){

			this.ITEM_PEDIDO = new HashSet<ItemPedidoDTO>();
            this.PROPOSTA_ITEM = new HashSet<PropostaItemDTO>();
		}

		// Normal Properties
		public Int32 LOC_ID { get; set; }
		public String LOC_DESCRICAO { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO { get; set; }

		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<PropostaItemDTO> PROPOSTA_ITEM { get; set; }

	}
}

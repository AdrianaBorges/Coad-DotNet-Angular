

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
	[Mapping(typeof(TIPO_NEGOCIACAO))]
	public class TipoNegociacaoDTO
	{
		public TipoNegociacaoDTO(){

			this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>();
		}

		// Normal Properties
		public Int32 TNE_ID { get; set; }
		public String TNE_DESCRICAO { get; set; }
		
		// Object Properties
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }

		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }

	}
}

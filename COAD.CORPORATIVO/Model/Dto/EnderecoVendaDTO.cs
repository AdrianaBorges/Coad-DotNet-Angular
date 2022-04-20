

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
	[Mapping(typeof(ENDERECO_VENDA))]
	public class EnderecoVendaDTO
	{
		public EnderecoVendaDTO(){

			this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>();
		}

		// Normal Properties
		public Int32 ENV_ID { get; set; }
		public String ENV_NOME { get; set; }
		public String ENV_EMAIL { get; set; }
		public String ENV_DDD { get; set; }
		public String ENV_TELEFONE { get; set; }
		public String ENV_LOGRADOURO { get; set; }
		public String ENV_COMPLEMENTO { get; set; }
		public String ENV_NUMERO { get; set; }
		public String ENV_BAIRRO { get; set; }
		public String ENV_CEP { get; set; }
		public Nullable<Int32> MUN_ID { get; set; }
		public String ENC_UF { get; set; }
		public Nullable<Int32> CLI_ID { get; set; }
		
		// Object Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual ClienteDto CLIENTES { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual MunicipioDTO MUNICIPIO { get; set; }
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }

		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }

	}
}

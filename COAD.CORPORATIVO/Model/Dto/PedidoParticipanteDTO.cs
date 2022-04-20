using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PEDIDO_PARTICIPANTE) )]
    public class PedidoParticipanteDTO
    {
        public int? PPR_ID { get; set; }

        [Required(ErrorMessage = "Informe o nome do participante")]
        public string PPR_NOME { get; set; }

        [Required(ErrorMessage = "Informe o CPF do participante")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "O cnpj deve ter no mínimo 11 caracteres e no máximo 14")]
        public string PPR_CPF_CNPJ { get; set; }

        [Required(ErrorMessage = "Informe o email do participante")]
        [EmailAddress(ErrorMessage = "Digite um E-Mail válido")]
        public string PPR_EMAIL { get; set; }

        [StringLength(2, MinimumLength = 2, ErrorMessage = "O DDD possúi 2 caracteres")]
        public string PPR_DDD { get; set; }

        [Required(ErrorMessage = "Informe o telefone do participante")]
        public string PPR_TELEFONE { get; set; }

        public int? CLI_ID { get; set; }
        public int? PED_CRM_ID { get; set; }
        public int? IPE_ID { get; set; }
        public Nullable<bool> PED_EH_O_COMPRADOR { get; set; }
        public Nullable<int> PRT_ID { get; set; }
        public Nullable<int> PPI_ID { get; set; }
    

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ClienteDto CLIENTES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PedidoCRMDTO PEDIDO_CRM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PropostaDTO PROPOSTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PropostaItemDTO PROPOSTA_ITEM { get; set; }
    
    }
    
}

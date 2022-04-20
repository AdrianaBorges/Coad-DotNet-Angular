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
    [Mapping(Source = typeof(HISTORICO_ATENDIMENTO))]
    public partial class HistoricoAtendimentoDTO
    {
        
        public int? HAT_ID { get; set; }

        public Nullable<System.DateTime> HAT_DATA_HIST { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string HAT_SOLICITANTE { get; set; }
        public Nullable<int> ACA_ID { get; set; }
        public Nullable<System.DateTime> HAT_DATA_RESOLUCAO { get; set; }
        public Nullable<int> TIP_ATEND_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<bool> HAT_IMP_ETIQUETA { get; set; }
        public string HAT_ORIGEM_ATEND { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> PED_CRM_ID { get; set; }
        public Nullable<int> PPI_ID { get; set; }

        [Required(ErrorMessage = "Digite a descrição para histórico")]
        [MinLength(5, ErrorMessage = "A descrição deve possuir no mínimo 5 caracteres.")]
        public string HAT_DESCRICAO { get; set; }

        public Nullable<int> UEN_ID { get; set; }
        public Nullable<int> AGE_ID { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string HAT_TEXTO_ETIQUETA { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public string ASN_NUM_ASSINATURA_ANT { get; set; }
        public string ASN_NUM_ASSINATURA_ATU { get; set; }

        public virtual AcaoAtendimentoDTO ACAO_ATENDIMENTO { get; set; }
        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual TipoAtendimentoDTO TIPO_ATENDIMENTO { get; set; }
        public virtual AgendamentoDTO AGENDAMENTO { get; set; }
        public virtual UENDTO UEN { get; set; }
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
        
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PedidoCRMDTO PEDIDO_CRM { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PropostaDTO PROPOSTA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AssinaturaTransferenciaDTO ASSINATURA_TRANSFERENCIA { get; set; }

    }
}

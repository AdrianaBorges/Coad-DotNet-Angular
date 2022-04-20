using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(ASSINATURA))]
    public class AssinaturaDTO 
    {
        public AssinaturaDTO()
        {
            this.CHEQUE_DEVOLVIDO = new HashSet<ChequeDevolvidoDTO>();
            this.ASSINATURA_EMAIL = new HashSet<AssinaturaEmailDTO>();
            this.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();
            this.ASSINATURA_TRANSFERENCIA = new HashSet<AssinaturaTransferenciaDTO>();
            this.CARTEIRA_ASSINATURA = new HashSet<CarteiraAssinaturaDTO>();
            this.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();
            this.CONTRATOS = new HashSet<ContratoDTO>();
            this.HISTORICO_ATENDIMENTO = new HashSet<HistoricoAtendimentoDTO>();
            this.URA_LOG = new HashSet<UraLogDTO>();
            this.HIST_ATEND_EMAIL = new HashSet<HistAtendEmailDTO>();
            this.HIST_ATEND_URA = new HashSet<HistAtendUraDTO>();
            this.CARTEIRA_CLIENTE = new HashSet<CarteiraClienteDTO>();
            this.MOTIVO_CANCELAMENTO = new HashSet<MotivoCancelamentoDTO>();
            this.ITEM_PEDIDO = new HashSet<ItemPedidoDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>();
            this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();
            this.REGISTRO_FATURAMENTO = new HashSet<RegistroFaturamentoDTO>();

        }
    
        public string ASN_NUM_ASSINATURA { get; set; }
        public string ASN_ANO_COAD { get; set; }
        public Nullable<int> ASN_CORTESIA { get; set; }
        public string ASN_A_C { get; set; }
        public string ASN_E_MAIL { get; set; }
        public string ASN_REMESSA { get; set; }
        public string ASN_ANO_REMESSA { get; set; }
        public string ASN_NUM_TP_ENVIO { get; set; }
        public Nullable<int> PRO_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> TP_ASS_ID { get; set; }
        public int ASN_QTDE_CONS_CONTRATO { get; set; }
        public int ASN_QTDE_CONS_ADICIONAL { get; set; }
        public int ASN_QTDE_CONS_UTILIZADA { get; set; }
        public Nullable<int> UEN_ID { get; set; }
        public Nullable<int> CMP_ID { get; set; }
        public virtual UENDTO UEN { get; set; }
        public virtual string ASN_MATERIA_ADICIONAL { get; set; }
        public virtual Boolean ASN_ATIVA { get; set; }
        public virtual string ASN_TIPO { get; set; }
        public virtual string ASN_PERFIL { get; set; }
        public Nullable<DateTime> ASN_DATA_EXPIRA { get; set; }
        public string ASN_PERMISSAO_PORTAL { get; set; }
        public string ASN_ATV_REM { get; set; }
        public string ASN_NUM_ASS_TRANSFERIDA { get; set; }
        public Nullable<bool> ASN_TRANSFERIDA { get; set; }
        public bool ASN_PROTOCOLADA { get; set; }
        public string ASN_ENTREGADOR { get; set; }

        public virtual ICollection<ChequeDevolvidoDTO> CHEQUE_DEVOLVIDO { get; set; }
        public virtual ICollection<AssinaturaEmailDTO> ASSINATURA_EMAIL { get; set; }
        public virtual ProdutosDTO PRODUTOS { get; set; }
        public virtual ICollection<AssinaturaTelefoneDTO> ASSINATURA_TELEFONE { get; set; }
        public virtual ICollection<AssinaturaTransferenciaDTO> ASSINATURA_TRANSFERENCIA { get; set; }
        public virtual ICollection<CarteiraAssinaturaDTO> CARTEIRA_ASSINATURA { get; set; }
        public virtual ICollection<ClienteEnderecoDto> CLIENTES_ENDERECO { get; set; }
        public virtual ICollection<ContratoDTO> CONTRATOS { get; set; }
        public virtual ClienteDto CLIENTES { get; set; }
        public virtual ICollection<UraLogDTO> URA_LOG { get; set; }
        public virtual TipoPeriodoAssinaturaDTO TIPO_PERIODO_ASSINATURA { get; set; }
        public virtual ICollection<HistoricoAtendimentoDTO> HISTORICO_ATENDIMENTO { get; set; }
        public virtual ICollection<HistAtendEmailDTO> HIST_ATEND_EMAIL { get; set; }
        public virtual ICollection<HistAtendUraDTO> HIST_ATEND_URA { get; set; }
        public virtual ICollection<CarteiraClienteDTO> CARTEIRA_CLIENTE { get; set; }
        public virtual ICollection<MotivoCancelamentoDTO> MOTIVO_CANCELAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RegistroFaturamentoDTO> REGISTRO_FATURAMENTO { get; set; }


        /// <summary>
        /// Pedido que Cancelou essa assinatura
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO1 { get; set; }

        /// <summary>
        /// Proposta Item que Cancelou essa assinatura
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaItemDTO> PROPOSTA_ITEM1 { get; set; }

        /// <summary>
        /// Assinaturas que deram origem a essa assinatura por meio de transferencia
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<AssinaturaDTO> ASSINATURA1 { get; set; }

        /// <summary>
        /// Assinatura gerada a partir da transferencia da assinatura base
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual AssinaturaDTO ASSINATURA2 { get; set; }
    }
}

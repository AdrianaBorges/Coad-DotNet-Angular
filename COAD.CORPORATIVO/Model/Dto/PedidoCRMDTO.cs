using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PEDIDO_CRM))]
    public class PedidoCRMDTO : IPedido
    {
        public PedidoCRMDTO()
        {
            this.HISTORICO_ATENDIMENTO = new HashSet<HistoricoAtendimentoDTO>();
            this.PEDIDO_PAGAMENTO = new HashSet<PedidoPagamentoDTO>();
            this.ITEM_PEDIDO = new HashSet<ItemPedidoDTO>();
            this.PEDIDO_PARTICIPANTE = new HashSet<PedidoParticipanteDTO>();
        }
    
        public int? PED_CRM_ID { get; set; }
        public Nullable<System.DateTime> PED_CRM_DATA { get; set; }

        //[Required(ErrorMessage = "Digite as observações")]
        public string PED_CRM_DESCRICAO { get; set; }

        public Nullable<int> PROSP_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public Nullable<int> HAT_ID { get; set; }
        public Nullable<int> CMP_ID { get; set; }
        public Nullable<decimal> PED_CRM_VALOR { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> PST_ID { get; set; }
        public Nullable<bool> PED_CRM_VENDA_INFORMADA { get; set; }
        public Nullable<int> UEN_ID { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public Nullable<bool> PED_EMPRESA_DO_SIMPLES { get; set; }
        public Nullable<bool> PED_CEM_POR_CENTO_FATURADO { get; set; }
        public string PED_CRM_COD_LEGADO { get; set; }
        public Nullable<int> erro { get; set; }
        public string mensagem { get; set; }
        public Nullable<int> PRT_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<int> TPD_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CAR_ID { get; set; }
        public Nullable<int> REP_ID_EMITENTE { get; set; }
        public Nullable<int> TNE_ID { get; set; }
        public Nullable<System.DateTime> PED_CRM_DATA_FATURAMENTO { get; set; }
        public Nullable<int> ENV_ID { get; set; }
        /// <summary>
        /// Esse campo não existe no banco de dados
        /// </summary>
        public bool EhOnline { get; set; }

        [EmailAddress(ErrorMessage = "Digite um E-Mail válido")]
        [RegularExpression(@"(.*[^\.\s]$)", ErrorMessage = "Digite um E-Mail válido")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O email deve possuir no máximo 60 caracteres")]
        public string PED_CRM_EMAIL_CONTATO { get; set; }
        
        [EmailAddress(ErrorMessage = "Digite um E-Mail válido")]
        [RegularExpression(@"(.*[^\.\s]$)", ErrorMessage = "Digite um E-Mail válido")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O email deve possuir no máximo 60 caracteres")]
        public string PED_CRM_EMAIL_NOTA_FISCAL { get; set; }

        public string PED_OBSERVACOES_NOTA_FISCAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ClienteDto CLIENTES { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }

        /// <summary>
        /// Representante da venda
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<HistoricoAtendimentoDTO> HISTORICO_ATENDIMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PedidoStatusDTO PEDIDO_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoPagamentoDTO> PEDIDO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual UENDTO UEN { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoParticipanteDTO> PEDIDO_PARTICIPANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PropostaDTO PROPOSTA { get; set; }

        public virtual EmpresaDTO EMPRESA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AssinaturaDTO ASSINATURA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPedidoCRMDTO TIPO_PEDIDO_CRM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual CarteiraDTO CARTEIRA { get; set; }

        /// <summary>
        /// Representante emitente
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE1 { get; set; }
        public int? CliId { get => CLI_ID; }

        [ScriptIgnore]
        public ICollection<IPedidoItem> PedidoItens
        {
            get
            {

                if (ITEM_PEDIDO != null)
                {
                    return ITEM_PEDIDO.Cast<IPedidoItem>().ToList();
                }

                return null;
            }
        }
        public bool? EmpresaDoSimples { get => PED_EMPRESA_DO_SIMPLES; }
        public bool? FaturadoCemPorCento { get => PED_CEM_POR_CENTO_FATURADO; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoNegociacaoDTO TIPO_NEGOCIACAO { get; set; }
        
        public EmpresaModel EMPRESAS { get; set; }
        public virtual EnderecoVendaDTO ENDERECO_VENDA { get; set; }

    }
}

using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Validations.Enumerados;
using COAD.SEGURANCA.Model;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(PROPOSTA))]
    public class PropostaDTO : IPedido
    {
        public PropostaDTO()
        {
            this.PEDIDO_PARTICIPANTE = new HashSet<PedidoParticipanteDTO>();
            this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();
        }
    
        public int? PRT_ID { get; set; }

        [Required(ErrorMessage = "Selecione o tipo da Proposta.")]
        public Nullable<int> TPP_ID { get; set; }
        
        public Nullable<int> CMP_ID { get; set; }

        [Required(ErrorMessage = "O Prospect não foi encontrado ou selecionado.")]
        public Nullable<int> CLI_ID { get; set; }

        public string ASN_NUM_ASSINATURA { get; set; }
        public Nullable<int> PST_ID { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public Nullable<int> RG_ID { get; set; }

        [RequiredIf("EhGerenteEmitindo", true, ErrorMessage = "Informe o representante")]
        public Nullable<int> REP_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_CONFIRMACAO_PAGAMENTO { get; set; }
        public Nullable<decimal> PRT_VALOR_UNITARIO { get; set; }
        public Nullable<int> PRT_QTD { get; set; }
        public Nullable<int> PRT_QTD_PARCELAS { get; set; }
        public Nullable<decimal> PRT_VALOR_ENTRADA { get; set; }
        public Nullable<decimal> PRT_VALOR_PARCELA { get; set; }
        public Nullable<decimal> PRT_VALOR_TOTAL { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public string PRT_OBSERVACOES { get; set; }
        public string PRT_OBSERVACOES_NOTA_FISCAL { get; set; }
        public Nullable<System.DateTime> PRT_DATA_FATURAMENTO_AGENDADA { get; set; }
        public Nullable<int> REP_ID_EMITENTE { get; set; }

        [RequiredIf("EhGerenteEmitindo", false, ErrorMessage = "Informe a Carteira")]
        public string CAR_ID { get; set; }
        public bool Forcar { get; set; }

        [Required(ErrorMessage = "Informe a empresa.")]
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<int> UEN_ID { get; set; }
        public Nullable<bool> PRT_EMPRESA_DO_SIMPLES { get; set; }
        public Nullable<bool> PRT_POR_CENTO_FATURADO { get; set; }

        //[Required(ErrorMessage = "Informe o Tipo de Negociação")]
        public Nullable<int> TNE_ID { get; set; }
        public Nullable<int> ENV_ID { get; set; }

        // Esse atributo não está no banco de dados
        public bool EhGerenteEmitindo { get; set; }

        [Required(ErrorMessage = "Digite o E-Mail de Contato")]
        [EmailAddress(ErrorMessage = "Digite um E-Mail válido")]
        [RegularExpression(@"(.*[^\.\s]$)", ErrorMessage = "Digite um E-Mail válido")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O email deve possuir no máximo 60 caracteres")]
        [EmailPedidoPropostaValidator(Campo = EmailValidacaoEnum.EMAIL_CONTATO, ErrorMessage = "O E-mail de contato já existe")]
        public string PRT_EMAIL_CONTATO { get; set; }
        
        [EmailAddress(ErrorMessage = "Digite um E-Mail válido")]
        [RegularExpression(@"(.*[^\.\s]$)", ErrorMessage = "Digite um E-Mail válido")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O email deve possuir no máximo 60 caracteres")]
        [EmailPedidoPropostaValidator(Campo = EmailValidacaoEnum.EMAIL_NOTA_FISCAL, ErrorMessage = "O E-mail de nota fiscal já existe")]
        public string PRT_EMAIL_NOTA_FISCAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AssinaturaDTO ASSINATURA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ParcelasDTO PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ClienteDto CLIENTES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PedidoParticipanteDTO> PEDIDO_PARTICIPANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PedidoStatusDTO PEDIDO_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }

        /// <summary>
        /// Representante da venda
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPropostaDTO TIPO_PROPOSTA { get; set; }

        //[RequiredList(1, ErrorMessage = "Uma proposta deve possuir no mínimo 1 produto.")]
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaItemDTO> PROPOSTA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }

        /// <summary>
        /// Representante Emitente
        /// </summary>
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual CarteiraDTO CARTEIRA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual UENDTO UEN { get; set; }

        public int? CliId { get => CLI_ID; }

        [ScriptIgnore]
        public ICollection<IPedidoItem> PedidoItens
        {
            get
            {
                if (PROPOSTA_ITEM != null)
                {
                    return PROPOSTA_ITEM.Cast<IPedidoItem>().ToList();
                }

                return null;
            }
        }

        public bool? EmpresaDoSimples { get => PRT_EMPRESA_DO_SIMPLES; }
        public bool? FaturadoCemPorCento { get => PRT_POR_CENTO_FATURADO; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoNegociacaoDTO TIPO_NEGOCIACAO { get; set; }

        public EmpresaModel EMPRESAS { get; set; }
        public virtual EnderecoVendaDTO ENDERECO_VENDA { get; set; }

    }
}

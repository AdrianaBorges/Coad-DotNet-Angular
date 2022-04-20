using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Validations;
using COAD.SEGURANCA.Model;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PRODUTO_COMPOSICAO))]
    public class ProdutoComposicaoDTO
    {
        public ProdutoComposicaoDTO()
        {
            this.AREA_CONSULTORIA_CURSO = new HashSet<AreaConsultoriaCursoDTO>();
            this.ASSINATURA = new HashSet<AssinaturaDTO>();
            this.ITEM_PEDIDO = new HashSet<ItemPedidoDTO>();

            this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();

            this.PRODUTO_COMPOSICAO_INFO_MARKETING = new HashSet<ProdutoComposicaoInfoMarketingDTO>();
            this.PRODUTO_COMPOSICAO_ITEM = new HashSet<ProdutoComposicaoItemDTO>();

            this.PRODUTO_COMPOSICAO1 = new HashSet<ProdutoComposicaoDTO>();
            this.PRODUTO_COMPOSICAO_TIPO_PERIODO = new HashSet<ProdutoComposicaoTipoPeriodoDTO>();
            this.TABELA_PRECO = new HashSet<TabelaPrecoDTO>();
            
            this.CONTRATOS = new HashSet<ContratoDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>();
            this.IMPORTACAO_SUSPECT = new HashSet<ImportacaoSuspectDTO>();

            this.NOTA_FISCAL_ITEM = new HashSet<NotaFiscalItemDTO>();
            this.NOTA_FISCAL_CONFIG = new HashSet<NotaFiscalConfigDTO>();

            this.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO = new HashSet<CampanhaVendasProdutoComposicaoDTO>();
            this.CARRINHO_COMPRAS_ITEM = new HashSet<CarrinhoComprasItemDTO>();
        }
    
        public int? CMP_ID { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Digite a Descricao")]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "A descrição deve possuir no máximo 120 caracteres")]
        public string CMP_DESCRICAO { get; set; }

        public string CMP_NOME_ESTRANGEIRO { get; set; }

        public Nullable<int> AREA_ID { get; set; }

        [DisplayName("Tipo de Composição")]
        [RequiredIf("EhCurso", false, ErrorMessage = "Selecione o Tipo de Composição")]
        public Nullable<int> TIPO_PRO_ID { get; set; }

        [DisplayName("Preço")]
        [Required(ErrorMessage = "Digite o valor da venda")]       
        public Nullable<decimal> CMP_VLR_VENDA { get; set; }

        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public string USU_LOGIN_EXCLUSAO { get; set; }
        public bool CMP_PRO_INTERESSE { get; set; }
        public int? UEN_ID { get; set; }
        public Nullable<int> CMP_ID_TRANSF { get; set; }
        public int? TIPO_ENVIO_ID { get; set; }

        public Nullable<int> LIN_PRO_ID { get; set; }
        public Nullable<int> CMP_ID_ORIGEM { get; set; }
        public Nullable<int> TIPO_VENDA_ID { get; set; }
        public Nullable<int> TPV_ID { get; set; }
        public bool CMP_RENOVACAO { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<bool> CMP_EXIBIR_VITRINE { get; set; }
        public string CMP_DESCRICAO_VITRINE { get; set; }

        public bool EhCurso { get; set; }
        public string CMP_MUNDIPAGG_PLANO_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<AreaConsultoriaCursoDTO> AREA_CONSULTORIA_CURSO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<AssinaturaDTO> ASSINATURA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual LinhaProdutoDTO LINHA_PRODUTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }
   
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ProdutoComposicaoInfoMarketingDTO> PRODUTO_COMPOSICAO_INFO_MARKETING { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ProdutoComposicaoItemDTO> PRODUTO_COMPOSICAO_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual TipoProdutoComposicaoDTO TIPO_PRODUTO_COMPOSICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<TabelaPrecoDTO> TABELA_PRECO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ProdutoComposicaoTipoPeriodoDTO> PRODUTO_COMPOSICAO_TIPO_PERIODO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ProdutoComposicaoDTO> PRODUTO_COMPOSICAO1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO2 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoVendaDTO TIPO_VENDA { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ContratoDTO> CONTRATOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoSuspectDTO> IMPORTACAO_SUSPECT { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalItemDTO> NOTA_FISCAL_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalConfigDTO> NOTA_FISCAL_CONFIG { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CampanhaVendasProdutoComposicaoDTO> CAMPANHA_VENDAS_PRODUTO_COMPOSICAO { get; set; }

        public EmpresaModel EMPRESAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CarrinhoComprasItemDTO> CARRINHO_COMPRAS_ITEM { get; set; }
    }
}

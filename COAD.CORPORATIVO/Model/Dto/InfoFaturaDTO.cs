using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(INFO_FATURA))]
    public class InfoFaturaDTO
    {

        public InfoFaturaDTO()
        {
            this.IMPOSTO_INFO_FATURA = new HashSet<ImpostoInfoFaturaDTO>();
            this.ITEM_PEDIDO = new HashSet<ItemPedidoDTO>();
            this.PROPOSTA_ITEM = new HashSet<PropostaItemDTO>();
            this.INFO_FATURA_ITEM = new HashSet<InfoFaturaItemDTO>();
            this.CARRINHO_COMPRAS_ITEM = new HashSet<CarrinhoComprasDTO>();
        }
    
        public int? IFF_ID { get; set; }
        public Nullable<decimal> IFF_VALOR_BRUTO { get; set; }
        public Nullable<decimal> IFF_TOTAL_LIQUIDO { get; set; }
        public Nullable<decimal> IFF_TOTAL_DESCONTADO { get; set; }
        public Nullable<decimal> IFF_PERCENTUAL_TOTAL_DESCONTADO { get; set; }
        public Nullable<decimal> IFF_VALOR_SERVICE { get; set; }
        public Nullable<decimal> IFF_VALOR_MATERIAL { get; set; }
        public Nullable<decimal> IFF_VALOR_UNITARIO_PRODUTO { get; set; }
        public Nullable<decimal> IFF_VALOR_UNITARIO_SERVICO { get; set; }
        public Nullable<decimal> IFF_VALOR_UNITARIO { get; set; }
        public Nullable<int> IFF_QTD_SERVICO { get; set; }
        public Nullable<int> IFF_QTD_PRODUTO { get; set; }
        public Nullable<int> PPI_ID { get; set; }
        public Nullable<int> NFC_ID { get; set; }

        public virtual Nullable<decimal> ValorLiquidoServico {
            get {
                var valorServico = (IFF_VALOR_SERVICE != null) ? IFF_VALOR_SERVICE : 0;
                var totalDesconto = (IFF_TOTAL_DESCONTADO != null) ? IFF_TOTAL_DESCONTADO : 0;

                return valorServico - totalDesconto;
            }
        }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ImpostoInfoFaturaDTO> IMPOSTO_INFO_FATURA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PropostaItemDTO> PROPOSTA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PropostaItemDTO> PROPOSTA_ITEM1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ItemPedidoDTO> ITEM_PEDIDO1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalConfigDTO NOTA_FISCAL_CONFIG { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<InfoFaturaItemDTO> INFO_FATURA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CarrinhoComprasDTO> CARRINHO_COMPRAS_ITEM { get; set; }

    }
}

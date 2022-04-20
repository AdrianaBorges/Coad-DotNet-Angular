

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
using GenericCrud.Models.Interfaces;

namespace COAD.CORPORATIVO.Model.Dto
{    
	[Mapping(typeof(NOTA_FISCAL_CONFIG))]
	public class NotaFiscalConfigDTO
	{
		public NotaFiscalConfigDTO(){

			this.CONFIG_IMPOSTO = new HashSet<ConfigImpostoDTO>();
            this.INFO_FATURA_ITEM = new HashSet<InfoFaturaItemDTO>();
            this.NOTA_FISCAL_LOTE_ITEM = new HashSet<NotaFiscalLoteItemDTO>();
        }

		// Normal Properties
		public Int32? NFC_ID { get; set; }

        [Required(ErrorMessage = "Informe a Descrição da Nota")]
		public String NFC_DESCRICAO_PRODUTO { get; set; }

        [Required(ErrorMessage = "Informe a porcentagem de valor")]
        [Range(1, 100, ErrorMessage = "A porcentagem de valor deve ter ser mínimo 1% e no máximo 100%")]
		public Nullable<Int32> NFC_PORCENTAGEM_VALOR { get; set; }
		public String NFC_OBSERVACAO_NOTA { get; set; }
		public String NFC_OBSERVACAO_FISCO { get; set; }

        [Required(ErrorMessage = "Informe o produto da configuração")]
        public Nullable<Int32> CMP_ID { get; set; }

        [Required(ErrorMessage = "Informe o tipo de Configuração")]
        public Nullable<int> NCT_ID { get; set; }
        public string NFC_COD_LISTA_SERVICO { get; set; }
        public Nullable<bool> NFC_APLICAR_100_POR_CENTO_FAT { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public string NFC_CODIGO_TRIBUTACAO_MUNICIPIO { get; set; }

        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }
		
		// Collections Properties
		
		[IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ICollection<ConfigImpostoDTO> CONFIG_IMPOSTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalConfigTipoDTO NOTA_FISCAL_CONFIG_TIPO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<InfoFaturaItemDTO> INFO_FATURA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalLoteItemDTO> NOTA_FISCAL_LOTE_ITEM { get; set; }
        
    }
}

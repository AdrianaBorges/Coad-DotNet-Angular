using Coad.GenericCrud.Validations;
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
    [Mapping(Source = typeof(CONFIG_IMPOSTO))]
    public class ConfigImpostoDTO
    {
        public ConfigImpostoDTO()
        {
            this.CONFIG_IMPOSTO_IMPOSTO = new HashSet<ConfigImpostoImpostoDTO>();
            this.CONFIG_IMPOSTO_REGIAO = new HashSet<ConfigImpostoRegiaoDTO>();
        }
    
        public int? CFI_ID { get; set; }
        public Nullable<decimal> CFI_VLR_MINIMO { get; set; }
        public Nullable<decimal> CFI_VLR_MAXIMO { get; set; }
        public Nullable<bool> CFI_QUALQUER_VALOR { get; set; }

        [Required(ErrorMessage = "Informe o tipo de cliente para a configuração")]
        public Nullable<int> TIPO_CLI_ID { get; set; }
        public Nullable<bool> CFI_EMPRESA_DO_SIMPLES { get; set; }
        
        public Nullable<decimal> CFI_VLR_DESCONTO_MIM { get; set; }

        [Required(ErrorMessage = "Informe a descrição da regra")]
        public string CFI_DESC_REGRA { get; set; }
        public Nullable<int> NFC_ID { get; set; }
        public Nullable<bool> CFI_CLIENTE_RETEM { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public string CFI_CODIGO_TRIBUTACAO_MUNICIPIO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ConfigImpostoImpostoDTO> CONFIG_IMPOSTO_IMPOSTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ConfigImpostoRegiaoDTO> CONFIG_IMPOSTO_REGIAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoClienteDTO TIPO_CLIENTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalConfigDTO NOTA_FISCAL_CONFIG { get; set; }
    }
}

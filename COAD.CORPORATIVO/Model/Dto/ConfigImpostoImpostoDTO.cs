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
    [Mapping(Source = typeof(CONFIG_IMPOSTO_IMPOSTO))]
    public class ConfigImpostoImpostoDTO
    {
        public int CFI_ID { get; set; }
        public int IMP_ID { get; set; }
        public Nullable<System.DateTime> DATETIME { get; set; }

        [Required(ErrorMessage = "Informe a líquota para a configuração")]
        [Range(0.01, 100, ErrorMessage = "O valor mínimo da alíquota deve ser 0,01% e o valor máximo 100%")]
        public Nullable<decimal> CII_ALIQUOTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ConfigImpostoDTO CONFIG_IMPOSTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ImpostoDTO IMPOSTO { get; set; }
        
    }
}

using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(CONFIG_ALOCACAO_CONTA))]
    public class ConfigAlocacaoContaDTO
    {
        public int CFA_ID { get; set; }
        public int CTA_ID { get; set; }
        public int TCC_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ConfigAlocacaoDTO CONFIG_ALOCACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ContaRefDTO CONTA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoConfigContaDTO TIPO_CONFIG_CONTA { get; set; }
    }
}

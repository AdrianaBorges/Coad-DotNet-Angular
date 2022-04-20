using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(CONFIG_ALOCACAO))]
    public class ConfigAlocacaoDTO
    {
        public ConfigAlocacaoDTO()
        {
            this.CONFIG_ALOCACAO_CONTA = new HashSet<ConfigAlocacaoContaDTO>();
        }

        public Nullable<int> CFA_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<int> RG_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ConfigAlocacaoContaDTO> CONFIG_ALOCACAO_CONTA { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }
    }
}

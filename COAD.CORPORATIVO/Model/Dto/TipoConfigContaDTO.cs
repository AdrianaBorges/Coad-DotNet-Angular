using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(TIPO_CONFIG_CONTA))]
    public class TipoConfigContaDTO
    {
        public TipoConfigContaDTO()
        {
            this.CONFIG_ALOCACAO_CONTA = new HashSet<ConfigAlocacaoContaDTO>();
        }
    
        public int TCC_ID { get; set; }
        public string TCC_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ConfigAlocacaoContaDTO> CONFIG_ALOCACAO_CONTA { get; set; }
    }
}

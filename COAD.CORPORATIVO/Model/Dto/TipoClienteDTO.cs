using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(TIPO_CLIENTE))]
    public partial class TipoClienteDTO
    {

        public TipoClienteDTO()
        {
            this.CLIENTES = new HashSet<ClienteDto>();
            this.CONFIG_IMPOSTO = new HashSet<ConfigImpostoDTO>();
        }

        public int TIPO_CLI_ID { get; set; }
        public string TIPO_CLI_DESCRICAO { get; set; }
        public Nullable<int> TIPO_CLI_ATIVO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ClienteDto> CLIENTES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ConfigImpostoDTO> CONFIG_IMPOSTO { get; set; }
    }
}

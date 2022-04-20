using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoComissaoDTO
    {
        public TipoComissaoDTO()
        {
            this.REPRESENTANTE_META = new HashSet<RepresentanteMetaDTO>();
        }

        public int TCO_ID { get; set; }
        public string TCO_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RepresentanteMetaDTO> REPRESENTANTE_META { get; set; }
    }
}

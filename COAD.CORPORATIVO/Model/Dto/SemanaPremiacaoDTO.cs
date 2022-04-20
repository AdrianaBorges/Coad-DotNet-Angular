using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class SemanaPremiacaoDTO
    {
        public SemanaPremiacaoDTO()
        {
            this.SEMANA_PREMIACAO_REPR = new HashSet<SemanaPremiacaoReprDTO>();
        }

        public int SPR_SEMANA { get; set; }
        public System.DateTime SPR_DATA_INI { get; set; }
        public System.DateTime SPR_DATA_FIM { get; set; }
        public string USU_LOGIN { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<SemanaPremiacaoReprDTO> SEMANA_PREMIACAO_REPR { get; set; }
    }
}

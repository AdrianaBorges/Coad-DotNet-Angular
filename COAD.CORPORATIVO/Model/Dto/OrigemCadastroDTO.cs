using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(ORIGEM_CADASTRO))]
    public class OrigemCadastroDTO
    {
        public OrigemCadastroDTO()
        {
            this.INFO_MARKETING = new HashSet<InfoMarketingDTO>();
            this.IMPORTACAO_SUSPECT = new HashSet<ImportacaoSuspectDTO>();
        }
    
        public int O_CAD_ID { get; set; }
        public string O_CAD_DESCRICAO { get; set; }
    
        public virtual ICollection<InfoMarketingDTO> INFO_MARKETING { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoSuspectDTO> IMPORTACAO_SUSPECT { get; set; }

    }
}
